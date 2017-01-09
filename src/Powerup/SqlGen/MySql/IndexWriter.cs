using DatabaseSchemaReader.DataSchema;
using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerup.SqlGen.MySql
{
    internal class IndexWriter : ISqlWriter<DatabaseIndex>
    {
        private static readonly Func<object, string> template;

        private const string codeTemplate = @"
DELIMITER $$
;
DROP PROCEDURE IF EXISTS powerup_drop_index_if_exists $$
;
CREATE PROCEDURE powerup_drop_index_if_exists(in tableschema varchar(128), in theTable varchar(128), in theIndexName varchar(128) )
BEGIN
 IF((SELECT COUNT(*) AS index_exists FROM information_schema.statistics WHERE TABLE_SCHEMA = tableschema and table_name =
theTable AND index_name = theIndexName) > 0) THEN
   SET @s = CONCAT('DROP INDEX ' , theIndexName , ' ON ' , theTable);
   PREPARE stmt FROM @s;
   EXECUTE stmt;
 END IF;
END $$
;
DELIMITER ;
;
CALL powerup_drop_index_if_exists('{{index.SchemaOwner}}', '{{index.TableName}}', '{{index.Name}}');
DROP PROCEDURE IF EXISTS powerup_drop_index_if_exists;

CREATE {{index_type}}INDEX {{index.Name}} ON {{index.TableName}}(
{{#each index.Columns}}
    {{Name}}{{#unless @last}},{{/unless}}
{{/each}}
);
";

        static IndexWriter()
        {
            Handlebars.Configuration.TextEncoder = new TextEncoder();
            Handlebars.RegisterHelper("index_type", (HandlebarsHelper)IndexTypeFunction);
            template = Handlebars.Compile(codeTemplate);
        }

        private static void IndexTypeFunction(TextWriter output, dynamic context, object[] arguments)
        {
            var index = context.index as DatabaseIndex;
            if (index.IsUnique)
                output.Write("UNIQUE ");
            if (index.IndexType == "FULLTEXT")
                output.Write("FULLTEXT ");
        }

        public string WriteSql(DatabaseIndex obj)
        {
            var data = new { index = obj };
            var result = template(data);
            return result;
        }
    }
}