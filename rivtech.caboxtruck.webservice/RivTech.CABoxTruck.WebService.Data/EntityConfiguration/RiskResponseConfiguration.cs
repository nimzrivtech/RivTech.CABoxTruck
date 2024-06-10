using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RiskResponseConfiguration : IEntityTypeConfiguration<RiskResponse>
    {
        public void Configure(EntityTypeBuilder<RiskResponse> builder)
        {
            builder.Property(e => e.RiskDetailId).HasColumnType("uniqueidentifier").IsRequired();
            builder.Property(e => e.QuestionId).HasColumnType("smallint").IsRequired();
            builder.Property(e => e.ResponseValue).HasColumnType("varchar(1000)");
            builder.HasOne(x => x.Question).WithMany().HasForeignKey(x => x.QuestionId).IsRequired();
        }
    }
}
