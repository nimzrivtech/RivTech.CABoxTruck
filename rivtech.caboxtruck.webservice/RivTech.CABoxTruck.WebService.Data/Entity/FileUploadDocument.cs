using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class FileUploadDocument : BaseEntity<Guid>, ICreatedData, IDeletedDate, IUpdatedData
    {
        public FileUploadDocument()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now.ToLocalTime();
        }
       
        public Guid? tableRefId { get; set; }
        public Guid? RiskDetailId { get; set; }
        public short? FileCategoryId { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
        public string MimeType { get; set; }
        public bool? IsConfidential { get; set; }
        public bool? IsUploaded { get; set; }
        public bool? IsSystemGenerated { get; set; }
        public bool? IsUserUploaded { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public bool? IsActive { get; set; }

        public long? RemovedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public FileCategory FileCategory { get; set; }
        public RiskDetail RiskDetail { get; set; }
    }
}
