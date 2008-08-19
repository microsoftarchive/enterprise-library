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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.RemotingInterception
{
    /// <summary>
    /// A class that wraps the inputs of a <see cref="IMethodCallMessage"/> into the
    /// <see cref="IParameterCollection"/> interface.
    /// </summary>
    class RemotingInputParameterCollection : ParameterCollection
    {
        /// <summary>
        /// Constructs a new <see cref="RemotingInputParameterCollection"/> that wraps the
        /// given method call and arguments.
        /// </summary>
        /// <param name="callMessage">The call message.</param>
        /// <param name="arguments">The arguments.</param>
        public RemotingInputParameterCollection(IMethodCallMessage callMessage, object[] arguments) 
            : base(arguments, callMessage.MethodBase.GetParameters(), 
                delegate(ParameterInfo info) { return !info.IsOut; })
        {
        }
    }
}
