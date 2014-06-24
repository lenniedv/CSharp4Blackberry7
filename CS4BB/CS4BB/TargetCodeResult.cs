using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS4BB
{
    public sealed class TargetCodeResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string CurrentCode { get; set; }

        /// <summary>
        /// Create a new source target
        /// </summary>
        /// <param name="aCode"></param>
        public TargetCodeResult(string aCode): this()
        {            
            this.CurrentCode = aCode;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TargetCodeResult()
        {
            this.Success = true;
            this.ErrorMessage = null;
        }

        /// <summary>
        /// Log the error
        /// </summary>
        /// <param name="aErrorMessage"></param>
        public void LogError(string aErrorMessage)
        {
            this.ErrorMessage = aErrorMessage;
            this.Success = false;
        }

        /// <summary>
        /// Get the current target Java code line
        /// </summary>
        /// <returns></returns>
        public string GetCurrentCode()
        {
            String result = this.CurrentCode;
            if (result.CompareTo("\n") == 0)
                result = "";
            return result;
        }

        /// <summary>
        /// Check if code is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValidCode()
        {
            return this.CurrentCode != null && this.CurrentCode != null && this.CurrentCode.Length > 0;
        }
    }
}
