using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Repositories;
using Droits.Services;
using Microsoft.Extensions.Logging;

namespace Droits.Tests.UnitTests.Services
{
    public class NoteServiceUnitTests
    {
        private readonly Mock<INoteRepository> _mockRepo;
        private readonly NoteService _service;

        public NoteServiceUnitTests()
        {
            _mockRepo = new Mock<INoteRepository>();
            var mockDroitService = new Mock<IDroitService>();
            var mockFileService = new Mock<IDroitFileService>();
            Mock<ILogger<NoteService>> mockLogger = new();
                
            _service = new NoteService(_mockRepo.Object, mockDroitService.Object, mockFileService.Object, mockLogger.Object);
        }

        [Fact]
        public async Task SaveNoteAsync_NewNote_AddsNote()
        {
            // Given
            var newNote = new Note { Text = "NewNoteText" };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Note>(),It.IsAny<bool>())).ReturnsAsync(newNote);

            // When
            var result = await _service.SaveNoteAsync(newNote);

            // Then
            Assert.Equal(newNote, result);
            _mockRepo.Verify(r => r.AddAsync(newNote, true), Times.Once);
        }

        [Fact]
        public async Task SaveNoteAsync_ExistingNote_UpdatesNote()
        {
            // Given
            var existingNote = new Note { Id = Guid.NewGuid(), Text = "ExistingNoteText" };
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Note>(),It.IsAny<bool>())).ReturnsAsync(existingNote);

            // When
            var result = await _service.SaveNoteAsync(existingNote);

            // Then
            Assert.Equal(existingNote, result);
            _mockRepo.Verify(r => r.UpdateAsync(existingNote, true), Times.Once);
        }

        [Fact]
        public async Task GetNoteAsync_ExistingId_ReturnsNote()
        {
            // Given
            var noteId = Guid.NewGuid();
            var expectedNote = new Note { Id = noteId, Text = "TestNoteText" };
            _mockRepo.Setup(r => r.GetNoteAsync(noteId)).ReturnsAsync(expectedNote);

            // When
            var result = await _service.GetNoteAsync(noteId);

            // Then
            Assert.Equal(expectedNote, result);
        }
    }
}