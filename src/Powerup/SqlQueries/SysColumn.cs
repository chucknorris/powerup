namespace Powerup.SqlQueries
{
    public class SysColumn
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override sealed string ToString()
        {
            return this.Name ?? base.ToString();
        }
    }
}