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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration
{
    using System;
    using System.Collections.Generic;

    internal static class WellKnownRetryStrategies
    {
        public const string Incremental = "incremental";
        public const string Backoff = "exponentialBackoff";
        public const string FixedInterval = "fixedInterval";

        public static readonly Dictionary<string, Type> AllKnownRetryStrategies = new Dictionary<string, Type>()
            {
                { "incremental", typeof(IncrementalData) },
                { "exponentialBackoff", typeof(ExponentialBackoffData) },
                { "fixedInterval", typeof(FixedIntervalData) }
            };
    }
}
