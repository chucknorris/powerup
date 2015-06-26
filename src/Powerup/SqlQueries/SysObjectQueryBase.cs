namespace Powerup.SqlQueries
{
    using Powerup.SqlObjects;
    using Powerup.Templates;

    public abstract class SysObjectQueryBase : QueryBase
    {
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