using MySql.Data.MySqlClient;
using Powerup.SqlGen;
using System.Data.Common;
using System.Data.SqlClient;

namespace Powerup
{
    public class Configuration
    {
        public string InitialCatalog { get; set; }
        public string DataSource { get; set; }
        public bool IntegratedSecurity { get; set; }
        public string UserID;
        public string Password { get; set; }
        public string OutputFolder { get; internal set; }
        public DbConnectionStringBuilder ConnectionStringBuilder { get; private set; }

        private string providerName = "System.Data.SqlClient";

        public string ProviderName
        {
            get { return providerName; }
            set
            {
                switch (value)
                {
                    case "mssql":
                        providerName = "System.Data.SqlClient";
                        break;

                    case "mysql":
                        providerName = "MySql.Data.MySqlClient";
                        break;

                    default:
                        providerName = value;
                        break;
                }
            }
        }

        public bool IsMsSql => providerName == "System.Data.SqlClient";
        public bool IsMySql => providerName.ToLowerInvariant().Contains("mysql");

        private ISqlGenerator generator;

        internal ISqlGenerator SqlGenerator
        {
            get
            {
                if (generator == null)
                    generator = new SqlGeneratorFactory().Create(providerName);
                return generator;
            }
        }

        public Configuration()
        {
        }

        public bool Initialize()
        {
            if (IsValid)
            {
                if (IsMsSql)
                    ConnectionStringBuilder = new SqlConnectionStringBuilder
                    {
                        DataSource = DataSource,
                        InitialCatalog = InitialCatalog,
                        IntegratedSecurity = IntegratedSecurity,
                        UserID = UserID,
                        Password = Password
                    };
                if (IsMySql)
                    ConnectionStringBuilder = new MySqlConnectionStringBuilder()
                    {
                        Server = DataSource,
                        Database = InitialCatalog,
                        UserID = UserID,
                        Password = Password
                    };
            }
            return ConnectionStringBuilder != null;
        }

        public DbConnection CreateConnection()
        {
            var dbConnection = DbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = ConnectionStringBuilder.ConnectionString;
            return dbConnection;
        }

        private DbProviderFactory DbProviderFactory => DbProviderFactories.GetFactory(providerName);

        public bool IsValid => !string.IsNullOrEmpty(DataSource) &&
                               !string.IsNullOrEmpty(InitialCatalog) &&
                               !string.IsNullOrEmpty(OutputFolder);
    }
}