using System;
using NUnit.Framework;
using Powerup.SqlObjects;
using Powerup.SqlQueries;
using Powerup.Templates;

namespace Powerup.Tests
{
    [TestFixture]
    public class TemplateTests
    {

        [Test]
        public void should_template_my_sproc()
        {
            var createText = "CREATE         PROCEDURE [dbo].[Procy]   ";


            var sproc = new CreateAlterTemplate(new SqlObject(new ProcedureQuery(),"Hello"));

            sproc.AddText(createText);
            Assert.That(sproc.TemplatedProcedure(), Is.Not.StringContaining(createText));
        }

        [Test]
        public void should_pick_up_procedures_defined_as_PROC()
        {
            var createText = "CREATE    PROC [dbo].[aProceddure]";

            var sproc = new CreateAlterTemplate((new SqlObject(new ProcedureQuery(), "Hello")));

            sproc.AddText(createText);
            Assert.That(sproc.TemplatedProcedure(), Is.Not.StringContaining(createText));
            Console.WriteLine(sproc.TemplatedProcedure());
        }


        [Test]
        public void template_should_leave_temporary_tables_intact()
        {
            var tableText = "CREATE TABLE #r";

            var result = new CreateAlterTemplate(new SqlObject(new ProcedureQuery(), "Test"));
            result.AddText(tableText);
            Assert.That(result.TemplatedProcedure(), Is.StringContaining(tableText));
        }

        [Test]
        public void Enum_to_string()
        {
            Console.WriteLine(SqlType.View);
        }

    }

    
}
