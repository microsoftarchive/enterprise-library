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
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Adds a <see cref="ConfigurationElement"/> type to a <see cref="ElementCollectionViewModel"/>
    /// </summary>
    public class DefaultCollectionElementAddCommand : CommandModel
    {
        private readonly string helpText;
        private readonly CommandPlacement commandPlacement;
        private ElementViewModel addedElementViewModel;
        private IApplicationModel applicationModel;

        public DefaultCollectionElementAddCommand(ConfigurationElementType configurationElementType, ElementCollectionViewModel collection)
        {
            this.ConfigurationElementType = configurationElementType.ElementType;
            this.ElementCollectionModel = collection;

            helpText = GetHelpText(ConfigurationElementType);
            commandPlacement = CommandPlacement.ContextAdd;
        }

        protected DefaultCollectionElementAddCommand(CommandAttribute commandAttribute, ConfigurationElementType configurationElementType, ElementCollectionViewModel collection)
            :base(commandAttribute)
        {
            this.ConfigurationElementType = configurationElementType.ElementType;
            this.ElementCollectionModel = collection;

            helpText = GetHelpText(ConfigurationElementType);
            commandPlacement = commandAttribute.CommandPlacement;
        }

        [InjectionMethod]
        public void DefaultCollectionElementAddCommandInitialization(IApplicationModel applicationModel)
        {
            this.applicationModel = applicationModel;
        }

        public virtual Type ConfigurationElementType { get; private set; }
        protected ElementCollectionViewModel ElementCollectionModel { get; private set; }

        public override string Title
        {
            get
            {
                string baseTitle = base.Title;
                if (string.IsNullOrEmpty(baseTitle)) 
                    baseTitle = GetDisplayName(ConfigurationElementType);  

                return string.Format("Add {0}", baseTitle); // todo: move to resource
            }
        }

        public ElementViewModel AddedElementViewModel
        {
            get { return addedElementViewModel; }
        }

        public override string  HelpText
        {
            get { return helpText; }
        }

        public override void Execute(object parameter)
        {
            addedElementViewModel = ElementCollectionModel.AddNewCollectionElement(ConfigurationElementType);
            addedElementViewModel.PropertiesShown = true;
            addedElementViewModel.Select();
            applicationModel.SetDirty();
        }

        public override CommandPlacement Placement
        {
            get
            {
                return commandPlacement;
            }
        }


        public override bool CanExecute(object parameter)
        {
            return true;
        }

        private static string GetDisplayName(Type configurationElementType)
        {
            return GetStringFromAttribute<DisplayNameAttribute>(configurationElementType, attr => attr.DisplayName, configurationElementType.Name);
        }

        private static string GetHelpText(Type configurationElementType)
        {
            return GetStringFromAttribute<DescriptionAttribute>(configurationElementType, attr => attr.Description);
        }

        private static string GetStringFromAttribute<TAttribute>(Type configurationElementType, Func<TAttribute, string> stringRetriever)
            where TAttribute : Attribute
        {
            return GetStringFromAttribute<TAttribute>(configurationElementType, stringRetriever, string.Empty);
        }

        private static string GetStringFromAttribute<TAttribute>(Type configurationElementType, Func<TAttribute, string> stringRetriever, string defaultValue)
            where TAttribute : Attribute
        {
            var attr = TypeDescriptor.GetAttributes(configurationElementType).OfType<TAttribute>().FirstOrDefault();

            if (attr == null)
            {
                return defaultValue;
            }

            return stringRetriever(attr);
        }
    }
}
