// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace AExpense.DataAccessLayer
{
    public interface IProfileStore
    {
        Dictionary<string, string> GetAttributesFor(string userName, string[] attributes);
    }
}