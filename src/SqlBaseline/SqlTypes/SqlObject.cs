namespace SqlBaseline.SqlTypes
{
    public class SqlObject
    {
        public SqlObject(IQueryBase parentQuery, string dataBase)
        {
            this.parentQuery = parentQuery;
            this.Database = dataBase;
        }

        readonly IQueryBase parentQuery;
        Template codeTemplate;

        public string Schema { get; set; }
        public string Name { get; set; }
        public int ObjectId { get; set; }
        public SqlType SqlType { get; set; }
        public string Code { get; set; }
        public string Query { get { return parentQuery.TextSql; } }
        public ITemplate ThingToTemplate { get { return codeTemplate; } }
        public string Folder { get { return parentQuery.Folder; } }
        public string Database { get; private set; }


        public void AddCodeTemplate(Template template)
        {
            codeTemplate = template;
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
