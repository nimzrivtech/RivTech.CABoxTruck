using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLvDatasetTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LvDataset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    DescriptionInUI = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    OrderInUI = table.Column<int>(type: "int", nullable: false),
                    SupportedInRaterVersion = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DatasetType = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvDataset", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LvDataset");
        }
    }
}
