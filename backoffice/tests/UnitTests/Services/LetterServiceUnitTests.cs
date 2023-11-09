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

        [Fact]
        public async Task GetTemplateBodyAsync_NoDroit_ReturnsOriginalContent()
        {
            // Given
            const LetterType letterType = LetterType.ReportAcknowledged;
            var templatePath = Path.Combine(Environment.CurrentDirectory, "Views/LetterTemplates", $"{letterType.ToString()}.Body.txt");
            const string templateContent = "This is a test.";
            await File.WriteAllTextAsync(templatePath, templateContent);

            // When
            var result = await _service.GetTemplateBodyAsync(letterType, null);

            // Then
            Assert.Equal(templateContent, result);
        }

        [Fact]
        public async Task GetTemplateSubjectAsync_WithDroit_ReturnsSubstitutedContent()
        {
            // Given
            const LetterType letterType = LetterType.ReportConfirmed;
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
            _mockRepo.Verify(r => r.UpdateAsync(letter), Times.Once);
            Assert.NotNull(result);
        }
    }
}
