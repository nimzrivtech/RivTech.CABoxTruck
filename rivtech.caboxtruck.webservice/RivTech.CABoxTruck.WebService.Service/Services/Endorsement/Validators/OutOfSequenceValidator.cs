using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Submission;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.Exceptions;
using RivTech.CABoxTruck.WebService.DTO.Notification;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.IServices.Billing;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Proration;
using RivTech.CABoxTruck.WebService.Service.Services.Premiums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.Validators
{
    public class OutOfSequenceValidator
    {
        private readonly INotificationService _notificationService;
        public OutOfSequenceValidator(
            INotificationService notificationService
        )
        {
            _notificationService = notificationService;
        }
        public List<OutOfSequenceValidationException> errorList { get; set; }
        public void Validate(IEndorsementChangeCollection endorsementChanges)
        {
            if (endorsementChanges is null || !endorsementChanges.Any())
                return;

            this.errorList = new List<OutOfSequenceValidationException>();

            ValidateLimits(endorsementChanges);
            ValidateRatingFactors(endorsementChanges);
            ValidateVehicle(endorsementChanges);
            ValidateDriver(endorsementChanges);
            ValidateAdditionalInterest(endorsementChanges);
            ValidateManuscripts(endorsementChanges);
            ValidateForms(endorsementChanges);

            if(errorList.Any())
                throw new OutOfSequenceChangeConflictException(errorList.Select(x => x.Message));
        }
        public async Task Validate(IEndorsementChangeCollection endorsementChanges, NotificationFilterDTO notificationFilterDTO)
        {
            if (endorsementChanges is null || !endorsementChanges.Any())
                return;

            this.errorList = new List<OutOfSequenceValidationException>();

            ValidateLimits(endorsementChanges);
            ValidateRatingFactors(endorsementChanges);
            ValidateVehicle(endorsementChanges);
            ValidateDriver(endorsementChanges);
            ValidateAdditionalInterest(endorsementChanges);
            ValidateManuscripts(endorsementChanges);
            ValidateForms(endorsementChanges);

            if (errorList.Any())
            {
                notificationFilterDTO.ErrorMessages = JsonConvert.SerializeObject(errorList.Select(x => x.Message));

                await _notificationService.UpdateNotificationStatus(notificationFilterDTO);

                throw new OutOfSequenceChangeConflictException(errorList.Select(x => x.Message));
            }
        }

        private void ValidateVehicle(IEndorsementChangeCollection endorsementChanges)
        {
            var vehicleUpdateChanges = endorsementChanges.Where(x => x is VehicleUpdateChangeItem).Select(x => x as VehicleUpdateChangeItem)
                                        .Where(x => !(x.IsEndorsement1CargoChange == true
                                            && x.IsEndorsement1ReeferChange == true));
            if (vehicleUpdateChanges.Any())
            {
                //throw new OutOfSequenceValidationException("Vehicle changes are not yet supported in Out of Sequence endorsement.");
                this.errorList.Add(new OutOfSequenceValidationException("Vehicle changes are not yet supported in Out of Sequence endorsement."));
            }
        }

        private void ValidateDriver(IEndorsementChangeCollection endorsementChanges)
        {
            var driverUpdateChanges = endorsementChanges.Where(x => x is DriverUpdateChangeItem).Select(x => x as DriverUpdateChangeItem);
            if (driverUpdateChanges.Any())
            {
                errorList.Add(new OutOfSequenceValidationException("Driver changes are not yet supported in Out of Sequence endorsement."));
            }
        }

        private void ValidateLimits(IEndorsementChangeCollection endorsementChanges)
        {
            bool hasLimitsChanges = endorsementChanges.Any(x => x is CoverageLimitsChangeItem);
            if (hasLimitsChanges)
            {
                //throw new OutOfSequenceValidationException("Limit changes are not yet supported in Out of Sequence endorsement.");
                this.errorList.Add(new OutOfSequenceValidationException("Limit changes are not yet supported in Out of Sequence endorsement."));
            }
        }
        private void ValidateAdditionalInterest(IEndorsementChangeCollection endorsementChanges)
        {
            bool hasAddtlChanges = endorsementChanges.Any(x => x is AdditionalInterestChangeItem);
            if (hasAddtlChanges)
            {
                //throw new OutOfSequenceValidationException("Additional Premium changes are not supported in Out of Sequence Endorsement.");
                this.errorList.Add(new OutOfSequenceValidationException("Additional Premium changes are not supported in Out of Sequence Endorsement."));
            }
        }

        private void ValidateManuscripts(IEndorsementChangeCollection endorsementChanges)
        {
            bool hasManuChanges = endorsementChanges.Any(x => x is ManusciptChangeItem);
            if (hasManuChanges)
            {
                //throw new OutOfSequenceValidationException("Manuscript changes are not supported in Out of Sequence Endorsement.");
                this.errorList.Add(new OutOfSequenceValidationException("Manuscript changes are not supported in Out of Sequence Endorsement."));
            }
        }
        private void ValidateForms(IEndorsementChangeCollection endorsementChanges)
        {
            bool hasHiredPDChanges = endorsementChanges.Any(x => x is FormHiredPhysicalDamageEndorsementChangeItem);
            if (hasHiredPDChanges)
            {
                //throw new OutOfSequenceValidationException("Hired PD premium changes are not supported in Out of Sequence Endorsement.");
                this.errorList.Add(new OutOfSequenceValidationException("Hired PD premium changes are not supported in Out of Sequence Endorsement."));
            }

            bool hasTrailerInterchangeChanges = endorsementChanges.Any(x => x is FormTrailerInterchangeCoverageChangeItem);
            if (hasTrailerInterchangeChanges)
            {
                //throw new OutOfSequenceValidationException("Trailer Interchange premium changes are not supported in Out of Sequence Endorsement.");
                this.errorList.Add(new OutOfSequenceValidationException("Trailer Interchange premium changes are not supported in Out of Sequence Endorsement."));
            }
        }

        private void ValidateRatingFactors(IEndorsementChangeCollection endorsementChanges)
        {
            bool hasRatingFactorChanges = endorsementChanges.Any(x => x is RatingFactorsChangeItem);
            if(hasRatingFactorChanges)
            {
                errorList.Add(new OutOfSequenceValidationException("Schedule RF changes are not yet supported in Out of Sequence endorsement"));
            }
        }


    }
}
