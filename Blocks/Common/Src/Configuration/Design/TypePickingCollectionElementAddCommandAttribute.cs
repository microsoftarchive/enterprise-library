using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TypePickingCommandAttribute : CommandAttribute
    {
        private string property;

        /// <summary>
        /// 
        /// </summary>
        public TypePickingCommandAttribute()
            :this("TypeName")
        {

        }

        ///<summary>
        /// Instantiates a new <see cref="TypePickingCommandAttribute"/>
        ///</summary>
        public TypePickingCommandAttribute(string property)
            :base(CommonDesignTime.CommandTypeNames.AddProviderUsingTypePickerCommand)
        {
            this.property = property;
            CommandPlacement = CommandPlacement.ContextAdd;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Property
        {
            get { return property; }
        }

    }
}
