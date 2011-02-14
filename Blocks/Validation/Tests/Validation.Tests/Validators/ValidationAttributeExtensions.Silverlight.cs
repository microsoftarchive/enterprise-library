using System.ComponentModel.DataAnnotations;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Validators
{
    public static class ValidationAttributeExtensions
    {
        public static bool IsValid(this ValidationAttribute attribute, object value)
        {
            var result = attribute.GetValidationResult(value, new ValidationContext(new object(), null, null));
            return result == null || string.IsNullOrEmpty(result.ErrorMessage);
        }
    }
}
