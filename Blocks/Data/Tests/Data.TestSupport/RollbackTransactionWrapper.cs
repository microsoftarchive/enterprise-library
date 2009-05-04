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
using System.Data.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Data.TestSupport
{
    /// <summary>
    /// Used by a few test fixtures to simplify the code to rollback a transaction
    /// </summary>
    public class RollbackTransactionWrapper : IDisposable
    {
        private DbTransaction transaction;

        public RollbackTransactionWrapper(DbTransaction transaction)
        {
            this.transaction = transaction;
        }

        public void Dispose()
        {
            transaction.Rollback();
        }

        public DbTransaction Transaction
        {
            get { return transaction; }
        }
    }
}

