namespace TwitterStreamingLib.Abstraction;

/// <summary>
/// Defines the behavior of a Twitter Stream Listener
/// </summary>
public interface ITwitterStreamListener
{
    /// <summary>
    /// Starts listening for a Stream of Tweets
    /// </summary>
    /// <param name="cancellationTokenSource"></param>
    /// <returns></returns>
    Task ListenAsync(CancellationTokenSource cancellationTokenSource);
}

