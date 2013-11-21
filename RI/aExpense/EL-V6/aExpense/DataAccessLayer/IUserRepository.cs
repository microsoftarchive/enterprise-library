// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using AExpense.Model;

namespace AExpense.DataAccessLayer
{
    public interface IUserRepository
    {
        User GetUser(string userName, bool throwOnError = true);
        void UpdateUserPreferredReimbursementMethod(User user);
    }
}