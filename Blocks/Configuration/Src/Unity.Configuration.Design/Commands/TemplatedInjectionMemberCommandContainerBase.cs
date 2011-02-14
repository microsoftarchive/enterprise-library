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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Unity;

namespace Microsoft.Practices.Unity.Configuration.Design.Commands
{
    public abstract class TemplatedInjectionMemberCommandContainerBase : CommandModel
    {
        readonly RegisterElementViewModel registerElement;
        readonly InjectionMemberCollectionViewModel collection;
        readonly DefaultCollectionElementAddCommand defaultAddCommand;
        readonly EventHandler<EventArgs> registerElementRegistrationTypeChangedHandler;

        protected TemplatedInjectionMemberCommandContainerBase(CommandAttribute commandAttribute, ElementCollectionViewModel collection, DefaultCollectionElementAddCommand defaultAddCommand, IUIServiceWpf uiService)
            : base(commandAttribute, uiService)
        {
            this.collection = collection as InjectionMemberCollectionViewModel;
            this.registerElement = collection.ParentElement as RegisterElementViewModel;

            this.defaultAddCommand = defaultAddCommand;

            this.registerElementRegistrationTypeChangedHandler = new EventHandler<EventArgs>(registerElement_RegistrationTypeChanged);
            this.registerElement.RegistrationTypeChanged += registerElementRegistrationTypeChangedHandler;
        }

        void registerElement_RegistrationTypeChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("ChildCommands");
        }

        public DefaultCollectionElementAddCommand DefaultAddCommand
        {
            get { return defaultAddCommand; }
        }

        public IEnumerable<TemplatedInjectionMemberCommandBase> TemplateCommands
        {
            get
            {
                if (registerElement.RegistrationType != null)
                {
                    return GetTemplateCommandsForType(registerElement.RegistrationType);
                }
                return Enumerable.Empty<TemplatedInjectionMemberCommandBase>();
            }
        }

        protected abstract IEnumerable<TemplatedInjectionMemberCommandBase> GetTemplateCommandsForType(Type registrationType);

        public override IEnumerable<CommandModel> ChildCommands
        {
            get
            {
                return new CommandModel[] { DefaultAddCommand }.Union(TemplateCommands.Cast<CommandModel>());
            }
        }

        protected override bool InnerCanExecute(object parameter)
        {
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (registerElement != null)
            {
                registerElement.RegistrationTypeChanged -= registerElementRegistrationTypeChangedHandler;
            }
        }
    }
}
