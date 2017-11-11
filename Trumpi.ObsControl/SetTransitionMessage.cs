using Newtonsoft.Json;

namespace Trumpi.ObsControl
{
    public class SetTransitionMessage : Message
    {
        [JsonProperty("transition-name")]
        public string TransitionName { get; set; }

        public override string RequestType => "SetCurrentTransition";
    }
}