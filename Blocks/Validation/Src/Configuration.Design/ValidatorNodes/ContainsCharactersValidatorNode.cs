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
using System.Drawing.Design;
using System.ComponentModel.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
    /// <summary>
    /// Respresents the designtime configuration node for an <see cref="ContainsCharactersValidatorData"/>.
    /// </summary>
    public class ContainsCharactersValidatorNode : ValueValidatorNode
    {
		private string characterSet;
		private ContainsCharacters containsCharacters;

		/// <summary>
        /// Creates an instance of <see cref="ContainsCharactersValidatorNode"/> based on default values.
        /// </summary>
        public ContainsCharactersValidatorNode()
			: this(new ContainsCharactersValidatorData(Resources.ContainsCharactersValidatorNodeName))
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="ContainsCharactersValidatorNode"/> based on runtime configuration data.
        /// </summary>
        /// <param name="validatorData">The corresponding runtime configuration data.</param>
		public ContainsCharactersValidatorNode(ContainsCharactersValidatorData validatorData)
            : base(validatorData)
        {
			this.characterSet = validatorData.CharacterSet;
			this.containsCharacters = validatorData.ContainsCharacters;
        }

		/// <summary>
		/// Gets or sets the character set for the represented validator.
		/// </summary>
		[SRDescription("CharacterSetDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[Required]
		public string CharacterSet
		{
			get { return characterSet; }
			set { characterSet = value; }
		}
		
		/// <summary>
		/// Gets or sets the <see cref="ContainsCharacters"/> set for the represented validator.
		/// </summary>
		[SRDescription("ContainsCharactersDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[Required]
		public ContainsCharacters ContainsCharacters
		{
			get { return containsCharacters; }
			set { containsCharacters = value; }
		}

		/// <summary>
        /// Returns the runtime configuration data that is represented by this node.
        /// </summary>
        /// <returns>An instance of <see cref="ContainsCharactersValidatorData"/> that can be persisted to a configuration file.</returns>
        public override ValidatorData CreateValidatorData()
        {
            ContainsCharactersValidatorData validatorData = new ContainsCharactersValidatorData(Name);
            SetValidatorBaseProperties(validatorData);
			SetValueValidatorBaseProperties(validatorData);

			validatorData.CharacterSet = characterSet;
			validatorData.ContainsCharacters = containsCharacters;

            return validatorData;
        }
    }
}