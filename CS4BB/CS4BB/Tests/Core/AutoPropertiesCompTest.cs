using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CS4BB.Tests.Core
{
    [TestFixture]
    class AutoPropertiesCompTest
    {
        [Test]
        public void Compile()
        {
            List<String> code = new List<String>();
            code.Add("public String Name { get; set; }");
            Generator gen = new Generator(code, true);
            gen.Run();
            Assert.IsFalse(gen.HasErrors(), "Not suppose to have errors");
            Assert.AreEqual("\nprivate String _name;\n\npublic String getName()\n{\nreturn this._name;\n}\n\npublic void setName(String aName)\n{\nthis._name = aName;\n}\n", gen.GetAllCode());
        }
    }
}
