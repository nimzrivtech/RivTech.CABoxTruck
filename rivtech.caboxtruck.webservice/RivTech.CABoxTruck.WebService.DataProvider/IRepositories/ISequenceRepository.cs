using RivTech.CABoxTruck.WebService.DTO.Enum;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface ISequenceRepository
    {
        int GetNextSequence(SequenceType sequence);
        string GetNextPolicyNumber();
    }
}
