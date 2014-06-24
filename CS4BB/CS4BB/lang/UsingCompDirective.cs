using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace CS4BB.lang
{
    public sealed class UsingDirectiveComp: ICommand
    {
        public bool Identify(SourceCode aSourceCode, string aCurrentCodeLine, int aLinePosition)
        {
            bool result = false;
            if (aCurrentCodeLine.StartsWith("using") && aCurrentCodeLine.IndexOf("{") == -1 && !aSourceCode.GetNextLine(aLinePosition).StartsWith("{"))
                result = true;

            return result;
        }

        public TargetCodeResult Compile(SourceCode aSourceCode, string aCurrentCodeLine, int aLinePosition)
        {
            TargetCodeResult result = new TargetCodeResult(aCurrentCodeLine);
            String usingNamespace = GetUsingNamespace(aSourceCode, aCurrentCodeLine);

            bool correctLine = IsUsingCorrect(usingNamespace);

            if (!correctLine && (IsSystemNamespace(usingNamespace) || IsProgramNamespace(aSourceCode, usingNamespace)))
                return new TargetCodeResult("");
            
            if (correctLine)
            {
                // TODO: For now we import all the Java classes in that package, we don't worry much about that since the Java compiler will take care of this for us
                // Suggest that we do change this in the future if required
                StringBuilder newLine = new StringBuilder();
                newLine.Append("import ").Append(usingNamespace).Append(".*;"); 
                result = new TargetCodeResult(newLine.ToString());
            }
            else
            {
                StringBuilder newLine = new StringBuilder();
                newLine.Append("//");
                newLine.Append(aCurrentCodeLine);
                newLine.Append("  // Not supported yet");
                result = new TargetCodeResult(newLine.ToString());
                result.LogError(aSourceCode.GetFileName() + ": Using directive not supported yet on line: " + aLinePosition);
            }
            return result;
        }

        private bool IsSystemNamespace(string aUsingNamespace)
        {
            bool result = false;
            if (aUsingNamespace.StartsWith("System") && aUsingNamespace.IndexOf(".") == -1)
                result = true;
            
            return result;
        }

        private bool IsProgramNamespace(SourceCode aSourceCode, string aUsingNamespace)
        {
            var found = (from code in aSourceCode.GetLines()
                         where code.StartsWith("namespace") && aSourceCode.ContainKeyword(code, aUsingNamespace) 
                         select code).FirstOrDefault();
            
            return found != null;
        }

        private bool IsUsingCorrect(string aUsingNamespace)
        {
            bool result = true;
            StreamReader str = new StreamReader(System.Configuration.ConfigurationManager.AppSettings["CSFolder"] + @"/xml/usingDirectives.xml");
            try
            {
                XmlSerializer xSerializer = new XmlSerializer(typeof(UsingDirectives));
                UsingDirectives usings = (UsingDirectives) xSerializer.Deserialize(str);
                if (usings != null)
                {
                    var found = (from u in usings.directive
                                 where aUsingNamespace.IndexOf(u.Name) > -1
                                 select u).FirstOrDefault();
                    
                    if (found == null)
                        result = false;
                }
            }
            finally
            {
                if (str != null)
                    str.Close();
            }
            return result;
        }

        private static String GetUsingNamespace(SourceCode aSourceCode, string aCurrentCodeLine)
        {
            String usingNamespace = aCurrentCodeLine;
            usingNamespace = aSourceCode.ReplaceKeyword(usingNamespace, "using", "").Trim();
            usingNamespace = usingNamespace.Replace(";", "").Trim();
            return usingNamespace;
        }
    }
}
