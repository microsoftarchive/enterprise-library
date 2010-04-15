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

using System.Linq;
using System.Windows.Input;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.Controls.given_elementcontainer
{
    [TestClass]
    public class when_creating_new_container : ContainerContext
    {
        private SectionViewModel exceptionSection;
        private ElementModelContainer elementContainer;
        private ElementViewModel policy;

        protected override void Arrange()
        {
            base.Arrange();

            var source = new DesignDictionaryConfigurationSource();
            new TestConfigurationBuilder().AddExceptionSettings().Build(source);

            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(source);
            exceptionSection =
                sourceModel.Sections.Where(x => x.ConfigurationType == typeof(ExceptionHandlingSettings)).Single();

            policy = exceptionSection.DescendentElements()
                .Where(x => x.ConfigurationType == typeof(ExceptionPolicyData))
                .First();

            this.Container.RegisterInstance(new Mock<IAssemblyDiscoveryService>().Object);
        }

        protected override void Act()
        {
            elementContainer = new ElementModelContainer
                                       {
                                           DataContext = policy
                                       };
        }

        [TestMethod]
        public void then_input_bindings_keys_mapped_to_commands()
        {
            var converter = new KeyGestureConverter();
            var commandAndGestures = elementContainer.InputBindings.OfType<InputBinding>()
                .Select(x =>
                            new
                            {
                                x.Command,
                                Gesture = converter.ConvertTo((KeyGesture)x.Gesture, typeof(string))
                            });

            Assert.IsTrue(commandAndGestures.Any(x => policy.Commands.Any(y => y == x.Command && y.KeyGesture == x.Gesture.ToString())));
        }
    }
}
