using System;

namespace RivTech.CABoxTruck.WebService.DTO.Billing
{
    public static class RoundingExtensions
    {
        public static decimal RoundTo2Decimals(this decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }
    }
}
