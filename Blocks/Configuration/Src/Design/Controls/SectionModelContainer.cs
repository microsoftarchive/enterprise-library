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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
#pragma warning disable 1591
    /// <summary>
    /// The container for a <see cref="SectionViewModel"/> providing.
    /// <br/>
    /// This is used by the design-time infrastructure and is not intended to be used directly from your code.
    /// </summary>
    /// <remarks>
    /// Handles selection and adornment of related elements within the container.
    /// </remarks>
    [TemplatePart(Name = "PART_Header", Type = typeof(ContentControl))]
    [TemplatePart(Name = "PART_RelationshipCanvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_Content", Type = typeof(ContentControl))]
    public class SectionModelContainer : Control, IDisposable
    {
        private const int RELATIONSHIP_LINE_Y_OFFSET = 12;

        private SelectionHelper selectionHelper;
        private SectionViewModel SectionModel;

        public RoutedCommand ShowRelationships { get; set; }

        private Canvas RelationshipCanvas { get; set; }
        private ContentControl Content { get; set; }
        private AdornedElement currentAdorner;

        static SectionModelContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SectionModelContainer), new FrameworkPropertyMetadata(typeof(SectionModelContainer)));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SectionModelContainer"/>.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Windows.Input.RoutedUICommand.#ctor(System.String,System.String,System.Type,System.Windows.Input.InputGestureCollection)", Justification = "Not a user-visible string")]
        public SectionModelContainer()
        {
            this.DataContextChanged += DataContextChangedHandler;
            Unloaded += UnloadHandler;

            GotKeyboardFocus += GotKeyboardFocusHandler;
            LostKeyboardFocus += LostKeyboardFocusHandler;

            var commandGestures = new InputGestureCollection
                                      {
                                          new MouseGesture(MouseAction.LeftClick)
                                      };

            ShowRelationships = new RoutedUICommand("Show ElementModel Relationships",
                                                    "ShowRelationships",
                                                    typeof(SectionModelContainer),
                                                    commandGestures);

        }

        void LostKeyboardFocusHandler(object sender, KeyboardFocusChangedEventArgs e)
        {
            RemoveAdorner();
        }

        void GotKeyboardFocusHandler(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.OriginalSource is ElementModelContainer)
            {
                AdornSelectedElement((ElementModelContainer)e.OriginalSource);
            }
            if (e.OriginalSource is SectionModelContainer)
            {
                RemoveAdorner();
            }
            var containingElementModelContainer = VisualTreeWalker.TryFindParent<ElementModelContainer>(e.OriginalSource as DependencyObject);
            if (containingElementModelContainer != null)
            {
                AdornSelectedElement(containingElementModelContainer);
            }
        }

        private void UnloadHandler(object sender, RoutedEventArgs e)
        {
            RemoveAdorner();
            selectionHelper.Clear();
            SectionModel = null;
        }

        private void RemoveAdorner()
        {
            if (currentAdorner != null)
            {
                if (currentAdorner.ElementContainerControl != null && currentAdorner.ElementContainerControl.Element != null)
                {
                    currentAdorner.ElementContainerControl.Element.Deleted -= AdornedElementDeleted;
                }

                currentAdorner.Dispose();
                currentAdorner = null;
            }
        }

        internal void AdornSelectedElement(ElementModelContainer elementContainer)
        {
            RemoveAdorner();

            if (elementContainer.Element == null) return;

            currentAdorner = new AdornedElement(elementContainer, RelationshipCanvas);
            currentAdorner.ElementContainerControl.Element.Deleted += AdornedElementDeleted;
            currentAdorner.DrawRelationships();
        }

        void AdornedElementDeleted(object sender, EventArgs e)
        {
            RemoveAdorner();
        }

        void DataContextChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            SectionModel = (SectionViewModel)e.NewValue;

            if (selectionHelper != null)
            {
                selectionHelper.Attach(SectionModel);
            }
        }

        private class AdornedElementWithRelationshipLine : AdornedElement
        {
            readonly Canvas rootCanvas;
            readonly ElementModelContainer destinationControl;
            readonly AdornedElement root;

            public AdornedElementWithRelationshipLine(AdornedElement root, Canvas rootCanvas, ElementModelContainer destinationControl)
                : base(destinationControl, rootCanvas)
            {
                this.root = root;
                this.destinationControl = destinationControl;
                this.rootCanvas = rootCanvas;
                this.destinationControl.Element.Deleted += ElementDeleted;
            }

            void ElementDeleted(object sender, EventArgs e)
            {
                RedrawRelationships();
            }

            public override void RedrawRelationships()
            {
                root.RedrawRelationships();
            }

            public override void DrawRelationships()
            {
                if (destinationControl != null)
                {
                    Point offsetOrigin = GetOffsetOrigin(root.ElementContainerControl, destinationControl);
                    Point offsetDestination = GetOffsetDestination(destinationControl, root.ElementContainerControl);
                    DrawRelationshipLine(offsetOrigin, offsetDestination);
                }
            }

            private void DrawRelationshipLine(Point origin, Point destination)
            {
                var relationshipPath = new Path
                                           {
                                               Stroke = Brushes.Black,
                                               StrokeThickness = 1,
                                               Visibility = Visibility.Visible,
                                           };

                var geometry = new StreamGeometry
                                   {
                                       FillRule = FillRule.EvenOdd
                                   };

                using (StreamGeometryContext ctx = geometry.Open())
                {
                    ctx.BeginFigure(new Point(origin.X, origin.Y), false, false);

                    var midPointX = Math.Abs(origin.X - destination.X) / 2;

                    if (origin.X > destination.X)
                    {
                        ctx.LineTo(new Point(origin.X - midPointX, origin.Y), true, true);
                        ctx.LineTo(new Point(origin.X - midPointX, destination.Y), true, true);
                    }
                    else
                    {
                        ctx.LineTo(new Point(origin.X + midPointX, origin.Y), true, true);
                        ctx.LineTo(new Point(origin.X + midPointX, destination.Y), true, true);
                    }

                    ctx.LineTo(new Point(destination.X, destination.Y), true, true);
                }

                geometry.Freeze();

                relationshipPath.Data = geometry;
                rootCanvas.Children.Add(relationshipPath);
            }

            private Point GetOffsetDestination(FrameworkElement destinationControl, FrameworkElement originControl)
            {
                Point destination = GetPointFromControl(destinationControl);
                Point origin = GetPointFromControl(originControl);

                Point offsetDestination;
                //If the destination is to the right of the origin
                if (destination.X > origin.X)
                {
                    //(destinationControl.ActualHeight / 2)
                    offsetDestination = new Point(destination.X, destination.Y + RELATIONSHIP_LINE_Y_OFFSET);
                }
                else
                {
                    //If the destination is to the left of the origin
                    //(destinationControl.ActualHeight / 2)
                    offsetDestination = new Point(destination.X + destinationControl.ActualWidth, destination.Y + RELATIONSHIP_LINE_Y_OFFSET);
                }
                return offsetDestination;
            }

            private Point GetPointFromControl(FrameworkElement control)
            {
                return control.TransformToVisual(rootCanvas).Transform(new Point(0, 0));
            }

            private Point GetOffsetOrigin(FrameworkElement originControl, FrameworkElement destinationControl)
            {
                Point destination = GetPointFromControl(destinationControl);
                Point origin = GetPointFromControl(originControl);

                Point offsetOrigin;
                //If the origin is to the right of the destination
                if (origin.X > destination.X)
                {
                    //(originControl.ActualHeight / 2)
                    offsetOrigin = new Point(origin.X, origin.Y + RELATIONSHIP_LINE_Y_OFFSET);
                }
                else
                {
                    //(originControl.ActualHeight / 2)
                    offsetOrigin = new Point(origin.X + originControl.ActualWidth, origin.Y + RELATIONSHIP_LINE_Y_OFFSET);
                }

                return offsetOrigin;
            }

            protected override void Dispose(bool disposing)
            {
                if (destinationControl != null && destinationControl.Element != null)
                {
                    destinationControl.Element.Deleted -= ElementDeleted;
                }

                base.Dispose(disposing);
            }
        }

        private class AdornedElement : IDisposable
        {
            readonly ElementModelContainer elementContainerControl;
            readonly Border adorner;
            readonly Canvas relationshipCanvas;
            readonly List<AdornedElementWithRelationshipLine> relatedItems = new List<AdornedElementWithRelationshipLine>();

            public AdornedElement(ElementModelContainer elementContainer, Canvas relationshipCanvas)
            {
                this.elementContainerControl = elementContainer;
                this.adorner = elementContainer.Template.FindName("PART_Adorner", elementContainer) as Border;
                this.relationshipCanvas = relationshipCanvas;

                ShowAdorner();
                elementContainerControl.SizeChanged += ElementSizeChanged;
                elementContainerControl.Element.ElementReferencesChanged += ElementRelationshipsChanged;
            }


            void ElementRelationshipsChanged(object sender, EventArgs e)
            {
                RedrawRelationships();
            }

            void ElementSizeChanged(object sender, SizeChangedEventArgs e)
            {
                RedrawRelationships();
            }

            private void ShowAdorner()
            {
                if (adorner != null)
                {
                    adorner.Visibility = Visibility.Visible;
                }
            }

            public virtual void RedrawRelationships()
            {
                foreach (var adornedRelatedElement in relatedItems)
                {
                    adornedRelatedElement.Dispose();
                }
                relatedItems.Clear();

                if (relationshipCanvas != null)
                {
                    relationshipCanvas.Children.Clear();
                }

                var sectionContainer = VisualTreeWalker.TryFindParent<SectionModelContainer>(relationshipCanvas);

                IEnumerable<ElementViewModel> relatedElements = elementContainerControl.Element.ContainingSection.GetRelatedElements(elementContainerControl.Element);

                foreach (var elementModel in relatedElements)
                {
                    var destinationControl = VisualTreeWalker.FindChild<ElementModelContainer>(x => x.Element.ElementId == elementModel.ElementId, sectionContainer);
                    if (destinationControl != null)
                    {
                        this.relatedItems.Add(new AdornedElementWithRelationshipLine(this, relationshipCanvas, destinationControl));
                    }
                }

                foreach (var relatedItem in relatedItems)
                {
                    relatedItem.DrawRelationships();
                }
            }

            public virtual void DrawRelationships()
            {
                RedrawRelationships();
            }

            public ElementModelContainer ElementContainerControl
            {
                get { return elementContainerControl; }
            }

            #region IDisposable Members

            protected virtual void Dispose(bool disposing)
            {
                if (elementContainerControl != null)
                {
                    elementContainerControl.SizeChanged -= ElementSizeChanged;
                }

                if (elementContainerControl.Element != null)
                {
                    elementContainerControl.Element.ElementReferencesChanged -= ElementRelationshipsChanged;
                }

                if (relationshipCanvas != null)
                {
                    relationshipCanvas.Children.Clear();
                }

                foreach (var relatedItem in relatedItems)
                {
                    relatedItem.Dispose();
                }

                if (adorner != null)
                {
                    adorner.Visibility = Visibility.Collapsed;
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            #endregion
        }

        /// <summary>
        /// Sets the relationship canvas and sets the PART_Content to <see cref="ElementViewModel.Bindable"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            Focusable = true;
            base.OnApplyTemplate();
            this.selectionHelper = new SelectionHelper(this);
            selectionHelper.Attach(SectionModel);

            RelationshipCanvas = Template.FindName("PART_RelationshipCanvas", this) as Canvas;
            Content = Template.FindName("PART_Content", this) as ContentControl;
            if (Content != null) Content.Content = SectionModel.Bindable;
        }

        #region IDisposable Members
        /// <summary>
        /// Indicates the object is being disposed.
        /// </summary>
        /// <param name="disposing">Indicates <see cref="Dispose(bool)"/> was invoked through an explicit call to <see cref="Dispose()"/> instead of a finalizer call.</param>
        /// <filterpriority>2</filterpriority>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveAdorner();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

#pragma warning restore 1591
}
