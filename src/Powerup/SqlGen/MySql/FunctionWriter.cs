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
            if (context.obj.ReturnType != null)
                writer.WriteSafeString($"returns {context.obj.ReturnType}");
        }
    }
}