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
    internal class FunctionWriter : ExecutableWriterBase<DatabaseFunction>
    {
        public FunctionWriter() : base("FUNCTION")
        {
        }

        protected override void ReturnsFunction(TextWriter writer, dynamic context, object[] arguments)
        {
            var func = context.obj as DatabaseFunction;
            if (func.ReturnType != null)
            {
                writer.Write("RETURNS ");
                var returnArg = func.Arguments.FirstOrDefault(arg => arg.Name == null && arg.DatabaseDataType == func.ReturnType);
                if (returnArg != null)
                    DataTypeWriter.Write(writer, returnArg);
                else
                    writer.WriteSafeString(func.ReturnType);
            }
        }
    }
}