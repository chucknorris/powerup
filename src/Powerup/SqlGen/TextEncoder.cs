using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Powerup.SqlGen
{
    internal class TextEncoder : HandlebarsDotNet.ITextEncoder
    {
        public string Encode(string value)
        {
            return value;
        }
    }
}