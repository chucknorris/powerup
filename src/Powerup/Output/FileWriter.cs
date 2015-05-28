using System;
using System.IO;
using System.Linq;
using Powerup.Templates;

namespace Powerup.Output
{
    public class FileWriter
    {
        private ITemplate sqlArtifact;
        private string path;

        public FileWriter(ITemplate sqlArtifact, string path)
        {
            this.sqlArtifact = sqlArtifact;
            this.path = path;
        }

        public WriteContext DoWrite()
        {
            DoFolder();
            WriteFile();
            return new WriteContext(true);
        }

        private void DoFolder()
        {
            var outputPath = Path.Combine(path, sqlArtifact.FolderName);
            if (Directory.Exists(outputPath))
            {

                return;

            }

            Console.WriteLine("** Creating directory {0} **", outputPath);
            Directory.CreateDirectory(outputPath);
        }

        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        private void WriteFile()
        {
            using (var sw = File.CreateText(Path.Combine(path, sqlArtifact.FolderName, CleanFileName(sqlArtifact.FileName))))
            {
                sw.Write(sqlArtifact.Content);
                Console.WriteLine("Creating file {0}\t\t[{1}]", sqlArtifact.FileName, sqlArtifact.Type);
            }
        } 
    }
}