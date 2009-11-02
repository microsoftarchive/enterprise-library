using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Console.Wpf.ViewModel.Services
{
    public class AnnotationService : TypeDescriptionProvider, IDisposable
    {
        static object registerSubstituteTypeForMetadataLock = new object();
        static Dictionary<Type, ICustomTypeDescriptor> registeredTypeDescriptors = new Dictionary<Type, ICustomTypeDescriptor>();
        

        public AnnotationService()
        {

        }

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

                        var substituteDescriptor = new AnnotatedTypeDescriptorProvider(type, originalAttributes.OfType<Attribute>(), originalProperties.OfType<PropertyDescriptor>() , metadataType);
                        registeredTypeDescriptors.Add(type, substituteDescriptor);

                        TypeDescriptor.AddProvider(this, type);
                    }
                }
            }
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            ICustomTypeDescriptor descriptor = null;
            if (registeredTypeDescriptors.TryGetValue(objectType, out descriptor))
            {
                return descriptor;
            }

            return base.GetTypeDescriptor(objectType, instance);
        }

        public void Dispose()
        {
            foreach (var annotatedType in registeredTypeDescriptors)
            {
                TypeDescriptor.RemoveProvider(this, annotatedType.Key);
            }
        }

        private class AnnotatedTypeDescriptorProvider : CustomTypeDescriptor
        {
            Attribute[] originalAttributes;
            IDictionary<string, PropertyDescriptor> originalProperties;
            Type actualType;
            Type metadataType;

            public AnnotatedTypeDescriptorProvider(Type actualType, IEnumerable<Attribute> originalAttributes, IEnumerable<PropertyDescriptor> originalProperties, Type metadataType)
            {
                this.actualType = actualType;
                this.metadataType = metadataType;

                this.originalAttributes = originalAttributes.ToArray();
                this.originalProperties = originalProperties.ToDictionary(x=>x.Name);
            }

            public override AttributeCollection GetAttributes()
            {
                var attributesFromMetadataType = TypeDescriptor.GetAttributes(metadataType).OfType<Attribute>();
                var combinedAttributes = MetadataCollection.CombineAttributes(originalAttributes, attributesFromMetadataType);

                return new AttributeCollection(combinedAttributes.ToArray()) ;
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
                :base(parentDescriptor, MetadataCollection.CombineAttributes(parentDescriptor.Attributes.OfType<Attribute>(), substitutedMetadata).ToArray() )
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
