using System;
using System.Collections.Generic;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.DTO.MappingProfileHelpers
{
    public static class PropertyConverters
    {
        public static DateTime? DateStringToDateTime(string dateString)
        {
            DateTime createdDate;
            if (DateTime.TryParse(dateString, out createdDate))
            {
                return createdDate;
            }

            return null;
        }

        public static string NullValueToEmptyString(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            return string.Empty;
        }

        public static string StringJoin(List<string> values)
        {
            return string.Join(Environment.NewLine, values.Select(s => NullValueToEmptyString(s)));
        }
    }
}