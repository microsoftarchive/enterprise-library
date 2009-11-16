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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel
{
   

    /// <summary>
    /// Collects a type from the user and populates the newly created <see cref="ElementViewModel"/> with type information.
    /// This will attempt to set the Name property and TypeName properties found in the <see cref="ElementViewModel.Properties"/> collection.
    /// </summary>
    public class TypePickingCollectionElementAddCommand : DefaultCollectionElementAddCommand
    {
        private readonly string propertyToSet;

        /// <summary>
        /// Initializes a new <see cref="TypePickingCollectionElementAddCommand"/> for a <see cref="NamedConfigurationElement"/> type and
        /// containing <see cref="ElementCollectionViewModel"/>.  If no property to set is explicitly defined, it will default to the
        /// TypeName property, following the pattern for setting <see cref="Type"/> information for many <see cref="ConfigurationElement"/>s in Enterprise Library.
        /// </summary>
        /// <param name="configurationElementType">The configuration element type.  This is expected to be a <see cref="NamedConfigurationElement"/></param>
        /// <param name="elementCollectionModel">The containing <see cref="ElementCollectionViewModel"/></param>
        public TypePickingCollectionElementAddCommand(TypePickingCommandAttribute commandAttribute, ConfigurationElementType configurationElementType, ElementCollectionViewModel elementCollectionModel)
            : base(commandAttribute, configurationElementType, elementCollectionModel)
        {
            this.propertyToSet = commandAttribute.Property;

            if (propertyToSet == null)
            {
                throw new ArgumentException(
                    "Target ConfigurationElement must have an accessible property named TypeName or a specified property of type string.");
            }
           
        }
 

        public override void Execute(object parameter)
        {
            var selectedType = GetSelectedType();

            if (selectedType != null && AfterSelectType(selectedType))
            {
                var createdElement = ElementCollectionModel.AddNewCollectionElement(ConfigurationElementType);

                SetProperties(createdElement, selectedType);
            }
        }

        protected virtual bool AfterSelectType(Type selectedType)
        {
            return true;
        }

        protected virtual void SetProperties(ElementViewModel createdElement, Type selectedType)
        {
            createdElement.Property("Name").Value = selectedType.Name;
            createdElement.Property(propertyToSet).Value = selectedType.AssemblyQualifiedName;
        }

        private Type GetSelectedType()
        {
            Type baseType = typeof(object);
            Type configurationType = null;
            TypeSelectorIncludes includes = TypeSelectorIncludes.None;

            PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(ConfigurationElementType).Find(propertyToSet,
                                                                                                      true);

            if (propertyDescriptor == null) throw new InvalidOperationException(string.Format("Could not find property {0} on configuration element type {1}", propertyToSet, ConfigurationElementType.Name));

            BaseTypeAttribute baseTypeAttribute = propertyDescriptor.Attributes.OfType<BaseTypeAttribute>().FirstOrDefault();
            if (baseTypeAttribute != null)
            {
                baseType = baseTypeAttribute.BaseType;
                includes = baseTypeAttribute.TypeSelectorIncludes;
                configurationType = baseTypeAttribute.ConfigurationType;
            }

            return GetSelectedType(baseType, baseType, includes, configurationType);
        }

        protected virtual Type GetSelectedType(Type selectedType, Type baseType, TypeSelectorIncludes selectorIncludes, Type configurationType)
        {
            TypeSelectorUI selector = new TypeSelectorUI(
                selectedType,
                baseType,
                selectorIncludes,
                configurationType);
            DialogResult result = selector.ShowDialog();
            if (result == DialogResult.OK)
            {
                return selector.SelectedType;
            }
            return null;
        }
    }
}
