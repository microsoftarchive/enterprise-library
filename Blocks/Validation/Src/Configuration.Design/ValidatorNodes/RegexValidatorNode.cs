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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.ComponentModel;
using System.Drawing.Design;
using System.ComponentModel.Design;
using System.Web.UI.Design.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
    /// <summary>
    /// Respresents the designtime configuration node for an <see cref="RegexValidatorData"/>.
    /// </summary>
    public class RegexValidatorNode : SingleValidatorNodeBase
    {
        private string pattern = string.Empty;
        private RegexOptions options;
		private string patternResourceName;
		private string patternResourceTypeName;

        /// <summary>
        /// Creates an instance of <see cref="RegexValidatorNode"/> based on default values.
        /// </summary>
        public RegexValidatorNode()
            : this(new RegexValidatorData(Resources.RegexValidatorNodeName))
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="RegexValidatorNode"/> based on runtime configuration data.
        /// </summary>
        /// <param name="validatorData">The corresponding runtime configuration data.</param>
        public RegexValidatorNode(RegexValidatorData validatorData)
            : base(validatorData)
        {
            if (!string.IsNullOrEmpty(validatorData.Pattern))
            {
                pattern = validatorData.Pattern;
            }
			patternResourceName = validatorData.PatternResourceName;
			patternResourceTypeName = validatorData.PatternResourceTypeName;
            options = validatorData.Options;
        }

        /// <summary>
        /// Gets or sets the regular expression used for validation.
        /// </summary>
        [SRDescription("RegexPatternDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [Editor(typeof(RegexTypeEditor), typeof(UITypeEditor))]
        [Required]
        [ValidRegexAttribute]
        public string Pattern
        {
            get { return pattern; }
            set { pattern = value; }
        }

        /// <summary>
        /// Gets or sets the options used to interpreted the regular expression.
        /// </summary>
        [SRDescription("RegexOptionsDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [Editor(typeof(FlagsEditor), typeof(UITypeEditor))]
        public RegexOptions Options
        {
            get { return options; }
            set { options = value; }
        }

		/// <summary>
		/// Gets or sets the name of the resource holding the regex pattern.
		/// </summary>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("RegexPatternResourceNameDescription", typeof(Resources))]
		public string PatternResourceName
		{
			get { return patternResourceName; }
			set { patternResourceName = value; }
		}

		/// <summary>
		/// Gets or sets the name of the resource type holding the regex pattern.
		/// </summary>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("RegexPatternResourceTypeDescription", typeof(Resources))]
		[Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
		[BaseType(typeof(Object), TypeSelectorIncludes.None)]
		public string PatternResourceTypeName
		{
			get { return patternResourceTypeName; }
			set { patternResourceTypeName = value; }
		}

        /// <summary>
        /// Returns the runtime configuration data that is represented by this node.
        /// </summary>
        /// <returns>An instance of <see cref="RegexValidatorData"/> that can be persisted to a configuration file.</returns>
        public override ValidatorData CreateValidatorData()
        {
            RegexValidatorData validatorData = new RegexValidatorData(Name);
            SetValidatorBaseProperties(validatorData);

            validatorData.Pattern = pattern;
			validatorData.Options = options;
			validatorData.PatternResourceName = patternResourceName;
			validatorData.PatternResourceTypeName = patternResourceTypeName;

            return validatorData;

        }
    }
}
