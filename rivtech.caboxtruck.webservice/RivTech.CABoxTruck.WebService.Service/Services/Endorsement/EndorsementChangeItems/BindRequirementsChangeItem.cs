using RivTech.CABoxTruck.WebService.DomainEntities.Endorsement.EndorsementChangeItems;
using RivTech.CABoxTruck.WebService.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Endorsement.EndorsementChangeItems
{
    public class BindRequirementsChangeItem : IEndorsementChangeItem
    {
        public string Type => nameof(BindRequirementsChangeItem);

        public string DisplayText => "";
        public Guid? Id { get; set; }

        public short? BindRequirementsOptionId { get; set; }
        public string Describe { get; set; }
        public bool? IsPreBind { get; set; }
        public short? BindStatusId { get; set; }
        public string Comments { get; set; }

        public string RelevantDocumentDesc { get; set; }
        public string FilePath { get; set; }

        public Guid? RiskDetailId { get; set; }
        public EnumerationDTO BindRequirementsOption { get; set; }
        public EnumerationDTO BindStatus { get; set; }

        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDefault { get; set; }
        public string AgentComments { get; set; }
        public DateTime? DeletedDate { get; set; }

        public List<FileUploadDocumentDTO> FileUploads { get; set; }
    }
}
