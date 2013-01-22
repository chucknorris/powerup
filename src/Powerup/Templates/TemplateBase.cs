using Powerup.SqlObjects;

namespace Powerup.Templates
{
    public abstract class TemplateBase : ITemplate
    {
     
        protected SqlObject _sqlObject;

        protected TemplateBase(SqlObject sqlObject)
        {
            _sqlObject = sqlObject;
        }

        public string Name { get { return _sqlObject.FullName(); } }
        public string Proc { get; protected set; }
        public string FolderName { get { return _sqlObject.Folder; } }
        public string Type { get { return _sqlObject.SqlType.ToString(); } }
        public int ObjectId { get { return _sqlObject.ObjectId; } }
        public string Content { get { return TemplatedProcedure(); } }

        public string FileName { get { return string.Format("{0}.{1}.sql", _sqlObject.Schema, _sqlObject.Name); } }

        public abstract string TemplatedProcedure();
        public abstract void AddText(string text);
    

    }
}