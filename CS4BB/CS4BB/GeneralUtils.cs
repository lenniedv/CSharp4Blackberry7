using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CS4BB
{
    public sealed class GeneralUtils
    {
        /// <summary>
        /// Get the command line argument at the given index
        /// </summary>
        /// <param name="aArguments"></param>
        /// <param name="aParameterIndex"></param>
        /// <returns></returns>
        public static string GetParameter(string[] aArguments, int aParameterIndex)
        {
            try
            {
                return aArguments[aParameterIndex].Trim();
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException("Pass the directory location to convert all C# files.");
            }
        }

        /// <summary>
        /// Get the source directory
        /// </summary>
        /// <param name="aArguments"></param>
        /// <returns></returns>
        public static string getSourceDirectoryName(string[] aArguments)
        {
            String result = null;
            for (int i = 0; i < aArguments.Length; i++)
            {
                if (aArguments[i].Trim().StartsWith("-d:"))
                {
                    String[] st = aArguments[i].Trim().Split(':');
                    StringBuilder dir = new StringBuilder();
                    for (int j = 1; j < st.Length; j++)
                        dir.Append(st[j]).Append(j == 1 ? ":" : "");

                    if (Directory.Exists(dir.ToString()))
                        result = dir.ToString();

                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Get the assembly version number
        /// </summary>
        /// <returns></returns>
        public static string GetVersionNumber()
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            AssemblyName myAssemblyName = myAssembly.GetName();
            return myAssemblyName.Version.ToString();
        }

        /// <summary>
        /// Write error to error file
        /// </summary>
        /// <param name="aSourceDirectory"></param>
        /// <param name="aErrors"></param>
        public static void WriteErrorFile(String aSourceDirectory, List<String> aErrors)
        {
            StreamWriter file = null;
            try
            {
                String errorFile = aSourceDirectory + @"/errors.txt";
                if (File.Exists(errorFile))
                    File.Delete(errorFile);

                file = new StreamWriter(errorFile);
                foreach(String error in aErrors)
                  file.WriteLine("- {0}", error);
                
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
