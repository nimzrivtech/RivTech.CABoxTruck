using Newtonsoft.Json;
using RivTech.CABoxTruck.WebService.DTO.Forms;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class FormFMotorCarrierBIPDPage : CommonValuesPage
    {
        public FormFMotorCarrierBIPDPage(ReportData reportData) : base(reportData)
        {
            TemplateName = "Form F - Uniform Motor Carrier BIPD Insurance Endorsement.docx";
            PageOrder = 47;
        }

        protected override void SetFormValues()
        {
            base.SetFormValues();
            if(PolicyState == "AL") {FormValues.Add("AL", "X");}
            if(PolicyState == "AK") {FormValues.Add("AK", "X");}
            if(PolicyState == "AZ") {FormValues.Add("AZ", "X");}
            if(PolicyState == "AR") {FormValues.Add("AR", "X");}
            if(PolicyState == "CA") {FormValues.Add("CA", "X");}
            if(PolicyState == "CO") {FormValues.Add("CO", "X");}
            if(PolicyState == "CT") {FormValues.Add("CT", "X");}
            if(PolicyState == "DE") {FormValues.Add("DE", "X");}
            if(PolicyState == "DC") {FormValues.Add("DC", "X");}
            if(PolicyState == "FL") {FormValues.Add("FL", "X");}
            if(PolicyState == "GA") {FormValues.Add("GA", "X");}
            if(PolicyState == "HI") {FormValues.Add("HI", "X");}
            if(PolicyState == "ID") {FormValues.Add("ID", "X");}
            if(PolicyState == "IL") {FormValues.Add("IL", "X");}
            if(PolicyState == "IN") {FormValues.Add("IN", "X");}
            if(PolicyState == "IA") {FormValues.Add("IA", "X");}
            if(PolicyState == "KS") {FormValues.Add("KS", "X");}
            if(PolicyState == "KY") {FormValues.Add("KY", "X");}
            if(PolicyState == "LA") {FormValues.Add("LA", "X");}
            if(PolicyState == "ME") {FormValues.Add("ME", "X");}
            if(PolicyState == "MD") {FormValues.Add("MD", "X");}
            if(PolicyState == "MA") {FormValues.Add("MA", "X");}
            if(PolicyState == "MI") {FormValues.Add("MI", "X");}
            if(PolicyState == "MN") {FormValues.Add("MN", "X");}
            if(PolicyState == "MS") {FormValues.Add("MS", "X");}
            if(PolicyState == "MO") {FormValues.Add("MO", "X");}
            if(PolicyState == "MT") {FormValues.Add("MT", "X");}
            if(PolicyState == "NE") {FormValues.Add("NE", "X");}
            if(PolicyState == "NV") {FormValues.Add("NV", "X");}
            if(PolicyState == "NH") {FormValues.Add("NH", "X");}
            if(PolicyState == "NJ") {FormValues.Add("NJ", "X");}
            if(PolicyState == "NM") {FormValues.Add("NM", "X");}
            if(PolicyState == "NY") {FormValues.Add("NY", "X");}
            if(PolicyState == "NC") {FormValues.Add("NC", "X");}
            if(PolicyState == "ND") {FormValues.Add("ND", "X");}
            if(PolicyState == "OH") {FormValues.Add("OH", "X");}
            if(PolicyState == "OK") {FormValues.Add("OK", "X");}
            if(PolicyState == "OR") {FormValues.Add("OR", "X");}
            if(PolicyState == "PA") {FormValues.Add("PA", "X");}
            if(PolicyState == "RI") {FormValues.Add("RI", "X");}
            if(PolicyState == "SC") {FormValues.Add("SC", "X");}
            if(PolicyState == "SD") {FormValues.Add("SD", "X");}
            if(PolicyState == "TN") {FormValues.Add("TN", "X");}
            if(PolicyState == "TX") {FormValues.Add("TX", "X");}
            if(PolicyState == "UT") {FormValues.Add("UT", "X");}
            if(PolicyState == "VT") {FormValues.Add("VT", "X");}
            if(PolicyState == "VA") {FormValues.Add("VA", "X");}
            if(PolicyState == "WA") {FormValues.Add("WA", "X");}
            if(PolicyState == "WV") {FormValues.Add("WV", "X");}
            if(PolicyState == "WI") {FormValues.Add("WI", "X");}
            if(PolicyState == "WY") {FormValues.Add("WY", "X");}
        }
    }
}