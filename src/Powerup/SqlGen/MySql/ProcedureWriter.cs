using DatabaseSchemaReader.DataSchema;
using HandlebarsDotNet;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerup.SqlGen.MySql
{
    internal class ProcedureWriter : ExecutableWriterBase<DatabaseStoredProcedure>
    {
        public ProcedureWriter() : base("PROCEDURE")
        {
        }

        protected override void InOutFunction(TextWriter writer, dynamic context, object[] parameters)
        {
            if (context.In && context.Out)
                writer.WriteSafeString("INOUT ");
            else if (context.In)
                writer.WriteSafeString("IN ");
            else if (context.Out)
                writer.WriteSafeString("OUT ");
        }
    }
}