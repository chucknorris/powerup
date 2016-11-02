using DatabaseSchemaReader.DataSchema;
using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerup.SqlGen.MySql
{
    internal class ViewWriter : ISqlWriter<DatabaseView>
    {
        private static readonly Func<object, string> template;

        private const string codeTemplate = @"DROP VIEW IF EXISTS {{view.Name}};
CREATE VIEW {{view.Name}} AS
{{view.Sql}};";

        static ViewWriter()
        {
            template = Handlebars.Compile(codeTemplate);
        }

        public string WriteSql(DatabaseView obj)
        {
            var data = new { view = obj };
            var result = template(data);
            return result;
        }
    }
}