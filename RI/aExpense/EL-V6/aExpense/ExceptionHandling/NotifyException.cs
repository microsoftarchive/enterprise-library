// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

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