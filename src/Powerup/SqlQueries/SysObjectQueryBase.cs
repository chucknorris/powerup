namespace Powerup.SqlQueries
{
    using System.Data.SqlClient;

    using Powerup.SqlObjects;
    using Powerup.Templates;

    public abstract class SysObjectQueryBase : QueryBase
    {
        public override void AddCode(SqlConnection connection, SqlObject obj)
        {
            using (var cmd = new SqlCommand(@"SELECT c.text
                FROM SYS.syscomments c
                where c.id = @1
                order by c.colid", connection))
            {
                cmd.Parameters.Add(new SqlParameter("@1", obj.ObjectId));
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows) return;
                    while (reader.Read())
                    {
                        obj.Code += reader[0].ToString();
                    }

                    obj.AddCodeTemplate();
                }
            }
        }

        public override ITemplate TemplateToUse(SqlObject sqlObject)
        {
            return new CreateAlterTemplate(sqlObject);
        }
    }
}