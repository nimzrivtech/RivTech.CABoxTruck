using Newtonsoft.Json.Linq;
using System.Linq;

namespace RivTech.CABoxTruck.WebService.Service.Services.Rater.Mapping
{
    public static class RaterInputMapperUtilExtensions
    {
        public static dynamic ParseListToValue(this object dbVal, JToken node)
        {
            if (dbVal != null)
            {
                var val = (node ?? "")
                        ?.Where(x => (string)x["value"] == dbVal.ToString())
                        ?.Select(c => (string)c["label"]).FirstOrDefault();
                return SanitizeStringVal(val);
            }
            else return null;
        }

        public static string SanitizeStringVal(this string str)
        {
            return str?.Replace('\u00A0', ' ').Replace("$", "");
        }

    }
}
