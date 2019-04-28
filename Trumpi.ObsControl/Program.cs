using System;
using Topshelf;

namespace Trumpi.ObsControl
{
    class Program
    {
        static void Main(string[] args)
        {
            
            /*CommandLineParser.CommandLineParser parser = new CommandLineParser.CommandLineParser();
            var options = new CommandLineOptions();
            parser.ExtractArgumentAttributes(options);
            parser.ParseCommandLine(args);

            if (options.Daemon)
            {*/
                var host = HostFactory.Run(c =>
                {
                    c.Service<Daemon>(s =>
                    {
                        s.ConstructUsing(sf => new Daemon());
                        s.WhenStarted(o => o.Start());
                        s.WhenStopped(o => o.Stop());
                    });
                    c.RunAsLocalSystem();
                    c.SetServiceName("obs-control");
                });

                var exitCode = (int) Convert.ChangeType(host, host.GetTypeCode());
                Environment.ExitCode = exitCode;
            /*}
            else
            {
                var action = new ObsAction();
                action.Connect();
                action.DoAction(options);
            }*/
        }
    }
}
