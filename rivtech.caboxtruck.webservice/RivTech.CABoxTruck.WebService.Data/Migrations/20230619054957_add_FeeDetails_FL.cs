using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addFeeDetailsFL : Migration
    {
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
            insert into FeeDetails values ('FL','RMFAL','Service Fee - Flat Multiple Units (AL)',25.00,1)
            insert into FeeDetails values ('FL','RMFPD','Service Fee - Flat Multiple Units (PD)',25.00,1)

            insert into FeeDetails values ('FL','RMFAL','Service Fee - Flat Midterm (AL)',0.00,1)
            insert into FeeDetails values ('FL','RMFPD','Service Fee - Flat Midterm (PD)',0.00,1)

            GO
        ";

        const string SQL_DELETE = @"
            delete from FeeDetails where StateCode in ('FL')
            GO
        ";
    }
}
