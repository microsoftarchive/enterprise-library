using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// Configuration object for ObjectCache for Silverlight.
    /// </summary>
    [ResourceDescription(typeof(CachingResources), "CustomCacheDataDescription")]
    [ResourceDisplayName(typeof(CachingResources), "CustomCacheDataDisplayName")]
    [TypePickingCommand(TitleResourceName = "CustomCacheDataDisplayName", 
        TitleResourceType = typeof(CachingResources), 
        Replace = CommandReplacement.NoCommand)]
    [Browsable(false)]
    public class CustomCacheData : CacheData, IHelperAssistedCustomConfigurationData<CustomCacheData>
    {
        private readonly CustomProviderDataHelper<CustomCacheData> helper;

        /// <summary>
        /// Initializes with default values.
        /// </summary>
        public CustomCacheData()
        {
            helper = new CustomProviderDataHelper<CustomCacheData>(this);
        }

        /// <summary>
        /// Initializes with name and provider type.
        /// </summary>
        public CustomCacheData(string name, Type type)
        {
            helper = new CustomProviderDataHelper<CustomCacheData>(this);
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Initializes with name and fully qualified type name of the provider type.
        /// </summary>
        public CustomCacheData(string name, string typeName)
        {
            helper = new CustomProviderDataHelper<CustomCacheData>(this);
            Name = name;
            TypeName = typeName;
        }

        /// <summary>
        /// Gets a <see cref="ConfigurationPropertyCollection"/> of the properties that are defined for 
        /// this configuration element when implemented in a derived class. 
        /// </summary>
        /// <value>
        /// A <see cref="ConfigurationPropertyCollection"/> of the properties that are defined for this
        /// configuration element when implemented in a derived class. 
        /// </value>
        protected override ConfigurationPropertyCollection Properties
        {
            get { return helper.Properties; }
        }

        /// <summary>
        /// Overridden in order to apply <see cref="BrowsableAttribute"/>.
        /// </summary>
        [Browsable(true)]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [ResourceDescription(typeof(CachingResources), "CustomCacheDataTypeNameDescription")]
        [ResourceDisplayName(typeof(CachingResources), "CustomCacheDataTypeNameDisplayName")]
        public override string TypeName
        {
            get { return base.TypeName; }
            set { base.TypeName = value; }
        }

        #region IHelperAssistedCustomConfigurationData<CustomCacheData> Members

        /// <summary>
        /// Gets or sets custom configuration attributes.
        /// </summary>
        [Validation(typeof(NameValueCollectionValidator))]
        public NameValueCollection Attributes
        {
            get { return helper.Attributes; }
        }

        /// <summary>
        /// Gets the helper.
        /// </summary>
        CustomProviderDataHelper<CustomCacheData> IHelperAssistedCustomConfigurationData<CustomCacheData>.
            Helper
        {
            get { return helper; }
        }

        /// <summary>Invokes the inherited behavior.</summary>
        object IHelperAssistedCustomConfigurationData<CustomCacheData>.BaseGetPropertyValue(
            ConfigurationProperty property)
        {
            return base[property];
        }

        /// <summary>Invokes the inherited behavior.</summary>
        void IHelperAssistedCustomConfigurationData<CustomCacheData>.BaseSetPropertyValue(
            ConfigurationProperty property, object value)
        {
            base[property] = value;
        }

        /// <summary>Invokes the inherited behavior.</summary>
        void IHelperAssistedCustomConfigurationData<CustomCacheData>.BaseUnmerge(
            ConfigurationElement sourceElement,
            ConfigurationElement parentElement,
            ConfigurationSaveMode saveMode)
        {
            base.Unmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        void IHelperAssistedCustomConfigurationData<CustomCacheData>.BaseReset(ConfigurationElement parentElement)
        {
            base.Reset(parentElement);
        }

        /// <summary>Invokes the inherited behavior.</summary>
        bool IHelperAssistedCustomConfigurationData<CustomCacheData>.BaseIsModified()
        {
            return base.IsModified();
        }

        #endregion


        /// <summary>
        /// Sets the attribute value for a key.
        /// </summary>
        /// <param name="key">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        public void SetAttributeValue(string key, string value)
        {
            helper.HandleSetAttributeValue(key, value);
        }

        /// <summary>
        /// Modifies the <see cref="CustomCacheData"/> object to remove all values that should not be saved. 
        /// </summary>
        /// <param name="sourceElement">A <see cref="ConfigurationElement"/> object at the current level containing a merged view of the properties.</param>
        /// <param name="parentElement">A parent <see cref="ConfigurationElement"/> object or <see langword="null"/> if this is the top level.</param>		
        /// <param name="saveMode">One of the <see cref="ConfigurationSaveMode"/> values.</param>
        protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement,
                                        ConfigurationSaveMode saveMode)
        {
            helper.HandleUnmerge(sourceElement, parentElement, saveMode);
        }

        /// <summary>
        /// Resets the internal state of the <see cref="CustomCacheData"/> object, 
        /// including the locks and the properties collection.
        /// </summary>
        /// <param name="parentElement">The parent element.</param>
        protected override void Reset(ConfigurationElement parentElement)
        {
            helper.HandleReset(parentElement);
        }

        /// <summary>
        /// Indicates whether this configuration element has been modified since it was last 
        /// saved or loaded when implemented in a derived class.
        /// </summary>
        /// <returns><see langword="true"/> if the element has been modified; otherwise, <see langword="false"/>. </returns>
        protected override bool IsModified()
        {
            return helper.HandleIsModified();
        }

        /// <summary>
        /// Called when an unknown attribute is encountered while deserializing the <see cref="CustomCacheData"/> object.
        /// </summary>
        /// <param name="name">The name of the unrecognized attribute.</param>
        /// <param name="value">The value of the unrecognized attribute.</param>
        /// <returns><see langword="true"/> if the processing of the element should continue; otherwise, <see langword="false"/>.</returns>
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            return helper.HandleOnDeserializeUnrecognizedAttribute(name, value);
        }
    }
}
