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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Console;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    public interface IApplicationModel
    {
        void SetDirty();

        bool Save();

        bool Save(string configurationFilePath);

        void Load(string configurationFilePath);

        string ConfigurationFilePath { get; }

        bool IsDirty { get; }

        bool EnsureCanSaveConfigurationFile(string configurationFile);

        void OnSelectedElementChanged(ElementViewModel element);

        event EventHandler<SelectedElementChangedEventHandlerArgs> SelectedElementChanged;
    }

    public class SelectedElementChangedEventHandlerArgs : EventArgs
    {
        readonly ElementViewModel selectedElement;
        public SelectedElementChangedEventHandlerArgs(ElementViewModel selectedElement)
        {
            this.selectedElement = selectedElement;
        }

        public ElementViewModel SelectedElement
        {
            get { return selectedElement; }
        }
    }
}
