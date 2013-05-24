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
using System.Collections.Specialized;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Provides a base extensible class for handler configuration extensions.  This class eases the handling 
    /// of the <see cref="IExceptionConfigurationAddExceptionHandlers"/> that is the typical entry point
    /// for most exception handler's fluent configuration interface.
    /// </summary>
    public abstract class ExceptionHandlerConfigurationExtension : IExceptionConfigurationForExceptionTypeOrPostHandling, IExceptionHandlerExtension
    {
        /// <summary>
        /// Initializes a new instance of the ExceptoinHandlerConfigurationExtensions
        /// </summary>
        /// <param name="context">The context for configuration.</param>
        /// <remarks>
        /// This constructor expects to the find the implementor of <paramref name="context"/> provide
        /// the <see cref="IExceptionConfigurationForExceptionTypeOrPostHandling"/> and <see cref="IExceptionHandlerExtension"/> interfaces.
        /// </remarks>
        protected ExceptionHandlerConfigurationExtension(IExceptionConfigurationAddExceptionHandlers context)
        {
            this.Context = (IExceptionConfigurationForExceptionTypeOrPostHandling)context;
            Debug.Assert(typeof (IExceptionHandlerExtension).IsAssignableFrom(context.GetType()));
        }

        /// <summary>
        /// The context for the extending handler in the fluent interface.  The extension interface
        /// is expected to return this context to enable continuation of configuring ExceptionHandling.
        /// </summary>
        protected IExceptionConfigurationForExceptionTypeOrPostHandling Context { get; private set; }


        /// <summary>
        /// The current exception type being built in the fluent interface.  Inheritors genereally should 
        /// add their <see cref="ExceptionHandlerData"/> to this during construction.
        /// </summary>
        public ExceptionTypeData CurrentExceptionTypeData
        {
            get
            {
                return ((IExceptionHandlerExtension)Context).CurrentExceptionTypeData;
            }
        }

        IExceptionConfigurationForExceptionType IExceptionConfigurationThenDoPostHandlingAction.ThenDoNothing()
        {
            return Context.ThenDoNothing();
        }

        IExceptionConfigurationForExceptionType IExceptionConfigurationThenDoPostHandlingAction.ThenNotifyRethrow()
        {
            return Context.ThenNotifyRethrow();
        }

        IExceptionConfigurationForExceptionType IExceptionConfigurationThenDoPostHandlingAction.ThenThrowNewException()
        {
            return Context.ThenThrowNewException();
        }

        IExceptionConfigurationForExceptionType IExceptionConfigurationGivenPolicyWithName.GivenPolicyWithName(string name)
        {
            return Context.GivenPolicyWithName(name);
        }

    }
}
