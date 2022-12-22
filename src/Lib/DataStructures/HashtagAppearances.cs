namespace TwitterStreamingLib.DataStructures;

/// <summary>
/// Provides an optimized manipulation and storage of hashtags based on appearances in the feed.
/// This was is chosen in order to keep time complexity to a minimum, specially when trasversing a list of hashtags
/// just to increase its appearance count.
/// With this data structure we know how many times a certain group of tags appear without incurring to a linear O(n) complexity.
/// Hashsets and Dictionaries make it possible.
/// </summary>
internal class HashtagAppearances
{
    private readonly LinkedList<PositionHashes> _positionHashesLinkedList;

    private readonly Dictionary<string, LinkedListNode<PositionHashes>> _hashtagNodesDictionary;

    public HashtagAppearances()
    {
        _hashtagNodesDictionary = new();
        _positionHashesLinkedList = new();

        // Let's create the first node with position of 1
        _positionHashesLinkedList.AddFirst(new PositionHashes()
        {
            Position = 1
        });
    }

    public void AddHashtag(string hashtag)
    {
        var cleanHashtag = hashtag.Trim();

        // When this is the first time we see this tag
        if (!_hashtagNodesDictionary.ContainsKey(cleanHashtag))
        {
            // Get the first item of linklist (already created)
            var node = _positionHashesLinkedList.First;

            // Add the tag to the list of tags inside that position
            node.Value.HashTags.Add(cleanHashtag);

            // Point the dictionary entry to the position node
            _hashtagNodesDictionary.Add(cleanHashtag, node);
        }
        // We've seen this tag before
        else
        {
            // Get its position node (number of appearances)
            var currentPositionNode = _hashtagNodesDictionary[cleanHashtag];

            // Get the next position
            var nextPositionNode = currentPositionNode.Next;

            if (nextPositionNode == null)
            {
                nextPositionNode = _positionHashesLinkedList.AddLast(new PositionHashes()
                {
                    Position = currentPositionNode.Value.Position + 1
                });
            }

            // Remove from current position
            currentPositionNode.Value.HashTags.Remove(cleanHashtag);

            // Add to the next node's hashes
            nextPositionNode.Value.HashTags.Add(cleanHashtag);

            // Repoint master list
            _hashtagNodesDictionary[cleanHashtag] = nextPositionNode;
        }
    }
}