using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS4BB.PreValidation
{
    public interface IValidate
    {
        String DoValidation(SourceCode aSourceCode); // TODO: We need to support more than one error in the future
    }
}
