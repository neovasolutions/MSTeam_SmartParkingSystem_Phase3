using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Configuration;

namespace SmartParkingSystemAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            ConfigureOAuthTokenConsumption(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            var issuer = "http://localhost:59822";
            string audienceId = "414e1927a3884f68abc79f7283837fd1";
            byte[] audienceSecret = TextEncodings.Base64Url.Decode("qMCdFDQuF23RV1Y-1Gq9L3cF3VmuFwVbam4fMTdAfpo");
            //var issuer = ConfigurationManager.AppSettings["as:TokenIssuer"];
            //string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            //byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }
    }
}