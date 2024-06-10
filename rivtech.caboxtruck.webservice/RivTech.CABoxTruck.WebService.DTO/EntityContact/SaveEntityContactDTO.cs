using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DTO
{
    public class SaveEntityContactDTO
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public Int16 ContactTypeId { get; set; }
        public Int16? CompanyPositionId { get; set; }
        public Int16? YearsInPosition { get; set; }
        public string PhoneExtension { get; set; }
        public string EmailAddress { get; set; }
    }
}
