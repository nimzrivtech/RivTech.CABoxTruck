using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class RiskNotesConfiguration : IEntityTypeConfiguration<RiskNotes>
    {
        public void Configure(EntityTypeBuilder<RiskNotes> builder)
        {
            builder.Property(e => e.Description).HasColumnType("varchar(MAX)");

            builder.HasOne(x => x.RiskDetail).WithMany().HasForeignKey(x => x.RiskDetailId);
            builder.HasOne(x => x.NoteCategoryOption).WithMany().HasForeignKey(x => x.CategoryId);
        }
    }
}
