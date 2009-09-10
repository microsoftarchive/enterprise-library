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
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using Console.Wpf.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Windows.Input;

namespace Console.Wpf.ViewModel
{
    public class ElementCollectionViewModelAdder : ICommand
    {
        private readonly ElementCollectionViewModel elementCollectionModel;
        private readonly string displayName;
        private readonly string helpText;
        private readonly Type configurationElementType;

        public ElementCollectionViewModelAdder(Type configurationElementType, ElementCollectionViewModel elementCollectionModel)
        {
            this.configurationElementType = configurationElementType;
            this.elementCollectionModel = elementCollectionModel;

            displayName = GetDisplayName(configurationElementType);
            helpText = GetHelpText(configurationElementType);
        }

        public string DisplayName
        {
            get { return displayName; }
        }

        public string HelpText
        {
            get { return helpText; }
        }


        public virtual void Execute(object parameter)
        {
            elementCollectionModel.CreateNewChildElement(configurationElementType);
        }

        public virtual bool CanExecute(object parameter) 
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if(handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private static string GetDisplayName(Type configurationElementType)
        {
            return GetStringFromAttribute<DisplayNameAttribute>(configurationElementType, attr => attr.DisplayName, configurationElementType.Name);
        }

        private static string GetHelpText(Type configurationElementType)
        {
            return GetStringFromAttribute<DescriptionAttribute>(configurationElementType, attr => attr.Description);
        }

        private static string GetStringFromAttribute<TAttribute>(Type configurationElementType, Func<TAttribute, string> stringRetriever)
            where TAttribute : Attribute
        {
            return GetStringFromAttribute<TAttribute>(configurationElementType, stringRetriever, string.Empty);
        }

        private static string GetStringFromAttribute<TAttribute>(Type configurationElementType, Func<TAttribute, string> stringRetriever, string defaultValue)
            where TAttribute : Attribute
        {
            var attr = TypeDescriptor.GetAttributes(configurationElementType).OfType<TAttribute>().FirstOrDefault();
                
            if (attr == null)
            {
                return defaultValue;
            }
            
            return stringRetriever(attr);
        }
    }
}
