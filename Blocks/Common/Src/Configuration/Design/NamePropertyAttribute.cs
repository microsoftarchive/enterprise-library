using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public class NamePropertyAttribute : Attribute
    {
        private readonly string propertyName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        public NamePropertyAttribute(string propertyName)
        {
            this.propertyName =  propertyName;
            this.NamePropertyDisplayFormat = "{0}";
            this.Order = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public string PropertyName
        {
            get { return propertyName; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string NamePropertyDisplayFormat { get; set; }
    }
}
