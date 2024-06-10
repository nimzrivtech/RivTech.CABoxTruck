using RivTech.CABoxTruck.WebService.Data.Entity;
using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class FileUploadPagingDTO
    {
        public PaginationDetails PaginationDetails { get; set; }
        public List<FileUploadDocument> FileUploadDocuments { get; set; }
    }
}
