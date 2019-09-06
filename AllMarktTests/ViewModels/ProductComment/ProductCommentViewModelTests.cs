using AllMarkt.ViewModels;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace AllMarktTests.ViewModels.ProductComment
{
    public class ProductCommentViewModelTests
    {
        [Theory]
        [InlineData("valid text")]
        [InlineData(" valid text ")]
        public void Text_MustContain_ActualValue(string text)
        {
            GetViewModels(1, text)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Fact]
        public void Text_CannotBe_LongerThan_1023Characters()
        {
            var maxLengthText = new string('c', 1023);
            var invalidText = new string('c', 1024);

            GetViewModels(1, maxLengthText)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            GetViewModels(1, invalidText)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                    new[]
                    {
                         new ValidationResult("The field Text must be a string or array type with a maximum length of '1023'.", new[]{ "Text" })
                    });
        }

        [Fact]
        public void Rating_ShouldBe_Between1And5()
        {
            //Arrange
            var validRating = 5;
            var invalidRating = 6;
            var invalidRating2 = 0;

            GetViewModels(validRating, "test")
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            GetViewModels(invalidRating, "test")
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The field Rating must be between 1 and 5.", new[]{"Rating"})
                });
            GetViewModels(invalidRating2, "test")
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The field Rating must be between 1 and 5.", new[]{"Rating"})
                });
        }

        private static IEnumerable<object> GetViewModels(int rating, string text = null)
        {
            yield return new ProductCommentViewModel
            {
                Rating = rating,
                Text = text,
                AddedById = 1,
                AddedByName = "UserTest",
                ProductId = 2,
                ProductName = "ProductTest",
                DateSent = DateTime.UtcNow
            };
        }
    }
}
