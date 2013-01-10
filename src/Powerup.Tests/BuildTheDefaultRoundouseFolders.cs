using NUnit.Framework;
using Powerup.SqlObjects;

namespace Powerup.Tests
{
    [TestFixture]
    public class BuildTheDefaultRoundouseFolders
    {
        [Test]
        public void A_roundhouse_folder_should_have_a_path()
        {
            var roundHouseFolder = new RhEntity();
            Assert.AreEqual(roundHouseFolder.GetFolder(SqlType.Procedure), @"db\procs");
        }
    }

    public class RhEntity
    {
        public string GetFolder(SqlType sqlType)
        {
            string folder = @"db\{0}";

            switch (sqlType)
            {
                case SqlType.Procedure:
                    return string.Format(folder,"procs");
                case SqlType.Function:
                    return string.Format(folder, "functions");
                default:
                    return string.Empty;
            }
            
        }
    }
}