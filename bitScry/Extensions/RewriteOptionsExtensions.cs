using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitScry.Extensions
{
    public static class RewriteOptionsExtensions
    {
        public static RewriteOptions AddRedirectToHttpsHost(this RewriteOptions options, HostString host)
        {
            options.Rules.Add(new RedirectToHttpsHost(host));
            return options;
        }

        public class RedirectToHttpsHost : IRule
        {
            public int? SSLPort { get; set; }
            public int StatusCode { get; set; } = 302;
            public HostString Host { get; set; }

            public RedirectToHttpsHost(HostString host)
            {
                Host = host;
            }

            public RedirectToHttpsHost(HostString host, int statusCode)
            {
                Host = host;
                StatusCode = statusCode;
            }

            public RedirectToHttpsHost(HostString host, int statusCode, int? sslPort)
            {
                Host = host;
                StatusCode = statusCode;
                SSLPort = sslPort;
            }

            public virtual void ApplyRule(RewriteContext context)
            {
                if (!context.HttpContext.Request.IsHttps)
                {
                    var host = context.HttpContext.Request.Host;

                    if (Host == host)
                    {
                        if (SSLPort.HasValue && SSLPort.Value > 0)
                        {
                            // a specific SSL port is specified
                            host = new HostString(host.Host, SSLPort.Value);
                        }
                        else
                        {
                            // clear the port
                            host = new HostString(host.Host);
                        }

                        var req = context.HttpContext.Request;
                        var newUrl = new StringBuilder().Append("https://").Append(host).Append(req.PathBase).Append(req.Path).Append(req.QueryString);
                        var response = context.HttpContext.Response;
                        response.StatusCode = StatusCode;
                        response.Headers[HeaderNames.Location] = newUrl.ToString();
                        context.Result = RuleResult.EndResponse;
                    }
                }
            }
        }
    }
}
