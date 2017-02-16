using System;
using Newtonsoft.Json;

namespace Trumpi.ObsControl
{
    public abstract class Message
    {
        protected Message()
        {
            MessageId = Guid.NewGuid().ToString();
        }

        [JsonProperty("request-type")]
        public abstract string RequestType { get; }

        [JsonProperty("message-id")]
        public string MessageId { get; set; }
    }
}