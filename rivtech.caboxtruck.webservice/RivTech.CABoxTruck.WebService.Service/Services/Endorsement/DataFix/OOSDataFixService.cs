using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Premiums;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DomainEntities.Premiums;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.DTO.Constants;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeComparers;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.DataFix
{
    public interface IOOSDataFixService
    {
        Task FixData(string policyNumber);
        Task FixDataWithCancelReinstate(string policyNumber);
        Task FixDataVehicleCompFT();
    }

    public class OOSDataFixService : IOOSDataFixService
    {
        private readonly IRiskService _riskService;
        private readonly INameAndAddressService _nameAndAddressService;
        private readonly IRiskRepository _riskRepository;
        private readonly IRiskDetailRepository _riskDetailRepository;
        private readonly IPolicyHistoryService _policyHistoryService;
        private readonly IBrokerInfoRepository _brokerInfoRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IEntityAddressRepository _entityAddressRepository;
        private readonly IDriverRepository _driverRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IPremiumService _premiumService;
        private readonly IRiskPremiumTransactionService _riskPremiumTransactionService;
        private readonly IChangeComparerContext _changeComparerContext;
        private readonly IEndorsementChangesService _endorsementChangesService;
        private readonly IRiskVehiclePremiumRepository _riskVehiclePremiumRepository;
        private readonly IRiskPolicyPremiumRepository _riskPolicyPremiumRepository;
        private readonly IBindingRepository _bindingRepository;

        public OOSDataFixService(
            IRiskService riskService,
            INameAndAddressService nameAndAddressService,
            IRiskRepository riskRepository,
            IRiskDetailRepository riskDetailRepository,
            IPolicyHistoryService policyHistoryService,
            IBrokerInfoRepository brokerInfoRepository,
            IAddressRepository addressRepository,
            IEntityAddressRepository entityAddressRepository,
            IDriverRepository driverRepository,
            IVehicleRepository vehicleRepository,
            IPremiumService premiumService,
            IRiskPremiumTransactionService riskPremiumTransactionService,
            IChangeComparerContext changeComparerContext,
            IEndorsementChangesService endorsementChangesService,
            IRiskVehiclePremiumRepository riskVehiclePremiumRepository,
            IRiskPolicyPremiumRepository riskPolicyPremiumRepository,
            IBindingRepository bindingRepository
        )
        {
            _riskService = riskService;
            _nameAndAddressService = nameAndAddressService;
            _riskRepository = riskRepository;
            _riskDetailRepository = riskDetailRepository;
            _policyHistoryService = policyHistoryService;
            _brokerInfoRepository = brokerInfoRepository;
            _addressRepository = addressRepository;
            _entityAddressRepository = entityAddressRepository;
            _driverRepository = driverRepository;
            _vehicleRepository = vehicleRepository;
            _premiumService = premiumService;
            _riskPremiumTransactionService = riskPremiumTransactionService;
            _changeComparerContext = changeComparerContext;
            _endorsementChangesService = endorsementChangesService;
            _riskVehiclePremiumRepository = riskVehiclePremiumRepository;
            _riskPolicyPremiumRepository = riskPolicyPremiumRepository;
            _bindingRepository = bindingRepository;
        }

        public async Task FixData(string policyNumber)
        {
            var policy = await _riskRepository.GetRiskByPolicyNumberAsync(policyNumber);
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(policy.Id);
            var policyHistories = await _policyHistoryService.GetEndorsementHistoryByRiskId(policy.Id);
            policyHistories = policyHistories.OrderBy(x => x.DateCreated).ToList();

            var riskDetails = await _riskDetailRepository.GetAsync(x => x.RiskId == policy.Id);
            riskDetails = riskDetails.OrderBy(x => x.CreatedDate);

            /*
            --Risk
            --1. CurrentEndorsementNumber
            --2. CurrentRiskDetailId
            --3. ForEndorsementRiskDetailId
            --4. LastReinstatedRiskDetailId
             */
            policy.CurrentEndorsementNumber = policyHistories.Max(x => Convert.ToInt32(x.EndorsementNumber));
            policy.ForEndorsementRiskDetailId = riskDetails.Last().Id;
            policy.CurrentRiskDetailId = riskDetails
                .Where(x => x.Status != RiskStatus.Canceled || x.Status != RiskStatus.Reinstated)
                .SkipLast(1) // skip for endorsement  riskdetail
                .Last().Id;
            await _riskRepository.SaveChangesAsync();


            /*
             * 	RiskDetail 
             * 	1. EndorsementNumber 
             * 	2. IsProcessed 
             * 	3. SourceRiskDetailId
             * 	4. VersionNumber
             * 	5. FromReinstatement
             */
            foreach (var riskDetail in riskDetails)
            {
                var policyHistory = policyHistories.FirstOrDefault(x => x.PreviousRiskDetailId == riskDetail.Id.ToString());
                var riskDTO = await _riskService.GetByIdIncludeAsync(riskDetail.Id);

                var riskDetailToUpdate = await _riskDetailRepository.FindAsync(riskDetail.Id);
                riskDetailToUpdate.EndorsementNumber = Convert.ToInt32(policyHistory?.EndorsementNumber ?? (policy.CurrentEndorsementNumber + 1).ToString());
                riskDetailToUpdate.IsProcessed = policyHistory != null;
                riskDetailToUpdate.VersionNumber = 0;
                riskDetailToUpdate.FromReinstatement = false;

                var previousHistory = policyHistories.FirstOrDefault(x => x.EndorsementNumber == (riskDetailToUpdate.EndorsementNumber - 1).ToString());
                Guid? previousRiskDetailId = previousHistory != null ? Guid.Parse(previousHistory.PreviousRiskDetailId) : (Guid?)null;

                riskDetailToUpdate.SourceRiskDetailId = previousRiskDetailId;

                await _riskDetailRepository.SaveChangesAsync();


                #region Driver
                /**
                 *  Driver
                 *  1. EffectiveDate
                 *  2. ExpirationDate
                 */

                foreach (var driver in riskDTO.Drivers)
                {
                    var driverToUpdate = await _driverRepository.FindAsync(driver.Id.Value);
                    var driverFullName = $"{driver.FirstName} {driver.LastName}";
                    if (policyHistory?.Details?.Contains(driverFullName) ?? false)
                    {
                        var details = JsonConvert.DeserializeObject<string[]>(policyHistory.Details);
                        var changeItem = details.Where(x => x.Contains(driverFullName));
                        if (changeItem.Contains("Added"))
                        {
                            driverToUpdate.EffectiveDate = policyHistory.EffectiveDate;
                            driverToUpdate.ExpirationDate = brokerInfo.ExpirationDate;
                        }
                        else if (changeItem.Contains("Removed"))
                        {
                            driverToUpdate.EffectiveDate ??= brokerInfo.EffectiveDate;
                            driverToUpdate.ExpirationDate = policyHistory.EffectiveDate;
                        }
                        else
                        {
                            driverToUpdate.EffectiveDate ??= brokerInfo.EffectiveDate;
                            driverToUpdate.ExpirationDate ??= brokerInfo.ExpirationDate;
                        }
                        await _driverRepository.SaveChangesAsync();
                    }
                }
                #endregion

                #region Garaging Address

                var currentEntityAddresses = await _entityAddressRepository.GetAsync(x => x.EntityId == riskDetailToUpdate.Insured.EntityId);

                //If no previous policy history, skip updating the addresses of current risk detail
                if (previousRiskDetailId != null)
                {
                    var previousRiskDetail = await _riskDetailRepository.FindAsync(previousRiskDetailId.Value);

                    //Get Entity Addresses from previous risk detail
                    var previousEntityAddresses = await _entityAddressRepository.GetAsync(x => x.EntityId == previousRiskDetail.Insured.EntityId);
                    var previousEntityAddressesStack = new Stack<EntityAddress>(previousEntityAddresses.Reverse());

                    //if (currentEntityAddresses.Count() != previousEntityAddressesStack.Count()) continue;
                    foreach (var item in currentEntityAddresses)
                    {
                        var currentEntityAddress = await _entityAddressRepository.FindAsync(item.Id);
                        var previousEntityAddress = previousEntityAddressesStack.Count != 0 ? previousEntityAddressesStack.Pop() : null;

                        if (previousEntityAddress != null)
                        {
                            currentEntityAddress.Address = previousEntityAddress.Address;
                            currentEntityAddress.AddressId = previousEntityAddress.AddressId;
                        }

                        await _entityAddressRepository.SaveChangesAsync();
                    }
                }

                //Update IsMainGaragingAddress for current risk detail
                //Set IsMainGaragingAddress of Current Entity Addresses if property is empty
                foreach (var item in currentEntityAddresses)
                {
                    var currentAddress = await _addressRepository.FindAsync(x => x.Id == item.AddressId);
                    var vehiclesFromCurrentRiskDetail = await _vehicleRepository.GetByRiskDetailAsync(riskDetailToUpdate.Id);
                    var vehicle = vehiclesFromCurrentRiskDetail.FirstOrDefault();

                    var currentEntityAddress = await _entityAddressRepository.FindAsync(item.Id);

                    currentEntityAddress.IsMainGaragingAddress = 
                        (currentAddress.IsMainGarage || currentAddress.Id == vehicle.GaragingAddressId);
                    await _entityAddressRepository.SaveChangesAsync();
                }

                var currentEntityGaragingAddresses = await _entityAddressRepository.GetAsync(x => x.AddressTypeId == 3 && x.EntityId == riskDetailToUpdate.Insured.EntityId);

                //Link the garaging address of current vehicles
                foreach (var item in currentEntityGaragingAddresses)
                {
                    if (item.IsMainGaragingAddress.Value)
                    {
                        var vehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDetailToUpdate.Id);

                        foreach (var vehicle in vehicles)
                        {
                            var currentVehicle = await _vehicleRepository.FindAsync(vehicle.Id);
                            currentVehicle.GaragingAddressId = item.AddressId;
                            await _vehicleRepository.SaveChangesAsync();
                        }
                    }
                }

                #endregion

                /**
                 *  Vehicle Premiums and transactions
                 */
                await _premiumService.UpdateVehiclePremiumsForDataFix(riskDetailToUpdate.Id,
                    (int)riskDTO.Binding.BindOptionId,
                    fromPolicy: riskDetailToUpdate.EndorsementNumber > 0);

                /**
                 * Policy Premiums
                 */
                await _premiumService.UpdatePolicyPremiums(riskDetailToUpdate.Id, (int)riskDTO.Binding.BindOptionId);

                await _riskPremiumTransactionService.CalculateAsync(
                    riskDetailToUpdate.Id,
                    previousHistory != null ? Guid.Parse(previousHistory.PreviousRiskDetailId) : (Guid?)null,
                    fromReinstatement: false);

                /**
                 * Endorsement Pending Changes
                 */
                if (riskDetailToUpdate.EndorsementNumber > 0)
                {
                    var changes = await _changeComparerContext.CompareChanges(riskDetailToUpdate.Id, Guid.Parse(previousHistory.PreviousRiskDetailId));
                    await _endorsementChangesService.UpdateEndorsementChangesAsync(changes, policy.Id, riskDetailToUpdate.EndorsementNumber);
                }
            }
        }


        public async Task FixDataWithCancelReinstate(string policyNumber)
        {
            var policy = await _riskRepository.GetRiskByPolicyNumberAsync(policyNumber);
            var brokerInfo = await _brokerInfoRepository.GetByRiskIdAllAsync(policy.Id);
            var policyHistories = await _policyHistoryService.GetEndorsementHistoryByRiskId(policy.Id);
            policyHistories = policyHistories.OrderBy(x => x.DateCreated).ToList();

            var riskDetails = await _riskDetailRepository.GetAsync(x => x.RiskId == policy.Id);
            riskDetails = riskDetails.OrderBy(x => x.CreatedDate);

            /*
            --Risk
            --1. CurrentEndorsementNumber
            --2. CurrentRiskDetailId
            --3. ForEndorsementRiskDetailId
            --4. LastReinstatedRiskDetailId
             */
            policy.CurrentEndorsementNumber = policyHistories.Max(x => Convert.ToInt32(x.EndorsementNumber));
            policy.ForEndorsementRiskDetailId = riskDetails.Last().Id;
            policy.CurrentRiskDetailId = riskDetails
                .Where(x => x.Status != RiskStatus.Canceled || x.Status != RiskStatus.Reinstated)
                .SkipLast(1) // skip for endorsement  riskdetail
                .Last().Id;
            await _riskRepository.SaveChangesAsync();


            /*
             * 	RiskDetail 
             * 	1. EndorsementNumber 
             * 	2. IsProcessed 
             * 	3. SourceRiskDetailId
             * 	4. VersionNumber
             * 	5. FromReinstatement
             */
            foreach (var riskDetail in riskDetails)
            {
                policy = await _riskRepository.FindAsync(policy.Id);
                var policyHistory = policyHistories.FirstOrDefault(x => x.PreviousRiskDetailId == riskDetail.Id.ToString());
                var riskDTO = await _riskService.GetByIdIncludeAsync(riskDetail.Id);

                var riskDetailToUpdate = await _riskDetailRepository.FindAsync(riskDetail.Id);

                riskDetailToUpdate.IsProcessed = policyHistory != null;
                riskDetailToUpdate.VersionNumber = 0;
                PolicyHistoryDTO previousHistory = null;
                var policyHistoryIndex = policyHistories.IndexOf(policyHistory);
                if (policyHistoryIndex > 0)
                {
                    previousHistory = policyHistories.Where(x => x.PolicyStatus != RiskStatus.Reinstated).ToArray()[policyHistoryIndex - 1];
                }
                else if (policyHistory is null)
                {
                    previousHistory = policyHistories.Where(x => x.PolicyStatus != RiskStatus.Reinstated).Last();
                }
                Guid? previousRiskDetailId = previousHistory != null ? Guid.Parse(previousHistory.PreviousRiskDetailId) : (Guid?)null;

                if (policyHistory is null || policyHistory.PolicyStatus == RiskStatus.Active)
                {
                    riskDetailToUpdate.EndorsementNumber = Convert.ToInt32(policyHistory?.EndorsementNumber ?? (policy.CurrentEndorsementNumber + 1).ToString());
                    riskDetailToUpdate.Status = RiskStatus.Active;

                    if (policyHistory != null)
                    {
                        policy.CurrentRiskDetailId = riskDetailToUpdate.Id;
                        policy.CurrentEndorsementNumber = riskDetailToUpdate.EndorsementNumber;
                    }
                }
                else
                {
                    riskDetailToUpdate.EndorsementNumber = policy.CurrentEndorsementNumber;
                }

                riskDetailToUpdate.FromReinstatement = policyHistories
                    .Any(x => x.PolicyStatus == RiskStatus.Reinstated && Guid.Parse(x.PreviousRiskDetailId) == previousRiskDetailId);

                if (riskDetailToUpdate.FromReinstatement)
                {
                    riskDetailToUpdate.SourceRiskDetailId = policy.CurrentRiskDetailId;
                }
                else
                {
                    riskDetailToUpdate.SourceRiskDetailId = previousRiskDetailId;
                }

                await _riskDetailRepository.SaveChangesAsync();
                await _riskRepository.SaveChangesAsync();


                #region Driver
                /**
                 *  Driver
                 *  1. EffectiveDate
                 *  2. ExpirationDate
                 */

                foreach (var driver in riskDTO.Drivers)
                {
                    var driverToUpdate = await _driverRepository.FindAsync(driver.Id.Value);
                    var driverFullName = $"{driver.FirstName} {driver.LastName}";
                    if (policyHistory?.Details?.Contains(driverFullName) ?? false)
                    {
                        var details = JsonConvert.DeserializeObject<string[]>(policyHistory.Details);
                        var changeItem = details.Where(x => x.Contains(driverFullName));
                        if (changeItem.Contains("Added"))
                        {
                            driverToUpdate.EffectiveDate = policyHistory.EffectiveDate;
                            driverToUpdate.ExpirationDate = brokerInfo.ExpirationDate;
                        }
                        else if (changeItem.Contains("Removed"))
                        {
                            driverToUpdate.EffectiveDate ??= brokerInfo.EffectiveDate;
                            driverToUpdate.ExpirationDate = policyHistory.EffectiveDate;
                        }
                        else
                        {
                            driverToUpdate.EffectiveDate ??= brokerInfo.EffectiveDate;
                            driverToUpdate.ExpirationDate ??= brokerInfo.ExpirationDate;
                        }
                        await _driverRepository.SaveChangesAsync();
                    }
                }
                #endregion

                #region Garaging Address

                var currentEntityAddresses = await _entityAddressRepository.GetAsync(x => x.EntityId == riskDetailToUpdate.Insured.EntityId);

                //If no previous policy history, skip updating the addresses of current risk detail
                if (previousRiskDetailId != null)
                {
                    //var previousRiskDetail = await _riskDetailRepository.FindAsync(previousRiskDetailId.Value);
                    var previousRiskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(previousRiskDetailId.Value);

                    //Get Entity Addresses from previous risk detail
                    var previousEntityAddresses = await _entityAddressRepository.GetAsync(x => x.EntityId == previousRiskDetail.Insured.EntityId);
                    var previousEntityAddressesStack = new Stack<EntityAddress>(previousEntityAddresses.Reverse());

                    //if (currentEntityAddresses.Count() != previousEntityAddressesStack.Count()) continue;
                    foreach (var item in currentEntityAddresses)
                    {
                        var currentEntityAddress = await _entityAddressRepository.FindAsync(item.Id);
                        var previousEntityAddress = previousEntityAddressesStack.Count != 0 ? previousEntityAddressesStack.Pop() : null;

                        if (previousEntityAddress != null)
                        {
                            currentEntityAddress.Address = previousEntityAddress.Address;
                            currentEntityAddress.AddressId = previousEntityAddress.AddressId;
                        }

                        await _entityAddressRepository.SaveChangesAsync();
                    }
                }

                //Update IsMainGaragingAddress for current risk detail
                //Set IsMainGaragingAddress of Current Entity Addresses if property is empty
                foreach (var item in currentEntityAddresses)
                {
                    var currentAddress = await _addressRepository.FindAsync(x => x.Id == item.AddressId);
                    var vehiclesFromCurrentRiskDetail = await _vehicleRepository.GetByRiskDetailAsync(riskDetailToUpdate.Id);
                    var vehicle = vehiclesFromCurrentRiskDetail.FirstOrDefault();

                    var currentEntityAddress = await _entityAddressRepository.FindAsync(item.Id);

                    currentEntityAddress.IsMainGaragingAddress =
                        (currentAddress.IsMainGarage || currentAddress.Id == vehicle.GaragingAddressId);
                    await _entityAddressRepository.SaveChangesAsync();
                }

                var currentEntityGaragingAddresses = await _entityAddressRepository.GetAsync(x => x.AddressTypeId == 3 && x.EntityId == riskDetailToUpdate.Insured.EntityId);

                //Link the garaging address of current vehicles
                foreach (var item in currentEntityGaragingAddresses)
                {
                    if (item.IsMainGaragingAddress.Value)
                    {
                        var vehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDetailToUpdate.Id);

                        foreach (var vehicle in vehicles)
                        {
                            var currentVehicle = await _vehicleRepository.FindAsync(vehicle.Id);
                            currentVehicle.GaragingAddressId = item.AddressId;
                            await _vehicleRepository.SaveChangesAsync();
                        }
                    }
                }

                #endregion

                var previousTransactionRiskId = previousRiskDetailId;
                //if (riskDetailToUpdate.FromReinstatement)
                //{
                //    var previousCanceledHistory = policyHistories[policyHistoryIndex - 1];
                //    previousTransactionRiskId = Guid.Parse(previousCanceledHistory.PreviousRiskDetailId);
                //}

                if (riskDetailToUpdate.Status == RiskStatus.Active || riskDetailToUpdate.Status.Equals("Quoted", StringComparison.OrdinalIgnoreCase))
                {
                    await _riskDetailRepository.SaveChangesAsync();

                    /**
                     *  Vehicle Premiums and transactions
                     */
                    await _premiumService.UpdateVehiclePremiumsForDataFix(riskDetailToUpdate.Id,
                        (int)riskDTO.Binding.BindOptionId,
                        fromPolicy: riskDetailToUpdate.EndorsementNumber > 0);

                    /**
                     * Policy Premiums
                     */
                    await _premiumService.UpdatePolicyPremiums(riskDetailToUpdate.Id, (int)riskDTO.Binding.BindOptionId);

                    await _riskPremiumTransactionService.CalculateAsync(
                        riskDetailToUpdate.Id,
                        previousTransactionRiskId,
                        fromReinstatement: riskDetailToUpdate.FromReinstatement);
                }
                else if (riskDetailToUpdate.Status == RiskStatus.Canceled)
                {
                    var riskVehiclePremiumsList = await _riskVehiclePremiumRepository.GetByRiskDetailAsync(previousRiskDetailId.Value);
                    foreach (var vehiclePremiums in riskVehiclePremiumsList)
                    {
                        var existingVehiclePremiums = await _riskVehiclePremiumRepository.GetByVehicleAsync(riskDetail.Id, vehiclePremiums.VehicleMainId);
                        if (existingVehiclePremiums != null)
                        {
                            await _riskVehiclePremiumRepository.DeleteAsync(existingVehiclePremiums);
                        }

                        var cloneVehiclePremiums = vehiclePremiums.Clone();
                        cloneVehiclePremiums.ChangeRiskDetail(riskDetail.Id);
                        cloneVehiclePremiums.Cancel(policyHistory.EffectiveDate.Value);
                        await _riskVehiclePremiumRepository.SaveAsync(cloneVehiclePremiums);
                    }

                    var glPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(previousRiskDetailId.Value, Premium.TYPE_POLICY_GL);
                    var existingGLPremiums = await _riskPolicyPremiumRepository.GetByRiskDetailAsync(riskDetail.Id, Premium.TYPE_POLICY_GL);
                    if (existingGLPremiums != null)
                    {
                        await _riskPolicyPremiumRepository.DeleteAsync(existingGLPremiums);
                    }
                    var clonedGLPremiums = glPremiums.Clone();
                    clonedGLPremiums.ChangeRiskDetail(riskDetail.Id);
                    clonedGLPremiums.Cancel(policyHistory.EffectiveDate.Value);
                    await _riskPolicyPremiumRepository.SaveAsync(clonedGLPremiums);

                    await _riskPremiumTransactionService.CalculateAsync(
                        riskDetailToUpdate.Id,
                        previousTransactionRiskId,
                        fromReinstatement: riskDetailToUpdate.FromReinstatement);

                    var reinstatePolicyHistory = policyHistories.SingleOrDefault(x => Guid.Parse(x.PreviousRiskDetailId) == riskDetail.Id && x.PolicyStatus == RiskStatus.Reinstated);
                    if (reinstatePolicyHistory != null)
                    {
                        var lastIssuedRiskDetail = (await _riskDetailRepository.GetAsync(x => x.Id == previousRiskDetailId.Value)).First();

                        await _premiumService.GenerateReinstatementPremiums(lastIssuedRiskDetail, riskDetail);

                        // Generate transactions for reinstatement premiums.
                        await _riskPremiumTransactionService.CalculateAfterReinstatementAsync(riskDetail);

                        var policyToUpdate = await _riskRepository.FindAsync(policy.Id);
                        policyToUpdate.LastReinstatedRiskDetailId = riskDetail.Id;
                        await _riskRepository.SaveChangesAsync();
                        policy = policyToUpdate;
                    }
                }


                /**
                 * Endorsement Pending Changes
                 */
                if (riskDetailToUpdate.EndorsementNumber > 0 && riskDetailToUpdate.Status != RiskStatus.Canceled)
                {
                    var changes = await _changeComparerContext.CompareChanges(riskDetailToUpdate.Id, Guid.Parse(previousHistory.PreviousRiskDetailId));
                    await _endorsementChangesService.UpdateEndorsementChangesAsync(changes, policy.Id, riskDetailToUpdate.EndorsementNumber);
                }
            }
        }

        public async Task FixDataVehicleCompFT()
        {
            var riskDTOs = await _riskService.GetAllAsync();

            foreach (var riskDTO in riskDTOs)
            {
                var riskDetail = await _riskDetailRepository.GetOneIncludeByIdAsync(riskDTO.Id.Value);

                var brokerInfo = await _brokerInfoRepository.FindAsync(x => x.RiskId == riskDTO.RiskId);
                var binding = await _bindingRepository.FindAsync(x => x.RiskId == riskDTO.RiskId);

                var optionNumber = (riskDTO.Status == "Active" || riskDTO.Status == "Canceled") ? binding.BindOptionId : 1;

                var riskCoverage = riskDetail.RiskCoverages.Where(x => x.OptionNumber == optionNumber).FirstOrDefault();

                if (riskCoverage == null || (riskCoverage.HasCompFtLimit() && riskCoverage.HasCollisionLimit()))
                    continue;

                var vehicles = await _vehicleRepository.GetByRiskDetailAsync(riskDTO.Id.Value, true);

                foreach (var vehicle in vehicles)
                {
                    var currentVehicle = await _vehicleRepository.FindAsync(vehicle.Id);

                    if (!riskCoverage.HasCompFtLimit())
                    {
                        currentVehicle.SetCompFtDeductible(null);
                    }

                    if (!riskCoverage.HasCollisionLimit())
                    {
                        currentVehicle.SetCollisionDeductible(null);
                    }

                    await _vehicleRepository.SaveChangesAsync();
                }
            }
        }

     }
}
