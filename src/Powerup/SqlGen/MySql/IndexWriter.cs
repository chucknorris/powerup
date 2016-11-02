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

        private const string codeTemplate = @"IF (SELECT 1 = 1 FROM INFORMATION_SCHEMA.STATISTICS
WHERE table_schema='{{table.DatabaseSchema}}' AND table_name='{{index.TableName}}' AND index_name='{{index.Name}}')
    DROP INDEX {{index.Name}} ON {{index.TableName}};
CREATE INDEX {{index.Name}} ON {{index.TableName}}(
{{#each index.Columns}}
    {{Name}},
{{/each}}
);";

        static IndexWriter()
        {
            Handlebars.Configuration.TextEncoder = new TextEncoder();
            template = Handlebars.Compile(codeTemplate);
        }

        public string WriteSql(DatabaseIndex obj)
        {
            var data = new { table = obj.Columns[0].Table, index = obj };
            var result = template(data);
            return result;
        }
    }
}