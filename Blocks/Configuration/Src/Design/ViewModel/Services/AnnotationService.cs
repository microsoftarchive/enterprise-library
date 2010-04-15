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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions;
using System.Security.Permissions;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    /// <summary>
    /// Service class that allows metadata to be added to classes by means of a body class.
    /// </summary>
    /// <remarks>
    /// In order to get an instance of this class, declare it as a constructor argument on the consuming component or use the <see cref="IUnityContainer"/> to obtain an instance from code.
    /// </remarks>
    /// <remarks>
    /// A body-class (or substitute type) is a class that declares property with matching names and allows custom attributes to be added without modifying the real configuration class.
    /// </remarks>
    public class AnnotationService : TypeDescriptionProvider, IDisposable
    {
        static object registerSubstituteTypeForMetadataLock = new object();
        static Dictionary<Type, ICustomTypeDescriptor> registeredTypeDescriptors = new Dictionary<Type, ICustomTypeDescriptor>();

        private readonly AssemblyLocator assemblyLocator;
        
        /// <summary>
        /// Initializes a new instance of <see cref="AnnotationService"/>.
        /// </summary>
        /// <param name="assemblyLocator">An <see cref="AssemblyLocator"/> used to discover bodyclasses with an <see cref="RegisterAsMetadataTypeAttribute"/>.</param>
        public AnnotationService(AssemblyLocator assemblyLocator)
        {
            this.assemblyLocator = assemblyLocator;
        }

        /// <summary>
        /// Discovers body classes decorated with the <see cref="RegisterAsMetadataTypeAttribute"/> attribute and registers them with.
        /// </summary>
        public void DiscoverSubstituteTypesFromAssemblies()
        {

            var entries = assemblyLocator.Assemblies
                    .FilterSelectManySafe(
                        a => FilteredSafeExtensions.FilterSelectManySafe(a.GetExportedTypes(), t => t.GetCustomAttributes(false).OfType<RegisterAsMetadataTypeAttribute>()
                                    .FilterSelectSafe(b => new { ExportedType = t, MetadataAttribute = b })
                                    ));

            foreach (var entry in entries)
            {
                RegisterSubstituteTypeForMetadata(entry.MetadataAttribute.TargetType, entry.ExportedType);
            }
        }

        /// <summary>
        /// Registers a body-class for a target <paramref name="type" />.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> that should be annotated by body-class <paramref name="metadataType"/>.</param>
        /// <param name="metadataType">The body-class that declares custom attributes for <paramref name="type" />. </param>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void RegisterSubstituteTypeForMetadata(Type type, Type metadataType)
        {
            if (!registeredTypeDescriptors.ContainsKey(type))
            {
                lock (registerSubstituteTypeForMetadataLock)
                {
                    if (!registeredTypeDescriptors.ContainsKey(type))
                    {
                        var originalProperties = TypeDescriptor.GetProperties(type);
                        var originalAttributes = TypeDescriptor.GetAttributes(type);

                        var substituteDescriptor = new AnnotatedTypeDescriptorProvider(originalAttributes.OfType<Attribute>(), originalProperties.OfType<PropertyDescriptor>(), metadataType);
                        registeredTypeDescriptors.Add(type, substituteDescriptor);

                        TypeDescriptor.AddProvider(this, type);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a custom type descriptor for the given type and object.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.ICustomTypeDescriptor"/> that can provide metadata for the type.
        /// </returns>
        /// <param name="objectType">The type of object for which to retrieve the type descriptor.</param><param name="instance">An instance of the type. Can be null if no instance was passed to the <see cref="T:System.ComponentModel.TypeDescriptor"/>.</param>
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            ICustomTypeDescriptor descriptor = null;
            if (registeredTypeDescriptors.TryGetValue(objectType, out descriptor))
            {
                return descriptor;
            }

            return base.GetTypeDescriptor(objectType, instance);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="AnnotationService"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources. </param>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]        
        protected virtual void Dispose(bool disposing)
        {
            foreach (var annotatedType in registeredTypeDescriptors)
            {
                TypeDescriptor.RemoveProvider(this, annotatedType.Key);
            }    
        }

        /// <summary>
        /// Releases all resources used by the <see cref="AnnotationService"/>.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private class AnnotatedTypeDescriptorProvider : CustomTypeDescriptor
        {
            Attribute[] originalAttributes;
            IDictionary<string, PropertyDescriptor> originalProperties;
            Type metadataType;

            public AnnotatedTypeDescriptorProvider(IEnumerable<Attribute> originalAttributes, IEnumerable<PropertyDescriptor> originalProperties, Type metadataType)
            {
                this.metadataType = metadataType;

                this.originalAttributes = originalAttributes.ToArray();
                this.originalProperties = originalProperties.ToDictionary(x => x.Name);
            }

            public override AttributeCollection GetAttributes()
            {
                var attributesFromMetadataType = TypeDescriptor.GetAttributes(metadataType).OfType<Attribute>();
                var combinedAttributes = MetadataCollection.CombineAttributes(originalAttributes, attributesFromMetadataType);

                return new AttributeCollection(combinedAttributes.ToArray());
            }

            public override PropertyDescriptorCollection GetProperties()
            {
                List<PropertyDescriptor> properties = new List<PropertyDescriptor>();

                foreach (PropertyDescriptor metadataProperty in TypeDescriptor.GetProperties(metadataType))
                {
                    PropertyDescriptor original = null;
                    if (originalProperties.TryGetValue(metadataProperty.Name, out original))
                    {
                        IEnumerable<Attribute> metadata = metadataProperty.Attributes.OfType<Attribute>();
                        AnnotatedPropertyDescriptor newProperty = new AnnotatedPropertyDescriptor(original, metadata);

                        properties.Add(newProperty);
                    }
                }

                var originalPropertiesWithoutCounterPart = originalProperties.Where(x => !properties.Select(y => y.Name).Contains(x.Key)).Select(x => x.Value);
                properties.AddRange(originalPropertiesWithoutCounterPart);

                return new PropertyDescriptorCollection(properties.ToArray());
            }

            public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                return TypeDescriptor.GetProperties(metadataType, attributes);
            }
        }

        private class AnnotatedPropertyDescriptor : PropertyDescriptor
        {
            PropertyDescriptor parentDescriptor;

            public AnnotatedPropertyDescriptor(PropertyDescriptor parentDescriptor, IEnumerable<Attribute> substitutedMetadata)
                : base(parentDescriptor, MetadataCollection.CombineAttributes(parentDescriptor.Attributes.OfType<Attribute>(), substitutedMetadata).ToArray())
            {
                this.parentDescriptor = parentDescriptor;
            }

            public override bool CanResetValue(object component)
            {
                return parentDescriptor.CanResetValue(component);
            }

            public override Type ComponentType
            {
                get { return parentDescriptor.ComponentType; }
            }

            public override object GetValue(object component)
            {
                return parentDescriptor.GetValue(component);
            }

            public override bool IsReadOnly
            {
                get { return parentDescriptor.IsReadOnly; }
            }

            public override Type PropertyType
            {
                get { return parentDescriptor.PropertyType; }
            }

            public override void ResetValue(object component)
            {
                parentDescriptor.ResetValue(component);
            }

            public override void SetValue(object component, object value)
            {
                parentDescriptor.SetValue(component, value);
            }

            public override bool ShouldSerializeValue(object component)
            {
                return parentDescriptor.ShouldSerializeValue(component);
            }
        }

    }
}
