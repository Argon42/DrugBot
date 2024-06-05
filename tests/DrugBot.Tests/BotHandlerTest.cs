using DrugBot.Core;
using DrugBot.Core.Bot;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.ExpressionBuilders.Logging;

namespace DrugBot.Tests;

public class BotHandlerTest
{
    [Test]
    public async Task MessageProcessing_WhenMessageTextIsNullOrEmpty_ShouldReturnImmediately()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<BotHandler>>();
        var mockProcessors = new List<IProcessor> { Mock.Of<IProcessor>() };
        var botHandler = new BotHandler(mockProcessors, mockLogger.Object);

        var message = new MessageStab { Text = null! };
        var bot = Mock.Of<IBot<UserStab, MessageStab>>();

        // Act
        await botHandler.MessageProcessing(message, bot, CancellationToken.None);

        // Assert
        mockProcessors.ForEach(p =>
            Mock.Get(p).Verify(proc => proc.HasTrigger(It.IsAny<MessageStab>(), It.IsAny<string[]>()), Times.Never));
    }

    [Test]
    public async Task MessageProcessing_WhenProcessorExists_ShouldCallTryProcessMessage()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<BotHandler>>();
        var mockProcessor = new Mock<IProcessor>();
        mockProcessor.Setup(p => p.HasTrigger(It.IsAny<MessageStab>(), It.IsAny<string[]>())).Returns(true);

        var botHandler = new BotHandler(new List<IProcessor> { mockProcessor.Object }, mockLogger.Object);

        var message = new MessageStab { Text = "triggered" };
        var bot = Mock.Of<IBot<UserStab, MessageStab>>();

        // Act
        await botHandler.MessageProcessing(message, bot, CancellationToken.None);

        // Assert
        mockProcessor.Verify(p => p.TryProcessMessage(bot, message, It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task MessageProcessing_WhenNoProcessorExists_ShouldNotCallTryProcessMessage()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<BotHandler>>();
        var mockProcessor = new Mock<IProcessor>();
        mockProcessor.Setup(p => p.HasTrigger(It.IsAny<MessageStab>(), It.IsAny<string[]>())).Returns(false);

        var botHandler = new BotHandler(new List<IProcessor> { mockProcessor.Object }, mockLogger.Object);

        var message = new MessageStab { Text = "not triggered" };
        var bot = Mock.Of<IBot<UserStab, MessageStab>>();

        // Act
        await botHandler.MessageProcessing(message, bot, CancellationToken.None);

        // Assert
        mockProcessor.Verify(
            p => p.TryProcessMessage(It.IsAny<IBot<UserStab, MessageStab>>(), It.IsAny<MessageStab>(),
                It.IsAny<CancellationToken>()), Times.Never());
    }

    [Test]
    public async Task MessageProcessing_WhenExceptionOccurs_ShouldLogError()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<BotHandler>>();
        var mockProcessor = new Mock<IProcessor>();
        mockProcessor.Setup(p => p.HasTrigger(It.IsAny<MessageStab>(), It.IsAny<string[]>()))
            .Throws(new Exception("Test exception"));

        var botHandler = new BotHandler(new List<IProcessor> { mockProcessor.Object }, mockLogger.Object);

        var message = new MessageStab { Text = "exception" };
        var bot = Mock.Of<IBot<UserStab, MessageStab>>();

        // Act
        await botHandler.MessageProcessing(message, bot, CancellationToken.None);

        // Assert
        mockLogger.Verify(Log.With
                .LogLevel(LogLevel.Error)
                .And.LogMessage("Error on message processing"),
            Times.Once);
    }
}