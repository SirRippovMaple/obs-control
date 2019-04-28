using Newtonsoft.Json;

namespace Trumpi.ObsControl
{
    public class SetSourceRenderMessage : Message
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("render")]
        public bool Render { get; set; }

        [JsonProperty("scene-name")]
        public string SceneName { get; set; }

        public override string RequestType => "SetSourceRender";
    }
}