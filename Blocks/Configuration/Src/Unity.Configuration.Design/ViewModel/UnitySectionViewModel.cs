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
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using System.ComponentModel;
using Microsoft.Practices.Unity.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Unity
{
    public class UnitySectionViewModel : SectionViewModel
    {
        WatchPropertyChangesWithinChangeScope watchAliasElementPropertyChanges;
        readonly ElementLookup lookup;

        public UnitySectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section, ElementLookup lookup)
            : base(builder, sectionName, section)
        {
            this.lookup = lookup;

        }

        public override void Initialize(InitializeContext context)
        {
            base.Initialize(context);

            IElementChangeScope aliasElementChangeScope = lookup.CreateChangeScope(x =>
                                                    typeof(NamespaceElement).IsAssignableFrom(x.ConfigurationType) ||
                                                    typeof(SectionExtensionElement).IsAssignableFrom(x.ConfigurationType) ||
                                                    typeof(AliasElement).IsAssignableFrom(x.ConfigurationType) ||
                                                    typeof(AssemblyElement).IsAssignableFrom(x.ConfigurationType));

            watchAliasElementPropertyChanges = new WatchPropertyChangesWithinChangeScope(aliasElementChangeScope, SignalRegistrationTypeChanges);
        }

        private void SignalRegistrationTypeChanges()
        {
            foreach (RegisterElementViewModel registerElement in lookup.FindInstancesOfConfigurationType(typeof(RegisterElement)))
            {
                registerElement.OnRegistrationTypeChanged();
            }
        }

        protected override object CreateBindable()
        {
            var containerCollection = DescendentElements().Where(x => x.ConfigurationType == typeof(ContainerElementCollection)).First();
            var aliasesCollection = DescendentElements().Where(x=>x.ConfigurationType == typeof(AliasElementCollection)).First();

            return new HorizontalListLayout(
                        new HeaderLayout(containerCollection.Name, containerCollection.AddCommands),
                        new HeaderLayout(DesignResources.RegistrationsHeader),
                        new HeaderLayout(DesignResources.InjectionMembersHeader),
                        new HeaderLayout(DesignResources.InjectionParametersHeader))
                        {
                            Contained = new TwoVerticalsLayout(
                                new ElementListLayout(containerCollection.ChildElements),
                                new HorizontalColumnBindingLayout( 
                                    new HeaderedListLayout(aliasesCollection, aliasesCollection.AddCommands),
                                    0)
                               )
                        };
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (watchAliasElementPropertyChanges != null)
            {
                watchAliasElementPropertyChanges.Dispose();
            }
        }
        
        private class WatchPropertyChangesWithinChangeScope : IDisposable
        {
            readonly Action action;
            readonly IElementChangeScope scope;
            readonly List<WatchForValuePropertyChanges> propertyWatchers;

            public WatchPropertyChangesWithinChangeScope(IElementChangeScope scope, Action action)
            {
                this.action = action;
                this.scope = scope;
                this.scope.CollectionChanged += new NotifyCollectionChangedEventHandler(scope_CollectionChanged);

                propertyWatchers = new List<WatchForValuePropertyChanges>();
                ReCreatePropertyWatchers(this.scope);
            }


            private void ClearPropertyWatchers()
            {
                foreach (var property in propertyWatchers.ToArray())
                {
                    property.Dispose();
                    propertyWatchers.Remove(property);
                }
            }

            private void ReCreatePropertyWatchers(IElementChangeScope elementChangeScope)
            {
                ClearPropertyWatchers();
                foreach (var property in elementChangeScope.SelectMany(x=>x.Properties))
                {
                    propertyWatchers.Add(new WatchForValuePropertyChanges(property, action));
                }
            }

            void scope_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                ReCreatePropertyWatchers(this.scope);
                action();
            }

            #region IDisposable Members

            public void Dispose()
            {
                ClearPropertyWatchers();
            }

            #endregion

            private class WatchForValuePropertyChanges : IDisposable
            {
                readonly Property property;
                readonly PropertyChangedEventHandler handler;
                readonly Action action;

                public WatchForValuePropertyChanges(Property property, Action action)
                {
                    this.handler = new PropertyChangedEventHandler(property_PropertyChanged);
                    this.property = property;
                    this.action = action;

                    this.property.PropertyChanged += handler;
                }

                void property_PropertyChanged(object sender, PropertyChangedEventArgs e)
                {
                    action();   
                }

                public void Dispose()
                {
                    this.property.PropertyChanged -= handler;
                }
            }

        }
    }
}
