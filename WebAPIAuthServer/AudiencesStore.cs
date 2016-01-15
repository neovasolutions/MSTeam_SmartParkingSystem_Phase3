using AuthorizationServer.Api.Entities;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Configuration;

namespace WebAPIAuthServer
{
    public static class AudiencesStore
    {
        public static ConcurrentDictionary<string, Audience> AudiencesList = new ConcurrentDictionary<string, Audience>();
        
        static AudiencesStore()
        {
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            string symmetricKeyAsBase64 = ConfigurationManager.AppSettings["as:AudienceSecret"];
            AudiencesList.TryAdd(audienceId,
                                new Audience
                                {
                                    ClientId = audienceId,
                                    Base64Secret = symmetricKeyAsBase64, 
                                                Name = "WebAPIProtResource 1" });
        }

        public static Audience AddAudience(string name)
        {
            var clientId = Guid.NewGuid().ToString("N");

            var key = new byte[32];
            RNGCryptoServiceProvider.Create().GetBytes(key);
            var base64Secret = TextEncodings.Base64Url.Encode(key);

            Audience newAudience = new Audience { ClientId = clientId, Base64Secret = base64Secret, Name = name };
            AudiencesList.TryAdd(clientId, newAudience);
            return newAudience;
        }

        public static Audience FindAudience(string clientId)
        {
            Audience audience = null;
            if (AudiencesList.TryGetValue(clientId, out audience))
            {
                return audience;
            }
            return null;
        }
    }
}