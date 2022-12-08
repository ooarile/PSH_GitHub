
using Serilog;

namespace Serial_protocol.Protocol.AsyncSocket
{
	public class helper
	{
		public static Serilog.ILogger Initialize(bool syncLogging, bool writeToConsole, string path, int? retainDays, string format = "[{Timestamp:yyyy-MM-dd}]	[{Timestamp:HH:mm:ss.fff}]	[{Level:u3}]	{Message:lj}	{SourceContext}	{NewLine}{Exception}")
		{
			// format example	"[{Timestamp:yyyy-MM-dd}]	[{Timestamp:HH:mm:ss.fff}]	{SourceContext}		{Message:lj}{NewLine}{Exception}"
			//					"[{Timestamp:yyyy-MM-dd}]	[{Timestamp:HH:mm:ss.fff}]	[{Level:u3}]	{SourceContext}		{Message:lj}{NewLine}{Exception}"

			// 로그 파일 최대 사이즈 : unlimit
			// 로그 파일 갯수 retainedDays

			var loggerConfig = new LoggerConfiguration();

			loggerConfig.MinimumLevel.Verbose();

			if (writeToConsole)
			{
				if (syncLogging)
					loggerConfig.WriteTo.Console(outputTemplate: format);
				else
					loggerConfig.WriteTo.Async(a => a.Console(outputTemplate: format));
			}

			if (syncLogging)
				loggerConfig.WriteTo.File(path, outputTemplate: format, fileSizeLimitBytes: null, rollingInterval: RollingInterval.Day, retainedFileCountLimit: retainDays);
			else
				loggerConfig.WriteTo.Async(a => { a.File(path, outputTemplate: format, fileSizeLimitBytes: null, rollingInterval: RollingInterval.Day, retainedFileCountLimit: retainDays); });


			return loggerConfig.CreateLogger();// Serilog.Log.Logger;
		}

	}
}
