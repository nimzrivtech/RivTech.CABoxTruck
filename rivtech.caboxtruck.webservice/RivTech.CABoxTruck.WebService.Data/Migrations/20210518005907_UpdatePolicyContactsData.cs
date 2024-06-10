using Microsoft.EntityFrameworkCore.Migrations;

namespace RivTech.CABoxTruck.WebService.Data.Migrations
{
    public partial class UpdatePolicyContactsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(UPDATE_SCRIPT);
            migrationBuilder.Sql(DELETE_SCRIPT);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

        // update email for each policy contacts
        const string UPDATE_SCRIPT = @"
            UPDATE [PolicyContacts] SET [Email] = 'acbond@auw.com' WHERE Id = 1
            UPDATE [PolicyContacts] SET [Email] = 'kajensen@auw.com' WHERE Id = 2
            UPDATE [PolicyContacts] SET [Email] = 'wrmuller@auw.com' WHERE Id = 3
            UPDATE [PolicyContacts] SET [Email] = 'knpaulsen@auw.com' WHERE Id = 4
            UPDATE [PolicyContacts] SET [Email] = 'trredman@auw.com' WHERE Id = 5
            UPDATE [PolicyContacts] SET [Email] = 'jrussel@auw.com' WHERE Id = 6
            UPDATE [PolicyContacts] SET [Email] = 'tltegley@auw.com' WHERE Id = 7
            UPDATE [PolicyContacts] SET [Email] = 'cjmillard@auw.com' WHERE Id = 8
            UPDATE [PolicyContacts] SET [Email] = 'jprunyan@auw.com' WHERE Id = 9
            UPDATE [PolicyContacts] SET [Email] = 'bbburnett@auw.com' WHERE Id = 10
            UPDATE [PolicyContacts] SET [Email] = 'jjclark@auw.com' WHERE Id = 11
            UPDATE [PolicyContacts] SET [Email] = 'jjcurttright@auw.com' WHERE Id = 12
            UPDATE [PolicyContacts] SET [Email] = 'rdfisher@auw.com' WHERE Id = 13
            UPDATE [PolicyContacts] SET [Email] = 'jrgrebenick@auw.com' WHERE Id = 14
            UPDATE [PolicyContacts] SET [Email] = 'jeosborne@auw.com' WHERE Id = 15
            UPDATE [PolicyContacts] SET [Email] = 'mpuetz@auw.com' WHERE Id = 16
            UPDATE [PolicyContacts] SET [Email] = 'kdsolon@auw.com' WHERE Id = 17
            UPDATE [PolicyContacts] SET [Email] = 'dbulik@rivpartners.com' WHERE Id = 18
            UPDATE [PolicyContacts] SET [Email] = 'jtericson@rivpartners.com' WHERE Id = 19
            UPDATE [PolicyContacts] SET [Email] = 'dbulik@rivpartners.com' WHERE Id = 20
            UPDATE [PolicyContacts] SET [Email] = 'jtericson@rivpartners.com' WHERE Id = 21
            UPDATE [PolicyContacts] SET [Email] = 'amenerson@auw.com' WHERE Id = 22
            UPDATE [PolicyContacts] SET [Email] = 'mjkiser@auw.com' WHERE Id = 23
            UPDATE [PolicyContacts] SET [Email] = 'seschendt@auw.com' WHERE Id = 24
            UPDATE [PolicyContacts] SET [Email] = 'catranisi@auw.com' WHERE Id = 26
            UPDATE [PolicyContacts] SET [Email] = 'kmbachman@auw.com' WHERE Id = 27
            UPDATE [PolicyContacts] SET [Email] = 'crfrey@auw.com' WHERE Id = 28
            UPDATE [PolicyContacts] SET [Email] = 'msury@auw.com' WHERE Id = 29
            UPDATE [PolicyContacts] SET [Email] = 'jagross@auw.com' WHERE Id = 30
            UPDATE [PolicyContacts] SET [Email] = 'kljohnson@auw.com' WHERE Id = 31
            UPDATE [PolicyContacts] SET [Email] = 'sjpopejoy@auw.com' WHERE Id = 32
            UPDATE [PolicyContacts] SET [Email] = 'cawood@auw.com' WHERE Id = 33
            UPDATE [PolicyContacts] SET [Email] = 'rcjones@auw.com' WHERE Id = 34
            UPDATE [PolicyContacts] SET [Email] = 'kmmeisinger@auw.com' WHERE Id = 35
            UPDATE [PolicyContacts] SET [Email] = 'kamuckey@auw.com' WHERE Id = 36
            UPDATE [PolicyContacts] SET [Email] = 'mjarehart@auw.com' WHERE Id = 37
            UPDATE [PolicyContacts] SET [Email] = 'mdjack@auw.com' WHERE Id = 38
            UPDATE [PolicyContacts] SET [Email] = 'klkester@auw.com' WHERE Id = 39
            UPDATE [PolicyContacts] SET [Email] = 'ajscott@auw.com' WHERE Id = 40
            UPDATE [PolicyContacts] SET [Email] = 'jachristoffersen@auw.com' WHERE Id = 42
            UPDATE [PolicyContacts] SET [Email] = 'tajroenfeldt@auw.com' WHERE Id = 43
            UPDATE [PolicyContacts] SET [Email] = 'pstvrdy@auw.com' WHERE Id = 44
            UPDATE [PolicyContacts] SET [Email] = 'rechrostek@auw.com' WHERE Id = 45
            UPDATE [PolicyContacts] SET [Email] = 'kcmurphy@auw.com' WHERE Id = 46
            UPDATE [PolicyContacts] SET [Email] = 'jfpeffer@auw.com' WHERE Id = 47
            UPDATE [PolicyContacts] SET [Email] = 'blsiegert@auw.com' WHERE Id = 48
        ";

        // removed records
        const string DELETE_SCRIPT = @"DELETE FROM [PolicyContacts] WHERE Id IN (25, -- Jody Sperling
										                                         41) -- Trevor Bruneau";
    }
}
