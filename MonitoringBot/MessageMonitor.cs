using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MonitoringBot.Util;
using System.Threading.Tasks;
using MonitoringBot.MySQL;

namespace MonitoringBot
{
    class MessageMonitor
    {
        private readonly DiscordSocketClient client;
        private readonly MySQLClient mysql;
        public MessageMonitor(DiscordSocketClient cli, Options options)
        {
            client = cli;
            mysql = new MySQLClient(options);
        }

        public async Task MessageReceived(SocketMessage message)
        {
            await InsertMessage(message, false);
        }

        public async Task MessageUpdated(Cacheable<IMessage, ulong> arg1, SocketMessage message,
            ISocketMessageChannel arg3)
        {
            await InsertMessage(message, true);
        }

        private async Task InsertMessage(SocketMessage message, bool isUpdate)
        {
            var userMessage = message as SocketUserMessage;
            var context = new CommandContext(client, userMessage);
            if (context.IsPrivate)
            {
                Debug.Log($"{(isUpdate ? "Update" : "Receive")} Direct Message\n" +
                          $"author:{message.Author.Id}:{message.Author.Username}\n" +
                          $"id:{message.Id}\n" +
                          $"text:{message.ToString()}");
                await InsertDirectMessage(userMessage, isUpdate);
            }
            else
            {
                Debug.Log($"{(isUpdate ? "Update" : "Receive")} Guild Message\n" +
                          $"guild:{context.Guild.Id}:{context.Guild.Name}\n" +
                          $"channel:{context.Channel.Id}:{context.Channel.Name}\n" +
                          $"author:{message.Author.Id}:{message.Author.Username}\n" +
                          $"id:{message.Id}\n" +
                          $"text:{message.ToString()}");
                await InsertGuildMessage(userMessage, context, isUpdate);
            }
        }

        private async Task InsertDirectMessage(SocketUserMessage message, bool isUpdate)
        {
            await mysql.InsertDirectMessage(message.Author.Id, message.Id, isUpdate, message.ToString(),
                message.Author.Id == 0 || message.Author.IsBot);
        }

        private async Task InsertGuildMessage(SocketUserMessage message, CommandContext context, bool isUpdate)
        {
            await mysql.InsertGuildMessage(context.Guild.Id, context.Channel.Id, message.Author.Id, message.Id,
                isUpdate, message.ToString(), message.Author.Id == 0 || message.Author.IsBot);
        }
    }
}
