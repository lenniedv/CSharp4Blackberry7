using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS4BB.PreValidation
{
    class NoCSharpIndexers: IValidate
    {
        public string DoValidation(SourceCode aSourceCode)
        {
            String result = null;

            bool foundIndexers = false;
            int pos = 1;
            foreach (String line in aSourceCode.GetLines())
            {
                if (line.IndexOf("this[") > -1 && (line.EndsWith("{") || aSourceCode.GetNextLine(pos).StartsWith("{")))
                    foundIndexers = true;
                pos++;
            }

            if (foundIndexers)
                result = aSourceCode.GetFileName() + ": Java unfortunatly doesn't support C# indexers. ";

            return result;
        }
    }
}
