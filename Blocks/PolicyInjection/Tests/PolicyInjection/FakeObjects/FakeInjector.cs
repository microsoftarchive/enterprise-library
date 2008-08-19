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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.FakeObjects
{
    /// <summary>
    /// An injector class that doesn't actually inject anything.
    /// </summary>
    [ConfigurationElementType(typeof(FakeInjectorData))]
    public class FakeInjector : PolicyInjector
    {
        private int extraValue;

        public override bool TypeSupportsInterception(Type t)
        {
            return true;
        }

        protected override object DoWrap(
            object instance, Type typeToReturn, PolicySet policiesForThisType)
        {
            return instance;
        }

        public int ExtraValue
        {
            get { return extraValue; }
            set { extraValue = value; }
        }
    }
}
