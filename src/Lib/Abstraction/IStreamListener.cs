namespace TwitterStreamingLib.Abstraction;

/// <summary>
/// Defines the behavior of a Twitter Stream Listener
/// </summary>
public interface IStreamListener
{
    /// <summary>
    /// Starts listening for a Stream of Twits
    /// </summary>
    Task StartListeningAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task StopListeningAsync();
}

