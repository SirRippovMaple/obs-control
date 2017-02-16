using Newtonsoft.Json;

namespace Trumpi.ObsControl
{
    public class SwitchSceneMessage : Message
    {
        [JsonProperty("scene-name")]
        public string SceneName { get; set; }

        public override string RequestType => "SetCurrentScene";
    }
}