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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Wizard
{
    /// <summary>
    /// Provides a general builder pattern and utility method for locating various elements
    /// in a <see cref="ViewModel"/> hierarchy.
    /// </summary>
    internal abstract class ConfigurationBuilder
    {
        protected ConfigurationSourceModel SourceModel { get; private set; }


        /// <summary>
        /// Creates a new instance of the <see cref="ConfigurationBuilder"/>.
        /// </summary>
        /// <param name="sourceModel">The <see cref="ConfigurationSourceModel"/> the builder works on.</param>
        protected ConfigurationBuilder(ConfigurationSourceModel sourceModel)
        {
            this.SourceModel = sourceModel;
        }

        /// <summary>
        /// Implemented in derived classes to add the configuration pieces into the <see cref="ConfigurationSourceModel"/>.
        /// </summary>
        public abstract void Build();

        /// <summary>
        /// Retrieves a section of the specified type from the <see cref="ConfigurationSourceModel"/>
        /// </summary>
        /// <typeparam name="T">The type of <see cref="System.Configuration.ConfigurationElement"/> the <see cref="SectionViewModel"/> represents.</typeparam>
        /// <returns>The first <see cref="SectionViewModel"/> with a <see cref="ElementViewModel.ConfigurationType"/> matching <typeparamref name="T"/>
        /// or <see langword="null"/></returns>
        protected SectionViewModel GetSectionOfType<T>()
        {
            return SourceModel.Sections
                .Where(s => s.ConfigurationType == typeof(T))
                .FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the <see cref="ElementCollectionViewModel"/> that manages
        /// the configuration elements of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ElementViewModel.ConfigurationType"/> that the collection maintains.</typeparam>
        /// <param name="root">The root <see cref="ElementViewModel"/> to search under.</param>
        /// <returns>The <see cref="ElementCollectionViewModel"/> that manages the type</returns>        
        protected static ElementCollectionViewModel GetCollectionOfType<T>(ElementViewModel root)
        {
            return root.DescendentElements(
                x => typeof(T).IsAssignableFrom(x.ConfigurationType))
                .OfType<ElementCollectionViewModel>().First();
        }
    }
}
