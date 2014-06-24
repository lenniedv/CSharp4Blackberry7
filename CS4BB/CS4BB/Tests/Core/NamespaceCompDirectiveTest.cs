using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CS4BB.Tests.Core
{
    [TestFixture]
    class NamespaceCompDirectiveTest
    {
        [Test]
        public void Compile()
        {
            List<String> code = new List<String>();
            code.Add("namespace com.lennie.test {");
            Generator gen = new Generator(code, true);
            gen.Run();
            Assert.IsFalse(gen.HasErrors(), "Not suppose to have errors");
            Assert.IsNullOrEmpty(gen.GetAllCode());
            //Assert.AreEqual("package com.lennie.test;", gen.GetAllCode());
        }
    }
}
