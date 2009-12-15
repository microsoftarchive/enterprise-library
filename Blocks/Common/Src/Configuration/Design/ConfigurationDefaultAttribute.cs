using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design
{
    /// <summary/>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple=true)]
    public class DesigntimeDefaultAttribute : Attribute
    {
        readonly string defaultValue;

        /// <summary/>
        public DesigntimeDefaultAttribute(string defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        /// <summary/>
        public string DefaultValue
        {
            get { return defaultValue; }
        }
    }
}
