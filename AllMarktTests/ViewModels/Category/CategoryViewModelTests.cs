using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AllMarkt.ViewModels;
using FluentAssertions;
using Xunit;

namespace AllMarktTests.ViewModels.Category
{
    public class CategoryViewModelTests
    {
        [Theory]
        [InlineData("valid name")]
        [InlineData(" valid name ")]
        public void NameMustContainActualValue(string name)
        {
            GetViewModels(name)
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
        public void NameCannotBeNullEmptyOrWhiteSpace(string name)
        {
            GetViewModels(name)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                    new[]
                    {
                        new ValidationResult("The Name field is required.", new[]{ "Name" })
                    });
        }

        [Fact]
        public void NameCannotBeLongerThan80Characters()
        {
            var maxLengthName = new string('c', 80);
            var invalidName = new string('c', 81);

            GetViewModels(maxLengthName)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            GetViewModels(invalidName)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                    new[]
                    {
                        new ValidationResult("The field Name must be a string or array type with a maximum length of '80'.", new[]{ "Name" })
                    });
        }

        [Fact]
        public void DescriptionCannotBeLongerThan255Characters()
        {
            var maxLengthDescription = new string('c', 255);
            var invalidDescription = new string('c', 256);

            GetViewModels("name", maxLengthDescription)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            GetViewModels("name", invalidDescription)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                    new[]
                    {
                        new ValidationResult("The field Description must be a string or array type with a maximum length of '255'.", new[]{ "Description" })
                    });
        }

        private static IEnumerable<object> GetViewModels(string name, string description = null)
        {
            yield return new CategoryViewModel
            {
                Name = name,
                Description = description
            };
        }
    }
}
