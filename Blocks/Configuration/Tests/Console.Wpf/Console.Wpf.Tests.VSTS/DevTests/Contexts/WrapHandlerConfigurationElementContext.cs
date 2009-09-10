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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.Contexts
{
    public abstract class WrapHandlerConfigurationElementContext : ArrangeActAssert
    {
        protected readonly string ExpectedExceptionTypeName = typeof (Exception).AssemblyQualifiedName;
        protected const string ExpectedName = "Wrap handler data";
        protected const string ExpectedExceptionMessage = "Wrapped exception message";

        protected WrapHandlerData HandlerData { get; private set; }

        protected override void Arrange()
        {
            var data = new WrapHandlerData
            {
                Name = ExpectedName,
                ExceptionMessage = ExpectedExceptionMessage,
                WrapExceptionTypeName = ExpectedExceptionTypeName
            };

            HandlerData = data;
        }

    }
}
