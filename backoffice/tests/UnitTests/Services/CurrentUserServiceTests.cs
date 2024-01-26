using System.Security.Claims;
using Droits.Services;
using Microsoft.AspNetCore.Http;

namespace Droits.Tests.UnitTests.Services;

public class CurrentUserServiceTests
{
    [Fact]
    public void GetCurrentUserId_ReturnsUserId_WhenClaimExists()
    {
        // Given
        var userId = Guid.NewGuid().ToString();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.NameIdentifier, userId)
        }));
        var context = new DefaultHttpContext { User = user };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(context);

        var service = new AccountService(accessor.Object);

        // When
        var result = service.GetCurrentUserId();

        // Then
        Assert.Equal(Guid.Parse(userId), result);
    }


    [Fact]
    public void GetCurrentUserId_ReturnsNull_WhenClaimDoesNotExist()
    {
        // Given
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var context = new DefaultHttpContext { User = user };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(context);

        var service = new AccountService(accessor.Object);

        // When
        var result = service.GetCurrentUserId();

        // Then
        Assert.False(result.HasValue);
    }
}