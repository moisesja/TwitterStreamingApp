namespace TwitterStreamingLib.Abstraction;

/// <summary>
/// 
/// </summary>
public interface ITweetHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tweetJson"></param>
    /// <returns></returns>
    Task HandleTweetAsync(string tweetJson);
}