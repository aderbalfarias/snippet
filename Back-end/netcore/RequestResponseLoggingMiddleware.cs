public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ITestService testeService)
        {
            try
            {
                var request = httpContext.Request;

                var requestResponseLog = new RequestResponseLog();
                
                if (request.Path.StartsWithSegments(new PathString("/api")))
                {
                    requestResponseLog.RequestMessage = await ReadRequestBody(request);
                    var originalBodyStream = httpContext.Response.Body;

                    using (var responseBody = new MemoryStream())
                    {
                        requestResponseLog.RequestDateTime = DateTime.Now;

                        // Execution of the request when call next
                        var response = httpContext.Response;
                        response.Body = responseBody;
                        await _next(httpContext);

                        requestResponseLog.ResponseDateTime = DateTime.Now;
                        requestResponseLog.ResponseMessage = await ReadResponseBody(response);

                        await responseBody.CopyToAsync(originalBodyStream);
                    }

                    requestResponseLog.ResponseCode = httpContext.Response.StatusCode.ToString();
                    requestResponseLog.Url = $"{httpContext.Request.Scheme}://" +
                        $"{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}";
                    requestResponseLog.Method = httpContext.Request.Method;
                    requestResponseLog.IpAddress = httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();

                    testeService.Save(data);
                }
                else
                {
                    await _next(httpContext);
                }
            }
            catch (Exception ex)
            {
                await _next(httpContext);
            }
        }

        // Simplify this
        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableRewind();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }
    }
