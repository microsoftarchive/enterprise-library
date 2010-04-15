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

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{

    /// <summary>
    /// A utility class that coordinates between an <see cref="ElementViewModel"/> and 
    /// connections with the user-interface, such as focus and input binding.
    /// <br/>
    /// This is used by the design-time infrastructure and is not intended to be used directly from your code.    
    /// </summary>
    public class SelectionHelper
    {
        private readonly Control control;
        private readonly PropertyChangedEventHandler attachedElementPropertyChangedHandler;
        private ElementViewModel attachedViewModel;


        ///<summary>
        /// Initializes a new instance of <see cref="SelectionHelper"/>.
        ///</summary>
        ///<param name="control">The control to coordinate with.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public SelectionHelper(Control control)
        {
            Guard.ArgumentNotNull(control, "control");
            this.control = control;
            this.control.GotFocus += new System.Windows.RoutedEventHandler(control_GotFocus);
            this.control.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(control_MouseLeftButtonDown);
            this.control.Loaded += new System.Windows.RoutedEventHandler(control_Loaded);

            attachedElementPropertyChangedHandler = new PropertyChangedEventHandler(attachedElementPropertyChanged);
        }

        private void attachedElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "IsSelected")
            {
                SetFocusIfSelected();
            }
        }

        private void SetFocusIfSelected()
        {
            if (attachedViewModel != null && attachedViewModel.IsSelected)
            {
                control.Focus();
            }
        }

        /// <summary>
        /// Attaches an <see cref="ElementViewModel"/> and begins monitoring for property changes.
        /// </summary>
        /// <param name="viewModel">The element to attach.</param>
        /// <remarks>
        /// When attached begins monitoring the <see cref="INotifyPropertyChanged.PropertyChanged"/> event of the <see cref="ElementViewModel"/>
        /// and establishes <see cref="KeyGesture"/> bindings based on the <see cref="ElementViewModel.Commands"/>.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public void Attach(ElementViewModel viewModel)
        {
            Guard.ArgumentNotNull(viewModel, "viewModel");

            if (attachedViewModel != null)
            {
                attachedViewModel.PropertyChanged -= attachedElementPropertyChangedHandler;
            }

            attachedViewModel = viewModel;
            attachedViewModel.PropertyChanged += attachedElementPropertyChangedHandler;
            CreateKeyBindings(attachedViewModel);
        }

        void control_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SetFocusIfSelected();
        }

        void control_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            attachedViewModel.Select();
            e.Handled = true;
        }

        void control_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        private void CreateKeyBindings(ElementViewModel attachedViewModel)
        {
            control.InputBindings.Clear();
            var converter = new KeyGestureConverter();

            foreach (var command in attachedViewModel.Commands)
            {
                if (!string.IsNullOrEmpty(command.KeyGesture))
                {
                    control.InputBindings.Add(
                        new InputBinding(command,
                                         (KeyGesture)converter.ConvertFrom(command.KeyGesture)
                            )
                        );
                }
            }
        }

        /// <summary>
        /// Clears the attached view model and unhooks the <see cref="UIElement.InputBindings"/>.
        /// </summary>
        public void Clear()
        {
            if (attachedViewModel != null)
            {
                attachedViewModel.PropertyChanged -= attachedElementPropertyChangedHandler;

                control.InputBindings.Clear();

                attachedViewModel = null;
            }
        }
    }
}
