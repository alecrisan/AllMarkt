using AllMarkt.ViewModels;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace AllMarktTests.ViewModels.ProductCategory
{
    public class ProductCategoryViewModelTests
    {
        public string Name { get; internal set; }

        [Theory]
        [InlineData("valid name")]
        [InlineData(" valid name ")]
        public void Name_MustContain_ActualValue(string name)
        {
            GetViewModelsProductCategories(name)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("\n")]
        [InlineData(" ")]
        public void Name_CannotBe_NullOrWhiteSpaces(string name)
        {
            GetViewModelsProductCategories(name)
                .GetValidationResults().Should().AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The Name field is required.", new[]{ "Name"})
                });
        }

        [Fact]
        public void Name_CannotBe_LongerThan_50Characters() {
            var maxLengthName = new string('c', 50);
            var invalidName = new string('c', 51);

            GetViewModelsProductCategories(maxLengthName)
                .GetValidationResults().Should().AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            GetViewModelsProductCategories(invalidName)
                .GetValidationResults()
                .Should().AllBeEquivalentTo(
                new[]{
                        new ValidationResult("The field Name must be a string or array type with a maximum length of '50'.", new[]{ "Name" }) });
        }

        [Fact]
        public void Description_CannotBe_LongerThan255()
        {
            var maxLengthDescription = new string('d', 255);
            var invalidDescription = new string('d', 256);

            GetViewModelsProductCategories("name",maxLengthDescription)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            GetViewModelsProductCategories("name", invalidDescription)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(new[]{
                        new ValidationResult("The field Description must be a string or array type with a maximum length of '255'.", new[]{ "Description" }) });
        }

        private static IEnumerable<object> GetViewModelsProductCategories(string name, string description = null, int shopId = 0, string ShopName = null)
        {
            yield return new ProductCategoryViewModel
            {
                Name = name,
                Description = description,
                ShopId = shopId,
                ShopName = ShopName
            };
        }
    }
}
