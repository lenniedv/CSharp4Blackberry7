using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS4BB.lang;
using System.IO;
using CS4BB.PreValidation;

namespace CS4BB
{
    class Generator
    {
        private SourceCode sourceCode;
        private bool displayProgress;
        private List<String> errors = new List<String>();
        private string directoryName;
        private bool writeJavaCode;
        private List<String> allCode = new List<String>();

        public bool UnitTestMode { get; set; }

        public Generator(SourceCode sourceCode)
        {
            this.sourceCode = sourceCode;
            this.displayProgress = false;
            this.writeJavaCode = false;
            this.UnitTestMode = false;
        }

        public Generator(string aDirectoryName, SourceCode aSourceCode, bool aDisplayProgress)
        {
            this.directoryName = aDirectoryName;
            this.sourceCode = aSourceCode;
            this.displayProgress = aDisplayProgress;
            this.writeJavaCode = true;
            this.UnitTestMode = false;
        }

        public Generator(List<String> aSourceCodeForTesting, bool aUnitTestMode)
        {
            this.sourceCode = new SourceCode(aSourceCodeForTesting);
            this.displayProgress = false;
            this.writeJavaCode = false;
            this.UnitTestMode = aUnitTestMode;
        }
        
        /// <summary>
        /// Start the compile process
        /// </summary>
        public void Run()
        {
            if (!UnitTestMode) 
                RunPreValidation();
            
            if (!HasErrors())
                RunCompile();
            else if (displayProgress)
                Console.WriteLine("Can't continue with compilation due to validation error.");
        }

        private void RunPreValidation()
        {
            if (displayProgress)
                Console.WriteLine("Start pre-validation... File: {0}", this.sourceCode.GetFileName());

            IValidate onlyOneClass = new OnlySingleClassAndInterfacePerFile();
            String result = onlyOneClass.DoValidation(this.sourceCode);
            if (result != null) errors.Add(result);

            result = null;
            IValidate noIndexers = new NoCSharpIndexers();
            result = noIndexers.DoValidation(this.sourceCode);
            if (result != null) errors.Add(result);
        }

        private void RunCompile()
        {
            if (displayProgress)
                Console.WriteLine("Start compiling... File: {0}", this.sourceCode.GetFileName());

            int pos = 1;
            
            foreach (String currentSourceCodeLine in this.sourceCode.GetLines())
            {
                TargetCodeResult targetCode = new TargetCodeResult(currentSourceCodeLine);

                // TODO: For now we list the commands here, much get a better idea to handle (??) load it from a list (??)

                ICommand usingDirective = new UsingDirectiveComp();
                ICommand namespaceComp = new NamespaceComp();
                ICommand classDef = new ClassDefinitionComp();
                ICommand openStatementBlock = new OpenStatementBlockComp();
                ICommand mainMethod = new MainMethodComp();
                ICommand methodDef = new MethodDefinitionComp();
                ICommand closeStatementBlock = new CloseStatementBlockComp();
                ICommand keywords = new KeywordsComp();
                ICommand autoProperty = new AutoPropertiesComp();

                if (usingDirective.Identify(this.sourceCode, currentSourceCodeLine, pos))
                    targetCode = usingDirective.Compile(this.sourceCode, currentSourceCodeLine, pos);

                if (namespaceComp.Identify(this.sourceCode, targetCode.GetCurrentCode(), pos))
                    targetCode = namespaceComp.Compile(this.sourceCode, targetCode.GetCurrentCode(), pos);

                if (classDef.Identify(this.sourceCode, targetCode.GetCurrentCode(), pos))
                    targetCode = classDef.Compile(this.sourceCode, targetCode.GetCurrentCode(), pos);

                if (openStatementBlock.Identify(this.sourceCode, targetCode.GetCurrentCode(), pos))
                    targetCode = openStatementBlock.Compile(this.sourceCode, targetCode.GetCurrentCode(), pos);

                if (mainMethod.Identify(this.sourceCode, targetCode.GetCurrentCode(), pos))
                    targetCode = mainMethod.Compile(this.sourceCode, targetCode.GetCurrentCode(), pos);

                if (methodDef.Identify(this.sourceCode, targetCode.GetCurrentCode(), pos))
                    targetCode = methodDef.Compile(this.sourceCode, targetCode.GetCurrentCode(), pos);

                if (closeStatementBlock.Identify(this.sourceCode, targetCode.GetCurrentCode(), pos))
                    targetCode = closeStatementBlock.Compile(this.sourceCode, targetCode.GetCurrentCode(), pos);

                if (keywords.Identify(this.sourceCode, targetCode.GetCurrentCode(), pos))
                    targetCode = keywords.Compile(this.sourceCode, targetCode.GetCurrentCode(), pos);

                if (autoProperty.Identify(this.sourceCode, targetCode.GetCurrentCode(), pos))
                    targetCode = autoProperty.Compile(this.sourceCode, targetCode.GetCurrentCode(), pos);

                // TODO: Add additional commands here

                if (!targetCode.Success)
                    this.errors.Add(targetCode.ErrorMessage);

                if (UnitTestMode)
                    this.allCode.Add(targetCode.GetCurrentCode());
                
                if (writeJavaCode && targetCode.IsValidCode())
                    WriteJavaLine(targetCode);

                pos++;
            }
        }

        /// <summary>
        /// Indicate if there are any compile errors
        /// </summary>
        /// <returns></returns>
        public bool HasErrors()
        {
            return this.errors != null && this.errors.Count > 0;
        }

        /// <summary>
        /// Return compile errors
        /// </summary>
        /// <returns></returns>
        public List<String> GetErrors()
        {
            return this.errors;
        }

        /// <summary>
        /// Return all the code back as a string
        /// </summary>
        /// <returns></returns>
        public String GetAllCode()
        {
            StringBuilder result = new StringBuilder();
            
            foreach (String code in this.allCode)
                result.Append(code);

            return result.ToString();
        }

        private void WriteJavaLine(TargetCodeResult currentLineResult)
        {
            StreamWriter file = null;
            try
            {
                String javaFile = this.directoryName + @"/" + this.sourceCode.GetJavaDestinationFileName();
                file = new StreamWriter(javaFile, true);
                String[] breakUpLines = currentLineResult.GetCurrentCode().Split('\n');
                if (breakUpLines.Length > 1)
                {
                    for (int i = 0; i < breakUpLines.Length; i++)
                        file.WriteLine(breakUpLines[i]);
                }
                else
                    file.WriteLine(currentLineResult.GetCurrentCode());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (file != null)
                    file.Close();
            }
        }
    }
}
