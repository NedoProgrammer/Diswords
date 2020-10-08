using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Diswords.Core.Json;
using Diswords.Locales;
using Microsoft.Extensions.DependencyInjection;

namespace Diswords
{
    public class DiswordsClient
    {
        public static List<DiswordsClient> Clients = new List<DiswordsClient>();
        private CommandService _commands;
        private IServiceProvider _services;
        public DiscordSocketClient Client;
        public List<Game.Diswords> Games = new List<Game.Diswords>();
        public List<JsonGuild> Guilds = new List<JsonGuild>();
        public List<JsonLanguage> Languages = new List<JsonLanguage>();

        public DiswordsClient(JsonConfig config, string token)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Token = token ?? throw new ArgumentNullException(nameof(token));
            Clients.Add(this);
        }

        public JsonConfig Config { get; }
        public string Token { get; }

        public static DiswordsClient Find(ulong botId)
        {
            return Clients.FirstOrDefault(client => client.Client.CurrentUser.Id == botId);
        }

        public async Task RunAsync()
        {
            Client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(_commands)
                .BuildServiceProvider();
            Client.Log += message =>
            {
                Console.WriteLine($"Diswords: {message.Message}");
                return Task.CompletedTask;
            };
            Client.JoinedGuild += guild =>
            {
                try
                {
                    var language = "";
                    if (guild.VoiceRegionId.StartsWith("us")) language = "en";
                    else if (guild.VoiceRegionId == "russia") language = "ru";
                    else language = Config.DefaultLanguage;
                    var locale = ILocale.Find(language);
                    guild.SystemChannel?.SendMessageAsync(locale.JoinedGuild);
                    var jsonGuild = new JsonGuild(language, Config.DefaultPrefix, guild.Id);
                    File.WriteAllText(
                        $"{Config.RootDirectory}/{Config.GuildsDirectoryName}{(Config.GuildsDirectoryName == "" ? "" : "/")}{guild.Id}.json",
                        jsonGuild.ToJson());
                    guild.SystemChannel?.SendMessageAsync(locale.SetupDone);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                return Task.CompletedTask;
            };
            Client.Ready += async () =>
            {
                Guilds.Clear();
                foreach (var guild in Directory.GetFiles($"{Config.RootDirectory}/{Config.GuildsDirectoryName}"))
                {
                    if (!guild.EndsWith(".json")) continue;
                    var id = Path.GetFileNameWithoutExtension(guild);
                    if (Client.Guilds.All(x => x.Id.ToString() != id)) continue;
                    Console.WriteLine($"Diswords: Loading guild {id}..");
                    try
                    {
                        Guilds.Add(JsonGuild.FromJson(await File.ReadAllTextAsync(guild)));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Diswords: Failed to load guild {id}:\n{e}");
                        throw;
                    }
                }
            };
            Client.MessageReceived += handleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _services);
            Console.WriteLine("Diswords: Logging in..");
            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();
            await Task.Delay(-1);
        }


        private async Task handleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(Client, message);
            if (message == null) return;
            if (message.Author.IsBot) return;
            var guild = getGuild(context.Guild.Id);
            if (guild == null) return;
            var argPos = 0;
            if (message.HasStringPrefix(guild.Prefix, ref argPos))
            {
                var searchResult = _commands.Search(context, argPos);
                if (searchResult.IsSuccess)
                {
                    var result = await _commands.ExecuteAsync(context, argPos, _services);
                    if (!result.IsSuccess)
                        if (result is ExecuteResult execResult)
                            await context.Channel.SendMessageAsync($"uh oh..\n```{execResult.Exception.StackTrace}```");
                }
            }
            else
            {
                if (Games.Any(g => g.Channel.Id == context.Channel.Id))
                    Games.First(g => g.Channel.Id == context.Channel.Id).CheckInput(message.Content, message.Author);
            }
        }

        private JsonGuild getGuild(ulong id)
        {
            var path =
                $"{Config.RootDirectory}/{Config.GuildsDirectoryName}{(Config.GuildsDirectoryName == "" ? "" : "/")}{id}.json";
            return !File.Exists(path) ? null : JsonGuild.FromJson(File.ReadAllText(path));
        }
    }
}