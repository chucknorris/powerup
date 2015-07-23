using Powerup.SqlQueries;
using Powerup.Templates;

namespace Powerup.SqlObjects
{
    public class SqlObject
    {
        public SqlObject(IQueryBase parentQuery, string dataBase)
        {
            this.parentQuery = parentQuery;
            this.Database = dataBase;

        }

        readonly IQueryBase parentQuery;
        ITemplate codeTemplate;

        public string Schema { get; set; }
        public string Name { get; set; }
        public int ObjectId { get; set; }
        public SqlType SqlType { get; set; }
        public string Code { get; set; }
        public ITemplate ThingToTemplate { get { return codeTemplate; } }
        public string Folder { get { return parentQuery.Folder; } }
        public string Database { get; private set; }

        public IQueryBase ParentQuery
        {
            get
            {
                return this.parentQuery;
            }
        }

        public void AddCodeTemplate()
        {
            codeTemplate = parentQuery.TemplateToUse(this);
            codeTemplate.AddText(Code);
        }

        public string FullName()
        {
            return string.Format("{0}.[{1}].[{2}]", Database, Schema, Name);
        }

        public override string ToString()
        {
            return FullName();
        }
    }
}
