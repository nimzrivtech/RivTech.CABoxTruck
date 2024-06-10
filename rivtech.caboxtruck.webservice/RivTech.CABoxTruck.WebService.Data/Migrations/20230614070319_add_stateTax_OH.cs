using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addstateTaxOH : Migration
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
            insert into StateTax values ('OH','SL','Main','2021-01-01 00:00:00.0000000',5.000)

            insert into NonTaxableFee values ('OH', 'INSTF', '2021-01-01 00:00:00.0000000')
            insert into NonTaxableFee values ('OH', 'REINF', '2021-01-01 00:00:00.0000000')
            insert into NonTaxableFee values ('OH', 'RMFAL', '2021-01-01 00:00:00.0000000')
            insert into NonTaxableFee values ('OH', 'RMFPD', '2021-01-01 00:00:00.0000000')

            insert into FeeDetails values ('OH','RMFAL','Service Fee - Multiple Units (AL)',100.00,1)
            insert into FeeDetails values ('OH','RMFAL','Service Fee - Single Units (AL)',200.00,1)
            insert into FeeDetails values ('OH','RMFAL','Service Fee - Midterm (AL)',100.00,1)
            insert into FeeDetails values ('OH','RMFPD','Service Fee - Multiple Units (PD)',50.00,1)
            insert into FeeDetails values ('OH','RMFPD','Service Fee - Single Units (PD)',100.00,1)
            insert into FeeDetails values ('OH','RMFPD','Service Fee - Midterm (PD)',50.00,1)
            insert into FeeDetails values ('OH','REINF','Reinstatement Fee',250.00,1)

            insert into FeeDetails values ('OH','REINF2','Reinstatement Fee',500.00,1)
            insert into FeeDetails values ('FL','REINF2','Reinstatement Fee',500.00,1)
            insert into FeeDetails values ('PA','REINF2','Reinstatement Fee',500.00,1)
            insert into FeeDetails values ('GA','REINF2','Reinstatement Fee',500.00,1)
            insert into FeeDetails values ('MO','REINF2','Reinstatement Fee',500.00,1)
            GO
        ";

        const string SQL_DELETE = @"
            delete from StateTax where StateCode in ('OH')
            delete from FeeDetails where StateCode in ('OH')
            GO
        ";
    }
}
