using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Console.Wpf.ViewModel;
using Console.Wpf.Tests.VSTS.Mocks;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_view_model_commands
{
    public abstract class given_element_view_model_commands : ContainerContext
    {
        protected SectionViewModel Viewmodel;

        protected override void Arrange()
        {
            base.Arrange();

            SectionWithDifferentCommands section = new SectionWithDifferentCommands();
            Viewmodel = SectionViewModel.CreateSection(Container, "SectionWithDifferentCommands", section);
        }
    }

    [TestClass]
    public class when_querying_commands_on_default_element : given_element_view_model_commands
    {
        ElementViewModel elementModel;

        protected override void Act()
        {
            elementModel = Viewmodel.DescendentElements(x => x.ConfigurationType == typeof(DefaultConfigurationElement)).First();
        }

        [TestMethod]
        public void then_element_has_delete_command()
        {
            Assert.AreEqual(1, elementModel.Commands.OfType<DefaultDeleteCommandModel>().Count());
        }
    }

    [TestClass]
    public class when_querying_commands_on_element_with_custom_command : given_element_view_model_commands
    {
        ElementViewModel elementModel;

        protected override void Act()
        {
            elementModel = Viewmodel.DescendentElements(x => x.ConfigurationType == typeof(ConfigurationElementWithCustomCommand)).First();
        }

        [TestMethod]
        public void then_element_has_custom_command()
        {
            Assert.AreEqual(1, elementModel.Commands.OfType<CommandModel>().Where(x => x.Title == "Custom command").Count());
        }

        [TestMethod]
        public void then_element_has_delete_command()
        {
            Assert.AreEqual(1, elementModel.Commands.OfType<DefaultDeleteCommandModel>().Count());
        }
    }

    [TestClass]
    public class when_querying_commands_on_collection_element : given_element_view_model_commands
    {
        ElementViewModel collectionElement;

        protected override void Act()
        {
            collectionElement = Viewmodel.DescendentElements(x => x.ConfigurationType == typeof(NameValueConfigurationCollection)).First();
        }

        [TestMethod]
        public void then_element_has_add_command()
        {
            Assert.AreEqual(1, collectionElement.Commands.Where(x=>x.Placement == CommandPlacement.ContextAdd).Count());
        }


        [TestMethod]
        public void then_element_does_not_have_default_delete_command()
        {
            Assert.IsFalse(collectionElement.Commands.Any(x => x.Placement == CommandPlacement.ContextDelete));
        }
    }

    [TestClass]
    public class when_querying_commands_on_collection_element_with_custom_addCommand: given_element_view_model_commands
    {
        ElementViewModel collectionElement;

        protected override void Act()
        {
            collectionElement = Viewmodel.DescendentElements(x => x.ConfigurationType == typeof(ConnectionStringSettingsCollection)).First();
        }

        [TestMethod]
        public void then_element_has_no_default_add_command()
        {
            Assert.AreEqual(0, collectionElement.Commands.OfType<DefaultCollectionElementAddCommand>().Count());
        }

        [TestMethod]
        public void then_element_has_custom_command()
        {
            Assert.AreEqual(1, collectionElement.Commands.OfType<CommandModel>().Where(x => x.Title == "Custom command").Count());
        }
    }

    [TestClass]
    public class when_querying_commands_on_collection_element_with_custom_deleteCommand : given_element_view_model_commands
    {
        ElementViewModel elementViewModel;

        protected override void Act()
        {
            elementViewModel = Viewmodel.DescendentElements(x => x.ConfigurationType == typeof(CustomDeleteConfigurationElement)).First();
        }

        [TestMethod]
        public void then_element_has_no_default_delete_command()
        {
            Assert.AreEqual(0, elementViewModel.Commands.OfType<DefaultDeleteCommandModel>().Count());
        }

        [TestMethod]
        public void then_element_has_custom_command()
        {
            Assert.AreEqual(1, elementViewModel.Commands.OfType<CommandModel>().Where(x => x.Title == "Custom command").Count());
        }
    }

    [TestClass]
    public class when_querying_commands_on_nonpolymprhic_collections : given_element_view_model_commands
    {
        private ElementViewModel elementViewModel;

        protected override void Act()
        {
            elementViewModel =
                Viewmodel.DescendentElements(x => x.ConfigurationType == typeof(NameValueConfigurationCollection)).First();
        }

        [TestMethod]
        public void then_add_command_is_element_add_command()
        {
            Assert.IsTrue(
                elementViewModel.Commands.Any(x => x.Placement == CommandPlacement.ContextAdd & x.GetType() == typeof(DefaultCollectionElementAddCommand))
                );
        }
    }

    [TestClass]
    public class when_querying_commands_on_polymorphics_collections : given_element_view_model_commands
    {
        private ElementViewModel elementViewModel;

        protected override void Act()
        {
            elementViewModel =
                Viewmodel.DescendentElements(x => x.ConfigurationType == typeof (PolymorphicCollection)).First();
        }

        [TestMethod]
        public void then_polymorphic_collection_has_default_add_command()
        {
            Assert.IsTrue(elementViewModel.Commands.Any(
                x =>
                x.Placement == CommandPlacement.ContextAdd && x.GetType() == typeof (DefaultElementCollectionAddCommand)));
        }
    }
}
