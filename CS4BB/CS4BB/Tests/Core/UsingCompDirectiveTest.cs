using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CS4BB.Tests.Core
{
    [TestFixture]
    public class UsingCompDirectiveTest
    {
        [Test]
        public void NotSupportedYet()
        {
            List<String> code = new List<String>();
            code.Add("using System.Linq;");
            Generator gen = new Generator(code, true);
            gen.Run();
            Assert.IsTrue(gen.HasErrors(), "got errors");
            Assert.AreEqual("//using System.Linq;  // Not supported yet", gen.GetAllCode());
        }

        [Test]
        public void Compile()
        {
            List<String> code = new List<String>();
            code.Add("using net.rim.device.api.ui;");
            Generator gen = new Generator(code, true);
            gen.Run();
            Assert.IsFalse(gen.HasErrors(), "Not suppose to have errors");
            Assert.AreEqual("import net.rim.device.api.ui.*;", gen.GetAllCode());
        }
    }
}
