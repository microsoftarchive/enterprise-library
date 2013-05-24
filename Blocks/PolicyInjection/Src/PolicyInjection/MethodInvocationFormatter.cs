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
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.Common
{
    /// <summary>
    /// Represents a formatter object that allows replacement tokens in a string.
    /// The supported tokens are:
    /// <list type="bullet">
    /// <item><term>{appdomain}</term><description>Includes the friendly name of the current application domain.</description></item>
    /// <item><term>{assembly}</term><description>Includes the assembly name.</description></item>
    /// <item><term>{namespace}</term><description>Includes the namespace of the target class.</description></item>
    /// <item><term>{type}</term><description>Includes the name of the type that contains the target method.</description></item>
    /// <item><term>{method}</term><description>Includes the name of the target method.</description></item>
    /// </list>
    /// </summary>
    public class MethodInvocationFormatter : ReplacementFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInvocationFormatter"/> class that replaces tokens
        /// using the information in the given method invocation.
        /// </summary>
        /// <param name="input"><see cref="IMethodInvocation"/> object that contains information
        /// about the current method call.</param>
        public MethodInvocationFormatter(IMethodInvocation input)
        {
            AddRange(new ReplacementToken[]
                         {
                             new ReplacementToken("{appdomain}",
                                                  delegate { return AppDomain.CurrentDomain.FriendlyName; }),
                             new ReplacementToken("{assembly}",
                                                  delegate { return input.MethodBase.DeclaringType.Assembly.FullName; }),
                             new ReplacementToken("{namespace}",
                                                  delegate { return input.MethodBase.DeclaringType.Namespace; }),
                             new ReplacementToken("{type}",
                                                  delegate { return input.MethodBase.DeclaringType.Name; }),
                             new ReplacementToken("{method}",
                                                  delegate { return input.MethodBase.Name; })
                         });
        }
    }
}
