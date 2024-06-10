using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RivTech.CABoxTruck.WebService.Data.Entity;

namespace RivTech.CABoxTruck.WebService.Data.EntityConfiguration
{
    public class QuestionValidationConfiguration : IEntityTypeConfiguration<QuestionValidation>
    {
        public void Configure(EntityTypeBuilder<QuestionValidation> builder)
        {
            builder.HasKey(qv => new { qv.QuestionId, qv.ValidationId });
        }
    }
}
