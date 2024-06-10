namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class BillingSummaryDetails
    {
        public string Description { get; set; }
        public decimal Written { get; set; }
        public decimal Billed { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get; set; }
    }
}