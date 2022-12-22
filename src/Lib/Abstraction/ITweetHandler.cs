namespace TwitterStreamingLib.Abstraction;

/// <summary>
/// Supported behavior of a tweet handler
/// </summary>
public interface ITweetHandler
{
    /// <summary>
    /// Processes a streamed tweet json object
    /// </summary>
    /// <param name="tweetJson"></param>
    /// <returns></returns>
    void HandleTweet(string tweetJson);
}