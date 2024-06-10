using RivTech.CABoxTruck.WebService.DTO.DocumentGeneration;
using System;

namespace RivTech.CABoxTruck.WebService.Service.IServices.DocumentGeneration
{
    public interface IPage
    {
        public string TemplateName { get; }
        public int PageOrder { get; }
        public Form GetData();
    }
}