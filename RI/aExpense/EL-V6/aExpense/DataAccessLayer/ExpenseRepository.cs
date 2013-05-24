#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Services;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Permissions;
using AExpense.Instrumentation;
using AExpense.Model;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.Utility;

namespace AExpense.DataAccessLayer
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly string expenseDatabaseConnectionString;
        private ObjectCache cache;
        private TimeSpan expiration;
        private const string UserKey = "User";
        private const string ApproverKey = "Approver";
        private ExceptionManager exManager;
        private IExpenseDataContext expenseDataContext;
        private RetryPolicy retryPolicy;

        [Dependency]
        public ValidatorFactory ValidatorFactory { get; set; }

        [InjectionConstructor]
        public ExpenseRepository()
        {
            this.Initialize();
            this.expenseDatabaseConnectionString = ConfigurationManager.ConnectionStrings["aExpense"].ConnectionString;
            AExpenseEvents.Log.ExpenseRepositoryInitialized();
        }

        // This overload is for testing purpose only.
        public ExpenseRepository(IExpenseDataContext expenseDataContext)
        {
            this.Initialize();
            this.expenseDataContext = expenseDataContext;
            this.ValidatorFactory = ServiceLocator.Current.GetInstance<ValidatorFactory>();
        }

        // This overload is for testing purpose only.
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public ExpenseRepository(string expenseDatabaseConnectionString, TimeSpan? cacheExpiration = null)
        {
            Initialize(cacheExpiration);
            this.expenseDatabaseConnectionString = expenseDatabaseConnectionString;
            AExpenseEvents.Log.ExpenseRepositoryInitialized();
            this.ValidatorFactory = ServiceLocator.Current.GetInstance<ValidatorFactory>();
        }

        [Tag("SaveExpensePolicyRule")] 
        public virtual void SaveExpense(Model.Expense expense)
        {
            Guard.ArgumentNotNull(expense, "expense");

            this.ValidateFromRuleset(expense);

            exManager.Process(() =>
            {
                AExpenseEvents.Log.SaveExpenseStarted(expense.Id, expense.Title);
                ProcessInContext<object>((db) =>
                {
                    AExpenseEvents.Log.SaveExpenseInsertStarted(expense.Id, expense.Title);
                    var entity = expense.ToEntity();
                    db.Expenses.InsertOnSubmit(entity);

                    foreach (var detail in expense.Details)
                    {
                        var detailEntity = detail.ToEntity(expense);
                        db.ExpenseDetails.InsertOnSubmit(detailEntity);
                    }

                    this.retryPolicy.ExecuteAction(() => db.SubmitChanges());

                    AExpenseEvents.Log.SaveExpenseInsertFinished(expense.Id, expense.Title);
                    return null;
                });

                UpdateCache(expense);
                AExpenseEvents.Log.SaveExpenseFinished(expense.Id, expense.Title);
            },
            Constants.NotifyPolicy);
        }

        public IEnumerable<Model.Expense> GetAllExpenses()
        {
            AExpenseEvents.Log.GetAllExpensesStarted();

            IList<Model.Expense> expenses = ProcessInContext((db) =>
                {
                    var exp = from e in db.Expenses
                              select e.ToModel();

                    return this.retryPolicy.ExecuteAction(() => exp.ToList());
                });

            AExpenseEvents.Log.GetAllExpensesFinished(expenses.Count);

            return expenses;
        }

        public IEnumerable<Model.Expense> GetExpensesForApproval(string approverName)
        {
            AExpenseEvents.Log.GetExpensesForApprovalStarted(approverName);

            IList<Model.Expense> result = cache.Get(ApproverKey + approverName) as IList<Model.Expense>;

            if (result == null)
            {
                AExpenseEvents.Log.GetExpensesForApprovalQueryStarted(approverName);
                result = (IList<Model.Expense>)ProcessInContext((db) =>
                {
                    var exp = from e in db.Expenses
                              where e.Approver == approverName
                              select e.ToModel();

                    return this.retryPolicy.ExecuteAction<IEnumerable<Model.Expense>>(() => exp.ToList());
                });

                AExpenseEvents.Log.GetExpensesForApprovalQueryFinished(approverName, result != null ? result.Count : 0);

                if (result != null)
                {
                    cache.Add(ApproverKey + approverName, result, GetExpiration());
                    AExpenseEvents.Log.GetExpensesForApprovalCacheUpdate(approverName);
                }
                else
                {
                    AExpenseEvents.Log.GetExpensesForApprovalQueryNoResults(approverName);
                }
            }
            else
            {
                AExpenseEvents.Log.GetExpensesForApprovalCacheHit(approverName);
            }

            AExpenseEvents.Log.GetExpensesForApprovalFinished(approverName, result != null ? result.Count : 0);
            return result;
        }

        public IEnumerable<Model.Expense> GetExpensesByUser(string userName)
        {
            IEnumerable<Model.Expense> result = cache.Get(UserKey + userName) as IEnumerable<Model.Expense>;

            if (result == null)
            {
                result = ProcessInContext((db) =>
                {
                    var exp = from e in db.Expenses
                              where e.UserName == userName
                              select e.ToModel();

                    return this.retryPolicy.ExecuteAction<IEnumerable<Model.Expense>>(() => exp.ToList());
                });

                if (result != null)
                {
                    cache.Add(UserKey + userName, result, GetExpiration());
                }
            }

            return result;
        }

        public Model.Expense GetExpenseById(Guid expenseId)
        {
            Model.Expense result = cache.Get(expenseId.ToString()) as Model.Expense;

            if (result == null)
            {
                result = ProcessInContext((db) =>
                {
                    var entity = (from e in db.Expenses
                                  where e.Id == expenseId
                                  select e).SingleOrDefault();

                    return this.retryPolicy.ExecuteAction<Model.Expense>(() => entity.ToModel());
                });

                if (result != null)
                {
                    cache.Add(expenseId.ToString(), result, GetExpiration());
                }
            }

            return result;
        }

        [ClaimsPrincipalPermission(SecurityAction.Demand, Operation = "UpdateApproved", Resource = "ExpenseRepository.UpdateApproved")]
        public void UpdateApproved(Model.Expense expense)
        {
            Guard.ArgumentNotNull(expense, "expense");

            Model.Expense udpatedExpense = ProcessInContext((db) =>
            {
                var exp = db.Expenses.SingleOrDefault(e => e.Id == expense.Id);
                if (exp == null)
                {
                    return null;
                }

                exp.Approved = expense.Approved;
                return this.retryPolicy.ExecuteAction(() =>
                    {
                        db.SubmitChanges();                        
                        return exp.ToModel();
                    });
            });

            UpdateCache(udpatedExpense);
            AExpenseEvents.Log.ExpenseApproved(udpatedExpense.Title, udpatedExpense.User.UserName, udpatedExpense.ApproverName);
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private T ProcessInContext<T>(Func<IExpenseDataContext, T> funcToProcess)
        {
            var db = this.expenseDataContext ?? ((IExpenseDataContext)new ExpensesDataContext(this.expenseDatabaseConnectionString));

            try
            {
                return funcToProcess.Invoke(db);
            }
            finally
            {
                if (this.expenseDataContext == null)
                {
                    db.Dispose();
                }
            }
        }

        private void Initialize(TimeSpan? cacheExpiration = null)
        {
            // MemoryCache is a simple in-memory caching sample which does not support a distributed caching scenario in case of web farm deployments.
            // For distributed scenarios it may be replaced by any solution that supports this like "AppFabric Caching API" (http://msdn.microsoft.com/en-us/library/hh334209.aspx)
            // The server bits for AppFabric 1.1 can be downloaded from here: http://www.microsoft.com/en-US/download/details.aspx?id=27115 
            // This can be implemented by adding a reference to the package "ServerAppFabric.Client" (https://nuget.org/packages/ServerAppFabric.Client)
            // and include a DataCache wrapper class that implemetns ObjectCache or simply replace all "cache" usage with changes like:
            // "cache.Add(expenseId.ToString(), expense, GetExpiration())" may be replaced by "cache.Add(expenseId.ToString(), expense, this.expiration)"
            // where cache is now defined as "private DataCache cache".
            this.cache = MemoryCache.Default;
            this.expiration = cacheExpiration ?? (TimeSpan)new InfiniteTimeSpanConverter().ConvertFromString(ConfigurationManager.AppSettings["CachedExpenseExpiration"]);
            this.exManager = ServiceLocator.Current.GetInstance<ExceptionManager>();

            // For a description of the scenarios supported by the Transient Fault Handling Application Block, read chapter 4 of Enterprise Library Developer's Guide.
            // To realize the benefits of the RetryPolicy, the application should be updated to use SQL Database (in the cloud) rather than SQL Server (on premises).  
            this.retryPolicy = RetryPolicyFactory.GetRetryPolicy<SqlDatabaseTransientErrorDetectionStrategy>();
        }

        // Showcase imperative validation from config rules just before saving
        private void ValidateFromRuleset(Model.Expense expense)
        {
            var validator = this.ValidatorFactory.CreateValidator<Model.Expense>("ExpenseRuleset");
            ValidationResults results = validator.Validate(expense);
            if (!results.IsValid)
            {
                throw new NotifyException(results.First().Message);
            }
        }

        private void UpdateCache(Model.Expense expense)
        {
            if (cache.Contains(expense.Id.ToString()))
            {
                cache.Remove(expense.Id.ToString());
                AExpenseEvents.Log.SaveExpenseCacheRemoved(expense.Id, expense.Id.ToString());
            }

            // Update cache and get full populated expense
            // so we can also force the update by ApproverName and/or UserName
            expense = GetExpenseById(expense.Id);

            if (!string.IsNullOrEmpty(expense.ApproverName) &&
                cache.Contains(ApproverKey + expense.ApproverName))
            {
                cache.Remove(ApproverKey + expense.ApproverName);
                AExpenseEvents.Log.SaveExpenseCacheRemoved(expense.Id, ApproverKey + expense.ApproverName);
            }

            if (expense.User != null && !string.IsNullOrEmpty(expense.User.UserName) &&
                cache.Contains(UserKey + expense.User.UserName))
            {
                cache.Remove(UserKey + expense.User.UserName);
                AExpenseEvents.Log.SaveExpenseCacheRemoved(expense.Id, UserKey + expense.User.UserName);
            }

            AExpenseEvents.Log.SaveExpenseCacheUpdated(expense.Id);
        }

        private CacheItemPolicy GetExpiration()
        {
            var policy = new CacheItemPolicy();

            if (this.expiration > TimeSpan.Zero &&
                this.expiration < TimeSpan.MaxValue)
            {
                policy.AbsoluteExpiration = DateTimeOffset.UtcNow.Add(this.expiration);
            }

            return policy;
        }
    }
}