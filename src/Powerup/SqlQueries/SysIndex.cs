namespace Powerup.SqlQueries
{
    using System.Collections.Generic;

    public class SysIndex
    {
        public SysIndex()
        {
            this.Columns = new List<SysColumn>();
        }

        public IList<SysColumn> Columns { get; }

        public SysTable Table { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public bool IsUnique { get; set; }

        public override sealed string ToString()
        {
            return this.Name ?? base.ToString();
        }
    }
}