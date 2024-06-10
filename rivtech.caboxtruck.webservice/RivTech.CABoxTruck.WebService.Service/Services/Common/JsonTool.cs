using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.Service.Services.Common
{
    public class JsonTool
    {
        private static JsonSerializerSettings JsonUTCSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-ddTH:mm:ss.fffK",
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };
            }
        }

        public static TDTO DeserializeObject<TDTO>(string dto) where TDTO : class
        {
            try
            {
                return JsonConvert.DeserializeObject<TDTO>(dto, JsonUTCSettings);
            }
            catch (Exception)
            {
                return JsonConvert.DeserializeObject<TDTO>(dto);
            }
        }
    }
}
