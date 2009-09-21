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
    /// <summary>
    /// 
    /// </summary>
    public static class EditorTypes
    {
        /// <summary>
        /// 
        /// </summary>
        public const string UITypeEditor = "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

        /// <summary>
        /// 
        /// </summary>
        public const string TypeSelector = "Console.Wpf.ComponentModel.Editors.TypeSelectorEditor, Console.Wpf";
        
        /// <summary>
        /// 
        /// </summary>
        public const string FilteredFilePath = "Console.Wpf.ComponentModel.Editors.FilteredFileNameEditor, Console.Wpf";
    }


    /// <summary>
    /// 
    /// </summary>
    public static class ConverterTypes
    {
        //public const string ElementReferenceConverter = "Console.Wpf.ComponentModel.Converters.ElementReferenceConverter, Console.Wpf";
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ViewModels
    {
        /// <summary>
        /// 
        /// </summary>
        public const string TabularViewModel = "Console.Wpf.ViewModel.TabularSectionViewModel, Console.Wpf";
        
        /// <summary>
        /// 
        /// </summary>
        public const string HierarchicalViewModel = "Console.Wpf.ViewModel.HierarchicalSectionViewModel, Console.Wpf";
        
        /// <summary>
        /// 
        /// </summary>
        public const string ConfigurationSourcesSectionViewModel = "Console.Wpf.ViewModel.BlockSpecifics.ConfigurationSourceSectionViewModel, Console.Wpf";

        /// <summary>
        /// 
        /// </summary>
        public const string LogggingSectionViewModel = "Console.Wpf.ViewModel.BlockSpecifics.LoggingSectionViewModel, Console.Wpf";

        /// <summary>
        /// 
        /// </summary>
        public const string TraceListenerElementCollectionViewModel = "Console.Wpf.ViewModel.BlockSpecifics.TraceListenerElementCollectionViewModel, Console.Wpf";

        /// <summary>
        /// 
        /// </summary>
        public const string TraceListenerReferenceElementViewModel = "Console.Wpf.ViewModel.BlockSpecifics.TraceListenerReferenceViewModel, Console.Wpf";

    }
}
