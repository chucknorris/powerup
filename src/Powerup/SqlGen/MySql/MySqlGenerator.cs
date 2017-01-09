using DatabaseSchemaReader.DataSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerup.SqlGen.MySql
{
    internal class MySqlGenerator : ISqlGenerator
    {
        private ProcedureWriter procedureWriter = new ProcedureWriter();
        private FunctionWriter functionWriter = new FunctionWriter();
        private ViewWriter viewWriter = new ViewWriter();
        private IndexWriter indexWriter = new IndexWriter();

        public ISqlWriter<DatabaseStoredProcedure> StoredProcedureSqlWriter => procedureWriter;
        public ISqlWriter<DatabaseFunction> FunctionSqlWriter => functionWriter;
        public ISqlWriter<DatabaseView> ViewSqlWriter => viewWriter;
        public ISqlWriter<DatabaseIndex> IndexSqlWriter => indexWriter;
    }
}