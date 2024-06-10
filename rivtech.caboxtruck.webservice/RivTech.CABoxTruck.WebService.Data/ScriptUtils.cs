using RivTech.CABoxTruck.WebService.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RivTech.CABoxTruck.WebService.Data
{
    internal class ScriptUtils
    {
        public static string CreateTableWithAzureChecking(string createScript, string tableName)
        {
            return $@"
                if (not exists(select 1 from sys.objects where name like '{tableName}') and {IsAzure()})
                BEGIN
                    declare @script nvarchar(max) = '{createScript.Replace("'", "''")}'

                    exec sp_executesql @script
                    print '{tableName} external table created...'
                END

                else
                begin
                        print 'Failed to create external table {tableName}.'
                end
                GO
            ";
        }

        public static string RunScriptWithAzureChecking(string script)
        {
            string sanitizedScript = RemoveGOStatements(script.Replace("'", "''"));
            return $@"
                if ({IsAzure()})
                BEGIN
                    declare @script nvarchar(max) = '{sanitizedScript}'

                    exec sp_executesql @script
                    print 'Successfully run script with azure checking...'
                END

                else
                begin
                    print 'Failed to run script with azure checking...'
                end
                GO
            ";
        }

        /// <summary>
        /// Checks if the current environment is using SQL Azure.   
        /// </summary>
        private static string IsAzure() => "IIF(ServerProperty('Edition') = 'SQL Azure', 1, 0) = 1";

        /// <summary>
        /// Removes GO command since it's not a valid T-SQL statement.
        /// <!-- Reference: https://learn.microsoft.com/en-us/sql/t-sql/language-elements/sql-server-utilities-statements-go?view=sql-server-ver16j -->
        /// </summary>
        private static string RemoveGOStatements(string script)
        {
            string wordToReplace = "GO"; 
            string patternWithWordBounardies = $"\\b{wordToReplace}\\b";  
            return Regex.Replace(script, patternWithWordBounardies, "", RegexOptions.IgnoreCase);
        }
    }

}
