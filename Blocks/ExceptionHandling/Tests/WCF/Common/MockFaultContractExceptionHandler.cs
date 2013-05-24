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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    [ConfigurationElementType(typeof(MockFaultContractExceptionHandlerData))]
    public class MockFaultContractExceptionHandler : IExceptionHandler
    {
        public Exception HandledException;

        #region IExceptionHandler Members

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            this.HandledException = exception;
            return new FaultContractWrapperException(new MockFaultContract(exception.Message), handlingInstanceId);
        }

        #endregion
    }

    public class MockFaultContractExceptionHandlerData : ExceptionHandlerData
    {
        public MockFaultContractExceptionHandlerData()
        {
        }

        public MockFaultContractExceptionHandlerData(string name)
            : base(name, typeof(FaultContractExceptionHandler))
        {
        }

        public override IExceptionHandler BuildExceptionHandler()
        {
            return new MockFaultContractExceptionHandler();
        }
    }

}
