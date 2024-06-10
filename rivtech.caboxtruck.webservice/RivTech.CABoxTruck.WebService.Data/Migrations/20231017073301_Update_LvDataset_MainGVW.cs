using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLvDatasetMainGVW : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_UPDATE);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_UPDATE);
        }

        const string SQL_UPDATE = @"
            Update [LvDataset]
            SET Description='Extra Heavy Trucks',DescriptionInUI='Extra Heavy Trucks',OrderInUI='2'
            Where ID=8

            Update [LvDataset]
            SET Description='Extra Heavy Truck Tractors',DescriptionInUI='Extra Heavy Truck Tractors',OrderInUI='1'
            Where ID=10
        ";
    }
}
