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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
    /// <summary>
    /// Class that contains attacheable properties that can be used to bind an <see cref="ICommand"/> implemention to a <see cref="Control"/>'s <see cref="Control.MouseDoubleClick"/> event
    /// </summary>
    public static class DoubleClickCommand
    {
        /// <summary>
        /// Attacheable property that can be used to bind an <see cref="ICommand"/> implemention to a <see cref="Control"/>'s <see cref="Control.MouseDoubleClick"/> event
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command",
                                                                        typeof(ICommand),
                                                                        typeof(DoubleClickCommand),
                                                                        new FrameworkPropertyMetadata(null, OnCommandPropertyChanged));
        /// <summary>
        /// Attacheable property that can be used to bind an <see cref="ICommand"/> parameter value to a <see cref="Control"/>'s <see cref="Control.MouseDoubleClick"/> event
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter",
                                                                        typeof(object),
                                                                        typeof(DoubleClickCommand),
                                                                        new FrameworkPropertyMetadata(null, OnCommandPropertyChanged));

        private static readonly DependencyProperty DoubleClickBehaviorProperty = DependencyProperty.RegisterAttached("DoubleClickBehavior",
                                                                        typeof(DoubleClickBehavior),
                                                                        typeof(DoubleClickCommand));

        private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EnsureAttachedDoubleClickBehaviorProperty(d);
        }

        private static void EnsureAttachedDoubleClickBehaviorProperty(DependencyObject d)
        {
            object currentDoubleClickBehavior = d.GetValue(DoubleClickBehaviorProperty);
            if (currentDoubleClickBehavior == null)
            {
                d.SetValue(DoubleClickBehaviorProperty, new DoubleClickBehavior(d));
            }
        }

        /// <summary>
        /// Gets the <see cref="ICommand"/> implementation bound to a <see cref="Control"/>'s <see cref="Control.MouseDoubleClick"/> event.
        /// </summary>
        /// <param name="extendee">The <see cref="Control"/> to get the <see cref="ICommand"/> implementation from.</param>
        /// <returns>If an <see cref="ICommand"/> was attached to <paramref name="extendee"/> returns the attachted <see cref="ICommand"/>. Otherwise <see langword="null"/>. </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "extendee"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static ICommand GetCommand(DependencyObject extendee)
        {
            Guard.ArgumentNotNull(extendee, "extendee");
            return (ICommand)extendee.GetValue(CommandProperty);
        }

        /// <summary>
        /// Sets an <see cref="ICommand"/> implementation to a <see cref="Control"/>'s <see cref="Control.MouseDoubleClick"/> event.
        /// </summary>
        /// <param name="extendee">The <see cref="Control"/> to set the <see cref="ICommand"/> implementation for.</param>
        /// <param name="command">The <see cref="ICommand"/> implementation to set to <paramref name="extendee"/>.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "extendee"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static void SetCommand(DependencyObject extendee, ICommand command)
        {
            Guard.ArgumentNotNull(extendee, "extendee");
            extendee.SetValue(CommandProperty, command);
        }

        /// <summary>
        /// Gets the <see cref="ICommand"/> parameter bound to a <see cref="Control"/>'s <see cref="Control.MouseDoubleClick"/> event.
        /// </summary>
        /// <param name="extendee">The <see cref="Control"/> to get the <see cref="ICommand"/> implementation from.</param>
        /// <returns>If an <see cref="ICommand"/> parameter was attached to <paramref name="extendee"/> returns the attachted parameter value. Otherwise <see langword="null"/>. </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "extendee"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static object GetCommandParameter(DependencyObject extendee)
        {
            Guard.ArgumentNotNull(extendee, "extendee");
            return extendee.GetValue(CommandParameterProperty);
        }

        /// <summary>
        /// Sets an <see cref="ICommand"/> parameter implementation to a <see cref="Control"/>'s <see cref="Control.MouseDoubleClick"/> event.
        /// </summary>
        /// <param name="extendee">The <see cref="Control"/> to set the <see cref="ICommand"/> parameter for.</param>
        /// <param name="commandParameter">The <see cref="ICommand"/> parameter to set to <paramref name="extendee"/>.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "extendee"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static void SetCommandParameter(DependencyObject extendee, object commandParameter)
        {
            Guard.ArgumentNotNull(extendee, "extendee");
            extendee.SetValue(CommandParameterProperty, commandParameter);
        }

        private class DoubleClickBehavior : IDisposable
        {
            readonly Control subject;

            public DoubleClickBehavior(DependencyObject subject)
            {
                this.subject = subject as Control;
                if (this.subject != null)
                {
                    this.subject.MouseDoubleClick += SubjectMouseDoubleClick;
                }
            }

            void SubjectMouseDoubleClick(object sender, MouseButtonEventArgs e)
            {
                ICommand command = subject.GetValue(CommandProperty) as ICommand;
                if (command != null)
                {
                    object commandParameter = subject.GetValue(CommandParameterProperty);
                    if (command.CanExecute(commandParameter))
                    {
                        command.Execute(commandParameter);
                    }
                }
            }

            public void Dispose()
            {
                if (this.subject != null)
                {
                    this.subject.MouseDoubleClick -= SubjectMouseDoubleClick;
                }

                GC.SuppressFinalize(this);
            }
        }
    }
}
