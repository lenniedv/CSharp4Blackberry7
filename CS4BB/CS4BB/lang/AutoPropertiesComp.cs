using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS4BB.lang
{
    public sealed class AutoPropertiesComp: ICommand
    {
        public bool Identify(SourceCode aSourceCode, string aCurrentCodeLine, int aLinePosition)
        {
            bool result = false;
            if (aSourceCode.ContainKeyword(aCurrentCodeLine, "get;") || aSourceCode.ContainKeyword(aCurrentCodeLine, "set;"))
                result = true;

            return result;
        }

        public TargetCodeResult Compile(SourceCode aSourceCode, string aCurrentCodeLine, int aLinePosition)
        {
            TargetCodeResult result = new TargetCodeResult(aCurrentCodeLine);

            int propertyNameLocationInCode = 0;
            
            String propertyName = GetPropertyName(aCurrentCodeLine, out propertyNameLocationInCode);
            String propertyType = GetPropertyType(aCurrentCodeLine, propertyNameLocationInCode);

            String memberVariableName = "_" + propertyName.ToLower();
            String parameterName = "a" + propertyName;

            StringBuilder newLine = new StringBuilder();
            newLine.Append("\n");
            newLine.Append("private ").Append(propertyType).Append(" ").Append(memberVariableName).Append(";\n\n");
            newLine.Append("public").Append(" ").Append(propertyType).Append(" ").Append("get").Append(propertyName).Append("()\n");
            newLine.Append("{\n");
            newLine.Append("return this.").Append(memberVariableName).Append(";\n");
            newLine.Append("}");
            newLine.Append("\n\n");
            newLine.Append("public void ").Append("set").Append(propertyName).Append("(").Append(propertyType).Append(" ").Append(parameterName).Append(")").Append("\n");
            newLine.Append("{\n");
            newLine.Append("this.").Append(memberVariableName).Append(" = ").Append(parameterName).Append(";\n");
            newLine.Append("}\n");

            result = new TargetCodeResult(newLine.ToString());

            return result;
        }

        private string GetPropertyType(string aCode, int aPropertyNameLocationInCode)
        {
            String result = "---ERROR---";
            String[] codeBreakdown = aCode.Split(' ');
            if (codeBreakdown.Length > 1 && aPropertyNameLocationInCode > 0)
            {
                int j = aPropertyNameLocationInCode;
                j--;
                result = codeBreakdown[j].Trim();
            }

            return result;
        }

        private String GetPropertyName(String aCode, out int aLocation)
        {
            aLocation = 0;
            String result = "---ERROR---";
            String[] codeBreakdown = aCode.Split(' ');
            for (int i = 0; i < codeBreakdown.Length; i++)
            {
                if (codeBreakdown[i].StartsWith("{"))
                {
                    int j = i;
                    j--;
                    result = codeBreakdown[j].Trim();
                    aLocation = j;
                }
            }

            return result;
        }
    }
}
