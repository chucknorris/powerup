using SqlBaseline.SqlObjects;
using SqlBaseline.SqlTypes;

namespace SqlBaseline.SqlQueries
{
    public class ViewQuery : QueryBase
    {
        public override string NameSql
        {
            get { return string.Format(nameSql, "= 'V'"); }
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