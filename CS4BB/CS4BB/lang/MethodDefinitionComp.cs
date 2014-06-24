using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS4BB.lang
{
    public sealed class MethodDefinitionComp: ICommand
    {
        public bool Identify(SourceCode aSourceCode, string aCurrentCodeLine, int aLinePosition)
        {
            // TODO: We need to also cater for C# properties later on

            bool result = false;
            if (aSourceCode.CountTokens(aCurrentCodeLine, '(') == 1 &&
                aSourceCode.CountTokens(aCurrentCodeLine, ')') == 1 &&
                aCurrentCodeLine.IndexOf(aSourceCode.GetFileName()) == -1 &&
                !aSourceCode.ContainKeyword(aCurrentCodeLine, "class") &&
                aCurrentCodeLine.Split(' ').Length > 2 &&
                !MainMethodComp.IdentifyMainMethod(aSourceCode, aCurrentCodeLine, aLinePosition) &&
                (aCurrentCodeLine.EndsWith("{") || aSourceCode.GetNextLine(aLinePosition).StartsWith("{")))
                result = true;

            return result;
        }

        public TargetCodeResult Compile(SourceCode aSourceCode, string aCurrentCodeLine, int aLinePosition)
        {
            String line = aCurrentCodeLine;
            if (aSourceCode.ContainKeyword(aCurrentCodeLine, "override"))
                line = aSourceCode.RemoveKeyword(line, "override");

            if (aSourceCode.ContainKeyword(aCurrentCodeLine, "virtual"))
                line = aSourceCode.RemoveKeyword(line,"virtual");

            if (aSourceCode.ContainKeyword(aCurrentCodeLine, "internal"))
                line = aSourceCode.ReplaceKeyword(line, "internal", "protected");

            if (aSourceCode.ContainKeyword(aCurrentCodeLine, "internal protected"))
                line = aSourceCode.ReplaceKeyword(line, "internal protected", "protected");

            StringBuilder newLine = new StringBuilder();

            newLine.Append(line);
            
            // TODO: We need to b able to indicate exception per method not just for all methods
            //if (aSourceCode.Arguments.ContainProgramArgument("throwexceptions"))
            //newLine.Append(" throws Exception");
            
            return new TargetCodeResult(newLine.ToString());
        }
    }
}
