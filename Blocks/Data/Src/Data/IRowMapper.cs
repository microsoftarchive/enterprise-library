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
    /// Represents the operation of mapping a <see cref="IDataRecord"/> to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type this row mapper will be mapping to.</typeparam>
    /// <seealso cref="ReflectionRowMapper&lt;TResult&gt;"/>
    public interface IRowMapper<TResult>
    {
        /// <summary>
        /// When implemented by a class, returns a new <typeparamref name="TResult"/> based on <paramref name="row"/>.
        /// </summary>
        /// <param name="row">The <see cref="IDataRecord"/> to map.</param>
        /// <returns>The instance of <typeparamref name="TResult"/> that is based on <paramref name="row"/>.</returns>
        TResult MapRow(IDataRecord row);

    }
}
