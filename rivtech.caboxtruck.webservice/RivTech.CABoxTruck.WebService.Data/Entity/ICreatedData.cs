using System;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public interface ICreatedData
    {
        DateTime CreatedDate { get; set; }
        long CreatedBy { get; set; }
    }
}
