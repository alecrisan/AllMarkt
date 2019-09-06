using AllMarkt.ViewModels;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace AllMarktTests.ViewModels.Category
{
    public class OrderViewModelTests
    {
        [Theory]
        [InlineData("valid shop", "valid customer", "valid address", "valid awb")]
        [InlineData(" valid shop ", " valid customer ", " valid address ", " valid awb ")]
        public void RequiredFields_MustContain_ActualValue(string shop, string customer, string address, string awb)
        {
            //Arrange
            var viewModels = GetViewModels(shop, customer, address, awb);

            //Act
            var result = viewModels.GetValidationResults();

            //Assert
            result.Should().AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData(null, null, null, null)]
        [InlineData("", "", "", "")]
        [InlineData(" ", " ", " ", " ")]
        [InlineData("\t", "\t", "\t", "\t")]
        [InlineData("\r", "\r", "\r", "\r")]
        [InlineData("\n", "\n", "\n", "\n")]
        public void RequiredFields_CannotBe_Null_Empty_OrWhiteSpace(string shop, string customer, string address, string awb)
        {
            //Arrange
            var viewModles = GetViewModels(shop, customer, address, awb);

            //Act
            var result = viewModles.GetValidationResults();

            //Assert
            result.Should().AllBeEquivalentTo(new[] {
                new ValidationResult("The ShopName field is required." , new[]{ "ShopName" }),
                
                new ValidationResult("The CustomerName field is required."  , new[]{ "CustomerName" }),

                new ValidationResult("The DeliveryAddress field is required."  , new[]{ "DeliveryAddress" }),

                new ValidationResult("The AWB field is required." , new[]{ "AWB" })
            });
        }

        [Fact]
        public void Names_CannotBe_LongerThan_50Characters()
        {
            //Arrange
            var maxLengthName = new string('c', 50);
            var invalidName = new string('c', 51);

            var maxLengthViewModels = GetViewModels(maxLengthName, maxLengthName, "address", "awb");
            var invalidNameViewModels = GetViewModels(invalidName, invalidName, "address", "awb");

            //Act
            var maxLengthResult = maxLengthViewModels.GetValidationResults();
            var invalidNameResult = invalidNameViewModels.GetValidationResults();

            //Assert
            maxLengthResult.Should().AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
            invalidNameResult.Should().AllBeEquivalentTo(new[]
                    {
                        new ValidationResult("The field ShopName must be a string or array type with a maximum length of '50'." , new[]{ "ShopName" }),

                        new ValidationResult("The field CustomerName must be a string or array type with a maximum length of '50'."  , new[]{ "CustomerName" })

                    });
        }

        [Fact]
        public void Address_CannotBe_LongerThan_255Characters()
        {
            //Arrange
            var maxLengthAddress = new string('c', 255);
            var invalidAddress = new string('c', 256);

            var maxLengthAddressViewModel = GetViewModels("shop", "customer", maxLengthAddress, "awb");
            var invalidAddressViewModel = GetViewModels("shop", "customer", invalidAddress, "awb");

            //Act
            var maxLengthAddressResult = maxLengthAddressViewModel.GetValidationResults();
            var invalidAddressResult = invalidAddressViewModel.GetValidationResults();

            //Assert
            maxLengthAddressResult.Should().AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
            invalidAddressResult.Should().AllBeEquivalentTo(new[]
                   {
                        new ValidationResult("The field DeliveryAddress must be a string or array type with a maximum length of '255'."   , new[]{ "DeliveryAddress" })
                   });
        }

        [Fact]
        public void Notes_CannotBe_LongerThan_255Characters()
        {
            //Arrange
            var maxLengthNotes = new string('c', 255);
            var invalidNotes = new string('c', 256);

            var maxLengthNotesViewModel = GetViewModels("shop", "customer", "address", "awb", "0123456789", maxLengthNotes);
            var invalidNotesViewModel = GetViewModels("shop", "customer", "address", "awb", "0123456789", invalidNotes);

            //Act
            var maxLengthNotesResult = maxLengthNotesViewModel.GetValidationResults();
            var invalidNotesResult = invalidNotesViewModel.GetValidationResults();

            //Assert
            maxLengthNotesResult.Should().AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
            invalidNotesResult.Should().AllBeEquivalentTo(new[]
                   {
                        new ValidationResult("The field AdditionalNotes must be a string or array type with a maximum length of '255'."    , new[]{ "AdditionalNotes" })
                   });
        }

        [Fact]

        public void PhoneNumber_MustContain_OnlyDigits()
        {
            //Arrange
            var validNumber = "0123456789";
            var invalidNumber = "01234c6789";

            var validNumberViewModels = GetViewModels("shop", "customer", "address", "awb", validNumber);
            var invalidNumberViewModels = GetViewModels("shop", "customer", "address", "awb", invalidNumber);

            //Act
            var validNumberResult = validNumberViewModels.GetValidationResults();
            var invalidNumberResult = invalidNumberViewModels.GetValidationResults();

            //Assert
            validNumberResult.Should().AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
            invalidNumberResult.Should().AllBeEquivalentTo(new[]
                   {
                        new ValidationResult("The field DeliveryPhoneNumber must match the regular expression '^0[0-9]{9}$'.", new[]{ "DeliveryPhoneNumber" })
                   });
        }

        [Theory]
        [InlineData("01234567899")]
        [InlineData("012345678")]
        public void PhoneNumber_CannotHaveLength_DifferentThan10(string invalidNumber)
        {
            //Arrange
            var validNumber = new string('0', 10);

            var validNumberViewModels = GetViewModels("shop", "customer", "address", "awb", validNumber);
            var invalidNumberViewModels = GetViewModels("shop", "customer", "address", "awb", invalidNumber);

            //Act
            var validNumberResult = validNumberViewModels.GetValidationResults();
            var invalidNumberResult = invalidNumberViewModels.GetValidationResults();

            //Assert
            validNumberResult.Should().AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
            invalidNumberResult.Should().AllBeEquivalentTo(new[]
                   {
                        new ValidationResult("The field DeliveryPhoneNumber must match the regular expression '^0[0-9]{9}$'.", new[]{ "DeliveryPhoneNumber" })
                   });
        }

        private static IEnumerable<object> GetViewModels
            (string shopName, string customerName, string address,
            string awb, string phone = null, string notes = null)
        {
            yield return new OrderViewModel
            {
                ShopName = shopName,
                CustomerName = customerName,
                DeliveryPhoneNumber = phone,
                DeliveryAddress = address,
                AdditionalNotes = notes,
                AWB = awb,

            };
        }
    }
}
