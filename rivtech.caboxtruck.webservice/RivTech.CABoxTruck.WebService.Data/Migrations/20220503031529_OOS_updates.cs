using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class OOS_updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE [dbo].[RiskVehiclePremium] DROP CONSTRAINT [FK_RiskVehiclePremium_RiskVehiclePremiumCoverage_CoveragePremiumId]");
            migrationBuilder.Sql("ALTER TABLE [dbo].[RiskPolicyPremium] DROP CONSTRAINT [FK_RiskPolicyPremium_RiskPolicyPremiumCoverage_CoveragePremiumId]");
            migrationBuilder.Sql("ALTER TABLE [dbo].[RiskVehiclePremiumCoverage] DROP CONSTRAINT [PK_RiskVehiclePremiumCoverage]");
            migrationBuilder.Sql("ALTER TABLE [dbo].[RiskPolicyPremiumCoverage] DROP CONSTRAINT [PK_RiskPolicyPremiumCoverage]");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "RiskVehiclePremiumCoverage",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "CoveragePremiumId",
                table: "RiskVehiclePremium",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "RiskPolicyPremiumCoverage",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "CoveragePremiumId",
                table: "RiskPolicyPremium",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.Sql(@" ALTER TABLE [dbo].[RiskVehiclePremiumCoverage] ADD  CONSTRAINT [PK_RiskVehiclePremiumCoverage] PRIMARY KEY CLUSTERED ( [Id] ASC ) ");
            migrationBuilder.Sql(@" ALTER TABLE [dbo].[RiskPolicyPremiumCoverage] ADD  CONSTRAINT [PK_RiskPolicyPremiumCoverage] PRIMARY KEY CLUSTERED ( [Id] ASC ) ");

            migrationBuilder.Sql("ALTER TABLE [dbo].[RiskVehiclePremium]  WITH CHECK ADD  CONSTRAINT [FK_RiskVehiclePremium_RiskVehiclePremiumCoverage_CoveragePremiumId] FOREIGN KEY([CoveragePremiumId]) REFERENCES[dbo].[RiskVehiclePremiumCoverage]([Id])");
            migrationBuilder.Sql("ALTER TABLE [dbo].[RiskPolicyPremium]  WITH CHECK ADD  CONSTRAINT [FK_RiskPolicyPremium_RiskPolicyPremiumCoverage_CoveragePremiumId] FOREIGN KEY([CoveragePremiumId]) REFERENCES[dbo].[RiskPolicyPremiumCoverage]([Id])");

            migrationBuilder.Sql("ALTER TABLE [dbo].[RiskVehiclePremium] CHECK CONSTRAINT [FK_RiskVehiclePremium_RiskVehiclePremiumCoverage_CoveragePremiumId]");
            migrationBuilder.Sql("ALTER TABLE [dbo].[RiskPolicyPremium] CHECK CONSTRAINT [FK_RiskPolicyPremium_RiskPolicyPremiumCoverage_CoveragePremiumId]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RiskVehiclePremiumCoverage",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "CoveragePremiumId",
                table: "RiskVehiclePremium",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RiskPolicyPremiumCoverage",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "CoveragePremiumId",
                table: "RiskPolicyPremium",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
