using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS4BB.Exceptions;

namespace CS4BB.lang
{
    class ClassDefinitionComp: ICommand
    {
        public bool Identify(SourceCode aSourceCode, string aCurrentCodeLine, int aLinePosition)
        {
            bool result = false;
            if ((aCurrentCodeLine.StartsWith("public") ||
                 aCurrentCodeLine.StartsWith("class") ||
                 aCurrentCodeLine.StartsWith("protected") ||
                 aCurrentCodeLine.StartsWith("private") ||
                 aCurrentCodeLine.StartsWith("sealed") ||
                 aCurrentCodeLine.StartsWith("internal") ||
                 aCurrentCodeLine.StartsWith("iternal protected")) &&
                aSourceCode.ContainKeyword(aCurrentCodeLine, "class") &&
                (aCurrentCodeLine.EndsWith("{") || aSourceCode.GetNextLine(aLinePosition).StartsWith("{")))
                result = true;
            return result;
        }

        public TargetCodeResult Compile(SourceCode aSourceCode, string aCurrentCodeLine, int aLinePosition)
        {
            TargetCodeResult result = new TargetCodeResult(aCurrentCodeLine);

            String superClassName = GetSupperClassName(aCurrentCodeLine);
            if (superClassName != null)
            {

                String classDef = GetClassDefinition(aCurrentCodeLine);

                StringBuilder newLine = new StringBuilder();
                if (classDef.StartsWith("class"))
                    newLine.Append("public ");

                newLine.Append(classDef);

                if (superClassName.IndexOf(",") > -1)
                    newLine.Append(" extends ").Append(GetExtendClass(superClassName)).Append(GetInterfaces(superClassName));
                else
                {
                    if (!superClassName.StartsWith("I"))
                        newLine.Append(" extends ").Append(superClassName);
                    else
                        newLine.Append(" implements ").Append(superClassName);
                }
                result = new TargetCodeResult(newLine.ToString());
            }

            return result;
        }

        private String GetInterfaces(string aSuperClassName)
        {
            StringBuilder result = new StringBuilder();
            String[] st = aSuperClassName.Split(',');
            if (st.Length > 0)
            {
                bool isFirstInterface = true;
                for (int i = 1; i < st.Length; i++)
                {
                    if (st[i].Trim().StartsWith("I"))
                    {
                        if (!isFirstInterface)
                            result.Append(", ");
                        
                        result.Append(st[i].Trim());
                        isFirstInterface = false;
                    }
                }
            }
            return result.Length > 0 ? " implements " + result.ToString() : "";
        }

        private String GetExtendClass(string aSuperClassName)
        {
            String result = "";
            String[] st = aSuperClassName.Split(',');
            if (st.Length > 0)
                result = st[0].Trim();
            return result;
        }

        private String GetClassDefinition(string aCurrentCodeLine)
        {
            String result = null;
            String[] st = aCurrentCodeLine.Split(':');
            if (st.Length > 0)
                result = st[0].Trim();

            return result;
        }

        private string GetSupperClassName(string aCurrentCodeLine)
        {
            String result = null;
            String[] st = aCurrentCodeLine.Split(':');
            if (st.Length > 1)
                result = st[1].Trim();
            
            return result;
        }
    }
}
