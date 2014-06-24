using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CS4BB.Tests.Core
{
    [TestFixture]
    class ClassDefinitionCompTest
    {
        [Test]
        public void NormalClass()
        {
            List<String> code = new List<String>();
            code.Add("public class HelloDude {");
            Generator gen = new Generator(code, true);
            gen.Run();
            Assert.IsFalse(gen.HasErrors(), "Not suppose to have errors");
            Assert.AreEqual("public class HelloDude {", gen.GetAllCode());
        }

        [Test]
        public void AddDefaultModifier()
        {
            List<String> code = new List<String>();
            code.Add("class HelloDude: ThisIsMyPlanet {");
            Generator gen = new Generator(code, true);
            gen.Run();
            Assert.IsFalse(gen.HasErrors(), "Not suppose to have errors");
            Assert.AreEqual("public class HelloDude extends ThisIsMyPlanet {", gen.GetAllCode());
        }

        [Test]
        public void ClassWithOneSuperClass()
        {
            List<String> code = new List<String>();
            code.Add("public class HelloDude: MyWorld {");
            Generator gen = new Generator(code, true);
            gen.Run();
            Assert.IsFalse(gen.HasErrors(), "Not suppose to have errors");
            Assert.AreEqual("public class HelloDude extends MyWorld {", gen.GetAllCode());
        }

        [Test]
        public void ImplementOnlyAnInterface()
        {
            List<String> code = new List<String>();
            code.Add("class HelloDude: IComparable {");
            Generator gen = new Generator(code, true);
            gen.Run();
            Assert.IsFalse(gen.HasErrors(), "Not suppose to have errors");
            Assert.AreEqual("public class HelloDude implements IComparable {", gen.GetAllCode());
        }

        [Test]
        public void ImplementInterfaceAndClass()
        {
            List<String> code = new List<String>();
            code.Add("class HelloDude: MyWorld, IComparable {");
            Generator gen = new Generator(code, true);
            gen.Run();
            Assert.IsFalse(gen.HasErrors(), "Not suppose to have errors");
            Assert.AreEqual("public class HelloDude extends MyWorld implements IComparable {", gen.GetAllCode());
        }
    }
}
