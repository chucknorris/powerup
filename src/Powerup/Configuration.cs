using System.Data.SqlClient;

namespace Powerup
{
    public class Configuration
    {
        public string OutputFolder { get; internal set; }
        public SqlConnectionStringBuilder ConnectionStringBuilder { get; private set; }

        public Configuration()
        {
            ConnectionStringBuilder = new SqlConnectionStringBuilder();
        }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(ConnectionStringBuilder.DataSource) &&
                          !string.IsNullOrEmpty(ConnectionStringBuilder.InitialCatalog) &&
                          !string.IsNullOrEmpty(OutputFolder);
            }
        }
    }
}