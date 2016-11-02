using DatabaseSchemaReader.DataSchema;
using HandlebarsDotNet;
using System;
using System.Collections.Generic;
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

DROP PROCEDURE IF EXISTS powerup_drop_index_if_exists $$
CREATE PROCEDURE powerup_drop_index_if_exists(in tableschema varchar(128), in theTable varchar(128), in theIndexName varchar(128) )
BEGIN
 IF((SELECT COUNT(*) AS index_exists FROM information_schema.statistics WHERE TABLE_SCHEMA = tableschema and table_name =
theTable AND index_name = theIndexName) > 0) THEN
   SET @s = CONCAT('DROP INDEX ' , theIndexName , ' ON ' , theTable);
   PREPARE stmt FROM @s;
   EXECUTE stmt;
 END IF;
END $$
CALL powerup_drop_index_if_exists('{{index.SchemaOwner}}', '{{index.TableName}}', '{{index.Name}}') $$

DROP PROCEDURE IF EXISTS powerup_drop_index_if_exists $$
DELIMITER ;

CREATE {{#if index.IsUnique}}UNIQUE {{/if}}INDEX {{index.Name}} ON {{index.TableName}}(
{{#each index.Columns}}
    {{Name}}{{#unless @last}},{{/unless}}
{{/each}}
);";

        static IndexWriter()
        {
            Handlebars.Configuration.TextEncoder = new TextEncoder();
            template = Handlebars.Compile(codeTemplate);
        }

        public string WriteSql(DatabaseIndex obj)
        {
            var data = new { index = obj };
            var result = template(data);
            return result;
        }
    }
}