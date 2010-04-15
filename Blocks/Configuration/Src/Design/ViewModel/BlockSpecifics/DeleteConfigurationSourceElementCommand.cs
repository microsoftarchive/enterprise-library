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
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{

#pragma warning disable 1591
    /// <summary>
    /// Delete command for <see cref="ConfigurationSourceElement"/> items that ensure
    /// the last element in the collection cannot be deleted.
    /// </summary>
    public class ConfigurationSourceElementDeleteCommand : CommandModel
    {
        ElementViewModel elementViewModel;
        IApplicationModel applicationModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSourceElementDeleteCommand"/> class.
        /// </summary>
        /// <param name="commandAttribute">Attribute defining information about the command.</param>
        /// <param name="elementViewModel">The <see cref="ElementViewModel"/> instance this command applies to.</param>
        /// <param name="uiService">UI Service used to display messages and windows to the user.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public ConfigurationSourceElementDeleteCommand(CommandAttribute commandAttribute, ElementViewModel elementViewModel, IUIServiceWpf uiService)
            : base(commandAttribute, uiService)
        {
            Guard.ArgumentNotNull(elementViewModel, "elementViewModel");

            this.elementViewModel = elementViewModel;
            if (!typeof(ConfigurationSourceElement).IsAssignableFrom(elementViewModel.ConfigurationType))
            {
                throw new ArgumentException(
                    Resources.CommandConfigurationSourceElementDeleteOnWrongType);
            }
        }

        /// <summary>
        /// Provides the title of the <see cref="CommandModel"/> command.  Typically this will appear as the title to a menu in the configuration tool.
        /// </summary>
        public override string Title
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture, Resources.DefaultDeleteElementTitleFormat, elementViewModel.Name);
            }
        }

        /// <summary>
        /// Initializes this command.  This is generally used by the dependency injection contianer.
        /// </summary>
        /// <param name="applicationModel"></param>
        [InjectionMethod]
        public void DefaultDeleteCommandModelInitialize(IApplicationModel applicationModel)
        {
            this.applicationModel = applicationModel;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        ///                 </param>
        protected override bool InnerCanExecute(object parameter)
        {
            var parentColleciton = elementViewModel.ParentElement as ElementCollectionViewModel;
            if (parentColleciton != null)
            {
                return (parentColleciton.ChildElements.Count() > 1);
            }

            return false;
        }

        /// <summary>
        /// Executes the command to delete the element if <see cref="InnerCanExecute"/> is true.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.
        ///                 </param>
        protected override void InnerExecute(object parameter)
        {
            if (InnerCanExecute(null))
            {
                elementViewModel.Delete();
                elementViewModel.Dispose();
                applicationModel.SetDirty();
            }
        }

        ///<summary>
        /// The logical placement of the command.
        ///</summary>
        public override CommandPlacement Placement
        {
            get { return CommandPlacement.ContextDelete; }
        }

        /// <summary>
        /// Defines the key gesture used for this command.
        /// </summary>
        public override string KeyGesture
        {
            get
            {
                return Key.Delete.ToString();
            }
        }
    }

#pragma warning restore 1591
}
