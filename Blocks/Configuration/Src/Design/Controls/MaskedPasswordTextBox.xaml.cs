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
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
    /// <summary>
    /// This only provides masking of password but does <b>not</b> securely
    /// store it.  It is intended for use in preventing exposure of password
    /// information on the screen, but not internally.
    /// </summary>
    public partial class MaskedPasswordTextBox : UserControl
    {
        private IValueChangeCoordinator coordinator;

        ///<summary>
        /// Initializes a new instance of see <see cref="MaskedPasswordTextBox"/>.
        ///</summary>
        public MaskedPasswordTextBox() 
        {
            InitializeComponent();
            coordinator = new PasswordCoordinator(passwordBox);
            coordinator.PropertyChanged += CoordinatorPropertyChangedHandler;
        }

        /// <summary>
        /// Initializes a new instance of see <see cref="MaskedPasswordTextBox"/>
        /// with a value changed coordinator.
        /// </summary>
        /// <remarks>This is largely here to support testability</remarks>
        /// <param name="coordinator"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public MaskedPasswordTextBox(IValueChangeCoordinator coordinator)
        {
            InitializeComponent();
            this.coordinator = coordinator;
            coordinator.PropertyChanged += CoordinatorPropertyChangedHandler;
        }

        private void CoordinatorPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            UnsecuredPassword = coordinator.Value;
        }

        ///<summary>
        /// The <b>unsecured</b> password value.
        ///</summary>
        public string UnsecuredPassword
        {
            get
            {
                return (string)GetValue(UnsecuredPasswordProperty);
            }
            set
            {
                SetValue(UnsecuredPasswordProperty, value);
            }
        }

        
        ///<summary>
        /// Represents the <b>unsecured</b> password.  This control is only intended
        /// to help mask password entry.
        ///</summary>
        public static readonly DependencyProperty UnsecuredPasswordProperty =
            DependencyProperty.Register("UnsecuredPassword", typeof(string), typeof(MaskedPasswordTextBox), new UIPropertyMetadata(string.Empty, UnsecuredPasswordChangedCallback));

        private static void UnsecuredPasswordChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MaskedPasswordTextBox maskedBox = d as MaskedPasswordTextBox;

            if (maskedBox != null)
            {
                maskedBox.coordinator.Value = (string)e.NewValue;
            }
        }


        private class PasswordCoordinator : IValueChangeCoordinator
        {
            private readonly PasswordBox box;
            private bool updatingValue;
            private bool notifyingHandlers;

            public PasswordCoordinator(PasswordBox box)
            {
                this.box = box;
                this.box.PasswordChanged += this.PasswordChangedHandler;
            }

            private void PasswordChangedHandler(object sender, RoutedEventArgs e)
            {
                if (updatingValue) return;

                notifyingHandlers = true;
                e.Handled = true;
                InvokePropertyChanged("Value");
                notifyingHandlers = false;
            }

            public string Value
            {
                get { return box.Password; }
                set
                {
                    if (notifyingHandlers) return;

                    updatingValue = true;
                    box.Password = value;
                    updatingValue = false;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void InvokePropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler changed = PropertyChanged;
                if (changed != null) changed(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// Used to support testing of the <see cref="MaskedPasswordTextBox"/>
    /// </summary>
    public interface IValueChangeCoordinator : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the coordinated value.
        /// </summary>
        string Value { get; set; }
    }
}
