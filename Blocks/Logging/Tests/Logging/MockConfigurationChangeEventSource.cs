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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests
{
    public class MockConfigurationChangeEventSource : ConfigurationChangeEventSource
    {
        public Dictionary<Type, Delegate> handlers = new Dictionary<Type, Delegate>();

        public override ConfigurationChangeEventSource.ISourceChangeEventSource<TSection> GetSection<TSection>()
        {
            return new SourceChangeEventSource<TSection>(this);
        }

        private class SourceChangeEventSource<T> : ConfigurationChangeEventSource.ISourceChangeEventSource<T>
            where T : ConfigurationSection
        {
            private MockConfigurationChangeEventSource owner;

            public SourceChangeEventSource(MockConfigurationChangeEventSource owner)
            {
                this.owner = owner;
            }

            public event EventHandler<SectionChangedEventArgs<T>> SectionChanged
            {
                add { this.owner.handlers[typeof(T)] = value; }
                remove { this.owner.handlers.Remove(typeof(T)); }
            }
        }

        public void OnSectionChanged<T>(SectionChangedEventArgs<T> sectionChangedEventArgs)
            where T : ConfigurationSection
        {
            Delegate handler = this.handlers[typeof(T)];
            var directHandler = (EventHandler<SectionChangedEventArgs<T>>) handler;
            directHandler(this, sectionChangedEventArgs);
        }
    }
}
