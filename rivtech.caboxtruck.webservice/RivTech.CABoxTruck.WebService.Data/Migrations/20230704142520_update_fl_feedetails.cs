using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateflfeedetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SQL_INSERT);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        const string SQL_INSERT = @"
            delete FeeDetails where StateCode = 'FL' and [Description] = 'Service Fee - Multiple Units (AL)'
            delete FeeDetails where StateCode = 'FL' and [Description] = 'Service Fee - Single Units (AL)'
            delete FeeDetails where StateCode = 'FL' and [Description] = 'Service Fee - Midterm (AL)'
            delete FeeDetails where StateCode = 'FL' and [Description] = 'Service Fee - Multiple Units (PD)'
            delete FeeDetails where StateCode = 'FL' and [Description] = 'Service Fee - Single Units (PD)'
            delete FeeDetails where StateCode = 'FL' and [Description] = 'Service Fee - Midterm (PD)'
            delete FeeDetails where StateCode = 'FL' and [Description] = 'Service Fee - Flat Multiple Units (PD)'
            delete FeeDetails where StateCode = 'FL' and [Description] = 'Service Fee - Flat Midterm (AL)'
            delete FeeDetails where StateCode = 'FL' and [Description] = 'Service Fee - Flat Midterm (PD)'

            GO
        ";

    }
}
