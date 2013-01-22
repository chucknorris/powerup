using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Powerup.SqlObjects;
using Powerup.SqlQueries;

namespace Powerup.Tests
{
    [TestFixture]
    public class BuidlingTheListOfEntitiesToCreate
    {
        IDbInteragator dbInteragator;

        [SetUp]
        public void Setup()
        {
            dbInteragator = new TestDb();
        }

        [Test]
        public void QueryTheDatabaseForAListOfTheEntities()
        {
            var entities = dbInteragator.ShowMeAll("Procedures");

            Assert.That(entities.Where(x => x.SqlType == SqlType.View), Is.Not.EqualTo(0));
        }
    }
     
    public interface IDbInteragator
    {
        IEnumerable<SqlObject> ShowMeAll(string type);
    }

    public class TestDb : IDbInteragator
    {
        public IEnumerable<SqlObject> ShowMeAll(string type)
        {
            return new List<SqlObject> {new SqlObject(new ProcedureQuery(),"GorupLib")
                                            {
                                                Name = "Test", ObjectId = 1, Schema = "Schema"
                                            }};
        }
    }
}