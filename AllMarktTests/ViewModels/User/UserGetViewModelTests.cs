using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using AllMarkt.ViewModels;
using FluentAssertions;
using Xunit;

namespace AllMarktTests.ViewModels.User
{
    public class UserGetViewModelTests
    {
        [Theory]
        [InlineData("email@yahoo.com", "123456", "dn", "Admin")]
        [InlineData(" testEmail@g.com ", "123456", " dn ", "Shop  ")]
        [InlineData("  testEmail@g.   com ", "123456", " dn ", " Customer ")]
        [InlineData("  test@g.com ", "123456", " dn ", "")]
        public void Email_Password_DisplayName_MustContain_Value(string email, string password, string displayName, string userRole)
        {
            GetViewModels(email, password, displayName, userRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData("email@yahoo.com", "123456", "display name", "Admin")]
        [InlineData("email@abc.com", "123456", "display name", "Admin")]
        [InlineData("email@.com", "123456", "display name", "Admin")]
        [InlineData("email@com", "123456", "display name", "Admin")]
        [InlineData("email@gmail.com", "123456", "display name", "Admin")]
        [InlineData("email@yahoo.fr", "123456", "display name", "Admin")]
        [InlineData("email@yahoo.abc", "123456", "display name", "Admin")]
        public void Valid_Email_MustContain_SpecificElementValidation(string validEmail, string validPassword, string validDisplayName, string validUserRole)
        {
            GetViewModels(validEmail, validPassword, validDisplayName, validUserRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData("emailyahoo.com", "123456", "display name", "Admin")]
        [InlineData("email@", "123456", "display name", "Admin")]
        [InlineData("email.com", "123456", "display name", "Admin")]
        [InlineData("@yahoo.com", "123456", "display name", "Admin")]
        public void Invalid_Email_Should_Retrieve_Error_ValidationResult(string invalidEmail, string validPassword, string validDisplayName, string validUserRole)
        {
            GetViewModels(invalidEmail, validPassword, validDisplayName, validUserRole)
             .GetValidationResults()
             .Should()
             .AllBeEquivalentTo(
             new[]
             {
                    new ValidationResult("The Email field is not a valid e-mail address.", new[]{ "Email" })
             });
        }

        [Theory]
        [InlineData(null, null, null, "")]
        [InlineData("", "", "", "")]
        [InlineData(" ", " ", " ", "")]
        public void Email_Password_DisplayName_CannotBe_Null_Empty_OrWhiteSpace(string email, string password, string displayName, string userRole)
        {
            GetViewModels(email, password, displayName, userRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The Email field is required.", new[]{ "Email" }),
                    new ValidationResult("The DisplayName field is required.", new[]{ "DisplayName" }),
                    new ValidationResult("The Password field is required.", new[]{ "Password" })
                });
        }

        [Theory]
        [InlineData("email@yahoo.com", "123456", "display name", null)]
        public void UserRole_CanBeNull(string validEmail, string validPassword, string validDisplayName, string validUserRole)
        {
            GetViewModels(validEmail, validPassword, validDisplayName, validUserRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData("email@yahoo.com", "123456", "display name", "Admin")]
        [InlineData("email@yahoo.com", "123456", "display name", "Moderator")]
        [InlineData("email@yahoo.com", "123456", "display name", "Shop")]
        [InlineData("email@yahoo.com", "123456", "display name", "Customer")]
        public void Valid_UserRole_MustBelongTo_EnumClass(string validEmail, string validPassword, string validDisplayName, string validUserRole)
        {
            GetViewModels(validEmail, validPassword, validDisplayName, validUserRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());
        }

        [Theory]
        [InlineData("email@yahoo.com", "123456", "display name", "Armin")]
        [InlineData("email@yahoo.com", "123456", "display name", " ")]
        [InlineData("email@yahoo.com", "123456", "display name", "user")]
        public void Invalid_UserRole_Should_Retrieve_Error_ValidationResult(string validEmail, string validPassword, string validDisplayName, string validUserRole)
        {
            GetViewModels(validEmail, validPassword, validDisplayName, validUserRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The field UserRole is invalid.", new[]{ "UserRole"})
                });
        }

        [Fact]
        public void Email_CannotBeLongerThan_80Characters()
        {
            string validPassword = "123456";
            string validDisplayName = "display name";
            string validUserRole = "Customer";

            string validEmailLength = GetFieldWithLength("abc@yahoo.com", 80);
            GetViewModels(validEmailLength, validPassword, validDisplayName, validUserRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            string invalidEmailLength = GetFieldWithLength("abc@yahoo.com", 81);
            GetViewModels(invalidEmailLength, validPassword, validDisplayName, validUserRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The field Email must be a string or array type with a maximum length of '80'.", new[]{ "Email"}),
                });
        }

        [Fact]
        public void Password_CannotBeLongerThan_64Characters()
        {
            string validEmail = "emailtest@u.com";
            string validDisplayName = "display name";
            string validUserRole = "Customer";

            string validPasswordLength = GetFieldWithLength("12345", 64);
            GetViewModels(validEmail, validPasswordLength, validDisplayName, validUserRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            string invalidPasswordLength = GetFieldWithLength("12345", 65);
            GetViewModels(validEmail, invalidPasswordLength, validDisplayName, validUserRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The field Password must be a string or array type with a maximum length of '64'.", new[]{ "Password"}),
                });
        }

        [Fact]
        public void DisplayName_CannotBe_LongerThan_50Characters()
        {
            string validEmail = "emailtest@u.com";
            string validPassword = "123456";
            string validUserRole = "Customer";


            string validDisplayNameLength = GetFieldWithLength("nume display", 50);

            GetViewModels(validEmail, validPassword, validDisplayNameLength, validUserRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(Enumerable.Empty<ValidationResult>());

            string invalidDisplayNameLength = GetFieldWithLength("nume display", 51);

            GetViewModels(validEmail, validPassword, invalidDisplayNameLength, validUserRole)
                .GetValidationResults()
                .Should()
                .AllBeEquivalentTo(
                new[]
                {
                    new ValidationResult("The field DisplayName must be a string or array type with a maximum length of '50'.", new[]{ "DisplayName"} )
                }
                );
        }

        private string GetFieldWithLength(string text, int length)
        {
            StringBuilder fieldBuild = new StringBuilder();
            string fieldWithCharacters = text;
            int defaultFieldLength = fieldWithCharacters.Length;
            string maxLengthFieldBesideFieldWithCharacters = new string('e', length - defaultFieldLength);
            var field = fieldBuild.Append(fieldWithCharacters).Append(maxLengthFieldBesideFieldWithCharacters);

            return field.ToString();
        }

        private static IEnumerable<object> GetViewModels(string email, string password, string displayName, string role)
        {
            yield return new UserGetViewModel
            {
                Email = email,
                Password = password,
                DisplayName = displayName,
                UserRole = role
            };
        }
    }
}
