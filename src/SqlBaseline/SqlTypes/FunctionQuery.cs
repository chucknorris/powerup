namespace SqlBaseline.SqlTypes
{
    public class FunctionQuery : QueryBase
    {
        public override string NameSql
        {
            get { return string.Format(nameSql, "in ('FN','TF')"); }
        }

        public override string Folder
        {
            get { return "functions"; }
        }

        public override SqlType SqlType
        {
            get { return SqlType.Function; }
        }
    }
}