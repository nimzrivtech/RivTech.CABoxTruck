using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.DTO.Generics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.IRepositories
{
    public interface IGenericsRepository
    {
        Task<IEnumerable<HolidayScheduleDTO>> GetHolidaySchedule();
    }
}
