using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity.Endorsements;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Exceptions;
using RivTech.CABoxTruck.WebService.DTO.Notification;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Validators;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement
{
    public interface IEndorsementProcessor
    {
        EndorsementPoint EndorsementPointHead { get; }
        Stack<EndorsementPoint> SkippedEndorsementPoints { get; }

        Task Process(Guid riskId, DateTime effectiveDate, IEndorsementChangeCollection newEndorsementChanges, RiskEndorsementPoints riskEndorsementPoints);
        Task Process(Guid riskId, DateTime effectiveDate, IEndorsementChangeCollection newEndorsementChanges, RiskEndorsementPoints riskEndorsementPoints, NotificationFilterDTO notificationFilterDTO);
    }

    public class EndorsementProcessor : IEndorsementProcessor
    {
        private readonly ITaxService _taxService;
        private readonly IEndorsementDataRepository _endorsementDataRepository;
        private readonly IEndorsementDataCloner _endorsementDataCloner;
        private readonly IEndorsementChangeItemModifierResolver _endorsementChangeItemModifierResolver;
        private readonly IEndorsementDataPremiumRater _endorsementDataPremiumRater;
        private readonly IEndorsementDataPremiumProrator _endorsementDataPremiumProrator;
        private readonly IRiskPremiumTransactionService _riskPremiumTransactionService;
        private readonly IEndorsementDataCleaner _endorsementDataCleaner;
        private readonly IRiskRepository _riskRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IEndorsementProcessStatusLogService _statusLogService;
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Stores the skipped endorsement points which will be re-applied after merging the new changes to EndorsementPointHead.  
        /// </summary>
        public Stack<EndorsementPoint> SkippedEndorsementPoints { get; } = new Stack<EndorsementPoint>();
        /// <summary>
        /// The head of RiskEndorsementPoints in which the new endorsement changes will be inserted after it.
        /// </summary>
        public EndorsementPoint EndorsementPointHead { get; private set; }

        public EndorsementProcessor(
            ITaxService taxService,
            IEndorsementDataRepository endorsementDataRepository,
            IEndorsementDataCloner endorsementDataCloner,
            IEndorsementChangeItemModifierResolver endorsementChangeItemModifierResolver,
            IEndorsementDataPremiumRater endorsementDataPremiumRater,
            IEndorsementDataPremiumProrator endorsementDataPremiumProrator,
            IRiskPremiumTransactionService riskPremiumTransactionService,
            IEndorsementDataCleaner endorsementDataCleaner,
            IRiskRepository riskRepository,
            IRiskDetailRepository riskDetailRepository,
            IEndorsementProcessStatusLogService statusLogService,
            INotificationService notificationService
        )
        {
            _taxService = taxService;
            _endorsementDataRepository = endorsementDataRepository;
            _endorsementDataCloner = endorsementDataCloner;
            _endorsementChangeItemModifierResolver = endorsementChangeItemModifierResolver;
            _endorsementDataPremiumRater = endorsementDataPremiumRater;
            _endorsementDataPremiumProrator = endorsementDataPremiumProrator;
            _riskPremiumTransactionService = riskPremiumTransactionService;
            _endorsementDataCleaner = endorsementDataCleaner;
            _riskRepository = riskRepository;
            _riskDetailRepository = riskDetailRepository;
            _statusLogService = statusLogService;
            _notificationService = notificationService;
        }

        public async Task Process(
            Guid riskId,
            DateTime effectiveDate,
            IEndorsementChangeCollection newEndorsementChanges,
            RiskEndorsementPoints riskEndorsementPoints
        )
        {
            try
            {
                await _statusLogService.LogAsync(riskId, "Cleaning up outdated endorsement data.");
                await _endorsementDataCleaner.CleanUpAsync(riskId);

                IEndorsementData lastProcessedEndorsementData;
                EndorsementPoint previousEndorsementPoint = riskEndorsementPoints.Peek();

                // Create EndorsementPoint for new endorsement changes.
                int newEndorsementNumber = riskEndorsementPoints.GetNextEndorsementNumber();
                var newEndorsementPoint = new EndorsementPoint(effectiveDate, newEndorsementChanges, newEndorsementNumber);
                newEndorsementPoint.EndorsementNumber = riskEndorsementPoints.GetNextEndorsementNumber();

                // Get the head of endorsement point where the new changes will be inserted.
                await _statusLogService.LogAsync(riskId, "Identifying the endorsement where new changes will be applied.");
                EndorsementPointHead = riskEndorsementPoints.FindPointBeforeDate(effectiveDate);
                await _statusLogService.LogAsync(riskId, $"New changes will be applied after Endorsement {EndorsementPointHead.EndorsementNumber}.");

                while (!riskEndorsementPoints.Peek().Equals(EndorsementPointHead))
                {
                    // If the last point of endorsementPoints does not match EndorsementPointHead,
                    // push into SkippedEndorsementPoints stack.
                    SkippedEndorsementPoints.Push(riskEndorsementPoints.Pop());
                }
                bool isOutOfSequence = SkippedEndorsementPoints.Any();
                var currentEndorsementPointToMerge = newEndorsementPoint;
                if (isOutOfSequence)
                {
                    await _statusLogService.LogAsync(riskId, "Endorsement identified as Out-of-Sequence.");
                    var validator = new OutOfSequenceValidator(_notificationService);

                    await _statusLogService.LogAsync(riskId, "Validating changes for Out-of-Sequence.");
                    validator.Validate(newEndorsementChanges);
                }

                // Initialize current values before iterating each endorsements.
                IEndorsementData currentEndorsementData = await _endorsementDataRepository.LoadAsync(EndorsementPointHead);
                do
                {
                    var endorsementNum = currentEndorsementPointToMerge.EndorsementNumber;
                    await _endorsementDataPremiumRater.PreloadAsync(currentEndorsementData);

                    if (isOutOfSequence)
                    {

                        // If endorsement is out of sequence,
                        // Clone current endorsement data to merge changes.
                        await _statusLogService.LogAsync(riskId, $"Applying changes of Endorsement {endorsementNum}.");
                        var clonedEndorsementData = await currentEndorsementData.CloneAsync(_endorsementDataCloner);
                        clonedEndorsementData.EndorsementNumber = currentEndorsementPointToMerge.EndorsementNumber;
                        clonedEndorsementData.EffectiveDate = currentEndorsementPointToMerge.EffectiveDate;
                        clonedEndorsementData.SourceRiskDetailId = currentEndorsementData.RiskDetailId;
                        clonedEndorsementData.EndorsementChanges = currentEndorsementPointToMerge.EndorsementChanges;
                        await _endorsementDataRepository.UpdateAsync(clonedEndorsementData);

                        var driverErrorList = new List<OutOfSequenceChangeConflictException>();
                        var vehicleErrorList = new List<OutOfSequenceChangeConflictException>();

                        // Apply endorsement changes into cloned endorsement data.
                        foreach (IEndorsementChangeItem changeItem in currentEndorsementPointToMerge.EndorsementChanges)
                        {
                            var changeModifier = _endorsementChangeItemModifierResolver.Resolve(changeItem);

                            // Try catch to handle OutOfSequenceChangeConflictException
                            try
                            {
                                await clonedEndorsementData.ApplyChangeAsync(changeItem, changeModifier);
                            }
                            catch (OutOfSequenceChangeConflictException ex)
                            {
                                if (changeItem is DriverRemoveChangeItem || changeItem is DriverReinstateChangeItem)
                                    driverErrorList.Add(ex);
                                if (changeItem is VehicleRemoveChangeItem || changeItem is VehicleReinstateChangeItem || changeItem is VehicleUpdateChangeItem)
                                    vehicleErrorList.Add(ex);
                            }
                        }

                        if (driverErrorList.Count() > 0 || vehicleErrorList.Count > 0)
                        {
                            throw HandleEndorsementErrorsAsync(driverErrorList,
                                    vehicleErrorList,
                                    currentEndorsementPointToMerge.EndorsementNumber,
                                    currentEndorsementPointToMerge.EffectiveDate);
                        }

                        currentEndorsementData = clonedEndorsementData;
                    }
                    else
                    {
                        // If not out of sequence,
                        // use the cloned riskdetail from previously issued endorsement.
                        await _statusLogService.LogAsync(riskId, $"Loading data of Endorsement {endorsementNum}.");
                        currentEndorsementData = await _endorsementDataRepository.LoadForEndorsementFromRiskAsync(riskEndorsementPoints.Peek());
                        await _statusLogService.LogAsync(riskId, $"Endorsement {endorsementNum} data loaded.");
                    }

                    await _endorsementDataRepository.UpdateVehiclesEffectiveDates(currentEndorsementData, effectiveDate, isOutOfSequence);
                    await _endorsementDataRepository.UpdateDriversEffectiveDates(currentEndorsementData, effectiveDate, isOutOfSequence);

                    // Calculate premium with rater.
                    await currentEndorsementData.RatePremiumAsync(_endorsementDataPremiumRater, newEndorsementNumber, forIssuance: !SkippedEndorsementPoints.Any());

                    // Prorate premiums after rating.
                    await _statusLogService.LogAsync(riskId, $"Prorating premiums of Endorsement {currentEndorsementPointToMerge.EndorsementNumber}.");
                    await currentEndorsementData.ProratePremiumsAsync(_endorsementDataPremiumProrator);
                    await _statusLogService.LogAsync(riskId, $"Done prorating premiums of Endorsement {currentEndorsementPointToMerge.EndorsementNumber}.");

                    lastProcessedEndorsementData = currentEndorsementData;
                    currentEndorsementPointToMerge = SkippedEndorsementPoints.Any() ? SkippedEndorsementPoints.Pop() : null;

                } while (currentEndorsementPointToMerge != null);

                // Calculate transaction changes.
                await _statusLogService.LogAsync(riskId, $"Calculating latest premium transactions.");
                IEndorsementData previousEndorsementData = await _endorsementDataRepository.LoadAsync(previousEndorsementPoint);
                await CalculateTransactionsAsync(riskId, lastProcessedEndorsementData.RiskDetailId, previousEndorsementData.RiskDetailId);

                // Update RiskCoverage Fees
                await _riskPremiumTransactionService.CalculateServiceFee(lastProcessedEndorsementData.RiskDetailId,
                    (EndorsementData)lastProcessedEndorsementData, 
                    (EndorsementData)previousEndorsementData);

                await _statusLogService.LogAsync(riskId, $"Calculating tax.");
                await _taxService.CalculateRiskTaxAsync(lastProcessedEndorsementData.RiskDetailId, isEndorsement: true);
            }
            catch (Exception)
            {
                await _endorsementDataCleaner.CleanUpAsync(riskId);
                throw; // re-throw the original exception.
            }
        }

        public async Task Process(
            Guid riskId,
            DateTime effectiveDate,
            IEndorsementChangeCollection newEndorsementChanges,
            RiskEndorsementPoints riskEndorsementPoints,
            NotificationFilterDTO notificationFilterDTO
        )
        {
            try
            {
                //await _statusLogService.LogAsync(riskId, "Cleaning up outdated endorsement data.");
                await _endorsementDataCleaner.CleanUpAsync(riskId);

                notificationFilterDTO.CompletionPercentage = 30;

                await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

                IEndorsementData lastProcessedEndorsementData;
                EndorsementPoint previousEndorsementPoint = riskEndorsementPoints.Peek();

                // Create EndorsementPoint for new endorsement changes.
                int newEndorsementNumber = riskEndorsementPoints.GetNextEndorsementNumber();
                var newEndorsementPoint = new EndorsementPoint(effectiveDate, newEndorsementChanges, newEndorsementNumber);
                newEndorsementPoint.EndorsementNumber = riskEndorsementPoints.GetNextEndorsementNumber();

                // Get the head of endorsement point where the new changes will be inserted.
                //await _statusLogService.LogAsync(riskId, "Identifying the endorsement where new changes will be applied.");
                EndorsementPointHead = riskEndorsementPoints.FindPointBeforeDate(effectiveDate);
                //await _statusLogService.LogAsync(riskId, $"New changes will be applied after Endorsement {EndorsementPointHead.EndorsementNumber}.");

                while (!riskEndorsementPoints.Peek().Equals(EndorsementPointHead))
                {
                    // If the last point of endorsementPoints does not match EndorsementPointHead,
                    // push into SkippedEndorsementPoints stack.
                    SkippedEndorsementPoints.Push(riskEndorsementPoints.Pop());
                }
                bool isOutOfSequence = SkippedEndorsementPoints.Any();
                var currentEndorsementPointToMerge = newEndorsementPoint;
                if (isOutOfSequence)
                {
                    //await _statusLogService.LogAsync(riskId, "Endorsement identified as Out-of-Sequence.");
                    var validator = new OutOfSequenceValidator(_notificationService);

                    //await _statusLogService.LogAsync(riskId, "Validating changes for Out-of-Sequence.");
                    await validator.Validate(newEndorsementChanges, notificationFilterDTO);
                }

                notificationFilterDTO.CompletionPercentage = 40;

                await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

                // Initialize current values before iterating each endorsements.
                IEndorsementData currentEndorsementData = await _endorsementDataRepository.LoadAsync(EndorsementPointHead);
                do
                {
                    var endorsementNum = currentEndorsementPointToMerge.EndorsementNumber;
                    await _endorsementDataPremiumRater.PreloadAsync(currentEndorsementData);

                    if (isOutOfSequence)
                    {

                        // If endorsement is out of sequence,
                        // Clone current endorsement data to merge changes.
                        //await _statusLogService.LogAsync(riskId, $"Applying changes of Endorsement {endorsementNum}.");
                        var clonedEndorsementData = await currentEndorsementData.CloneAsync(_endorsementDataCloner);
                        clonedEndorsementData.EndorsementNumber = currentEndorsementPointToMerge.EndorsementNumber;
                        clonedEndorsementData.EffectiveDate = currentEndorsementPointToMerge.EffectiveDate;
                        clonedEndorsementData.SourceRiskDetailId = currentEndorsementData.RiskDetailId;
                        clonedEndorsementData.EndorsementChanges = currentEndorsementPointToMerge.EndorsementChanges;
                        await _endorsementDataRepository.UpdateAsync(clonedEndorsementData);

                        var driverErrorList = new List<OutOfSequenceChangeConflictException>();
                        var vehicleErrorList = new List<OutOfSequenceChangeConflictException>();

                        // Apply endorsement changes into cloned endorsement data.
                        foreach (IEndorsementChangeItem changeItem in currentEndorsementPointToMerge.EndorsementChanges)
                        {
                            var changeModifier = _endorsementChangeItemModifierResolver.Resolve(changeItem);

                            // Try catch to handle OutOfSequenceChangeConflictException
                            try
                            {
                                await clonedEndorsementData.ApplyChangeAsync(changeItem, changeModifier);
                            }
                            catch (OutOfSequenceChangeConflictException ex)
                            {
                                if (changeItem is DriverRemoveChangeItem || changeItem is DriverReinstateChangeItem)
                                    driverErrorList.Add(ex);
                                if (changeItem is VehicleRemoveChangeItem || changeItem is VehicleReinstateChangeItem || changeItem is VehicleUpdateChangeItem)
                                    vehicleErrorList.Add(ex);
                            }
                        }

                        if (driverErrorList.Count() > 0 || vehicleErrorList.Count > 0)
                        {
                            throw HandleEndorsementErrorsAsync(driverErrorList,
                                    vehicleErrorList,
                                    currentEndorsementPointToMerge.EndorsementNumber,
                                    currentEndorsementPointToMerge.EffectiveDate);
                        }

                        currentEndorsementData = clonedEndorsementData;
                    }
                    else
                    {
                        // If not out of sequence,
                        // use the cloned riskdetail from previously issued endorsement.
                        //await _statusLogService.LogAsync(riskId, $"Loading data of Endorsement {endorsementNum}.");
                        currentEndorsementData = await _endorsementDataRepository.LoadForEndorsementFromRiskAsync(riskEndorsementPoints.Peek());
                        //await _statusLogService.LogAsync(riskId, $"Endorsement {endorsementNum} data loaded.");
                    }

                    await _endorsementDataRepository.UpdateVehiclesEffectiveDates(currentEndorsementData, effectiveDate, isOutOfSequence);
                    await _endorsementDataRepository.UpdateDriversEffectiveDates(currentEndorsementData, effectiveDate, isOutOfSequence);

                    // Calculate premium with rater.
                    await currentEndorsementData.RatePremiumAsync(_endorsementDataPremiumRater, newEndorsementNumber, forIssuance: !SkippedEndorsementPoints.Any());

                    // Prorate premiums after rating.
                    //await _statusLogService.LogAsync(riskId, $"Prorating premiums of Endorsement {currentEndorsementPointToMerge.EndorsementNumber}.");
                    await currentEndorsementData.ProratePremiumsAsync(_endorsementDataPremiumProrator);
                    //await _statusLogService.LogAsync(riskId, $"Done prorating premiums of Endorsement {currentEndorsementPointToMerge.EndorsementNumber}.");

                    lastProcessedEndorsementData = currentEndorsementData;
                    currentEndorsementPointToMerge = SkippedEndorsementPoints.Any() ? SkippedEndorsementPoints.Pop() : null;

                } while (currentEndorsementPointToMerge != null);


                notificationFilterDTO.CompletionPercentage = 50;

                await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

                // Calculate transaction changes.
                //await _statusLogService.LogAsync(riskId, $"Calculating latest premium transactions.");
                IEndorsementData previousEndorsementData = await _endorsementDataRepository.LoadAsync(previousEndorsementPoint);
                await CalculateTransactionsAsync(riskId, lastProcessedEndorsementData.RiskDetailId, previousEndorsementData.RiskDetailId);


                notificationFilterDTO.CompletionPercentage = 60;

                await _notificationService.UpdateNotificationStatus(notificationFilterDTO);
                // Update RiskCoverage Fees
                await _riskPremiumTransactionService.CalculateServiceFee(lastProcessedEndorsementData.RiskDetailId,
                    (EndorsementData)lastProcessedEndorsementData,
                    (EndorsementData)previousEndorsementData);


                notificationFilterDTO.CompletionPercentage = 80;

                await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

                //await _statusLogService.LogAsync(riskId, $"Calculating tax.");
                await _taxService.CalculateRiskTaxAsync(lastProcessedEndorsementData.RiskDetailId, isEndorsement: true);

                notificationFilterDTO.CompletionPercentage = 90;

                await _notificationService.UpdateNotificationStatus(notificationFilterDTO);
            }
            catch (Exception)
            {
                await _endorsementDataCleaner.CleanUpAsync(riskId);
                throw; // re-throw the original exception.
            }
        }

        /// <summary>
        /// When the ris.ForEndorsementRiskDetailId is from reinstatement, use its RiskDetailId as previousRiskDetailId 
        /// otherwise use the provided previousRiskDetailId.
        /// </summary>
        private async Task CalculateTransactionsAsync(Guid riskId, Guid lastProcessedRiskDetailId, Guid previousRiskDetailId)
        {
            var risk = await _riskRepository.FindAsync(riskId);
            var forEndorsementRiskDetail = await _riskDetailRepository.FindAsync(risk.ForEndorsementRiskDetailId.Value);
            var previousTransactionRiskDetailId = previousRiskDetailId;
            var fromReinstatement = false;
            if (forEndorsementRiskDetail.FromReinstatement)
            {
                previousTransactionRiskDetailId = risk.LastReinstatedRiskDetailId.Value;
                fromReinstatement = true;
            }

            await _riskPremiumTransactionService.CalculateAsync(lastProcessedRiskDetailId, previousTransactionRiskDetailId, fromReinstatement: fromReinstatement);
        }

        private OutOfSequenceChangeConflictException HandleEndorsementErrorsAsync(
            List<OutOfSequenceChangeConflictException> driverList,
            List<OutOfSequenceChangeConflictException> vehicleList,
            int endorsementNumber,
            DateTime effectiveDate)
        {
            var returnErrorList = new List<string>();
            var driverRemoveDoesntExist = new List<string>();
            var driverRemoveDeleted = new List<string>();
            var driverReinstateDoesntExist = new List<string>();
            var driverReinstateActive = new List<string>();
            foreach (var error in driverList)
            {
                if (error.oosType == "DriverRemoveDoesntExist")
                    driverRemoveDoesntExist.Add(error.Message);
                if (error.oosType == "DriverRemoveDeleted")
                    driverRemoveDeleted.Add(error.Message);
                if (error.oosType == "DriverReinstateDoesntExist")
                    driverReinstateDoesntExist.Add(error.Message);
                if (error.oosType == "DriverReinstateActive")
                    driverReinstateActive.Add(error.Message);
            }

            if (driverRemoveDoesntExist.Any())
                returnErrorList.Add(@$"Driver '{string.Join(", ", driverRemoveDoesntExist)}' does not yet exist
                    on Endorsement {endorsementNumber} effective {effectiveDate:MM/dd/yyyy}.");

            if (driverRemoveDeleted.Any())
                returnErrorList.Add(@$"Failed applying Endorsement {endorsementNumber}
                    because driver '{string.Join(", ", driverRemoveDeleted)}' has already been deleted on.");

            if (driverReinstateDoesntExist.Any())
                returnErrorList.Add(@$"Failed applying Endorsement {endorsementNumber} effective {effectiveDate:MM/dd/yyyy}
                    because driver '{string.Join(", ", driverReinstateDoesntExist)}' doesn't exist.");

            if (driverReinstateActive.Count() > 0)
                returnErrorList.Add(@$"Failed applying Endorsement {endorsementNumber} effective {effectiveDate:MM/dd/yyyy}
                    because driver '{string.Join(", ", driverReinstateActive)}' cannot be reinstated while still active.");

            var vehicleRemoveDoesntExist = new List<string>();
            var vehicleReinstateDoesntExist = new List<string>();
            var vehicleReinstateActive = new List<string>();
            var vehicleUpdateDoenstExist = new List<string>();
            foreach (var error in vehicleList)
            {
                if (error.oosType == "VehicleRemoveDoesntExist")
                    vehicleRemoveDoesntExist.Add(error.Message);
                if (error.oosType == "VehicleReinstateDoesntExist")
                    vehicleReinstateDoesntExist.Add(error.Message);
                if (error.oosType == "VehicleReinstateActive")
                    vehicleReinstateActive.Add(error.Message);
                if (error.oosType == "VehicleUpdateDoenstExist")
                    vehicleUpdateDoenstExist.Add(error.Message);
            }

            if (vehicleRemoveDoesntExist.Any())
                returnErrorList.Add(@$"VIN {string.Join(", ", vehicleRemoveDoesntExist)} 
                    does not yet exist on Endorsement {endorsementNumber} effective {effectiveDate:MM/dd/yyyy}.");

            if (vehicleReinstateDoesntExist.Any())
                returnErrorList.Add(@$"Failed applying Endorsement {endorsementNumber} effective {effectiveDate:MM/dd/yyyy}
                    because VIN '{string.Join(", ", vehicleReinstateDoesntExist)}' doesn't exist.");

            if (vehicleReinstateActive.Any())
                returnErrorList.Add(@$"Failed applying Endorsement {endorsementNumber} effective {effectiveDate:MM/dd/yyyy}
                    because VIN '{string.Join(", ", vehicleReinstateActive)}' cannot be reinstated while still active.");

            if (vehicleUpdateDoenstExist.Any())
                returnErrorList.Add(@$"Vehicle {string.Join(", ", vehicleUpdateDoenstExist)} does not yet exist
                    when trying to apply changes of Endorsement {endorsementNumber} effective {effectiveDate:MM/dd/yyyy}.");

            return new OutOfSequenceChangeConflictException(returnErrorList);
        }
    }
}
