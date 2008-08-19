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
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
    /// <summary>
    /// Allows for multiple member selection.
    /// </summary>
    public class TypeMemberChooser
    {
        IUIService uiService;

        /// <summary>
		/// Initializes a new instance of the <see cref="TypeMemberChooser"/> class.
        /// </summary>
		/// <param name="uiService">The <see cref="IUIService"/> to perform UI operations.</param>
        public TypeMemberChooser(IUIService uiService)
        {
            this.uiService = uiService;
        }

        /// <summary>
		/// Launches a <see cref="TypeMemberChooserUI"/> to perform the selection of multiple members.
        /// </summary>
        /// <param name="type">The type for which the members are to be selected.</param>
        /// <returns>A colleciton with the selected members.</returns>
        public IEnumerable<MemberInfo> ChooseMembers(Type type)
        {
            using (TypeMemberChooserUI chooserUI = new TypeMemberChooserUI(type, uiService))
            {
                if (DialogResult.OK == chooserUI.ShowDialog(uiService.OwnerWindow))
                {
                    return chooserUI.GetSelectedMembers();
                }
                else
                {
                    return new List<MemberInfo>();
                }
            }
        }
    }
}
