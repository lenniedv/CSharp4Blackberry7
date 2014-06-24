using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using CS4BB.Exceptions;

namespace CS4BB
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("C# For Blackberry Version " + GeneralUtils.GetVersionNumber());
            Console.WriteLine("GNU Lesser General Public License: http://www.gnu.org/licenses/lgpl.html\n\n");
            bool debugMode = false; // TODO: Read this from application config

            try
            {
                if (!debugMode && args.Length == 0)
                    throw new ArgumentException("Pass the directory location to convert all C# files.");

                String directoryName = GetWorkDirectory(args, debugMode);
                FileInfo[] sourceFileList = GetSourceFiles(directoryName);

                ProgramArguments arguments = new ProgramArguments(args);

                bool compileSuccessful = false;
                CSharpCompile comp = new CSharpCompile(sourceFileList);
                if (comp.Run())
                    compileSuccessful = DoCompile(args, directoryName, sourceFileList, arguments);

                if (compileSuccessful)
                    new BlackberryCompile(directoryName, arguments).Run();

                if (debugMode)
                {
                    Console.WriteLine("In debugging mode, press any key to continue...");
                    Console.ReadLine();
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (CompileErrorException ex)
            {
                Console.WriteLine("\nCompiler halt error, unable to continue because: \n");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Compiler Error: {0}, {1}", ex.Message, ex.StackTrace);
            }
        }

        private static bool DoCompile(string[] aArgs, String aDirectoryName, FileInfo[] aSourceFileList, ProgramArguments aArguments)
        {
            bool result = false;
            Console.WriteLine("Done");
            foreach (FileInfo sourceFile in aSourceFileList)
            {
                SourceCode sourceCode = new SourceCode(sourceFile, aArguments);
                if (!sourceCode.DoesHaveCode())
                    throw new ArgumentException(String.Format("There is no C# source code in the file: {0}", sourceFile.Name));

                if (File.Exists(sourceCode.GetJavaDestinationFullName()))
                    File.Delete(sourceCode.GetJavaDestinationFullName());

                Generator gen = new Generator(aDirectoryName, sourceCode, true);
                gen.Run();

                Console.WriteLine();
                if (gen.HasErrors())
                {
                    Console.WriteLine("\nPlease resolve the following errors first:\n ");

                    foreach (String error in gen.GetErrors())
                        Console.WriteLine("- {0}", error);

                    GeneralUtils.WriteErrorFile(aDirectoryName, gen.GetErrors());
                    Console.WriteLine();
                }
                else
                    result = true;
            }
            return result;
        }

        private static FileInfo[] GetSourceFiles(String aDirectoryName)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(aDirectoryName);
            FileInfo[] sourceFileList = sourceDirectory.GetFiles("*.cs");
            if (sourceFileList == null || sourceFileList.Length == 0)
                new ArgumentException(String.Format("No C# files found to compile in directory: {0}", aDirectoryName));
            return sourceFileList;
        }

        private static String GetWorkDirectory(string[] aArgs, bool aDebugMode)
        {
            String directoryName = aDebugMode ? @"C:\Lennie\Work\CSharpBlackberry\CS4BB\HelloWorldCSDemo" : GeneralUtils.getSourceDirectoryName(aArgs);

            if (directoryName == null || !Directory.Exists(directoryName))
                throw new ArgumentException(String.Format("Directory doesn't exist: {0}", directoryName));
            return directoryName;
        }
    }
}
