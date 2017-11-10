using Newtonsoft.Json;

namespace Trumpi.ObsControl
{
    public class SetSceneItemPositionMessage : Message
    {
        [JsonProperty("item")]
        public string ItemName { get; set; }

        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        public override string RequestType => "SetSceneItemPosition";
    }
}