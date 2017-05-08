using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Exceptions
{
    public class SignatureException : Exception
    {
        public SignatureException(string msg) : base(msg)
        {

        }
    }
}
