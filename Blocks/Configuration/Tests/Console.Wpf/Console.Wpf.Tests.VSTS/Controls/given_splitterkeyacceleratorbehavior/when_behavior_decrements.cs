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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Controls.given_splitterkeyacceleratorbehavior
{
    [TestClass]
    public class when_behavior_decrements: GridBehaviorContext
    {
        protected override void Act()
        {
            Behavior.DoMove(-5);
        }

        [TestMethod]
        public void then_colum_decreses_by_amount()
        {
            Assert.AreEqual(OriginalWidth - 5, ColumnDefinition.Width.Value);
        }
    }

    [TestClass]
    public class when_behavior_attempts_decrement_below_min : GridBehaviorContext
    {
        protected override void Arrange()
        {
            base.Arrange();

            ColumnDefinition.MinWidth = 51;
        }

        protected override void Act()
        {
            Behavior.DoMove(-5);
        }

        [TestMethod]
        public void then_does_not_decrement()
        {
            Assert.AreEqual(OriginalWidth, this.ColumnDefinition.Width.Value);
        }
    }
}
