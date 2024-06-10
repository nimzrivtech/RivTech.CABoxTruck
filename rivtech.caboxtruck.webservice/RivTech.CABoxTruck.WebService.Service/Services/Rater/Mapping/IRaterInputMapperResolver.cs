namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping
{
    public interface IRaterInputMapperResolver
    {
        IRaterInputMapper Resolve(string name);
    }
}