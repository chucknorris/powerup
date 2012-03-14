namespace SqlBaseline.SqlTypes
{
    public interface IQueryBase
    {
        string NameSql { get; }
        string TextSql { get; }
        string Database { get; }
        string Folder { get; }
        SqlObject MakeSqlObject(string dataBase, string schema, string name, int objectId);
        ITemplate TemplateToUse(SqlObject sqlObject);
    }
}