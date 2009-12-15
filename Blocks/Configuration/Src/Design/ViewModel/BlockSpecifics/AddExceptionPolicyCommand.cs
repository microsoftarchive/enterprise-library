using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
    public class AddExceptionPolicyCommand : DefaultCollectionElementAddCommand
    {
        public AddExceptionPolicyCommand(ElementCollectionViewModel collection)
            :base(new ConfigurationElementType(typeof(ExceptionPolicyData)), collection)
        {

        }

        public override void Execute(object parameter)
        {
            base.Execute(parameter);

            if (AddedElementViewModel != null)
            {
                var exceptionTypesElement = (ElementCollectionViewModel)AddedElementViewModel.ChildElement("ExceptionTypes");
                var addedExceptionType = exceptionTypesElement.AddNewCollectionElement(typeof(ExceptionTypeData));
                
                addedExceptionType.Property("Name").Value = "All Exceptions";
                addedExceptionType.Property("TypeName").Value = typeof(Exception).AssemblyQualifiedName;
            }
        }
    }
}
