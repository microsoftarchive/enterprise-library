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

using System.Collections.Generic;
using System.Windows;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Threading;

namespace Console.Wpf.Tests.VSTS.Controls.TypeBrowser
{
    [TestClass]
    public class TypeBrowserViewModelFixture
    {
        // initialization
        [TestMethod]
        public void ViewModelIsInitialized()
        {
            var groups =
                new[] {
                    new AssemblyGroup(
                        "test",
                        new[] {
                            typeof(TestAssembly1.Namespace1.Class1).Assembly,
                            typeof(TestAssembly2.Namespace1.Class1).Assembly})};

            var model = new TypeBrowserViewModel(null);
            model.UpdateAssemblyGroups(groups);

            Assert.IsNull(model.TypeName);
            Assert.IsNull(model.ConcreteType);
            Assert.IsFalse(model.HasGenericParameters);
            Assert.AreEqual(0, model.GenericTypeParameters.Count);
            TreeAssert.IsMatch(
                new
                {
                    DisplayName = "test",
                    Assemblies = new object[]
                        {
                            new 
                            {
                                DisplayName = "TestAssembly1",
                                Visibility = Visibility.Visible, IsExpanded = false,
                                Namespaces = new object []
                                    {
                                        new 
                                        {
                                            DisplayName = "TestAssembly1.Namespace1",
                                            Visibility = Visibility.Visible, IsExpanded = false,
                                            Types = new object[]
                                                {
                                                    new 
                                                    {
                                                        DisplayName = "Class1",
                                                        FullName = "TestAssembly1.Namespace1.Class1",
                                                        Visibility = Visibility.Visible, IsExpanded = false
                                                    },
                                                    new 
                                                    {
                                                        DisplayName = "Class2",
                                                        FullName = "TestAssembly1.Namespace1.Class2",
                                                        Visibility = Visibility.Visible, IsExpanded = false
                                                    },
                                                    new 
                                                    {
                                                        DisplayName = "InternalClass1",
                                                        FullName = "TestAssembly1.Namespace1.InternalClass1",
                                                        Visibility = Visibility.Visible, IsExpanded = false
                                                    },
                                                }
                                        },
                                        new 
                                        {
                                            DisplayName = "TestAssembly1.Namespace2",
                                            Visibility = Visibility.Visible, IsExpanded = false,
                                            Types = new object[]
                                                {
                                                    new 
                                                    {
                                                        DisplayName = "AnotherInternalClass",
                                                        FullName = "TestAssembly1.Namespace2.AnotherInternalClass",
                                                        Visibility = Visibility.Visible, IsExpanded = false
                                                    }
                                                }

                                        }
                                    }
                            },
                            new 
                            {
                                DisplayName = "TestAssembly2",
                                Visibility = Visibility.Visible, IsExpanded = false,
                                Namespaces = new object []
                                    {
                                        new 
                                        {
                                            DisplayName = "TestAssembly2",
                                            Visibility = Visibility.Visible, IsExpanded = false,
                                            Types = new object[]
                                                {
                                                    new 
                                                    {
                                                        DisplayName = "Class1",
                                                        FullName = "TestAssembly2.Class1",
                                                        Visibility = Visibility.Visible, IsExpanded = false
                                                    },
                                                    new 
                                                    {
                                                        DisplayName = "NamespaceClass",
                                                        FullName = "TestAssembly2.NamespaceClass",
                                                        Visibility = Visibility.Visible, IsExpanded = false
                                                    }
                                                }
                                        },
                                        new 
                                        {
                                            DisplayName = "TestAssembly2.Namespace1",
                                            Visibility = Visibility.Visible, IsExpanded = false,
                                            Types = new object[]
                                                {
                                                    new 
                                                    {
                                                        DisplayName = "Class1",
                                                        FullName = "TestAssembly2.Namespace1.Class1",
                                                        Visibility = Visibility.Visible, IsExpanded = false
                                                    },
                                                    new 
                                                    {
                                                        DisplayName = "Class12",
                                                        FullName = "TestAssembly2.Namespace1.Class12",
                                                        Visibility = Visibility.Visible, IsExpanded = false
                                                    }
                                                }

                                        }
                                    }
                            }
                        }
                },
            model.AssemblyGroups[0]);
        }

        // selection

        [TestMethod]
        public void SelectingNonGenericTypeNode_UpdatesTextToFullTypeName()
        {
            var groups =
                new[] {
                    new AssemblyGroup(
                        "test",
                        new[] {
                            typeof(TestAssembly1.Namespace1.Class1).Assembly,
                            typeof(TestAssembly2.Namespace1.Class1).Assembly})};

            var model = new TypeBrowserViewModel(null);
            model.UpdateAssemblyGroups(groups);

            var modifiedProperties = new List<string>();
            model.PropertyChanged += (s, a) =>
                {
                    modifiedProperties.Add(a.PropertyName);
                };

            model.AssemblyGroups[0].Assemblies[0].Namespaces[0].Types[0].IsSelected = true;

            CollectionAssert.AreEqual(new[] { "HasGenericParameters", "ConcreteType", "TypeName" }, modifiedProperties);
            Assert.IsNotNull(model.ConcreteType);
            Assert.IsFalse(model.HasGenericParameters);
            Assert.AreEqual("TestAssembly1.Namespace1.Class1", model.TypeName);
        }

        [TestMethod]
        public void SelectingNonTypeNodeWhenTypeIsSelected_DoesChangeTextAndKeepsSelectedType()
        {
            var groups =
                new[] {
                    new AssemblyGroup(
                        "test",
                        new[] {
                            typeof(TestAssembly1.Namespace1.Class1).Assembly,
                            typeof(TestAssembly2.Namespace1.Class1).Assembly})};

            var model = new TypeBrowserViewModel(null);
            model.UpdateAssemblyGroups(groups);

            model.AssemblyGroups[0].Assemblies[0].Namespaces[0].Types[0].IsSelected = true;

            var modifiedProperties = new List<string>();
            model.PropertyChanged += (s, a) =>
                {
                    modifiedProperties.Add(a.PropertyName);
                };

            // simulate selection change
            model.AssemblyGroups[0].Assemblies[0].Namespaces[0].Types[0].IsSelected = false;
            model.AssemblyGroups[0].Assemblies[0].Namespaces[1].IsSelected = true;

            Assert.AreEqual(0, modifiedProperties.Count);
            Assert.IsNotNull(model.ConcreteType);
            Assert.IsFalse(model.HasGenericParameters);
            Assert.AreEqual("TestAssembly1.Namespace1.Class1", model.TypeName);
        }

        // search

        [TestMethod]
        public void UpdatingTheTypeName_PerformsSearch()
        {
            var groups =
                new[] {
                    new AssemblyGroup(
                        "test",
                        new[] {
                            typeof(TestAssembly1.Namespace1.Class1).Assembly,
                            typeof(TestAssembly2.Namespace1.Class1).Assembly})};

            var model = new TypeBrowserViewModel(null);
            model.UpdateAssemblyGroups(groups);

            var modifiedProperties = new List<string>();
            model.PropertyChanged += (s, a) =>
                {
                    modifiedProperties.Add(a.PropertyName);
                };

            var frame = new DispatcherFrame();
            model.AssemblyGroups[0].Assemblies[0].Namespaces[0].PropertyChanged += (s, a) =>
                {
                    frame.Continue = false;
                };

            model.TypeName = "Class1";
            Dispatcher.PushFrame(frame);

            TreeAssert.IsMatch(
                new
                {
                    DisplayName = "test",
                    Assemblies = new object[]
                        {
                            new 
                            {
                                DisplayName = "TestAssembly1",
                                Visibility = Visibility.Visible, IsExpanded = true,
                                Namespaces = new object []
                                    {
                                        new 
                                        {
                                            DisplayName = "TestAssembly1.Namespace1",
                                            Visibility = Visibility.Visible, IsExpanded = true,
                                            Types = new object[]
                                                {
                                                    new 
                                                    {
                                                        DisplayName = "Class1",
                                                        FullName = "TestAssembly1.Namespace1.Class1",
                                                        Visibility = Visibility.Visible, IsExpanded = false
                                                    },
                                                    new 
                                                    {
                                                        DisplayName = "Class2",
                                                        FullName = "TestAssembly1.Namespace1.Class2",
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
                                                    },
                                                    new 
                                                    {
                                                        DisplayName = "InternalClass1",
                                                        FullName = "TestAssembly1.Namespace1.InternalClass1",
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
                                                    },
                                                }
                                        },
                                        new 
                                        {
                                            DisplayName = "TestAssembly1.Namespace2",
                                            Visibility = Visibility.Collapsed, IsExpanded = false,
                                            Types = new object[]
                                                {
                                                    new 
                                                    {
                                                        DisplayName = "AnotherInternalClass",
                                                        FullName = "TestAssembly1.Namespace2.AnotherInternalClass",
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
                                                    }
                                                }

                                        }
                                    }
                            },
                            new 
                            {
                                DisplayName = "TestAssembly2",
                                Visibility = Visibility.Visible, IsExpanded = true,
                                Namespaces = new object []
                                    {
                                        new 
                                        {
                                            DisplayName = "TestAssembly2",
                                            Visibility = Visibility.Visible, IsExpanded = true,
                                            Types = new object[]
                                                {
                                                    new 
                                                    {
                                                        DisplayName = "Class1",
                                                        FullName = "TestAssembly2.Class1",
                                                        Visibility = Visibility.Visible, IsExpanded = false
                                                    },
                                                    new 
                                                    {
                                                        DisplayName = "NamespaceClass",
                                                        FullName = "TestAssembly2.NamespaceClass",
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
                                                    }
                                                }
                                        },
                                        new 
                                        {
                                            DisplayName = "TestAssembly2.Namespace1",
                                            Visibility = Visibility.Visible, IsExpanded = true,
                                            Types = new object[]
                                                {
                                                    new 
                                                    {
                                                        DisplayName = "Class1",
                                                        FullName = "TestAssembly2.Namespace1.Class1",
                                                        Visibility = Visibility.Visible, IsExpanded = false
                                                    },
                                                    new 
                                                    {
                                                        DisplayName = "Class12",
                                                        FullName = "TestAssembly2.Namespace1.Class12",
                                                        Visibility = Visibility.Visible, IsExpanded = false
                                                    }
                                                }

                                        }
                                    }
                            }
                        }
                },
                model.AssemblyGroups[0]);
        }

        // selection through search
        [TestMethod]
        public void UpdatingTheTypeNameToASingleMatch_PerformsSearchAndSetsSelectionButDoesNotChangeTypeName()
        {
            var groups =
                new[] {
                    new AssemblyGroup(
                        "test",
                        new[] {
                            typeof(TestAssembly1.Namespace1.Class1).Assembly,
                            typeof(TestAssembly2.Namespace1.Class1).Assembly})};

            var model = new TypeBrowserViewModel(null);
            model.UpdateAssemblyGroups(groups);

            var modifiedProperties = new List<string>();
            model.PropertyChanged += (s, a) =>
                {
                    modifiedProperties.Add(a.PropertyName);
                };

            var frame = new DispatcherFrame();
            model.AssemblyGroups[0].Assemblies[0].Namespaces[0].PropertyChanged += (s, a) =>
                {
                    frame.Continue = false;
                };

            model.TypeName = "NamespaceClass";
            Dispatcher.PushFrame(frame);

            Assert.IsNotNull(model.ConcreteType);
            Assert.AreEqual("NamespaceClass", model.TypeName);
            TreeAssert.IsMatch(
                new
                {
                    DisplayName = "test",
                    Assemblies = new object[]
                        {
                            new 
                            {
                                DisplayName = "TestAssembly1",
                                Visibility = Visibility.Collapsed, IsExpanded = false,
                                Namespaces = new object []
                                    {
                                        new 
                                        {
                                            DisplayName = "TestAssembly1.Namespace1",
                                            Visibility = Visibility.Collapsed, IsExpanded = false,
                                            Types = new object[]
                                                {
                                                    new 
                                                    {
                                                        DisplayName = "Class1",
                                                        FullName = "TestAssembly1.Namespace1.Class1",
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
                                                    },
                                                    new 
                                                    {
                                                        DisplayName = "Class2",
                                                        FullName = "TestAssembly1.Namespace1.Class2",
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
                                                    },
                                                    new 
                                                    {
                                                        DisplayName = "InternalClass1",
                                                        FullName = "TestAssembly1.Namespace1.InternalClass1",
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
                                                    },
                                                }
                                        },
                                        new 
                                        {
                                            DisplayName = "TestAssembly1.Namespace2",
                                            Visibility = Visibility.Collapsed, IsExpanded = false,
                                            Types = new object[]
                                                {
                                                    new 
                                                    {
                                                        DisplayName = "AnotherInternalClass",
                                                        FullName = "TestAssembly1.Namespace2.AnotherInternalClass",
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
                                                    }
                                                }

                                        }
                                    }
                            },
                            new 
                            {
                                DisplayName = "TestAssembly2",
                                Visibility = Visibility.Visible, IsExpanded = true,
                                Namespaces = new object []
                                    {
                                        new 
                                        {
                                            DisplayName = "TestAssembly2",
                                            Visibility = Visibility.Visible, IsExpanded = true,
                                            Types = new object[]
                                                {
                                                    new 
                                                    {
                                                        DisplayName = "Class1",
                                                        FullName = "TestAssembly2.Class1",
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
                                                    },
                                                    new 
                                                    {
                                                        DisplayName = "NamespaceClass",
                                                        FullName = "TestAssembly2.NamespaceClass",
                                                        Visibility = Visibility.Visible, IsExpanded = false
                                                    }
                                                }
                                        },
                                        new 
                                        {
                                            DisplayName = "TestAssembly2.Namespace1",
                                            Visibility = Visibility.Collapsed, IsExpanded = false,
                                            Types = new object[]
                                                {
                                                    new 
                                                    {
                                                        DisplayName = "Class1",
                                                        FullName = "TestAssembly2.Namespace1.Class1",
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
                                                    },
                                                    new 
                                                    {
                                                        DisplayName = "Class12",
                                                        FullName = "TestAssembly2.Namespace1.Class12",
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
                                                    }
                                                }

                                        }
                                    }
                            }
                        }
                },
                model.AssemblyGroups[0]);
        }
    }
}
