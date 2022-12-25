using System;
using TwitterStreamingLib.Abstraction;

namespace TwitterStreamingConsole
{
	internal class ConsoleReporting
	{
		private readonly ITwitterAnalysis _twitterAnalysis;

        public ConsoleReporting(ITwitterAnalysis twitterAnalysis)
		{
			_twitterAnalysis = twitterAnalysis;
        }

		public void ReportStatistics(CancellationToken cancellationToken)
		{
            // Wait until the Streaming starts working
            Thread.Sleep(1000);

            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine($"Number of received Tweets: {_twitterAnalysis.GetTweetsCount()}");
                Console.WriteLine($"Number of Tweets with no tags: {_twitterAnalysis.GetCountOfTweetsWithNoHashtag()}");

                Console.WriteLine("Popular tags");
                var popularTags = _twitterAnalysis.GetTop10Hashtags();

                foreach (var item in popularTags)
                {
                    Console.WriteLine($"Number of appearances {item.Key}:");

                    foreach (var tag in item.Value)
                    {
                        Console.WriteLine($"\t{tag}");
                    }
                }

                // Refresh stats every second
                Thread.Sleep(1000);
            }
        }
	}
}