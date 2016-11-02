using DatabaseSchemaReader.DataSchema;
using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Powerup.SqlGen.MySql
{
    internal class ExecutableWriterBase<T> : ISqlWriter<T>
    {
        private const string codeTemplate = @"DROP PROCEDURE IF EXISTS {{obj.Name}};
DELIMITER $$
;
CREATE PROCEDURE {{obj.Name}} (
{{#each args}}
    {{in_out}}{{Name}} {{data_type}}{{#unless @last}},{{/unless}}
{{/each}}
) {{returns}}
{{obj.Sql}}$$
DELIMITER ;
;";

        private Func<object, string> template = null;

        protected ExecutableWriterBase(string objType)
        {
            Handlebars.RegisterHelper((string)"in_out", (HandlebarsHelper)InOutFunction);
            Handlebars.RegisterHelper((string)"returns", (HandlebarsHelper)ReturnsFunction);
            Handlebars.RegisterHelper((string)"data_type", (HandlebarsHelper)DataTypeFunction);
            Handlebars.Configuration.TextEncoder = new TextEncoder();
            template = Handlebars.Compile(codeTemplate.Replace("PROCEDURE", objType));
        }

        protected virtual void InOutFunction(TextWriter writer, dynamic context, object[] parameters)
        {
        }

        private static void DataTypeFunction(TextWriter writer, dynamic context, object[] parameters)
        {
            DataTypeWriter.Write(writer, (DatabaseArgument)context);
        }

        protected virtual void ReturnsFunction(TextWriter writer, dynamic context, object[] arguments)
        {
        }

        public string WriteSql(T obj)
        {
            return InnerWriteSql(obj);
        }

        protected string InnerWriteSql(dynamic obj)
        {
            obj.Arguments.Sort((Comparison<DatabaseArgument>)CompareByOrdinal);
            var args = ((List<DatabaseArgument>)obj.Arguments).Where(arg => arg.Name != null);
            var result = template(new { obj = obj, args = args });
            return result;
        }

        private int CompareByOrdinal(DatabaseArgument arg1, DatabaseArgument arg2)
        {
            return arg1.Ordinal.CompareTo(arg2.Ordinal);
        }
    }
}