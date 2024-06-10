using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class AddCourierCoverageEndorsement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(queryAddCourierCoverageEndorsement);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(queryRemoveCourierCoverageEndorsement);
		}

        const string queryAddCourierCoverageEndorsement = @"
			IF NOT EXISTS (SELECT * FROM Form WHERE Id = 'CourierCoverageEndorsement')
			   BEGIN
				   INSERT INTO Form
					(
						[Id]
						,[Name]
						,[SortOrder]
						,[IsActive]
						,[FormNumber]
						,[FileName]
						,[DecPageOrder]
						,[IsSupplementalDocument]
						,[CreatedBy]
						,[CreatedDate]
						,[IsMandatory]
						,[IsDefault]
					)
					VALUES
					(
						'CourierCoverageEndorsement',
						'Courier Coverage Endorsement',
						37,
						1,
						'RPCA033',
						'BT Courier Coverage Endorsement.docx',
						37,
						0,
						0,
						GETDATE(),
						0,
						1
					);

					UPDATE Form
					SET SortOrder = 38, DecPageOrder = 38
					WHERE Id = 'RefrigerationCargoEndorsement';

					UPDATE Form
					SET SortOrder = 39, DecPageOrder = 39
					WHERE Id = 'UninsuredMotoristCoverageBodilyInjury';
			   END
        ";

		const string queryRemoveCourierCoverageEndorsement = @"
			DELETE FROM Form
			WHERE Id = 'CourierCoverageEndorsement'

			UPDATE Form
			SET SortOrder = 37, DecPageOrder = 37
			WHERE Id = 'RefrigerationCargoEndorsement';

			UPDATE Form
			SET SortOrder = 38, DecPageOrder = 38
			WHERE Id = 'UninsuredMotoristCoverageBodilyInjury';
        ";
	}
}
