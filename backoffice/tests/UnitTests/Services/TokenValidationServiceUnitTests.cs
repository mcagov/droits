using System.Security.Claims;
using Droits.Models.Entities;
using Droits.Services;

namespace Droits.Tests.UnitTests.Services
{
    public class TokenValidationServiceUnitTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly TokenValidationService _tokenValidationService;

        public TokenValidationServiceUnitTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _tokenValidationService = new TokenValidationService(_userServiceMock.Object);
        }

        [Fact]
        public async Task OnTokenValidated_Should_UpdateClaims_WhenUserIsFound()
        {
            // Given
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "user_object_id"),
                new Claim("name", "RoW User"),
                new Claim("preferred_username", "row.user@m.ca"),
            });

            var applicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "row.user@m.ca",
            };

            _userServiceMock.Setup(x => x.GetOrCreateUserAsync("user_object_id", "RoW User", "row.user@m.ca"))
                .ReturnsAsync(applicationUser);

            // When
            await _tokenValidationService.OnTokenValidated(identity);

            // Then
            Assert.Contains(identity.Claims, claim => claim.Type == ClaimTypes.NameIdentifier && claim.Value == applicationUser.Id.ToString());
            Assert.Contains(identity.Claims, claim => claim is { Type: ClaimTypes.Email, Value: "row.user@m.ca" });
        }

        [Fact]
        public async Task OnTokenValidated_Should_ThrowException_WhenObjectIdClaimNotFound()
        {
            // When
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim("name", "RoW User"),
                new Claim("preferred_username", "row.user@m.ca"),
            });

            // Then
            await Assert.ThrowsAsync<Exception>(() => _tokenValidationService.OnTokenValidated(identity));
        }

        [Fact]
        public async Task OnTokenValidated_Should_UpdateClaims_WhenUserIdIsDifferent()
        {
            // Given
            var identity = new ClaimsIdentity(new List<Claim>
            {
                new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", "user_object_id"),
                new Claim("name", "RoW User"),
                new Claim("preferred_username", "row.user@m.ca"),
                new Claim(ClaimTypes.NameIdentifier, "different_user_id"), 
                new Claim(ClaimTypes.Email, "old_email@example.com"), 
            });

            var applicationUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "row.user@m.ca",
            };

            _userServiceMock.Setup(x => x.GetOrCreateUserAsync("user_object_id", "RoW User", "row.user@m.ca"))
                .ReturnsAsync(applicationUser);

            // When
            await _tokenValidationService.OnTokenValidated(identity);

            // Then
            Assert.Contains(identity.Claims, claim => claim.Type == ClaimTypes.NameIdentifier && claim.Value == applicationUser.Id.ToString());
            Assert.Contains(identity.Claims, claim => claim.Type == ClaimTypes.Email && claim.Value == "row.user@m.ca");
            Assert.DoesNotContain(identity.Claims, claim => claim.Type == ClaimTypes.NameIdentifier && claim.Value == "different_user_id");
            Assert.DoesNotContain(identity.Claims, claim => claim.Type == ClaimTypes.Email && claim.Value == "old_email@example.com");
        }
    }
}
