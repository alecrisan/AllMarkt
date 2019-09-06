using AllMarkt.ViewModels;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace AllMarktTests.ViewModels.Product
{
    public class ProductViewModelTests
    {
        [Theory]
        [InlineData("valid name")]
        [InlineData(" valid name ")]
        public void Name_MustContain_ActualValue(string name)
        {
            GetViewModels(name)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData("\r")]
        [InlineData("\n")]
        public void Name_CannotBe_Null_Or_Empty(string name)
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
        public void Name_CannotBeLongerThan_50Characters()
        {
            //Arrange
            var maxLengthName = new string('c', 50);
            var invalidName = new string('c', 51);

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
                        new ValidationResult("The field Name must be a string or array type with a maximum length of '50'.", new[]{ "Name" })
                    });
        }

        [Theory]
        [InlineData("valid desc")]
        [InlineData(" valid desc ")]
        public void Description_MustContain_ActualValue(string description)
        {
            GetViewModels("name", description)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData("\r")]
        [InlineData("\n")]
        public void Description_CannotBe_Null_Or_Empty(string description)
        {
            GetViewModels("name", description)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                    new[]
                    {
                        new ValidationResult("The Description field is required.", new[]{ "Description" })
                    });

        }

        [Fact]
        public void Description_CannotBeLongerThan_255Characters()
        {
            //Arrange
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

        private static IEnumerable<object> GetViewModels(string name, string description = "description", double price = 0, string imageURI = null, bool state = true, int productCategoryId = 1, string productCategoryName = "ProductCategory")
        {
            yield return new ProductViewModel
            {
                Name = name,
                Description = description,
                Price = price,
                ImageURI = imageURI,
                State = state,
                ProductCategoryId = productCategoryId,
                ProductCategoryName = productCategoryName
            };
        }
    }
}

