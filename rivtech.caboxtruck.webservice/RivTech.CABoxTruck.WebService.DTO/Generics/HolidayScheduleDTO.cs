using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DTO.Generics
{
    public class HolidayScheduleDTO
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string HolidayName { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
    }
}
