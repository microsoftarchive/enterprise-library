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

using System.Web;
using Microsoft.Practices.Unity;

namespace AExpense
{
    internal static class HttpApplicationStateExtensions
    {
        private const string GlobalContainerKey = "EntLibContainer";

        public static void SetContainer(this HttpApplicationState appState, IUnityContainer container)
        {
            appState[GlobalContainerKey] = container;
        }

        public static IUnityContainer GetContainer(this HttpApplicationState appState)
        {
            return appState[GlobalContainerKey] as IUnityContainer;
        }
    }
}