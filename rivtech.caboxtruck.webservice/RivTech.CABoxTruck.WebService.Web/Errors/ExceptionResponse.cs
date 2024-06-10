using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RivTech.CABoxTruck.WebService.Errors
{
    public class ExceptionResponse : ErrorResponse
    {
        public ExceptionResponse(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }

        public string Details { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}
