using DatabaseSchemaReader.DataSchema;
using HandlebarsDotNet;
using System;
using System.IO;

namespace Powerup.SqlGen.MySql
{
    internal class ExecutableWriterBase<T> : ISqlWriter<T>
    {
        private const string codeTemplate = @"DROP PROCEDURE IF EXISTS {{obj.Name}};
DELIMITER $$
CREATE PROCEDURE {{obj.Name}} (
{{#each obj.Arguments}}
    {{in_out}}{{Name}} {{data_type}},
{{/each}}
) {{returns}}
{{obj.Sql}}$$
DELIMITER ;";

        private Func<object, string> template = null;

        protected ExecutableWriterBase(string objType)
        {
            Handlebars.RegisterHelper((string)"in_out", (HandlebarsHelper)InOutFunction);
            Handlebars.RegisterHelper((string)"returns", (HandlebarsHelper)ReturnsFunction);
            Handlebars.RegisterHelper((string)"data_type", (HandlebarsHelper)DataTypeFunction);
            template = Handlebars.Compile(codeTemplate.Replace("PROCEDURE", objType));
        }

        private static void InOutFunction(TextWriter writer, dynamic context, object[] parameters)
        {
            if (context.In && context.Out)
                writer.WriteSafeString("INOUT ");
            else if (context.In)
                writer.WriteSafeString("IN ");
            else if (context.Out)
                writer.WriteSafeString("OUT ");
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
            var result = template(new { obj = obj });
            return result;
        }

        private int CompareByOrdinal(DatabaseArgument arg1, DatabaseArgument arg2)
        {
            return arg1.Ordinal.CompareTo(arg2.Ordinal);
        }
    }
}