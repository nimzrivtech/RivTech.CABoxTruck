using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatestatetaxnontaxablefeedetails : Migration
    {
        /// <inheritdoc />
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
            insert into StateTax values ('GA','SL','Main','2021-01-01 00:00:00.0000000',4.000)
            insert into StateTax values ('MO','SL','Main','2021-01-01 00:00:00.0000000',5.000)
            insert into StateTax values ('PA','SL','Main','2021-01-01 00:00:00.0000000',3.000)
            insert into StateTax values ('PA','SF','F','2021-01-01 00:00:00.0000000',20)

            insert into NonTaxableFee values ('PA', 'INSTF', '2021-01-01 00:00:00.0000000')
            insert into NonTaxableFee values ('PA', 'REINF', '2021-01-01 00:00:00.0000000')
            insert into NonTaxableFee values ('PA', 'RMFAL', '2021-01-01 00:00:00.0000000')
            insert into NonTaxableFee values ('PA', 'RMFPD', '2021-01-01 00:00:00.0000000')

            insert into FeeDetails values ('PA','RMFAL','Service Fee - Multiple Units (AL)',100.00,1)
            insert into FeeDetails values ('PA','RMFAL','Service Fee - Single Units (AL)',200.00,1)
            insert into FeeDetails values ('PA','RMFAL','Service Fee - Midterm (AL)',100.00,1)
            insert into FeeDetails values ('PA','RMFPD','Service Fee - Multiple Units (PD)',50.00,1)
            insert into FeeDetails values ('PA','RMFPD','Service Fee - Single Units (PD)',100.00,1)
            insert into FeeDetails values ('PA','RMFPD','Service Fee - Midterm (PD)',50.00,1)
            insert into FeeDetails values ('PA','REINF','Reinstatement Fee',250.00,1)

            insert into FeeDetails values ('GA','RMFAL','Service Fee - Multiple Units (AL)',100.00,1)
            insert into FeeDetails values ('GA','RMFAL','Service Fee - Single Units (AL)',200.00,1)
            insert into FeeDetails values ('GA','RMFAL','Service Fee - Midterm (AL)',100.00,1)
            insert into FeeDetails values ('GA','RMFPD','Service Fee - Multiple Units (PD)',50.00,1)
            insert into FeeDetails values ('GA','RMFPD','Service Fee - Single Units (PD)',100.00,1)
            insert into FeeDetails values ('GA','RMFPD','Service Fee - Midterm (PD)',50.00,1)
            insert into FeeDetails values ('GA','REINF','Reinstatement Fee',250.00,1)

            insert into FeeDetails values ('MO','RMFAL','Service Fee - Multiple Units (AL)',100.00,1)
            insert into FeeDetails values ('MO','RMFAL','Service Fee - Single Units (AL)',200.00,1)
            insert into FeeDetails values ('MO','RMFAL','Service Fee - Midterm (AL)',100.00,1)
            insert into FeeDetails values ('MO','RMFPD','Service Fee - Multiple Units (PD)',50.00,1)
            insert into FeeDetails values ('MO','RMFPD','Service Fee - Single Units (PD)',100.00,1)
            insert into FeeDetails values ('MO','RMFPD','Service Fee - Midterm (PD)',50.00,1)
            insert into FeeDetails values ('MO','REINF','Reinstatement Fee',250.00,1)
            GO
        ";

        const string SQL_DELETE = @"
            delete from StateTax where StateCode in ('GA','MO','PA')
            delete from NonTaxableFee where StateCode = 'PA'
            delete from FeeDetails where StateCode in ('GA','MO','PA')
            GO
        ";
    }
}
