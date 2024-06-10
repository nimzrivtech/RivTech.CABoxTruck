using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Entity
{
    public class PolicyContacts : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public bool IsAssignedTo { get; set; }
    }
}
