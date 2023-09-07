using Serilog.Core;
using Serilog.Events;

namespace TechnicalTest2023
{
    /// <summary>
    /// Taken from https://medium.com/@kerimemurla/how-to-create-a-serilog-enricher-that-adds-http-information-to-your-logs-1588fc83ccbb
    /// All of the logging implementation was based on this article
    /// </summary>
    public class HttpContextEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextEnricher(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext is null)
            {
                return;
            }

            var httpContextModel = new HttpContextModel
            {
                Method = httpContext.Request.Method
            };

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("HttpContext", httpContextModel, true));
        }
    }

    public class HttpContextModel
    {
        public string Method { get; init; }
    }
}
