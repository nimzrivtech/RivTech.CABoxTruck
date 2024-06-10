using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addstateTaxFL : Migration
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
            insert into StateTax values ('FL','SL','Main','2021-01-01 00:00:00.0000000',5.000)
            insert into StateTax values ('FL','SF','P','2021-01-01 00:00:00.0000000',0.100)


            insert into FeeDetails values ('FL','RMFAL','Service Fee - Multiple Units (AL)',25.00,1)
            insert into FeeDetails values ('FL','RMFAL','Service Fee - Single Units (AL)',25.00,1)
            insert into FeeDetails values ('FL','RMFAL','Service Fee - Midterm (AL)',0.00,1)
            insert into FeeDetails values ('FL','RMFPD','Service Fee - Multiple Units (PD)',25.00,1)
            insert into FeeDetails values ('FL','RMFPD','Service Fee - Single Units (PD)',25.00,1)
            insert into FeeDetails values ('FL','RMFPD','Service Fee - Midterm (PD)',0.00,1)
            insert into FeeDetails values ('FL','REINF','Reinstatement Fee',250.00,1)
            GO
        ";

        const string SQL_DELETE = @"
            delete from StateTax where StateCode in ('FL')
            delete from FeeDetails where StateCode in ('FL')
            GO
        ";
    }
}
