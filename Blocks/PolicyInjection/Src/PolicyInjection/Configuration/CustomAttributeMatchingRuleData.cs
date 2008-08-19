//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// Configuration element for the <see cref="CustomAttributeMatchingRule"/> configuration.
    /// </summary>
    [Assembler(typeof(CustomAttributeMatchingRuleAssembler))]
    public class CustomAttributeMatchingRuleData : MatchingRuleData
    {
        private static AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        private const string SearchInheritanceChainPropertyName = "searchInheritanceChain";
        private const string AttributeTypePropertyName = "attributeType";

        /// <summary>
        /// Creates a new <see cref="CustomAttributeMatchingRuleData"/>.
        /// </summary>
        public CustomAttributeMatchingRuleData()
        {
        }

        /// <summary>
        /// Creates a new <see cref="CustomAttributeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="name">Name of the matching rule.</param>
        public CustomAttributeMatchingRuleData(string name)
            : base(name, typeof(CustomAttributeMatchingRule))
        {
        }

        /// <summary>
        /// Creates a new <see cref="CustomAttributeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="name">Name of the matching rule.</param>
        /// <param name="attributeType">Attribute to find on the target.</param>
        /// <param name="searchInheritanceChain">Should we search the inheritance chain to find the attribute?</param>
        public CustomAttributeMatchingRuleData(string name, Type attributeType, bool searchInheritanceChain)
            :this(name, typeConverter.ConvertToString(attributeType), searchInheritanceChain)
        {
        }

        /// <summary>
        /// Creates a new <see cref="CustomAttributeMatchingRuleData"/> instance.
        /// </summary>
        /// <param name="name">Name of the matching rule.</param>
        /// <param name="attributeTypeName">Name of the attribute type to match on the target.</param>
        /// <param name="searchInheritanceChain">Should we search the inheritance chain to find the attribute?</param>
        public CustomAttributeMatchingRuleData(string name, string attributeTypeName, bool searchInheritanceChain)
            : base(name, typeof(CustomAttributeMatchingRule))
        {
            SearchInheritanceChain = searchInheritanceChain;
            AttributeTypeName = attributeTypeName;
        }

        /// <summary>
        /// Should we search the inheritance chain to find the attribute?
        /// </summary>
        /// <value>The "searchInheritanceChain" config attribute.</value>
        [ConfigurationProperty(SearchInheritanceChainPropertyName)]
        public bool SearchInheritanceChain
        {
            get { return (bool)base[SearchInheritanceChainPropertyName]; }
            set { base[SearchInheritanceChainPropertyName] = value; }
        }

        /// <summary>
        /// Name of attribute type to match.
        /// </summary>
        /// <value>The "attributeType" config attribute.</value>
        [ConfigurationProperty(AttributeTypePropertyName, IsRequired = true)]
        public string AttributeTypeName
        {
            get { return (string)base[AttributeTypePropertyName]; }
            set { base[AttributeTypePropertyName] = value; }
        }

        /// <summary>
        /// The underlying type object for the attribute we want to search for.
        /// </summary>
        /// <value>This wraps the AttributeTypeName property in a type converter.</value>
        public Type AttributeType
        {
            get { return (Type)typeConverter.ConvertFrom(AttributeTypeName); }
            set { AttributeTypeName = typeConverter.ConvertToString(value); }
        }
    }

    /// <summary>
    /// A class used by ObjectBuilder to construct a <see cref="CustomAttributeMatchingRule"/>
    /// instance from a <see cref="CustomAttributeMatchingRuleData"/> instance.
    /// </summary>
    public class CustomAttributeMatchingRuleAssembler : IAssembler<IMatchingRule, MatchingRuleData>
    {
        /// <summary>
        /// Builds an instance of the subtype of <typeparamref name="TObject"/> type the receiver knows how to build, based on 
        /// a configuration object.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of the <typeparamref name="TObject"/> subtype.</returns>
        public IMatchingRule Assemble(IBuilderContext context, MatchingRuleData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            CustomAttributeMatchingRuleData castedRuleData = (CustomAttributeMatchingRuleData)objectConfiguration;

            CustomAttributeMatchingRule matchingRule = new CustomAttributeMatchingRule(castedRuleData.AttributeType, castedRuleData.SearchInheritanceChain);

            return matchingRule;
        }
    }
}
