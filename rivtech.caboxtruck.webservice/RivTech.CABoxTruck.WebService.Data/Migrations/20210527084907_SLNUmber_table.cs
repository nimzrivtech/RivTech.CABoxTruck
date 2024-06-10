using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class SLNUmber_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SLNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditedOfficeId = table.Column<short>(type: "smallint", nullable: false),
                    StateCode = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true),
                    LicenseNumber = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SLNumber", x => x.Id);
                });


            migrationBuilder.Sql(this.rivington());
            migrationBuilder.Sql(this.vale());
            migrationBuilder.Sql(this.applied());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SLNumber");
        }

        private string rivington()
        {
            return @"INSERT INTO SLNumber (CreditedOfficeId, StateCode, LicenseNumber, IsActive)
                        VALUES (1, 'AK', '100137622',1),
                         (1, 'AL', '775407',1),
                         (1, 'AR', '8653594',1),
                         (1, 'AZ', '8653594',1),
                         (1, 'CA', '0F15123',1),
                         (1, 'CO', '523989',1),
                         (1, 'CT', '2677364',1),
                         (1, 'DC', '3000040633',1),
                         (1, 'DE', '3000038484',1),
                         (1, 'FL', 'W292971',1),
                         (1, 'GA', '3112452',1),
                         (1, 'HI', '510448',1),
                         (1, 'IA', '8653594',1),
                         (1, 'ID', '783482',1),
                         (1, 'IL', '8742354',1),
                         (1, 'IN', '3513192',1),
                         (1, 'KS', '8653594',1),
                         (1, 'KY', '936583',1),
                         (1, 'LA', '714092',1),
                         (1, 'MA', '2115065',1),
                         (1, 'MD', '2150537',1),
                         (1, 'ME', 'PRN278584',1),
                         (1, 'MI', '8653594',1),
                         (1, 'MN', '40683465',1),
                         (1, 'MO', '8392938',1),
                         (1, 'MS', '10460145',1),
                         (1, 'MT', '3000038477',1),
                         (1, 'NC', '8653594',1),
                         (1, 'ND', '8653594',1),
                         (1, 'NE', '8653594',1),
                         (1, 'NH', '8653594',1),
                         (1, 'NJ', '1141303',1),
                         (1, 'NM', '8653594',1),
                         (1, 'NV', '3543358',1),
                         (1, 'NY', '1024521',1),
                         (1, 'OH', '1289637',1),
                         (1, 'OK', '100277098',1),
                         (1, 'OR', '8653594',1),
                         (1, 'PA', '940877',1),
                         (1, 'RI', '3000038479',1),
                         (1, 'SC', '8653594',1),
                         (1, 'SD', '40434090',1),
                         (1, 'TN', '952068',1),
                         (1, 'TX', '2514710',1),
                         (1, 'UT', '775837',1),
                         (1, 'VA', '663215',1),
                         (1, 'VT', '3543612',1),
                         (1, 'WA', '936632',1),
                         (1, 'WI', '8653594',1),
                         (1, 'WV', '8653594',1),
                         (1, 'WY', '435743',1)";
        }

        private string vale()
        {
            return @"INSERT INTO SLNumber (CreditedOfficeId, StateCode, LicenseNumber, IsActive)
                     VALUES  (2, 'AK', '100137622',1),
                     (2, 'AL', '775407',1),
                     (2, 'AR', '8653594',1),
                     (2, 'AZ', '8653594',1),
                     (2, 'CA', '0F15123',1),
                     (2, 'CO', '523989',1),
                     (2, 'CT', '2677364',1),
                     (2, 'DC', '3000040633',1),
                     (2, 'DE', '3000038484',1),
                     (2, 'FL', 'W292971',1),
                     (2, 'GA', '3112452',1),
                     (2, 'HI', '510448',1),
                     (2, 'IA', '8653594',1),
                     (2, 'ID', '783482',1),
                     (2, 'IL', '8653594',1),
                     (2, 'IN', '3513192',1),
                     (2, 'KS', '8653594',1),
                     (2, 'KY', '936583',1),
                     (2, 'LA', '714092',1),
                     (2, 'MA', '2115065',1),
                     (2, 'MD', '2150537',1),
                     (2, 'ME', 'PRN278584',1),
                     (2, 'MI', '8653594',1),
                     (2, 'MN', '40683465',1),
                     (2, 'MO', '8392938',1),
                     (2, 'MS', '10460145',1),
                     (2, 'MT', '3000038477',1),
                     (2, 'NC', '8653594',1),
                     (2, 'ND', '8653594',1),
                     (2, 'NE', '8653594',1),
                     (2, 'NH', '8653594',1),
                     (2, 'NJ', '1141303',1),
                     (2, 'NM', '8653594',1),
                     (2, 'NV', '3543358',1),
                     (2, 'NY', '1024521',1),
                     (2, 'OH', '1289637',1),
                     (2, 'OK', '100277098',1),
                     (2, 'OR', '8653594',1),
                     (2, 'PA', '940877',1),
                     (2, 'RI', '3000038479',1),
                     (2, 'SC', '8653594',1),
                     (2, 'SD', '40434090',1),
                     (2, 'TN', '952068',1),
                     (2, 'TX', '2514710',1),
                     (2, 'UT', '775837',1),
                     (2, 'VA', '663215',1),
                     (2, 'VT', '3543612',1),
                     (2, 'WA', '936632',1),
                     (2, 'WI', '8653594',1),
                     (2, 'WV', '8653594',1),
                     (2, 'WY', '435743',1)";
        }

        private string applied()
        {
            return @"INSERT INTO SLNumber (CreditedOfficeId, StateCode, LicenseNumber, IsActive)
                     VALUES (3, 'AK', '100137622',1),
                     (3, 'AL', '775407',1),
                     (3, 'AR', '8653594',1),
                     (3, 'AZ', '8653594',1),
                     (3, 'CA', '0F15123',1),
                     (3, 'CO', '523989',1),
                     (3, 'CT', '2677364',1),
                     (3, 'DC', '3000040633',1),
                     (3, 'DE', '3000038484',1),
                     (3, 'FL', 'W292971',1),
                     (3, 'GA', '3112452',1),
                     (3, 'HI', '510448',1),
                     (3, 'IA', '8653594',1),
                     (3, 'ID', '783482',1),
                     (3, 'IL', '8653594',1),
                     (3, 'IN', '3513192',1),
                     (3, 'KS', '8653594',1),
                     (3, 'KY', '936583',1),
                     (3, 'LA', '714092',1),
                     (3, 'MA', '2115065',1),
                     (3, 'MD', '2150537',1),
                     (3, 'ME', 'PRN278584',1),
                     (3, 'MI', '8653594',1),
                     (3, 'MN', '40683465',1),
                     (3, 'MO', '8392938',1),
                     (3, 'MS', '10460145',1),
                     (3, 'MT', '3000038477',1),
                     (3, 'NC', '8653594',1),
                     (3, 'ND', '8653594',1),
                     (3, 'NE', '8653594',1),
                     (3, 'NH', '8653594',1),
                     (3, 'NJ', '1141303',1),
                     (3, 'NM', '8653594',1),
                     (3, 'NV', '3543358',1),
                     (3, 'NY', '1024521',1),
                     (3, 'OH', '1289637',1),
                     (3, 'OK', '100277098',1),
                     (3, 'OR', '8653594',1),
                     (3, 'PA', '940877',1),
                     (3, 'RI', '3000038479',1),
                     (3, 'SC', '8653594',1),
                     (3, 'SD', '40434090',1),
                     (3, 'TN', '952068',1),
                     (3, 'TX', '2514710',1),
                     (3, 'UT', '775837',1),
                     (3, 'VA', '663215',1),
                     (3, 'VT', '3543612',1),
                     (3, 'WA', '936632',1),
                     (3, 'WI', '8653594',1),
                     (3, 'WV', '8653594',1),
                     (3, 'WY', '435743',1)";
        }
    }
}
