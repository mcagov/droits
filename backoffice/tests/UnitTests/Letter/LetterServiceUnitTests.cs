using Droits.Clients;
using Droits.Services;
using Droits.Repositories;
using Microsoft.Extensions.Logging;

namespace Droits.Tests.UnitTests.Letter;

public class LetterServiceUnitTests
{
    private readonly ILetterService _service;

    public LetterServiceUnitTests()
    {
        Mock<ILogger<LetterService>> mockLogger = new Mock<ILogger<LetterService>>();
        Mock<IGovNotifyClient> mockClient = new Mock<IGovNotifyClient>();
        Mock<ILetterRepository> mockLetterRepository = new Mock<ILetterRepository>();

        _service = new LetterService(
            mockLogger.Object,
            mockClient.Object,
            mockLetterRepository.Object);
    }



}
