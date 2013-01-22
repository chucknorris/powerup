using Powerup.SqlObjects;
using Powerup.Templates;

namespace Powerup.SqlQueries
{
    public class FunctionQuery : QueryBase
    {
        public override string NameSql
        {
            get { return string.Format(nameSql, "in ('FN','TF','IF')"); }
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