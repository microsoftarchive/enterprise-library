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

// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------

using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls.Toolkit
{
    /// <summary>
    ///     A visual state that can be transitioned into.
    /// </summary>
    [ContentProperty("Storyboard")]
    [RuntimeNameProperty("Name")]
    public class VisualState : DependencyObject
    {
        /// <summary>
        ///     The name of the VisualState.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        private static readonly DependencyProperty StoryboardProperty = 
            DependencyProperty.Register(
            "Storyboard", 
            typeof(Storyboard), 
            typeof(VisualState));

        /// <summary>
        ///     Storyboard defining the values of properties in this visual state.
        /// </summary>        
        public Storyboard Storyboard
        {
            get { return (Storyboard)GetValue(StoryboardProperty); }
            set { SetValue(StoryboardProperty, value); }
        }
    }
}
