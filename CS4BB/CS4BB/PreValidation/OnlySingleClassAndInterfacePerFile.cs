using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS4BB.lang;

namespace CS4BB.PreValidation
{
    public sealed class OnlySingleClassAndInterfacePerFile: IValidate
    {
        public String DoValidation(SourceCode aSourceCode)
        {
            String result = null;
            
            int totalClasses = CountClassName(aSourceCode);
            int totalInterfaces = CountInterfaces(aSourceCode);

            if (totalClasses == 0 && totalInterfaces == 0)
                result = aSourceCode.GetFileName() + ": A C# file must at least contain a class or an interface. ";

            if (totalClasses == 1 && totalInterfaces != 0)
                result = aSourceCode.GetFileName() + ": A C# file can only have one class or interface just like Java. ";
            else if (totalInterfaces == 1 && totalClasses != 0)
                result = aSourceCode.GetFileName() + ": A C# file can only have one class or interface just like Java. ";

            return result;
        }

        private int CountClassName(SourceCode aSourceCode)
        {
            int result = 0;

            ClassDefinitionComp classDef = new ClassDefinitionComp();

            int pos = 1;
            foreach(String line in aSourceCode.GetLines())
            {
                if (classDef.Identify(aSourceCode, line, pos))
                    result++;
                pos++;
            }
            return result;
        }

        private int CountInterfaces(SourceCode aSourceCode)
        {
            int result = 0;
            int pos = 1;
            foreach(String line in aSourceCode.GetLines())
            {
                if (aSourceCode.ContainKeyword(line, "interface")  && (line.EndsWith("{") || aSourceCode.GetNextLine(pos).StartsWith("{")))
                    result++;
                pos++;
            }
            return result;
        }
    }
}
