using Powerup.SqlObjects;

namespace Powerup.Templates
{
    public class DropCreateTemplate : TemplateBase
    {
        public DropCreateTemplate(SqlObject sqlObject) : base(sqlObject)
        {
        }

        public override string TemplatedProcedure()
        {
            return string.Format(
                @"DECLARE @Name nvarchar(128), @Type nvarchar(20), @Schema nvarchar(128)
SELECT @Name = N'{0}', @Type = N'{1}', @Schema = N'{2}'

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID('[' + @Schema + '].[' + @Name + ']'))  
    EXECUTE('DROP ' + @Type + ' [' + @Schema + '].[' + @Name + ']')

PRINT 'Creating ' + @Type + ' [' + @Schema + '].[' + @Name + ']'
GO

{3}",
                this._sqlObject.Name,
                this._sqlObject.SqlType.ToString().ToUpper(),
                this._sqlObject.Schema,
                this.Proc);
        }

        public override void AddText(string text)
        {
            this.Proc = text;
        }
    }
}