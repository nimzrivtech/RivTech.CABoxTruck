using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class FTPDocumentTemporary : BaseEntity<Guid>, ICreatedData, IDeletedDate, IUpdatedData
    {
        public Guid? RiskId { get; set; }
        public Guid? RiskDetailId { get; set; }
        public string BatchId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool IsUploaded { get; set; }
        public string Source { get; set; }
        public bool IsCompiled { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
