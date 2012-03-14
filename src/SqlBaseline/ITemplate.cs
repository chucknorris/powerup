namespace SqlBaseline
{
    public interface ITemplate
    {
        string FolderName { get; }
        string FileName { get; }
        string Content { get; }
        string Type { get; }
        void AddText(string text);
    }
}