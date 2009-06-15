//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Security.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
	/// <summary>
	/// Configuration object for Custom Providers.
	/// </summary>
	public class CustomAuthorizationProviderData
		: AuthorizationProviderData, IHelperAssistedCustomConfigurationData<CustomAuthorizationProviderData>
	{
		private readonly CustomProviderDataHelper<CustomAuthorizationProviderData> helper;

		/// <summary>
		/// Initializes with default values.
		/// </summary>
		public CustomAuthorizationProviderData()
		{
			helper = new CustomProviderDataHelper<CustomAuthorizationProviderData>(this);
		}

		/// <summary>
		/// Initializes with name and provider type.
		/// </summary>
		public CustomAuthorizationProviderData(string name, Type type)
		{
			helper = new CustomProviderDataHelper<CustomAuthorizationProviderData>(this);
			Name = name;
			Type = type;
		}

		/// <summary>
		/// Initializes with name and provider type.
		/// </summary>
		public CustomAuthorizationProviderData(string name, string typeName)
		{
			helper = new CustomProviderDataHelper<CustomAuthorizationProviderData>(this);
			Name = name;
			TypeName = typeName;
		}

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
		/// Gets or sets custom configuration attributes.
		/// </summary>        		
		public NameValueCollection Attributes
		{
			get { return helper.Attributes; }
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
		/// Modifies the <see cref="CustomAuthorizationProviderData"/> object to remove all values that should not be saved. 
		/// </summary>
		/// <param name="sourceElement">A <see cref="ConfigurationElement"/> object at the current level containing a merged view of the properties.</param>
		/// <param name="parentElement">A parent <see cref="ConfigurationElement"/> object or <see langword="null"/> if this is the top level.</param>		
		/// <param name="saveMode">One of the <see cref="ConfigurationSaveMode"/> values.</param>
		protected override void Unmerge(ConfigurationElement sourceElement,
		                                ConfigurationElement parentElement,
		                                ConfigurationSaveMode saveMode)
		{
			helper.HandleUnmerge(sourceElement, parentElement, saveMode);
		}

		/// <summary>
		/// Resets the internal state of the <see cref="CustomAuthorizationProviderData"/> object, 
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
		/// Called when an unknown attribute is encountered while deserializing the <see cref="CustomAuthorizationProviderData"/> object.
		/// </summary>
		/// <param name="name">The name of the unrecognized attribute.</param>
		/// <param name="value">The value of the unrecognized attribute.</param>
		/// <returns><see langword="true"/> if the processing of the element should continue; otherwise, <see langword="false"/>.</returns>
		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			return helper.HandleOnDeserializeUnrecognizedAttribute(name, value);
		}

		/// <summary>
		/// Gets the helper.
		/// </summary>
		CustomProviderDataHelper<CustomAuthorizationProviderData>
			IHelperAssistedCustomConfigurationData<CustomAuthorizationProviderData>.Helper
		{
			get { return helper; }
		}

		/// <summary>Invokes the inherited behavior.</summary>
		object IHelperAssistedCustomConfigurationData<CustomAuthorizationProviderData>.BaseGetPropertyValue(
			ConfigurationProperty property)
		{
			return base[property];
		}

		/// <summary>Invokes the inherited behavior.</summary>
		void IHelperAssistedCustomConfigurationData<CustomAuthorizationProviderData>.BaseSetPropertyValue(
			ConfigurationProperty property, object value)
		{
			base[property] = value;
		}

		/// <summary>Invokes the inherited behavior.</summary>
		void IHelperAssistedCustomConfigurationData<CustomAuthorizationProviderData>.BaseUnmerge(
			ConfigurationElement sourceElement,
			ConfigurationElement parentElement,
			ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
		}

		/// <summary>Invokes the inherited behavior.</summary>
		void IHelperAssistedCustomConfigurationData<CustomAuthorizationProviderData>.BaseReset(
			ConfigurationElement parentElement)
		{
			base.Reset(parentElement);
		}

		/// <summary>Invokes the inherited behavior.</summary>
		bool IHelperAssistedCustomConfigurationData<CustomAuthorizationProviderData>.BaseIsModified()
		{
			return base.IsModified();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            //missing type is handled by configuration system.

            if (!typeof(IAuthorizationProvider).IsAssignableFrom(this.Type))
            {
                throw new ConfigurationErrorsException(string.Format(Resources.Culture, Resources.ExceptionTypeForCustomAuthProviderMustDeriveFrom, Name, this.Type.FullName)); 
            }

            yield return new TypeRegistration(
                RegistrationExpressionBuilder.BuildExpression(this.Type, Attributes),
                typeof(IAuthorizationProvider)) 
                { 
                    Name = this.Name,
                    Lifetime = TypeRegistrationLifetime.Transient
                };

            
        }
    }
}
