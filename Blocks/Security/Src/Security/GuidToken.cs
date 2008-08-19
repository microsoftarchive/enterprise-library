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

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// Implementation of <c>IToken</c> for a <c>Guid</c>.
    /// </summary>
    public class GuidToken : IToken
    {
        private Guid guid;

        /// <summary>
        /// Creates a GuidToken with a new <c>Guid</c>.
        /// </summary>
        public GuidToken()
        {
            guid = Guid.NewGuid();
        }

        /// <summary>
        /// Creates a GuidToken with a defined <c>Guid</c>.
        /// </summary>
        /// <param name="guid">User-provided <c>Guid</c></param>
        public GuidToken(Guid guid)
        {
            this.guid = guid;
        }

        /// <summary>
        /// Returns the ToString representation of the <c>Guid</c>.
        /// </summary>
        public string Value
        {
            get { return guid.ToString(); }
        }
    }
}