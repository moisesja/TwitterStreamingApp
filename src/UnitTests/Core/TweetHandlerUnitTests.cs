using TwitterStreamingLib.Core;

namespace TwitterStreamingUnitTests.Core;

public class TweetHandlerUnitTests
{
    [Fact]
    public async Task Test_HandleTweetAsync_Ok()
    {
        var handler = new TweetHandler();

        await handler.HandleTweetAsync("");
    }
}