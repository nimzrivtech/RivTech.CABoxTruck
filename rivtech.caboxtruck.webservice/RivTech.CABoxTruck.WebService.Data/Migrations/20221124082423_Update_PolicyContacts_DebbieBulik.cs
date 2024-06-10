using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class Update_PolicyContacts_DebbieBulik : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_POLICY_CONTACTS);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        const string UPDATE_POLICY_CONTACTS = @"
            SET IDENTITY_INSERT [dbo].[PolicyContacts] ON
            DELETE FROM [dbo].[PolicyContacts] WHERE [Name] = 'Debbie Bulik'
            SET IDENTITY_INSERT [dbo].[PolicyContacts] OFF";
    }
}
