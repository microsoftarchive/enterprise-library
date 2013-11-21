// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AExpense.DataAccessLayer
{
    public interface IExpenseTable<TEntity> : IEnumerable<TEntity> where TEntity : class 
    {
        IEnumerable<TEntity> Collection { get; }
        void InsertOnSubmit(TEntity entity);
    }
}