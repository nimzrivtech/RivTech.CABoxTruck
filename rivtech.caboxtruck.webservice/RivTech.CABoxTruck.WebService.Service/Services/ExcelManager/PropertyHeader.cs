using System;

namespace RivTech.CABoxTruck.WebService.Service.Services.ExcelManager
{
    public class PropertyHeader
    {
        public string PropertyName { get; set; }
        public Type PropertyType { get; set; }
        public string Header { get; set; }
        public int ColumnIndex { get; set; } = -1;
    }
}