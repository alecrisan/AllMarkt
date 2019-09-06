using AllMarkt.ViewModels;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace AllMarktTests.ViewModels.ShopCategory
{
    public class ShopCategoryViewModelTests
    {
        [Theory]
        [InlineData(1, 2)]
        public void ShopId_And_CategoryId_MustContain_Actual_Value(int shopId, int categoryId)
        {
            GetViewModels(shopId, categoryId)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(-1, 1)]
        public void ShopId__CannotBe_Zero_Or_NegativValue(int shopId, int categoryId)
        {
            GetViewModels(shopId, categoryId)
                .GetValidationResults()
                .Should()
                 .AllBeEquivalentTo(
                    new[]
                    {
                        new ValidationResult("Only positive number allowed", new[]{ "ShopId" })
                    });
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, -1)]
        public void CategoryId_CannotBe_Zero_Or_NegativValue(int shopId, int categoryId)
        {
            GetViewModels(shopId, categoryId)
                .GetValidationResults()
                .Should()
                 .AllBeEquivalentTo(
                    new[]
                    {
                        new ValidationResult("Only positive number allowed", new[]{ "CategoryId" })
                    });
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, -1)]
        public void ShopId_AndCategoryId_CannotBe_Zero_Or_NegativValue(int shopId, int categoryId)
        {
            GetViewModels(shopId, categoryId)
                .GetValidationResults()
                .Should()
                 .AllBeEquivalentTo(
                    new[]
                    {
                        new ValidationResult("Only positive number allowed", new[]{ "ShopId" }),
                        new ValidationResult("Only positive number allowed", new[]{ "CategoryId" })
                    });
        }

        private static IEnumerable<object> GetViewModels(int shopId, int categoryId)
        {
            yield return new ShopCategoryViewModel
            {
                ShopId = shopId,
                CategoryId = categoryId
            };
        }
    }
}
