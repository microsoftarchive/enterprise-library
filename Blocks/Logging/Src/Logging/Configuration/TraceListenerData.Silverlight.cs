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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration for a <see cref="TraceListener"/>.
    /// </summary>
    /// <remarks>
    /// Since trace listeners are not under our control, the building mechanism can't rely 
    /// on annotations to the trace listeners to determine the concrete <see cref="TraceListenerData"/> subtype 
    /// when deserializing. Because of this, the schema for <see cref="TraceListenerData"/> includes the actual 
    /// type of the instance to build.
    /// </remarks>
    public abstract class TraceListenerData : NamedConfigurationElement
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TraceListenerData"/> class.
        /// </summary>
        protected TraceListenerData()
        {
        }

        /// <summary>
        /// Returns the type <see cref="TypeRegistration"/> entries for this configuration object.
        /// </summary>
        /// <returns>A set of registry entries.</returns>        
        public virtual IEnumerable<TypeRegistration> GetRegistrations()
        {
            yield return GetTraceListenerTypeRegistration();
        }

        /// <summary>
        /// Returns the <see cref="TypeRegistration"/> entry for the actual trace listener represented by this 
        /// configuration object.
        /// </summary>
        /// <returns>A registry entry.</returns>
        protected TypeRegistration GetTraceListenerTypeRegistration()
        {
            IEnumerable<MemberBinding> extraBindings;

            Expression creationExpression = GetCreationExpression().Body;

            NewExpression newExpression = creationExpression as NewExpression;
            if (newExpression != null)
            {
                extraBindings = new MemberBinding[0];
            }
            else
            {
                MemberInitExpression memberInitExpression = creationExpression as MemberInitExpression;
                if (memberInitExpression != null)
                {
                    newExpression = memberInitExpression.NewExpression;
                    extraBindings = memberInitExpression.Bindings;
                }
                else
                {
                    throw new NotSupportedException(Logging.Properties.Resources.ExceptionCreationLinqExpressionMustBeNew);
                }
            }

            MemberInitExpression memberInit =
                LambdaExpression.MemberInit(newExpression, GetSharedBindings().Concat(extraBindings));

            TypeRegistration registration =
                new TypeRegistration<TraceListener>(LambdaExpression.Lambda<Func<TraceListener>>(memberInit))
                {
                    Name = this.Name,
                    Lifetime = TypeRegistrationLifetime.Singleton
                };

            return registration;
        }

        private IEnumerable<MemberBinding> GetSharedBindings()
        {
            var bindings = new List<MemberBinding>();
            AddNameBinding(bindings);
            return bindings;
        }

        private void AddNameBinding(IList<MemberBinding> bindings)
        {
            bindings.Add(
                Expression.Bind(
                    typeof(TraceListener).GetMember("Name")[0],
                    Expression.Constant(this.Name)));
        }

        /// <summary>
        /// Gets the creation expression used to produce a <see cref="TypeRegistration"/> during
        /// <see cref="GetRegistrations"/>.
        /// </summary>
        /// <returns>A <see cref="Expression"/> that creates a <see cref="TraceListener"/></returns>
        protected abstract Expression<Func<TraceListener>> GetCreationExpression();
    }
}
