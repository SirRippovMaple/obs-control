using System;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Trumpi.ObsControl
{
    class Program
    {
        static void Main(string[] args)
        {
            global::CommandLineParser.CommandLineParser parser = new global::CommandLineParser.CommandLineParser();
            var options = new CommandLineOptions();
            parser.ExtractArgumentAttributes(options);
            parser.ParseCommandLine(args);

            using (var ws = new WebSocket("ws://localhost:4444"))
            {
                ws.Connect();

                if (!string.IsNullOrEmpty(options.SceneName))
                {
                    var message = new SwitchSceneMessage {SceneName = options.SceneName};
                    ws.Send(JsonConvert.SerializeObject(message));
                }
                if (!string.IsNullOrEmpty(options.ShowSource))
                {
                    var message = new SetSourceRenderMessage {Source = options.ShowSource, Render = true};
                    ws.Send(JsonConvert.SerializeObject(message));
                }
                if (!string.IsNullOrEmpty(options.HideSource))
                {
                    var message = new SetSourceRenderMessage {Source = options.HideSource, Render = false};
                    ws.Send(JsonConvert.SerializeObject(message));
                }
            }
        }
    }
}
