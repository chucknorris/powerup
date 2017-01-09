using DatabaseSchemaReader.DataSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerup.SqlGen
{
    internal interface ISqlGenerator
    {
        ISqlWriter<DatabaseStoredProcedure> StoredProcedureSqlWriter { get; }
        ISqlWriter<DatabaseFunction> FunctionSqlWriter { get; }
        ISqlWriter<DatabaseView> ViewSqlWriter { get; }
        ISqlWriter<DatabaseIndex> IndexSqlWriter { get; }
    }
}