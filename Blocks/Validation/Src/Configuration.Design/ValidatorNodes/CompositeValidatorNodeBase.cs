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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
	/// <summary>
	/// Represents the designtime configuration node for any composite validator.
	/// </summary>
    [Image(typeof(CompositeValidatorNodeBase), "CompositeValidatorNode")]
	[SelectedImage(typeof(CompositeValidatorNodeBase), "CompositeValidatorNode")]
	public abstract class CompositeValidatorNodeBase : ValidatorNodeBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CompositeValidatorNodeBase"/> class.
		/// </summary>
		/// <param name="name"></param>
		protected CompositeValidatorNodeBase(string name)
			: base(name)
		{
		}

		/// <summary>
		/// Returns false, since the contents of this configuration node is ordered.
		/// </summary>
		public override bool SortChildren
		{
			get
			{
				return false;
			}
		}
	}
}
