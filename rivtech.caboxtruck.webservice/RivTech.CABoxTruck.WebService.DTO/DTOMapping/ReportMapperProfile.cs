using AutoMapper;
using RivTech.CABoxTruck.WebService.Common;
using RivTech.CABoxTruck.WebService.Common.ReportTemplates;
using RivTech.CABoxTruck.WebService.Data.DALDapperModels;
using RivTech.CABoxTruck.WebService.DTO.Report;
using System.Globalization;

namespace RivTech.CABoxTruck.WebService.DTO.DTOMapping
{
    public class ReportMapperProfile : Profile
    {
        public ReportMapperProfile()
        {
            //DriverReport
            CreateMap<DriverDTO, DriverReportTemplate>()
                .ForMember(d => d.FirstName, opt => opt.MapFrom(src => src.FirstName ?? string.Empty))
                .ForMember(d => d.LastName, opt => opt.MapFrom(src => src.LastName ?? string.Empty))
                .ForMember(d => d.MiddleName, opt => opt.MapFrom(src => src.MiddleName ?? string.Empty))
                .ForMember(d => d.LicenseNumber, opt => opt.MapFrom(src => src.LicenseNumber ?? string.Empty))
                .ForMember(d => d.State, opt => opt.MapFrom(src => src.State ?? string.Empty))
                .ForMember(d => d.BirthDate, opt =>
                    opt.MapFrom(src => src.BirthDate.HasValue ? src.BirthDate.Value.ToString("MM/dd/yyyy") : string.Empty))
                .ForMember(d => d.YrsDrivingExp, opt =>
                    opt.MapFrom(src => src.YrsDrivingExp.HasValue ? src.YrsDrivingExp.ToString() : string.Empty))
                .ForMember(d => d.YrsCommDrivingExp, opt =>
                    opt.MapFrom(src => src.YrsCommDrivingExp.HasValue ? src.YrsCommDrivingExp.ToString() : string.Empty));

            //VehicleReport
            CreateMap<VehicleDTO, VehicleReportTemplate>()
                .ForMember(d => d.VIN, opt => opt.MapFrom(src => src.VIN ?? string.Empty ?? string.Empty))
                .ForMember(d => d.Year, opt => opt.MapFrom(src => src.Year.ToString() ?? string.Empty))
                .ForMember(d => d.Make, opt => opt.MapFrom(src => src.Make ?? string.Empty))
                .ForMember(d => d.Model, opt => opt.MapFrom(src => src.Model ?? string.Empty))
                .ForMember(d => d.RegisteredState, opt => opt.MapFrom(src => src.RegisteredState ?? string.Empty))
                .ForMember(d => d.StatedAmount, opt => opt.MapFrom(src => src.StatedAmount.ToString("C0", new CultureInfo("en-US")) ?? string.Empty))
                .ForMember(d => d.VehicleType, opt => opt.MapFrom(src => src.VehicleType ?? string.Empty))
                .ForMember(d => d.Description, opt => opt.MapFrom(src => src.Description ?? string.Empty ?? string.Empty))
                .ForMember(d => d.BusinessClass, opt => opt.MapFrom(src => src.BusinessClass ?? string.Empty))
                .ForMember(d => d.GrossVehicleWeight, opt => opt.MapFrom(src => src.GrossVehicleWeight ?? string.Empty))
                .ForMember(d => d.Radius, opt => opt.MapFrom(src => src.Radius.HasValue ? src.Radius.Value.ToString("F0") ?? string.Empty : string.Empty))
                .ForMember(d => d.IsCoverageFireTheft, opt =>
                    opt.MapFrom(src => src.IsCoverageFireTheft.HasValue ? (src.IsCoverageFireTheft.Value ? "Yes" : "No") : string.Empty))
                .ForMember(d => d.IsCoverageCollision, opt =>
                    opt.MapFrom(src => src.IsCoverageCollision.HasValue ? (src.IsCoverageCollision.Value ? "Yes" : "No") : string.Empty))
                .ForMember(d => d.CoverageFireTheft, opt => opt.MapFrom(src => src.CoverageFireTheft ?? string.Empty))
                .ForMember(d => d.CoverageCollisionDeductible, opt => opt.MapFrom(src => src.CoverageCollisionDeductible ?? string.Empty))
                .ForMember(d => d.HasCoverageCargo, opt =>
                    opt.MapFrom(src => src.HasCoverageCargo.HasValue ? (src.HasCoverageCargo.Value ? "Yes" : "No") : string.Empty))
                .ForMember(d => d.HasCoverageRefrigeration, opt =>
                    opt.MapFrom(src => src.HasCoverageRefrigeration.HasValue ? (src.HasCoverageRefrigeration.Value ? "Yes" : "No") : string.Empty));

            //DMV Report
            CreateMap<DMVReportModel, DMVReportTemplate>()
                .ForMember(d => d.ErrorCorrection, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.Agent, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.IDState, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.InsuredDateOfBirth, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.InsuredFirstName, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.InsuredMiddleName, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.InsuredGender, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.ZipCode4N, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.GarageAddress, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.GarageCity, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.GarageState, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.GarageZip5N, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.GarageZip4N, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.LicensePlate, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.HireIndicator, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.HireCancellationReasons, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.DeductibleCollision, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.DeductibleComprehensive, opt => opt.MapFrom(src => string.Empty))
                .ForMember(d => d.PriorPolicyNumber, opt => opt.MapFrom(src => string.Empty))

                .ForMember(d => d.IndividualLimit2, opt => opt.MapFrom(src => src.IndividualLimit2.Replace("$", string.Empty)))
                .ForMember(d => d.OccurrenceLimit2, opt => opt.MapFrom(src => src.OccurrenceLimit2.Replace("$", string.Empty)))
                .ForMember(d => d.CSLLimit1, opt => opt.MapFrom(src =>src.CSLLimit1.ToString("#,#", CultureInfo.InvariantCulture)))
                .ForMember(d => d.EffectiveDate, opt => opt.MapFrom(src => src.EffectiveDate.ToString("MM/dd/yyyy")))
                .ForMember(d => d.ExpirationDate, opt => opt.MapFrom(src => src.ExpirationDate.ToString("MM/dd/yyyy")))
                .ForMember(d => d.TransactionEffectiveDate, opt => opt.MapFrom(src => src.TransactionEffectiveDate.ToString("MM/dd/yyyy")))
                .ForMember(d => d.IssuanceDate, opt => opt.MapFrom(src => src.IssuanceDate.ToString("MM/dd/yyyy")))
                .ForMember(d => d.VehicleEffectiveDate, opt => opt.MapFrom(src => src.VehicleEffectiveDate.ToString("MM/dd/yyyy")))
                .ForMember(d => d.VehicleExpirationDate, opt => opt.MapFrom(src => src.VehicleExpirationDate.ToString("MM/dd/yyyy")));

            //POM Report
            CreateMap<POMReportModel, POMReportTemplate>()
                .ForMember(p => p.Count, opt => opt.MapFrom(src => src.Count))
                .ForMember(p => p.PolicyNumber, opt => opt.MapFrom(src => src.PolicyNumber))
                .ForMember(p => p.InsuredName, opt => opt.MapFrom(src => src.InsuredName))
                .ForMember(p => p.MailingAddress, opt => opt.MapFrom(src => src.MailingAddress))
                .ForMember(p => p.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate.ToString("MM/dd/yy")))
                .ForMember(p => p.CancellationDate, opt => opt.MapFrom(src => src.CancellationDate.ToString("MM/dd/yy")))
                .ForMember(p => p.NoticeType, opt => opt.MapFrom(src => src.NoticeType))
                .ForMember(p => p.Postage, opt => opt.MapFrom(src => src.Postage.ToString()))
                .ForMember(p => p.Fee, opt => opt.MapFrom(src => src.Fee.ToString()));
            //.ForMember(p => p.TotalNotices, opt => opt.MapFrom(src => src.TotalNotices));

            CreateMap<AgingReportDTO, AgingReportTemplate>()
                .ForMember(template => template.PolicyNumber, model => model.MapFrom(src => src.PolicyNumber ?? string.Empty))
                .ForMember(template => template.PolicyInception, model => model.MapFrom(src => src.PolicyInception))
                .ForMember(template => template.Insured, model => model.MapFrom(src => src.Insured ?? string.Empty))
                .ForMember(template => template.InvoiceNumber, model => model.MapFrom(src => src.InvoiceNumber ?? string.Empty))
                .ForMember(template => template.InvoiceDate, model => model.MapFrom(src => src.InvoiceDate))
                .ForMember(template => template.PastDueAmount, model => model.MapFrom(src => src.PastDueAmount))
                .ForMember(template => template.DueDate, model => model.MapFrom(src => src.DueDate))
                .ForMember(template => template.TotalAmountDue, model => model.MapFrom(src => src.TotalAmountDue))
                .ForMember(template => template.Current, model => model.MapFrom(src => src.Current ?? string.Empty))
                .ForMember(template => template.ZeroToThirty, model => model.MapFrom(src => src.ZeroToThirty ?? string.Empty))
                .ForMember(template => template.ThirtyOneToSixty, model => model.MapFrom(src => src.ThirtyOneToSixty ?? string.Empty))
                .ForMember(template => template.SixtyOneToNinety, model => model.MapFrom(src => src.SixtyOneToNinety ?? string.Empty))
                .ForMember(template => template.NinetyOnePlus, model => model.MapFrom(src => src.NinetyOnePlus ?? string.Empty));
        }
    }
}
