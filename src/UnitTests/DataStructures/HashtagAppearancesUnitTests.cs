using TwitterStreamingLib.DataStructures;

namespace TwitterStreamingUnitTests.DataStructures;

public class HashtagAppearancesUnitTests
{
    [Fact]
    public void AddHashtag_Test()
    {
        var hashtagAppearances = new HashtagAppearances();

        hashtagAppearances.AddHashtag(" Is ");
        hashtagAppearances.AddHashtag(" this    ");
        hashtagAppearances.AddHashtag("    the");
        hashtagAppearances.AddHashtag("end");
        hashtagAppearances.AddHashtag(" of");
        hashtagAppearances.AddHashtag("the");
        hashtagAppearances.AddHashtag(" world");
        hashtagAppearances.AddHashtag(" as   ");
        hashtagAppearances.AddHashtag(" we ");
        hashtagAppearances.AddHashtag(" know     ");
        hashtagAppearances.AddHashtag(" it");

        hashtagAppearances.AddHashtag("    Is ");
        hashtagAppearances.AddHashtag(" this    ");
        hashtagAppearances.AddHashtag("    the");
        hashtagAppearances.AddHashtag("end");
        hashtagAppearances.AddHashtag(" of");
        hashtagAppearances.AddHashtag("the");
        hashtagAppearances.AddHashtag(" world");
        hashtagAppearances.AddHashtag(" as   ");
        hashtagAppearances.AddHashtag(" we ");
        hashtagAppearances.AddHashtag(" know     ");

        hashtagAppearances.AddHashtag(" Is      ");
        hashtagAppearances.AddHashtag(" this    ");
        hashtagAppearances.AddHashtag("    the");
        hashtagAppearances.AddHashtag("end");
        hashtagAppearances.AddHashtag(" of");
        hashtagAppearances.AddHashtag("the");
        hashtagAppearances.AddHashtag(" world");
        hashtagAppearances.AddHashtag(" as   ");
        hashtagAppearances.AddHashtag(" we ");

        hashtagAppearances.AddHashtag("Is ");
        hashtagAppearances.AddHashtag(" this    ");
        hashtagAppearances.AddHashtag("    the");
        hashtagAppearances.AddHashtag("end");
        hashtagAppearances.AddHashtag(" of");
        hashtagAppearances.AddHashtag("the");
        hashtagAppearances.AddHashtag(" world");
        hashtagAppearances.AddHashtag(" as   ");

        hashtagAppearances.AddHashtag(" Is ");
        hashtagAppearances.AddHashtag(" this    ");
        hashtagAppearances.AddHashtag("    the");
        hashtagAppearances.AddHashtag("end");
        hashtagAppearances.AddHashtag(" of");
        hashtagAppearances.AddHashtag("the");
        hashtagAppearances.AddHashtag(" world");

        hashtagAppearances.AddHashtag(" Is ");
        hashtagAppearances.AddHashtag(" this    ");
        hashtagAppearances.AddHashtag("    the");
        hashtagAppearances.AddHashtag("end");
        hashtagAppearances.AddHashtag(" of");
        hashtagAppearances.AddHashtag("the");

        hashtagAppearances.AddHashtag(" Is ");
        hashtagAppearances.AddHashtag(" this    ");
        hashtagAppearances.AddHashtag("    the");
        hashtagAppearances.AddHashtag("end");
        hashtagAppearances.AddHashtag(" of");

        hashtagAppearances.AddHashtag(" Is ");
        hashtagAppearances.AddHashtag(" this    ");
        hashtagAppearances.AddHashtag("    the");
        hashtagAppearances.AddHashtag("end");

        hashtagAppearances.AddHashtag(" Is ");
        hashtagAppearances.AddHashtag(" this    ");
        hashtagAppearances.AddHashtag("    the");

        hashtagAppearances.AddHashtag(" Is ");
        hashtagAppearances.AddHashtag(" this    ");

        hashtagAppearances.AddHashtag(" Is ");

        Assert.Equal(0, hashtagAppearances.GetNumberOfAppearances("Nowordindictionary"));
        Assert.Equal(11, hashtagAppearances.GetNumberOfAppearances("Is"));
        Assert.Equal(10, hashtagAppearances.GetNumberOfAppearances("this"));
        Assert.Equal(15, hashtagAppearances.GetNumberOfAppearances("the"));
        Assert.Equal(8, hashtagAppearances.GetNumberOfAppearances("end"));
        Assert.Equal(7, hashtagAppearances.GetNumberOfAppearances("of"));
    }
}

