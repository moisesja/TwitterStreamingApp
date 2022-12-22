using System;
using Microsoft.Extensions.Configuration;

namespace TwitterStreamingLib.Configuration;

public record ListenerConfiguration
{
	public ListenerConfiguration(IConfiguration configuration)
	{
        BearerToken = Environment.GetEnvironmentVariable("BEARER_TOKEN") ??
            configuration["BearerToken"];

        var retryWaitText = Environment.GetEnvironmentVariable("RETRY_WAIT") ??
            configuration["RetryWait"];

		if (string.IsNullOrWhiteSpace(retryWaitText))
		{
			throw new ApplicationException("Unable to get 'RetryWait' configuration.");
		}

		RetryWait = int.Parse(retryWaitText);
    }

	/// <summary>
	/// Authorization token
	/// </summary>
	public string BearerToken { get; init; }

	/// <summary>
	/// In seconds
	/// </summary>
	public int RetryWait { get; init; }
}