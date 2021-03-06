﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS4BB.lang
{
    public sealed class OpenStatementBlockComp: ICommand
    {
        public bool Identify(SourceCode aSourceCode, string aCurrentCodeLine, int aLinePosition)
        {
            bool result = false;

            if (aCurrentCodeLine.StartsWith("{") && aSourceCode.GetPreviousLine(aCurrentCodeLine, aLinePosition).StartsWith("namespace"))
                result = true;

            return result;
        }

        public TargetCodeResult Compile(SourceCode aSourceCode, string aCurrentCodeLine, int aLinePosition)
        {
            return new TargetCodeResult("\n");
        }
    }
}
