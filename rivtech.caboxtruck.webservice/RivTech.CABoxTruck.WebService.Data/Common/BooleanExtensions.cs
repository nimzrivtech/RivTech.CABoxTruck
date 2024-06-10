using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Data.Common
{
    public static class BooleanExtensions
    {
        public static string ToYesNo(this bool obj)
        {
            return (obj == true) ? "Yes" : "No";
        }

        public static string ToYesNo(this bool? obj)
        {
            return obj.HasValue 
                ? ( obj.Value == true ? "Yes" : "No") 
                : "";
        }
    }
}
