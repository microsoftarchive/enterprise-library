//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    [ResourceDescription(typeof(DesignResources), "TypeConversionValidatorDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "TypeConversionValidatorDataDisplayName")]
    partial class TypeConversionValidatorData
    {
        private static readonly AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="TypeConversionValidatorData"/> class.</para>
        /// </summary>
        public TypeConversionValidatorData() { Type = typeof(TypeConversionValidator); }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="TypeConversionValidatorData"/> class with a name.</para>
        /// </summary>
        /// <param name="name">The name for the instance.</param>
        public TypeConversionValidatorData(string name)
            : base(name, typeof(TypeConversionValidator))
        { }

        private const string TargetTypeNamePropertyName = "targetType";
        /// <summary>
        /// Gets or sets name of the type the represented <see cref="TypeConversionValidator"/> must use for testing conversion.
        /// </summary>
        [ConfigurationProperty(TargetTypeNamePropertyName, IsRequired=true)]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(object))]
        [ResourceDescription(typeof(DesignResources), "TypeConversionValidatorDataTargetTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "TypeConversionValidatorDataTargetTypeNameDisplayName")]
        public string TargetTypeName
        {
            get { return (string)this[TargetTypeNamePropertyName]; }
            set { this[TargetTypeNamePropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the target element type.
        /// </summary>
        public Type TargetType
        {
            get { return (Type)typeConverter.ConvertFrom(TargetTypeName); }
            set { TargetTypeName = typeConverter.ConvertToString(value); }
        }
    }
}
