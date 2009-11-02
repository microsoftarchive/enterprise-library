using System;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Unity
{
    /// <summary>
    /// An <see cref="InjectionParameterValue"/> class that can be passed
    /// as the value for a dependency in the Unity RegisterType API.
    /// </summary>
    public class ValidatorParameter : InjectionParameterValue
    {
        private readonly string ruleSet;
        private readonly ValidationSpecificationSource validationSource;
        private readonly Type validatorType;

        /// <summary>
        /// Creates the <see cref="ValidatorParameter"/> object, which resolves
        /// the default ruleset and all sources.
        /// </summary>
        /// <param name="typeToValidate">Type to construct a validator for.</param>
        public ValidatorParameter(Type typeToValidate)
            : this(typeToValidate, null, ValidationSpecificationSource.All)
        {
        }

        /// <summary>
        /// Creates the <see cref="ValidatorParameter"/> object which resolves
        /// using the given <paramref name="ruleSet"/> and all sources.
        /// </summary>
        /// <param name="typeToValidate">Type to construct a validator for.</param>
        /// <param name="ruleSet">Rule set to use.</param>
        public ValidatorParameter(Type typeToValidate, string ruleSet)
            : this(typeToValidate, ruleSet, ValidationSpecificationSource.All)
        {
        }

        /// <summary>
        /// Creates the <see cref="ValidatorParameter"/> object which resolves
        /// using the default rule set and the given <paramref name="validationSource"/>.
        /// </summary>
        /// <param name="typeToValidate">Type to construct a validator for.</param>
        /// <param name="validationSource"><see cref="ValidationSpecificationSource"/> controlling which
        /// set of validator to retrieve.</param>
        public ValidatorParameter(Type typeToValidate, ValidationSpecificationSource validationSource)
            : this(typeToValidate, null, validationSource)
        {
        }

        /// <summary>
        /// Resolve a validator with the given ruleset and validation source.
        /// </summary>
        /// <param name="typeToValidate">Type to construct a validator for.</param>
        /// <param name="ruleSet">Ruleset to use, or null for the default.</param>
        /// <param name="validationSource"><see cref="ValidationSpecificationSource"/> controlling which
        /// set of validator to retrieve.</param>
        public ValidatorParameter(Type typeToValidate, string ruleSet, ValidationSpecificationSource validationSource)
        {
            validatorType = typeof (Validator<>).MakeGenericType(typeToValidate);
            this.ruleSet = ruleSet;
            this.validationSource = validationSource;
        }

        /// <summary>
        /// Name for the type represented by this <see cref="T:Microsoft.Practices.Unity.InjectionParameterValue"/>.
        ///             This may be an actual type name or a generic argument name.
        /// </summary>
        public override string ParameterTypeName
        {
            get { return validatorType.Name; }
        }

        /// <summary>
        /// Test to see if this parameter value has a matching type for the given type.
        /// </summary>
        /// <param name="t">Type to check.</param>
        /// <returns>
        /// True if this parameter value is compatible with type <paramref name="t"/>,
        ///             false if not.
        /// </returns>
        public override bool MatchesType(Type t)
        {
            return t == validatorType;
        }

        /// <summary>
        /// Return a <see cref="T:Microsoft.Practices.ObjectBuilder2.IDependencyResolverPolicy"/> instance that will
        ///             return this types value for the parameter.
        /// </summary>
        /// <param name="typeToBuild">Type that contains the member that needs this parameter. Used
        ///             to resolve open generic parameters.</param>
        /// <returns>
        /// The <see cref="T:Microsoft.Practices.ObjectBuilder2.IDependencyResolverPolicy"/>.
        /// </returns>
        public override IDependencyResolverPolicy GetResolverPolicy(Type typeToBuild)
        {
            return new ValidatorResolver(ruleSet, validationSource, validatorType);
        }
    }

    /// <summary>
    /// A version of <see cref="ValidatorParameter"/> that lets you specify the type to
    /// validate using generic syntax.
    /// </summary>
    /// <typeparam name="T">Type to construct a validator for.</typeparam>
    public class ValidatorParameter<T> : ValidatorParameter
    {
        /// <summary>
        /// Creates the <see cref="ValidatorParameter"/> object, which resolves
        /// the default ruleset and all sources.
        /// </summary>
        public ValidatorParameter()
            : base(typeof(T), null, ValidationSpecificationSource.All)
        {
        }

        /// <summary>
        /// Creates the <see cref="ValidatorParameter"/> object which resolves
        /// using the given <paramref name="ruleSet"/> and all sources.
        /// </summary>
        /// <param name="ruleSet">Rule set to use.</param>
        public ValidatorParameter(string ruleSet)
            : base(typeof(T), ruleSet, ValidationSpecificationSource.All)
        {
        }

        /// <summary>
        /// Creates the <see cref="ValidatorParameter"/> object which resolves
        /// using the default rule set and the given <paramref name="validationSource"/>.
        /// </summary>
        /// <param name="validationSource"><see cref="ValidationSpecificationSource"/> controlling which
        /// set of validator to retrieve.</param>
        public ValidatorParameter(ValidationSpecificationSource validationSource)
            : base(typeof(T), null, validationSource)
        {
        }

        /// <summary>
        /// Resolve a validator with the given ruleset and validation source.
        /// </summary>
        /// <param name="ruleSet">Ruleset to use, or null for the default.</param>
        /// <param name="validationSource"><see cref="ValidationSpecificationSource"/> controlling which
        /// set of validator to retrieve.</param>
        public ValidatorParameter(string ruleSet, ValidationSpecificationSource validationSource)
            : base(typeof(T), ruleSet, validationSource)
        {
        }
    }
}