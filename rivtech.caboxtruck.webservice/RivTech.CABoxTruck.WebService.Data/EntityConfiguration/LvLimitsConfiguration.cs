using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class LvLimitsConfiguration : IEntityTypeConfiguration<LvLimits>
    {
        public void Configure(EntityTypeBuilder<LvLimits> builder)
        {
            builder.Property(e => e.LimitDisplay).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.LimitValue).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.SingleLimit).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.PerPerson).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.PerAccident).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.StateCode).HasColumnType("varchar(10)").HasMaxLength(10);
            builder.Property(e => e.LimitType).HasColumnType("varchar(10)").HasMaxLength(10);
            builder.Property(e => e.Deductible).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.Description).HasColumnType("varchar(100)").HasMaxLength(100);
        }
    }
}
