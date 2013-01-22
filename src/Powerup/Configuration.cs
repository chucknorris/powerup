namespace Powerup
{
    public class Configuration
    {
        public string ServerName { get; internal set; }
        public string DatabaseName { get; internal set; }
        public string OutputFolder { get; internal set; }

        public string DatabaseConnection { get { return string.Format("Data Source={0};Initial Catalog={1};Integrated security=true", ServerName, DatabaseName); }  }

        public bool IsValid
        {
            get { return !string.IsNullOrEmpty(ServerName) &&
                            !string.IsNullOrEmpty(DatabaseName) && 
                            !string.IsNullOrEmpty(OutputFolder); }
        }
    }
}