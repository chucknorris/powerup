using DatabaseSchemaReader.DataSchema;
using System.Collections.Generic;
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
    }
}