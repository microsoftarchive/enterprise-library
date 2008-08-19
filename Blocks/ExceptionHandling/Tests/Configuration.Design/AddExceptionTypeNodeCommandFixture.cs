//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Tests
{
    [TestClass]
    public class AddExceptionTypeNodeCommandFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void AddExceptionTypeNodeChangesNodeName()
        {
            AddExceptionTypeNodeCommandTest addExceptionTypeNodeCommand = new AddExceptionTypeNodeCommandTest(ServiceProvider);

            addExceptionTypeNodeCommand.Execute(ApplicationNode);

            ExceptionTypeNode exceptionTypeNode = (ExceptionTypeNode)Hierarchy.FindNodeByType(ApplicationNode, typeof(ExceptionTypeNode));

            Assert.IsNotNull(exceptionTypeNode);
            Assert.AreEqual("Exception", exceptionTypeNode.Name);
        }

        class AddExceptionTypeNodeCommandTest : AddExceptionTypeNodeCommand
        {
            public AddExceptionTypeNodeCommandTest(IServiceProvider serviceProvider)
                : base(serviceProvider, typeof(ExceptionTypeNode)) {}

            protected override Type SelectedType
            {
                get { return typeof(Exception); }
            }
        }
    }
}