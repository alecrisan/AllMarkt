using AllMarkt.ViewModels;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace AllMarktTests.ViewModels.OrderItem
{
    public class OrderItemViewModelTests
    {
        [Theory]
        [InlineData("valid name")]
        [InlineData(" valid name ")]
        public void Name_MustContain_ActualValue(string name)
        {
            //Arrange
            var viewModels = GetViewModels(name);

            //Act
            var result = viewModels.GetValidationResults();

            //Assert
            result.Should().AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        [InlineData("\n")]

        public void Name_CannotBe_Null_Empty_OrWhiteSpace(string name)
        {
            //Arrange
            var viewModels = GetViewModels(name);

            //Act
            var result = viewModels.GetValidationResults();

            //Assert
            result.Should().AllBeEquivalentTo(new[]
                {
                    new ValidationResult("The Name field is required." ,new[]{"Name"})
                });
        }

        private static IEnumerable<object> GetViewModels(string name)
        {
            yield return new OrderItemViewModel
            {
                Name = name,
                Amount = 1
            };
        }
    }
}
