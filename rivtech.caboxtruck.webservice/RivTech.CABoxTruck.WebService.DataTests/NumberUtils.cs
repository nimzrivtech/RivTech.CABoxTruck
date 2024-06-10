using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DataTests
{
    public static class NumberUtils
    {
        public static decimal To2Decimals(this decimal num)
        {
            return Math.Round(num, 2, MidpointRounding.AwayFromZero);
        }
    }
}
