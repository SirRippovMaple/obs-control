using Newtonsoft.Json;

namespace Trumpi.ObsControl
{
    public class SetSceneItemTransform : Message
    {
        [JsonProperty("item")]
        public string ItemName { get; set; }

        [JsonProperty("x-scale")]
        public double XScale { get; set; } = 1;

        [JsonProperty("y-scale")]
        public double YScale { get; set; } = 1;

        [JsonProperty("rotation")]
        public double Rotation { get; set; }

        public override string RequestType => "SetSceneItemTransform";
    }
}