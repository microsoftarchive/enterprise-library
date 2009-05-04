//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Formatters
{
    [TestClass]
    public class ExceptionFormatterFixture
    {
        [TestMethod]
        public void SkippedNonReadableProperty()
        {
            ExceptionFormatter formatter = new ExceptionFormatter();

            Exception nonReadablePropertyException = new ExceptionWithNonReadableProperty("MyException");
            
            string message = formatter.GetMessage(nonReadablePropertyException);
            
            Assert.IsTrue(message.Length > 0);
        }

    }

    internal class ExceptionWithNonReadableProperty : Exception
    {
        public ExceptionWithNonReadableProperty(string message)
            : base(message)
        { }

        public string NonReadableProperty
        {
            set { ; }
        }
    }
}
