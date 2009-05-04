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
using System.Drawing.Design;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
    /// <summary>
    /// Represents a merged configuration <see cref="PropertyDescriptor"/>.
    /// </summary>
    public class MergedConfigurationProperty : PropertyDescriptor
    {
        readonly object actualInstance;
        readonly IConfigurationUIHierarchy configurationHierarchy;
        readonly PropertyDescriptor innerProperyDescriptor;
        readonly ConfigurationNodeMergeData mergedConfigurationData;

        /// <summary>
        /// Initialize a new instance of the <see cref="MergedConfigurationProperty"/> class with the merged configuration data, A <see cref="PropertyDescriptor"/>, the instance and the <see cref="IConfigurationUIHierarchy"/>.
        /// </summary>
        /// <param name="mergedConfigurationData">A <see cref="ConfigurationNodeMergeData"/> object.</param>
        /// <param name="innerProperyDescriptor">The <see cref="PropertyDescriptor"/> for the node.</param>
        /// <param name="actualInstance">The instance for the property.</param>
        /// <param name="configurationHierarchy">An <see cref="IConfigurationUIHierarchy"/> object.</param>
        public MergedConfigurationProperty(ConfigurationNodeMergeData mergedConfigurationData,
                                           PropertyDescriptor innerProperyDescriptor,
                                           object actualInstance,
                                           IConfigurationUIHierarchy configurationHierarchy)
            : base(innerProperyDescriptor)
        {
            this.actualInstance = actualInstance;
            this.mergedConfigurationData = mergedConfigurationData;
            this.innerProperyDescriptor = innerProperyDescriptor;
            this.configurationHierarchy = configurationHierarchy;
        }

        ///<summary>
        ///Gets the type of the component this property is bound to.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Type"></see> that represents the type of component this property is bound to. When the <see cref="M:System.ComponentModel.PropertyDescriptor.GetValue(System.Object)"></see> or <see cref="M:System.ComponentModel.PropertyDescriptor.SetValue(System.Object,System.Object)"></see> methods are invoked, the object specified might be an instance of this type.
        ///</returns>
        ///
        public override Type ComponentType
        {
            get { return innerProperyDescriptor.ComponentType; }
        }

        ///<summary>
        ///Gets the type converter for this property.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.ComponentModel.TypeConverter"></see> that is used to convert the <see cref="T:System.Type"></see> of this property.
        ///</returns>
        ///<PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" /></PermissionSet>
        public override TypeConverter Converter
        {
            get
            {
                TypeConverter converter = base.Converter;
                if (converter != null)
                {
                    converter = new TypeConverterProxy(converter, actualInstance);
                }
                return converter;
            }
        }

        ///<summary>
        ///Gets a value indicating whether this property is read-only.
        ///</summary>
        ///
        ///<returns>
        ///true if the property is read-only; otherwise, false.
        ///</returns>
        ///
        public override bool IsReadOnly
        {
            get { return !mergedConfigurationData.OverrideProperties; }
        }

        ///<summary>
        ///Gets the type of the property.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Type"></see> that represents the type of the property.
        ///</returns>
        ///
        public override Type PropertyType
        {
            get { return innerProperyDescriptor.PropertyType; }
        }

        ///<summary>
        ///When overridden in a derived class, returns whether resetting an object changes its value.
        ///</summary>
        ///
        ///<returns>
        ///true if resetting the component changes its value; otherwise, false.
        ///</returns>
        ///
        ///<param name="component">The component to test for reset capability. </param>
        public override bool CanResetValue(object component)
        {
            return true;
        }

        ///<summary>
        ///Gets an editor of the specified type.
        ///</summary>
        ///
        ///<returns>
        ///An instance of the requested editor type, or null if an editor cannot be found.
        ///</returns>
        ///
        ///<param name="editorBaseType">The base type of editor, which is used to differentiate between multiple editors that a property supports. </param>
        public override object GetEditor(Type editorBaseType)
        {
            if (IsReadOnly) return null;
            if (editorBaseType == typeof(UITypeEditor))
            {
                UITypeEditor typeEditor = (UITypeEditor)innerProperyDescriptor.GetEditor(editorBaseType);
                if (typeEditor != null)
                {
                    return new UITypeEditorProxy(typeEditor, actualInstance);
                }
                return null;
            }
            return base.GetEditor(editorBaseType);
        }

        ///<summary>
        ///Gets the current value of the property on a component.
        ///</summary>
        ///
        ///<returns>
        ///The value of a property for a given component.
        ///</returns>
        ///
        ///<param name="component">The component with the property for which to retrieve the value. </param>
        public override object GetValue(object component)
        {
            object defaultValue = innerProperyDescriptor.GetValue(component);
            return mergedConfigurationData.GetPropertyValue(Name, PropertyType, defaultValue, configurationHierarchy);
        }

        ///<summary>
        ///Resets the value for this property of the component to the default value.
        ///</summary>
        ///
        ///<param name="component">The component with the property value that is to be reset to the default value. </param>
        public override void ResetValue(object component)
        {
            mergedConfigurationData.ResetPropertyValue(Name);
        }

        ///<summary>
        ///Sets the value of the component to a different value.
        ///</summary>
        ///
        ///<param name="component">The component with the property value that is to be set. </param>
        ///<param name="value">The new value. </param>
        public override void SetValue(object component,
                                      object value)
        {
            mergedConfigurationData.SetPropertyValue(Name, value);
        }

        ///<summary>
        ///Determines a value indicating whether the value of this property needs to be persisted.
        ///</summary>
        ///
        ///<returns>
        ///true if the property should be persisted; otherwise, false.
        ///</returns>
        ///
        ///<param name="component">The component with the property to be examined for persistence. </param>
        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        class TypeConverterProxy : TypeConverter
        {
            readonly object actualInstance;
            readonly TypeConverter actualTypeConverter;

            public TypeConverterProxy(TypeConverter actualTypeConverter,
                                      object actualInstance)
            {
                this.actualTypeConverter = actualTypeConverter;
                this.actualInstance = actualInstance;
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context,
                                                Type sourceType)
            {
                ITypeDescriptorContext wrappedContext = new TypeDescriptorContext(context, actualInstance);
                return actualTypeConverter.CanConvertFrom(wrappedContext, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context,
                                              Type destinationType)
            {
                ITypeDescriptorContext wrappedContext = new TypeDescriptorContext(context, actualInstance);
                return actualTypeConverter.CanConvertTo(wrappedContext, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context,
                                               CultureInfo culture,
                                               object value)
            {
                ITypeDescriptorContext wrappedContext = new TypeDescriptorContext(context, actualInstance);
                return actualTypeConverter.ConvertFrom(wrappedContext, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext context,
                                             CultureInfo culture,
                                             object value,
                                             Type destinationType)
            {
                ITypeDescriptorContext wrappedContext = new TypeDescriptorContext(context, actualInstance);
                return actualTypeConverter.ConvertTo(wrappedContext, culture, value, destinationType);
            }

            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                return false;
            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                ITypeDescriptorContext wrappedContext = new TypeDescriptorContext(context, actualInstance);
                return actualTypeConverter.GetStandardValues(wrappedContext);
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                ITypeDescriptorContext wrappedContext = new TypeDescriptorContext(context, actualInstance);
                return actualTypeConverter.GetStandardValuesExclusive(wrappedContext);
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                ITypeDescriptorContext wrappedContext = new TypeDescriptorContext(context, actualInstance);
                return actualTypeConverter.GetStandardValuesSupported(wrappedContext);
            }

            public override bool IsValid(ITypeDescriptorContext context,
                                         object value)
            {
                ITypeDescriptorContext wrappedContext = new TypeDescriptorContext(context, actualInstance);
                return actualTypeConverter.IsValid(wrappedContext, value);
            }
        }

        class TypeDescriptorContext : ITypeDescriptorContext
        {
            readonly object instance;
            readonly ITypeDescriptorContext originalTypeDescriptor;

            public TypeDescriptorContext(ITypeDescriptorContext originalTypeDescriptor,
                                         object instance)
            {
                this.instance = instance;
                this.originalTypeDescriptor = originalTypeDescriptor;
            }

            public IContainer Container
            {
                get { return originalTypeDescriptor.Container; }
            }

            public object Instance
            {
                get { return instance; }
            }

            public PropertyDescriptor PropertyDescriptor
            {
                get { return originalTypeDescriptor.PropertyDescriptor; }
            }

            public object GetService(Type serviceType)
            {
                return originalTypeDescriptor.GetService(serviceType);
            }

            public void OnComponentChanged()
            {
                originalTypeDescriptor.OnComponentChanged();
            }

            public bool OnComponentChanging()
            {
                return originalTypeDescriptor.OnComponentChanging();
            }
        }

        class UITypeEditorProxy : UITypeEditor
        {
            readonly object actualInstance;
            readonly UITypeEditor uiTypeEditor;

            public UITypeEditorProxy(UITypeEditor uiTypeEditor,
                                     object actualInstance)
            {
                this.actualInstance = actualInstance;
                this.uiTypeEditor = uiTypeEditor;
            }

            public override bool IsDropDownResizable
            {
                get { return uiTypeEditor.IsDropDownResizable; }
            }

            public override object EditValue(ITypeDescriptorContext context,
                                             IServiceProvider provider,
                                             object value)
            {
                ITypeDescriptorContext wrappedContext = new TypeDescriptorContext(context, actualInstance);
                return uiTypeEditor.EditValue(wrappedContext, provider, value);
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                ITypeDescriptorContext wrappedContext = new TypeDescriptorContext(context, actualInstance);
                return uiTypeEditor.GetEditStyle(wrappedContext);
            }

            public override bool GetPaintValueSupported(ITypeDescriptorContext context)
            {
                ITypeDescriptorContext wrappedContext = new TypeDescriptorContext(context, actualInstance);
                return uiTypeEditor.GetPaintValueSupported(wrappedContext);
            }

            public override void PaintValue(PaintValueEventArgs e)
            {
                uiTypeEditor.PaintValue(e);
            }
        }
    }
}
