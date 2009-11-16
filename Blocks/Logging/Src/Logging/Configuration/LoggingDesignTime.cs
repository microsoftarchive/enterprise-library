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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Defines design-time constants for custom Logging view models and commands.
    /// </summary>
    public static class LoggingDesignTime
    {
        /// <summary>
        /// Custom logging view model type names.
        /// </summary>
        public static class ViewModelTypeNames
        {
            ///<summary/>
            public const string LogggingSectionViewModel = 
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.LoggingSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary/>
            public const string SystemDiagnosticsTraceListenerDataViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.SystemDiagnosticsTraceListenerDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
            
            ///<summary/>
            public const string CustomTraceListenerDataViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.CustomTraceListenerDataViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary />
            public const string TraceListenerReferenceViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.TraceListenerReferenceViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary />
            public const string TraceListenerReferenceNameProperty =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.TraceListenerReferenceNameProperty, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary/>
            public const string TraceListenerElementCollectionViewModel = 
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.TraceListenerElementCollectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary/>
            public const string CategoryFilterViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.CategoryFilterEntryViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }
        
        ///<summary/>
        public static class EditorTypeNames
        {
            ///<summary/>
            public const string OverridenTraceListenerCollectionEditor = 
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.OverridenTraceListenerCollectionEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }

        /// <summary/>
        public static class CommandTypeNames
        {
            /// <summary/>
            public const string AddLoggingBlockCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.AddLoggingBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }
    }
}
