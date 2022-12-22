using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using TwitterStreamingLib.Abstraction;

namespace TwitterStreamingLib.Core;

public class TweetHandler : ITweetHandler
{
    private readonly ILogger<TweetHandler> _logger;
    private readonly ITweetRepository _repository;

    /// <summary>
    /// https://stackoverflow.com/questions/36895543/which-characters-are-allowed-in-hashtags
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    internal MatchCollection GetHashtags(string text)
    {
        string HASHTAG_LETTERS = @"\p{L}\p{M}";
        string HASHTAG_NUMERALS = @"\p{Nd}";
        string HASHTAG_SPECIAL_CHARS = @"_\u200c\u200d\ua67e\u05be\u05f3\u05f4\uff5e\u301c\u309b\u309c\u30a0\u30fb\u3003\u0f0b\u0f0c\u00b7";
        string HASHTAG_LETTERS_NUMERALS = HASHTAG_LETTERS + HASHTAG_NUMERALS + HASHTAG_SPECIAL_CHARS;
        string HASHTAG_LETTERS_NUMERALS_SET = "[" + HASHTAG_LETTERS_NUMERALS + "]";
        string HASHTAG_LETTERS_SET = "[" + HASHTAG_LETTERS + "]";
        var hashtagRegex = new Regex("(^|[^&" + HASHTAG_LETTERS_NUMERALS + @"])(#|\uFF03)(?!\uFE0F|\u20E3)(" + HASHTAG_LETTERS_NUMERALS_SET + "*" + HASHTAG_LETTERS_SET + HASHTAG_LETTERS_NUMERALS_SET + "*)", RegexOptions.IgnoreCase);

        return hashtagRegex.Matches(text);
    }

    public TweetHandler(ILogger<TweetHandler> logger, ITweetRepository repository)
	{
        _logger = logger;
        _repository = repository;
	}

    /// <inheritdoc/>
    public void HandleTweet(string tweetJson)
    {
        _logger.LogDebug("Handling tweet {tweetJson}", tweetJson);

        if (string.IsNullOrWhiteSpace(tweetJson))
        {
            throw new ArgumentNullException("tweetJson", "The tweetJson parameter can't be null or empty.");    
        }

        var tweetIdentifier = _repository.Insert(tweetJson);

        // Load Json object
        var tweetNode = JsonNode.Parse(tweetJson);

        var contents = tweetNode?["data"]?["text"]?.ToString();

        if (string.IsNullOrWhiteSpace(contents))
        {
            // TODO: Create a parse specific exception rather than this generic exception. 
            throw new ApplicationException("Unable to retrieve contents from the Tweet Json");
        }

        var hashtagMatches = GetHashtags(contents);

        if (hashtagMatches.Count == 0)
        {
            // Flag that no tags were found on this tweet
            _repository.FlagTweetContainsNoTags(tweetIdentifier);
        }
        else
        {
            foreach (Match match in hashtagMatches)
            {
                _repository.PersistHashValue(tweetIdentifier, match.Value);
            }
        }
    }
}