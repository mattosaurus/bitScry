using Microsoft.Azure.ServiceBus.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.Models.Projects.EventHubSAS
{
    public class EventHubSASDetails
    {
        /// <summary>
        /// The name of the Shared Access Policy in the portal
        /// </summary>
        [JsonProperty("senderKeyName")]
        [DisplayName("Sender Key Name")]
        public string SenderKeyName { get; set; }

        /// <summary>
        /// The key of the Shared Access Policy in the portal
        /// </summary>
        [JsonProperty("senderKey")]
        [DisplayName("Sender Key")]
        public string SenderKey { get; set; }

        /// <summary>
        /// Your Service Bus Namespace
        /// </summary>
        [JsonProperty("serviceNamespace")]
        [DisplayName("Service Namespace")]
        public string ServiceNamespace { get; set; }

        /// <summary>
        /// The name of your Event Hub
        /// </summary>
        [JsonProperty("hubName")]
        [DisplayName("Hub Name")]
        public string HubName { get; set; }

        /// <summary>
        /// The name of the Publisher (this is something you can make up, but should be unique for each device)
        /// </summary>
        [JsonProperty("publisherName")]
        [DisplayName("Publisher Name")]
        public string PublisherName { get; set; }

        /// <summary>
        /// Should be very long for devices that can't renew tokens and less for devices that can update their tokens
        /// </summary>
        [JsonProperty("tokenTimeToLiveSeconds")]
        [DisplayName("Token Time To Live (Seconds)")]
        public int TokenTimeToLiveSeconds { get; set; }

        /// <summary>
        /// Should be very long for devices that can't renew tokens and less for devices that can update their tokens
        /// </summary>
        [JsonProperty("tokenTimeToLive")]
        [DisplayName("Token Time To Live")]
        public TimeSpan TokenTimeToLive
        {
            get
            {
                return new TimeSpan(0, 0, TokenTimeToLiveSeconds);
            }
        }

        public SecurityToken SecurityToken { get; set; }
    }
}
