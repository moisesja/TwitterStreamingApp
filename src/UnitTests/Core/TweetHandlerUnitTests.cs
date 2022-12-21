using Microsoft.Extensions.Logging;
using Moq;
using TwitterStreamingLib.Abstraction;
using TwitterStreamingLib.Core;

namespace TwitterStreamingUnitTests.Core;

public class TweetHandlerUnitTests
{
    /// <summary>
    /// Mocking routine
    /// </summary>
    /// <returns></returns>
    private (TweetHandler, Mock<ITweetRepository>) CreatetTweetHandler()
    {
        Mock<ILogger<TweetHandler>> loggerMock = new();

        Mock<ITweetRepository> repositoryMock = new();
        repositoryMock.SetReturnsDefault<Guid>(Guid.NewGuid());

        return (new TweetHandler(loggerMock.Object, repositoryMock.Object), repositoryMock);
    }

    [Fact]
    public async Task Test_HandleTweetAsync_EmptyJson()
    {
        var (handler, _) = CreatetTweetHandler();
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await handler.HandleTweetAsync(string.Empty));
    }

    [Theory]
    [InlineData("{\"data\":{\"edit_history_tweet_ids\":[\"1605594247092117505\"],\"id\":\"1605594247092117505\",\"text\":\"RT @ST0NEHENGE: Winter solstice: The science behind the shortest day of the year https://t.co/UhH0ZER2b4\"}}")]
    [InlineData("{\"data\":{\"edit_history_tweet_ids\":[\"1605662735873761283\"],\"id\":\"1605662735873761283\",\"text\":\"@kohei__forgive こちらこそありがとうございます😊\\n痩せる痩せるサギで全く痩せられてないので、参考にさせて頂きます✨\"}}")]
    public async Task Test_HandleTweetAsync_NoHashtag(string tweet)
    {
        var (handler, repositoryMock) = CreatetTweetHandler();
        await handler.HandleTweetAsync(tweet);

        repositoryMock.Verify(call => call.InsertAsync(It.IsAny<string>()), Times.Exactly(1), "Call must be made once");
        repositoryMock.Verify(call => call.FlagTweetContainsNoTags(It.IsAny<Guid>()), Times.Exactly(1), "Call must be made once");
        repositoryMock.VerifyNoOtherCalls();
    }
}