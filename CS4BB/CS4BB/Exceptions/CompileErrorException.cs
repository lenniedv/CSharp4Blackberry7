using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS4BB.Exceptions
{
    public sealed class CompileErrorException: Exception
    {
        public CompileErrorException(string aMessage) : base(aMessage)
        {}
    }
}
