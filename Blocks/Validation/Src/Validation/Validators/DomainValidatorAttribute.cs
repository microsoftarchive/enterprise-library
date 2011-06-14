//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
	/// <summary>
	/// Represents a <see cref="DomainValidatorAttribute"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property
		| AttributeTargets.Field
		| AttributeTargets.Method
		| AttributeTargets.Parameter,
		AllowMultiple = true,
		Inherited = false)]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019",
		Justification = "Fields are used internally")]
	public sealed class DomainValidatorAttribute : ValueValidatorAttribute
	{
		private object[] domain;

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
		/// </summary>
		public DomainValidatorAttribute()
			: this(new object[0])
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
		/// </summary>
		/// <param name="domain">List of values to be used in the validation.</param>
        public DomainValidatorAttribute(params object[] domain)
			: base()
		{
			ValidatorArgumentsValidatorHelper.ValidateDomainValidator(domain);

			this.domain = domain;
		}

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
        /// </summary>
        /// <param name="domain1">Value to be used in the validation.</param>
        public DomainValidatorAttribute(object domain1) 
            : this( new [] { domain1 } ) {}

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
        /// </summary>
        /// <param name="domain1">Value to be used in the validation.</param>
        /// <param name="domain2">Value to be used in the validation.</param>
        public DomainValidatorAttribute(object domain1, object domain2) 
            : this( new [] { domain1, domain2 } ) {}
        
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
        /// </summary>
        /// <param name="domain1">Value to be used in the validation.</param>
        /// <param name="domain2">Value to be used in the validation.</param>
        /// <param name="domain3">Value to be used in the validation.</param>
        public DomainValidatorAttribute(object domain1, object domain2, object domain3) 
            : this( new [] { domain1, domain2, domain3 } ) {}
        
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
        /// </summary>
        /// <param name="domain1">Value to be used in the validation.</param>
        /// <param name="domain2">Value to be used in the validation.</param>
        /// <param name="domain3">Value to be used in the validation.</param>
        /// <param name="domain4">Value to be used in the validation.</param>
        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4)
            : this( new [] { domain1, domain2, domain3, domain4 } ) {}
        
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
        /// </summary>
        /// <param name="domain1">Value to be used in the validation.</param>
        /// <param name="domain2">Value to be used in the validation.</param>
        /// <param name="domain3">Value to be used in the validation.</param>
        /// <param name="domain4">Value to be used in the validation.</param>
        /// <param name="domain5">Value to be used in the validation.</param>
        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5)
            : this( new [] { domain1, domain2, domain3, domain4, domain5 } ) {}
        
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
        /// </summary>
        /// <param name="domain1">Value to be used in the validation.</param>
        /// <param name="domain2">Value to be used in the validation.</param>
        /// <param name="domain3">Value to be used in the validation.</param>
        /// <param name="domain4">Value to be used in the validation.</param>
        /// <param name="domain5">Value to be used in the validation.</param>
        /// <param name="domain6">Value to be used in the validation.</param>
        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5, object domain6)
            : this( new [] { domain1, domain2, domain3, domain4, domain5, domain6 } ) {}
        
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
        /// </summary>
        /// <param name="domain1">Value to be used in the validation.</param>
        /// <param name="domain2">Value to be used in the validation.</param>
        /// <param name="domain3">Value to be used in the validation.</param>
        /// <param name="domain4">Value to be used in the validation.</param>
        /// <param name="domain5">Value to be used in the validation.</param>
        /// <param name="domain6">Value to be used in the validation.</param>
        /// <param name="domain7">Value to be used in the validation.</param>
        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5, object domain6, object domain7)
            : this( new [] { domain1, domain2, domain3, domain4, domain5, domain6, domain7 } ) {}
        
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
        /// </summary>
        /// <param name="domain1">Value to be used in the validation.</param>
        /// <param name="domain2">Value to be used in the validation.</param>
        /// <param name="domain3">Value to be used in the validation.</param>
        /// <param name="domain4">Value to be used in the validation.</param>
        /// <param name="domain5">Value to be used in the validation.</param>
        /// <param name="domain6">Value to be used in the validation.</param>
        /// <param name="domain7">Value to be used in the validation.</param>
        /// <param name="domain8">Value to be used in the validation.</param>
        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5, object domain6, object domain7, object domain8)
            : this( new [] { domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8 } ) {}
        
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
        /// </summary>
        /// <param name="domain1">Value to be used in the validation.</param>
        /// <param name="domain2">Value to be used in the validation.</param>
        /// <param name="domain3">Value to be used in the validation.</param>
        /// <param name="domain4">Value to be used in the validation.</param>
        /// <param name="domain5">Value to be used in the validation.</param>
        /// <param name="domain6">Value to be used in the validation.</param>
        /// <param name="domain7">Value to be used in the validation.</param>
        /// <param name="domain8">Value to be used in the validation.</param>
        /// <param name="domain9">Value to be used in the validation.</param>
        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5, object domain6, object domain7, object domain8, object domain9)
            : this(new[] { domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8, domain9 }) { }
        
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
        /// </summary>
        /// <param name="domain1">Value to be used in the validation.</param>
        /// <param name="domain2">Value to be used in the validation.</param>
        /// <param name="domain3">Value to be used in the validation.</param>
        /// <param name="domain4">Value to be used in the validation.</param>
        /// <param name="domain5">Value to be used in the validation.</param>
        /// <param name="domain6">Value to be used in the validation.</param>
        /// <param name="domain7">Value to be used in the validation.</param>
        /// <param name="domain8">Value to be used in the validation.</param>
        /// <param name="domain9">Value to be used in the validation.</param>
        /// <param name="domain10">Value to be used in the validation.</param>
        /// <param name="domain11">Value to be used in the validation.</param>
        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5, object domain6, object domain7, object domain8, object domain9, object domain10, object domain11)
            : this( new [] { domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8, domain9, domain10, domain11 } ) {}

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
        /// </summary>
        /// <param name="domain1">Value to be used in the validation.</param>
        /// <param name="domain2">Value to be used in the validation.</param>
        /// <param name="domain3">Value to be used in the validation.</param>
        /// <param name="domain4">Value to be used in the validation.</param>
        /// <param name="domain5">Value to be used in the validation.</param>
        /// <param name="domain6">Value to be used in the validation.</param>
        /// <param name="domain7">Value to be used in the validation.</param>
        /// <param name="domain8">Value to be used in the validation.</param>
        /// <param name="domain9">Value to be used in the validation.</param>
        /// <param name="domain10">Value to be used in the validation.</param>
        /// <param name="domain11">Value to be used in the validation.</param>
        /// <param name="domain12">Value to be used in the validation.</param>
        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5, object domain6, object domain7, object domain8, object domain9, object domain10, object domain11, object domain12)
            : this(new[] { domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8, domain9, domain10, domain11, domain12 }) { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
        /// </summary>
        /// <param name="domain1">Value to be used in the validation.</param>
        /// <param name="domain2">Value to be used in the validation.</param>
        /// <param name="domain3">Value to be used in the validation.</param>
        /// <param name="domain4">Value to be used in the validation.</param>
        /// <param name="domain5">Value to be used in the validation.</param>
        /// <param name="domain6">Value to be used in the validation.</param>
        /// <param name="domain7">Value to be used in the validation.</param>
        /// <param name="domain8">Value to be used in the validation.</param>
        /// <param name="domain9">Value to be used in the validation.</param>
        /// <param name="domain10">Value to be used in the validation.</param>
        /// <param name="domain11">Value to be used in the validation.</param>
        /// <param name="domain12">Value to be used in the validation.</param>
        /// <param name="domain13">Value to be used in the validation.</param>
        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5, object domain6, object domain7, object domain8, object domain9, object domain10, object domain11, object domain12, object domain13)
            : this(new[] { domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8, domain9, domain10, domain11, domain12, domain13 }) { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
        /// </summary>
        /// <param name="domain1">Value to be used in the validation.</param>
        /// <param name="domain2">Value to be used in the validation.</param>
        /// <param name="domain3">Value to be used in the validation.</param>
        /// <param name="domain4">Value to be used in the validation.</param>
        /// <param name="domain5">Value to be used in the validation.</param>
        /// <param name="domain6">Value to be used in the validation.</param>
        /// <param name="domain7">Value to be used in the validation.</param>
        /// <param name="domain8">Value to be used in the validation.</param>
        /// <param name="domain9">Value to be used in the validation.</param>
        /// <param name="domain10">Value to be used in the validation.</param>
        /// <param name="domain11">Value to be used in the validation.</param>
        /// <param name="domain12">Value to be used in the validation.</param>
        /// <param name="domain13">Value to be used in the validation.</param>
        /// <param name="domain14">Value to be used in the validation.</param>
        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5, object domain6, object domain7, object domain8, object domain9, object domain10, object domain11, object domain12, object domain13, object domain14)
            : this(new[] { domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8, domain9, domain10, domain11, domain12, domain13, domain14 }) { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
        /// </summary>
        /// <param name="domain1">Value to be used in the validation.</param>
        /// <param name="domain2">Value to be used in the validation.</param>
        /// <param name="domain3">Value to be used in the validation.</param>
        /// <param name="domain4">Value to be used in the validation.</param>
        /// <param name="domain5">Value to be used in the validation.</param>
        /// <param name="domain6">Value to be used in the validation.</param>
        /// <param name="domain7">Value to be used in the validation.</param>
        /// <param name="domain8">Value to be used in the validation.</param>
        /// <param name="domain9">Value to be used in the validation.</param>
        /// <param name="domain10">Value to be used in the validation.</param>
        /// <param name="domain11">Value to be used in the validation.</param>
        /// <param name="domain12">Value to be used in the validation.</param>
        /// <param name="domain13">Value to be used in the validation.</param>
        /// <param name="domain14">Value to be used in the validation.</param>
        /// <param name="domain15">Value to be used in the validation.</param>
        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5, object domain6, object domain7, object domain8, object domain9, object domain10, object domain11, object domain12, object domain13, object domain14, object domain15)
            : this(new[] { domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8, domain9, domain10, domain11, domain12, domain13, domain14, domain15 }) { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="DomainValidatorAttribute"/>.</para>
        /// </summary>
        /// <param name="domain1">Value to be used in the validation.</param>
        /// <param name="domain2">Value to be used in the validation.</param>
        /// <param name="domain3">Value to be used in the validation.</param>
        /// <param name="domain4">Value to be used in the validation.</param>
        /// <param name="domain5">Value to be used in the validation.</param>
        /// <param name="domain6">Value to be used in the validation.</param>
        /// <param name="domain7">Value to be used in the validation.</param>
        /// <param name="domain8">Value to be used in the validation.</param>
        /// <param name="domain9">Value to be used in the validation.</param>
        /// <param name="domain10">Value to be used in the validation.</param>
        /// <param name="domain11">Value to be used in the validation.</param>
        /// <param name="domain12">Value to be used in the validation.</param>
        /// <param name="domain13">Value to be used in the validation.</param>
        /// <param name="domain14">Value to be used in the validation.</param>
        /// <param name="domain15">Value to be used in the validation.</param>
        /// <param name="domain16">Value to be used in the validation.</param>
        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5, object domain6, object domain7, object domain8, object domain9, object domain10, object domain11, object domain12, object domain13, object domain14, object domain15, object domain16)
            : this(new[] { domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8, domain9, domain10, domain11, domain12, domain13, domain14, domain15, domain16 }) { }

        /// <summary>
        /// 1st value to be used in the validation
        /// </summary>
        public object Domain1 { get { return GetDomain(1); } }

        /// <summary>
        /// 2nd value to be used in the validation
        /// </summary>
        public object Domain2 { get { return GetDomain(2); } }

        /// <summary>
        /// 3rd value to be used in the validation
        /// </summary>
        public object Domain3 { get { return GetDomain(3); } }

        /// <summary>
        /// 4th value to be used in the validation
        /// </summary>
        public object Domain4 { get { return GetDomain(4); } }

        /// <summary>
        /// 5th value to be used in the validation
        /// </summary>
        public object Domain5 { get { return GetDomain(5); } }

        /// <summary>
        /// 6th value to be used in the validation
        /// </summary>
        public object Domain6 { get { return GetDomain(6); } }

        /// <summary>
        /// 7th value to be used in the validation
        /// </summary>
        public object Domain7 { get { return GetDomain(7); } }

        /// <summary>
        /// 8th value to be used in the validation
        /// </summary>
        public object Domain8 { get { return GetDomain(8); } }

        /// <summary>
        /// 9th value to be used in the validation
        /// </summary>
        public object Domain9 { get { return GetDomain(9); } }

        /// <summary>
        /// 10th value to be used in the validation
        /// </summary>
        public object Domain10 { get { return GetDomain(10); } }

        /// <summary>
        /// 11th value to be used in the validation
        /// </summary>
        public object Domain11 { get { return GetDomain(11); } }

        /// <summary>
        /// 12th value to be used in the validation
        /// </summary>
        public object Domain12 { get { return GetDomain(12); } }

        /// <summary>
        /// 13th value to be used in the validation
        /// </summary>
        public object Domain13 { get { return GetDomain(13); } }

        /// <summary>
        /// 14th value to be used in the validation
        /// </summary>
        public object Domain14 { get { return GetDomain(14); } }

        /// <summary>
        /// 15th value to be used in the validation
        /// </summary>
        public object Domain15 { get { return GetDomain(15); } }

        /// <summary>
        /// 16th value to be used in the validation
        /// </summary>
        public object Domain16 { get { return GetDomain(16); } }

        /// <summary>
        /// Return the domain object corresponding to the specified index.
        /// </summary>
        /// <param name="index">The index of the domain object (1 based).</param>
        private object GetDomain(int index)
        {
            if (index < 1)
            {
                throw new InvalidOperationException();
            }

            return domain.Length >= index ? domain[index - 1] : null;
        }

        /// <summary>
        /// List of values to be used in the validation.
        /// </summary>
	    private object[] Domain
	    {
	        get { return domain; }
	    }

	    /// <summary>
		/// Creates the <see cref="DomainValidatorAttribute"/> described by the attribute object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <remarks>This operation must be overridden by subclasses.</remarks>
		/// <returns>The created <see cref="DomainValidatorAttribute"/>.</returns>
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new DomainValidator<object>(Negated, Domain);
		}

#if !SILVERLIGHT
        private readonly Guid typeId = Guid.NewGuid();

        /// <summary>
        /// Gets a unique identifier for this attribute.
        /// </summary>
        public override object TypeId
        {
            get
            {
                return this.typeId;
            }
        }
#endif
    }
}
