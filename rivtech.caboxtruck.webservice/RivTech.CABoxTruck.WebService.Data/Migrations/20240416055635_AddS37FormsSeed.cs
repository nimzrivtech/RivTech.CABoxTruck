using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddS37FormsSeed : Migration
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
            INSERT INTO [Form] VALUES('PermissiveExclusionEndorsement','Permissive Use Exclusion Endorsement', 100, 1, 'RPCAAC050_1', 'Permissive Use Exclusion Endorsement.docx', 100, 0, 0, GETDATE(), 0, 0)
            INSERT INTO [Form] VALUES('UnreportedVehicleDeductible','Unreported Vehicle Deductible', 101, 1, 'RPCAAC051_1', 'Unreported Vehicle Deductible.docx', 101, 0, 0, GETDATE(), 0, 0)
            INSERT INTO [Form] VALUES('UnreportedDriverDeductible','Unreported Driver Deductible', 102, 1, 'RPCAAC052_1', 'Unreported Driver Deductible.docx', 102, 0, 0, GETDATE(), 0, 0)
            INSERT INTO [Form] VALUES('FormFMotorCarrierBIPD','Form F - Uniform Motor Carrier Bodily Injury and Property Damage Liability Insurance Endorsement', 103, 1, 'RPCAAC055_1', 'Form F - Uniform Motor Carrier BIPD Insurance Endorsement.docx', 103, 0, 0, GETDATE(), 1, 0)
            GO
        ";

        const string SQL_DELETE = @"
            DELETE FROM [Form] WHERE Id = 'PermissiveExclusionEndorsement'
            DELETE FROM [Form] WHERE Id = 'UnreportedVehicleDeductible'
            DELETE FROM [Form] WHERE Id = 'UnreportedDriverDeductible'
            DELETE FROM [Form] WHERE Id = 'FormFMotorCarrierBIPD'
            GO
        ";
    }
}
