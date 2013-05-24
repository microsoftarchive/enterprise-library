//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Console.Wpf.Tests.VSTS.Mocks;

namespace Console.Wpf.Tests.VSTS.DevTests.given_type_name_parser
{
    [TestClass]
    public class when_parsing_generic_type_name : ContainerContext
    {
        string typeToParse = "System.Collections.Generic.Dictionary`2[[Console.Wpf.Tests.VSTS.DevTests.given_type_name_parser.when_parsing_generic_type_name, Console.Wpf.Tests.VSTS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[Console.Wpf.Tests.VSTS.DevTests.Contexts.ContainerContext, Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]";
        string parsedName;

        protected override void Act()
        {
            parsedName = TypeNameParserHelper.ParseTypeName(typeToParse);
        }

        [TestMethod]
        public void then_type_generic_name_is_parsed_correctly()
        {
            Assert.AreEqual("Dictionary<when_parsing_generic_type_name, ContainerContext>", parsedName);
        }
    }

    [TestClass]
    public class when_parsing_type_name : ContainerContext
    {
        string typeToParse = "Microsoft.Practices.EnterpriseLibrary.Caching.Cache";
        string parsedName;

        protected override void Act()
        {
            parsedName = TypeNameParserHelper.ParseTypeName(typeToParse);
        }

        [TestMethod]
        public void then_type_name_is_parsed_correctly()
        {
            Assert.AreEqual("Cache", parsedName);
        }
    }
}
