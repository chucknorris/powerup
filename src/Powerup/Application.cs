using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Powerup.SqlObjects;
using Powerup.SqlQueries;

namespace Powerup
{
    public class Application
    {
        Configuration conn;

        readonly List<IQueryBase> _typesToFind;
        List<SqlObject> _sqlObject = new List<SqlObject>();

        public Application(Configuration conn)
        {
            this.conn = conn;
            _typesToFind = new List<IQueryBase>
                                               {
                                                   new ProcedureQuery(),
                                                   new FunctionQuery(),
                                                   new ViewQuery(),
                                                   new IndexQuery()
                                               };

        }

        public IEnumerable<SqlObject> TheObjects()
        {
            return _sqlObject;
        }

        public void BuildEntities()
        {
            Console.WriteLine(conn.ConnectionStringBuilder.ConnectionString);
            using (var con = conn.ConnectionStringBuilder.CreateConnection())
            {
                con.Open();
                foreach (var type in _typesToFind)
                {
                    using (var cmd = new SqlCommand(type.NameSql, con))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows) return;
                            while (reader.Read())
                            {
                                _sqlObject.Add(type.MakeSqlObject(con.Database,
                                                                  reader[1].ToString(),
                                                                  reader[0].ToString(),
                                                                  (int)reader[2]));
                            }
                        }
                    }
                }
            }

        }

        public void AddCodeToEntities()
        {
            using (var con = this.conn.ConnectionStringBuilder.CreateConnection())
            {
                con.Open();
                foreach (var obj in this._sqlObject)
                {
                    obj.ParentQuery.AddCode(con, obj);
                }
            }
        }

        public void WriteOutPut()
        {

        }
    }
}