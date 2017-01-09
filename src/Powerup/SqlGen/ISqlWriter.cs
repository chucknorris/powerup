using DatabaseSchemaReader.DataSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Powerup.SqlGen
{
    internal interface ISqlWriter<T>
    {
        string WriteSql(T obj);
    }
}