using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    public class AssemblyGroup
    {
        private string name;
        private IEnumerable<Assembly> assemblies;

        public AssemblyGroup(string name, IEnumerable<Assembly> assemblies)
        {
            this.name = name;
            this.assemblies = assemblies;
        }

        public string Name
        {
            get { return this.name; }
        }

        public IEnumerable<Assembly> Assemblies
        {
            get { return this.assemblies; }
        }
    }
}
