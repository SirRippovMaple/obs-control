using CommandLineParser.Arguments;

namespace Trumpi.ObsControl
{
    public class CommandLineOptions
    {
        [ValueArgument(typeof(string), 'n', "scene")]
        public string SceneName { get; set; }
    }
}