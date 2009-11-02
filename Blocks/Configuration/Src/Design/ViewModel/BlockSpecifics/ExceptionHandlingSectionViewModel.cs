using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Console.Wpf.ViewModel.BlockSpecifics
{
    public class ExceptionHandlingSectionViewModel : PositionedSectionViewModel
    {
        public ExceptionHandlingSectionViewModel(IUnityContainer builder, string sectionName, ConfigurationSection section)
            : base(builder, sectionName, section)
        {
            var policies = Positioning.PositionCollection("Exception Policies", 
                typeof(NamedElementCollection<ExceptionPolicyData>), 
                typeof(ExceptionPolicyData), 
                new PositioningInstructions { FixedColumn = 0, FixedRow = 0 });

            var exceptionTypes = policies.PositionNestedCollection(typeof(ExceptionTypeData));
            exceptionTypes.PositionNestedCollection(typeof(ExceptionHandlerData));

            Positioning.PositionHeader("Exception Types", new PositioningInstructions { FixedRow = 0, FixedColumn = 1 });
            Positioning.PositionHeader("Handlers", new PositioningInstructions { FixedRow = 0, FixedColumn = 2 });

            
        }
    }
}
