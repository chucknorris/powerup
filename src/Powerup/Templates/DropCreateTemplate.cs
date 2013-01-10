using Powerup.SqlObjects;

namespace Powerup.Templates
{
    public class DropCreateTemplate : TemplateBase
    {
        /// <summary>
        /// 0: Name
        /// 1: Schema
        /// 2: Code
        /// 3: Type (PROCEDURE, VIEW, etc)
        /// 4: Database
        /// </summary>
        const string Template =
            @"IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'{1}.{0}')) DROP {3} {1}.{0}
GO

{2} ";

        public DropCreateTemplate(SqlObject sqlObject) : base(sqlObject)
        {
        }

        public override string TemplatedProcedure()
        {
            return string.Format(Template,
                                    _sqlObject.Name,
                                    _sqlObject.Schema,
                                    Proc,
                                    _sqlObject.SqlType.ToString().ToUpper());
        }

        public override void AddText(string text)
        {
            this.Proc = text;
        }
    }
}