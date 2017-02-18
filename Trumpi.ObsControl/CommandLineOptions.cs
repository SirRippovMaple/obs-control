using CommandLineParser.Arguments;

namespace Trumpi.ObsControl
{
    public class CommandLineOptions
    {
        [ValueArgument(typeof(string), 'n', "scene")]
        public string SceneName { get; set; }

        [ValueArgument(typeof(string), 's', "show")]
        public string ShowSource { get; set; }

        [ValueArgument(typeof(string), 'h', "hide")]
        public string HideSource { get; set; }
    }
}