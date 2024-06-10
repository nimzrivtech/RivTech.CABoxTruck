using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Net;

namespace RivTech.CABoxTruck.WebService.Errors
{
    public class ValidationErrorResponse : ErrorResponse
    {
        public ValidationErrorResponse() : base((int)HttpStatusCode.BadRequest)
        {
        }

        public IEnumerable<string> Errors { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}
