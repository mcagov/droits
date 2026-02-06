using Droits.Clients;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Repositories;
using Droits.Services;
using Microsoft.Extensions.Logging;
using Notify.Models.Responses;

namespace Droits.Tests.UnitTests.Services
{
    public class LetterServiceUnitTests
    {
        private readonly Mock<IGovNotifyClient> _mockGovNotifyClient;
        private readonly Mock<ILetterRepository> _mockRepo;
        private readonly Mock<IDroitService> _mockDroitService;
        private readonly LetterService _service;
        private readonly Mock<IAccountService> _mockAccountService;

        public LetterServiceUnitTests()
        {
            _mockGovNotifyClient = new Mock<IGovNotifyClient>();
            _mockRepo = new Mock<ILetterRepository>();
            _mockDroitService = new Mock<IDroitService>();
            _mockAccountService = new Mock<IAccountService>();
            Mock<ILogger<LetterService>> mockLogger = new();
            _service = new LetterService(
                mockLogger.Object,
                _mockGovNotifyClient.Object,
                _mockRepo.Object,
                _mockDroitService.Object,
                _mockAccountService.Object);
        }

        [Theory]
        [InlineData(LetterType.ReportConfirmed)]
        [InlineData(LetterType.LloydsOfLondonNotification)]
        [InlineData(LetterType.ClosureOwnerNotFound)]
        [InlineData(LetterType.ClosureOwnerWaivesRights)]
        [InlineData(LetterType.ClosureDredgeFindOwnerNotFound)]
        [InlineData(LetterType.ClosureDredgeFindOwnerWaivesRights)]
        [InlineData(LetterType.ClosureWessexArchaeology)]
        [InlineData(LetterType.ClosureCustom)]
        [InlineData(LetterType.DredgeFindEod)]
        [InlineData(LetterType.CustomLetter)]
        [InlineData(LetterType.CustomEmail)]
        [InlineData(LetterType.ClosureMuseumLetterOwnerFound)]
        [InlineData(LetterType.ClosureMuseumLetterOwnerNotFound)]
        [InlineData(LetterType.ClosureOwnerFoundMuseumDonationAgreed)]
        [InlineData(LetterType.ClosureOwnerNotFoundMuseumDonationAgreed)]
        public async Task GetTemplateBodyAsync_NoDroit_ReturnsOriginalContent(LetterType letterType)
        {
            // Given
            var templatePath = Path.Combine(Environment.CurrentDirectory, "Views/LetterTemplates", $"{letterType.ToString()}.Body.txt");
            const string templateContent = "This is a test.";
            await File.WriteAllTextAsync(templatePath, templateContent);

            // When
            var result = await _service.GetTemplateBodyAsync(letterType, null);

            // Then
            Assert.Equal(templateContent, result);
        }

        [Theory]
        [InlineData(LetterType.ReportConfirmed)]
        [InlineData(LetterType.LloydsOfLondonNotification)]
        [InlineData(LetterType.ClosureOwnerNotFound)]
        [InlineData(LetterType.ClosureOwnerWaivesRights)]
        [InlineData(LetterType.ClosureDredgeFindOwnerNotFound)]
        [InlineData(LetterType.ClosureDredgeFindOwnerWaivesRights)]
        [InlineData(LetterType.ClosureWessexArchaeology)]
        [InlineData(LetterType.ClosureCustom)]
        [InlineData(LetterType.DredgeFindEod)]
        [InlineData(LetterType.CustomLetter)]
        [InlineData(LetterType.CustomEmail)]
        [InlineData(LetterType.ClosureMuseumLetterOwnerFound)]
        [InlineData(LetterType.ClosureMuseumLetterOwnerNotFound)]
        [InlineData(LetterType.ClosureOwnerFoundMuseumDonationAgreed)]
        [InlineData(LetterType.ClosureOwnerNotFoundMuseumDonationAgreed)]
        public async Task GetTemplateSubjectAsync_WithDroit_ReturnsSubstitutedContent(LetterType letterType)
        {
            // Given
            var droit = new Droit();
            _mockDroitService.Setup(d => d.GetDroitAsync(It.IsAny<Guid>())).ReturnsAsync(droit);
            var templatePath = Path.Combine(Environment.CurrentDirectory, "Views/LetterTemplates", $"{letterType.ToString()}.Subject.txt");
            var templateContent = "Droit ((reference)) has been confirmed";
            await File.WriteAllTextAsync(templatePath, templateContent);

            // When
            var result = await _service.GetTemplateSubjectAsync(letterType, droit);

            // Then
            Assert.Contains(droit.Reference, result);
            Assert.DoesNotContain("((reference))", result);
        }
        
        [Fact]
        public void Validate_TemplateType_Exists_For_Every_LetterType()
        {
            // Given
            var expectedTypes = new[]
            {
                LetterType.ReportAcknowledged,
                LetterType.ReportCompleteInformation,
                LetterType.ReportConfirmed,
                LetterType.LloydsOfLondonNotification,
                LetterType.ClosureOwnerNotFound,
                LetterType.ClosureOwnerWaivesRights,
                LetterType.ClosureDredgeFindOwnerNotFound,
                LetterType.ClosureDredgeFindOwnerWaivesRights,
                LetterType.ClosureWessexArchaeology,
                LetterType.ClosureCustom,
                LetterType.DredgeFindEod,
                LetterType.CustomLetter,
                LetterType.CustomEmail,
                LetterType.ClosureMuseumLetterOwnerFound,
                LetterType.ClosureMuseumLetterOwnerNotFound,
                LetterType.ClosureOwnerFoundMuseumDonationAgreed,
                LetterType.ClosureOwnerNotFoundMuseumDonationAgreed
            };
            
            // When
            var actualLetterTypes = Enum.GetValues(typeof(LetterType)).Cast<LetterType>();

            // Then
            Assert.Equal(expectedTypes, actualLetterTypes);
        }
        
        
        [Fact]
        public async Task GetTemplateSubjectAsync_WithDroit_UppercaseReturnsSubstitutedContent()
        {
            // Given
            const LetterType letterType = LetterType.ReportConfirmed;
            var droit = new Droit();
            _mockDroitService.Setup(d => d.GetDroitAsync(It.IsAny<Guid>())).ReturnsAsync(droit);
            var templatePath = Path.Combine(Environment.CurrentDirectory, "Views/LetterTemplates", $"{letterType.ToString()}.Subject.txt");
            var templateContent = "Droit ((REFERENCE)) has been confirmed";
            await File.WriteAllTextAsync(templatePath, templateContent);

            // When
            var result = await _service.GetTemplateSubjectAsync(letterType, droit);

            // Then
            Assert.Contains(droit.Reference, result);
            Assert.DoesNotContain("((REFERENCE))", result);
        }

        [Fact]
        public async Task SendLetterAsync_ValidId_SendsLetterAndMarksAsSent()
        {
            // Given
            var letterId = Guid.NewGuid();
            var letter = new Letter { Id = letterId };
            _mockRepo.Setup(r => r.GetLetterAsync(letterId)).ReturnsAsync(letter);
            _mockGovNotifyClient.Setup(c => c.SendLetterAsync(letter)).ReturnsAsync(new EmailNotificationResponse());

            // When
            var result = await _service.SendLetterAsync(letterId);

            // Then
            _mockRepo.Verify(r => r.UpdateAsync(letter, true), Times.Once);
            Assert.NotNull(result);
        }
        
    }
}
