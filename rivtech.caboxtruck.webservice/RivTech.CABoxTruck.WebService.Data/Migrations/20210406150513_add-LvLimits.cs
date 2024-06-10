using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class addLvLimits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LvAutoBILimits");

            migrationBuilder.DropTable(
                name: "LvAutoPDLimits");

            migrationBuilder.DropTable(
                name: "LvCargoLimits");

            migrationBuilder.DropTable(
                name: "LvCollisionLimits");

            migrationBuilder.DropTable(
                name: "LvComprehensiveLimits");

            migrationBuilder.DropTable(
                name: "LvGlLimits");

            migrationBuilder.DropTable(
                name: "LvLiabilityDeductibleLimits");

            migrationBuilder.DropTable(
                name: "LvMedPayLimits");

            migrationBuilder.DropTable(
                name: "LvPipLimits");

            migrationBuilder.DropTable(
                name: "LvRefLimits");

            migrationBuilder.DropTable(
                name: "LvUIMBILimits");

            migrationBuilder.DropTable(
                name: "LvUIMPDLimits");

            migrationBuilder.DropTable(
                name: "LvUMBILimits");

            migrationBuilder.DropTable(
                name: "LvUMPDLimits");

            migrationBuilder.CreateTable(
                name: "LvLimits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LimitDisplay = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LimitValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerAccident = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StateCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    LimitType = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvLimits", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LvLimits");

            migrationBuilder.CreateTable(
                name: "LvAutoBILimits",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LimitDisplay = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LimitValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerAccident = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StateCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvAutoBILimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvAutoPDLimits",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LimitDisplay = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LimitValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerAccident = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StateCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvAutoPDLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvCargoLimits",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LimitDisplay = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LimitValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerAccident = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StateCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvCargoLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvCollisionLimits",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LimitDisplay = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LimitValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerAccident = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StateCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvCollisionLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvComprehensiveLimits",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LimitDisplay = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LimitValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerAccident = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StateCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvComprehensiveLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvGlLimits",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LimitDisplay = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LimitValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerAccident = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StateCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvGlLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvLiabilityDeductibleLimits",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LimitDisplay = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LimitValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerAccident = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StateCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvLiabilityDeductibleLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvMedPayLimits",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LimitDisplay = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LimitValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerAccident = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StateCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvMedPayLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvPipLimits",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LimitDisplay = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LimitValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerAccident = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StateCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvPipLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvRefLimits",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LimitDisplay = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LimitValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerAccident = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StateCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvRefLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvUIMBILimits",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LimitDisplay = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LimitValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerAccident = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StateCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvUIMBILimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvUIMPDLimits",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LimitDisplay = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LimitValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerAccident = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StateCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvUIMPDLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvUMBILimits",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LimitDisplay = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LimitValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerAccident = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StateCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvUMBILimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LvUMPDLimits",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LimitDisplay = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LimitValue = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerAccident = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    PerPerson = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    SingleLimit = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    StateCode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LvUMPDLimits", x => x.Id);
                });
        }
    }
}
