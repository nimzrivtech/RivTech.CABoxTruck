using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class RiskFormDTO
    {
        public Guid? Id { get; set; }
        public Guid RiskDetailId { get; set; }
        public string FormId { get; set; }
        public bool? IsSelected { get; set; }
        public string Other { get; set; }

        public FormDTO Form { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsUserUploaded { get; set; }
    }

    public class FormDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string FormNumber { get; set; }
        public bool? IsMandatory { get; set; }
        public bool? IsDefault { get; set; }
        public string FileName { get; set; }
        public bool IsSupplementalDocument { get; set; }
        public DateTime? CreatedDate { get; set; }
        public IEnumerable<FormVersionDTO> FormVersions { get; set; }
        public FormVersionDTO LatestFormVersion(DateTime? quoteDate, bool isRaterVersionLocked) 
        {
            var formVersion =FormVersions?.Where(fv => quoteDate >= fv.EffectiveDate)?
                   .OrderBy(fv => fv.EffectiveDate)?
                   .FirstOrDefault() ??
                   FormVersions?
                   .OrderByDescending(fv => fv.EffectiveDate)?
                   .FirstOrDefault();

            if (formVersion != null && (quoteDate != null || !isRaterVersionLocked))
                FormNumber = formVersion?.FormNumber;

            return formVersion;
        }
    }

    public class FormVersionDTO
    {
        public string Id { get; set; }
        public string FormId { get; set; }
        public int Version { get; set; }
        public string FormNumber { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}