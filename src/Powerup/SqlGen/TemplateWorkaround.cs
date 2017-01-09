using Powerup.SqlObjects;
using Powerup.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerup.SqlGen
{
    public class TemplateWorkaround : ITemplate
    {
        protected SqlObject _sqlObject;

        public TemplateWorkaround(SqlObject sqlObject)
        {
            _sqlObject = sqlObject;
        }

        public string Name { get { return _sqlObject.FullName(); } }
        public string Proc { get; protected set; }
        public string FolderName { get { return _sqlObject.Folder; } }
        public string Type { get { return _sqlObject.SqlType.ToString(); } }
        public int ObjectId { get { return _sqlObject.ObjectId; } }
        public string Content { get { return _sqlObject.Code; } }

        public string FileName { get { return string.Format("{1}.sql", _sqlObject.Schema, _sqlObject.Name); } }

        public void AddText(string text)
        {
            throw new NotImplementedException();
        }
    }
}