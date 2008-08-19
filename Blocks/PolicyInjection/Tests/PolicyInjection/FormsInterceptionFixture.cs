//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.RemotingInterception;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests
{
    [TestClass]
    public class FormsInterceptionFixture
    {
        [TestMethod]
        public void CanInterceptOnFormsClass()
        {
            CallCountHandler countHandler = new CallCountHandler();
            RemotingPolicyInjector factory = new RemotingPolicyInjector(GetPolicySet(countHandler));

            IMyForm magicForm = factory.Create<MyForm, IMyForm>();
        }

        PolicySet GetPolicySet(ICallHandler handler)
        {
            RuleDrivenPolicy magicPolicy = new RuleDrivenPolicy();
            magicPolicy.RuleSet.Add(new TagAttributeMatchingRule("Magic"));
            magicPolicy.Handlers.Add(handler);
            return new PolicySet(magicPolicy);
        }

        public interface IMyForm
        {
            void Magic();
        }

        public class MyForm : Form, IMyForm
        {
            FlowLayoutPanel flowPanel;

            public MyForm()
            {
                flowPanel = new FlowLayoutPanel();
                Controls.Add(flowPanel);
            }

            [Tag("Magic")]
            public void Magic()
            {
                Label label = new Label();
                label.Text = "Magic";
                flowPanel.Controls.Add(label);
            }
        }
    }
}