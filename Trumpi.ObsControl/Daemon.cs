using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace Trumpi.ObsControl
{
    public class Daemon
    {
        private UdpClient _udp;
        private ObsAction _action;
        private bool _stopping;

        public void Start()
        {
            _action = new ObsAction();
            _udp = new UdpClient(5555);
            _udp.BeginReceive(DoWork, null);
        }

        public void Stop()
        {
            _stopping = true;
            _udp.Close();
            _action.Dispose();
        }

        private void DoWork(IAsyncResult asyncResult)
        {
            try
            {
                var remoteEndpoint = new IPEndPoint(IPAddress.Loopback, 0);
                var data = _udp.EndReceive(asyncResult, ref remoteEndpoint);
                _udp.BeginReceive(DoWork, null);

                var args = CommandLineToArgs(Encoding.UTF8.GetString(data));
                CommandLineParser.CommandLineParser parser = new CommandLineParser.CommandLineParser();
                var options = new CommandLineOptions();
                parser.ExtractArgumentAttributes(options);
                parser.ParseCommandLine(args);
                _action.DoAction(options);
            }
            catch (Exception e)
            {
                if (!_stopping)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        [DllImport("shell32.dll", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine, out int pNumArgs);

        public static string[] CommandLineToArgs(string commandLine)
        {
            var argv = CommandLineToArgvW(commandLine, out var argc);
            if (argv == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception();
            try
            {
                var args = new string[argc];
                for (var i = 0; i < args.Length; i++)
                {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p);
                }

                return args;
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }
        }
    }
}