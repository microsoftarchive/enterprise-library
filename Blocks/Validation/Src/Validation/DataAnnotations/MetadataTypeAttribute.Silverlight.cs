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
using System.ComponentModel.DataAnnotations;

using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.DataAnnotations
{
    /// <summary>
    /// Specifies the metadata class to associate with a data model class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This attribute might be deprecated in the future in favor of a Silverlight built-in System.ComponentModel.DataAnnotations.MetatadataTypeAttribute.
    /// The current implementation allows a Silverlight application using Validation Application Block to take advantage of this feature which is available
    /// in the .NET Framework, and it can be useful for cross-tier validation scenarios.
    /// </para>
    /// <para>
    /// The <see cref="MetadataTypeAttribute"/> attribute enables you to associate a class with a data-model partial class.
    /// In this associated class you provide additional metadata information that is not in the data model.
    /// For example, in the associated class you can apply the <see cref="RequiredAttribute"/>  attribute to a data field. 
    /// This enforces that a value is provided for the field even if this constraint is not required by the database schema.
    /// </para>
    /// <para>
    /// You use the <see cref="MetadataTypeAttribute"/> attribute as follows:
    ///  - In your application, create a file in which you create the data-model partial class that you want to modify.
    ///  - Create the associated metadata class.
    ///  - Apply the <see cref="MetadataTypeAttribute"/> attribute to the partial entity class, specifying the associated class.
    /// </para>
    /// <para>
    /// When you apply this attribute, you must adhere to the following usage constraints:
    ///  - The attribute can only be applied to a class.
    ///  - The attribute cannot be inherited by derived classes.
    ///  - The attribute can be applied only one time.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class MetadataTypeAttribute : Attribute
    {
        private readonly Type metadataClassType;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataTypeAttribute"/> class.
        /// </summary>
        /// <param name="metadataClassType">The metadata class to reference.</param>
        public MetadataTypeAttribute(Type metadataClassType)
        {
            this.metadataClassType = metadataClassType;
        }

        /// <summary>
        /// Gets the metadata class that is associated with a data-model partial class.
        /// </summary>
        public Type MetadataClassType
        {
            get
            {
                if (this.metadataClassType == null)
                {
                    throw new InvalidOperationException(Resources.MetadataTypeAttribute_TypeCannotBeNull);
                }

                return this.metadataClassType;
            }
        }
    }
}
