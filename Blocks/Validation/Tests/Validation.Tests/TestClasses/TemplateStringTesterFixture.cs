//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    [TestClass]
    public class TemplateToRETranslatorFixture
    {
        readonly string[] replacements = { "replacement0", "replacement1", "replacement2", "replacement3" };

        [TestMethod]
        public void CanConvertTemplateWithoutParameters()
        {
            string template = @"feijf[^epoaijf\]\\]\]";
            string formattedString = string.Format(template, replacements);
            Regex templateRegex = TemplateStringTester.Translate(template);

            Assert.IsTrue(templateRegex.IsMatch(formattedString));
        }

        [TestMethod]
        public void CanConvertTemplateWithNonRepeatingParameters()
        {
            string template = @"feijf{0}[^epoaijf\]{1}\\]\]";
            string formattedString = string.Format(template, replacements);
            Regex templateRegex = TemplateStringTester.Translate(template);

            Assert.IsTrue(templateRegex.IsMatch(formattedString));
        }

        [TestMethod]
        public void CanConvertTemplateWithRepeatingParameters()
        {
            string template = @"feijf{0}[^epoaijf\]{1}\\]{1}\]";
            string formattedString = string.Format(template, replacements);
            Regex templateRegex = TemplateStringTester.Translate(template);

            Assert.IsTrue(templateRegex.IsMatch(formattedString));
        }

        [TestMethod]
        public void ConvertedTemplateWithRepeatingParametersDoesHonorParameterIdentity()
        {
            string template = @"feijf{0}[^epoaijf\]{1}\\]{1}\]";
            string promiscuous = @"feijf{0}[^epoaijf\]{1}\\]{2}\]"; // will use parameter 2 instead of parameter 1
            string formattedString = string.Format(promiscuous, replacements);
            Regex templateRegex = TemplateStringTester.Translate(template);

            Assert.IsFalse(templateRegex.IsMatch(formattedString));
        }

        [TestMethod]
        public void CanExtractParameters()
        {
            string template = @"feijf{0}[^epoaijf\]{3}{1}\\]{1}\]";
            string formattedString = string.Format(template, replacements);
            Match match = TemplateStringTester.Match(template, formattedString);

            Assert.IsTrue(match.Groups["param0"].Success);
            Assert.AreEqual(replacements[0], match.Groups["param0"].Value);
            Assert.IsTrue(match.Groups["param1"].Success);
            Assert.AreEqual(replacements[1], match.Groups["param1"].Value);
            Assert.IsFalse(match.Groups["param2"].Success);
            Assert.IsTrue(match.Groups["param3"].Success);
            Assert.AreEqual(replacements[3], match.Groups["param3"].Value);
        }
    }
}
