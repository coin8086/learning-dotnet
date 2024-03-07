//See https://learn.microsoft.com/en-us/dotnet/core/extensions/console-log-formatter
//and https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings

using Microsoft.Extensions.Logging;

using ILoggerFactory loggerFactory =
    LoggerFactory.Create(builder =>
    {
        builder.AddSimpleConsole(options =>
        {
            options.IncludeScopes = true;
            //options.SingleLine = true;
            options.TimestampFormat = "HH:mm:ss.fff ";
        });
        //builder.AddSystemdConsole(options =>
        //{
        //    options.IncludeScopes = true;
        //    options.TimestampFormat = "HH:mm:ss ";
        //});
     }
);

ILogger<Program> logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("Log out of a scope");
using (logger.BeginScope("[Log Scope A]"))
{
    logger.LogInformation("Log within a scope");

    using (logger.BeginScope("[Log Scope B]"))
    {
        logger.LogInformation("Log within a scope");
    }

    logger.LogInformation("Log within a scope");
}
logger.LogInformation("Log out of a scope");
