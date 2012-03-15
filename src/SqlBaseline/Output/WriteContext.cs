namespace SqlBaseline.Output
{
    public class WriteContext
    {
        private bool succes;
        public bool DidItWork { get { return succes; } }

        public WriteContext(bool state)
        {
            succes = state;
        }

        
    }
}