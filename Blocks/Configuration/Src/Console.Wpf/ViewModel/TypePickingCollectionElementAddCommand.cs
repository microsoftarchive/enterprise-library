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
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Console.Wpf.ComponentModel.Editors;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.ViewModel
{
    /// <summary>
    /// Collects a type from the user and populates the newly created <see cref="ElementViewModel"/> with type information.
    /// This will attempt to set the Name property and TypeName properties found in the <see cref="ElementViewModel.Properties"/> collection.
    /// </summary>
    public class TypePickingCollectionElementAddCommand : CollectionElementAddCommand
    {
        private readonly PropertyInfo propertyToSet;

        /// <summary>
        /// Initializes a new <see cref="TypePickingCollectionElementAddCommand"/> for a <see cref="NamedConfigurationElement"/> type and
        /// containing <see cref="ElementCollectionViewModel"/>.  If no property to set is explicitly defined, it will default to the
        /// TypeName property, following the pattern for setting <see cref="Type"/> information for many <see cref="ConfigurationElement"/>s in Enterprise Library.
        /// </summary>
        /// <param name="configurationElementType">The configuration element type.  This is expected to be a <see cref="NamedConfigurationElement"/></param>
        /// <param name="elementCollectionModel">The containing <see cref="ElementCollectionViewModel"/></param>
        public TypePickingCollectionElementAddCommand(Type configurationElementType, ElementCollectionViewModel elementCollectionModel) 
            : this(configurationElementType, elementCollectionModel, configurationElementType.GetProperty("TypeName", typeof(string)))
        {
            
        }
        
        //todo: should propertyToSet come from the custom attribute that indicates a custom type picker
        public TypePickingCollectionElementAddCommand(Type configurationElementType, ElementCollectionViewModel elementCollectionModel, PropertyInfo propertyToSet) 
            : base(configurationElementType, elementCollectionModel)
        {
            this.propertyToSet = propertyToSet;

            if (propertyToSet == null)
            {
                throw new ArgumentException(
                    "Target ConfigurationElement must have an accessible property named TypeName or a specified property of type string.");
            }
        }

        public override void Execute(object parameter)
        {
            var selectedType = GetSelectedType();

            if (selectedType != null)
            {
                
                var newNamedElement = ElementCollectionModel.CreateNewChildElement(ConfigurationElementType);

                newNamedElement.Property("Name").Value = selectedType.Name;
                newNamedElement.Property(propertyToSet.Name).Value = selectedType.AssemblyQualifiedName;

            }
        }

        //todo: should be driven by BaseType attribute on property.
        protected virtual Type GetSelectedType()
        {
            TypeSelectorUI selector = new TypeSelectorUI(
                typeof(Exception),
                typeof(Exception),
                TypeSelectorIncludes.BaseType |
                TypeSelectorIncludes.AbstractTypes);
            DialogResult result = selector.ShowDialog();
            if (result == DialogResult.OK)
            {
                return selector.SelectedType;
            }
            return null;
        }
    }
}
