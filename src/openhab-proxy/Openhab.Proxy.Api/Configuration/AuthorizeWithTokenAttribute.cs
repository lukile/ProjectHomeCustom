using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Openhab.Proxy.Api.Configuration
{
    public class AuthorizeWithTokenAttribute : TypeFilterAttribute
    {

        public AuthorizeWithTokenAttribute() : base(typeof(AuthorizeWithAccessTokenAttribute))
        {
        }

        public class AuthorizeWithAccessTokenAttribute : ActionFilterAttribute
        {
            private readonly Dictionary<Guid, (string token, string @group)> _tokenMap = new Dictionary<Guid, (string token, string @group)>
            {
                {new Guid("4285833b-753e-4c29-a38b-a280da6250fa"), (token: "dev", group: "dev")},
                {new Guid("d608bd60-66bd-4549-8db3-781cb678eb56"), (token: "NEIZ0", group: "G1")},
                {new Guid("a4accbae-c9a1-41c1-bf50-c16ccf847dfe"), (token: "XT6M1", group: "G2")},
                {new Guid("01c9e186-c1f1-4e94-8bf5-ad86b297d9ba"), (token: "B6QBJ", group: "G3")},
                {new Guid("680fbd1d-0b27-41c7-8981-17d657f9f440"), (token: "U43FR", group: "G4")},
                {new Guid("689cfd21-dc3d-451e-8dac-2c83193e3174"), (token: "U5DWT", group: "G5")}
            };

            public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var authorizationHeader = context.HttpContext.Request.Headers.TryGetValue("Authorization", out var headerValue);
                var bearerValue = headerValue.ToString().Replace(JwtBearerDefaults.AuthenticationScheme, string.Empty, StringComparison.InvariantCultureIgnoreCase).Trim();
                if (!authorizationHeader || string.IsNullOrEmpty(bearerValue))
                {
#if DEBUG
                    bearerValue = "4285833b-753e-4c29-a38b-a280da6250fa";
#else
                    Unauthorized(context);
                    return;
#endif
                }

                if (!Guid.TryParse(bearerValue, out var token))
                {

                    Unauthorized(context);
                    return;
                }


                var tokenIsValid = _tokenMap.ContainsKey(token);
                if (!tokenIsValid)
                {
                    Unauthorized(context);
                    return;
                }

                var groupTag = _tokenMap[token];
                ConfigureControllerWithToken(context, token, groupTag);
                await base.OnActionExecutionAsync(context, next);

            }

            private static void Unauthorized(ActionExecutingContext actionContext)
            {
                actionContext.Result = new JsonResult(new { Error = "missing or invalid access token" })
                {
                    StatusCode = (int?)HttpStatusCode.Forbidden
                };
            }

            private static void ConfigureControllerWithToken(ActionExecutingContext context, Guid guid, (string token, string @group) tuple)
            {
                var (token, @group) = tuple;
                context.HttpContext.Items.Add("Token", token);
                ((ITokenController)context.Controller).Uuid = guid;
                ((ITokenController)context.Controller).Token = token;
                ((ITokenController)context.Controller).Group = @group;
            }
        }
    }
}

