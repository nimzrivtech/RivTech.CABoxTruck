using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSLNumberSLBrokerAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProducerName",
                table: "SLNumber",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SLBroker",
                table: "SLNumber",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SLBrokerAddressCityStateZip",
                table: "SLNumber",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SLBrokerAddressLineOne",
                table: "SLNumber",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProducerName",
                table: "SLNumber");

            migrationBuilder.DropColumn(
                name: "SLBroker",
                table: "SLNumber");

            migrationBuilder.DropColumn(
                name: "SLBrokerAddressCityStateZip",
                table: "SLNumber");

            migrationBuilder.DropColumn(
                name: "SLBrokerAddressLineOne",
                table: "SLNumber");
        }
    }
}
