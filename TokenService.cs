using Microsoft.Extensions.Logging;

namespace NetUtils {
	public class TokenService {
		private readonly ILogger<TokenService> _logger;
		private readonly Func<Task<(string Token, string InstanceUrl, string ExpiresAt)>> _getAccessTokenAsync;
		private readonly object _tokenLock = new object();
		private string _cachedToken;
		private string _cachedInstanceUrl;
		private DateTimeOffset? _tokenExpiresAt;

		public TokenService(
			ILogger<TokenService> logger,
			Func<Task<(string Token, string InstanceUrl, string ExpiresAt)>> getAccessTokenAsync) {
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_getAccessTokenAsync = getAccessTokenAsync ?? throw new ArgumentNullException(nameof(getAccessTokenAsync));
		}

		/// <summary>
		/// Retrieves a valid access token and instance URL, either from cache or by fetching a new one.
		/// </summary>
		/// <param name="context">Optional context for logging (e.g., object name).</param>
		/// <returns>A tuple containing the access token and instance URL.</returns>
		/// <exception cref="InvalidOperationException">Thrown if authentication fails.</exception>
		public async Task<(string Token, string InstanceUrl)> GetTokenAsync(string context = "TokenService") {
			// Check cache for valid token
			lock (_tokenLock) {
				if (!string.IsNullOrEmpty(_cachedToken) &&
					!string.IsNullOrEmpty(_cachedInstanceUrl) &&
					(_tokenExpiresAt == null || _tokenExpiresAt > DateTimeOffset.UtcNow.AddSeconds(30))) {
					_logger.LogDebug("Using cached token for {Context}", context);
					return (_cachedToken, _cachedInstanceUrl);
				}
			}

			// Fetch new token if cache is invalid or expired
			_logger.LogDebug("Fetching new token for {Context}", context);
			var (newToken, newInstanceUrl, expiresAt) = await _getAccessTokenAsync();

			if (string.IsNullOrEmpty(newToken) || string.IsNullOrEmpty(newInstanceUrl)) {
				_logger.LogError("Invalid token or instance URL for {Context}", context);
				throw new InvalidOperationException("Authentication failed: missing token or instance URL");
			}

			// Parse expiresAt string to DateTimeOffset
			DateTimeOffset? parsedExpiresAt = null;
			if (!string.IsNullOrEmpty(expiresAt)) {
				// Try parsing as ISO 8601 timestamp
				if (DateTimeOffset.TryParse(expiresAt, out var parsedDateTime)) {
					parsedExpiresAt = parsedDateTime;
				}
				// Try parsing as seconds (duration)
				else if (double.TryParse(expiresAt, out var seconds)) {
					parsedExpiresAt = DateTimeOffset.UtcNow.AddSeconds(seconds);
				} else {
					_logger.LogWarning("Invalid expiresAt format for {Context}: {ExpiresAt}. Using default expiration.", context, expiresAt);
				}
			}

			// Update cache
			lock (_tokenLock) {
				_cachedToken = newToken;
				_cachedInstanceUrl = newInstanceUrl;
				_tokenExpiresAt = parsedExpiresAt ?? DateTimeOffset.UtcNow.AddHours(1); // Default to 1 hour if null or invalid
				_logger.LogDebug("Cached new token for {Context}, expires at {ExpiresAt}", context, _tokenExpiresAt);
			}

			return (newToken, newInstanceUrl);
		}
	}
}
