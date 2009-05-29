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
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// The primary implementation of <see cref="ConfigurationChangeEventSource"/>.
    /// </summary>
    public class ConfigurationChangeEventSourceImpl : ConfigurationChangeEventSource
    {
        private readonly EventHandlerList sectionChangeHandlers = new EventHandlerList();

        private class SectionChangeEventSource<TSection> : ISourceChangeEventSource<TSection>
            where TSection : ConfigurationSection
        {
            private readonly ConfigurationChangeEventSourceImpl outer;

            public SectionChangeEventSource(ConfigurationChangeEventSourceImpl outer)
            {
                this.outer = outer;
            }

            public event EventHandler<SectionChangedEventArgs<TSection>> SectionChanged
            {
                add
                {
                    lock (outer.sectionChangeHandlers)
                    {
                        outer.sectionChangeHandlers.AddHandler(typeof(TSection), value);
                    }
                }
                remove
                {
                    lock (outer.sectionChangeHandlers)
                    {
                        outer.sectionChangeHandlers.RemoveHandler(typeof(TSection), value);
                    }
                }
            }
        }

        /// <summary>
        /// Used to get an object that lets you register a change handler for a
        /// particular configuration section.
        /// </summary>
        /// <typeparam name="TSection">Type of the configuration section to register against.</typeparam>
        /// <returns>The object that implements the SectionChanged event.</returns>
        public override ISourceChangeEventSource<TSection> GetSection<TSection>()
        {
            return new SectionChangeEventSource<TSection>(this);
        }

        /// <summary>
        /// Used to raise the <see cref="ConfigurationChangeEventSource.SourceChanged"/> event,
        /// supplying the appropriate event args.
        /// </summary>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> that changed.</param>
        /// <param name="container"><see cref="IServiceLocator"/> which has been configured with the
        /// contents of the <paramref name="configurationSource"/>.</param>
        /// <param name="changedSectionNames">Section names in the <paramref name="configurationSource"/>
        /// that have changed, as reported by the configuration source.</param>
        public void ConfigurationSourceChanged(IConfigurationSource configurationSource, 
            IServiceLocator container,
            IEnumerable<string> changedSectionNames)
        {
            // Fire the main "the source has changed" event
            OnSourceChanged(configurationSource, container, changedSectionNames);

            // Fire the individual "section has changed" methods
            foreach(var name in changedSectionNames)
            {
                var section = configurationSource.GetSection(name);
                if(section != null)
                {
                    OnSectionChanged(section, container);
                }
            }

        }

        /// <summary>
        /// Used to raise the <see cref="ConfigurationChangeEventSource.SourceChanged"/> event,
        /// and the associated section changed events.
        /// </summary>
        /// <remarks>This overload is primarily provided for test convenience.</remarks>
        /// <param name="configurationSource">The <see cref="IConfigurationSource"/> that changed.</param>
        /// <param name="container"><see cref="IServiceLocator"/> which has been configured with the
        /// contents of the <paramref name="configurationSource"/>.</param>
        /// <param name="changedSectionNames">Section names in the <paramref name="configurationSource"/>
        /// that have changed, as reported by the configuration source.</param>
        public void ConfigurationSourceChanged(IConfigurationSource configurationSource, 
            IServiceLocator container,
            params string[] changedSectionNames)
        {
            ConfigurationSourceChanged(configurationSource, container, (IEnumerable<string>) changedSectionNames);
        }

        // Helper method trampoline - figure out the appropriate concrete generic
        // method to call based on the type of the configuration section and call it.
        private void OnSectionChanged(ConfigurationSection section, IServiceLocator container)
        {
            var concreteMethod = genericSectionChangedMethod.MakeGenericMethod(section.GetType());
            concreteMethod.Invoke(this, new object[] {section, container});
        }

        // Helper method - looks up the appropriate event handler by type, builds the appropriate event
        // args, and invokes the handlers.
// ReSharper disable UnusedMember.Local - this member is found and invoked via reflection
        private void OnSectionChanged<TSection>(TSection section, IServiceLocator container) where TSection : ConfigurationSection
// ReSharper restore UnusedMember.Local
        {
            var handler =
                (EventHandler<SectionChangedEventArgs<TSection>>) sectionChangeHandlers[typeof (TSection)];

            if(handler != null)
            {
                var eventArgs = new SectionChangedEventArgs<TSection>(section, container);
                handler(this, eventArgs);
            }
        }

        // Stores the generic version of the SectionChanged<T> MethodInfo - just look it up once.
        private static readonly MethodInfo genericSectionChangedMethod = GetGenericSectionChangedMethod();

        // Helper methods to initialize genericSectionChangedMethod field
        private static MethodInfo GetGenericSectionChangedMethod()
        {
            var methods = from methodInfo in GetMethods()
                where methodInfo.Name == "OnSectionChanged" && methodInfo.IsGenericMethodDefinition
                select methodInfo;

            return methods.First();
        }

        private static MethodInfo[] GetMethods()
        {
            return typeof (ConfigurationChangeEventSourceImpl).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}
