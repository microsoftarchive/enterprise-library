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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls.Toolkit
{
#pragma warning disable 1591
    /// <summary>
    /// Defines a transition between VisualStates.
    /// </summary>
    [ContentProperty("Storyboard")]
    public class VisualTransition : DependencyObject
    {
        public VisualTransition()
        {
            DynamicStoryboardCompleted = true;
            ExplicitStoryboardCompleted = true;
        }

        /// <summary>
        /// Name of the state to transition from.
        /// </summary>
        public string From 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Name of the state to transition to.
        /// </summary>
        public string To 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Storyboard providing fine grained control of the transition.
        /// </summary>
        public Storyboard Storyboard 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Duration of the transition.
        /// </summary>
        [TypeConverter(typeof(DurationConverter))]
        public Duration GeneratedDuration 
        { 
            get { return _generatedDuration; } 
            set { _generatedDuration = value; } 
        }

        internal bool IsDefault
        {
            get { return From == null && To == null; }
        }

        internal bool DynamicStoryboardCompleted
        {
            get;
            set;
        }

        internal bool ExplicitStoryboardCompleted
        {
            get;
            set;
        }

        private Duration _generatedDuration = new Duration(new TimeSpan());
    }

#pragma warning restore 1591
}
