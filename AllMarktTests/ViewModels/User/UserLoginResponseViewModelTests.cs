using AllMarkt.ViewModels;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace AllMarktTests.ViewModels.User
{
    public class UserLoginResponseViewModelTests
    {
        private const string validToken = "agsdjagsjdghasjdasasdjhawdjkghskdjdhfkjsdhfjksdhfjkheukyhrukheaskgjhkdufhgjkdfhgjkdh23874y2378423jkashd72";
        private const string validEmail = "email@email.com";
        private const string validUserRole = "Admin";

        [Theory]
        [InlineData(null, validEmail, validUserRole)]
        [InlineData("", validEmail, validUserRole)]
        [InlineData(" ", validEmail, validUserRole)]
        [InlineData("\t", validEmail, validUserRole)]
        [InlineData("\r", validEmail, validUserRole)]
        [InlineData("\n", validEmail, validUserRole)]
        public void Token_CannotBe_Null_Empty_OrWhiteSpace(string token, string email, string userRole)
        {
            GetViewModels(token, email, userRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                    new[]
                    {
                        new ValidationResult("The Token field is required.", new[]{ "Token" })
                    });
        }

        [Theory]
        [InlineData("token", validEmail, validUserRole)]
        [InlineData(" t o k e n", validEmail, validUserRole)]
        public void Token_MustContain_Actual_Value(string token, string email, string userRole)
        {
            GetViewModels(token, email, userRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData(validToken, "email@yahoo.com", validUserRole)]
        [InlineData(validToken, " testEmail@g.com ", validUserRole)]
        [InlineData(validToken, "  testEmail@g.   com ", validUserRole)]
        [InlineData(validToken, "  test@g.com ", validUserRole)]
        public void Email_MustContain_Value(string token, string email, string userRole)
        {
            GetViewModels(token, email, userRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData(validToken, "email@yahoo.com", validUserRole)]
        [InlineData(validToken, "email@abc.com", validUserRole)]
        [InlineData(validToken, "email@.com", validUserRole)]
        [InlineData(validToken, "email@com", validUserRole)]
        [InlineData(validToken, "email@gmail.com", validUserRole)]
        [InlineData(validToken, "email@yahoo.fr", validUserRole)]
        [InlineData(validToken, "email@yahoo.abc", validUserRole)]
        public void Valid_Email_MustContain_SpecificElementValidation(string token, string email, string userRole)
        {
            GetViewModels(token, email, userRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData(validToken, "emailyahoo.com", validUserRole)]
        [InlineData(validToken, "email@", validUserRole)]
        [InlineData(validToken, "email.com", validUserRole)]
        [InlineData(validToken, "@yahoo.com", validUserRole)]
        public void Invalid_Email_Should_Retrieve_Error_ValidationResult(string token, string email, string userRole)
        {
            GetViewModels(token, email, userRole)
             .GetValidationResults()
             .Should()
             .AllBeEquivalentTo(
             new[]
             {
                    new ValidationResult("The Email field is not a valid e-mail address.", new[]{ "Email" })
             });
        }

        [Theory]
        [InlineData(validToken, validEmail, "Admin")]
        [InlineData(validToken, validEmail, "Shop")]
        [InlineData(validToken, validEmail, "Moderator")]
        [InlineData(validToken, validEmail, "Customer")]
        public void Valid_UserRole_MustContain_SpecificElementValidation(string token, string email, string userRole)
        {
            GetViewModels(token, email, userRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData(validToken, validEmail, null)]
        [InlineData(validToken, validEmail, "")]
        [InlineData(validToken, validEmail, " ")]
        [InlineData(validToken, validEmail, "\t")]
        [InlineData(validToken, validEmail, "\r")]
        [InlineData(validToken, validEmail, "\n")]
        public void UserRole_CannotBe_Null_Empty_OrWhiteSpace(string token, string email, string userRole)
        {
            GetViewModels(token, email, userRole)
             .GetValidationResults()
             .Should()
             .AllBeEquivalentTo(
             new[]
             {
                    new ValidationResult("The UserRole field is required.", new[]{ "UserRole" })
             });
        }

        private static IEnumerable<object> GetViewModels(string token, string email, string userRole)
        {
            yield return new UserLoginResponseViewModel
            {
                Token = token,
                Email = email,
                UserRole = userRole
            };
        }
    }
}
