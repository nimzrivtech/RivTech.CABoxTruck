using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class DriverUpdateChangeModifier : ChangeModifierBase<DriverUpdateChangeItem>
    {
        private readonly IDriverService _driverService;

        public DriverUpdateChangeModifier(IDriverService driverService)
        {
            _driverService = driverService;
        }

        public async override Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            var data = (EndorsementData)endorsementData;
            DriverUpdateChangeItem change = CastOriginalType(changeItem);

            var drivers = await _driverService.GetByRiskDetailIdIncludeAsync(data.RiskDetailId);
            var driver = drivers.First(x => x.LicenseNumber == change.LicenseNumber);

            if (change.LicenseNumber != null)
                driver.LicenseNumber = change.LicenseNumber;

            if (change.FirstName != null)
                driver.FirstName = change.FirstName;

            if (change.LastName != null)
                driver.LastName = change.LastName;

            if (change.MiddleName != null)
                driver.MiddleName = change.MiddleName;
            if (change.BirthDate != null)
                driver.BirthDate = change.BirthDate;

            if (change.YrsDrivingExp != null)
                driver.YrsDrivingExp = change.YrsDrivingExp;

            if (change.YrsCommDrivingExp != null)
                driver.YrsCommDrivingExp = change.YrsCommDrivingExp;

            if (change.State != null)
                driver.State = change.State;

            if (change.StateFull != null)
                driver.StateFull = change.StateFull;

            if (change.LicenseNumber != null)
                driver.LicenseNumber = change.LicenseNumber;

            if (change.IsMvr != null)
                driver.IsMvr = change.IsMvr;

            if (change.YearPoint != null)
                driver.YearPoint = change.YearPoint;

            if (change.AgeFactor != null)
                driver.AgeFactor = change.AgeFactor;

            if (change.TotalFactor != null)
                driver.TotalFactor = change.TotalFactor;

            if (change.IsOutOfState != null)
                driver.IsOutOfState = change.IsOutOfState;

            if (change.MvrDate != null)
                driver.MvrDate = change.MvrDate;

            if (change.HireDate != null)
                driver.HireDate = change.HireDate;

            if (change.isExcludeKO != null)
                driver.isExcludeKO = change.isExcludeKO;

            if (change.koReason != null)
                driver.koReason = change.koReason;

            if (change.Options != null)
                driver.Options = change.Options;

            if (change.IsActive != null)
                driver.IsActive = change.IsActive;
            if (change.DeletedDate != null)
                driver.DeletedDate = change.DeletedDate;
            if (change.IsValidated != null)
                driver.IsValidated = change.IsValidated;
            if (change.IsValidatedKO != null)
                driver.IsValidatedKO = change.IsValidatedKO;
            if (change.EffectiveDate != null)
                driver.EffectiveDate = change.EffectiveDate;
            if (change.ExpirationDate != null)
                driver.ExpirationDate = change.ExpirationDate;

            await _driverService.UpdateAsync(driver);
        }
    }
}
