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

using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Default Delete Command implementation. <br/>
    /// Invokes the <see cref="ElementViewModel.Delete()"/> method on execution.
    /// </summary>
    public class DefaultDeleteCommandModel : CommandModel
    {
        ElementViewModel elementViewModel;
        IApplicationModel applicationModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDeleteCommandModel"/> class.
        /// </summary>
        /// <param name="elementViewModel">The <see cref="ElementViewModel"/> instance this command applies to.</param>
        /// <param name="uiService"><see cref="IUIServiceWpf"/> for invoking messages or other windows.</param>
        public DefaultDeleteCommandModel(ElementViewModel elementViewModel, IUIServiceWpf uiService)
            : base(uiService)
        {
            this.elementViewModel = elementViewModel;
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
        /// Initializes the command with expected items, items this depends on
        /// are usually provided by the dependency injection contianer.
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
            return elementViewModel.GetType() != typeof(ElementViewModel);
        }

        /// <summary>
        /// Deletes the element.
        /// </summary>
        /// <param name="parameter">Not used</param>
        protected override void InnerExecute(object parameter)
        {
            elementViewModel.Delete();
            elementViewModel.Dispose();
            applicationModel.SetDirty();
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

        /// <summary>
        /// The application model injected.
        /// </summary>
        protected IApplicationModel ApplicationModel { get { return applicationModel; } }
    }
}
