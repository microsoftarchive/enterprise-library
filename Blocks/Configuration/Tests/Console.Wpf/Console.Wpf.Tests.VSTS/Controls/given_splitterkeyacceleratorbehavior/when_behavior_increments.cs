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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Controls.given_splitterkeyacceleratorbehavior
{
    [TestClass]
    public class when_behavior_increments : GridBehaviorContext
    {
        protected override void Act()
        {
            Behavior.DoMove(5);
        }

        [TestMethod]
        public void then_column_increases_by_amount()
        {
            Assert.AreEqual(OriginalWidth + 5, ColumnDefinition.Width.Value);
        }
    }

    [TestClass]
    public class when_behavior_attempts_increment_above_max : GridBehaviorContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            ColumnDefinition.MaxWidth = 49;
        }

        protected override void Act()
        {
            Behavior.DoMove(5);
        }

        [TestMethod]
        public void then_does_not_increment()
        {
            Assert.AreEqual(OriginalWidth, this.ColumnDefinition.Width.Value);
        }
    }

   

   [TestClass]
    public class when_behavior_references_non_existent_column : GridBehaviorContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            this.Grid.SetValue(GridSplitterKeyAccelerator.TargetColumnProperty, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void then_grid_move_should_throw()
        {
            this.Behavior.DoMove(10);    
        }
    }
}
