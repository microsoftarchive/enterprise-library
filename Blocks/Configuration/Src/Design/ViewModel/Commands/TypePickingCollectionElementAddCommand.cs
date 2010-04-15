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
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
    /// <summary>
    /// Collects a type from the user and populates the newly created <see cref="ElementViewModel"/> with type information.
    /// This will attempt to set the Name property and TypeName properties found in the <see cref="ElementViewModel.Properties"/> collection.
    /// </summary>
    public class TypePickingCollectionElementAddCommand : DefaultCollectionElementAddCommand, IServiceProvider
    {
        private readonly string propertyToSet;
        private readonly IAssemblyDiscoveryService discoveryService;

        /// <summary>
        /// Initializes a new <see cref="TypePickingCollectionElementAddCommand"/> for a <see cref="NamedConfigurationElement"/> type and
        /// containing <see cref="ElementCollectionViewModel"/>.  If no property to set is explicitly defined, it will default to the
        /// TypeName property, following the pattern for setting <see cref="Type"/> information for many <see cref="ConfigurationElement"/>s in Enterprise Library.
        /// </summary>
        /// <param name="commandAttribute">Attribute used to define data for the command.</param>
        /// <param name="configurationElementType">The configuration element type.  This is expected to be a <see cref="NamedConfigurationElement"/></param>
        /// <param name="elementCollectionModel">The containing <see cref="ElementCollectionViewModel"/></param>
        /// <param name="uiService">The UI service for displaying windows and messages to the user.</param>
        /// <param name="discoveryService">Service for discover assemblies</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public TypePickingCollectionElementAddCommand(IUIServiceWpf uiService, IAssemblyDiscoveryService discoveryService, TypePickingCommandAttribute commandAttribute, ConfigurationElementType configurationElementType, ElementCollectionViewModel elementCollectionModel)
            : base(commandAttribute, configurationElementType, elementCollectionModel, uiService)
        {
            Guard.ArgumentNotNull(commandAttribute, "commandAttribute");

            this.discoveryService = discoveryService;
            this.propertyToSet = commandAttribute.Property;

            if (propertyToSet == null)
            {
                throw new ArgumentException(
                    "Target ConfigurationElement must have an accessible property named TypeName or a specified property of type string.");
            }
        }

        private Type createdElementTypeInstance;

        ///<summary>
        /// Gets or sets the value of the element type to create during execute.
        ///</summary>
        public Type CreatedElementType
        {
            set
            {
                createdElementTypeInstance = value;
            }
            get
            {
                if (createdElementTypeInstance == null)
                {
                    createdElementTypeInstance = ConfigurationElementType;
                }

                return createdElementTypeInstance;
            }
        }

        /// <summary>
        /// Adds a new child element to <see cref="DefaultCollectionElementAddCommand.ElementCollectionModel"/>.
        /// </summary>
        /// <param name="parameter">Not used. </param>
        /// <remarks>
        /// Collects the type from the user with the <see cref="TypeBrowser"/>.
        /// <br/>
        /// After collecting the type, inheritors of <see cref="TypePickingCollectionElementAddCommand"/> have
        /// an opportunity to take action if the override <see cref="AfterSelectType"/>.
        /// <br/>
        /// Then, a new element of the collected type is added through <see cref="ElementCollectionViewModel.AddNewCollectionElement"/>.
        /// </remarks>
        protected override void InnerExecute(object parameter)
        {
            var selectedType = GetSelectedType();

            if (selectedType != null && AfterSelectType(selectedType))
            {
                var createdElement = ElementCollectionModel.AddNewCollectionElement(CreatedElementType);
                createdElement.PropertiesShown = true;
                SetProperties(createdElement, selectedType);
            }
        }

        /// <summary>
        /// When overridden in a child class, provides the opportunity to determine
        /// if the element should be added to the collection.
        /// </summary>
        /// <param name="selectedType">The type selected by the user.</param>
        /// <returns>
        /// Should return <see langword="true"/> if an instance of <paramref name="selectedType"/> should be added to the collection.
        /// Otherwise, should return <see langword="false"/>
        /// </returns>
        protected virtual bool AfterSelectType(Type selectedType)
        {
            return true;
        }

        /// <summary>
        /// Sets properties on the newly minted <see cref="ElementViewModel"/>.
        /// </summary>
        /// <param name="createdElement">The element created.</param>
        /// <param name="selectedType">The element type created.</param>
        /// <remarks>
        /// By default, this sets the <see cref="ElementViewModel.NameProperty"/>'s value, if present.
        /// The command also updates the <see cref="Property.Value"/> of the property specified by <see cref="TypePickingCommandAttribute.Property"/> to
        /// the assembly qualified name of the selected type.
        /// </remarks>
        protected virtual void SetProperties(ElementViewModel createdElement, Type selectedType)
        {
            if (createdElement.NameProperty != null)
            {
                createdElement.NameProperty.Value = ElementCollectionModel.FindUniqueNewName(selectedType.Name);
            }
            createdElement.Property(propertyToSet).Value = selectedType.AssemblyQualifiedName;
        }

        private Type GetSelectedType()
        {
            Type baseType = typeof(object);
            Type configurationType = null;
            TypeSelectorIncludes includes = TypeSelectorIncludes.None;

            PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(ConfigurationElementType).Find(propertyToSet,
                                                                                                      true);

            if (propertyDescriptor == null) throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Could not find property {0} on configuration element type {1}", propertyToSet, ConfigurationElementType.Name));

            BaseTypeAttribute baseTypeAttribute = propertyDescriptor.Attributes.OfType<BaseTypeAttribute>().FirstOrDefault();
            if (baseTypeAttribute != null)
            {
                baseType = baseTypeAttribute.BaseType;
                includes = baseTypeAttribute.TypeSelectorIncludes;
                configurationType = baseTypeAttribute.ConfigurationType;
            }

            return GetSelectedType(baseType, baseType, includes, configurationType);
        }

        /// <summary>
        /// Retrieves and returns the selected type from the user.
        /// </summary>
        /// <param name="selectedType">The type to select in the type selection dialog.</param>
        /// <param name="baseType">The base type (class or interface) from which the constrained type should derive.</param>
        /// <param name="selectorIncludes">Indicates the types that can be browsed.</param>
        /// <param name="configurationType">The base type from which a type specified by the 
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ConfigurationElementTypeAttribute"/>
        /// bound to the constrained type should derive, or <see langword="null"/> if no such constraint is necessary.
        /// </param>
        /// <returns>
        /// The selected <see cref="Type"/> or <see langword="null"/> if not type is selected.
        /// </returns>
        protected virtual Type GetSelectedType(Type selectedType, Type baseType, TypeSelectorIncludes selectorIncludes, Type configurationType)
        {
            var viewModel = new TypeBrowserViewModel(new TypeBuildNodeConstraint(baseType, configurationType, selectorIncludes), this);
            var selector = new TypeBrowser(viewModel, this.discoveryService);

            Nullable<bool> result = false;
            if (this.UIService != null)
            {
                result = UIService.ShowDialog(selector);
            }
            else
            {
                result = selector.ShowDialog();
            }

            if (result.HasValue && result.Value)
            {
                return selector.SelectedType;
            }
            return null;
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == typeof(IUIServiceWpf)) return this.UIService;
            if (serviceType == typeof(IUIService)) return this.UIService;
            if (serviceType == typeof(IAssemblyDiscoveryService)) return this.discoveryService;
            return null;
        }
    }
}
