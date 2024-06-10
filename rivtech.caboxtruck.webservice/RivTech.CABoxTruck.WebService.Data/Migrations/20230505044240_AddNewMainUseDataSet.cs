using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewMainUseDataSet : Migration
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
            INSERT INTO MainUse (Id, [Description], IsActive) VALUES (4, 'Heavy Loads/Special', 1);
            INSERT INTO MainUse (Id, [Description], IsActive) VALUES (5, 'Logging', 1);
            INSERT INTO MainUse (Id, [Description], IsActive) VALUES (6, 'Non-Business', 1);
            INSERT INTO MainUse (Id, [Description], IsActive) VALUES (7, 'Pizza/Prepared Food', 1);
            INSERT INTO MainUse (Id, [Description], IsActive) VALUES (8, 'Funeral', 1);
            INSERT INTO MainUse (Id, [Description], IsActive) VALUES (9, 'Child Care', 1);
            INSERT INTO MainUse (Id, [Description], IsActive) VALUES (10, 'Church', 1);
            INSERT INTO MainUse (Id, [Description], IsActive) VALUES (11, 'Social Service Passenger Transport', 1);
            INSERT INTO MainUse (Id, [Description], IsActive) VALUES (12, 'Livery', 1);
            INSERT INTO MainUse (Id, [Description], IsActive) VALUES (13, 'Repossession', 1);
            INSERT INTO MainUse (Id, [Description], IsActive) VALUES (14, 'Coal Hauling', 1);
            INSERT INTO MainUse (Id, [Description], IsActive) VALUES (15, 'Towing 24/7', 1);


        ";

        const string SQL_DELETE = @"
            DELETE FROM MainUse WHERE Id = 4;
            DELETE FROM MainUse WHERE Id = 5;
            DELETE FROM MainUse WHERE Id = 6;
            DELETE FROM MainUse WHERE Id = 7;
            DELETE FROM MainUse WHERE Id = 8;
            DELETE FROM MainUse WHERE Id = 9;
            DELETE FROM MainUse WHERE Id = 10;
            DELETE FROM MainUse WHERE Id = 11;
            DELETE FROM MainUse WHERE Id = 12;
            DELETE FROM MainUse WHERE Id = 13;
            DELETE FROM MainUse WHERE Id = 14;
            DELETE FROM MainUse WHERE Id = 15;


        ";
    }
}
