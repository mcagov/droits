using Droits.Controllers;
using Droits.Exceptions;
using Droits.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Droits.Models.Entities;
using Droits.Models.Enums;
using Droits.Models.FormModels;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Droits.Tests.UnitTests.Controllers
{
    public class NoteControllerUnitTests
    {
        private readonly Mock<INoteService> _mockService;
        private readonly NoteController _controller;
        private readonly Mock<ITempDataDictionary> _mockTempData;

        public NoteControllerUnitTests()
        {
            var mockLogger = new Mock<ILogger<NoteController>>();
            _mockService = new Mock<INoteService>();
            _mockTempData = new Mock<ITempDataDictionary>();

            _controller = new NoteController(mockLogger.Object, _mockService.Object)
            {
                TempData = _mockTempData.Object
            };
        }

        [Fact]
        public async void Edit_ValidId_ReturnsViewWithNoteForm()
        {
            // Given
            var noteId = Guid.NewGuid();
            var expectedNote = new Note { Id = noteId, Text = "Test" , Title = "Test Title", Type = NoteType.ExternalReference};
            _mockService.Setup(service => service.GetNoteAsync(noteId)).ReturnsAsync(expectedNote);

            // When
            var result = await _controller.Edit(noteId);

            // Then
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<NoteForm>(viewResult.ViewData.Model);
            Assert.Equal(expectedNote.Text, model.Text);
            Assert.Equal(expectedNote.Title, model.Title);
            Assert.Equal(expectedNote.Type, model.Type);
        }

        [Fact]
        public async void Edit_NoteNotFoundException_RedirectsToIndex()
        {
            // Given
            var noteId = Guid.NewGuid();
            _mockService.Setup(service => service.GetNoteAsync(noteId)).Throws(new NoteNotFoundException());
            _mockTempData.Setup(t => t["ErrorMessage"]).Returns("Note not found");

            // When
            var result = await _controller.Edit(noteId);

            // Then
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async void Save_ModelStateInvalid_ReturnsEditViewWithForm()
        {
            // Given
            var form = new NoteForm();
            _controller.ModelState.AddModelError("error", "some error");
            _mockTempData.Setup(t => t["ErrorMessage"]).Returns("Could not save Note");

            // When
            var result = await _controller.Save(form);

            // Then
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Edit", viewResult.ViewName);
            var model = Assert.IsAssignableFrom<NoteForm>(viewResult.ViewData.Model);
            Assert.Equal(form, model);
        }

        [Theory]
        [InlineData("Droit", typeof(NoteForm))]
        [InlineData("Wreck", typeof(NoteForm))]
        [InlineData("Salvor", typeof(NoteForm))]
        [InlineData("Letter", typeof(NoteForm))]
        public async void Save_ValidFormWithEntityAssociation_RedirectsToAssociatedController(string controllerName, Type formType)
        {
            // Given
            var entityId = Guid.NewGuid();
            var form = (NoteForm)Activator.CreateInstance(formType)!;
            form.GetType().GetProperty($"{controllerName}Id")?.SetValue(form, entityId);
            _mockTempData.Setup(t => t["SuccessMessage"]).Returns("Note saved successfully.");

            // When
            var result = await _controller.Save(form);

            // Then
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("View", redirectResult.ActionName);
            Assert.Equal(controllerName, redirectResult.ControllerName);
            Assert.Equal(entityId, redirectResult.RouteValues?["id"]);
        }

        [Fact]
        public void Add_GetRequest_ReturnsEditViewWithEmptyNoteForm()
        {
            // Given
            var noteForm = new NoteForm();

            // When
            var result = _controller.Add(noteForm);

            // Then
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Edit", viewResult.ViewName);
            var model = Assert.IsAssignableFrom<NoteForm>(viewResult.ViewData.Model);
            Assert.Equal(string.Empty, model.Text);
        }

        [Fact]
        public async void Edit_InvalidId_RedirectsToIndex()
        {
            // Given
            var noteId = Guid.NewGuid();
            _mockService.Setup(service => service.GetNoteAsync(noteId)).Throws(new NoteNotFoundException());

            // When
            var result = await _controller.Edit(noteId);

            // Then
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async void Edit_DefaultId_ReturnsEditViewWithEmptyNoteForm()
        {
            // Given
            var noteId = default(Guid);

            // When
            var result = await _controller.Edit(noteId);

            // Then
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<NoteForm>(viewResult.ViewData.Model);
            Assert.Equal(string.Empty, model.Text);
        }

        [Theory]
        [InlineData("DroitId")]
        [InlineData("WreckId")]
        [InlineData("SalvorId")]
        [InlineData("LetterId")]
        public void Add_WithEntityId_ReturnsEditViewWithNoteFormHavingEntityId(string entityIdProperty)
        {
            // Given
            var entityId = Guid.NewGuid();
            var noteForm = new NoteForm();
            noteForm.GetType().GetProperty(entityIdProperty)?.SetValue(noteForm, entityId);

            // When
            var result = _controller.Add(noteForm);

            // Then
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<NoteForm>(viewResult.ViewData.Model);
            Assert.Equal(entityId, model.GetType().GetProperty(entityIdProperty)?.GetValue(model));

            foreach (var prop in new[] { "DroitId", "WreckId", "SalvorId", "LetterId" })
            {
                if (prop != entityIdProperty)
                {
                    Assert.Null(model.GetType().GetProperty(prop)?.GetValue(model));
                }
            }
        }
    }
}
