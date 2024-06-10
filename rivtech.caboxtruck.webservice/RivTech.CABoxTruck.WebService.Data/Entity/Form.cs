using RivTech.CABoxTruck.WebService.Data.Entity.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class Form : BaseEntity<string>, ICreatedData
    {
        public Form()
        {
            CreatedDate = DateTime.Now;
        }

        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public string FormNumber { get; set; }
        public bool? IsMandatory { get; set; }
        public bool? IsDefault { get; set; }
        public string FileName { get; set; }
        public int? DecPageOrder { get; set; }
        public bool IsSupplementalDocument { get; set; }

        public IEnumerable<FormVersion> FormVersions { get; set; }
        public FormVersion LatestFormVersion => FormVersions?.OrderByDescending(x => x.Version)?.FirstOrDefault();

        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}