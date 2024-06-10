using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSymbolTenResponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SymbolTenResponse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "varchar(1000)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymbolTenResponse", x => x.Id);
                });

            migrationBuilder.Sql(SQL_INSERT);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SymbolTenResponse");

            migrationBuilder.Sql(SQL_DELETE);
        }

        const string SQL_INSERT = @"
            SET IDENTITY_INSERT SymbolTenResponse ON
            INSERT INTO [SymbolTenResponse] (Id, Description, IsActive) VALUES(1, 'Covered Auto Designation Symbols {VAL},8,9 for operations on behalf of any additional insured for which {VAL},8,9 symbols are contractually required and the Company has an executed contract on file. Covered Auto Designation Symbol 7 for all other operations.', 1)
            INSERT INTO [SymbolTenResponse] (Id, Description, IsActive) VALUES(2, 'Covered Auto Designation Symbols {VAL},8,9 for operations on behalf of any additional insured for which {VAL},8,9 symbols are contractually required and the Company has an executed contract on file. Covered Auto Designation Symbol 7 for all other operations. Coverage for hired and non-owned vehicles (symbols 8 & 9) are specifically excluded for all operations involving affiliates or subsidiaries.', 1)
            INSERT INTO [SymbolTenResponse] (Id, Description, IsActive) VALUES(3, 'Covered Auto Designation Symbols {VAL},8,9 for operations on behalf of any additional insured for which {VAL},8,9 symbols are contractually required and the Company has an executed contract on file. Covered Auto Designation Symbol 7 for all other operations. Coverage for hired and non-owned vehicles (symbols 8 & 9) are specifically excluded for all operations involving non-disclosed affiliates or non-disclosed subsidiaries.', 1)
            INSERT INTO [SymbolTenResponse] (Id, Description, IsActive) VALUES(4, 'Covered Auto Designation Symbols {VAL},8,9 for operations on behalf of any additional insured for which {VAL},8,9 symbols are contractually required and the Company has an executed contract on file. Covered Auto Designation Symbol 7 for all other operations. Coverage for hired and non-owned vehicles is specifically excluded if operated with an unlisted driver.', 1)
            INSERT INTO [SymbolTenResponse] (Id, Description, IsActive) VALUES(5, 'Covered Auto Designation Symbols {VAL},8,9 for operations on behalf of any additional insured for which {VAL},8,9 symbols are contractually required and the Company has an executed contract on file. Covered Auto Designation Symbol 7 for all other operations. Coverage for hired and non-owned vehicles (symbols 8 & 9) are specifically excluded for all operations in which the hired auto is hired with a driver.', 1)
            INSERT INTO [SymbolTenResponse] (Id, Description, IsActive) VALUES(6, 'Other', 1)
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM [SymbolTenResponse] WHERE Id IN (1,2,3,4,5,6)
            GO
        ";
    }
}
