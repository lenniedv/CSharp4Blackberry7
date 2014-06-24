using System.IO;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System;

namespace CS4BB
{
    class CSharpCompile
    {
        private FileInfo[] sourceFileList;
        
        public CSharpCompile(FileInfo[] aSourceFileList)
        {
            this.sourceFileList = aSourceFileList;
        }

        public bool Run()
        {
            bool result = true;

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            List<string> compSourceFileList = new List<string>();
            foreach (FileInfo sourceFile in sourceFileList)
                compSourceFileList.Add(sourceFile.FullName);

            Console.Write("Compiling... ");

            CompilerResults cr = CompileAll(provider, compSourceFileList);
            if (cr.Errors.Count > 0)
            {
                Console.WriteLine("\n\nThe following compile error occured: \n\n");
                foreach (CompilerError ce in cr.Errors)
                    Console.WriteLine("- {0}", ce.ToString());
                result = false;
            }

            return result;
        }

        private CompilerResults CompileAll(CodeDomProvider provider, List<string> sourceFile)
        {
            String lib = System.Configuration.ConfigurationManager.AppSettings["CSFolder"] + @"/CS4BBLib.dll";
            if (!File.Exists(lib))
                throw new ArgumentException("Unable to find '" + lib + "' Please make sure that the library exists. ");

            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add(lib);
            
            cp.TreatWarningsAsErrors = true;
            
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile.ToArray());
            return cr;
        }
    }
}
