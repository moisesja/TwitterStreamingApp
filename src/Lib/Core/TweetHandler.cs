using System.Text.RegularExpressions;
using TwitterStreamingLib.Abstraction;

namespace TwitterStreamingLib.Core;

public class TweetHandler : ITweetHandler
{
    //private readonly TweeterAnalysis _tweeterAnalysis;

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

    /*
    public TweetHandler(TweeterAnalysis tweeterAnalysis)
    {
        _tweeterAnalysis = tweeterAnalysis;
    }*/

    public TweetHandler()
	{
	}

    public Task HandleTweetAsync(string tweetJson)
    {
        if (string.IsNullOrWhiteSpace(tweetJson))
        {
            throw new ArgumentNullException("tweetJson", "The tweetJson parameter can't be null or empty.");    
        }

        throw new NotImplementedException();
    }
}

