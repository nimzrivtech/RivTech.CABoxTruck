using Microsoft.EntityFrameworkCore;
using RivTech.CABoxTruck.WebService.Data.Context;
using RivTech.CABoxTruck.WebService.Data.Entity.Billing;
using RivTech.CABoxTruck.WebService.DataProvider.IRepositories.Billing;
using RivTech.CABoxTruck.WebService.DTO.Billing.Taxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RivTech.CABoxTruck.WebService.DataProvider.Repositories.Billing
{
    public class StateTaxRepository : IStateTaxRepository
    {
        private readonly AppDbContext _dbContext;

        public StateTaxRepository(
            AppDbContext dbContext
        ) {
            _dbContext = dbContext;
        }

        public StateTax Get(string stateCode, DateTime effectiveDate)
        {
            //var dataSource = new List<StateTaxData>()
            //{
            //    new StateTaxData { State = "CA", TaxType = TaxType.SurplusLine.Id, TaxSubtype = SLTaxKind.Main.Id, Rate = 3m  },
            //    new StateTaxData { State = "CA", TaxType = TaxType.StampingFee.Id, TaxSubtype = StampingFeeTaxType.Percentage.Id, Rate = 3m  },

            //    new StateTaxData { State = "OR", TaxType = TaxType.SurplusLine.Id, TaxSubtype = SLTaxKind.Main.Id, Rate = 2m  },
            //    new StateTaxData { State = "OR", TaxType = TaxType.SurplusLine.Id, TaxSubtype = SLTaxKind.FireMarshal.Id, Rate = 0.3m  },
            //    new StateTaxData { State = "OR", TaxType = TaxType.StampingFee.Id, TaxSubtype = StampingFeeTaxType.Flat.Id, Rate = 10m  },
            //};

            List<StateTaxData> stateTaxData = _dbContext.StateTax
                .Where(x => x.StateCode == stateCode)
                .AsEnumerable()
                .GroupBy(x => new { x.StateCode, x.TaxType, x.TaxSubtype })
                .Select(g =>
                    // Get latest item from each group of tax types.
                    g.OrderByDescending(x => x.EffectiveDate)
                        .FirstOrDefault(x => effectiveDate.Date >= x.EffectiveDate.Date)
                ).Where(x => x != null) // Exclude group with null.
                ?.ToList() ?? new List<StateTaxData>();

            var slTaxes = new List<SurplusLineTax>();
            StampingFeeTax stampingFeeTax = null;
            string stampingFeeTaxType = null;
            foreach (var stateTaxItem in stateTaxData)
            {
                if (TaxType.SurplusLine.Id.Equals(stateTaxItem.TaxType))
                {
                    slTaxes.Add(new SurplusLineTax(SLTaxKind.FromId(stateTaxItem.TaxSubtype), stateTaxItem.Rate));
                }
                else if (TaxType.StampingFee.Id.Equals(stateTaxItem.TaxType))
                {
                    stampingFeeTax = StampingFeeTax.FromTypeId(stateTaxItem.TaxSubtype, stateTaxItem.Rate);
                    stampingFeeTaxType = stateTaxItem.TaxSubtype;
                }
            }

            return new StateTax(stateCode, slTaxes, stampingFeeTax, stampingFeeTaxType);
        }


        public async Task<StateTax> GetAsync(string stateCode, DateTime effectiveDate)
        {
            IEnumerable<StateTaxData> stateTax = await _dbContext.StateTax
                .Where(x => x.StateCode == stateCode)
                .ToListAsync();

            IEnumerable<StateTaxData> latestEffectiveStateTaxes = stateTax
                .GroupBy(x => new { x.StateCode, x.TaxType, x.TaxSubtype })
                .Select(g =>
                    // Get latest item from each group of tax types.
                    g.OrderByDescending(x => x.EffectiveDate)
                        .FirstOrDefault(x => effectiveDate.Date >= x.EffectiveDate.Date)
                ).Where(x => x != null) // Exclude group with null.
                 ?? new List<StateTaxData>();

            latestEffectiveStateTaxes = latestEffectiveStateTaxes.Where(x => x != null);

            var slTaxes = new List<SurplusLineTax>();
            StampingFeeTax stampingFeeTax = null;
            string stampingFeeTaxType = null;
            foreach (var stateTaxItem in latestEffectiveStateTaxes)
            {
                if (TaxType.SurplusLine.Id.Equals(stateTaxItem.TaxType))
                {
                    slTaxes.Add(new SurplusLineTax(SLTaxKind.FromId(stateTaxItem.TaxSubtype), stateTaxItem.Rate));
                }
                else if (TaxType.StampingFee.Id.Equals(stateTaxItem.TaxType))
                {
                    stampingFeeTax = StampingFeeTax.FromTypeId(stateTaxItem.TaxSubtype, stateTaxItem.Rate);
                    stampingFeeTaxType = stateTaxItem.TaxSubtype;
                }
            }

            return new StateTax(stateCode, slTaxes, stampingFeeTax, stampingFeeTaxType);
        }
    }
}
