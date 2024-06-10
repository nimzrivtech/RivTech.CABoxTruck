using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.Service.Services.Common
{
    public class FileHelperUtility
    {
        /// <summary>
        /// https://stackoverflow.com/questions/146134/how-to-remove-illegal-characters-from-path-and-filenames
        /// </summary>
        /// <param name="illegal"></param>
        /// <returns></returns>
        public static string ScrubbedName(string illegal)
        {
            var regexSearch = new StringBuilder((new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars())));
            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch?.ToString())));
            illegal = r.Replace(illegal, "");
            return illegal;
        }

        public static string MakeFileNameUnique(string fileName)
        {
            return $"{Path.GetFileNameWithoutExtension(fileName)}_{Guid.NewGuid().ToString()?.Substring(0, 5)}{Path.GetExtension(fileName)}";
        }

        public static string DocumentFileNameFormatted(string policyNumber, string copyType, string documentType, DateTime dateTime)
        {
            DateTimeConverter dateTimeConverter = new DateTimeConverter();
            DateTime updatedDateTime;
            try
            {
                updatedDateTime = dateTimeConverter.ChangeTimeOfTriggerDate(dateTime).Result;
            }
            catch (Exception)
            {
                updatedDateTime = dateTime;
            }

            if (string.IsNullOrEmpty(copyType))
            {
                return $"{policyNumber}_{documentType}_{updatedDateTime.ToString("MMddyyHHmmssfff")}.pdf";
            }
            else
            {
                return $"{policyNumber}_{copyType}_{documentType}_{updatedDateTime.ToString("MMddyyHHmmssfff")}.pdf";
            }
        }

        public static string EncodeFilePath(string filePath)
        {
            return filePath.Replace(" ", "%20").Replace("#", "%23");
        }
    }

    public class DateTimeConverter
    {
        public DateTime SetDateTimeByLocalTime(DateTime? date, Double? offset = null)
        {
            try
            {
                double ESTOffset = -5;
                var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                if (offset == null || offset == -4)
                {
                    var tempResult = TimeZoneInfo.ConvertTimeFromUtc(date ?? DateTime.UtcNow, easternZone);

                    return tempResult;
                }

                var result = new DateTimeOffset(date ?? DateTime.UtcNow).ToOffset(TimeSpan.FromHours(offset ?? ESTOffset)).DateTime;

                return result;
            }
            catch
            {
                return date ?? DateTime.UtcNow;
            }
        }

        public async Task<DateTime> ChangeTimeOfTriggerDate(DateTime requestDate)
        {
            await Task.Delay(1);

            if (!string.IsNullOrEmpty(requestDate.ToString()))
            {
                var dateTime = Convert.ToDateTime(requestDate);
                var timeNow = DateTime.UtcNow;

                return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,
                    timeNow.Hour, timeNow.Minute, timeNow.Second, timeNow.Millisecond);
            }

            return DateTime.UtcNow;
        }
    }
}
