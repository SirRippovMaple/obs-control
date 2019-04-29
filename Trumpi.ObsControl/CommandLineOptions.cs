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

        [ValueArgument(typeof(string), 'a', "animate")]
        public string Animate { get; set; }

        [ValueArgument(typeof(string), 't', "transition")]
        public string Transition { get; set; }

        [ValueArgument(typeof(string), 'f', "setflag", AllowMultiple = true)]
        public string[] SetFlag { get; set; }

        [ValueArgument(typeof(string), 'x', "unsetflag", AllowMultiple = true)]
        public string[] UnsetFlag { get; set; }

        [ValueArgument(typeof(string), 'i', "ifflag", AllowMultiple = true)]
        public string[] IfFlag { get; set; }

        [ValueArgument(typeof(string), 'j', "ifnotflag", AllowMultiple = true)]
        public string[] IfNotFlag { get; set; }
    }
}