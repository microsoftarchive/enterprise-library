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
            //Hide relationships
            var container = args.OriginalSource as ElementModelContainer;
            if (container != null)
            {
                //Flip the bit
                container.IsExpanded = !container.IsExpanded;
                //ReDraw the relationship lines 
                ActivateRelationships(container.Content as ElementViewModel);
            }
        }

        public void CanShowRelationships(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            //var container = sender as ElementModelContainer;
            //if (container != null)
            //{
            //    if (container.IsExpanded)
            //        e.CanExecute = false;
            //    else
            //    {
            //        var model = container.Content as ElementViewModel;
            //        if (model != null)
            //        {

            //            IEnumerable<ElementViewModel> relatedModels = SectionModel.GetRelatedElements(model);
            //            foreach (var relatedModel in relatedModels)
            //            {
            //                ElementViewModel theRelatedModel = relatedModel;
            //                Guid elementId = RelationshipMapping.FirstOrDefault(pair => pair.Value == theRelatedModel).Key;
            //                var relatedContainer =
            //                    VisualTreeWalker.FindName<ElementModelContainer>(GetXamlFriendlyName(elementId), RootGrid);

            //                if (relatedContainer!= null && relatedContainer.IsExpanded)
            //                {
            //                    //If any of the related containers are open then we won't show relationships on the whole
            //                    //relationship tree
            //                    e.CanExecute = false;
            //                }
            //            }
            //        }
            //    }
            //}		
        }

        public void OnShowRelationships(object sender, ExecutedRoutedEventArgs e)
        {
            var contentControl = e.OriginalSource as ContentControl;
            if (contentControl != null)
            {
                var model = contentControl.Content as ElementViewModel;
                if (model != null)
                {
                    ActivateRelationships(model);
                }
            }
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

        void BlockVisualizerDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SectionModel = DataContext as SectionViewModel;
            //Set up change handlers
            if (SectionModel != null)
            {
                SectionModel.UpdateVisualGrid += ElementsCollectionChanged;
            }
        }

        private void ActivateRelationships(ElementViewModel model)
        {
            //Remove any previous adorners and lines
            ClearAdorners();
            IEnumerable<ElementViewModel> relatedItems = SectionModel.GetRelatedElements(model);

            Guid originId = RelationshipMapping.FirstOrDefault(pair => pair.Value == model).Key;
            var originControl = VisualTreeWalker.FindName<ContentControl>(GetXamlFriendlyName(originId), RootGrid);

            AdornElementModelControl(originControl);
            foreach (var elementModel in relatedItems)
            {
                Guid destinationId = RelationshipMapping.FirstOrDefault(pair => pair.Value == elementModel).Key;

                //Find the relatedItems in the grid.
                var destinationControl = VisualTreeWalker.FindName<FrameworkElement>(GetXamlFriendlyName(destinationId), RootGrid);
                if (destinationControl != null)
                {
                    Point offsetOrigin = GetOffsetOrigin(originControl, destinationControl);
                    Point offsetDestination = GetOffsetDestination(destinationControl, originControl);
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
            return control.TransformToAncestor(RootGrid).Transform(new Point(0, 0));
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
        private void ClearAdorners()
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

            GenerateGridLayout();
            RelationshipCanvas = Template.FindName("PART_RelationshipCanvas", this) as Canvas;
        }

        private void ElementsCollectionChanged(object sender, EventArgs e)
        {
            GenerateGridLayout();
        }

        private static string GetXamlFriendlyName(Guid contentControlId)
        {
            return String.Format("{0}{1}", "_", contentControlId.ToString().Replace("-", "_"));
        }

        #region Grid Layout Methods
        private void GenerateGridLayout()
        {
            RootGrid = Template.FindName("PART_RootGrid", this) as Grid;
            if (RootGrid != null)
            {
                //Clear any previous children 
                RootGrid.Children.Clear();
                ClearAdorners();
                RelationshipMapping.Clear();

                GenerateGridColumns(RootGrid);
                GenerateGridRows(RootGrid);

                foreach (ViewModel.ViewModel elementModel in SectionModel.GetGridVisuals())
                {
                    LayoutElementInGrid(RootGrid, elementModel);
                }
            }
        }

        private static GridWidthConverter widthConverter = new GridWidthConverter();
        private void LayoutElementInGrid(Grid theGrid, ViewModel.ViewModel elementModel)
        {
            var containerWidthBinding = new Binding
                                            {
                                                Source = theGrid.ColumnDefinitions[(elementModel.Column) * 2],
                                                Converter = widthConverter,
                                                Mode = BindingMode.TwoWay,
                                                Path = new PropertyPath("Width")
                                            };

            FrameworkElement control;
            control = elementModel.CustomVisual ?? new ElementModelContainer
                                                       {
                                                           Content = elementModel,
                                                           Focusable = true
                                                       };

            control.SetBinding(MaxWidthProperty, containerWidthBinding);

            control.CommandBindings.Add(new CommandBinding(ShowRelationships,
                                                                  OnShowRelationships,
                                                                  CanShowRelationships));
            Grid.SetRow(control, elementModel.Row);

            //Account for grid splitters
            Grid.SetColumn(control, (elementModel.Column) * 2);

            // todo: Set rowspan instead of min
            Grid.SetRowSpan(control, Math.Max(1, elementModel.RowSpan));

            var contentControlId = Guid.NewGuid();
            //Ensure unique names for all content controls
            control.Name = GetXamlFriendlyName(contentControlId);
            theGrid.Children.Add(control);
            //Add an entry for lookup later when relationships are visualized
            ElementViewModel relatableElement = elementModel as ElementViewModel;
            if (relatableElement != null)
            {
                //Only store a mapping for real element view models (not header or sections)
                RelationshipMapping.Add(contentControlId, relatableElement);
            }
        }

        private void GenerateGridRows(Grid theGrid)
        {
            theGrid.RowDefinitions.Clear();
            for (int i = 0; i <= SectionModel.Rows; i++)
            {
                theGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
            }
        }

        private void GenerateGridColumns(Grid theGrid)
        {
            theGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < SectionModel.Columns; i++)
            {
                theGrid.ColumnDefinitions.Add(new ColumnDefinition
                                                {
                                                    Width = new GridLength(1, GridUnitType.Auto),
                                                    MinWidth = 60
                                                });

                //Add a column and splitter in between each column
                theGrid.ColumnDefinitions.Add(new ColumnDefinition
                                                {
                                                    Width = new GridLength(3, GridUnitType.Pixel),
                                                });
            }

            // Add extra column definition for the grid splitter to work appropriately
            theGrid.ColumnDefinitions.Add( new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto)});

            AddGridSplitters(theGrid);
        }

        private void AddGridSplitters(Grid theGrid)
        {
            for (int i = 1; i < (SectionModel.Columns * 2); i += 2)
            {
                var splitter = new GridSplitter()
                                {
                                    BorderBrush = Brushes.Black,
                                    BorderThickness = new Thickness(1),
                                    Margin = new Thickness(-26, 0, 0, 0),
                                    Opacity = .15,
                                    Background = Brushes.Transparent,
                                    Width = 3,
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    VerticalAlignment = VerticalAlignment.Stretch
                                };
                splitter.DragStarted += SplitterDragStarted;

                Grid.SetColumn(splitter, i);
                Grid.SetRowSpan(splitter, SectionModel.Rows + 1);
                theGrid.Children.Add(splitter);
            }
        }

        void SplitterDragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            if (RelationshipCanvas.Children.Count > 0)
            {
                ClearAdorners();
            }
        }

        public static void RemoveItemFromGrid(Grid theGrid, ContentControl elementToRemove)
        {
            theGrid.Children.Remove(elementToRemove);
        }
        #endregion
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
