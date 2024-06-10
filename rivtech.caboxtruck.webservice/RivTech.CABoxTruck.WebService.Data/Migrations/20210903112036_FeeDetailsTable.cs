using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class FeeDetailsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StateCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeeKindId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeDetails", x => x.Id);
                });

            migrationBuilder.Sql(INSERT_FEEKIND_DATA);
            migrationBuilder.Sql(INSERT_INIT_DATA);
            migrationBuilder.Sql(INSERT_NONTAXABLEFEE_DATA);
            migrationBuilder.Sql(INSERT_STATETAX_DATA);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeDetails");
        }

        const string INSERT_INIT_DATA = @"
            INSERT INTO [dbo].[FeeDetails] ([StateCode], [FeeKindId], [Description], [Amount], [IsActive])
            VALUES('NJ','RMFAL','Service Fee - Multiple Units (AL)',100, 1),
		            ('NJ','RMFAL','Service Fee - Single Units (AL)',200, 1),
		            ('NJ','RMFAL','Service Fee - Midterm (AL)',100, 1),
		            ('NJ','RMFPD','Service Fee - Multiple Units (PD)',50, 1),
		            ('NJ','RMFPD','Service Fee - Single Units (PD)',100, 1),
		            ('NJ','RMFPD','Service Fee - Midterm (PD)',50, 1),
		            ('NJ','REINF','Reinstatement Fee',250, 1),
		            ('CA','RMFAL','Service Fee - Multiple Units (AL)',100, 1),
		            ('CA','RMFAL','Service Fee - Single Units (AL)',200, 1),
		            ('CA','RMFAL','Service Fee - Midterm (AL)',100, 1),
		            ('CA','RMFPD','Service Fee - Multiple Units (PD)',50, 1),
		            ('CA','RMFPD','Service Fee - Single Units (PD)',100, 1),
		            ('CA','RMFPD','Service Fee - Midterm (PD)',50, 1),
		            ('CA','REINF','Reinstatement Fee',250, 1),
		            ('TX','RMFAL','Service Fee - Multiple Units (AL)',100, 1),
		            ('TX','RMFAL','Service Fee - Single Units (AL)',200, 1),
		            ('TX','RMFAL','Service Fee - Midterm (AL)',100, 1),
		            ('TX','RMFPD','Service Fee - Multiple Units (PD)',50, 1),
		            ('TX','RMFPD','Service Fee - Single Units (PD)',100, 1),
		            ('TX','RMFPD','Service Fee - Midterm (PD)',50, 1),
		            ('TX','REINF','Reinstatement Fee',250, 1)
        ";

        const string INSERT_FEEKIND_DATA = @"
            INSERT INTO [dbo].[FeeKind] ([Id], [Description], [IsActive])
					VALUES	('INSTF', 'Installment Fee', 1)
        ";

        const string INSERT_NONTAXABLEFEE_DATA = @"
            INSERT INTO [dbo].[NonTaxableFee] ([StateCode], [FeeKindId], [EffectiveDate])
	                                    VALUES	('NJ', 'INSTF', '2021-01-01 00:00:00.0000000'),
			                                    ('NJ', 'REINSTAT', '2021-01-01 00:00:00.0000000'),
			                                    ('CA', 'RMFAL', '2021-01-01 00:00:00.0000000'),
			                                    ('CA', 'RMFPD', '2021-01-01 00:00:00.0000000'),
			                                    ('CA', 'INSTF', '2021-01-01 00:00:00.0000000'),
			                                    ('CA', 'REINSTAT', '2021-01-01 00:00:00.0000000')
        ";

        const string INSERT_STATETAX_DATA = @"
            INSERT INTO [dbo].[StateTax] ([StateCode], [TaxType], [TaxSubtype], [EffectiveDate], [Rate])
					  VALUES ('CA', 'SL', 'Main', '2021-01-01 00:00:00.0000000', 3.000),
							 ('CA', 'SF', 'P', '2021-01-01 00:00:00.0000000', 0.250),
							 ('TX', 'SL', 'Main', '2021-01-01 00:00:00.0000000', 4.850),
							 ('TX', 'SF', 'P', '2021-01-01 00:00:00.0000000', 0.075)
        ";
    }
}
