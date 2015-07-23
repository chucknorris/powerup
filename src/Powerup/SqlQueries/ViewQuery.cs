using Powerup.SqlObjects;

namespace Powerup.SqlQueries
{
    public class ViewQuery : SysObjectQueryBase
    {
        public override string NameSql
        {
            get
            {
                return @"SELECT  o.name Name, s.name [Schema], o.object_id
                FROM    sys.objects o
                INNER JOIN sys.schemas s 
	                ON o.schema_id = s.schema_id
                where o.type = 'V'
	                and o.name not like 'dt_%'
                order by o.object_id";
            }
        }

        public override string Folder
        {
            get { return "views"; }
        }

        public override SqlType SqlType
        {
            get { return SqlType.View; }
        }
    }
}