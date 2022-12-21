namespace TwitterStreamingLib.Abstraction;

/// <summary>
/// 
/// </summary>
public interface IStreamHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tweetJson"></param>
    /// <returns></returns>
    Task HandleTweetAsync(string tweetJson);
}