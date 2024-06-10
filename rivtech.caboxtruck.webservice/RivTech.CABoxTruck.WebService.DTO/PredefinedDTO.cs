using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class ZipCodeDTO
    {
        public Int16 RecID { get; set; }
        public string ZipCodeID { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Int16 StateFullName { get; set; }
        public Int16 IsActive { get; set; }
    }

    public class GenderDTO
    {
        public Int16 GenderID { get; set; }
        public string Description { get; set; }
        public Int16 IsActive { get; set; }
    }

    public class MaritalStatusDTO
    {
        public Int16 MaritalStatusID { get; set; }
        public string Description { get; set; }
        public Int16 IsActive { get; set; }
    }

    public class DamageToPremisesLimitDTO
    {
        public Int16 DamageToPremisesLimitID { get; set; }
        public string Description { get; set; }
        public Int16 IsActive { get; set; }
    }

    public class MedicalExpenseLimitDTO
    {
        public Int16 MedicalExpenseLimitID { get; set; }
        public string Description { get; set; }
        public Int16 IsActive { get; set; }
    }

    public class CoverageServiceTypeDTO
    {
        public Int32 CoverageServiceTypeID { get; set; }
        public string Description { get; set; }
        public Int16 ServiceClass { get; set; }
        public string CoverageType { get; set; }
        public Int16 IsActive { get; set; }
    }

    public class EnvILFDTO
    {
        public Int32 EnvILFID { get; set; }
        public string Limit { get; set; }
        public decimal ILF { get; set; }
        public decimal MP { get; set; }
        public Int16 IsActive { get; set; }
    }
    public class CPLDocumentTypeDTO
    {
        public Int16 CPLDocumentTypeID { get; set; }
        public string Description { get; set; }
        public Int16 IsActive { get; set; }
    }

    public class EILTermLengthDTO
    {
        public Int16 EILTermLengthID { get; set; }
        public string Description { get; set; }
        public Int16 IsActive { get; set; }
    }

    public class EILClassTypeDTO
    {
        public Int16 EILClassTypeID { get; set; }
        public string Description { get; set; }
        public Int16 IsActive { get; set; }
    }

    public class TPLRatingMethodDTO
    {
        public Int16 TPLRatingMethodID { get; set; }
        public string Description { get; set; }
        public Int16 IsActive { get; set; }
    }

    public class TPLHazardClassDTO
    {
        public Int16 TPLHazardClassID { get; set; }
        public string Description { get; set; }
        public Int16 IsActive { get; set; }
    }

    public class DebitCreditTypeDTO
    {
        public Int16 DebitCreditTypeID { get; set; }
        public string Description { get; set; }
        public Int16 IsActive { get; set; }

    }

   
}
