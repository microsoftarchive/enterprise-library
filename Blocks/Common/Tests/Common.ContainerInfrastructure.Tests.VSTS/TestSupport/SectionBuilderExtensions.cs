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

using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;

namespace Common.ContainerInfrastructure.Tests.VSTS.TestSupport
{
    public static class SectionBuilderExtensions
    {
        public static ExceptionSectionBuilder ExceptionSection(this SectionBuilder builder)
        {
            return new ExceptionSectionBuilder();
        }

        public static ConnectionStringsSectionBuilder ConnectionStringsSection(this SectionBuilder builder)
        {
            return new ConnectionStringsSectionBuilder();
        }

        public static PiabSectionBuilder PiabSection(this SectionBuilder builder)
        {
            return new PiabSectionBuilder();
        }
    }
}
