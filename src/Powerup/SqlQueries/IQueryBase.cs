using Powerup.SqlObjects;
using Powerup.Templates;

namespace Powerup.SqlQueries
{
    using System.Data.SqlClient;

    public interface IQueryBase
    {
        string NameSql { get; }

        void AddCode(SqlConnection connection, SqlObject obj);

        string Database { get; }
        string Folder { get; }
        SqlObject MakeSqlObject(string dataBase, string schema, string name, int objectId);
        ITemplate TemplateToUse(SqlObject sqlObject);
    }
}