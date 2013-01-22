using System.Text.RegularExpressions;
using Powerup.SqlObjects;

namespace Powerup.Templates
{
    public class CreateAlterTemplate : TemplateBase
    {
        /// <summary>
        /// 0: Name
        /// 1: Schema
        /// 2: Code
        /// 3: Type (PROCEDURE, VIEW, etc)
        /// 4: Database
        /// </summary>

        const string template =
            @"
DECLARE @Name VarChar(100),@Type VarChar(20), @Schema VarChar(20)
            SELECT @Name = '{0}', @Type = '{3}', @Schema = '{1}'

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(@Schema + '.' +  @Name))
BEGIN
  DECLARE @SQL varchar(1000)
  SET @SQL = 'CREATE ' + @Type + ' ' + @Schema + '.' + @Name + ' AS SELECT * FROM sys.objects'
  EXECUTE(@SQL)
END 
PRINT 'Updating ' + @Type + ' ' + @Schema + '.' + @Name
GO

{2}
";

        readonly Regex removeCreate = new Regex(@"^(CREATE)\s.*(PROCEDURE |PROC |VIEW [\s\w\.])(.*)$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

        public CreateAlterTemplate(SqlObject sqlObject)
            : base(sqlObject)
        {
        }

        public override string TemplatedProcedure()
        {
            return string.Format(template,
                                    _sqlObject.Name,
                                    _sqlObject.Schema,
                                    Proc,
                                    _sqlObject.SqlType.ToString().ToUpper());
        }



        public override void AddText(string text)
        {
            this.Proc = removeCreate.Replace(text, "ALTER $2 $3");
        }
        
    }
}
