using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.ViewModel.Commands;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class AddExceptionHandlingBlockCommand : AddApplicationBlockCommand
    {
        public AddExceptionHandlingBlockCommand(ConfigurationSourceModel configurationSourceModel, AddApplicationBlockCommandAttribute attribute)
            : base(configurationSourceModel, attribute)
        {

        }
        protected override System.Configuration.ConfigurationSection CreateConfigurationSection()
        {
            return new ExceptionHandlingSettings
            {
                ExceptionPolicies = 
                {{
                     new ExceptionPolicyData
                     {
                         Name = "Policy",
                         ExceptionTypes = 
                         {{
                              new ExceptionTypeData
                              {
                                  Name = "All Exceptions",
                                  Type = typeof(Exception)
                              }
                         }}
                     }
                 }}
            };
        }
    }
}
