using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Web;
using System.Net.Http.Headers;

namespace ServiAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            //config.Formatters.XmlFormatter.UseXmlSerializer = true;
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.MessageHandlers.Add(new ValidarTokenHandler());
        }

        internal class ValidarTokenHandler : DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                   CancellationToken cancellationToken)
            {
                HttpStatusCode statusCode;
                string token;

                if (!TryRetrieveToken(request, out token))
                {
                    statusCode = HttpStatusCode.Unauthorized;
                    return base.SendAsync(request, cancellationToken);
                }

                try
                {
                    var claveSecreta = ConfigurationManager.AppSettings["ClaveSecreta"];
                    var issuerToken = ConfigurationManager.AppSettings["Issuer"];
                    var audienceToken = ConfigurationManager.AppSettings["Audience"];

                    var securityKey = new SymmetricSecurityKey(
                        System.Text.Encoding.Default.GetBytes(claveSecreta));

                    SecurityToken securityToken;
                    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    TokenValidationParameters validationParameters = new TokenValidationParameters()
                    {
                        ValidAudience = audienceToken,
                        ValidIssuer = issuerToken,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        // DELEGADO PERSONALIZADO PERA COMPROBAR
                        // LA CADUCIDAD EL TOKEN.
                        LifetimeValidator = this.LifetimeValidator,
                        IssuerSigningKey = securityKey
                    };

                    // COMPRUEBA LA VALIDEZ DEL TOKEN
                    Thread.CurrentPrincipal = tokenHandler.ValidateToken(token,
                                                                         validationParameters,
                                                                         out securityToken);
                    HttpContext.Current.User = tokenHandler.ValidateToken(token,
                                                                          validationParameters,
                                                                          out securityToken);

                    return base.SendAsync(request, cancellationToken);
                }
                catch (SecurityTokenValidationException ex)
                {
                    statusCode = HttpStatusCode.Unauthorized;
                }
                catch (Exception ex)
                {
                    statusCode = HttpStatusCode.InternalServerError;
                }

                return Task<HttpResponseMessage>.Factory.StartNew(() =>
                            new HttpResponseMessage(statusCode) { });
            }

            // RECUPERA EL TOKEN DE LA PETICIÓN
            private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
            {
                token = null;
                IEnumerable<string> authzHeaders;
                if (!request.Headers.TryGetValues("Authorization", out authzHeaders) ||
                                                  authzHeaders.Count() > 1)
                {
                    return false;
                }
                var bearerToken = authzHeaders.ElementAt(0);
                token = bearerToken.StartsWith("Bearer ") ?
                        bearerToken.Substring(7) : bearerToken;
                return true;
            }

            // COMPRUEBA LA CADUCIDAD DEL TOKEN
            public bool LifetimeValidator(DateTime? notBefore,
                                          DateTime? expires,
                                          SecurityToken securityToken,
                                          TokenValidationParameters validationParameters)
            {
                var valid = false;

                if ((expires.HasValue && DateTime.UtcNow < expires)
                    && (notBefore.HasValue && DateTime.UtcNow > notBefore))
                { valid = true; }

                return valid;
            }
        }

    }
}
