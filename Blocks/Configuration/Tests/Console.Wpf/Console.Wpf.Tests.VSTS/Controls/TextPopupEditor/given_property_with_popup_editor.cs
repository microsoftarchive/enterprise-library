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
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Moq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.Tests.VSTS.Controls.TextPopupEditor
{

    [TestClass]
    public class when_popup_editor_invokes_and_cancel_changes : ArrangeActAssert
    {
        private Mock<IUIServiceWpf> mockIUIService;
        private Mock<IServiceProvider> mockService;
        private BindableProperty bindable;
        private Mock<ITypeDescriptorContext> mockTypeDescriptor;
        private PopupTextEditor editor;
        private object editorReturnedValue;

        protected override void Arrange()
        {
            base.Arrange();

            mockIUIService = new Mock<IUIServiceWpf>();
            mockIUIService.Setup(x => x.ShowDialog(It.IsAny<TextEditDialog>()))
                .Callback<Window>(w => ((PopupEditorValue)w.DataContext).Value = "NewValue")
                .Returns(false).Verifiable("Dialog not shown.");

            mockService = new Mock<IServiceProvider>();
            mockService.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IUIServiceWpf))))
                .Returns(mockIUIService.Object);

            bindable = new BindableProperty(new MockProperty(mockService.Object, null, null));

            mockTypeDescriptor = new Mock<ITypeDescriptorContext>();
            mockTypeDescriptor.Setup(x => x.PropertyDescriptor).Returns(bindable);
        }

        protected override void Act()
        {
            editor = new PopupTextEditor();
            editorReturnedValue = editor.EditValue(mockTypeDescriptor.Object, mockService.Object, "initialValue");
        }

        [TestMethod]
        public void then_value_returned_is_original()
        {
            Assert.AreEqual("initialValue", editorReturnedValue);
        }
    }

    [TestClass]
    public class when_popup_editor_invoked_with_changes : ArrangeActAssert
    {
        private PopupTextEditor editor;
        private Mock<IServiceProvider> mockService;
        private Mock<IUIServiceWpf> mockIUIService;
        private BindableProperty bindable;
        private Mock<ITypeDescriptorContext> mockTypeDescriptor;
        private object editorReturnedValue;

        protected override void Arrange()
        {
            base.Arrange();

            mockIUIService = new Mock<IUIServiceWpf>();
            mockIUIService.Setup(x => x.ShowDialog(It.IsAny<TextEditDialog>()))
                .Callback<Window>(w => ((PopupEditorValue)w.DataContext).Value = "NewValue")
                .Returns(true).Verifiable("Dialog not shown.");

            mockService = new Mock<IServiceProvider>();
            mockService.Setup(x => x.GetService(It.Is<Type>(t => t == typeof(IUIServiceWpf)))).Returns(
                mockIUIService.Object);

            bindable = new BindableProperty(
                        new MockProperty(mockService.Object, null, null, new EditorWithReadOnlyTextAttribute(true)));

            mockTypeDescriptor = new Mock<ITypeDescriptorContext>();
            mockTypeDescriptor.Setup(x => x.PropertyDescriptor).Returns(bindable);
        }

        protected override void Act()
        {
            editor = new PopupTextEditor();
            editorReturnedValue = editor.EditValue(mockTypeDescriptor.Object, mockService.Object, "initialValue");
        }

        [TestMethod]
        public void then_shows_dialog()
        {
            mockIUIService.Verify();
        }

        [TestMethod]
        public void then_new_value_is_returned()
        {
            Assert.AreEqual("NewValue", editorReturnedValue);
        }

        [TestMethod]
        public void then_new_value_is_set_on_bindable()
        {
            Assert.AreEqual("NewValue", bindable.GetValue(null));
        }
    }

    class MockProperty : Property
    {
        public MockProperty(IServiceProvider serviceProvider,
                            object component,
                            PropertyDescriptor declaringProperty, params Attribute[] additionalAttributes)
            : base(serviceProvider, component, declaringProperty, additionalAttributes)
        {
        }

        public override string PropertyName
        {
            get
            {
                return "MockProperty";
            }
        }

        protected override void SetValue(object value)
        {
            this.BackingPropertyValue = value;   
        }

        protected override object GetValue()
        {
            return this.BackingPropertyValue;
        }

        public object BackingPropertyValue { get; set; }
    }
}
