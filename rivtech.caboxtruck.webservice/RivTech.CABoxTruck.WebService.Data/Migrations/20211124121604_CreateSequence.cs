using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class CreateSequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE SEQUENCE NextPolicyNumber
                                    MINVALUE 1
                                    START WITH 1
                                    INCREMENT BY 1

                                    CREATE SEQUENCE NextSubmissionNumber
                                    MINVALUE 1
                                    START WITH 1
                                    INCREMENT BY 1


                                    CREATE SEQUENCE NextQuoteNumber
                                    MINVALUE 1
                                    START WITH 1
                                    INCREMENT BY 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"drop sequence NextPolicyNumber
                                    drop sequence NextSubmissionNumber
                                    drop sequence NextQuoteNumber");
        }
    }
}
