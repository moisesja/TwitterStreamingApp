using Microsoft.Extensions.Logging;
using TwitterStreamingLib.Abstraction;
using TwitterStreamingLib.DataStructures;

namespace TwitterStreamingLib.Core;

/// <summary>
/// This class violates the SOLID principle of Single Responsability as it combines functionality of what logically should be
/// handled in 2 separate modules. This is due to the fact that we're not persisting any data and thus memory management is applied.
/// I am, however, using the principle of interface segregation (and of polymorphism in the OO Programming Lingo) to consume this service
/// according to its functionality.
/// Additional Note:
/// This service must be consumed as a singleton, and thus care must be taken to avoid Data Races and allow for thread-safety
/// </summary>
public class TweetRepository : ITweetRepository, ITwitterAnalysis
{
    private long _tweetsCount = 0;
    private long _tweetsWithNoHashesCount = 0;

    private readonly HashtagAppearances _hashtagAppearances;

    /// <summary>
    /// This resource gets locked for all calls to make sure there is data corruption due to contingent threads
    /// </summary>
    private object _resouce = new();

    public TweetRepository(HashtagAppearances hashtagAppearances)
    {
        _hashtagAppearances = hashtagAppearances;
    }

    #region ITweetRepository

    /// <summary>
    /// This method follows a typical Insert Repository pattern where the method returns a record identity. In this case
    /// we simply make it up, but never use it.
    /// </summary>
    /// <param name="tweetJson"></param>
    /// <returns></returns>
    public Guid Insert(string tweetJson)
    {
        lock (_resouce)
        {
            _tweetsCount++;
            return Guid.NewGuid();
        } 
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tweetIdentifier">Not used, but declared for when data is truly persisted</param>
    /// <param name="value">Hashtag Value</param>
    public void PersistHashValue(Guid tweetIdentifier, string value)
    {
        // This method is thread-safe
        _hashtagAppearances.AddHashtag(value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tweetIdentifier">Not used, but declared for when data is truly persisted</param>
    public void FlagTweetContainsNoTags(Guid tweetIdentifier)
    {
        lock (_resouce)
        {
            _tweetsWithNoHashesCount++;
        }
    }

    #endregion

    #region ITwitterAnalysis

    public long GetTweetsCount()
    {
        lock (_resouce)
        {
            return _tweetsCount;
        }
    }

    public IDictionary<int, HashSet<string>> GetTop10Hashtags()
    {
        lock (_resouce)
        {
            if (_hashtagAppearances.GetNumberOfPositions() == 0)
            {
                return new Dictionary<int, HashSet<string>>(0);
            }

            var result = new Dictionary<int, HashSet<string>>(10);

            var node = _hashtagAppearances.GetLastPositionNode();

            var counter = 1;

            while (node != null && counter <= 10)
            {
                // We will not count those tags that appear only once as they produce way too much noise
                if (node.Value.Position > 1 && node.Value.HashTags.Count > 0)
                {
                    result.Add(node.Value.Position, node.Value.HashTags);
                    counter++;
                }
                node = node.Previous;
            }

            return result;
        }
    }

    public long GetCountOfTweetsWithNoHashtag()
    {
        lock (_resouce)
        {
            return _tweetsWithNoHashesCount;
        }
    }

    #endregion

    /*
    
    public ICollection<HashtagCount> GetTop10Hashtags()
    {
        if (_linkedHashtags.Count == 0)
        {
            return new List<HashtagCount>(0);
        }

        var rawResults = new List<HashtagCount>();

        var node = _linkedHashtags.First;

        while (node != null && node.Value.Count > 1)
        {
            rawResults.Add(node.Value);
            node = node.Next;
        }

        var results = (from htc in rawResults
                      orderby htc.Count descending
                      select htc).Take(10).ToList();

        return results;
    }
     */
}