using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addstateTaxTN : Migration
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
            insert into StateTax values ('TN','SL','Main','2021-01-01 00:00:00.0000000',5.000)
            insert into StateTax values ('TN','SF','P','2021-01-01 00:00:00.0000000',0.175)

            insert into FeeDetails values ('TN','RMFAL','Service Fee - Multiple Units (AL)',100.00,1)
            insert into FeeDetails values ('TN','RMFAL','Service Fee - Single Units (AL)',200.00,1)
            insert into FeeDetails values ('TN','RMFAL','Service Fee - Midterm (AL)',100.00,1)
            insert into FeeDetails values ('TN','RMFPD','Service Fee - Multiple Units (PD)',50.00,1)
            insert into FeeDetails values ('TN','RMFPD','Service Fee - Single Units (PD)',100.00,1)
            insert into FeeDetails values ('TN','RMFPD','Service Fee - Midterm (PD)',50.00,1)
            insert into FeeDetails values ('TN','REINF','Reinstatement Fee',250.00,1)
            insert into FeeDetails values ('TN','REINF2','Reinstatement Fee',500.00,1)
            GO
        ";

        const string SQL_DELETE = @"
            delete from StateTax where StateCode in ('TN')
            delete from FeeDetails where StateCode in ('TN')
            GO
        ";
    }
}
