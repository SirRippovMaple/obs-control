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

                var message = new SwitchSceneMessage {SceneName = options.SceneName};
                ws.Send(JsonConvert.SerializeObject(message));
            }
        }
    }
}
