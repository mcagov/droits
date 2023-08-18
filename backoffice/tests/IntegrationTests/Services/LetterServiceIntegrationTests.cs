using System;
using System.IO;
using System.Threading.Tasks;
using Droits.Clients;
using Droits.Data;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels;
using Droits.Models.ViewModels;
using Droits.Repositories;
using Droits.Services;
using Droits.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Notify.Models.Responses;
using Xunit;

namespace Droits.Tests.IntegrationTests.Services
{
    public class LetterServiceIntegrationTests : IClassFixture<TestFixture>
    {
        private readonly Mock<IAccountService> _mockCurrentUserService = new();
        private readonly Mock<ILogger<LetterService>> _mockLogger = new();
        private readonly Mock<IDroitService> _mockDroitService = new();

        private readonly LetterService _service;
        private readonly Mock<IGovNotifyClient> _mockClient;
        private readonly DroitsContext _dbContext;

        private string _templatePath;

        public LetterServiceIntegrationTests()
        {
            _mockClient = new Mock<IGovNotifyClient>();
            _dbContext = TestDbContextFactory.CreateDbContext();
            DatabaseSeeder.SeedData(_dbContext); // Seed the test data

            var repo = new LetterRepository(_dbContext, _mockCurrentUserService.Object);
            _service = new LetterService(_mockLogger.Object, _mockClient.Object,
                repo, _mockDroitService.Object);

            _templatePath = Path.Combine(Environment.CurrentDirectory, "Views/LetterTemplates");
        }

        [Fact]
        public async Task SendLetterAsync_ValidId_SendsLetterAndMarksAsSent()
        {
            // Given
            var sampleLetter = new Letter
            {
                Id = default,
                Subject = "Sample Subject",
                Body = "Sample Body",
                Recipient = "sample@example.com",
                Type = LetterType.ReportAcknowledged
            };
            sampleLetter = await _service.SaveLetterAsync(new LetterForm(sampleLetter));
            
            await _dbContext.SaveChangesAsync();

            _mockClient.Setup(c => c.SendLetterAsync(sampleLetter))
                .ReturnsAsync(new EmailNotificationResponse());
            
            // When
            var response = await _service.SendLetterAsync(sampleLetter.Id);
            var sentLetter = await _service.GetLetterAsync(sampleLetter.Id);

            // Then
            Assert.NotNull(response);
            Assert.True(sentLetter.DateSent.HasValue);
        }

        [Fact]
        public async Task GetLettersListViewAsync_ReturnsLetterListView()
        {
            // Given
            

            // When
            var searchOptions = new SearchOptions();
            var result = await _service.GetLettersListViewAsync(searchOptions);

            // Then
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTemplateBodyAsync_WithDroit_ReturnsSubstitutedContent()
        {
            // Given
            const LetterType letterType = LetterType.ReportConfirmed;
            var droit = new Droit();
            var templateContent = "Ref: ((reference))!";
            _templatePath = Path.Combine(_templatePath, $"{letterType.ToString()}.Body.txt");
            await File.WriteAllTextAsync(_templatePath, templateContent);

            // When
            var result = await _service.GetTemplateBodyAsync(letterType, droit);

            // Then
            Assert.Contains(droit.Reference, result);
            Assert.DoesNotContain("((reference))", result);

            // Clean up after the test
            File.Delete(_templatePath);
        }

        [Fact]
        public async Task SaveLetterAsync_NewLetter_ShouldAddLetterToRepository()
        {
            // Given
            var form = new LetterForm
            {
                Subject = "Test Subject",
                Body = "Test Body",
                Recipient = "test@example.com",
                Type = LetterType.ReportAcknowledged
            };

            // When
            var result = await _service.SaveLetterAsync(form);

            // Then
            var savedLetter = await _dbContext.Letters.FirstOrDefaultAsync(l => l.Id == result.Id);
            Assert.NotNull(savedLetter);

            _dbContext.Letters.Remove(savedLetter);
            await _dbContext.SaveChangesAsync();
        }
        
    }
}
