using RivTech.CABoxTruck.WebService.Data.Entity.DataSets;
using System;
using System.Collections.Generic;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class Binding : BaseEntity<Guid>, ICreatedData, IUpdatedData
    {
        public Binding()
        {
            Id = Guid.NewGuid();
        }

        public short? BindOptionId { get; set; }
        public short? PaymentTypesId { get; set; }

        public short? CreditedOfficeId { get; set; }

        public string SurplusLIneNum { get; set; }

        public string SLANum { get; set; }

        public string ProducerSLNumber { get; set; }

        public string SLState { get; set; }

        public decimal MinimumEarned { get; set; }

        //public Guid RiskDetailId { get; set; }
        public Guid RiskId { get; set; }
        public BindOption BindOption { get; set; }
        public PaymentTypes PaymentTypes { get; set; }
        public CreditedOffice CreditedOffice { get; set; }

       // public RiskDetail RiskDetail { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }


    public class BindingRequirements : BaseEntity<Guid>, ICreatedData, IUpdatedData
    {
        public BindingRequirements()
        {
            Id = Guid.NewGuid();
        }


        public short? BindRequirementsOptionId { get; set; }
        public string Describe { get; set; }
        public bool? IsPreBind { get; set; }
        public short? BindStatusId { get; set; }
        public string Comments { get; set; }

        public string RelevantDocumentDesc { get; set; }

        public Guid RiskDetailId { get; set; }

        public BindRequirementsOption BindRequirementsOption { get; set; }
        public BindStatus BindStatus { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public bool? IsDefault { get; set; }
        public string AgentComments { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DeletedDate { get; set; }
    }


    public class QuoteConditions : BaseEntity<Guid>, ICreatedData, IUpdatedData
    {
        public QuoteConditions()
        {
            Id = Guid.NewGuid();
        }

        public short? QuoteConditionsOptionId { get; set; }
        public string Describe { get; set; }
        public string Comments { get; set; }

        public string RelevantDocumentDesc { get; set; }

        public Guid RiskDetailId { get; set; }

        public QuoteConditionsOption QuoteConditionsOption { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }


        public bool? IsActive { get; set; }
        public bool? IsDefault { get; set; }
        public DateTime? DeletedDate { get; set; }

    }
}
