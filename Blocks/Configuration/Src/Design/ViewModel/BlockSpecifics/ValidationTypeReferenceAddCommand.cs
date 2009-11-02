using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class ValidationTypeReferenceAddCommand : TypePickingCollectionElementAddCommand
    {
        public ValidationTypeReferenceAddCommand(TypePickingCommandAttribute commandAttribute, ConfigurationElementType configurationElementType, ElementCollectionViewModel elementCollectionModel)
            : base(commandAttribute, configurationElementType, elementCollectionModel)
        {
        }

        protected override void SetProperties(ElementViewModel createdElement, Type selectedType)
        {
            createdElement.Property("Name").Value = selectedType.FullName; 
            createdElement.Property("AssemblyName").Value = selectedType.Assembly.GetName().FullName;
        }
    }
}
