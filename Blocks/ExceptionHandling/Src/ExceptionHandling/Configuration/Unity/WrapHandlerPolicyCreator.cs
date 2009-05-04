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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Unity
{
    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to create the container policies required to create a <see cref="WrapHandler"/>.
    /// </summary>
    public class WrapHandlerPolicyCreator : IContainerPolicyCreator
    {
        void IContainerPolicyCreator.CreatePolicies(
            IPolicyList policyList,
            string instanceName,
            ConfigurationElement configurationObject,
            IConfigurationSource configurationSource)
        {
            WrapHandlerData castConfigurationObject = (WrapHandlerData)configurationObject;

            new PolicyBuilder<WrapHandler, WrapHandlerData>(
                NamedTypeBuildKey.Make<WrapHandler>(instanceName),
                castConfigurationObject,
                c => new WrapHandler(
                    new ResourceStringResolver(
                        c.ExceptionMessageResourceType,
                        c.ExceptionMessageResourceName,
                        c.ExceptionMessage),
                    c.WrapExceptionType))
                .AddPoliciesToPolicyList(policyList);
        }
    }
}
