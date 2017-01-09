using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;
using Powerup.SqlGen;
using Powerup.SqlGen.MySql;
using Powerup.SqlObjects;
using Powerup.SqlQueries;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Powerup
{
    public class Application
    {
        private Configuration configuration;

        private readonly List<IQueryBase> _typesToFind;
        private List<SqlObject> _sqlObjects = new List<SqlObject>();

        public Application(Configuration configuration)
        {
            this.configuration = configuration;
            _typesToFind = new List<IQueryBase>
                                               {
                                                   new ProcedureQuery(),
                                                   new FunctionQuery(),
                                                   new ViewQuery(),
                                                   new IndexQuery(),
                                                   new SequenceQuery()
                                               };
        }

        public IEnumerable<SqlObject> TheObjects()
        {
            return _sqlObjects;
        }

        public void BuildEntities()
        {
            Console.WriteLine(configuration.ConnectionStringBuilder.ConnectionString);
            // Keep building the mssql scripts without change for backwards compatibility
            if (configuration.IsMsSql)
                BuildEntitiesMsSql();
            else
                BuildEntitiesGeneric();
        }

        private void BuildEntitiesGeneric()
        {
            using (
                var dbReader = new DatabaseReader(configuration.ConnectionStringBuilder.ConnectionString,
                    configuration.ProviderName))
            {
                var schema = dbReader.ReadAll();
                ReadStoredProceduresGeneric(schema);
                ReadFunctionsGeneric(schema);
                ReadViewsGeneric(schema);
                if (configuration.IsMySql)
                    ReadIndexesMySql(schema);
            }
        }

        private void BuildEntitiesMsSql()
        {
            using (var con = configuration.CreateConnection())
            {
                con.Open();
                foreach (var type in _typesToFind)
                {
                    using (var cmd = new SqlCommand(type.NameSql, con as SqlConnection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows) return;
                            while (reader.Read())
                            {
                                _sqlObjects.Add(type.MakeSqlObject(con.Database,
                                    reader[1].ToString(),
                                    reader[0].ToString(),
                                    (int)reader[2]));
                            }
                        }
                    }
                }
            }
        }

        private void ReadStoredProceduresGeneric(DatabaseSchema schema)
        {
            IQueryBase type = new ProcedureQuery();
            var writer = configuration.SqlGenerator.StoredProcedureSqlWriter;
            var databaseStoredProcedures = schema.StoredProcedures.Where(obj => obj.SchemaOwner == configuration.InitialCatalog);
            foreach (var obj in databaseStoredProcedures)
            {
                var sqlObject = type.MakeSqlObject(configuration.InitialCatalog, schema.Owner, obj.Name, 0);
                sqlObject.Code = writer.WriteSql(obj);
                sqlObject.SetTemplateWorkaround(new TemplateWorkaround(sqlObject));
                _sqlObjects.Add(sqlObject);
            }
        }

        private void ReadFunctionsGeneric(DatabaseSchema schema)
        {
            IQueryBase type = new FunctionQuery();
            var writer = configuration.SqlGenerator.FunctionSqlWriter;
            var databaseFunctions = schema.Functions.Where(obj => obj.SchemaOwner == configuration.InitialCatalog);
            foreach (var obj in databaseFunctions)
            {
                var sqlObject = type.MakeSqlObject(configuration.InitialCatalog, schema.Owner, obj.Name, 0);
                sqlObject.Code = writer.WriteSql(obj);
                sqlObject.SetTemplateWorkaround(new TemplateWorkaround(sqlObject));
                _sqlObjects.Add(sqlObject);
            }
        }

        private void ReadViewsGeneric(DatabaseSchema schema)
        {
            IQueryBase type = new ViewQuery();
            var writer = configuration.SqlGenerator.ViewSqlWriter;
            var databaseViews = schema.Views.Where(obj => obj.SchemaOwner == configuration.InitialCatalog);
            foreach (var obj in databaseViews)
            {
                var sqlObject = type.MakeSqlObject(configuration.InitialCatalog, schema.Owner, obj.Name, 0);
                sqlObject.Code = writer.WriteSql(obj);
                sqlObject.SetTemplateWorkaround(new TemplateWorkaround(sqlObject));
                _sqlObjects.Add(sqlObject);
            }
        }

        private void ReadIndexesMySql(DatabaseSchema schema)
        {
            var type = new IndexQuery();
            var writer = configuration.SqlGenerator.IndexSqlWriter;
            var databaseTables = schema.Tables.Where(obj => obj.SchemaOwner == configuration.InitialCatalog);
            foreach (var table in databaseTables)
            {
                foreach (var index in table.Indexes.Where(ix => ix.Name != "PRIMARY" && !ix.Name.ToLowerInvariant().Contains("fk_")))
                {
                    var sqlObject = type.MakeSqlObject(configuration.InitialCatalog, schema.Owner, index.Name, 0);
                    sqlObject.Code = writer.WriteSql(index);
                    sqlObject.SetTemplateWorkaround(new TemplateWorkaround(sqlObject));
                    _sqlObjects.Add(sqlObject);
                }
            }
        }

        public void AddCodeToEntities()
        {
            // Keep building the mssql scripts without change for backwards compatibility
            if (configuration.IsMsSql)
                AddCodeToEntitiesMsSql();
            else
                AddCodeToEntitiesGeneric();
        }

        private void AddCodeToEntitiesGeneric()
        {
            //Already added in BuildEntitiesGeneric
        }

        private void AddCodeToEntitiesMsSql()
        {
            using (var con = this.configuration.CreateConnection())
            {
                con.Open();
                foreach (var obj in this._sqlObjects)
                {
                    obj.ParentQuery.AddCode(con as SqlConnection, obj);
                }
            }
        }

        public void WriteOutPut()
        {
        }
    }
}