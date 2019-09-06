using AllMarkt.Tools;
using AllMarkt.ViewModels;
using FluentAssertions;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace AllMarktTests.Tools
{
    public class ClaimsGetterTests
    {
        private readonly IEnumerable<Claim> Claims;
        private readonly IClaimsGetter claimsGetter;
        private readonly UserGetViewModel User;
        
        public ClaimsGetterTests()
        {
            User = new UserGetViewModel
            {
                Id = 1,
                DisplayName = "Ion",
                UserRole = "Admin",
                Email = "ion@ion.ion"
            };
            claimsGetter = new ClaimsGetter();
            Claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, User.Id.ToString()),
                new Claim(ClaimTypes.Name, User.DisplayName),
                new Claim(ClaimTypes.Role, User.UserRole),
                new Claim(ClaimTypes.Email, User.Email)
            };
        }

        [Fact]
        public void ClaimsGetter_UserId_Returns_Actual_UserId()
        {
            //Arrange

            //Act
            var claimId = claimsGetter.UserId(Claims);

            //Assert
            claimId.Should().Be(User.Id);
        }

        [Fact]
        public void ClaimsGetter_UserId_Returns_MinusOne_When_Claims_Are_Null()
        {
            //Arrange

            //Act
            var claimId = claimsGetter.UserId(null);

            //Assert
            claimId.Should().Be(-1);
        }

        [Fact]
        public void ClaimsGetter_DisplayName_Returns_Actual_DisplayName()
        {
            //Arrange

            //Act
            var claimDisplayName = claimsGetter.DisplayName(Claims);

            //Assert
            claimDisplayName.Should().Be(User.DisplayName);
        }

        [Fact]
        public void ClaimsGetter_DisplayName_Returns_EmptyString_When_Claims_Are_Null()
        {
            //Arrange

            //Act
            var claimDisplayName = claimsGetter.DisplayName(null);

            //Assert
            claimDisplayName.Should().Be("");
        }

        [Fact]
        public void ClaimsGetter_UserRole_Returns_Actual_UserRole()
        {
            //Arrange

            //Act
            var claimUserRole = claimsGetter.UserRole(Claims);

            //Assert
            claimUserRole.Should().Be(User.UserRole);
        }

        [Fact]
        public void ClaimsGetter_UserRole_Returns_Empty_String_When_Claims_Are_Null()
        {
            //Arrange

            //Act
            var claimUserRole = claimsGetter.UserRole(null);

            //Assert
            claimUserRole.Should().Be("");
        }

        [Fact]
        public void ClaimsGetter_Email_Returns_Actual_Email()
        {
            //Arrange

            //Act
            var claimEmail = claimsGetter.Email(Claims);

            //Assert
            claimEmail.Should().Be(User.Email);
        }

        [Fact]
        public void ClaimsGetter_Email_Returns_Empty_String_When_Claims_Are_Null()
        {
            //Arrange

            //Act
            var claimEmail = claimsGetter.Email(null);

            //Assert
            claimEmail.Should().Be("");
        }
    }
}
