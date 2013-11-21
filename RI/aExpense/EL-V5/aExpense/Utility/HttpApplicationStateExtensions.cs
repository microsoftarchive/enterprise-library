// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

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