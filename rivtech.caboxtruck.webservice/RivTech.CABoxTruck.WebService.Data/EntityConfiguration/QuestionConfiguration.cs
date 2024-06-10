using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.Property(e => e.Description).HasColumnType("varchar(250)").IsRequired();
            builder.Property(e => e.EffectiveDate).IsRequired();
            builder.Property(e => e.ExpirationDate).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.LastUpdatedAt).IsRequired();
            builder.Property(e => e.CategoryId).HasColumnType("smallint").IsRequired();
            builder.Property(e => e.SectionId).HasColumnType("smallint").IsRequired();

            builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).IsRequired();
            builder.HasOne(x => x.Section).WithMany().HasForeignKey(x => x.SectionId).IsRequired();         
        }
    }
}
