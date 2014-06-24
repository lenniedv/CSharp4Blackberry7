using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS4BB
{
    public sealed class ProgramArguments
    {
        private Dictionary<string, string> arguments;
        private string[] args;

        public ProgramArguments(string[] aArgs)
        {
            this.arguments = ResolveArguments(aArgs);
        }

        private Dictionary<string, string> ResolveArguments(string[] aArguments)
        {
            if (this.arguments == null)
            {
                this.arguments = new Dictionary<string, string>();
                for (int i = 0; i < aArguments.Length; i++)
                {
                    if (aArguments[i] != null && aArguments[i].StartsWith("-"))
                    {
                        if (aArguments[i].Trim().IndexOf(":") == -1)
                        {
                            String dictKey = aArguments[i].Trim().Remove(0, 1);
                            this.arguments.Add(dictKey, "");
                        }
                        else
                        {
                            String[] st = aArguments[i].Trim().Split(':');
                            String dictKey = st[0].Trim().Remove(0, 1);
                            StringBuilder val = new StringBuilder();
                            for (int j = 1; j < st.Length; j++)
                                val.Append(st[j]);

                            this.arguments.Add(dictKey, val.ToString());
                        }
                    }
                }
            }
            return this.arguments;
        }

        /// <summary>
        /// Indicate if program contain a given argument
        /// </summary>
        /// <param name="aSeachArgument"></param>
        /// <returns></returns>
        public bool ContainProgramArgument(string aSeachArgument)
        {
            bool result = false;

            if (arguments != null && !String.IsNullOrEmpty(aSeachArgument))
                result = this.arguments.ContainsKey(aSeachArgument);

            return result;
        }


        /// <summary>
        /// Get the program parameter value
        /// </summary>
        /// <param name="aSearchKey"></param>
        /// <returns></returns>
        public String GetProgramArgumentValue(string aSearchKey)
        {
            String result = "";
            if (this.arguments != null && this.arguments.ContainsKey(aSearchKey))
                this.arguments.TryGetValue(aSearchKey, out result);

            return result;
        }
    }
}
