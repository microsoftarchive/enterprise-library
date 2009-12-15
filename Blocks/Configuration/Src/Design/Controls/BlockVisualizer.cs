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
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Converters;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;


namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Visualizer"),
    TemplatePart(Name = "PART_RootGrid", Type = typeof(Grid))]
    public class BlockVisualizer : Control
    {
        private Dictionary<Guid, ElementViewModel> RelationshipMapping { get; set; }
        private Dictionary<FrameworkElement, Border> ActiveAdorners { get; set; }
        public RoutedCommand ShowRelationships { get; set; }

        protected SectionViewModel SectionModel { get; set; }
        private Grid RootGrid { get; set; }
        private Canvas RelationshipCanvas { get; set; }
        private ContentControl Content { get; set; }

        private const int RELATIONSHIP_LINE_Y_OFFSET = 12;

        static BlockVisualizer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BlockVisualizer), new FrameworkPropertyMetadata(typeof(BlockVisualizer)));
        }


        public BlockVisualizer()
        {
            DataContextChanged += BlockVisualizerDataContextChanged;
            GotFocus += BlockVisualizerGotFocus;

            var commandGestures = new InputGestureCollection
			                      	{
			                      		new MouseGesture(MouseAction.LeftClick)
			                      	};

            ShowRelationships = new RoutedUICommand("Show ElementModel Relationships",
                                                    "ShowRelationships",
                                                    typeof(BlockVisualizer),
                                                    commandGestures);

            RelationshipMapping = new Dictionary<Guid, ElementViewModel>();
            ActiveAdorners = new Dictionary<FrameworkElement, Border>();

            AddHandler(ElementModelContainer.ShowPropertiesEvent, new PropertiesRoutedEventHandler(HandleShowProperties));
        }

        private void HandleShowProperties(object sender, PropertiesRoutedEventArgs args)
        {
            ClearAdorners();

            //Hide relationships
            var container = args.OriginalSource as ElementModelContainer;
            if (container != null)
            {
                //Flip the bit
                container.IsExpanded = !container.IsExpanded;
                //ReDraw the relationship lines 
                ActivateRelationships(container);
            }
        }

        public void CanShowRelationships(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void BlockVisualizerGotFocus(object sender, RoutedEventArgs e)
        {
            //All focus events will bubble up to here so we can activate the relationships
            //as focus shifts
            var contentControl = e.OriginalSource as ContentControl;
            if (contentControl != null)
            {
                if (ShowRelationships.CanExecute(null, contentControl))
                {
                    ShowRelationships.Execute(null, contentControl);
                }
            }
        }


        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
        }

        void BlockVisualizerDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SectionModel = DataContext as SectionViewModel;
            SectionModel.DescendentElementsChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(SectionModel_DescendentElementsChanged);
        }

        void SectionModel_DescendentElementsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ClearAdorners();
        }

        internal void SetSelectedElement(ElementModelContainer elementContainer)
        {
            ActivateRelationships(elementContainer);
        }

        public void ActivateRelationships(ElementModelContainer elementContainer)
        { 
            //Remove any previous adorners and lines
            ClearAdorners();
            IEnumerable<ElementViewModel> relatedItems = SectionModel.GetRelatedElements(elementContainer.Element);

            AdornElementModelControl(elementContainer);
            foreach (var elementModel in relatedItems)
            {
                Guid destinationId = RelationshipMapping.FirstOrDefault(pair => pair.Value == elementModel).Key;

                //Find the relatedItems in the grid.
                var destinationControl = VisualTreeWalker.FindChild<ElementModelContainer>(x => x.Element == elementModel, this);
                if (destinationControl != null)
                {
                    Point offsetOrigin = GetOffsetOrigin(elementContainer, destinationControl);
                    Point offsetDestination = GetOffsetDestination(destinationControl, elementContainer);
                    DrawRelationshipLines(offsetOrigin, offsetDestination);
                    AdornElementModelControl(destinationControl);
                }
            }
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
            return control.TransformToAncestor(this).Transform(new Point(0, 0));
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

        #region Relationship Visualization
        public void ClearAdorners()
        {
            foreach (var activeAdorner in ActiveAdorners)
            {
                //Remove the adorners
                Border adorner = activeAdorner.Value;
                adorner.Visibility = Visibility.Collapsed;
            }

            //clear the dictionary
            ActiveAdorners.Clear();
            ClearRelationshipLines();
        }

        private void ClearRelationshipLines()
        {
            if (RelationshipCanvas != null)
            {
                RelationshipCanvas.Children.Clear();
            }
        }

        private void AdornElementModelControl(FrameworkElement control)
        {
            var container = control as ElementModelContainer;
            if (container != null)
            {
                var adorner = container.Template.FindName("PART_Adorner", container) as Border;
                if (adorner != null)
                {
                    adorner.Visibility = Visibility.Visible;
                    //Add to the active adorners collection so we can clear them later
                    if (!ActiveAdorners.ContainsKey(control))
                    {
                        ActiveAdorners.Add(control, adorner);
                    }
                }
            }
        }

        private void DrawRelationshipLines(Point origin, Point destination)
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
            RelationshipCanvas.Children.Add(relationshipPath);
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            RelationshipCanvas = Template.FindName("PART_RelationshipCanvas", this) as Canvas;
            Content = Template.FindName("PART_Content", this) as ContentControl;
            Content.Content = SectionModel.Bindable;
        }

        private void ElementsCollectionChanged(object sender, EventArgs e)
        {
        }

    }

    internal delegate void PropertiesRoutedEventHandler(object sender, PropertiesRoutedEventArgs args);
    internal class PropertiesRoutedEventArgs : RoutedEventArgs
    {
        public Boolean IsOpen { get; set; }
        public PropertiesRoutedEventArgs(RoutedEvent routedEvent)
            : base(routedEvent)
        {

        }
    }
}
