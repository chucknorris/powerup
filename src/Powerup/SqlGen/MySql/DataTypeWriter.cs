using DatabaseSchemaReader.DataSchema;
using System.IO;

namespace Powerup.SqlGen.MySql
{
    internal class DataTypeWriter
    {
        internal static void Write(TextWriter writer, DatabaseArgument arg)
        {
            writer.Write(arg.DatabaseDataType.ToUpperInvariant());
            if (arg.Scale != null && arg.Scale != 0)
                writer.Write($"({arg.Precision}, {arg.Scale})");
            else if (arg.Length != null && !arg.DatabaseDataType.ToLowerInvariant().Contains("text") &&
                     !arg.DatabaseDataType.ToLowerInvariant().Contains("blob"))
                writer.Write($"({arg.Length})");
        }
    }
}