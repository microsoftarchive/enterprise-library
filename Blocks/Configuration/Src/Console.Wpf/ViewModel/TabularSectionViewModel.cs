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
using System.Configuration;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.ViewModel
{
    public class TabularSectionViewModel : SectionViewModel
    {
        Dictionary<Type, int> columnsByConfigurationType = new Dictionary<Type, int>();

        ColumnMappingCalculationContext columnMappings;

        public TabularSectionViewModel(IServiceProvider serviceProvider, ConfigurationSection section)
            :base(serviceProvider, section) 
        {
            InitializeLayout(section);
        }


        public TabularSectionViewModel(IServiceProvider serviceProvider, ConfigurationSection section, IEnumerable<Attribute> metadataAttributes)
            : base(serviceProvider, section, metadataAttributes)
        {
            InitializeLayout(section);
        }

        private void InitializeLayout(ConfigurationSection section)
        {
            Type sectionType = section.GetType();

            columnMappings = new ColumnMappingCalculationContext(sectionType);
        }

        public override IEnumerable<ViewModel> GetAdditionalGridVisuals()
        {
            return columnMappings
                        .Select( x=> new {Mapping = x, Instance = DescendentElements().OfType<ElementCollectionViewModel>().Where(y => y.ConfigurationType == x.ConfigurationCollectionType).FirstOrDefault()})
                        .Select(x => x.Instance != null 
                            ? (ViewModel) new ElementViewModelWrappingHeaderViewModel(x.Instance, true) { Column = x.Mapping.Column, Row = 0}
                            : (ViewModel) new StringHeaderViewModel(x.Mapping.ConfigurationElementType.Name) { Column = x.Mapping.Column, Row = 0 });
        }

        public override void UpdateLayout()
        {
            foreach (var columnMapping in columnMappings)
            {
                int row = 1;
                foreach (var element in DescendentElements().Where(x => columnMapping.ConfigurationElementType.IsAssignableFrom(x.ConfigurationType)))
                {
                    element.Column = columnMapping.Column;
                    element.Row = row++;
                }
            }
        }


        public IEnumerable<Type> FlattenTypes(Type type)
        {
            foreach(Type leafType in TypeDescriptor.GetProperties(type).OfType<PropertyDescriptor>().Select(x => x.PropertyType))
            {
                yield return leafType;

                foreach (Type leafOfLeafType in FlattenTypes(leafType))
                {
                    yield return leafOfLeafType;
                }
            }
        }


        private class ColumnMappingCalculationContext : List<ColumnMapping>
        {
            public ColumnMappingCalculationContext(Type t)
            {
                foreach (var prop in TypeDescriptor.GetProperties(t).OfType<PropertyDescriptor>())
                {
                    AnalyzeTypeHierarchy(prop);
                }

                CalculateColumns();
            }

            private void AnalyzeTypeHierarchy(PropertyDescriptor declaringProperty)
            {
                if (typeof(ConfigurationElementCollection).IsAssignableFrom(declaringProperty.PropertyType))
                {
                    RecreateColumnMappings(declaringProperty, declaringProperty.PropertyType);
                }
            }

            private void RecreateColumnMappings(PropertyDescriptor declaringProperty, Type configurationCollectionType)
            {
                var collectionInfo = declaringProperty.Attributes.OfType<ConfigurationCollectionAttribute>().FirstOrDefault();
                if (collectionInfo != null)
                {
                    Add(new ColumnMapping
                    {
                        ConfigurationCollectionType = configurationCollectionType,
                        ConfigurationElementType = collectionInfo.ItemType,
                        References = TypeDescriptor.GetProperties(collectionInfo.ItemType)
                                                        .OfType<PropertyDescriptor>()
                                                        .Select(x=>new { Property = x, ReferenceAtt = x.Attributes.OfType<ReferenceAttribute>().FirstOrDefault()})
                                                        .Where(x=>x.ReferenceAtt != null)
                                                        .Select(x=>x.ReferenceAtt.TargetType)
                                                        .ToArray(),
                        Column = this.Count
                    });
                }
            }

            private void CalculateColumns()
            {
            }
        }

        private class ColumnMapping
        {
            public Type ConfigurationCollectionType { get; set; }
            public Type ConfigurationElementType { get; set; }
            public Type[] References { get; set; }
            public int Column { get; set; }
        }
    }
}
