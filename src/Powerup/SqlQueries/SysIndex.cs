namespace Powerup.SqlQueries
{
    using System.Collections.Generic;

    public class SysIndex
    {
        public SysIndex()
        {
            this.Columns = new List<SysIndexColumn>();
        }

        public IList<SysIndexColumn> Columns { get; }

        public SysTable Table { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public bool IsUnique { get; set; }

        public bool HasFilter { get; set; }

        public string FilterDefinition { get; set; }


        public override sealed string ToString()
        {
            return this.Name ?? base.ToString();
        }
    }
}