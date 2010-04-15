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
    /// This is used by the design-time infrastructure and is not intended to be used directly from your code.
    /// <br />
    /// Associates a <see cref="KeyGesture"/> to control the expansion or contraction of one <see cref="Grid"/> column.
    /// </summary>
    public static class GridSplitterKeyAccelerator
    {
        /// <summary>
        /// The target column number to control with the specified input gesture.
        /// </summary>
        public static readonly DependencyProperty TargetColumnProperty =
            DependencyProperty.RegisterAttached(
                "TargetColumn",
                typeof(int),
                typeof(GridSplitterKeyAccelerator),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnTargetColumnChanged)));

        /// <summary>
        /// Returns the target column number to expand or contract in response to <see cref="KeyGesture"/>
        /// </summary>
        /// <param name="grid">The grid containing the column.</param>
        /// <returns>The the column number</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static int GetTargetColumn(Grid grid)
        {
            Guard.ArgumentNotNull(grid, "grid");
            return (int)grid.GetValue(TargetColumnProperty);
        }

        /// <summary>
        /// Sets the target column number to expand or contravct in reponse to <see cref="KeyGesture"/>
        /// </summary>
        /// <param name="grid">The grid containing the column.</param>
        /// <param name="columnIndex">The column number to address.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static void SetTargetColumn(Grid grid, int columnIndex)
        {
            Guard.ArgumentNotNull(grid, "grid");
            grid.SetValue(TargetColumnProperty, columnIndex);
        }

        /// <summary>
        /// The <see cref="KeyGesture"/> used to decrement the width of the <see cref="TargetColumnProperty"/>
        /// </summary>
        public static readonly DependencyProperty DecrementKeyGestureProperty =
            DependencyProperty.RegisterAttached(
                "DecrementKeyGesture",
                typeof(KeyGesture),
                typeof(GridSplitterKeyAccelerator),
                new FrameworkPropertyMetadata(new KeyGesture(Key.Left, ModifierKeys.Alt),
                                              new PropertyChangedCallback(OnGestureChanged)));

        /// <summary>
        /// Retrieves the <see cref="KeyGesture"/> attached property value for decrementing the column width.
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static KeyGesture GetDecrementKeyGesture(Grid grid)
        {
            Guard.ArgumentNotNull(grid, "grid");
            return (KeyGesture)grid.GetValue(DecrementKeyGestureProperty);
        }

        /// <summary>
        /// Sets the <see cref="KeyGesture"/> attached property value for decrementing the column width.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="gesture"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static void SetDecrementKeyGesture(Grid grid, KeyGesture gesture)
        {
            Guard.ArgumentNotNull(grid, "grid");
            grid.SetValue(DecrementKeyGestureProperty, gesture);
        }

        /// <summary>
        /// The <see cref="KeyGesture"/> used to increment the width of the <see cref="TargetColumnProperty"/>
        /// </summary>
        public static readonly DependencyProperty IncrementKeyGestureProperty =
           DependencyProperty.RegisterAttached(
               "IncrementKeyGesture",
               typeof(KeyGesture),
               typeof(GridSplitterKeyAccelerator),
               new FrameworkPropertyMetadata(new KeyGesture(Key.Right, ModifierKeys.Alt),
                                             new PropertyChangedCallback(OnGestureChanged)));

        /// <summary>
        /// Retrieves the <see cref="KeyGesture"/> attached property value for incrementing the column width.
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static KeyGesture GetIncrementKeyGesture(Grid grid)
        {
            Guard.ArgumentNotNull(grid, "grid");
            return (KeyGesture)grid.GetValue(IncrementKeyGestureProperty);
        }

        /// <summary>
        /// Sets the <see cref="KeyGesture"/> attached property value for incrementing the column width.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="gesture"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public static void SetIncrementKeyGesture(Grid grid, KeyGesture gesture)
        {
            Guard.ArgumentNotNull(grid, "grid");
            grid.SetValue(IncrementKeyGestureProperty, gesture);
        }

        private static readonly DependencyProperty SplitterKeyboardBehaviorProperty =
            DependencyProperty.RegisterAttached(
                "GridSplitterKeyAccelerator",
                typeof(GridSplitterKeyAcceleratorBehavior),
                typeof(GridSplitterKeyAccelerator));

        private static void SetSplitterKeyboardBehavior(DependencyObject grid, GridSplitterKeyAcceleratorBehavior behavior)
        {
            grid.SetValue(SplitterKeyboardBehaviorProperty, behavior);
        }

        private static GridSplitterKeyAcceleratorBehavior GetSplitterKeyboardBehavior(DependencyObject grid)
        {
            return grid.GetValue(SplitterKeyboardBehaviorProperty) as GridSplitterKeyAcceleratorBehavior;
        }

        private static void OnGestureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetupAttachedProperty(d);
        }

        private static void OnTargetColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetupAttachedProperty(d);
        }

        private static void SetupAttachedProperty(DependencyObject d)
        {
            var containingGrid = d as Grid;
            if (containingGrid == null)
                throw new ArgumentException("TargetColumnProperty expected to be attached to a grid.");

            if (GetSplitterKeyboardBehavior(containingGrid) == null)
            {
                var behavior = new GridSplitterKeyAcceleratorBehavior();
                behavior.Attach(containingGrid);
                SetSplitterKeyboardBehavior(containingGrid, behavior);
            }
        }
    }

    /// <summary>
    /// Behavior attached to <see cref="Grid"/> to manage handling increment and decrement responses to <see cref="KeyGesture"/>
    /// This is attached to a grid using the <seealso cref="GridSplitterKeyAccelerator"/> attached properties.
    /// </summary>
    public class GridSplitterKeyAcceleratorBehavior
    {
        private const double ColumnIncrementSize = 5;
        private Grid target;

        /// <summary>
        /// Attaches to the grid to begin conitoring <see cref="UIElement.KeyDown"/> events.
        /// </summary>
        /// <param name="grid"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public void Attach(Grid grid)
        {
            Guard.ArgumentNotNull(grid, "grid");

            target = grid;
            grid.KeyDown += ControlKeyDownHandler;
        }

        private void ControlKeyDownHandler(object sender, KeyEventArgs e)
        {
            var incrementGesture = GridSplitterKeyAccelerator.GetIncrementKeyGesture(target);
            var decrementGesture = GridSplitterKeyAccelerator.GetDecrementKeyGesture(target);

            if (decrementGesture.Matches(target, e))
            {
                MoveGrid(-ColumnIncrementSize);
                e.Handled = true;
                return;
            }

            if (incrementGesture.Matches(target, e))
            {
                MoveGrid(ColumnIncrementSize);
                e.Handled = true;
                return;
            }
        }

        /// <summary>
        /// Moves the <see cref="GridSplitterKeyAccelerator.TargetColumnProperty"/> by the amount specified.
        /// </summary>
        /// <remarks>
        /// This will decrement as long as the value is not below <see cref="ColumnDefinition.MinWidth"/>, once it is, it no longer decrements.
        /// </remarks>
        /// <param name="delta">The amount to increment (if positive) or decrement (if negative) the grid.</param>
        protected void MoveGrid(double delta)
        {
            var columnIndex = GridSplitterKeyAccelerator.GetTargetColumn(target);
            var columnDefinition = target.ColumnDefinitions[columnIndex];
            var width = columnDefinition.Width;
            if (width.Value <= columnDefinition.MinWidth && delta < 0) return;
            if (width.Value >= columnDefinition.MaxWidth && delta > 0) return;

            columnDefinition.Width = new GridLength(Math.Max(0, width.Value + delta), width.GridUnitType);
        }
    }
}
