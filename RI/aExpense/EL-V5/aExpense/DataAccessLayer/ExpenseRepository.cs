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
using System.Globalization;
using System.Linq;
using AExpense.Model;
using AExpense.Security;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
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
        private ICacheManager cache;
        private TimeSpan expiration;
        private const string UserKey = "User";
        private const string ApproverKey = "Approver";
        private ExceptionManager exManager;
        private IExpenseDataContext expenseDataContext;

        [Dependency]
        public ValidatorFactory ValidatorFactory { get; set; }

        /// <summary>
        /// Default initializer with configuration settings.
        /// </summary>
        [InjectionConstructor]
        public ExpenseRepository()
        {
            this.Initialize();
            this.expenseDatabaseConnectionString = ConfigurationManager.ConnectionStrings["aExpense"].ConnectionString;
        }

        // This overload is for testing purpose only.
        public ExpenseRepository(IExpenseDataContext expenseDataContext)
        {
            Initialize();
            this.expenseDataContext = expenseDataContext;
            this.ValidatorFactory = ServiceLocator.Current.GetInstance<ValidatorFactory>();
        }

        // This overload is for testing purpose only.
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public ExpenseRepository(string expenseDatabaseConnectionString, TimeSpan? cacheExpiration = null)
        {
            Initialize(cacheExpiration);
            this.expenseDatabaseConnectionString = expenseDatabaseConnectionString;
            this.ValidatorFactory = ServiceLocator.Current.GetInstance<ValidatorFactory>();
        }

        [Tag("SaveExpensePolicyRule")] 
        public virtual void SaveExpense(Model.Expense expense)
        {
            Guard.ArgumentNotNull(expense, "expense");

            this.ValidateFromRuleset(expense);

            exManager.Process(() =>
            {
                ProcessInContext<object>( (db)=>{
                    var entity = expense.ToEntity();
                    db.Expenses.InsertOnSubmit(entity);

                    foreach (var detail in expense.Details)
                    {
                        var detailEntity = detail.ToEntity(expense);
                        db.ExpenseDetails.InsertOnSubmit(detailEntity);
                    }

                    db.SubmitChanges();
                    return null;
                });

                UpdateCache(expense);
            },
            Constants.NotifyPolicy);
        }

        public IEnumerable<Model.Expense> GetAllExpenses()
        {
            return ProcessInContext(db => 
            {
                var exp = from e in db.Expenses select e.ToModel();
                return exp.ToList();
            });
        }

        public IEnumerable<Model.Expense> GetExpensesForApproval(string approverName)
        {
            IEnumerable<Model.Expense> result = cache.GetData(ApproverKey + approverName) as IEnumerable<Model.Expense>;

            if (result == null)
            {
                result = ProcessInContext((db) =>
                {
                    var exp = from e in db.Expenses
                              where e.Approver == approverName
                              select e.ToModel();

                    return exp.ToList();
                });

                if (result != null)
                {
                    cache.Add(ApproverKey + approverName, result, CacheItemPriority.Normal, null, GetExpiration());
                }
            }

            return result;
        }

        public IEnumerable<Model.Expense> GetExpensesByUser(string userName)
        {
            IEnumerable<Model.Expense> result = cache.GetData(UserKey + userName) as IEnumerable<Model.Expense>;

            if (result == null)
            {
                result = ProcessInContext((db) =>
                {
                    var exp = from e in db.Expenses
                              where e.UserName == userName
                              select e.ToModel();

                    return exp.ToList();
                });

                if (result != null)
                {
                    cache.Add(UserKey + userName, result, CacheItemPriority.Normal, null, GetExpiration());
                }
            }

            return result;
        }

        public Model.Expense GetExpenseById(Guid expenseId)
        {
            Model.Expense result = cache.GetData(expenseId.ToString()) as Model.Expense;

            if (result == null)
            {
                result = ProcessInContext((db) =>
                {
                    var entity = (from e in db.Expenses
                                  where e.Id == expenseId
                                  select e).SingleOrDefault();

                    return entity.ToModel();
                });

                if (result != null)
                {
                    cache.Add(expenseId.ToString(), result, CacheItemPriority.Normal, null, GetExpiration());
                }
            }

            return result;
        }

        [RulesPrincipalPermission(System.Security.Permissions.SecurityAction.Demand, Rule = "ManagersOnly")]
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
                db.SubmitChanges();               
                return exp.ToModel();
            });

            UpdateCache(udpatedExpense);

            Logger.Write(string.Format(CultureInfo.CurrentCulture, "The expense '{0}' for user '{1}' was approved by user '{2}'.", udpatedExpense.Title, udpatedExpense.User.UserName, udpatedExpense.ApproverName), Constants.ExpenseOperationsCategory);
        }

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
            this.cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>("Cache Manager");
            this.expiration = cacheExpiration ?? (TimeSpan)new InfiniteTimeSpanConverter().ConvertFromString(ConfigurationManager.AppSettings["CachedExpenseExpiration"]);
            this.exManager = EnterpriseLibraryContainer.Current.GetInstance<ExceptionManager>();
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
                cache.Remove(expense.Id.ToString());

            // Update cache and get full populated expense
            // so we can also force the update by ApproverName and/or UserName
            expense = GetExpenseById(expense.Id);

            if (!string.IsNullOrEmpty(expense.ApproverName) && 
                cache.Contains(ApproverKey + expense.ApproverName))
                cache.Remove(ApproverKey + expense.ApproverName);

            if (expense.User != null && !string.IsNullOrEmpty(expense.User.UserName) &&
                cache.Contains(UserKey + expense.User.UserName))
                cache.Remove(UserKey + expense.User.UserName);
        }

        private ICacheItemExpiration GetExpiration()
        {
            if (this.expiration > TimeSpan.Zero &&
                this.expiration < TimeSpan.MaxValue)
            {
                return new AbsoluteTime(this.expiration);
            }
            return new NeverExpired();
        }
    }
}