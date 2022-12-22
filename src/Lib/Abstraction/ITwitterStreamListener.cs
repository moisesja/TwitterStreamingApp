namespace TwitterStreamingLib.Abstraction;

/// <summary>
/// Defines the behavior of a Twitter Stream Listener
/// </summary>
public interface ITwitterStreamListener
{
    /// <summary>
    /// Starts listening for a Stream of Twits
    /// </summary>
    Task ListenAsync();

    /// <summary>
    /// Allows for the interruption of the listener
    /// </summary>
    CancellationToken CancellationToken { get; set; }
}

