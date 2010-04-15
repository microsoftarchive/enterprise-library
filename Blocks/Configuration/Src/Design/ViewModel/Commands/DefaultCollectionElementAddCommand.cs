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
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Adds a <see cref="ConfigurationElement"/> type to a <see cref="ElementCollectionViewModel"/>
    /// </summary>
    public class DefaultCollectionElementAddCommand : CommandModel
    {
        private string helpText;
        private bool helpTextLoaded;
        private readonly CommandPlacement commandPlacement;
        private ElementViewModel addedElementViewModel;
        private IApplicationModel applicationModel;

        ///<summary>
        /// Initializes a new instance of <see cref="DefaultCollectionElementAddCommand"/>.
        ///</summary>
        ///<param name="configurationElementType">The configuration element type to add when executed.</param>
        ///<param name="collection">The collection to add the element to.</param>
        ///<param name="uiService">The user-interface service to use when displaying dialogs and windows to the user.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public DefaultCollectionElementAddCommand(ConfigurationElementType configurationElementType, ElementCollectionViewModel collection, IUIServiceWpf uiService)
            : base(uiService)
        {
            Guard.ArgumentNotNull(configurationElementType, "configurationElementType");

            this.ConfigurationElementType = configurationElementType.ElementType;
            this.ElementCollectionModel = collection;

            commandPlacement = CommandPlacement.ContextAdd;
        }

        ///<summary>
        /// Initializes a new instance of <see cref="DefaultCollectionElementAddCommand"/>.
        ///</summary>
        ///<param name="commandAttribute">The <see cref="CommandAttribute"/> providing context for this command.</param>
        ///<param name="configurationElementType">The configuration element type to add when executed.</param>
        ///<param name="collection">The collection to add the element to.</param>
        ///<param name="uiService">The user-interface service to use when displaying dialogs and windows to the user.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        protected DefaultCollectionElementAddCommand(CommandAttribute commandAttribute, ConfigurationElementType configurationElementType, ElementCollectionViewModel collection, IUIServiceWpf uiService)
            : base(commandAttribute, uiService)
        {
            Guard.ArgumentNotNull(configurationElementType, "configurationElementType");

            this.ConfigurationElementType = configurationElementType.ElementType;
            this.ElementCollectionModel = collection;

            commandPlacement = commandAttribute.CommandPlacement;
        }

        ///<summary>
        /// Additional initialization dependencies for <see cref="DefaultCollectionElementAddCommand"/>.
        ///</summary>
        ///<param name="applicationModel">The design-time application model.</param>
        [InjectionMethod]
        public void DefaultCollectionElementAddCommandInitialization(IApplicationModel applicationModel)
        {
            this.applicationModel = applicationModel;
        }

        ///<summary>
        /// Gets the <see cref="ConfigurationElementType"/> being added to the collection.
        ///</summary>
        public virtual Type ConfigurationElementType { get; private set; }

        /// <summary>
        /// Gets the collection where the new element will be added.
        /// </summary>
        protected ElementCollectionViewModel ElementCollectionModel { get; private set; }

        /// <summary>
        /// Provides the title of the <see cref="CommandModel"/> command.  Typically this will appear as the title to a menu in the configuration tool.
        /// </summary>
        public override string Title
        {
            get
            {
                string baseTitle = base.Title;
                if (string.IsNullOrEmpty(baseTitle))
                    baseTitle = GetDisplayName(ConfigurationElementType);

                return string.Format(CultureInfo.CurrentCulture, Resources.DefaultCollectionElementCommandTitle, baseTitle);
            }
        }

        /// <summary>
        /// The added element view model created during execution of the command.
        /// </summary>
        public ElementViewModel AddedElementViewModel
        {
            get { return addedElementViewModel; }
        }

        ///<summary>
        /// The command's related help text.
        ///</summary>
        public override string HelpText
        {
            get
            {
                if (!helpTextLoaded)
                {
                    helpText = GetHelpText(ConfigurationElementType);
                    helpTextLoaded = true;
                }

                return helpText;
            }
        }

        /// <summary>
        /// Adds a new child element to <see cref="ElementCollectionModel"/>.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        /// </param>
        /// <remarks>
        /// Adds a new element to the collection through <see cref="ElementCollectionViewModel.AddNewCollectionElement"/>
        /// and makes the new element the selected element.
        /// </remarks>
        protected override void InnerExecute(object parameter)
        {
            addedElementViewModel = ElementCollectionModel.AddNewCollectionElement(ConfigurationElementType);
            addedElementViewModel.PropertiesShown = true;
            addedElementViewModel.Select();
            applicationModel.SetDirty();
        }

        ///<summary>
        /// The logical placement of the command.
        ///</summary>
        public override CommandPlacement Placement
        {
            get
            {
                return commandPlacement;
            }
        }


        /// <summary>
        /// When implemented by a child, determines if the command can execute.
        /// </summary>
        /// <returns>
        /// Returns <see langword="true"/> if this command can be executed; otherwise, <see langword="false"/>.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        ///                 </param>
        protected override bool InnerCanExecute(object parameter)
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
