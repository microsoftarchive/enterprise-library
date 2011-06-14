//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage
{
    public class EntryComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            var xBytes = (byte[])x;
            var yBytes = (byte[])y;

            for (int i = 0; i < Math.Max(xBytes.Length, yBytes.Length); i++)
            {
                if (xBytes.Length > i && yBytes.Length > i)
                {
                    var comparison = xBytes[i].CompareTo(yBytes[i]);
                    if (comparison != 0)
                    {
                        return comparison;
                    }
                }
                else
                {
                    return xBytes.Length > i ? -1 : 1;
                }
            }

            return 0;
        }
    }
}
