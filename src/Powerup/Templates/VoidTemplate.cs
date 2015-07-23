namespace Powerup.Templates
{
    using System;
    using System.Text;

    using Powerup.SqlObjects;

    public class VoidTemplate : TemplateBase
    {
        StringBuilder buffer = new StringBuilder();

        public VoidTemplate(SqlObject sqlObject)
            : base(sqlObject)
        {
        }

        public override void AddText(string text)
        {
            this.buffer.Append(text);
        }

        public override string TemplatedProcedure()
        {
            return this.buffer.ToString();
        }
    }
}