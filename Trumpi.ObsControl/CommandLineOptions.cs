using CommandLineParser.Arguments;

namespace Trumpi.ObsControl
{
    public class CommandLineOptions
    {
        [SwitchArgument('g', "getscene", false)]
        public bool GetScene { get; set; }

        [ValueArgument(typeof(string), 'n', "scene")]
        public string SceneName { get; set; }

        [ValueArgument(typeof(string), 's', "show")]
        public string ShowSource { get; set; }

        [ValueArgument(typeof(string), 'h', "hide")]
        public string HideSource { get; set; }
    }
}