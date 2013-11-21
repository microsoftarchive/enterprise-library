// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using AExpense.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AExpense.DataAccessLayer;

namespace aExpense.UnitTests.Mocks
{
    public class MockExpenseTable<TEntity> : IExpenseTable<TEntity> where TEntity: class
    {
        List<TEntity> _table;

        public MockExpenseTable(List<TEntity> table)
        {
            _table = table;
        }
        public IEnumerable<TEntity> Collection
        {
            get { throw new NotImplementedException(); }
        }

        public void InsertOnSubmit(TEntity entity)
        {
            this._table.Add(entity);
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return this._table.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._table.GetEnumerator();
        }
    }
}
