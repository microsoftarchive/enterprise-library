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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests
{
    public class given_profile_with_type_filters : ArrangeActAssert
    {
        [TestClass]
        public class when_all_objects_are_allowed : given_profile_with_type_filters
        {
            private bool match;

            protected override void Act()
            {
                match = ProfileTestObjects.ProfileWithAllObjectTypeAreAllowed.Check(typeof(object));
            }

            [TestMethod]
            public void then_check_type_is_true()
            {
                Assert.IsTrue(match);
            }
        }

        [TestClass]
        public class when_object_type_is_allowed_but_derived_type_is_denied : given_profile_with_type_filters
        {
            private bool matchObject;
            private bool matchString;

            protected override void Act()
            {
                var profile = ProfileTestObjects.ProfileWithObjectIsAllowedButStringIsDenied;
                matchObject = profile.Check(typeof(object));
                matchString = profile.Check(typeof(string));
            }

            [TestMethod]
            public void then_check_type_is_true_for_object_type()
            {
                Assert.IsTrue(matchObject);
            }

            [TestMethod]
            public void then_check_type_is_false_for_derived_type()
            {
                Assert.IsFalse(matchString);
            }
        }

        [TestClass]
        public class when_object_type_is_denied_but_derived_type_is_allowed : given_profile_with_type_filters
        {
            private bool matchObject;
            private bool matchString;

            protected override void Act()
            {
                var profile = ProfileTestObjects.ProfileWithObjectIsDeniedButStringIsAllowed;
                matchObject = profile.Check(typeof(object));
                matchString = profile.Check(typeof(string));
            }

            [TestMethod]
            public void then_check_type_is_false_for_object_type()
            {
                Assert.IsFalse(matchObject);
            }

            [TestMethod]
            public void then_check_type_is_true_for_derived_type()
            {
                Assert.IsTrue(matchString);
            }
        }

        private static class ProfileTestObjects
        {
            static public Profile ProfileWithAllObjectTypeAreAllowed
            {
                get
                {
                    return new Profile()
                    {
                        MatchFilters = new MatchFilter[]
                                                 {
                                                     new TypeMatchFilter()
                                                         {
                                                             Name = typeof (object).ToString(),
                                                             MatchKind = MatchKind.Allow
                                                         }
                                                 }
                    };
                }
            }

            static public Profile ProfileWithObjectIsDeniedButStringIsAllowed
            {
                get
                {
                    return new Profile()
                    {
                        MatchFilters = new MatchFilter[]
                                                 {
                                                     new TypeMatchFilter()
                                                         {
                                                             Name = typeof (object).ToString(),
                                                             MatchKind = MatchKind.Deny
                                                         },
                                                    new TypeMatchFilter()
                                                    {
                                                        Name = typeof (string).ToString(),
                                                        MatchKind = MatchKind.Allow
                                                    }
                                                 }
                    };
                }
            }
            static public Profile ProfileWithObjectIsAllowedButStringIsDenied
            {
                get
                {
                    return new Profile()
                    {
                        MatchFilters = new MatchFilter[]
                                                 {
                                                     new TypeMatchFilter()
                                                         {
                                                             Name = typeof (object).ToString(),
                                                             MatchKind = MatchKind.Allow
                                                         },
                                                    new TypeMatchFilter()
                                                    {
                                                        Name = typeof (string).ToString(),
                                                        MatchKind = MatchKind.Deny
                                                    }
                                                 }
                    };
                }
            }
        }
    }    
    
    public class given_profile_with_assembly_filters : ArrangeActAssert
    {
        [TestClass]
        public class when_assembly_is_allowed : given_profile_with_assembly_filters
        {
            private bool matchSystem;

            protected override void Act()
            {
                var profile = ProfileTestObjects.ProfileWithSystemIsAllowed;
                matchSystem = profile.Check(typeof(object));
            }

            [TestMethod]
            public void then_check_type_is_true()
            {
                Assert.IsTrue(matchSystem);
            }
        }

        [TestClass]
        public class when_assembly_is_denied : given_profile_with_assembly_filters
        {
            private bool matchSystem;

            protected override void Act()
            {
                var profile = ProfileTestObjects.ProfileWithSystemIsDenied;
                matchSystem = profile.Check(typeof(object));
            }

            [TestMethod]
            public void then_check_type_is_false()
            {
                Assert.IsFalse(matchSystem);
            }
        }

        private static class ProfileTestObjects
        {
            static public Profile ProfileWithSystemIsAllowed
            {
                get
                {
                    return new Profile()
                    {
                        MatchFilters = new MatchFilter[]
                                                 {
                                                     new AssemblyMatchFilter()
                                                         {
                                                             Name = "mscorlib",
                                                             MatchKind = MatchKind.Allow
                                                         }
                                                 }
                    };
                }
            }

            static public Profile ProfileWithSystemIsDenied
            {
                get
                {
                    return new Profile()
                    {
                        MatchFilters = new MatchFilter[]
                                                 {
                                                     new AssemblyMatchFilter()
                                                         {
                                                             Name = "mscorlib",
                                                             MatchKind = MatchKind.Deny
                                                         }
                                                 }
                    };
                }
            }
        }
    }

    public class given_profile_with_both_assembly_and_type_filters : ArrangeActAssert
    {
        [TestClass]
        public class when_assembly_is_allowed_but_type_inside_it_is_denied : given_profile_with_both_assembly_and_type_filters
        {
            private bool matchObjectType;

            protected override void Act()
            {
                var profile = ProfileTestObjects.ProfileWithSystemIsAllowedButObjectTypeIsDenied;
                matchObjectType = profile.Check(typeof (object));
            }

            [TestMethod]
            public void then_check_type_is_false_for_object_type()
            {
                Assert.IsTrue(matchObjectType);
            }
        }

        [TestClass]
        public class when_assembly_is_denied_but_type_inside_it_is_allowed : given_profile_with_both_assembly_and_type_filters
        {
            private bool matchObjectType;

            protected override void Act()
            {
                var profile = ProfileTestObjects.ProfileWithSystemIsDeniedButObjectTypeIsAllowed;
                matchObjectType = profile.Check(typeof(object));
            }

            [TestMethod]
            public void then_check_type_is_true_for_object_type()
            {
                Assert.IsTrue(matchObjectType);
            }
        }

        private static class ProfileTestObjects
        {
            static public Profile ProfileWithSystemIsAllowedButObjectTypeIsDenied
            {
                get
                {
                    return new Profile()
                    {
                        MatchFilters = new MatchFilter[]
                                                 {
                                                     new AssemblyMatchFilter()
                                                         {
                                                             Name = "System",
                                                             MatchKind = MatchKind.Allow
                                                         },
                                                          new TypeMatchFilter()
                                                         {
                                                             Name = typeof(object).Name,
                                                             MatchKind = MatchKind.Deny
                                                         }
                                                 }
                    };
                }
            }

            static public Profile ProfileWithSystemIsDeniedButObjectTypeIsAllowed
            {
                get
                {
                    return new Profile()
                    {
                        MatchFilters = new MatchFilter[]
                                                 {
                                                     new AssemblyMatchFilter()
                                                         {
                                                             Name = "System",
                                                             MatchKind = MatchKind.Deny
                                                         },
                                                          new TypeMatchFilter()
                                                         {
                                                             Name = typeof(object).Name,
                                                             MatchKind = MatchKind.Allow
                                                         }
                                                 }
                    };
                }
            }
        }
    }
}
