﻿using Microsoft.Extensions.Logging;
using Polly;
using TwitterStreamingLib.Abstraction;
using TwitterStreamingLib.Configuration;

namespace TwitterStreamingLib.Core
{
    public class TwitterStreamListener : ITwitterStreamListener
    {
        private readonly ILogger<TwitterStreamListener> _logger;
        private readonly HttpClient _httpClient;
        private readonly ITweetHandler _tweetHandler;
        private readonly ListenerConfiguration _configuration;

        public TwitterStreamListener(ILogger<TwitterStreamListener> logger, HttpClient httpClient,
            ITweetHandler tweetHandler, ListenerConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;
            _tweetHandler = tweetHandler;
            _configuration = configuration;
        }

        /// <inheritdoc/>
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;

        /// <inheritdoc/>
        public async Task ListenAsync()
        {
            try
            {
                const string API_URI = "https://api.twitter.com/2/tweets/sample/stream";

                // Periodically the stream will be closed by the streaming service. This maybe caused by several reasons.
                // See https://developer.twitter.com/en/docs/twitter-api/tweets/volume-streams/integrate/handling-disconnections
                // Implementing a perpetual retry policy using Polly.
                // According to Twitter, the wait for retry should be set to 20 secs but it will be driven through Configuration
                var retryPolicy = Policy.Handle<OperationCanceledException>()
                    .WaitAndRetryForeverAsync(sleepDurationProvider: _ => TimeSpan.FromSeconds(_configuration.RetryWait));

                await retryPolicy.ExecuteAsync(async () =>
                {
                    using (var stream = await _httpClient.GetStreamAsync(API_URI, this.CancellationToken))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var line = await reader.ReadLineAsync();

                            while (!string.IsNullOrWhiteSpace(line))
                            {
                                _tweetHandler.HandleTweet(line);
                                line = await reader.ReadLineAsync();
                            }

                            throw new OperationCanceledException("Twitter has stopped the streaming connection.");
                        }
                    }
                });
            }
            catch (Exception exc)
            {
                throw new ApplicationException("Error ocurred while Listening for Twitter Streamed Data.", exc);
            }
        }

        /*
         * private readonly string _bearerToken;
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly TweetHandler _tweetHandler;

        public StreamListener(string bearerToken, TweetHandler tweetHandler)
        {
            _bearerToken = bearerToken;
            _httpClient.DefaultRequestHeaders.Authorization = new ($"Bearer", bearerToken);
            _tweetHandler = tweetHandler;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ListenAsync()
        {
            
        }
         */
    }
}

