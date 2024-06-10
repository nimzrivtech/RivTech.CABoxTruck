using RivTech.CABoxTruck.WebService.DTO.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class BindingDTO
    {
        public Guid? Id { get; set; }

        public short? BindOptionId { get; set; }
        public short? PaymentTypesId { get; set; }
        public short? CreditedOfficeId { get; set; }
        public string SurplusLIneNum { get; set; }
        public string SLANum { get; set; }
        public string ProducerSLNumber { get; set; }
        public string SLState { get; set; }
        public decimal MinimumEarned { get; set; }

        public Guid? RiskId { get; set; }
        public EnumerationDTO BindOption { get; set; }
        public EnumerationDTO PaymentTypes { get; set; }
        public EnumerationDTO CreditedOffice { get; set; }

        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class BindingRequirementsDTO : AzureBlobSaveDTO
    {
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

    public class QuoteConditionsDTO : AzureBlobSaveDTO
    {
        public Guid? Id { get; set; }

        public short? QuoteConditionsOptionId { get; set; }
        public string Describe { get; set; }
        public string Comments { get; set; }

        public string RelevantDocumentDesc { get; set; }
        public string FilePath { get; set; }

        public Guid? RiskDetailId { get; set; }
        public EnumerationDTO QuoteConditionsOption { get; set; }

        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDefault { get; set; }

        public bool? IsActive { get; set; }
        public DateTime? DeletedDate { get; set; }

        public List<FileUploadDocumentDTO> FileUploads { get; set; }

        public string SortableText => QuoteConditionsOption?.Description;
    }

    public interface AzureBlobSaveDTO
    {
        Guid? Id { get; set; }
        Guid? RiskDetailId { get; set; }
        long? CreatedBy { get; set; }
        DateTime? CreatedDate { get; set; }
        DateTime? UpdatedDate { get; set; }

        bool? IsActive { get; set; }
        DateTime? DeletedDate { get; set; }
        List<FileUploadDocumentDTO> FileUploads { get; set; }
    }


}
