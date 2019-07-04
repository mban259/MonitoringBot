using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace MonitoringBot
{
    public class Options
    {
        [Option('t', "token", Required = true)]
        public string Token { get; set; }
        [Option('s', "server", Required = true)]
        public string Server { get; set; }
        [Option('u', "user", Required = true)]
        public string User { get; set; }
        [Option('p', "password", Required = true)]
        public string Password { get; set; }
    }
}
