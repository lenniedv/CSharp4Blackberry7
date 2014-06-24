using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CS4BB.Tests.Core
{
    [TestFixture]
    class MethodDefinitionCompTest
    {
        [Test]
        public void IdentifyOne()
        {
            List<String> code = new List<String>();
            code.Add("protected String getSurname() {");
            Generator gen = new Generator(code, true);
            gen.Run();
            Assert.IsFalse(gen.HasErrors(), "Not suppose to have errors");
            Assert.AreEqual("protected String getSurname() {", gen.GetAllCode());
        }

        [Test]
        public void IdentifyTwo()
        {
            List<String> code = new List<String>();
            code.Add("void setSurname() {");
            Generator gen = new Generator(code, true);
            gen.Run();
            Assert.IsFalse(gen.HasErrors(), "Not suppose to have errors");
            Assert.AreEqual("void setSurname() {", gen.GetAllCode());
        }

        [Test]
        public void IdentifyThree()
        {
            List<String> code = new List<String>();
            code.Add("virtual void setSurname() {");
            Generator gen = new Generator(code, true);
            gen.Run();
            Assert.IsFalse(gen.HasErrors(), "Not suppose to have errors");
            Assert.AreEqual("void setSurname() { ", gen.GetAllCode());
        }
    }
}
