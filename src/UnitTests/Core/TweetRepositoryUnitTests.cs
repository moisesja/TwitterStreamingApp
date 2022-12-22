using TwitterStreamingLib.Core;

namespace TwitterStreamingUnitTests.Core;

public class TweetRepositoryUnitTests
{
    [Fact]
    public void test()
    {
        var repository = new TweetRepository(null);
        repository.FlagTweetContainsNoTags(Guid.Empty);
    }

    // TODO: Add Unit Testing
}

