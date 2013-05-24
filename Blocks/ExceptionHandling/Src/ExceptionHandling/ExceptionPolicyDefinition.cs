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
using System.Linq;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Represents a policy for handling exceptions.
    /// </summary>
    public class ExceptionPolicyDefinition
    {
        private IDictionary<Type, ExceptionPolicyEntry> policyEntries;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionPolicyDefinition"/> class with the policy name and a set of policy entries.
        /// </summary>
        /// <param name="policyName">The policy name.</param>
        /// <param name="policyEntries">A set of <see cref="ExceptionPolicyEntry"/> objects.</param>
        public ExceptionPolicyDefinition(string policyName, IEnumerable<ExceptionPolicyEntry> policyEntries)
            : this(policyName, policyEntries.ToDictionary(e => e.ExceptionType))
        {
        }

        /// 
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionPolicyDefinition"/> class with the policy name and a dictionary of policy entries.
        /// </summary>
        /// <param name="policyName">The policy name.</param>
        /// <param name="policyEntries">A set of <see cref="ExceptionPolicyEntry"/> objects.</param>
        public ExceptionPolicyDefinition(string policyName, IDictionary<Type, ExceptionPolicyEntry> policyEntries)
        {
            if (policyEntries == null) throw new ArgumentNullException("policyEntries");
            if (string.IsNullOrEmpty(policyName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "policyName");

            this.policyEntries = policyEntries;
            this.PolicyName = policyName;

            InjectPolicyNameIntoEntries();
        }

        /// <summary>
        /// Checks if there is a policy entry that matches
        /// the type of the exception object specified by the
        /// <see cref="Exception"/> parameter,
        /// and if so, invokes the handlers associated with that entry.
        /// </summary>
        /// <param name="exceptionToHandle">The <c>Exception</c> to handle.</param>
        /// <returns><see langword="true"/> if  rethrowing an exception is recommended; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// The algorithm for matching the exception object to a 
        /// set of handlers mimics that of a standard .NET Framework exception policy.
        /// The specified exception object will be matched to a single 
        /// exception policy entry by traversing its inheritance hierarchy. 
        /// This means that if a <c>FileNotFoundException</c>, for example, is 
        /// caught, but the only exception type that the exception policy 
        /// knows how to handle is System.Exception, the event handlers 
        /// for <c>System.Exception</c> will be invoked because 
        /// <c>FileNotFoundException</c> ultimately derives from <c>System.Exception</c>.
        /// </remarks>
        public bool HandleException(Exception exceptionToHandle)
        {
            if (exceptionToHandle == null) throw new ArgumentNullException("exceptionToHandle");

            ExceptionPolicyEntry entry = GetPolicyEntry(exceptionToHandle);

            if (entry == null)
            {
                return true;
            }

            return entry.Handle(exceptionToHandle);
        }

        private ExceptionPolicyEntry GetPolicyEntry(Exception ex)
        {
            Type exceptionType = ex.GetType();
            ExceptionPolicyEntry entry = this.FindExceptionPolicyEntry(exceptionType);
            return entry;
        }


        /// <summary>
        /// Gets the policy entry associated with the specified key.
        /// </summary>
        /// <param name="exceptionType">Type of the exception.</param>
        /// <returns>The <see cref="ExceptionPolicyEntry"/> corresponding to this exception type.</returns>
        public ExceptionPolicyEntry GetPolicyEntry(Type exceptionType)
        {
            if (policyEntries.ContainsKey(exceptionType))
            {
                return policyEntries[exceptionType];
            }
            return null;
        }

        /// <devDoc>
        /// Traverses the specified type's inheritance hiearchy.
        /// </devDoc>
        private ExceptionPolicyEntry FindExceptionPolicyEntry(Type exceptionType)
        {
            ExceptionPolicyEntry entry = null;

            while (exceptionType != typeof(Object))
            {
                entry = GetPolicyEntry(exceptionType);

                if (entry == null)
                {
                    exceptionType = exceptionType.BaseType;
                }
                else
                {
                    //we've found the handlers, now continue on
                    break;
                }
            }

            return entry;
        }

        private void InjectPolicyNameIntoEntries()
        {
            foreach (ExceptionPolicyEntry entry in policyEntries.Values)
            {
                entry.PolicyName = PolicyName;
            }
        }

        /// <summary>
        /// Name of this exception policy.
        /// </summary>
        public string PolicyName { get; private set; }
    }
}
