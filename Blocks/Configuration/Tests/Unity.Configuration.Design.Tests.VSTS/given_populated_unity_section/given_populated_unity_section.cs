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
using Console.Wpf.Tests.VSTS.BlockSpecific.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;

namespace Microsoft.Practices.Unity.Configuration.Design.Tests.given_populated_unity_section
{
    public abstract class given_populated_unity_section : given_empty_application_model_unity
    {
        protected UnityConfigurationSection Section;

        protected override void Arrange()
        {
            base.Arrange();

            var ctor = new ConstructorElement();
            ctor.Parameters.Add(
                new ParameterElement
                {
                    Name = "dependencyParameter",
                    Value = new DependencyElement {Name = "dep1"}
                });

            ctor.Parameters.Add(
                new ParameterElement
                {
                    Name = "valueParameter",
                    Value = new ValueElement
                    {
                        Value = "123",
                        TypeConverterTypeName = "IntConverter"
                    }
                });

            ctor.Parameters.Add(
                new ParameterElement
                {
                    Name = "optionalParameter",
                    Value = new OptionalElement()
                });

            var registration = new RegisterElement
            {
                TypeName = "MyType"
            };

            registration.InjectionMembers.Add(ctor);

            var container = new ContainerElement();
            container.Registrations.Add(registration);

            Section = new UnityConfigurationSection();
            Section.Containers.Add(container);
        }
    }
}
