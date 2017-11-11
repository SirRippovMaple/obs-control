using Newtonsoft.Json;

namespace Trumpi.ObsControl
{
    public class SetTransitionDuration : Message
    {
        [JsonProperty("duration")]
        public int Duration { get; set; }

        public override string RequestType => "SetTransitionDuration";
    }
}