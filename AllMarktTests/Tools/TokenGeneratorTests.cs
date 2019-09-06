using AllMarkt.Entities;
using AllMarkt.Tools;
using AllMarkt.ViewModels;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Xunit;

namespace AllMarktTests.Tools
{
    public class TokenGeneratorTests
    {

        private AppSettings _appSettings;
        private ITokenGenerator _tokenGenerator;

        private readonly string Token;
        private readonly byte[] Key;
        private readonly TokenValidationParameters Validations;
        private readonly UserTokenDataModel User;
        private readonly JwtSecurityTokenHandler Handler;

        public TokenGeneratorTests()
        {
            _appSettings = new AppSettings
            {
                Secret = "STRINGU ALA BLANAOSSTRINGU ALA BLANAOS BLANAOSSTRINGU ALA BLANAOS BLANAOSSTRINGU ALA BLANAOS BLANAOSSTRINGU ALA BLANAOS",
                TokenLifetimeDays = 7
            };
            _tokenGenerator = new TokenGenerator();
            User = new UserTokenDataModel
            {
                Id = 1,
                DisplayName = "testttt",
                Email = "ion@ion.ion",
                UserRole = UserRole.Admin.ToString()
            };

            Token = _tokenGenerator.Generate(_appSettings, User);

            Key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            Validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            Handler = new JwtSecurityTokenHandler();
        }

        [Fact]
        public void GenerateToken_Returns_Actual_UserId()
        {
            //Arrange

            //Act
            var claims = Handler.ValidateToken(Token, Validations, out var tokenSecure);

            var claimId = int.Parse(
                claims.Claims
                .First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);

            //Assert
            claimId.Should().Be(User.Id);
        }

        [Fact]
        public void GenerateToken_Returns_Actual_UserRole()
        {
            //Arrange 

            //Act
            var claims = Handler.ValidateToken(Token, Validations, out var tokenSecure);

            var claimRole = claims.Claims
                .First(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value;

            //Assert
            claimRole.Should().Be(User.UserRole);
        }

        [Fact]
        public void GenerateToken_Returns_Actual_UserDisplayName()
        {
            //Arrange

            //Act
            var claims = Handler.ValidateToken(Token, Validations, out var tokenSecure);

            var claimName = claims.Claims
                .First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            //Assert
            claimName.Should().Be(User.DisplayName);
        }

        [Fact]
        public void GenerateToken_Returns_Actual_UserEmail()
        {
            //Arrange

            //Act
            var claims = Handler.ValidateToken(Token, Validations, out var tokenSecure);

            var claimEmail = claims.Claims
                .First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;

            //Assert
            claimEmail.Should().Be(User.Email);
        }
    }
}
