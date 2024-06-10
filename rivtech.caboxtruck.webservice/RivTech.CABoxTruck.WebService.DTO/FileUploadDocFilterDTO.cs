using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class FileUploadDocFilterDTO
    {
        public Guid? RiskDetailId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortField { get; set; }
        public string Order { get; set; }
        public bool isPolicy { get; set; }
        public bool isBilling { get; set; }
        public string Keyword { get; set; }
    }
}
