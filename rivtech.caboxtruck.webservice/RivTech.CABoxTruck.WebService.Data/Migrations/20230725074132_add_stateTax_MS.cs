using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addstateTaxMS : Migration
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
            insert into StateTax values ('MS','SL','Main','2021-01-01 00:00:00.0000000',7.000)
            insert into StateTax values ('MS','SF','P','2021-01-01 00:00:00.0000000',0.250)

            insert into FeeDetails values ('MS','RMFAL','Service Fee - Multiple Units (AL)',100.00,1)
            insert into FeeDetails values ('MS','RMFAL','Service Fee - Single Units (AL)',200.00,1)
            insert into FeeDetails values ('MS','RMFAL','Service Fee - Midterm (AL)',100.00,1)
            insert into FeeDetails values ('MS','RMFPD','Service Fee - Multiple Units (PD)',50.00,1)
            insert into FeeDetails values ('MS','RMFPD','Service Fee - Single Units (PD)',100.00,1)
            insert into FeeDetails values ('MS','RMFPD','Service Fee - Midterm (PD)',50.00,1)
            insert into FeeDetails values ('MS','REINF','Reinstatement Fee',250.00,1)
            insert into FeeDetails values ('MS','REINF2','Reinstatement Fee',500.00,1)
            GO
        ";

        const string SQL_DELETE = @"
            delete from StateTax where StateCode in ('MS')
            delete from FeeDetails where StateCode in ('MS')
            GO
        ";
    }
}
