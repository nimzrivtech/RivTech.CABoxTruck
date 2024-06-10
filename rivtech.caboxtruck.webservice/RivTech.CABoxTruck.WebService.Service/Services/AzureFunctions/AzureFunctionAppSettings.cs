namespace RivTech.CABoxTruck.WebService.Service.Services.AzureFunctions
{
    public class AzureFunctionAppSettings
    {
        public const string APP_SETTINGS_SECTION_KEY = "AzureFunction";
        public string BaseUrl { get; set; }
        public string ContentGeneratorFunctionApiCode { get; set; }
    }
}
