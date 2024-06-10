using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class Banner : BaseEntity<Guid>, ICreatedData
    {
        //public Banner()
        //{
        //    Id = Guid.NewGuid();
        //}

        public string Description { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string PageLink { get; set; }
        public int DisplayOrder { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public long RemovedBy { get; set; }
        public DateTime? RemovedDate { get; set; }
    }
}
