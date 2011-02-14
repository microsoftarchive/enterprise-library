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

using System.Windows.Markup;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
    /// <summary>
    /// Configuration object to describe an instance of class <see cref="AndCompositeValidator"/>.
    /// </summary>
    /// <seealso cref="AndCompositeValidator"/>
    /// <seealso cref="ValidatorData"/>
    [ContentProperty("Validators")]
    partial class AndCompositeValidatorData
    {
        private NamedElementCollection<ValidatorData> validators = new NamedElementCollection<ValidatorData>();
        /// <summary>
        /// Gets the collection with the definitions for the validators composed by 
        /// the represented <see cref="AndCompositeValidator"/>.
        /// </summary>
        public NamedElementCollection<ValidatorData> Validators
        {
            get { return this.validators; }
        }

        ///// <summary>
        ///// Overridden in order to hide from the configuration designtime.
        ///// </summary>
        //[Browsable(false)]
        //public override string MessageTemplate
        //{
        //    get { return base.MessageTemplate; }
        //    set { base.MessageTemplate = value; }
        //}

        ///// <summary>
        ///// Overridden in order to hide from the configuration designtime.
        ///// </summary>
        //[Browsable(false)]
        //public override string MessageTemplateResourceName
        //{
        //    get { return base.MessageTemplateResourceName; }
        //    set { base.MessageTemplateResourceName = value; }
        //}
        ///// <summary>
        ///// Overridden in order to hide from the configuration designtime.
        ///// </summary>
        //[Browsable(false)]
        //public override string MessageTemplateResourceTypeName
        //{
        //    get { return base.MessageTemplateResourceTypeName; }
        //    set { base.MessageTemplateResourceTypeName = value; }
        //}
        ///// <summary>
        ///// Overridden in order to hide from the configuration designtime.
        ///// </summary>
        //[Browsable(false)]
        //public override string Tag
        //{
        //    get { return base.Tag; }
        //    set { base.Tag = value; }
        //}
    }
}
