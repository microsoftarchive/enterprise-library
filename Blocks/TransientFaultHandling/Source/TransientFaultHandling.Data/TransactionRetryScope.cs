#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Transactions;

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling
{
    /// <summary>
    /// Provides support for retry policy-aware transactional scope.
    /// </summary>
    public sealed class TransactionRetryScope : IDisposable
    {
        private readonly RetryPolicy retryPolicy;
        private readonly TransactionScopeInitializer transactionScopeInit;
        private readonly Action unitOfWork;
        private TransactionScope transactionScope;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionRetryScope"/> class. 
        /// Implements no retry policy, but just invokes the unit of work exactly once.
        /// </summary>
        /// <param name="unitOfWork">A delegate that represents the executable unit of work that will be retried upon failure.</param>
        public TransactionRetryScope(Action unitOfWork)
            : this(TransactionScopeOption.Required, unitOfWork)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionRetryScope"/> class with the specified retry policy.
        /// </summary>
        /// <param name="retryPolicy">The retry policy that determines whether to retry the execution of the entire scope if a transient fault is encountered.</param>
        /// <param name="unitOfWork">A delegate that represents the executable unit of work that will be retried upon failure.</param>
        public TransactionRetryScope(RetryPolicy retryPolicy, Action unitOfWork)
            : this(TransactionScopeOption.Required, retryPolicy, unitOfWork)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionRetryScope"/> class with the specified requirements.
        /// Implements no retry policy, but just invokes the unit of work exactly once.
        /// </summary>
        /// <param name="scopeOption">One of the enumeration values that specifies the transaction requirements associated with this transaction scope.</param>
        /// <param name="unitOfWork">A delegate that represents the executable unit of work that will be retried upon failure.</param>
        public TransactionRetryScope(TransactionScopeOption scopeOption, Action unitOfWork)
            : this(scopeOption, RetryPolicy.NoRetry, unitOfWork)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionRetryScope"/> class with the specified time-out value and requirements.
        /// Implements no retry policy, but just invokes the unit of work exactly once.
        /// </summary>
        /// <param name="scopeOption">One of the enumeration values that specifies the transaction requirements associated with this transaction scope.</param>
        /// <param name="scopeTimeout">The TimeSpan after which the transaction scope times out and aborts the transaction.</param>
        /// <param name="unitOfWork">A delegate that represents the executable unit of work that will be retried upon failure.</param>
        public TransactionRetryScope(TransactionScopeOption scopeOption, TimeSpan scopeTimeout, Action unitOfWork)
            : this(scopeOption, scopeTimeout, RetryPolicy.NoRetry, unitOfWork)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionRetryScope"/> class with the specified time-out value, transaction scope options, and retry policy.
        /// Uses the ReadCommitted isolation level by default.
        /// </summary>
        /// <param name="scopeOption">One of the enumeration values that specifies the transaction requirements associated with this transaction scope.</param>
        /// <param name="scopeTimeout">The TimeSpan after which the transaction scope times out and aborts the transaction.</param>
        /// <param name="retryPolicy">The retry policy that determines whether to retry the execution of the entire scope if a transient fault is encountered.</param>
        /// <param name="unitOfWork">A delegate that represents the executable unit of work that will be retried upon failure.</param>
        public TransactionRetryScope(TransactionScopeOption scopeOption, TimeSpan scopeTimeout, RetryPolicy retryPolicy, Action unitOfWork)
        {
            this.transactionScopeInit = () =>
            {
                var txOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = scopeTimeout };

                return new TransactionScope(scopeOption, txOptions);
            };

            this.transactionScope = this.transactionScopeInit();
            this.retryPolicy = retryPolicy;
            this.unitOfWork = unitOfWork;

            // Set up the callback method for the specified retry policy.
            this.InitializeRetryPolicy();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionRetryScope"/> class with the specified requirements.
        /// Implements no retry policy, but just invokes the unit of work exactly once.
        /// </summary>
        /// <param name="scopeOption">One of the enumeration values that specifies the transaction requirements associated with this transaction scope.</param>
        /// <param name="transactionOptions">A <see cref="System.Transactions.TransactionOptions"/> structure that describes the transaction options to use if a new transaction is created. If an existing transaction is used, the time-out value in this parameter applies to the transaction scope. If that time expires before the scope is disposed, the transaction is aborted.</param>
        /// <param name="unitOfWork">A delegate that represents the executable unit of work that will be retried upon failure.</param>
        public TransactionRetryScope(TransactionScopeOption scopeOption, TransactionOptions transactionOptions, Action unitOfWork)
            : this(scopeOption, transactionOptions, RetryPolicy.NoRetry, unitOfWork)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionRetryScope"/> class with the specified requirements and retry policy.
        /// </summary>
        /// <param name="scopeOption">One of the enumeration values that specifies the transaction requirements associated with this transaction scope.</param>
        /// <param name="transactionOptions">A <see cref="System.Transactions.TransactionOptions"/> structure that describes the transaction options to use if a new transaction is created. If an existing transaction is used, the time-out value in this parameter applies to the transaction scope. If that time expires before the scope is disposed, the transaction is aborted.</param>
        /// <param name="retryPolicy">The retry policy that determines whether to retry the execution of the entire scope if a transient fault is encountered.</param>
        /// <param name="unitOfWork">A delegate that represents the executable unit of work that will be retried upon failure.</param>
        public TransactionRetryScope(TransactionScopeOption scopeOption, TransactionOptions transactionOptions, RetryPolicy retryPolicy, Action unitOfWork)
        {
            this.transactionScopeInit = () => new TransactionScope(scopeOption, transactionOptions);

            this.transactionScope = this.transactionScopeInit();
            this.retryPolicy = retryPolicy;
            this.unitOfWork = unitOfWork;

            // Set up the callback method for the specified retry policy.
            this.InitializeRetryPolicy();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionRetryScope"/> class with the specified requirements and retry policy.
        /// Uses the ReadCommitted isolation level by default.
        /// </summary>
        /// <param name="scopeOption">One of the enumeration values that specifies the transaction requirements associated with this transaction scope.</param>
        /// <param name="retryPolicy">The retry policy that determines whether to retry the execution of the entire scope if a transient fault is encountered.</param>
        /// <param name="unitOfWork">A delegate that represents the executable unit of work that will be retried upon failure.</param>
        public TransactionRetryScope(TransactionScopeOption scopeOption, RetryPolicy retryPolicy, Action unitOfWork)
            : this(scopeOption, TimeSpan.Zero, retryPolicy, unitOfWork)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionRetryScope"/> class and sets the specified transaction as the ambient transaction, 
        /// so that transactional work performed inside the scope uses this transaction. Implements no retry policy, but just invokes the unit of work exactly once.
        /// </summary>
        /// <param name="tx">The transaction to be set as the ambient transaction, so that transactional work performed inside the scope uses this transaction.</param>
        /// <param name="unitOfWork">A delegate that represents the executable unit of work that will be retried upon failure.</param>
        public TransactionRetryScope(Transaction tx, Action unitOfWork)
            : this(tx, RetryPolicy.NoRetry, unitOfWork)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionRetryScope"/> class with the specified retry policy and sets the specified transaction as the ambient transaction, 
        /// so that transactional work performed inside the scope uses this transaction.
        /// </summary>
        /// <param name="tx">The transaction to be set as the ambient transaction, so that transactional work performed inside the scope uses this transaction.</param>
        /// <param name="retryPolicy">The retry policy that determines whether to retry the execution of the entire scope if a transient fault is encountered.</param>
        /// <param name="unitOfWork">A delegate that represents the executable unit of work that will be retried upon failure.</param>
        public TransactionRetryScope(Transaction tx, RetryPolicy retryPolicy, Action unitOfWork)
        {
            this.transactionScopeInit = () => new TransactionScope(tx);

            this.transactionScope = this.transactionScopeInit();
            this.retryPolicy = retryPolicy;
            this.unitOfWork = unitOfWork;

            // Set up the callback method for the specified retry policy.
            this.InitializeRetryPolicy();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionRetryScope"/> class with the specified time-out value, and sets the specified transaction as the ambient transaction, 
        /// so that transactional work performed inside the scope uses this transaction. Implements no retry policy, but just invokes the unit of work exactly once.
        /// </summary>
        /// <param name="tx">The transaction to be set as the ambient transaction, so that transactional work performed inside the scope uses this transaction.</param>
        /// <param name="scopeTimeout">The TimeSpan after which the transaction scope times out and aborts the transaction.</param>
        /// <param name="unitOfWork">A delegate that represents the executable unit of work that will be retried upon failure.</param>
        public TransactionRetryScope(Transaction tx, TimeSpan scopeTimeout, Action unitOfWork)
            : this(tx, scopeTimeout, RetryPolicy.NoRetry, unitOfWork)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionRetryScope"/> class with the specified time-out value, and sets the specified transaction as the ambient transaction, 
        /// so that transactional work performed inside the scope uses this transaction. Uses the specified retry policy.
        /// </summary>
        /// <param name="tx">The transaction to be set as the ambient transaction, so that transactional work performed inside the scope uses this transaction.</param>
        /// <param name="scopeTimeout">The TimeSpan after which the transaction scope times out and aborts the transaction.</param>
        /// <param name="retryPolicy">The retry policy that determines whether to retry the execution of the entire scope if a transient fault is encountered.</param>
        /// <param name="unitOfWork">A delegate that represents the executable unit of work that will be retried upon failure.</param>
        public TransactionRetryScope(Transaction tx, TimeSpan scopeTimeout, RetryPolicy retryPolicy, Action unitOfWork)
        {
            this.transactionScopeInit = () => new TransactionScope(tx, scopeTimeout);

            this.transactionScope = this.transactionScopeInit();
            this.retryPolicy = retryPolicy;
            this.unitOfWork = unitOfWork;

            // Set up the callback method for the specified retry policy.
            this.InitializeRetryPolicy();
        }
        #endregion

        private delegate TransactionScope TransactionScopeInitializer();

        #region Public properties
        /// <summary>
        /// Gets the policy that determines whether to retry the execution of the entire scope if a transient fault is encountered.
        /// </summary>
        public RetryPolicy RetryPolicy
        {
            get { return this.retryPolicy; }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Executes the underlying unit of work and retries as prescribed by the current retry policy.
        /// </summary>
        public void InvokeUnitOfWork()
        {
            this.retryPolicy.ExecuteAction(this.unitOfWork);
        }

        /// <summary>
        /// Indicates that all operations within the scope have been completed successfully.
        /// </summary>
        public void Complete()
        {
            // Invoke the main method to indicate that all operations within the scope are completed successfully.
            if (this.transactionScope != null)
            {
                this.transactionScope.Complete();
            }
        }
        #endregion

        #region IDisposable implementation
        /// <summary>
        /// Ends the transaction scope.
        /// </summary>
        public void Dispose()
        {
            if (this.transactionScope != null)
            {
                this.transactionScope.Dispose();
            }
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Configures the specified retry policy to work with a transactional scope.
        /// </summary>
        private void InitializeRetryPolicy()
        {
            this.retryPolicy.Retrying += (sender, args) =>
            {
                try
                {
                    // Should recycle the scope upon failure. This will also rollback the entire transaction.
                    if (this.transactionScope != null)
                    {
                        this.transactionScope.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    // Something went wrong when disposing the transactional scope, we should interrupt the retry cycle.
#pragma warning disable 0618
                    throw new RetryLimitExceededException(ex);
#pragma warning restore 0618
                }

                // Get a new instance of a transactional scope.
                this.transactionScope = this.transactionScopeInit();
            };
        }
        #endregion
    }
}
