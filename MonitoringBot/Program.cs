using System;
using System.Runtime.CompilerServices;
using Discord;
using Discord.WebSocket;
using MonitoringBot.Util;
using System.Threading.Tasks;
using CommandLine;

namespace MonitoringBot
{
    class Program
    {


        private DiscordSocketClient client;
        private MessageMonitor monitor;
        private Options options;
        static void Main(string[] args)
        {
            new Program().MainAsync(args).GetAwaiter().GetResult();
        }

        public async Task MainAsync(string[] args)
        {
            var res = Parser.Default.ParseArguments<Options>(args);
            if (res.Tag == ParserResultType.NotParsed)
            {
                return;
            }

            options = ((Parsed<Options>)res).Value;
            client = new DiscordSocketClient();
            monitor = new MessageMonitor(client, options);
            client.Log += Log;
            client.Ready += Ready;
            client.MessageReceived += monitor.MessageReceived;
            client.MessageUpdated += monitor.MessageUpdated;
            await client.LoginAsync(TokenType.Bot, options.Token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task Ready()
        {

        }

        private async Task Log(LogMessage message)
        {
            Debug.Log(message.Message);
        }
    }
}
