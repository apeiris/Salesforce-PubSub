using Microsoft.Extensions.Logging;  // Ensure this is imported
using System.Runtime.CompilerServices;

public interface IAppLogger<T> {
	void LogInfo(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);
	void LogError(string message, Exception ex = null, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);
}

public class AppLogger<T> : IAppLogger<T> {
	private readonly ILogger<T> _logger;

	// Constructor takes ILogger<T> which is automatically injected by DI
	public AppLogger(ILogger<T> logger) {
		_logger = logger;
	}

	public void LogInfo(string message, string memberName = "", string filePath = "", int lineNumber = 0) {
		_logger.LogInformation("[{File}:{Line} {Member}] {Message}",
			System.IO.Path.GetFileName(filePath), lineNumber, memberName, message);
	}

	public void LogError(string message, Exception ex = null, string memberName = "", string filePath = "", int lineNumber = 0) {
		_logger.LogError(ex, "[{File}:{Line} {Member}] {Message}",
			System.IO.Path.GetFileName(filePath), lineNumber, memberName, message);
	}
}
