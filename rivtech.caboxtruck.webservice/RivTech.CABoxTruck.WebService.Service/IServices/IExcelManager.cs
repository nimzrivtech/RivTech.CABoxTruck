using System.Collections.Generic;
using System.IO;
using RivTech.CABoxTruck.WebService.DTO;
using RivTech.CABoxTruck.WebService.Service.Services.ExcelManager;

namespace RivTech.CABoxTruck.WebService.Service.IServices
{
    public interface IExcelManager
    {
        List<PropertyHeader> HeaderMap { get; set; }
        List<VehicleDTO> AddVehicles(Stream fileStream);

        //can we cahnge VehiclePropertyHeader to a generic class so we can use it on both
        List<DriverDTO> AddDrivers(Stream fileStream);
    }
}