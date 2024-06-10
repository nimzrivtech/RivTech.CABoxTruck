using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuoteConditionsOptionInsuranceLimitDescReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_INSERT);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_DELETE);
        }

        const string SQL_INSERT = @"
            INSERT INTO QuoteConditionsOption VALUES (22,'Please review carefully. This quote does not necessarily provide the terms and/or coverages originally requested. No coverage is provided.', 1)
            UPDATE QuoteConditionsOption SET Description = 'Policy includes a Physical Damage insurance limit of insurance. We will limit the maximum loss for any one auto to $300,000 and any one loss, catastrophe or terminal loss to $1,000,000.' WHERE Id = 13
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM QuoteConditionsOption WHERE Id = 22
            UPDATE QuoteConditionsOption SET Description = 'Policy includes a Physical Damage Insurance Limit of Insurance.' WHERE Id = 13
            GO
        ";
    }
}
