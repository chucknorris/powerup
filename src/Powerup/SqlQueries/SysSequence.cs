namespace Powerup.SqlQueries
{
    public class SysSequence
    {
        public string Name { get; set; }
        public string UserTypeId { get; set; }
        public int StartWith { get; set; }
        public int Increment { get; set; }
        public string MinValue { get; set; }
        public string MaxValue { get; set; }
        public bool Cycle { get; set; }
        public bool Cache { get; set; }

        public string StrCache
        {
            get { return !Cache ? "NO CACHE" : "CACHE"; }
        }
        public string StrCycle
        {
            get { return !Cycle ? "NO CYCLE" : "CYCLE"; }
        }
    }
}
