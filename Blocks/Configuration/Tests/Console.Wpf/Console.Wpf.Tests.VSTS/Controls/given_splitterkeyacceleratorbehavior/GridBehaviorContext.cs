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
using System.Windows.Controls;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;

namespace Console.Wpf.Tests.VSTS.Controls.given_splitterkeyacceleratorbehavior
{
    public abstract class GridBehaviorContext : ArrangeActAssert
    {
        protected ColumnDefinition ColumnDefinition { get; private set;}
        protected Grid Grid { get; private set; }
        protected TestableSplitterBehavior Behavior { get; private set; }

        protected readonly double OriginalWidth = 50;

        protected override void Arrange()
        {
            base.Arrange();

            this.Grid = new Grid();
            this.Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(OriginalWidth) });
            ColumnDefinition = this.Grid.ColumnDefinitions[0];

            Behavior = new TestableSplitterBehavior();
            Behavior.Attach(this.Grid);
        }

        protected class TestableSplitterBehavior : GridSplitterKeyAcceleratorBehavior
        {
            public void DoMove(double value)
            {
                base.MoveGrid(value);
            }
        }
    }
}
