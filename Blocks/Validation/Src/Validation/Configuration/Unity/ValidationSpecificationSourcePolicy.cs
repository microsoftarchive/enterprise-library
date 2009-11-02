using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Unity
{
    ///<summary>
    /// A small objectbuilder policy used to pass the requested
    /// <see cref="ValidationSpecificationSource"/> along to the
    /// <see cref="ValidatorCreationStrategy"/>.
    ///</summary>
    public class ValidationSpecificationSourcePolicy : IBuilderPolicy
    {
        /// <summary>
        /// Create a new instance of <see cref="ValidationSpecificationSourcePolicy"/>
        /// with the <see cref="Source"/> property initialized to All.
        /// </summary>
        public ValidationSpecificationSourcePolicy()
        {
            Source = ValidationSpecificationSource.All;    
        }

        /// <summary>
        /// Create a new instance of <see cref="ValidationSpecificationSourcePolicy"/>
        /// with the <see cref="Source"/> property initialized to the given value.
        /// </summary>
        /// <param name="source">The desired <see cref="ValidationSpecificationSource"/>.</param>
        public ValidationSpecificationSourcePolicy(ValidationSpecificationSource source)
        {
            Source = source;
        }

        ///<summary>
        /// The desired <see cref="ValidationSpecificationSource"/>.
        ///</summary>
        public ValidationSpecificationSource Source { get; private set; }
    }
}
