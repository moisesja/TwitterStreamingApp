namespace TwitterStreamingLib.Abstraction;

/// <summary>
/// All supported behavior for a Tweet Persistance Layer
/// </summary>
public interface ITweetRepository
{
    Guid Insert(string tweetJson);

    void FlagTweetContainsNoTags(Guid tweetIdentifier);

    void PersistHashValue(Guid tweetIdentifier, string value);
}