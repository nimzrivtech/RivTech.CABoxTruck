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
    public class FormHiredPhysicalDamageEndorsementModifier : ChangeModifierBase<FormHiredPhysicalDamageEndorsementChangeItem>
    {
        private readonly IFormsService _formsService;
        private readonly IMapper _mapper;
        public FormHiredPhysicalDamageEndorsementModifier(IFormsService formsService, IMapper mapper)
        {
            _formsService = formsService;
            _mapper = mapper;
        }

        public override async Task ModifyAsync(IEndorsementData endorsementData, IEndorsementChangeItem changeItem)
        {
            //var data = (EndorsementData)endorsementData;
            //var change = CastOriginalType(changeItem);

            //var riskFormDTO = data.RiskDTO.RiskForms.FirstOrDefault(x => x.FormId == "HiredPhysicalDamage");

            //if (!string.IsNullOrWhiteSpace(riskFormDTO.Other))
            //{
            //    var jsonData = JsonConvert.DeserializeObject<FormHiredPhysicalDamageEndorsementChangeItem>(riskFormDTO.Other);

            //    jsonData.EstimatedAdditionalPremium = change.EstimatedAdditionalPremium ?? jsonData.EstimatedAdditionalPremium;
            //    jsonData.VehicleDays = change.VehicleDays ?? jsonData.VehicleDays;
            //    jsonData.RatePerDay = change.RatePerDay ?? jsonData.RatePerDay;
            //    jsonData.GrossCombinedWeight = change.GrossCombinedWeight ?? jsonData.GrossCombinedWeight;
            //    jsonData.AccidentLimitPerVehicle = change.AccidentLimitPerVehicle ?? jsonData.AccidentLimitPerVehicle;
            //    jsonData.Deductible = change.Deductible ?? jsonData.Deductible;
            //    jsonData.PhysdamLimit = change.PhysdamLimit ?? jsonData.PhysdamLimit;

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
