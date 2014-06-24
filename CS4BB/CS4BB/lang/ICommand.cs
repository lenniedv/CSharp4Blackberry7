using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS4BB.lang
{
    public interface ICommand
    {
        // TODO: currently we're passing these parameters along the way all the time, see if I can make it easier (cleanup exercise)
        bool Identify(SourceCode aSourceCode, string aCurrentCodeLine, int aLinePosition);
        TargetCodeResult Compile(SourceCode aSourceCode, string aCurrentCodeLine, int aLinePosition);
    }
}
