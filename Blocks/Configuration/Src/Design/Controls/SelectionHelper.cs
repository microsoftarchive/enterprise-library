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
using System.Windows.Controls;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls.Primitives;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
    public class SelectionHelper
    {
        private readonly Control control;
        private readonly PropertyChangedEventHandler attachedElementPropertyChangedHandler;
        private ElementViewModel attachedViewModel;
        

        public SelectionHelper(Control control)
        {
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


        public void Attach(ElementViewModel viewModel)
        {
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
    }
}
