using Powerup.SqlObjects;
using Powerup.Templates;

namespace Powerup.SqlQueries
{
    public class FunctionQuery : SysObjectQueryBase
    {
        public override string NameSql
        {
            get
            {
                return @"SELECT  o.name Name, s.name [Schema], o.object_id
                FROM    sys.objects o
                INNER JOIN sys.schemas s 
	                ON o.schema_id = s.schema_id
                where o.type in ('FN','TF','IF')
	                and o.name not like 'dt_%'
                order by o.object_id";
            }
        }

        public override string Folder
        {
            get { return "functions"; }
        }

        public override SqlType SqlType
        {
            get { return SqlType.Function; }
        }

        public override ITemplate TemplateToUse(SqlObject sqlObject)
        {
            return new DropCreateTemplate(sqlObject);
        }
    }
}