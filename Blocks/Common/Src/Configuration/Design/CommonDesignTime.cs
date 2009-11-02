//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
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

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{

    /// <summary/>
    public static class CommonDesignTime
    {
        /// <summary/>
        public static class CommandTypeNames
        {
            /// <summary/>
            public const string AddSateliteProviderCommand = "Console.Wpf.ViewModel.Commands.AddSateliteProviderCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
            
            /// <summary/>
            public const string AddApplicationBlockCommand = "Console.Wpf.ViewModel.Commands.AddApplicationBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary/>
            public const string AddProviderUsingTypePickerCommand = "Console.Wpf.ViewModel.TypePickingCollectionElementAddCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }


        /// <summary/>
        public static class EditorTypes
        {
            ///<summary>
            ///</summary>
            public const string CollectionEditor = "Console.Wpf.Controls.CollectionElementEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary>
            /// </summary>
            public const string UITypeEditor = "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

            /// <summary>
            /// 
            /// </summary>
            public const string TypeSelector = "Console.Wpf.ComponentModel.Editors.TypeSelectorEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary>
            /// 
            /// </summary>
            public const string FilteredFilePath = "Console.Wpf.ComponentModel.Editors.FilteredFileNameEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary>
            /// 
            /// </summary>
            public const string FrameworkElement = "System.Windows.FrameworkElement, PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";

            /// <summary>
            /// 
            /// </summary>
            public const string MultilineText = "Console.Wpf.ComponentModel.Editors.MultilineTextEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary>
            /// 
            /// </summary>
            public const string Flags = "Console.Wpf.ComponentModel.Editors.FlagsEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary>
            /// 
            /// </summary>
            public const string RegexTypeEditor = "System.Web.UI.Design.WebControls.RegexTypeEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

            /// <summary>
            /// 
            /// </summary>
            public const string ConnectionStringEditor = "System.Web.UI.Design.ConnectionStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

            /// <summary>
            /// 
            /// </summary>
            public const string TemplateEditor = "Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters.TemplateEditor, Console.Wpf";
        }


        /// <summary/>
        public static class ViewModelTypeNames
        {

            ///<summary/>
            public const string ConfigurationPropertyViewModel =
                "Console.Wpf.ViewModel.ConfigurationProperty, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary/>
            public const string HierarchicalViewModel = "Console.Wpf.ViewModel.HierarchicalSectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }
    }
}
