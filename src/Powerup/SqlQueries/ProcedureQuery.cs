using Powerup.SqlObjects;

namespace Powerup.SqlQueries
{
    public class ProcedureQuery : SysObjectQueryBase
    {
        public override string NameSql
        {
            get
            {
                return @"SELECT  o.name Name, s.name [Schema], o.object_id
                FROM    sys.objects o
                INNER JOIN sys.schemas s 
	                ON o.schema_id = s.schema_id
                where o.type = 'P'
	                and o.name not like 'dt_%'
                order by o.object_id";
            }
        }

        public override string Folder { get {return "sprocs";}}
        public override SqlType SqlType { get { return SqlType.Procedure; } }
    }

}