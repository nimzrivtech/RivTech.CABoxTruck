using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateStateTaxAlabama : Migration
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
            insert into StateTax values ('AL','SL','Main','2021-01-01 00:00:00.0000000',6.000)

            insert into FeeDetails values ('AL','RMFAL','Service Fee - Flat Multiple Units (AL)',30.00,1)
            insert into FeeDetails values ('AL','REINF','Reinstatement Fee',15.00,1)
            GO
        ";

        const string SQL_DELETE = @"
            delete from StateTax where StateCode in ('AL')
            delete from FeeDetails where StateCode in ('AL')
            GO
        ";
    }
}
