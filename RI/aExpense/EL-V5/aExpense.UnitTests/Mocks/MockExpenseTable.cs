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
