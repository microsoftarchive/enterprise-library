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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Represents the configuration settings for any trace listener.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "SystemDiagnosticsTraceListenerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "SystemDiagnosticsTraceListenerDataDisplayName")]
    [ViewModel(LoggingDesignTime.ViewModelTypeNames.SystemDiagnosticsTraceListenerDataViewModel)]
    [OmitCustomAttributesPropertyAttribute]
    public class SystemDiagnosticsTraceListenerData
        : BasicCustomTraceListenerData
    {
        /// <summary>
        /// Initializes with default values.
        /// </summary>
        public SystemDiagnosticsTraceListenerData()
        {
            ListenerDataType = typeof(SystemDiagnosticsTraceListenerData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SystemDiagnosticsTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="initData">The init data.</param>
        public SystemDiagnosticsTraceListenerData(string name, Type type, string initData)
            : base(name, type, initData)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:SystemDiagnosticsTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="typeName">The type.</param>
        /// <param name="initData">The init data.</param>
        public SystemDiagnosticsTraceListenerData(string name, string typeName, string initData)
            : base(name, typeName, initData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SystemDiagnosticsTraceListenerData"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="initData">The init data.</param>
        /// <param name="traceOutputOptions">The trace output options.</param>
        public SystemDiagnosticsTraceListenerData(string name, Type type, string initData,
                                                  TraceOptions traceOutputOptions)
            : base(name, type, initData, traceOutputOptions)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(System.Diagnostics.TraceListener))]
        [System.ComponentModel.Browsable(true)]
        [DesignTimeReadOnly(false)]
        [ViewModel(LoggingDesignTime.ViewModelTypeNames.TypeNameElementProperty)]
        public override string TypeName
        {
            get
            {
                return base.TypeName;
            }
            set
            {
                base.TypeName = value;
            }
        }
    }
}
