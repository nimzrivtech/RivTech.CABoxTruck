using System;
using RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration;
using RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration.DTO;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration
{
    public abstract class PageFactory<T> where  T: IPage
    {
        public static T GetInstance(ReportData reportData)
        {
            return (T) Activator.CreateInstance(typeof(T), reportData);
        }
    }
}
