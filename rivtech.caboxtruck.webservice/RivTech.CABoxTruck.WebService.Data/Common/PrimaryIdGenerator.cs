using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace RivTech.CABoxTruck.WebService.Data.Common
{
    public class PrimaryIdGenerator : ValueGenerator<Guid>
    {
        public override Guid Next(EntityEntry entry)
        {
            return Guid.NewGuid();
        }

        public override bool GeneratesTemporaryValues => false;
    }
}
