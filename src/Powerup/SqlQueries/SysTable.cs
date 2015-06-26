namespace Powerup.SqlQueries
{
    public class SysTable
    {
        public string Name { get; set; }

        public string Schema { get; set; }

        public override sealed string ToString()
        {
            return this.Name ?? base.ToString();
        }
    }
}