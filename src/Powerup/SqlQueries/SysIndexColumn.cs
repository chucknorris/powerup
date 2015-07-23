namespace Powerup.SqlQueries
{
    public class SysIndexColumn
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDescending { get; set; }

        public bool IsIncluded { get; set; }

        public override sealed string ToString()
        {
            return this.Name ?? base.ToString();
        }
    }
}