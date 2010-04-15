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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.EnvironmentalOverrides
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// <br/>
    /// Represents a reference to a <see cref="ElementViewModel"/> from within a Delta Configuration file,
    /// Which might or might not be established.
    /// </summary>
    public class EnvironmentOverriddenElementReference : IDisposable
    {
        readonly ElementLookup lookup;
        private readonly EnvironmentalOverridesSection environmentSourceModel;
        private EnvironmentOverriddenElementPayload environmentOverridenElementData;
        private ElementViewModel elementViewModel;
        private ElementReference elementViewModelReference;

        public EnvironmentOverriddenElementReference(ElementLookup lookup, EnvironmentalOverridesSection environmentSourceModel)
        {
            this.lookup = lookup;
            this.environmentSourceModel = environmentSourceModel;
        }

        public void InitializeWithConfigurationElementPayload(EnvironmentOverriddenElementPayload environmentOverriddenElementData)
        {
            if (this.environmentOverridenElementData != null) throw new InvalidOperationException();

            this.environmentOverridenElementData = environmentOverriddenElementData;
            
            InitializeElementReference(environmentOverriddenElementData.ElementPath);
        }

        private void InitializeElementReference(string elementPath)
        {
            elementViewModelReference = lookup.CreateReference(elementPath);
            
            elementViewModelReference.ElementDeleted += ElementViewModelReferenceElementDeleted;
            elementViewModelReference.PathChanged += ElementViewModelReferencePathChanged;

            if (elementViewModelReference.Element != null)
            {
                this.elementViewModel = elementViewModelReference.Element;
            }
            else
            {
                elementViewModelReference.ElementFound += ElementViewModelReferenceElementFound;
            }
        }

        public void InitializeWithElementViewModel(ElementViewModel subject)
        {
            if (elementViewModelReference != null)
            {
                throw new InvalidOperationException();
            }
            elementViewModel = subject;
            subject.Deleted += SubjectDeleted;
            subject.PropertyChanged += SubjectPropertyChanged;
            environmentOverridenElementData = new EnvironmentOverriddenElementPayload(environmentSourceModel, subject.Path);
        }

        void SubjectPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Path")
            {
                environmentOverridenElementData.UpdateElementPath(elementViewModel.Path);
            }
        }

        void SubjectDeleted(object sender, EventArgs e)
        {
            DeletePayload();
        }

        public EnvironmentOverriddenElementPayload EnvironmentOverriddenElementPayload
        {
            get { return environmentOverridenElementData; }
        }

        void ElementViewModelReferencePathChanged(object sender, PropertyValueChangedEventArgs<string> args)
        {
            environmentOverridenElementData.UpdateElementPath(args.Value);
        }

        void ElementViewModelReferenceElementDeleted(object sender, EventArgs e)
        {
            DeletePayload();
        }

        private void DeletePayload()
        {
            environmentOverridenElementData.Delete();
            environmentOverridenElementData = null;

            if (elementViewModelReference != null)
            {
                elementViewModelReference.Dispose();
                elementViewModelReference = null;
            }

            if (elementViewModel != null)
            {
                elementViewModel.PropertyChanged -= SubjectPropertyChanged;
                elementViewModel.Deleted -= SubjectDeleted;
            }
        }

        void ElementViewModelReferenceElementFound(object sender, EventArgs e)
        {
            this.elementViewModel = elementViewModelReference.Element;
        }

        public Guid? ElementId
        {
            get { return elementViewModel == null ? (Guid?) null : elementViewModel.ElementId; }
        }

        #region IDisposable Members

        protected virtual void Dispose(bool disposing)
        {
            if (elementViewModelReference != null)
            {
                elementViewModelReference.ElementFound -= ElementViewModelReferenceElementFound;
                elementViewModelReference.ElementDeleted -= ElementViewModelReferenceElementDeleted;
                elementViewModelReference.PathChanged -= ElementViewModelReferencePathChanged;

                elementViewModelReference.Dispose();
            }

            if (elementViewModel != null)
            {
                elementViewModel.PropertyChanged -= SubjectPropertyChanged;
                elementViewModel.Deleted -= SubjectDeleted;
            }    
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
#pragma warning restore 1591
}
