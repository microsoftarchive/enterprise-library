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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Controls.given_maskedpasswordbox
{
    [TestClass]
    public class when_password_updates : ArrangeActAssert
    {
        private MaskedPasswordTextBox maskedPasswordControl;
        private TestPasswordChangeCoordinator coordinator;

        protected override void Arrange()
        {
            base.Arrange();

            coordinator = new TestPasswordChangeCoordinator();
            maskedPasswordControl = new MaskedPasswordTextBox(coordinator);
        }

        protected override void Act()
        {
            coordinator.Value = "UnsecuredPasswordValue";
            coordinator.FirePropertyChanged();
        }

        [TestMethod]
        public void then_control_reflects_updated_value()
        {
            Assert.AreEqual("UnsecuredPasswordValue", maskedPasswordControl.UnsecuredPassword);                        
        }
    }

    [TestClass]
    public class when_unsecuredpassword_set : ArrangeActAssert
    {
        private MaskedPasswordTextBox maskedPasswordControl;
        private TestPasswordChangeCoordinator coordinator;

        protected override void Arrange()
        {
            base.Arrange();

            coordinator = new TestPasswordChangeCoordinator();
            maskedPasswordControl = new MaskedPasswordTextBox(coordinator);
        }

        protected override void Act()
        {
            maskedPasswordControl.UnsecuredPassword = "NewValue";
        }

        [TestMethod] 
        public void then_coordinator_value_changed()
        {
            Assert.AreEqual("NewValue", coordinator.Value);
        }
    }

    internal class TestPasswordChangeCoordinator :  IValueChangeCoordinator    
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(string property)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, new PropertyChangedEventArgs(property));
        }

        public string Value { get; set;}

        public void FirePropertyChanged()
        {
            InvokePropertyChanged("Value");
        }
    }
}
