#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Data.SqlClient;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Tests
{
    internal static class FakeSqlExceptionGenerator
    {
        public static SqlException[] GenerateFakeSqlExceptions(params int[] errorCodes)
        {
            SqlException[] exceptions = new SqlException[errorCodes.Length];

            for (int i = 0; i < errorCodes.Length; i++)
            {
                exceptions[i] = GenerateFakeSqlException(errorCodes[i]);
            }

            return exceptions;
        }

        public static SqlException GenerateFakeSqlException(int errorCode)
        {
            SqlError sqlError = GenerateFakeSqlError(errorCode);
            SqlErrorCollection collection = GenerateFakeSqlErrorCollection(sqlError);

            return (SqlException)Activator.CreateInstance(typeof(SqlException), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { string.Empty, collection }, null);
        }

        public static SqlException GenerateFakeSqlException(int errorCode, string errorMessage)
        {
            SqlError sqlError = GenerateFakeSqlError(errorCode, errorMessage);
            SqlErrorCollection collection = GenerateFakeSqlErrorCollection(sqlError);

            return (SqlException)Activator.CreateInstance(typeof(SqlException), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { errorMessage, collection }, null);
        }

        private static SqlErrorCollection GenerateFakeSqlErrorCollection(params SqlError[] errors)
        {
            Type type = typeof(SqlErrorCollection);

            SqlErrorCollection collection = (SqlErrorCollection)Activator.CreateInstance(type, true);

            MethodInfo method = type.GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (SqlError error in errors)
            {
                method.Invoke(collection, new object[] { error });
            }

            return collection;
        }

        public static SqlError GenerateFakeSqlError(int errorCode)
        {
            return (SqlError)Activator.CreateInstance(typeof(SqlError), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { errorCode, default(byte), default(byte), string.Empty, string.Empty, string.Empty, 0 }, null);
        }

        public static SqlError GenerateFakeSqlError(int errorCode, string errorMessage)
        {
            return (SqlError)Activator.CreateInstance(typeof(SqlError), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { errorCode, default(byte), default(byte), string.Empty, errorMessage, string.Empty, 0 }, null);
        }
    }
}
