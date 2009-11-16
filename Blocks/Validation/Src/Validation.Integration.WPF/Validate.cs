//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WPF
{
    /// <summary>
    /// Provides methods and attached properties that support validation of bindings using the 
    /// Validation Application Block.
    /// </summary>
    public static class Validate
    {
        /// <summary>
        /// Identifies the BindingFor attached property.
        /// </summary>
        public static readonly DependencyProperty BindingForProperty =
            DependencyProperty.RegisterAttached(
                "BindingFor",
                typeof(string),
                typeof(Validate),
                new PropertyMetadata(OnAttachedPropertyChanged));

        /// <summary>
        /// Sets the value of the BindingFor attached property to a specified element.
        /// </summary>
        /// <param name="element">The element to which the attached property is written.</param>
        /// <param name="property">The name of the bound property to validate.</param>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void SetBindingFor(FrameworkElement element, string property)
        {
            element.SetValue(BindingForProperty, property);
        }

        /// <summary>
        /// Gets the value of the BindingFor attached property for a specified element.
        /// </summary>
        /// <param name="element">The element from which the property value is read.</param>
        /// <returns>The BindingFor property value for the element.</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static string GetBindingFor(FrameworkElement element)
        {
            return (string)element.GetValue(BindingForProperty);
        }

        private static void OnAttachedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            FrameworkElement element = dependencyObject as FrameworkElement;
            UpdateValidatorRule(element, args);
        }

        private static void UpdateValidatorRule(FrameworkElement element, DependencyPropertyChangedEventArgs args)
        {
            // Defer the initialization if not yet initialized
            if (!element.IsInitialized)
            {
                EventHandler handler = null;
                handler =
                    (o, e) =>
                    {
                        element.Initialized -= handler;
                        UpdateValidatorRule(element, args);
                    };

                element.Initialized += handler;
                return;
            }

            if (args.OldValue != null)
            {
                var expression = GetBindingExpressionForPropertyName(element, (string)args.OldValue);
                Binding binding = expression.ParentBinding;

                // remove the previous validation rule, if any
                var currentRule = binding.ValidationRules.OfType<ValidatorRule>().FirstOrDefault();
                if (currentRule != null)
                {
                    binding.ValidationRules.Remove(currentRule);
                }
            }
            if (args.NewValue != null)
            {
                var expression = GetBindingExpressionForPropertyName(element, (string)args.NewValue);
                Binding binding = expression.ParentBinding;

                // Add the rule if necessary
                if (!binding.ValidationRules.OfType<ValidatorRule>().Any())
                {
                    binding.ValidationRules.Add(new ValidatorRule(expression));
                }
            }
        }

        private static BindingExpression GetBindingExpressionForPropertyName(FrameworkElement element, string dependencyPropertyName)
        {
            // Get the property to be validated
            var descriptor = DependencyPropertyDescriptor.FromName(dependencyPropertyName, element.GetType(), element.GetType());
            if (descriptor == null)
            {
                // TODO should throw?
                throw new ArgumentException("value", "Cannot find property for name " + dependencyPropertyName);
            }
            var dependencyProperty = descriptor.DependencyProperty;

            // Get the BindingExpression for the property to validate
            var bindingExpression = element.GetBindingExpression(dependencyProperty);
            if (bindingExpression == null)
            {
                // TODO should throw?
                throw new ArgumentException("value", "Property does not have a binding " + dependencyPropertyName);
            }

            return bindingExpression;
        }
    }
}
