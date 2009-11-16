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

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls
{
	public class RelationshipAdorner : Adorner 
	{

		public RelationshipAdorner(UIElement adornerElement) : base(adornerElement)
		{
			IsHitTestVisible = false;
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			Brush strokeBrush;
			try
			{
				strokeBrush = FindResource("AdornerStrokeBrush") as Brush;
			}
			catch (ResourceReferenceKeyNotFoundException)
			{
				strokeBrush = new SolidColorBrush(Color.FromRgb(182,182,218));
			}

			Brush fillBrush;
			try
			{
				fillBrush = FindResource("AdornerFillBrush") as Brush;
			}
			catch (ResourceReferenceKeyNotFoundException)
			{
				var brush = new SolidColorBrush(Color.FromRgb(185, 185, 255))
				            	{
				            		Opacity = .25
				            	};
				fillBrush = brush;
			}

			drawingContext.DrawRectangle(fillBrush,
			                             new Pen(strokeBrush, 1),
			                             new Rect(new Point(0, 0), DesiredSize));
			base.OnRender(drawingContext);
		}
	}
}
