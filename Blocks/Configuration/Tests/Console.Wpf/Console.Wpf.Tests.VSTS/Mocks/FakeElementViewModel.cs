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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Console;
using Moq;

namespace Console.Wpf.Tests.VSTS.Mocks
{
    public class FakeElementViewModel : ElementViewModel
    {
        public FakeElementViewModel(IEnumerable<Attribute> attributes)
            : base(null, new TestHandlerData(), attributes)
        {
            base.ElementViewModelServiceDependencies(null, ApplicationModel.Object);
        }

        public FakeElementViewModel()
            : this(new Attribute[0])
        {
        }

        public Mock<IApplicationModel> ApplicationModel = new Mock<IApplicationModel>(MockBehavior.Loose);

        protected override IEnumerable<CommandModel> GetAllCommands()
        {
            return Enumerable.Empty<CommandModel>();
        }
    }
}
