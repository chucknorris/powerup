namespace Powerup.SqlQueries
{
    using Powerup.SqlObjects;
    using Powerup.Templates;

    public abstract class SysObjectQueryBase : QueryBase
    {
        protected string nameSql = @"SELECT  o.name Name, s.name [Schema], o.object_id
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

        public override string TextSql { get { return textQuery; } }

        public override ITemplate TemplateToUse(SqlObject sqlObject)
        {
            return new CreateAlterTemplate(sqlObject);
        }
    }
}