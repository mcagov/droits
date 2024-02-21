using Droits.Exceptions;
using Droits.Models.Entities;
using Droits.Repositories;
using Droits.Services;

namespace Droits.Tests.UnitTests.Services
{
    public class UserServiceUnitTests
    {
        private readonly Mock<IUserRepository> _mockRepo;
        private readonly UserService _service;

        public UserServiceUnitTests()
        {
            _mockRepo = new Mock<IUserRepository>();
            _service = new UserService(_mockRepo.Object);
        }

        [Fact]
        public async Task SaveUserAsync_NewUser_AddsUser()
        {
            // Given
            var newUser = new ApplicationUser { Id = Guid.Empty };
            _mockRepo.Setup(r => r.AddUserAsync(newUser)).ReturnsAsync(newUser);

            // When
            var result = await _service.SaveUserAsync(newUser);

            // Then
            Assert.Equal(newUser, result);
            _mockRepo.Verify(r => r.AddUserAsync(newUser), Times.Once);
            _mockRepo.Verify(r => r.UpdateUserAsync(newUser), Times.Never);
        }

        [Fact]
        public async Task SaveUserAsync_ExistingUser_UpdatesUser()
        {
            // Given
            var existingUser = new ApplicationUser { Id = Guid.NewGuid() };
            _mockRepo.Setup(r => r.UpdateUserAsync(existingUser)).ReturnsAsync(existingUser);

            // When
            var result = await _service.SaveUserAsync(existingUser);

            // Then
            Assert.Equal(existingUser, result);
            _mockRepo.Verify(r => r.UpdateUserAsync(existingUser), Times.Once);
            _mockRepo.Verify(r => r.AddUserAsync(existingUser), Times.Never);
        }

        [Fact]
        public async Task GetUserAsync_ExistingId_ReturnsUser()
        {
            // Given
            var userId = Guid.NewGuid();
            var expectedUser = new ApplicationUser { Id = userId };
            _mockRepo.Setup(r => r.GetUserAsync(userId)).ReturnsAsync(expectedUser);

            // When
            var result = await _service.GetUserAsync(userId);

            // Then
            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public async Task GetOrCreateUserAsync_UserExists_ReturnsExistingUser()
        {
            // Given
            var authId = "auth123";
            var existingUser = new ApplicationUser { Id = Guid.NewGuid(), AuthId = authId };
            _mockRepo.Setup(r => r.GetUserByAuthIdAsync(authId)).ReturnsAsync(existingUser);

            // When
            var result = await _service.GetOrCreateUserAsync(authId, "John Doe", "john@example.com");

            // Then
            Assert.Equal(existingUser, result);
            _mockRepo.Verify(r => r.GetUserByAuthIdAsync(authId), Times.Once);
            _mockRepo.Verify(r => r.AddUserAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }
        
        [Fact]
        public async Task GetOrCreateUserAsync_UserWithEmailExists_ReturnsExistingUser()
        {
            // Given
            const string emailAddress = "john@example.com";
            const string authId = "auth123";
            var existingUser = new ApplicationUser { Id = Guid.NewGuid(), Email = emailAddress };
            _mockRepo.Setup(r => r.GetUserByAuthIdAsync(It.IsAny<string>())).Throws<UserNotFoundException>();
            _mockRepo.Setup(r => r.GetUserByEmailAddressAsync(emailAddress))
                .ReturnsAsync(existingUser);

            var updatedUser = new ApplicationUser()
            {
                Id = existingUser.Id,
                Email = emailAddress,
                Name = "John Doe",
                AuthId = authId
            };
            
            _mockRepo.Setup(r => r.UpdateUserAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(updatedUser);
            
            // When
            var result = await _service.GetOrCreateUserAsync(authId, "John Doe", emailAddress);

            // Then
            Assert.Equal(updatedUser, result);
            _mockRepo.Verify(r => r.GetUserByAuthIdAsync(authId), Times.Once);
            _mockRepo.Verify(r => r.GetUserByEmailAddressAsync(emailAddress), Times.Once);
            _mockRepo.Verify(r => r.UpdateUserAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _mockRepo.Verify(r => r.AddUserAsync(It.IsAny<ApplicationUser>()), Times.Never);

        }

        [Fact]
        public async Task GetOrCreateUserAsync_UserDoesNotExist_AddsNewUser()
        {
            // Given
            var authId = "auth123";
            var newUser = new ApplicationUser { Id = Guid.NewGuid(), AuthId = authId };
            _mockRepo.Setup(r => r.GetUserByAuthIdAsync(authId)).Throws<UserNotFoundException>();
            _mockRepo.Setup(r => r.GetUserByEmailAddressAsync(It.IsAny<string>())).Throws<UserNotFoundException>();
            _mockRepo.Setup(r => r.AddUserAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(newUser);

            // When
            var result = await _service.GetOrCreateUserAsync(authId, "Jane Doe", "jane@example.com");

            // Then
            Assert.Equal(newUser, result);
            _mockRepo.Verify(r => r.GetUserByAuthIdAsync(authId), Times.Once);
            _mockRepo.Verify(r => r.AddUserAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }
    }
}
