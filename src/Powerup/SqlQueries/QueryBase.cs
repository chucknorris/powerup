using System.Collections.Generic;
using Powerup.SqlObjects;
using Powerup.Templates;

namespace Powerup.SqlQueries
{
    public abstract class QueryBase : IQueryBase
    {
        protected string nameSql =
            @"SELECT  o.name Name, s.name [Schema], o.object_id
                FROM    sys.objects o
                INNER JOIN sys.schemas s 
	                ON o.schema_id = s.schema_id
                where o.type {0}
	                and o.name not like 'dt_%'
                order by o.object_id";

        string textQuery = @"SELECT c.text
                FROM SYS.syscomments c
                where c.id = @1
                order by c.colid";

        protected QueryBase()
        {
            SqlObjects = new List<SqlObject>();
        }

        public abstract string NameSql { get; }
        public string TextSql { get { return textQuery; } }
        public abstract string Folder { get; }
        public string Database { get; private set; }
        public IList<SqlObject> SqlObjects { get; private set; }
        public abstract SqlType SqlType { get; }
        
        public virtual ITemplate TemplateToUse(SqlObject sqlObject)
        {
            return new CreateAlterTemplate(sqlObject);
        }

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