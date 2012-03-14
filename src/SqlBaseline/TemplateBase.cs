using SqlBaseline.SqlTypes;

namespace SqlBaseline
{
    public abstract class TemplateBase : ITemplate
    {
        protected TemplateBase(SqlObject sqlObject)
        {
            _sqlObject = sqlObject;
        }

        protected SqlObject _sqlObject;
        public string Name { get { return _sqlObject.FullName();  } }
        public string Proc { get; protected set; }
        public string FolderName { get { return _sqlObject.Folder; } }
        public string FileName { get { return string.Format("{0}.sql", _sqlObject.Name); } }
        public string Type { get { return _sqlObject.SqlType.ToString(); } }
        public int ObjectId { get { return _sqlObject.ObjectId; } }
        public string Content { get { return TemplatedProcedure(); } }
        
        public abstract string TemplatedProcedure();
        public abstract void AddText(string text);
    }
}