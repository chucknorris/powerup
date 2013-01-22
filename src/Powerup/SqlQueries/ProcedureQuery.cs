using Powerup.SqlObjects;

namespace Powerup.SqlQueries
{
    public class ProcedureQuery : QueryBase
    {
        public override string NameSql { get { return string.Format(nameSql, "= 'P'"); } }
        public override string Folder { get {return "sprocs";}}
        public override SqlType SqlType { get { return SqlType.Procedure; } }
    }

}