using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
                ReplaySubject<dynamic> s = new ReplaySubject<dynamic>();
                ws.OnMessage += (o, eventArgs) =>
                {
                    s.OnNext(JsonConvert.DeserializeObject(eventArgs.Data));
                };

                if (options.GetScene)
                {
                    var message = new GetSceneMessage();
                    ws.Send(JsonConvert.SerializeObject(message));
                    var result = s
                        .FirstAsync(x => x["message-id"] == message.MessageId)
                        .Timeout(TimeSpan.FromSeconds(5))
                        .Wait();
                    Console.WriteLine(result["name"]);
                    Environment.SetEnvironmentVariable("ObsScene", (string) result["name"], EnvironmentVariableTarget.User);
                }
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
