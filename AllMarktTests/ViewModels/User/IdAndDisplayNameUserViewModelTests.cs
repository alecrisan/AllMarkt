using AllMarkt.ViewModels;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace AllMarktTests.ViewModels.User
{
    public class IdAndDisplayNameUserViewModelTests
    {
        [Theory]
        [InlineData("user")]
        [InlineData(" u s e r ")]
        public void DisplayName_MustContain_Actual_Value(string displayName)
        {
            GetViewModels(displayName)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        [InlineData("\n")]
        public void DisplayName_CannotBe_Null_Empty_OrWhiteSpace(string displayName)
        {
            GetViewModels(displayName)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                    new[]
                    {
                        new ValidationResult("The DisplayName field is required.", new[]{ "DisplayName" })
                    });
        }

        private static IEnumerable<object> GetViewModels(string displayName)
        {
            yield return new IdAndDisplayNameUserViewModel
            {
                DisplayName = displayName
            };
        }
    }
}
