using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addstateTaxOR : Migration
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
            insert into StateTax values ('OR','SL','Main','2021-01-01 00:00:00.0000000',2.300)
            insert into StateTax values ('OR','SF','F','2021-01-01 00:00:00.0000000',10)


            insert into FeeDetails values ('OR','RMFAL','Service Fee - Multiple Units (AL)',100.00,1)
            insert into FeeDetails values ('OR','RMFAL','Service Fee - Single Units (AL)',200.00,1)
            insert into FeeDetails values ('OR','RMFAL','Service Fee - Midterm (AL)',100.00,1)
            insert into FeeDetails values ('OR','RMFPD','Service Fee - Multiple Units (PD)',50.00,1)
            insert into FeeDetails values ('OR','RMFPD','Service Fee - Single Units (PD)',100.00,1)
            insert into FeeDetails values ('OR','RMFPD','Service Fee - Midterm (PD)',50.00,1)
            insert into FeeDetails values ('OR','REINF','Reinstatement Fee',100.00,1)
            GO
        ";

        const string SQL_DELETE = @"
            delete from StateTax where StateCode in ('OR')
            delete from FeeDetails where StateCode in ('OR')
            GO
        ";
    }
}
