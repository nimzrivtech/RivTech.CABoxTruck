using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AddREINF2ToFeeDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(INSERT_REINF2_DATA);
        }

		const string INSERT_REINF2_DATA = @"
            INSERT INTO [dbo].[FeeDetails] ([StateCode], [FeeKindId], [Description], [Amount], [IsActive])
            VALUES  ('CA','REINF2','Reinstatement Fee',500, 1),
                    ('NJ','REINF2','Reinstatement Fee',500, 1),
                    ('TX','REINF2','Reinstatement Fee',500, 1)
        ";

		protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DELETE_REINF2_DATA);
        }
        const string DELETE_REINF2_DATA = @"
            DELETE FROM [dbo].[FeeDetails] WHERE FeeKindId = 'REINF2'
        ";
    }
}
