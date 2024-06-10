using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;

namespace RivTech.CABoxTruck.WebService.Errors
{
    public class ErrorResponse
    {
        public int ReferenceNumber { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        public ErrorResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            ReferenceNumber = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                (int)HttpStatusCode.BadRequest => "Bad request",
                (int)HttpStatusCode.Unauthorized => "Authorization failed",
                (int)HttpStatusCode.NotFound => "Resource not found",
                (int)HttpStatusCode.InternalServerError => "Internal server error",
                _ => null
            };
        }
    }
}
