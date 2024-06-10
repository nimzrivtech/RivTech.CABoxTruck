namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public class PolicyTotals
    {
        public PolicyTotals(decimal premium, decimal fee, decimal tax)
        {
            Premium = premium.RoundTo2Decimals();
            Fee = fee.RoundTo2Decimals();
            Tax = tax.RoundTo2Decimals();
        }

        public decimal Premium { get; }
        public decimal Fee { get; }
        public decimal Tax { get; }
        public decimal Total => Premium + Fee + Tax;
    }
}
