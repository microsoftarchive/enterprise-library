using System;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Represents a constraint on a <see cref="TypeBuildNode"/>.
    /// </summary>
    public class TypeBuildNodeConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBuildNodeConstraint"/> class.
        /// </summary>
        /// <param name="baseType">The base type (class or interface) from which the constrained type should derive.</param>
        /// <param name="configurationType">The base type from which a type specified by the 
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ConfigurationElementTypeAttribute"/>
        /// bound to the constrained type should derive, or <see langword="null"/> if no such constraint is necessary.
        /// </param>
        /// <param name="typeSelectorIncludes">Additional constraints.</param>
        public TypeBuildNodeConstraint(
            Type baseType,
            Type configurationType,
            TypeSelectorIncludes typeSelectorIncludes)
        {
            this.BaseType = baseType;
            this.ConfigurationType = configurationType;
            this.TypeSelectorIncludes = typeSelectorIncludes;
        }

        /// <summary>
        /// Gets the base type (class or interface) from which the constrained type should derive.
        /// </summary>
        public Type BaseType { get; internal set; }

        /// <summary>
        /// Gets the base type from which a type specified by the 
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ConfigurationElementTypeAttribute"/> 
        /// bound to the constrained type should derive, or <see langword="null"/> if no such constraint is necessary.
        /// </summary>
        public Type ConfigurationType { get; internal set; }

        /// <summary>
        /// Gets the additional constraints.
        /// </summary>
        public TypeSelectorIncludes TypeSelectorIncludes { get; internal set; }
    }
}
