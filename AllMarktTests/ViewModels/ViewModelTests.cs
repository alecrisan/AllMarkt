using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AllMarktTests.ViewModels
{
    public static class ViewModelTests
    {
        public static IEnumerable<ValidationResult> GetValidationResults(this object viewModel)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(viewModel, null, null);
            Validator.TryValidateObject(viewModel, validationContext, validationResults, true);
            return validationResults;
        }

        public static IEnumerable<IEnumerable<ValidationResult>> GetValidationResults(this IEnumerable<object> viewModels)
            => viewModels.Select(GetValidationResults);

        public static IEnumerable<IEnumerable<ValidationResult>> GetValidationResults(params object[] viewModels)
            => viewModels.Select(GetValidationResults);
    }
}