using Microsoft.Azure.ServiceBus.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.Models.Projects.EventHubSAS
{
    public static class EventHubSASToken
    {
        private static TokenProvider CreateTokenProvider(string senderKeyName, string senderKey, TimeSpan tokenTimeToLive)
        {
            return TokenProvider.CreateSharedAccessSignatureTokenProvider(senderKeyName, senderKey, tokenTimeToLive);
        }

        private static Uri CreateServiceUri(string serviceNamespace, string hubName, string publisherName)
        {
            if (string.IsNullOrEmpty(publisherName))
            {
                return CreateServiceUri(serviceNamespace, hubName);
            }
            else
            {
                return new Uri(string.Format("https://{0}.servicebus.windows.net/{1}/publishers/{2}/messages", serviceNamespace, hubName, publisherName));
            }
        }

        private static Uri CreateServiceUri(string serviceNamespace, string hubName)
        {
            return new Uri(string.Format("https://{0}.servicebus.windows.net/{1}/messages", serviceNamespace, hubName));
        }

        public static SecurityToken CreateSecurityToken(string senderKeyName, string senderKey, string serviceNamespace, string hubName, string publisherName, TimeSpan tokenTimeToLive)
        {
            TokenProvider tokenProvider = CreateTokenProvider(senderKeyName, senderKey, tokenTimeToLive);
            Uri serviceUri = CreateServiceUri(serviceNamespace, hubName, publisherName);
            return tokenProvider.GetTokenAsync(serviceUri.ToString(), tokenTimeToLive).Result;
        }

        public static SecurityToken CreateSecurityToken(string senderKeyName, string senderKey, string serviceNamespace, string hubName, TimeSpan tokenTimeToLive)
        {
            TokenProvider tokenProvider = CreateTokenProvider(senderKeyName, senderKey, tokenTimeToLive);
            Uri serviceUri = CreateServiceUri(serviceNamespace, hubName);
            return tokenProvider.GetTokenAsync(serviceUri.ToString(), tokenTimeToLive).Result;
        }

        public static async Task<SecurityToken> CreateSecurityTokenAsync(string senderKeyName, string senderKey, string serviceNamespace, string hubName, string publisherName, TimeSpan tokenTimeToLive)
        {
            TokenProvider tokenProvider = CreateTokenProvider(senderKeyName, senderKey, tokenTimeToLive);
            Uri serviceUri = CreateServiceUri(serviceNamespace, hubName, publisherName);
            return await tokenProvider.GetTokenAsync(serviceUri.ToString(), tokenTimeToLive);
        }

        public static async Task<SecurityToken> CreateSecurityTokenAsync(string senderKeyName, string senderKey, string serviceNamespace, string hubName, TimeSpan tokenTimeToLive)
        {
            TokenProvider tokenProvider = CreateTokenProvider(senderKeyName, senderKey, tokenTimeToLive);
            Uri serviceUri = CreateServiceUri(serviceNamespace, hubName);
            return await tokenProvider.GetTokenAsync(serviceUri.ToString(), tokenTimeToLive);
        }
    }
}
