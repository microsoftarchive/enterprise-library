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

using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.Controls.TypeBrowser
{
    [TestClass]
    public class SearchOperationFixture
    {
        private TypeBrowserViewModel model;

        [TestInitialize]
        public void SetUp()
        {
            var groups =
                new[] {
                    new AssemblyGroup(
                        "test", 
                        new [] 
                        {
                            typeof(TestAssembly1.Namespace1.Class1).Assembly, 
                            typeof(TestAssembly2.Namespace1.Class1).Assembly
                        })
                };
            this.model = new TypeBrowserViewModel(null);
            this.model.UpdateAssemblyGroups(groups);
        }

        [TestMethod]
        public void EmptyString()
        {
            // everything visible, everything collapsed
            var match =
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
                };


            var action = new SearchAction("", model.AssemblyGroups);

            var frame = new DispatcherFrame();
            TypeNode selectedNode = null;
            action.Completed += (s, a) => { selectedNode = a.Result; frame.Continue = false; };
            action.Run();
            Dispatcher.PushFrame(frame);

            TreeAssert.IsMatch(match, model.AssemblyGroups[0]);
            Assert.IsNull(selectedNode);
        }

        [TestMethod]
        public void PrefixForExistingClassName_CollapsesNonMatchingClassNodeAndParentNodesWithNoMatchingChildren()
        {
            // collapses non-matching types
            // collapses parents with no visible children
            // expands parents with visible children
            var match =
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
                                                        Visibility = Visibility.Visible, IsExpanded = false
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
                };


            var action = new SearchAction("Cla", model.AssemblyGroups);

            var frame = new DispatcherFrame();
            TypeNode selectedNode = null;
            action.Completed += (s, a) => { selectedNode = a.Result; frame.Continue = false; };
            action.Run();
            Dispatcher.PushFrame(frame);

            TreeAssert.IsMatch(match, model.AssemblyGroups[0]);
            Assert.IsNull(selectedNode);
        }

        [TestMethod]
        public void PrefixForNonExistingClassName_CollapsesEverything()
        {
            // collapses everything
            var match =
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
                                Visibility = Visibility.Collapsed, IsExpanded = false,
                                Namespaces = new object []
                                    {
                                        new 
                                        {
                                            DisplayName = "TestAssembly2",
                                            Visibility = Visibility.Collapsed, IsExpanded = false,
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
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
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
                };


            var action = new SearchAction("Invalid", model.AssemblyGroups);

            var frame = new DispatcherFrame();
            TypeNode selectedNode = null;
            action.Completed += (s, a) => { selectedNode = a.Result; frame.Continue = false; };
            action.Run();
            Dispatcher.PushFrame(frame);

            TreeAssert.IsMatch(match, model.AssemblyGroups[0]);
            Assert.IsNull(selectedNode);
        }

        [TestMethod]
        public void PartialMatchForUniqueName_SelectsMatchingEntry()
        {
            // collapses everything
            var match =
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
                                                        Visibility = Visibility.Visible, IsExpanded = false
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
                                Visibility = Visibility.Collapsed, IsExpanded = false,
                                Namespaces = new object []
                                    {
                                        new 
                                        {
                                            DisplayName = "TestAssembly2",
                                            Visibility = Visibility.Collapsed, IsExpanded = false,
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
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
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
                };


            var action = new SearchAction("Internal", model.AssemblyGroups);

            var frame = new DispatcherFrame();
            TypeNode selectedNode = null;
            action.Completed += (s, a) => { selectedNode = a.Result; frame.Continue = false; };
            action.Run();
            Dispatcher.PushFrame(frame);

            TreeAssert.IsMatch(match, model.AssemblyGroups[0]);
            Assert.IsNotNull(selectedNode);
            Assert.AreEqual("TestAssembly1.Namespace1.InternalClass1", selectedNode.FullName);
        }

        [TestMethod]
        public void NonInitialSubstringWithoutDots_DoesNotMatch()
        {
            // collapses everything
            var match =
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
                                Visibility = Visibility.Collapsed, IsExpanded = false,
                                Namespaces = new object []
                                    {
                                        new 
                                        {
                                            DisplayName = "TestAssembly2",
                                            Visibility = Visibility.Collapsed, IsExpanded = false,
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
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
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
                };


            var action = new SearchAction("lass", model.AssemblyGroups);

            var frame = new DispatcherFrame();
            TypeNode selectedNode = null;
            action.Completed += (s, a) => { selectedNode = a.Result; frame.Continue = false; };
            action.Run();
            Dispatcher.PushFrame(frame);

            TreeAssert.IsMatch(match, model.AssemblyGroups[0]);
            Assert.IsNull(selectedNode);
        }

        [TestMethod]
        public void StringWithoutDots_DoesNotMatchNamespace()
        {
            // collapses everything
            var match =
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
                                Visibility = Visibility.Collapsed, IsExpanded = false,
                                Namespaces = new object []
                                    {
                                        new 
                                        {
                                            DisplayName = "TestAssembly2",
                                            Visibility = Visibility.Collapsed, IsExpanded = false,
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
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
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
                };


            var action = new SearchAction("TestAssembly1", model.AssemblyGroups);

            var frame = new DispatcherFrame();
            TypeNode selectedNode = null;
            action.Completed += (s, a) => { selectedNode = a.Result; frame.Continue = false; };
            action.Run();
            Dispatcher.PushFrame(frame);

            TreeAssert.IsMatch(match, model.AssemblyGroups[0]);
            Assert.IsNull(selectedNode);
        }

        [TestMethod]
        public void StringWithDots_DoesMatchNamespaceAndCollapsesNamespaceNodes()
        {
            // collapses everything
            var match =
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
                                Visibility = Visibility.Collapsed, IsExpanded = false,
                                Namespaces = new object []
                                    {
                                        new 
                                        {
                                            DisplayName = "TestAssembly2",
                                            Visibility = Visibility.Collapsed, IsExpanded = false,
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
                                                        Visibility = Visibility.Collapsed, IsExpanded = false
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
                };


            var action = new SearchAction("TestAssembly1.", model.AssemblyGroups);

            var frame = new DispatcherFrame();
            TypeNode selectedNode = null;
            action.Completed += (s, a) => { selectedNode = a.Result; frame.Continue = false; };
            action.Run();
            Dispatcher.PushFrame(frame);

            TreeAssert.IsMatch(match, model.AssemblyGroups[0]);
            Assert.IsNull(selectedNode);
        }

        //[TestMethod]
        //public void NonInitialSubstringWithDots_DoesMatch()
        //{
        //    // collapses everything
        //    var match =
        //        new
        //        {
        //            DisplayName = "test",
        //            Assemblies = new object[]
        //                {
        //                    new 
        //                    {
        //                        DisplayName = "TestAssembly1",
        //                        IsSelected = false, Visibility = Visibility.Visible, IsExpanded = true,
        //                        Namespaces = new object []
        //                            {
        //                                new 
        //                                {
        //                                    DisplayName = "TestAssembly1.Namespace1",
        //                                    IsSelected = false, Visibility = Visibility.Visible, IsExpanded = false,
        //                                    Types = new object[]
        //                                        {
        //                                            new 
        //                                            {
        //                                                DisplayName = "Class1",
        //                                                FullName = "TestAssembly1.Namespace1.Class1",
        //                                                IsSelected = false, Visibility = Visibility.Visible, IsExpanded = false
        //                                            },
        //                                            new 
        //                                            {
        //                                                DisplayName = "Class2",
        //                                                FullName = "TestAssembly1.Namespace1.Class2",
        //                                                IsSelected = false, Visibility = Visibility.Visible, IsExpanded = false
        //                                            },
        //                                            new 
        //                                            {
        //                                                DisplayName = "InternalClass1",
        //                                                FullName = "TestAssembly1.Namespace1.InternalClass1",
        //                                                IsSelected = false, Visibility = Visibility.Visible, IsExpanded = false
        //                                            },
        //                                        }
        //                                },
        //                                new 
        //                                {
        //                                    DisplayName = "TestAssembly1.Namespace2",
        //                                    IsSelected = false, Visibility = Visibility.Visible, IsExpanded = false,
        //                                    Types = new object[]
        //                                        {
        //                                            new 
        //                                            {
        //                                                DisplayName = "AnotherInternalClass",
        //                                                FullName = "TestAssembly1.Namespace2.AnotherInternalClass",
        //                                                IsSelected = false, Visibility = Visibility.Visible, IsExpanded = false
        //                                            }
        //                                        }

        //                                }
        //                            }
        //                    },
        //                    new 
        //                    {
        //                        DisplayName = "TestAssembly2",
        //                        IsSelected = false, Visibility = Visibility.Collapsed, IsExpanded = false,
        //                        Namespaces = new object []
        //                            {
        //                                new 
        //                                {
        //                                    DisplayName = "TestAssembly2",
        //                                    IsSelected = false, Visibility = Visibility.Collapsed, IsExpanded = false,
        //                                    Types = new object[]
        //                                        {
        //                                            new 
        //                                            {
        //                                                DisplayName = "Class1",
        //                                                FullName = "TestAssembly2.Class1",
        //                                                IsSelected = false, Visibility = Visibility.Collapsed, IsExpanded = false
        //                                            },
        //                                            new 
        //                                            {
        //                                                DisplayName = "NamespaceClass",
        //                                                FullName = "TestAssembly2.NamespaceClass",
        //                                                IsSelected = false, Visibility = Visibility.Collapsed, IsExpanded = false
        //                                            }

        //                                        }
        //                                },
        //                                new 
        //                                {
        //                                    DisplayName = "TestAssembly2.Namespace1",
        //                                    IsSelected = false, Visibility = Visibility.Collapsed, IsExpanded = false,
        //                                    Types = new object[]
        //                                        {
        //                                            new 
        //                                            {
        //                                                DisplayName = "Class1",
        //                                                FullName = "TestAssembly2.Namespace1.Class1",
        //                                                IsSelected = false, Visibility = Visibility.Collapsed, IsExpanded = false
        //                                            }
        //                                        }

        //                                }
        //                            }
        //                    }
        //                }
        //        };


        //    var action = new SearchAction("Assembly1.", model.AssemblyGroups);

        //    var frame = new DispatcherFrame();
        //    action.Completed += (s, a) => { frame.Continue = false; };
        //    action.Run();
        //    Dispatcher.PushFrame(frame);

        //    TreeAssert.IsMatch(match, model.AssemblyGroups[0]);
        //}

        [TestMethod]
        public void StringWithDots_DoesMatchNamespaceAndDoesNotCollapseNamespacesIfNamespaceDoesNotStartWithString()
        {
            // collapses everything
            var match =
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
                };


            var action = new SearchAction("TestAssembly2.Namespace", model.AssemblyGroups);

            var frame = new DispatcherFrame();
            TypeNode selectedNode = null;
            action.Completed += (s, a) => { selectedNode = a.Result; frame.Continue = false; };
            action.Run();
            Dispatcher.PushFrame(frame);

            TreeAssert.IsMatch(match, model.AssemblyGroups[0]);
            Assert.IsNull(selectedNode);
        }

        [TestMethod]
        public void StringWithDots_DoesMatchNamespaceAndDoesNotCollapseNamespacesIfNamespaceDoesNotMatchString()
        {
            // collapses everything
            var match =
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
                };


            var action = new SearchAction("Assembly2.Namespace", model.AssemblyGroups);

            var frame = new DispatcherFrame();
            TypeNode selectedNode = null;
            action.Completed += (s, a) => { selectedNode = a.Result; frame.Continue = false; };
            action.Run();
            Dispatcher.PushFrame(frame);

            TreeAssert.IsMatch(match, model.AssemblyGroups[0]);
            Assert.IsNull(selectedNode);
        }

        [TestMethod]
        public void FirstFullNameMatch_IsSelected()
        {
            var match =
                new
                {
                    DisplayName = "test",
                    Assemblies = new object[]
                        {
                            new 
                            {
                                DisplayName = "TestAssembly1",
                                Visibility = Visibility.Collapsed, IsExpanded = false,
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
                                            Visibility = Visibility.Collapsed, IsExpanded = false,
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
                };


            var action = new SearchAction("TestAssembly2.Namespace1.Class1", model.AssemblyGroups);

            var frame = new DispatcherFrame();
            TypeNode selectedNode = null;
            action.Completed += (s, a) => { selectedNode = a.Result; frame.Continue = false; };
            action.Run();
            Dispatcher.PushFrame(frame);

            TreeAssert.IsMatch(match, model.AssemblyGroups[0]);
            Assert.IsNotNull(selectedNode);
            Assert.AreEqual("TestAssembly2.Namespace1.Class1", selectedNode.FullName);
        }
    }
}
