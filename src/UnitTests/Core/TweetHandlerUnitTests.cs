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

    [Fact]
    public async Task Test_HandleTweetAsync_InvalidJson()
    {
        var (handler, _) = CreatetTweetHandler();
        await Assert.ThrowsAsync<ApplicationException>(async () => await handler.HandleTweetAsync("{\"datum\":{\"edit_history_tweet_ids\":[\"1605667332830810112\"],\"id\":\"1605667332830810112\",\"texto\":\"RT @SMTOWNGLOBAL: 2022 Winter SMTOWN : SMCU PALACE - FOREST Image #RedVelvet #WENDY\\n\\n➫ 2022.12.26 6PM (KST)\\n\\n#SMTOWN2023 #SMCU_PALACE \\n#202…\"}}"));
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

    [Theory]
    [InlineData("{\"data\":{\"edit_history_tweet_ids\":[\"1605666498172702720\"],\"id\":\"1605666498172702720\",\"text\":\"d resistance for 1  0.0Z8  ask me 1221  #娇喘 #叫床 #手冲  https://t.co/UmtiOwC4UH\"}}")]
    public async Task Test_HandleTweetAsync_WithDoubleHashtag(string tweet)
    {
        var (handler, repositoryMock) = CreatetTweetHandler();
        await handler.HandleTweetAsync(tweet);

        repositoryMock.Verify(call => call.InsertAsync(It.IsAny<string>()), Times.Exactly(1), "Call must be made once");
        repositoryMock.Verify(call => call.PersistHashValueAsync(It.IsAny<Guid>(), It.IsAny<string>()), Times.AtMost(3), "Call must be made three times");
        repositoryMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("Hace 15 años, pasé 12 horas de labor de parto tratando de sacar a #ElElfo al mundo. Ahora, almorzamos juntos y le consulto sus decisiones sobre zapatos, ropa y cosas que le gustan. Dios me ha bendecido con hijo maravilloso y me ha dado sabiduría para formar un buen muchacho.")]
    [InlineData("RT @SMTOWNGLOBAL: 2022 Winter SMTOWN : SMCU PALACE - FOREST Image #RedVelvet #WENDY\\\\n\\\\n➫ 2022.12.26 6PM (KST)\\\\n\\\\n#SMTOWN2023 #SMCU_PALACE \\\\n#202…")]
    public void Test_GetHashtags_WithTags(string content)
    {
        var (handler, _) = CreatetTweetHandler();
        var matches = handler.GetHashtags(content);

        Assert.True(matches.Count > 0);
    }

    [Fact]
    public void Test_GetHashtags_NoTags()
    {
        var (handler, _) = CreatetTweetHandler();
        var matches = handler.GetHashtags("Hello World, this is twitter.");

        Assert.True(matches.Count == 0);
    }
}