using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace Openhab.Proxy.Api.Configuration
{
    public class AppInsightsInitializer : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppInsightsInitializer(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public void Initialize(ITelemetry telemetry)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return;

            httpContext.Request.Headers.TryGetValue("Authorization", out var headerValue);
            var bearerValue = headerValue.ToString().Replace(JwtBearerDefaults.AuthenticationScheme, string.Empty, StringComparison.InvariantCultureIgnoreCase).Trim();
            telemetry.Context.User.AuthenticatedUserId = bearerValue;
            telemetry.Context.User.AccountId = bearerValue;

        }
    }
}
