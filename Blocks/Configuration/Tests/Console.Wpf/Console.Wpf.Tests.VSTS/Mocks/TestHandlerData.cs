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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Console.Wpf.Tests.VSTS.Mocks
{
    public class TestHandlerData : NameTypeConfigurationElement
    {
        public TestHandlerData()
        {
            this.Type = typeof(TestHandler);
        }
    }

    public class TestHandlerAnotherDerivedData : TestHandlerData
    {
    }

    public class TestHandlerDerivedData : TestHandlerData
    {
    }

    [ACustomCollectionElementAddCommandAttribute(typeof(CustomElementCollectionAddCommand))]
    [ACustomCollectionElementAddCommandAttribute(typeof(AnotherCustomElementCollectionAddCommand), Replace = CommandReplacement.NoCommand)]
    public class TestHandlerSonOfDerivedData : TestHandlerDerivedData
    {
    }

    public class CustomTestHandlerData : TestHandlerData
    {
    }

    [DisplayName(DisplayName)]
    [ACustomCollectionElementAddCommand(typeof(CustomElementCollectionAddCommand))]
    public class TestHandlerDataWithChildren : NameTypeConfigurationElement
    {
        private const string childrenProperty = "children";
        public const string DisplayName = "TestHandler";

        public TestHandlerDataWithChildren()
        {
            this.Type = typeof(TestHandler);

            this[childrenProperty] = new NamedElementCollection<TestHandlerData>();
        }

        [ConfigurationProperty(childrenProperty)]
        [ConfigurationCollection(typeof(TestHandlerData))]
        public NamedElementCollection<TestHandlerData> Children
        {
            get
            {
                return (NamedElementCollection<TestHandlerData>)this[childrenProperty];
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ACustomCollectionElementAddCommandAttribute : CommandAttribute
    {
        public ACustomCollectionElementAddCommandAttribute(Type commandType)
            : base(commandType.AssemblyQualifiedName)
        {
            this.CommandPlacement = CommandPlacement.ContextAdd;
            this.Replace = CommandReplacement.DefaultAddCommandReplacement;
        }


    }

    public class CustomElementCollectionAddCommand : DefaultCollectionElementAddCommand
    {
        public CustomElementCollectionAddCommand(ConfigurationElementType configurationElementType, ElementCollectionViewModel elementCollectionModel, IUIServiceWpf uiService) :
            base(configurationElementType, elementCollectionModel, uiService)
        {
        }
    }

    public class AnotherCustomElementCollectionAddCommand : DefaultCollectionElementAddCommand
    {
        public AnotherCustomElementCollectionAddCommand(ConfigurationElementType configurationElementType, ElementCollectionViewModel elementCollectionModel, IUIServiceWpf uiService) :
            base(configurationElementType, elementCollectionModel, uiService)
        {
        }
    }
}
