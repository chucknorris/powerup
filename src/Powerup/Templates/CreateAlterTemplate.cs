using System.Text.RegularExpressions;
using Powerup.SqlObjects;

namespace Powerup.Templates
{
    public class CreateAlterTemplate : TemplateBase
    {
        readonly Regex removeCreate = new Regex(@"^(?:\s*)?(CREATE)\s.*(PROCEDURE |PROC |VIEW )(.*)$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

        public CreateAlterTemplate(SqlObject sqlObject)
            : base(sqlObject)
        {
        }

        public override string TemplatedProcedure()
        {
            return string.Format(
                @"DECLARE @Name nvarchar(128), @Type nvarchar(20), @Schema nvarchar(128)
SELECT @Name = N'{0}', @Type = N'{1}', @Schema = N'{2}'

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID('[' + @Schema + '].[' + @Name + ']'))
    EXECUTE('CREATE ' + @Type + ' [' + @Schema + '].[' + @Name + '] AS SELECT * FROM sys.objects')

PRINT 'Creating/Updating ' + @Type + ' [' + @Schema + '].[' + @Name + ']'
GO

{3}",
                this._sqlObject.Name,
                this._sqlObject.SqlType.ToString().ToUpper(),
                this._sqlObject.Schema,
                this.Proc);
        }

        public override void AddText(string text)
        {
            this.Proc = removeCreate.Replace(text, "ALTER $2 $3");
        }

    }
}
