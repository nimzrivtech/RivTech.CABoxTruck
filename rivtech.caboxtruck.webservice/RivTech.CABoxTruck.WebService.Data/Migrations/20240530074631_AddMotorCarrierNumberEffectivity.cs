using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMotorCarrierNumberEffectivity : Migration
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
            INSERT INTO FieldVersion (Id,FieldName,Description,EffectiveDate) VALUES(NEWID(),'S38MotorCarrierNumber','New Field Motor Carrier Number','05-30-2024')
            INSERT INTO [Form] VALUES('MotorCarrierEndorsement','Motor Carrier Endorsement', 48, 1, 'RPCAAC054_1', 'Motor Carrier Endorsement.docx', 48, 0, 0, GETDATE(), 1, 0)
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM FieldVersion WHERE FieldName = 'S38MotorCarrierNumber'
            DELETE FROM Form WHERE Id = 'MotorCarrierEndorsement'
            GO
        ";
    }
}
