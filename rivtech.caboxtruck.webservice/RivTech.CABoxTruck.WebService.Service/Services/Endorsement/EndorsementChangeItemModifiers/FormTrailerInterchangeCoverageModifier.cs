using AutoMapper;
using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement;
using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.Service.IServices;
using RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItemModifiers
{
    public class FormTrailerInterchangeCoverageModifier : ChangeModifierBase<FormTrailerInterchangeCoverageChangeItem>
    {
        private readonly IFormsService _formsService;
        private readonly IMapper _mapper;
        public FormTrailerInterchangeCoverageModifier(IFormsService formsService, IMapper mapper)
        {
            _formsService = formsService;
            _mapper = mapper;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            //var data = (EndorsementData)endorsementData;
            //var change = CastOriginalType(changeItem);

            //var riskFormDTO = data.RiskDTO.RiskForms.FirstOrDefault(x => x.FormId == "TrailerInterchangeCoverage");

            //if (!string.IsNullOrWhiteSpace(riskFormDTO.Other))
            //{
            //    var jsonData = JsonConvert.DeserializeObject<FormTrailerInterchangeCoverageChangeItem>(riskFormDTO.Other);

            //    jsonData.CompreLimitOfInsurance = change.CompreLimitOfInsurance ?? jsonData.CompreLimitOfInsurance;
            //    jsonData.CompreDeductible = change.CompreDeductible ?? jsonData.CompreDeductible;
            //    jsonData.ComprePremium = change.ComprePremium ?? jsonData.ComprePremium;
            //    jsonData.LossLimitOfInsurance = change.LossLimitOfInsurance ?? jsonData.LossLimitOfInsurance;
            //    jsonData.LossDeductible = change.LossDeductible ?? jsonData.LossDeductible;
            //    jsonData.LossPremium = change.LossPremium ?? jsonData.LossPremium;
            //    jsonData.CollisionLimitOfInsurance = change.CollisionLimitOfInsurance ?? jsonData.CollisionLimitOfInsurance;
            //    jsonData.CollisionDeductible = change.CollisionDeductible ?? jsonData.CollisionDeductible;
            //    jsonData.CollisionPremium = change.CollisionPremium ?? jsonData.CollisionPremium;
            //    jsonData.FireLimitOfInsurance = change.FireLimitOfInsurance ?? jsonData.FireLimitOfInsurance;
            //    jsonData.FireDeductible = change.FireDeductible ?? jsonData.FireDeductible;
            //    jsonData.FirePremium = change.FirePremium ?? jsonData.FirePremium;
            //    jsonData.FireTheftLimitOfInsurance = change.FireTheftLimitOfInsurance;
            //    jsonData.FireTheftDeductible = change.FireTheftDeductible;
            //    jsonData.FireTheftPremium = change.FireTheftPremium;

            //    riskFormDTO.Other = JsonConvert.SerializeObject(jsonData);
            //}
            //else
            //{ 
            //    riskFormDTO.Other = null; 
            //}

            //await _formsService.SingleFormUpdateAsync(riskFormDTO);
        }
    }
}
