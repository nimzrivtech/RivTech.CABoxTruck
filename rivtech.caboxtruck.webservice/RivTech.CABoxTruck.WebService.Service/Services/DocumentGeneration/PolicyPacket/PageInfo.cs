namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.PolicyPacket
{
    public class PageInfo
    {
        public string Id { get; set; }

        public string FormId { get; set; }

        public string TemplateName { get; set; }

        public int PageOrder { get; set; }
    }

    public class StateInfo
    {
        public string StateCode { get; set; }

        public string FullStateName { get; set; }
    }
}