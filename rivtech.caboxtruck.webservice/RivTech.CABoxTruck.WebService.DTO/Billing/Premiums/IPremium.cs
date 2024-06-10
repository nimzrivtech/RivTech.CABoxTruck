namespace RivTech.CABoxTruck.WebService.DTO.Billing.Premiums
{
    public interface IPremium : IAmount
    {
        string Description { get; }
    }
}