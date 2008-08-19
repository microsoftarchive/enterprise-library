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
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms
{
    /// <summary>
    /// Contains the data for the <see cref="ValidationProvider.ValidationPerformed"/> event.
    /// </summary>
    /// <seealso cref="ValidationProvider.ValidationPerformed"/>
	public class ValidationPerformedEventArgs : EventArgs
	{
		private Control validatedControl;
		private ValidationResults validationResults;

		internal ValidationPerformedEventArgs(Control validatedControl, ValidationResults validationResults)
		{
			this.validatedControl = validatedControl;
			this.validationResults = validationResults;
		}

        /// <summary>
        /// Gets the control that was validated.
        /// </summary>
		public Control ValidatedControl
		{
			get { return validatedControl; }
		}

        /// <summary>
        /// Gets the results of the validation for the control.
        /// </summary>
		public ValidationResults ValidationResults
		{
			get { return validationResults; }
		}
	}
}