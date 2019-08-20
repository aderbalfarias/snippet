using ProjectName.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProjectName.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<ConfigKeys> _appConfigKeys;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, 
            ILogger<ExceptionMiddleware> logger,
            IOptions<ConfigKeys> appConfigKeys)
        {
            _logger = logger;
            _next = next;
            _appConfigKeys = appConfigKeys;
        }

        public async Task Invoke(HttpContext httpContext, IHostingEnvironment hostingEnvironment)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                Log(httpContext, e, hostingEnvironment);
                throw;
            }
        }

        private void Log(HttpContext context, Exception exception, IHostingEnvironment hostingEnvironment)
        {
            var logsPath = _appConfigKeys.Value.LogPath;
            var now = DateTime.UtcNow;
            var fileName = $"{now.ToString("yyyyMMdd")}.log";
            var filePath = Path.Combine(logsPath, hostingEnvironment.EnvironmentName, fileName);

            _logger.LogError($"Exception logged: {now}");

            // ensure that directory exists
            new FileInfo(filePath).Directory.Create();

            using (StreamWriter outputFile = new StreamWriter(filePath, true))
            {
                outputFile.WriteLine($"{now.ToString("HH:mm:ss")} => {context.Request.Path}");
                outputFile.WriteLine(exception.Message);
                outputFile.WriteLine(Environment.NewLine);
            }
        }
    }
}
