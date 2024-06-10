namespace RivTech.CABoxTruck.WebService.DTO
{
    public class AddressTypeDTO : EnumerationDTO
    {
        public static AddressTypeDTO Billing => new AddressTypeDTO {Id = 1, Description = "Billing"};
        public static AddressTypeDTO Business => new AddressTypeDTO { Id = 2, Description = "Business" };
    }
}
