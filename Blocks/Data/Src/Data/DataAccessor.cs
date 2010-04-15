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

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// An interface representing an object that wraps a database operation.
    /// An Accessor is executed, at which point it will go out to the database
    /// and return a <see cref="IEnumerable{TResult}"/> of whatever type <typeparam name="TResult"/>
    /// is.
    /// </summary>
    public abstract class DataAccessor<TResult>
    {
        /// <summary>
        /// Execute the database operation synchronously, returning the
        /// <see cref="IEnumerable{TResult}"/> sequence containing the
        /// resulting objects.
        /// </summary> 
        /// <param name="parameterValues">Parameters to pass to the database.</param>
        /// <returns>The sequence of result objects.</returns>
        public abstract IEnumerable<TResult> Execute(params object[] parameterValues);
        
        /// <summary>Begin executing the database object asynchronously, returning
        /// a <see cref="IAsyncResult"/> object that can be used to retrieve
        /// the result set after the operation completes.</summary>
        /// <param name="callback">Callback to execute when the operation's results are available. May
        /// be null if you don't wish to use a callback.</param>
        /// <param name="state">Extra information that will be passed to the callback. May be null.</param>
        /// <param name="parameterValues">Parameters to pass to the database.</param>
        /// <remarks>This operation will throw if the underlying <see cref="Database"/> object does not
        /// support asynchronous operation.</remarks>
        /// <exception cref="InvalidOperationException">The underlying database does not support asynchronous operation.</exception>
        /// <returns>The <see cref="IAsyncResult"/> representing this operation.</returns>
        public abstract IAsyncResult BeginExecute(AsyncCallback callback, object state, params object[] parameterValues);

        /// <summary>Complete an operation started by <see cref="BeginExecute"/>.</summary>
        /// <returns>The result sequence.</returns>
        public abstract IEnumerable<TResult> EndExecute(IAsyncResult asyncResult);
    }
}
