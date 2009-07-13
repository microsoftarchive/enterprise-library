//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// Represents the operation of mapping a <see cref="IDataReader"/> to an enumerable of <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The element type this result set mapper will be mapping to.</typeparam>
    public interface IResultSetMapper<TResult>
    {

        /// <summary>
        /// When implemented by a class, returns an enumerable of <typeparamref name="TResult"/> based on <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> to map.</param>
        /// <returns>The enurable of <typeparamref name="TResult"/> that is based on <paramref name="reader"/>.</returns>
        IEnumerable<TResult> MapSet(IDataReader reader);
    
    }
}
