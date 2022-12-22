namespace TwitterStreamingLib.DataStructures;

/// <summary>
/// Maintains a LinkedList Node, a position, and the hashtags that have the same appearances as the position
/// </summary>
public class PositionHashes
{
	public int Position { get; init; }

	public HashSet<string> HashTags { get; init; } = new();
}