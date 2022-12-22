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
public class TwitterAnalysis : ITweetRepository, ITwitterAnalysis
{
    private readonly LinkedList<HashtagCounter> _linkedHashtags = new();
    private readonly Dictionary<string, LinkedListNode<HashtagCounter>> _dictionary = new();

    private long _tweetsCount = 0;
    private long _tweetsWithNoHashesCount = 0;

    /// <summary>
    /// This resource gets locked for all calls to make sure there is data corruption due to contingent threads
    /// </summary>
    private object _resouce = new();

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
        lock (_resouce)
        {
            if (_dictionary.ContainsKey(value))
            {
                var node = _dictionary[value];
                node.Value.Instances++;

                // 
                _linkedHashtags.Remove(node);
                _linkedHashtags.AddFirst(node);
            }
            else
            {
                var hashtagCount = new HashtagCounter(value)
                {
                    Instances = 1
                };

                var node = _linkedHashtags.AddLast(hashtagCount);
                _dictionary.Add(value, node);
            }
        }
    }

    public void FlagTweetContainsNoTags(Guid tweetIdentifier)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region ITwitterAnalysis

    public long GetCountOfTweetsWithNoHashtag()
    {
        throw new NotImplementedException();
    }

    public ICollection<HashtagCounter> GetTop10Hashtags()
    {
        throw new NotImplementedException();
    }

    public long GetTweetCount()
    {
        throw new NotImplementedException();
    }

    #endregion

    /*
     

    
    public long GetTweetCount()
    {
        return _tweetCount;
    }

    public void AddHashtag(string hashtag)
    {
        
    }

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