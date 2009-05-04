//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest
{
    [ApplyNoPolicies]
    class CriticalFakeDal : MarshalByRefObject
    {
        private bool throwException;
        private double balance = 0.0;

        public bool ThrowException
        {
            get { return throwException; }
            set { throwException = value; }
        }

        public double Balance
        {
            get { return balance; }
            set { balance = value; }
        }

        public int DoSomething(string x)
        {
            if (throwException)
                throw new InvalidOperationException("Catastrophic");
            return 42;
        }

        public string SomethingCritical()
        {
            return "Don't intercept me";
        }
    }
}
