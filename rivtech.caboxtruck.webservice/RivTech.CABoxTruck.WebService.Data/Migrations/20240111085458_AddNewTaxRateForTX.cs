using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTaxRateForTX : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_INSERT);
        }
        
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_DELETE);
        }

        const string SQL_INSERT = @"
            INSERT INTO StateTax(StateCode, TaxType, TaxSubtype, EffectiveDate, Rate) VALUES('TX', 'SF', 'P', '2024-01-01', 0.040)
            GO
        ";
        const string SQL_DELETE = @"
            DELETE FROM StateTax WHERE StateCode = 'TX' AND TaxType = 'SF' AND TaxSubtype = 'P' AND EffectiveDate = '2024-01-01 00:00:00.0000000'
            GO
        ";
    }
    
}
