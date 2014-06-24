using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CS4BB
{
    class BlackberryCompile
    {
        private string directoryName;
        private ProgramArguments arguments;

        public BlackberryCompile(string directoryName, ProgramArguments aArguments)
        {
            this.directoryName = directoryName;
            this.arguments = aArguments;
        }

        internal void Run()
        {
            Console.WriteLine("Start Blackberry Compiler...\n\n");

            String compilerBin = System.Configuration.ConfigurationManager.AppSettings["BlackberryCompilerBin"];
            String mainLib = System.Configuration.ConfigurationManager.AppSettings["BlackberryCompilerLib"];
            if (!File.Exists(compilerBin + @"\rapc.exe"))
                throw new ArgumentException("Unable to find Blackberry compiler at " + compilerBin);

            if (!File.Exists(mainLib))
                throw new ArgumentException("Unable to find main Blackberry library at " + mainLib);

            String applicationName = GetApplicationName();

            StringBuilder compileCommand = new StringBuilder();
            compileCommand.Append("rapc.exe -quiet ");
            compileCommand.Append("codename=").Append(applicationName).Append(" ");
            compileCommand.Append(createBlackberryConfigurationFile(applicationName)).Append(" ");
            compileCommand.Append("import=").Append(mainLib).Append(" ");

            DirectoryInfo javaDirectory = new DirectoryInfo(directoryName);
            FileInfo[] javaFileList = javaDirectory.GetFiles("*.java");
            foreach (FileInfo currentFile in javaFileList)
                compileCommand.Append(currentFile.FullName.Trim()).Append(" ");

            runCompiler(compileCommand.ToString());
        }

        private void runCompiler(String aCommand)
        {
            StringBuilder compileBatFile = new StringBuilder();
            StreamWriter file = null;
            try
            {
                compileBatFile.Append(this.directoryName).Append(@"\").Append("compile.bat");

                if (File.Exists(compileBatFile.ToString()))
                    File.Delete(compileBatFile.ToString());

                file = new StreamWriter(compileBatFile.ToString(), true);
                file.WriteLine(aCommand);
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

            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(compileBatFile.ToString());
            psi.RedirectStandardOutput = true;
            psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            psi.UseShellExecute = false;
            System.Diagnostics.Process listFiles;
            listFiles = System.Diagnostics.Process.Start(psi);
            System.IO.StreamReader myOutput = listFiles.StandardOutput;
            listFiles.WaitForExit(2000);
        }

        private String createBlackberryConfigurationFile(String aApplicationName)
        {
            // TODO: We will put this into another class and make it more configurable 

            StringBuilder configFileName = new StringBuilder();

            StringBuilder output = new StringBuilder();
            output.Append("MIDlet-Name: ").Append(aApplicationName).Append("\n");
            output.Append("MIDlet-Version: 0.0\n");
            output.Append("MIDlet-Vendor: <unknown>\n");
            output.Append("MIDlet-Jar-URL: ").Append(aApplicationName).Append(".jar").Append("\n");
            output.Append("MIDlet-Jar-Size: 0\n");
            output.Append("MicroEdition-Profile: MIDP-2.0\n");
            output.Append("MicroEdition-Configuration: CLDC-1.1\n");
            output.Append("MIDlet-1: ").Append(GetApplicationTitleAndIcon()).Append("\n");
            output.Append("RIM-MIDlet-Flags-1: 0\n");

            StreamWriter file = null;
            try
            {
                configFileName.Append(this.directoryName).Append(@"\").Append(aApplicationName).Append(".rapc");

                if (File.Exists(configFileName.ToString()))
                    File.Delete(configFileName.ToString());

                file = new StreamWriter(configFileName.ToString(), true);
                String[] breakUpLines = output.ToString().Split('\n');
                if (breakUpLines.Length > 1)
                {
                    for (int i = 0; i < breakUpLines.Length; i++)
                        file.WriteLine(breakUpLines[i]);
                }
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

            return configFileName.ToString();
        }

        private String GetApplicationTitleAndIcon()
        {
            StringBuilder result = new StringBuilder();

            if (this.arguments.ContainProgramArgument("title"))
                result.Append(this.arguments.GetProgramArgumentValue("title"));
            else
                result.Append(",");

            if (this.arguments.ContainProgramArgument("icon"))
                result.Append(",").Append(this.arguments.GetProgramArgumentValue("icon"));
            else
                result.Append(",");

            return result.ToString();
        }

        private String GetApplicationName()
        {
            String result = null;
            
            if (this.arguments.ContainProgramArgument("name"))
                result = this.arguments.GetProgramArgumentValue("name");
            else
                result = "Default";
            
            return result;
        }
    }
}
