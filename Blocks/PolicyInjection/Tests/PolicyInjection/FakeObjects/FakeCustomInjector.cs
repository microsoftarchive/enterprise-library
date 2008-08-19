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
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.FakeObjects
{
    /// <summary>
    /// An injector that uses the CustomInjectorData configuration element
    /// so we can test the config tool.
    /// </summary>
    [ConfigurationElementType(typeof(CustomInjectorData))]
    public class FakeCustomInjector : PolicyInjector
    {
        public FakeCustomInjector(NameValueCollection configArguments)
        {
        }
        
        /// <summary>
        /// Checks to see if the given type can be intercepted.
        /// </summary>
        /// <param name="t">Type to check.</param>
        /// <returns>True if this type can be intercepted, false if it cannot.</returns>
        public override bool TypeSupportsInterception(Type t)
        {
            return true;
        }

        /// <summary>
        /// Wraps the given instance in a proxy with interception hooked up if it
        /// is required by policy. If not required, returns the unwrapped instance.
        /// </summary>
        /// <param name="instance">object to wrap.</param>
        /// <param name="typeToReturn">Type of the reference to return.</param>
        /// <param name="policiesForThisType">Policy set specific to typeToReturn.</param>
        /// <returns>The object with policy added.</returns>
        protected override object DoWrap(object instance, Type typeToReturn, PolicySet policiesForThisType)
        {
            return instance;
        }
    }
}
