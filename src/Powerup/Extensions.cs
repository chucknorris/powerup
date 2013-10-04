using System.Data.SqlClient;

namespace Powerup
{
    static class Extensions
    {
        public static SqlConnection CreateConnection(this SqlConnectionStringBuilder builder)
        {
            return new SqlConnection(builder.ConnectionString);
        }
    }
}
