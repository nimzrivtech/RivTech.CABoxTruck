using DocumentFormat.OpenXml.Bibliography;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace RivTech.CABoxTruck.WebService.Service.Services.Common
{
    public static class SystemExtension
    {
        public static T Clone<T>(this T source)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (StreamWriter writer = new StreamWriter(memoryStream))
            using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.PreserveReferencesHandling = PreserveReferencesHandling.None;
                serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                serializer.MaxDepth = 256;
                serializer.Serialize(jsonWriter, source);
                jsonWriter.Flush();

                memoryStream.Seek(0, SeekOrigin.Begin);

                using (StreamReader reader = new StreamReader(memoryStream))
                using (JsonTextReader jsonReader = new JsonTextReader(reader))
                {
                    JsonSerializer ser = new JsonSerializer();
                    ser.PreserveReferencesHandling = PreserveReferencesHandling.None;
                    ser.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    ser.MaxDepth = 256;
                    var result = ser.Deserialize<T>(jsonReader);
                    return result;
                }
            }
        }
    }
}
