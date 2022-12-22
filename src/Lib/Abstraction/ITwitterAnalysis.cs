using TwitterStreamingLib.DataStructures;

namespace TwitterStreamingLib.Abstraction;

/// <summary>
/// Basic behavior of Twitter Reporting
/// </summary>
public interface ITwitterAnalysis
{
    /// <summary>
    /// Gets the count of all processed tweets
    /// </summary>
    /// <returns></returns>
    long GetTweetCount();

    /// <summary>
    /// Lists top repeating hashtags
    /// </summary>
    /// <returns></returns>
    ICollection<HashtagCounter> GetTop10Hashtags();

    /// <summary>
    /// Number of tweets where no hashtag was detected
    /// </summary>
    /// <returns></returns>
    long GetCountOfTweetsWithNoHashtag();
}