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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;

namespace Console.Wpf.Tests.VSTS.WizardTests.given_wizard
{
    public class TestableWizard : WizardModel
    {
        public TestableWizard(IUIServiceWpf uiService, IEnumerable<WizardStep> steps)
            : base(uiService)
        {
            foreach (var step in steps)
            {
                AddStep(step);
            }

            // default execute action is to call base
            ExecuteAction = () => { BaseExecute(); };
        }


        private void BaseExecute()
        {
            base.Execute();
        }

        public Action ExecuteAction { get; set; }

        protected override string GetWizardTitle()
        {
            return "TestTitle";
        }

        public void DoFinish()
        {
            base.Finish();
        }

        protected override void Execute()
        {
            if (ExecuteAction != null)
            {
                ExecuteAction();
            }
        }
    }

    public class TestableWizardStep : WizardStep
    {
        private bool isValid;

        public void SetIsValid(bool isValid)
        {
            this.isValid = isValid;
            OnPropertyChanged("IsValid");
        }

        public override bool IsValid
        {
            get
            {
                return isValid;
            }
        }

        public override string Title
        {
            get { return "TestableStep"; }
        }

        public override void Execute()
        {
            ExecuteCalled = true;
        }

        public bool ExecuteCalled { get; set; }
    }
}
