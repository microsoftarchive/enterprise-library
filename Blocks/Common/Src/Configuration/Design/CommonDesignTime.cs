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
            public const string AddSateliteProviderCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands.AddSateliteProviderCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
            
            /// <summary/>
            public const string AddApplicationBlockCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Commands.AddApplicationBlockCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary/>
            public const string AddProviderUsingTypePickerCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.TypePickingCollectionElementAddCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary/>
            public const string ExportAdmTemplateCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.ExportAdmTemplateCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary/>
            public const string HiddenCommand = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Commands.HiddenCommand, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }


        /// <summary/>
        public static class EditorTypes
        {
            /// <summary/>
            public const string DatePickerEditor = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors.DatePickerEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
            
            ///<summary/>
            public const string CollectionEditor = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors.ElementCollectionEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary/>
            public const string UITypeEditor = "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

            /// <summary/>
            public const string TypeSelector = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors.TypeSelectorEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary/>
            public const string FilteredFilePath = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors.FilteredFileNameEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary/>
            public const string FrameworkElement = "System.Windows.FrameworkElement, PresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";

            /// <summary>
            /// 
            /// </summary>
            public const string MultilineText = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors.MultilineTextEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary>
            /// 
            /// </summary>
            public const string Flags = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors.FlagsEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

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
            public const string TemplateEditor = "Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters.TemplateEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary>
            /// 
            /// </summary>
            public const string OverridesEditor = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.IEnvironmentalOverridesEditor, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }


        /// <summary/>
        public static class ViewModelTypeNames
        {

            ///<summary/>
            public const string ConfigurationPropertyViewModel =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.ConfigurationProperty, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            /// <summary/>
            public const string SectionViewModel = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.SectionViewModel, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        }

        ///<summary>
        ///</summary>
        public static class ValidationTypeNames
        {
            /// <summary>
            /// </summary>
            public const string FileValidator = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.FilePathValidator, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary>
            ///</summary>
            public const string RequiredFieldValidator = "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.RequiredFieldValidator, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";

            ///<summary>
            /// Type name for <see cref="TypeValidator"/>.
            ///</summary>
            public const string TypeValidator =
                "Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.TypeValidator, Microsoft.Practices.EnterpriseLibrary.Configuration.Design";
        } 
    }
}
