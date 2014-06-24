using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS4BB.lang
{
    public sealed class MainMethodComp: ICommand
    {
        public bool Identify(SourceCode aSourceCode, string aCurrentCodeLine, int aLinePosition)
        {
            return IdentifyMainMethod(aSourceCode, aCurrentCodeLine, aLinePosition);
        }

        public static bool IdentifyMainMethod(SourceCode aSourceCode, string aCurrentCodeLine, int aLinePosition)
        {
            bool result = false;
            if (aSourceCode.ContainKeyword(aCurrentCodeLine, "static") && aCurrentCodeLine.IndexOf("Main") > -1  && (aCurrentCodeLine.EndsWith("{") || aSourceCode.GetNextLine(aLinePosition).StartsWith("{")))
                result = true;
            return result;
        }

        public TargetCodeResult Compile(SourceCode aSourceCode, string aCurrentCodeLine, int aLinePosition)
        {
            StringBuilder newLine = new StringBuilder();

            newLine.Append("public static void main(String[] args)");
            if (aCurrentCodeLine.EndsWith("{"))
                newLine.Append(" { ");
            return new TargetCodeResult(newLine.ToString());
        }
    }
}
