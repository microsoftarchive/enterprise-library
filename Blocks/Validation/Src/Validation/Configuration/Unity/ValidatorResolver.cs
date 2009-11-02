using System;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Unity
{
    /// <summary>
    /// A <see cref="IDependencyResolverPolicy"/> object that will resolve a
    /// <see cref="Validator{T}"/>.
    /// </summary>
    public class ValidatorResolver : IDependencyResolverPolicy
    {
        private readonly string ruleSet;
        private readonly ValidationSpecificationSource validationSource;
        private readonly Type validatorType;

        /// <summary>
        /// Create a new instance of <see cref="ValidatorResolver"/> with the given
        /// parameters.
        /// </summary>
        /// <param name="ruleSet">Rule set name, or null to use default.</param>
        /// <param name="validationSource"><see cref="ValidationSpecificationSource"/> to use when resolving.</param>
        /// <param name="validatorType">Type of validator to resolve.</param>
        public ValidatorResolver(string ruleSet, ValidationSpecificationSource validationSource, Type validatorType)
        {
            this.ruleSet = ruleSet;
            this.validationSource = validationSource;
            this.validatorType = validatorType;
        }

        #region IDependencyResolverPolicy Members

        /// <summary>
        /// Get the value for a dependency.
        /// </summary>
        /// <param name="context">Current build context.</param>
        /// <returns>
        /// The value for the dependency.
        /// </returns>
        public object Resolve(IBuilderContext context)
        {
            return context.NewBuildUp(new NamedTypeBuildKey(validatorType, ruleSet),
                policies => policies.Set(
                    new ValidationSpecificationSourcePolicy(validationSource),
                    new NamedTypeBuildKey(validatorType, ruleSet)));
        }

        #endregion
    }
}