using Powerup.SqlGen.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerup.SqlGen
{
    internal class SqlGeneratorFactory
    {
        internal ISqlGenerator Create(string providerName)
        {
            if (providerName.ToLowerInvariant().Contains("mysql"))
                return new MySqlGenerator();
            throw new NotImplementedException("Only MySql generation is supported, it should be straightforward to add support to more RDBMS.");
        }
    }
}