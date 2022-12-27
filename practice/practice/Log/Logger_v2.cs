using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace practice.Log
{
    internal class Logger_v2
    {
        public static Serilog.ILogger InitializeSerilog()
        {
            // Verbose(0) => Debug => Information => Warning => Error => Fatal(5)

            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()                 // 로그 남기는 기준 ex) Verbose 이상 다 남겨
                .WriteTo.Console()
                .WriteTo.File("logs\\serilog.txt",      // myapp20191120 날짜 형식으로 로그 남김
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 31
                )
                .CreateLogger();

            // Json 형식으로도 로그 가능
            // 			Schema_Test dataModel = new Schema_Test() { pk = 1, id = "wolfre" };
            // 			var testValue = 35;
            // 			log.Information("Schema {@A} in {@B}", dataModel, testValue);
            // 			// 전역 설정
            // 			Log.Logger = log;
            // 			Log.Information("The Global logger!!!");
            // 
            // 			Log.CloseAndFlush();
            return logger;// Serilog.Log.Logger;
        }


        public static Serilog.ILogger Initialize(bool syncLogging, bool writeToConsole, string path, int? retainDays = null /*unlimit*/, 
            string format = "[{Timestamp:yyyy-MM-dd}]	[{Timestamp:HH:mm:ss.fff}]	[{Level:u3}]	{Message:lj}	{SourceContext}	{NewLine}{Exception}")
        {
            // format example	"[{Timestamp:yyyy-MM-dd}]	[{Timestamp:HH:mm:ss.fff}]	{SourceContext}		{Message:lj}{NewLine}{Exception}"
            //					"[{Timestamp:yyyy-MM-dd}]	[{Timestamp:HH:mm:ss.fff}]	[{Level:u3}]	{SourceContext}		{Message:lj}{NewLine}{Exception}"

            // 로그 파일 최대 사이즈 : unlimit
            // 로그 파일 갯수 retainedDays

            var loggerConfig = new LoggerConfiguration();

            //레벨 순서 : Verboxs < Debug < Information < Warning < Error < Fatal
            //미니멈레벨 이상 레벨만 사용할 수 있다
            //디폴트는 information
            loggerConfig.MinimumLevel.Verbose();

            if (writeToConsole)
            {
                if (syncLogging)
                    loggerConfig.WriteTo.Console(outputTemplate: format);
                else
                    loggerConfig.WriteTo.Async(a => a.Console(outputTemplate: format));
            }

            if (syncLogging)
                loggerConfig.WriteTo.File(path, outputTemplate: format, fileSizeLimitBytes: null, 
                    rollingInterval: RollingInterval.Day, retainedFileCountLimit: retainDays);
            else
                loggerConfig.WriteTo.Async(a => { a.File(path, outputTemplate: format, fileSizeLimitBytes: null,
                    rollingInterval: RollingInterval.Day, retainedFileCountLimit: retainDays); });


            return loggerConfig.CreateLogger();// Serilog.Log.Logger;
        }
    }
}
