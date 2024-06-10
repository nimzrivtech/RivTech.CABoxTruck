using System.Globalization;

namespace RivTech.CABoxTruck.WebService.Service.Services.DocumentGeneration
{
    public static class CurrencyExtension
    {
        public static string ToCurrency(this decimal value, int decimalPlaces = 2)
        {
            var format = $"{{0:C{decimalPlaces}}}";
            var culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            return string.Format(culture, format, value);
        }

        public static string ToCurrency(this decimal? value, int decimalPlaces = 2)
        {
            var decimalValue = value ?? 0;
            return decimalValue.ToCurrency(decimalPlaces);
        }
    }
}