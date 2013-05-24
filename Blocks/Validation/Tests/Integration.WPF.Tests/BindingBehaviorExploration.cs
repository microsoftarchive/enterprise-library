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

using System.Windows;
using System.Windows.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWC = System.Windows.Controls;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WPF.Tests
{
    [TestClass]
    public class BindingBehaviorExploration
    {
        [TestMethod]
        public void BindingSetsValue()
        {
            var source = new BindingTestSource { SourceProperty = 100 };
            var target = new BindingTestTarget();
            var binding = new Binding("SourceProperty")
            {
                Source = source,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(target, BindingTestTarget.TargetPropertyProperty, binding);

            Assert.AreEqual("100", target.TargetProperty);

            target.TargetProperty = "10";

            Assert.AreEqual("10", target.TargetProperty);
            Assert.AreEqual(10, source.SourceProperty);
            Assert.IsFalse(SWC.Validation.GetHasError(target));
        }

        [TestMethod]
        public void BindingLogsErrorIfValueIfValueConversionFails()
        {
            var source = new BindingTestSource { SourceProperty = 100 };
            var target = new BindingTestTarget();
            var binding = new Binding("SourceProperty")
            {
                Source = source,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(target, BindingTestTarget.TargetPropertyProperty, binding);

            Assert.AreEqual("100", target.TargetProperty);

            target.TargetProperty = "zzz";

            Assert.AreEqual("zzz", target.TargetProperty);
            Assert.AreEqual(100, source.SourceProperty);
            Assert.IsTrue(SWC.Validation.GetHasError(target));
        }

        [TestMethod]
        public void BindingLogsErrorIfValueConversionFailsAndBindingObservesExceptions()
        {
            var source = new BindingTestSource { SourceProperty = 100 };
            var target = new BindingTestTarget();
            var binding = new Binding("SourceProperty")
            {
                Source = source,
                Mode = BindingMode.TwoWay,
                ValidatesOnExceptions = true
            };
            BindingOperations.SetBinding(target, BindingTestTarget.TargetPropertyProperty, binding);

            Assert.AreEqual("100", target.TargetProperty);

            target.TargetProperty = "zzz";

            Assert.AreEqual("zzz", target.TargetProperty);
            Assert.AreEqual(100, source.SourceProperty);
            Assert.IsTrue(SWC.Validation.GetHasError(target));
        }

        [TestMethod]
        public void BindingForPropertyInvalidPathIsIgnoredIfNotThrowingExceptions()
        {
            var source = new BindingTestSource { SourceProperty = 100 };
            var target = new BindingTestTarget();
            var binding = new Binding("InvalidProperty")
            {
                Source = source,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(target, BindingTestTarget.TargetPropertyProperty, binding);

            target.TargetProperty = "10";

            Assert.AreEqual("10", target.TargetProperty);
            Assert.AreEqual(100, source.SourceProperty);
            Assert.IsFalse(SWC.Validation.GetHasError(target));
        }

        [TestMethod]
        public void ClearingABinding()
        {
            var source = new BindingTestSource { SourceProperty = 100 };
            var target = new BindingTestTarget();
            var binding = new Binding("SourceProperty")
            {
                Source = source,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(target, BindingTestTarget.TargetPropertyProperty, binding);

            var bindingOperation = BindingOperations.GetBindingExpression(target, BindingTestTarget.TargetPropertyProperty);

            Assert.AreEqual("100", target.TargetProperty);

            BindingOperations.ClearBinding(target, BindingTestTarget.TargetPropertyProperty);

            target.TargetProperty = "10";

            Assert.AreEqual("10", target.TargetProperty);
            Assert.AreEqual(100, source.SourceProperty);
            Assert.IsFalse(SWC.Validation.GetHasError(target));
        }

        [TestMethod]
        public void DataItemIsByDefault()
        {
            var target = new BindingTestTarget();
            var binding = new Binding("SourceProperty")
            {
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(target, BindingTestTarget.TargetPropertyProperty, binding);

            Assert.IsNull(target.TargetProperty);

            var bindingExpression = BindingOperations.GetBindingExpression(target, BindingTestTarget.TargetPropertyProperty);

            Assert.IsNull(bindingExpression.DataItem);
            Assert.AreEqual("SourceProperty", bindingExpression.ParentBinding.Path.Path);
        }

        [TestMethod]
        public void DataItemIsDataContextIfSourceIsNotExplicitlySet()
        {
            var target = new BindingTestTarget();
            var sourceDataContext = new BindingTestSource();
            target.DataContext = sourceDataContext;
            var binding = new Binding("SourceProperty")
            {
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(target, BindingTestTarget.TargetPropertyProperty, binding);

            var bindingExpression = BindingOperations.GetBindingExpression(target, BindingTestTarget.TargetPropertyProperty);

            Assert.AreSame(sourceDataContext, bindingExpression.DataItem);
            Assert.AreEqual("SourceProperty", bindingExpression.ParentBinding.Path.Path);
        }

        [TestMethod]
        public void DataItemIsSourceIfExplicitlySet()
        {
            var target = new BindingTestTarget();
            var sourceExplicit = new BindingTestSource();
            var sourceDataContext = new BindingTestSource();
            target.DataContext = sourceDataContext;
            var binding = new Binding("SourceProperty")
            {
                Source = sourceExplicit,
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(target, BindingTestTarget.TargetPropertyProperty, binding);

            var bindingExpression = BindingOperations.GetBindingExpression(target, BindingTestTarget.TargetPropertyProperty);

            Assert.AreSame(sourceExplicit, bindingExpression.DataItem);
            Assert.AreEqual("SourceProperty", bindingExpression.ParentBinding.Path.Path);
        }

        public class BindingTestTarget : FrameworkElement
        {
            public static readonly DependencyProperty TargetPropertyProperty =
                DependencyProperty.Register(
                    "TargetProperty",
                    typeof(string),
                    typeof(BindingTestTarget));

            public string TargetProperty
            {
                get { return (string)base.GetValue(TargetPropertyProperty); }
                set { base.SetValue(TargetPropertyProperty, value); }
            }
        }

        public class BindingTestSource
        {
            public int SourceProperty { get; set; }
        }
    }
}
