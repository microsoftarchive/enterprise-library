using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Console.Wpf
{
    public class ConfigurationElementType
    {
        Type elementType;
        public ConfigurationElementType(Type elementType)
        {
            this.elementType = elementType;
        }

        public Type ElementType
        {
            get { return elementType; }
        }
    }
}
