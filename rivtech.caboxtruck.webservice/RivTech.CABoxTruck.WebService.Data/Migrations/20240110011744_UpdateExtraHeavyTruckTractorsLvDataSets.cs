using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExtraHeavyTruckTractorsLvDataSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_BuildingWreckingOperators);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_ExtraHeavyTruckTractors);
        }

        const string UPDATE_BuildingWreckingOperators = @"
            UPDATE LvDataset
            SET [Description] = 'Building Wrecking Operators', DescriptionInUI = 'Building Wrecking Operators'
            WHERE Id = 124
        ";

        const string UPDATE_ExtraHeavyTruckTractors = @"
            UPDATE LvDataset
            SET [Description] = 'Extra Heavy Truck Tractors', DescriptionInUI = 'Extra Heavy Truck Tractors'
            WHERE Id = 124
        ";
    }
}
