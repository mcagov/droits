using Droits.Models.Entities;
using Droits.Models.FormModels;
using Droits.Repositories;
using Droits.Services;
using Microsoft.AspNetCore.Http;

namespace Droits.Tests.UnitTests.Services;

public class ImageServiceUnitTests
{
    private readonly Mock<IImageRepository> _mockRepo;
    private readonly ImageService _service;

    public ImageServiceUnitTests()
    {
        _mockRepo = new Mock<IImageRepository>();
        _service = new ImageService(_mockRepo.Object);
    }


    [Fact]
    public async Task GetImageStream_ReturnsAStream()
    {
        // Given
        var fakeStream = new MemoryStream();
        var key = "myKey";

        _mockRepo.Setup(r => r.GetImageStreamAsync(key)).ReturnsAsync(fakeStream);
        
        // When
        var response = await _service.GetImageStreamAsync(key);
        
        // Then
        Assert.Equal(fakeStream,response);
    }
    
    [Fact]
    public async Task SaveImageForm_ForANewImage_ReturnsAnImage()
    {
        // Given
        Image image;
        var mockFormFile = new Mock<IFormFile>();
        mockFormFile.Setup(f => f.FileName).Returns("test.jpg");
        mockFormFile.Setup(f => f.Length).Returns(1024);
        var imageForm = new ImageForm()
        {
            Id = new Guid(),
            Title = "Test",
            WreckMaterialId = new Guid(),
            ImageFile = mockFormFile.Object
        };
        image = imageForm.ApplyChanges(new Image());

        _mockRepo.Setup(r => r.AddAsync(It.IsAny<Image>())).ReturnsAsync(image);
        _mockRepo.Setup(r => r.UploadImageFileAsync(image, imageForm.ImageFile)).Callback<Image, IFormFile>((image, imageFile) => {});
        
        // When
        var response = await _service.SaveImageFormAsync(imageForm);
        
        // Then
        Assert.Equal(image,response);
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Image>()), Times.Once);

    }
    
    [Fact]
    public async Task SaveImageForm_ForAnExistingImage_ReturnsAnImage()
    {
        // Given
        var existingImage = new Image()
        {
            Id = Guid.NewGuid(),
            Title = "Test2"
        };
        var mockFormFile = new Mock<IFormFile>();
        mockFormFile.Setup(f => f.FileName).Returns("test2.jpg");
        mockFormFile.Setup(f => f.Length).Returns(1024);
        var imageForm = new ImageForm(existingImage)
        {
            WreckMaterialId = new Guid(),
            ImageFile = mockFormFile.Object
        };

        _mockRepo.Setup(r => r.GetImageAsync(It.IsAny<Guid>())).ReturnsAsync(existingImage);
        _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Image>())).ReturnsAsync(existingImage);
        _mockRepo.Setup(r => r.UploadImageFileAsync(existingImage, imageForm.ImageFile)).Callback<Image, IFormFile>((image, imageFile) => {});
        
        // When
        var response = await _service.SaveImageFormAsync(imageForm);
        
        // Then
        Assert.Equal(existingImage,response);
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Image>()), Times.Once);
    }
}