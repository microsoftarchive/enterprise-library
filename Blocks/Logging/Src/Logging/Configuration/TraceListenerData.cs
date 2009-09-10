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
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

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
    public class TraceListenerData : NameTypeConfigurationElement
    {
        private AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        internal const string TraceListenerNameSuffix = "\u200Cimplementation";

        /// <summary>
        /// Name of the property that holds the type for a <see cref="TraceListenerData"/>.
        /// </summary>
        /// <remarks>
        /// This property will hold the type of the object it holds it. However, it's used during the 
        /// deserialization process when the actual type of configuration element to create has to be determined.
        /// </remarks>
        protected internal const string listenerDataTypeProperty = "listenerDataType";

        /// <summary>
        /// Name of the property that holds the <see cref="TraceOptions"/> of a <see cref="TraceListenerData"/>.
        /// </summary>
        protected internal const string traceOutputOptionsProperty = "traceOutputOptions";

        /// <summary>
        /// Name of the property that holds the Filter of a <see cref="TraceListenerData"/>
        /// </summary>
        protected internal const string filterProperty = "filter";

        private static IDictionary<string, string> emptyAttributes = new Dictionary<string, string>(0);

        /// <summary>
        /// Initializes an instance of the <see cref="TraceListenerData"/> class.
        /// </summary>
        public TraceListenerData()
        {
            
        }

        /// <summary>
        /// Initializes an instance of <see cref="TraceListenerData"/> for the given <paramref name="traceListenerType"/>.
        /// </summary>
        /// <param name="traceListenerType">Type of trace listener this element represents.</param>
        public TraceListenerData(Type traceListenerType)
            : base(null, traceListenerType)
        {
            this.ListenerDataType = this.GetType();
        }

        /// <summary>
        /// Initializes an instance of <see cref="TraceListenerData"/> with a name and <see cref="TraceOptions"/> for 
        /// a TraceListenerType.
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        /// <param name="traceListenerType">The trace listener type.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        protected TraceListenerData(string name, Type traceListenerType, TraceOptions traceOutputOptions)
            : base(name, traceListenerType)
        {
            this.ListenerDataType = this.GetType();
            this.TraceOutputOptions = traceOutputOptions;
        }

        /// <summary>
        /// Initializes an instance of <see cref="TraceListenerData"/> with a name, a <see cref="TraceOptions"/> for 
        /// a TraceListenerType and a <see cref="SourceLevels"/> for a Filter.
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        /// <param name="traceListenerType">The trace listener type.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter.</param>
        protected TraceListenerData(string name, Type traceListenerType, TraceOptions traceOutputOptions, SourceLevels filter)
            : base(name, traceListenerType)
        {
            this.ListenerDataType = this.GetType();
            this.TraceOutputOptions = traceOutputOptions;
            this.Filter = filter;
        }

        /// <summary>
        /// Gets or sets the type of the actual <see cref="TraceListenerData"/> type.
        /// </summary>
        /// <remarks>
        /// Should match the this.GetType().
        /// </remarks>
        public Type ListenerDataType
        {
            get { return (Type)typeConverter.ConvertFrom(ListenerDataTypeName); }
            set { ListenerDataTypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the actual <see cref="TraceListenerData"/> type.
        /// </summary>
        /// <value>
        /// the fully qualified name of the actual <see cref="TraceListenerData"/> type.
        /// </value>
        [ConfigurationProperty(listenerDataTypeProperty, IsRequired = true)]
        public string ListenerDataTypeName
        {
            get { return (string)this[listenerDataTypeProperty]; }
            set { this[listenerDataTypeProperty] = value; }
        }
        /// <summary>
        /// Gets or sets the <see cref="TraceOptions"/> for the represented <see cref="TraceListener"/>.
        /// </summary>
        [ConfigurationProperty(traceOutputOptionsProperty, IsRequired = false, DefaultValue=TraceOptions.None)]
        public TraceOptions TraceOutputOptions
        {
            get
            {
                return (TraceOptions)this[traceOutputOptionsProperty];
            }
            set
            {
                this[traceOutputOptionsProperty] = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Filter"/> for the represented <see cref="TraceListener"/>
        /// </summary>
        [ConfigurationProperty(filterProperty, IsRequired = false, DefaultValue = SourceLevels.All)]
        public SourceLevels Filter
        {
            get { return (SourceLevels)this[filterProperty]; }
            set { this[filterProperty] = value; }
        }

        /// <summary>
        /// Returns the type <see cref="TypeRegistration"/> entries for this configuration object.
        /// </summary>
        /// <returns>A set of registry entries.</returns>        
        public virtual IEnumerable<TypeRegistration> GetRegistrations()
        {
            yield return GetTraceListenerTypeRegistration();
            yield return GetTraceListenerWrapperTypeRegistration();
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
                    Name = this.WrappedTraceListenerName,
                    Lifetime = TypeRegistrationLifetime.Transient
                };

            return registration;
        }

        private IEnumerable<MemberBinding> GetSharedBindings()
        {
            var bindings = new List<MemberBinding>();
            AddFilterBinding(bindings);
            AddOutputTraceOptionsBinding(bindings);
            AddNameBinding(bindings);
            return bindings;
        }

        private void AddFilterBinding(IList<MemberBinding> bindings)
        {
            if (this.Filter != SourceLevels.All)
            {
                bindings.Add(
                    Expression.Bind(
                        Type.GetMember("Filter")[0],
                        Expression.Constant(new EventTypeFilter(this.Filter))
                        )
                    );
            }
        }

        private void AddOutputTraceOptionsBinding(IList<MemberBinding> bindings)
        {
            bindings.Add(
                Expression.Bind(
                    Type.GetMember("TraceOutputOptions")[0],
                    Expression.Constant(this.TraceOutputOptions)
                    )
                );
        }

        private void AddNameBinding(IList<MemberBinding> bindings)
        {
            bindings.Add(
                Expression.Bind(
                    Type.GetMember("Name")[0],
                    Expression.Constant(this.WrappedTraceListenerName)
                    )
                );
        }

        /// <summary>
        /// Gets the creation expression used to produce a <see cref="TypeRegistration"/> during
        /// <see cref="GetRegistrations"/>.
        /// </summary>
        /// <remarks>
        /// This must be overridden by a subclass, but is not marked as abstract due to configuration serialization needs.
        /// </remarks>
        /// <returns>A <see cref="Expression"/> that creates a <see cref="TraceListener"/></returns>
        protected virtual Expression<Func<TraceListener>> GetCreationExpression()
        {
            throw new NotImplementedException(Logging.Properties.Resources.ExceptionMethodMustBeImplementedBySubclasses);
        }

        /// <summary>
        /// Returns the <see cref="TypeRegistration"/> entry for trace listener wrapper used to support configuration 
        /// updates.
        /// </summary>
        /// <returns>A registry entry.</returns>
        protected TypeRegistration GetTraceListenerWrapperTypeRegistration()
        {
            return
                new TypeRegistration<TraceListener>(() =>
                    new ReconfigurableTraceListenerWrapper(
                        Container.Resolved<TraceListener>(this.WrappedTraceListenerName),
                        Container.Resolved<ILoggingUpdateCoordinator>())
                    {
                        Name = this.Name
                    })
                {
                    Name = this.Name,
                    Lifetime = TypeRegistrationLifetime.Singleton
                };
        }

        /// <summary>
        /// Gets the name to use for the actual trace listener represented by this configuration object.
        /// </summary>
        protected string WrappedTraceListenerName
        {
            get { return this.Name + TraceListenerNameSuffix; }
        }
    }
}
