using System.Collections.Generic;
using Powerup.SqlObjects;
using Powerup.Templates;

namespace Powerup.SqlQueries
{
    using System.Data.SqlClient;

    public abstract class QueryBase : IQueryBase
    {
        protected QueryBase()
        {
            SqlObjects = new List<SqlObject>();
        }

        public abstract string NameSql { get; }

        public abstract string Folder { get; }

        public abstract void AddCode(SqlConnection connection, SqlObject obj);

        public string Database { get; private set; }
        public IList<SqlObject> SqlObjects { get; private set; }
        public abstract SqlType SqlType { get; }

        public abstract ITemplate TemplateToUse(SqlObject sqlObject);

        public void AddSqlObject(SqlObject sqlObject)
        {
            SqlObjects.Add(sqlObject);
        }

        public SqlObject MakeSqlObject(string dataBase, string schema, string name, int objectId)
        {
            return new SqlObject(this, dataBase)
                       {
                           Name = name,
                           Schema = schema,
                           ObjectId = objectId,
                           SqlType = SqlType
                       };
        }
    }
}