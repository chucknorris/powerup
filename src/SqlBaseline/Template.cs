using System.Text.RegularExpressions;
using SqlBaseline.SqlTypes;

namespace SqlBaseline
{
    public class Template : ITemplate
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
        
        SqlObject _sqlObject;


        readonly Regex removeCreate = new Regex(@"^(CREATE)\s.*(PROCEDURE|VIEW|FUNCTION[\s\w\.])(.*)$", RegexOptions.Multiline | RegexOptions.IgnoreCase);


        public Template(SqlObject sqlObject)
        {
            _sqlObject = sqlObject;
        }

        public string Name { get { return _sqlObject.FullName();  } }
        public string Proc { get; private set; }
        public string FolderName { get { return _sqlObject.Folder; } }
        public string FileName { get { return string.Format("{0}.sql", _sqlObject.Name); } }
        public string Type { get { return _sqlObject.SqlType.ToString(); } }
        public int ObjectId { get { return _sqlObject.ObjectId; } }
        public string Content { get { return TemplatedProcedure(); } }
        
        public void SetSqlObject(SqlObject sqlObject)
        {
            _sqlObject = sqlObject;
        }

        public string TemplatedProcedure()
        {
            return string.Format(template,
                                    _sqlObject.Name,
                                    _sqlObject.Schema,
                                    Proc,
                                    _sqlObject.SqlType.ToString().ToUpper());
        }

        public void AddText(string text)
        {
            this.Proc = removeCreate.Replace(text, "ALTER $2 $3");
        }
        
    }
}
