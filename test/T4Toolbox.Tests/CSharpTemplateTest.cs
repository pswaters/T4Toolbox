﻿// <copyright file="CSharpTemplateTest.cs" company="Oleg Sych">
//  Copyright © Oleg Sych. All Rights Reserved.
// </copyright>

namespace T4Toolbox.Tests
{
    using System;
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CSharpTemplateTest : IDisposable
    {
        private readonly CSharpTemplate template = new TestCSharpTemplate();

        public void Dispose()
        {
            this.template.Dispose();
        }

        [TestMethod]
        public void FieldNameReturnsCamelCasedIdentifier()
        {
            Assert.AreEqual("testName", this.template.FieldName("Test Name"));
        }

        [TestMethod]
        public void IdentifierRemovesWhiteSpace()
        {
            Assert.AreEqual("TestName", this.template.Identifier(" Test Name "));
        }

        [TestMethod]
        public void IdentifierConvertsReservedKeywordToLiteralIdentifier()
        {
            Assert.AreEqual("@class", this.template.Identifier("class"));
        }

        [TestMethod]
        public void PropertyNameReturnsPascalCasedIdentifier()
        {
            Assert.AreEqual("TestName", this.template.PropertyName("Test Name"));
        }

        [TestMethod]
        public void TransformTextGeneratesFileHeader()
        {
            using (var transformation = new FakeTransformation())
            using (var context = new TransformationContext(transformation, transformation.GenerationEnvironment))
            {
                this.template.Context = context;
                transformation.Host.TemplateFile = Path.GetRandomFileName();
                string output = this.template.TransformText();
                StringAssert.Contains(output, "<autogenerated>");                
                StringAssert.Contains(output, transformation.Host.TemplateFile);
            }
        }

        // CSharpTemplate is abstract class. Need a concrete descendant to test it.
        private class TestCSharpTemplate : CSharpTemplate
        {
        }
    }
}