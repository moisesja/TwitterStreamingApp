namespace TwitterStreamingLib.DataStructures;

/// <summary>
/// Data Record to keep track of a Hashtag and the number of instances it appears.
/// </summary>
public record HashtagCounter
{
    public HashtagCounter(string hashTag)
    {
        Hashtag = hashTag;
    }

    /// <summary>
    /// Inmutable property. 
    /// </summary>
    public string Hashtag { get; init; }

    /// <summary>
    /// This value will be incremented
    /// </summary>
    public long Instances { get; set; }
}

