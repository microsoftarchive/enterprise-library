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
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;

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
        public DefaultDeleteCommandModel(ElementViewModel elementViewModel)
        {
            this.elementViewModel = elementViewModel;
        }

        public override string Title
        {
            get
            {
                return string.Format("Delete {0}", elementViewModel.Name); // todo: move to resource
            }
        }

        [InjectionMethod]
        public void DefaultDeleteCommandModelInitialize(IApplicationModel applicationModel)
        {
            this.applicationModel = applicationModel;
        }

        public override bool CanExecute(object parameter)
        {
            return elementViewModel.GetType() !=  typeof(ElementViewModel);
        }

        public override void Execute(object parameter)
        {
            elementViewModel.Delete();
            applicationModel.SetDirty();
        }

        public override CommandPlacement Placement
        {
            get { return CommandPlacement.ContextDelete; }
        }

        public override string KeyGesture
        {
            get
            {
                return Key.Delete.ToString();
            }
        }
    }
}
