using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Trumpi.ObsControl
{
    public class ObsAction : IDisposable
    {
        private readonly WebSocket _ws = new WebSocket("ws://localhost:4444");
        private readonly ReplaySubject<dynamic> _s = new ReplaySubject<dynamic>();
        private readonly Dictionary<string, bool> _flags = new Dictionary<string, bool>();

        public void Connect()
        {
            _ws.Connect();
            _ws.OnMessage += (o, eventArgs) =>
            {
                _s.OnNext(JsonConvert.DeserializeObject(eventArgs.Data));
            };
        }

        public void DoAction(CommandLineOptions options)
        {
            if (!_ws.IsAlive)
            {
                _ws.Connect();
            }

            foreach (var flagName in options.SetFlag)
            {
                _flags[flagName] = true;
            }

            foreach (var flagName in options.UnsetFlag)
            {
                _flags[flagName] = false;
            }

            foreach (var flagName in options.IfFlag)
            {
                if (!_flags.ContainsKey(flagName) || _flags[flagName] == false)
                {
                    return;
                }
            }

            foreach (var flagName in options.IfNotFlag)
            {
                if (_flags.ContainsKey(flagName) && _flags[flagName])
                {
                    return;
                }
            }

            if (!string.IsNullOrEmpty(options.Animate))
            {
                var components = options.Animate.Split(';');
                TimeSpan duration = TimeSpan.FromSeconds(int.Parse(components[0]));
                var animations = components.Skip(1).Select(x => new Animation(x.Split(','))).ToArray();
                Stopwatch timer = new Stopwatch();
                timer.Start();

                double percentage;

                do
                {
                    var elapsedTime = timer.ElapsedMilliseconds;
                    percentage = Math.Min(1, elapsedTime / duration.TotalMilliseconds);
                    foreach (var animation in animations)
                    {
                        var currentY = (animation.EndY - animation.StartY) * percentage + animation.StartY;
                        var currentX = (animation.EndX - animation.StartX) * percentage + animation.StartX;
                        var currentRotation = (animation.EndRotation - animation.StartRotation) * percentage + animation.StartRotation;
                        var currentYScale = (animation.EndYScale - animation.StartYScale) * percentage + animation.StartYScale;
                        var currentXScale = (animation.EndXScale - animation.StartXScale) * percentage + animation.StartXScale;

                        var message = new SetSceneItemPositionMessage
                        {
                            ItemName = animation.ItemName,
                            X = currentX,
                            Y = currentY
                        };
                        var transformMessage = new SetSceneItemTransform
                        {
                            ItemName = animation.ItemName,
                            Rotation = currentRotation,
                            YScale = currentYScale,
                            XScale = currentXScale
                        };

                        _ws.Send(JsonConvert.SerializeObject(message));
                        _ws.Send(JsonConvert.SerializeObject(transformMessage));
                    }
                    Thread.Sleep(1000 / 60);
                } while (percentage < 1);
                return;
            }
            if (options.GetScene)
            {
                var message = new GetSceneMessage();
                _ws.Send(JsonConvert.SerializeObject(message));
                var result = WaitForResponse(_s, message);
                Console.WriteLine(result["name"]);
                Environment.SetEnvironmentVariable("ObsScene", (string)result["name"], EnvironmentVariableTarget.User);
            }
            else if (!string.IsNullOrEmpty(options.ShowSource))
            {
                var message = new SetSourceRenderMessage { Source = options.ShowSource, Render = true, SceneName = options.SceneName };
                _ws.Send(JsonConvert.SerializeObject(message));
            }
            else if (!string.IsNullOrEmpty(options.HideSource))
            {
                var message = new SetSourceRenderMessage { Source = options.HideSource, Render = false, SceneName = options.SceneName };
                _ws.Send(JsonConvert.SerializeObject(message));
            }
            else if (!string.IsNullOrEmpty(options.SceneName))
            {
                var message = new SwitchSceneMessage { SceneName = options.SceneName };
                _ws.Send(JsonConvert.SerializeObject(message));
            }
            else if (!string.IsNullOrEmpty(options.Transition))
            {
                var components = options.Transition.Split(';');
                var transitionMessage = new SetTransitionMessage { TransitionName = components[0] };
                _ws.Send(JsonConvert.SerializeObject(transitionMessage));
                var transitionDurationMessage = new SetTransitionDuration { Duration = int.Parse(components[1]) };
                _ws.Send(JsonConvert.SerializeObject(transitionDurationMessage));
            }
        }

        private static dynamic WaitForResponse(ReplaySubject<dynamic> s, Message message)
        {
            var result = s
                .FirstAsync(x => x["message-id"] == message.MessageId)
                .Timeout(TimeSpan.FromSeconds(5))
                .Wait();
            return result;
        }

        public void Dispose()
        {
            ((IDisposable) _ws)?.Dispose();
            _s?.Dispose();
        }
    }
}