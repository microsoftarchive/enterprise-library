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
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AExpense
{
    /// <summary>
    /// Exception for showing user friendly exception message
    /// </summary>
    [Serializable]
    public class NotifyException : Exception
    {
        public NotifyException() : base()
        {
        }

        public NotifyException(string message) : base (message)
        {
        }

        public NotifyException(string message, Exception inner) : base(message, inner)
        {
        }

        // This constructor is needed for serialization.
        protected NotifyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}