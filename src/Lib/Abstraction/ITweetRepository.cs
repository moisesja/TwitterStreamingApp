namespace TwitterStreamingLib.Abstraction;

/// <summary>
/// All supported behavior for a Tweet Persistance Layer
/// </summary>
public interface ITweetRepository
{
    Task<Guid> InsertAsync(string tweetJson);

    Task FlagTweetContainsNoTags(Guid tweetIdentifier);

    Task PersistHashValueAsync(Guid tweetIdentifier, string value);
}