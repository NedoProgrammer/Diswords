using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Diswords.Core.Helpers;
using Diswords.Core.Json;
using Diswords.Locales;
using Microsoft.Extensions.DependencyInjection;

namespace Diswords
{
    /// <summary>
    ///     The class that will handle all the Discord commands and games.
    ///     Basically, the brain of this project.
    /// </summary>
    public class DiswordsClient
    {
        /// <summary>
        ///     List of clients created, which are automatically added to this list when the constructor is called.
        ///     <para>Useful for finding the correct client and also run multiple clients at the same time.</para>
        /// </summary>
        public static List<DiswordsClient> Clients = new List<DiswordsClient>();

        /// <summary>
        ///     A list of guilds that the client stores.
        ///     See <see cref="JsonGuild" /> to see what variables are stored in each guild.
        ///     <remarks>It is not static because each client is a different bot and each bot can have different servers.</remarks>
        /// </summary>
        public static List<JsonGuild> StaticGuilds;

        /// <summary>
        ///     A list of languages that the client stores.
        ///     See <see cref="JsonLanguage" /> to see what each language contains.
        /// </summary>
        public static List<JsonLanguage> StaticLanguages;

        /// <summary>
        ///     Variable made for Discord.NET to work properly
        ///     See <see cref="CommandService" />
        /// </summary>
        private CommandService _commands;

        /// <summary>
        ///     Variable made for Discord.NET to work properly
        ///     See <see cref="IServiceProvider" />
        /// </summary>
        private IServiceProvider _services;

        /// <summary>
        ///     Variable made for Discord.NET to work properly
        ///     See <see cref="DiscordSocketClient" />
        /// </summary>
        public DiscordSocketClient Client;

        /// <summary>
        ///     A list of games that the client stores.
        ///     <para>Will be used for handling a lot of games at the same time.</para>
        /// </summary>
        public List<Game.Diswords> Games = new List<Game.Diswords>();

        public List<JsonGuild> Guilds = new List<JsonGuild>();
        public List<JsonLanguage> Languages = new List<JsonLanguage>();

        /// <summary>
        ///     The constructor
        /// </summary>
        /// <param name="config">The config that the created bot will use.</param>
        public DiswordsClient(JsonConfig config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Clients.Add(this);
        }

        /// <summary>
        ///     The config that the bot (client) uses.
        ///     See <see cref="JsonConfig" />
        /// </summary>
        public JsonConfig Config { get; }

        /// <summary>
        ///     Method to find the bot by it's user ID.
        /// </summary>
        /// <param name="botId"></param>
        /// <returns>The first client found or null.</returns>
        public static DiswordsClient Find(ulong botId)
        {
            return Clients.FirstOrDefault(client => client.Client.CurrentUser.Id == botId);
        }

        /// <summary>
        ///     Run the bot.
        ///     <remarks>It will start an infinite loop: <c>await Task.Delay(-1)</c></remarks>
        /// </summary>
        public async Task RunAsync()
        {
            if (StaticGuilds == null || StaticLanguages == null)
            {
                StaticGuilds = new List<JsonGuild>();
                StaticLanguages = new List<JsonLanguage>();
                await LoadResources();
            }

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
            Client.JoinedGuild += async guild =>
            {
                try
                {
                    string language;
                    if (guild.VoiceRegionId.StartsWith("us")) language = "en";
                    else if (guild.VoiceRegionId == "russia") language = "ru";
                    else language = Config.DefaultLanguage;
                    var locale = ILocale.Find(language);
                    guild.SystemChannel?.SendMessageAsync(locale.JoinedGuild);
                    var jsonGuild = new JsonGuild(language, Config.DefaultPrefix, guild.Id);
                    var path =
                        $"{Config.RootDirectory}{Path.DirectorySeparatorChar}{Config.GuildsDirectoryName}{(Config.GuildsDirectoryName == "" ? "" : Path.DirectorySeparatorChar.ToString())}{Client.CurrentUser.Id}";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    await File.WriteAllTextAsync(
                        $"{Config.RootDirectory}{Path.DirectorySeparatorChar}{Config.GuildsDirectoryName}{(Config.GuildsDirectoryName == "" ? "" : Path.DirectorySeparatorChar.ToString())}{Client.CurrentUser.Id}{Path.DirectorySeparatorChar}{guild.Id}.json",
                        jsonGuild.ToJson());
                    Guilds.Add(jsonGuild);
                    guild.SystemChannel?.SendMessageAsync(locale.SetupDone);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Diswords: {e}");
                    throw;
                }
            };
            Client.LeftGuild += async guild =>
            {
                var path =
                    $"{Config.RootDirectory}{Path.DirectorySeparatorChar}{Config.GuildsDirectoryName}{(Config.GuildsDirectoryName == "" ? "" : Path.DirectorySeparatorChar.ToString())}{Client.CurrentUser.Id}";
                if (!Directory.Exists(path))
                    return;
                var jsonGuild = GetGuild(guild.Id);
                if (jsonGuild != null)
                {
                    Directory.Delete(path, true);
                }
            };
            Client.MessageReceived += HandleCommandAsync;
            Client.Ready += LoadPrivateResources;
            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), _services);
            Console.WriteLine("Diswords: Logging in..");
            await Client.LoginAsync(TokenType.Bot, Config.Token);
            await Client.StartAsync();
            await Task.Delay(-1);
        }

        /// <summary>
        ///     Load every guild and language into memory. (Lists)
        ///     <para>
        ///         <bold>There are two types of lists: static and private.</bold>
        ///     </para>
        ///     <remarks>This method loads guilds and languages into <bold>static</bold> lists.</remarks>
        ///     <para>See <see cref="LoadPrivateResources" /> for loading guilds and languages into <bold>private</bold> lists.</para>
        /// </summary>
        private async Task LoadResources()
        {
            Console.WriteLine("Diswords: Loading static resources..");
            StaticGuilds.Clear();
            StaticLanguages.Clear();
            foreach (var guild in Directory.EnumerateFiles(
                $"{Config.RootDirectory}{Path.DirectorySeparatorChar}{Config.GuildsDirectoryName}",
                "*.json", SearchOption.AllDirectories))
            {
                if (!guild.EndsWith(".json")) continue;
                var id = Path.GetFileNameWithoutExtension(guild);
                Console.WriteLine($"Diswords: Loading guild {id}..");
                try
                {
                    StaticGuilds.Add(await JsonGuild.FromJsonFile(guild));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Diswords: Failed to load guild {id}:\n{e}");
                    throw;
                }
            }

            foreach (var language in Directory.EnumerateFiles(
                $"{Config.RootDirectory}{Path.DirectorySeparatorChar}{Config.LanguagesDirectoryName}",
                "*.json", SearchOption.AllDirectories))
            {
                if (!language.EndsWith(".json")) continue;
                var languageName = Path.GetFileNameWithoutExtension(language);
                Console.WriteLine($"Diswords: Loading language {languageName}..");
                try
                {
                    StaticLanguages.Add(await JsonLanguage.FromJsonFile(language));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Diswords: Failed to load guild {languageName}:\n{e}");
                    throw;
                }
            }
        }

        private Task LoadPrivateResources()
        {
            Console.WriteLine("Diswords: Loading private resources..");
            Languages.Clear();
            Guilds.Clear();
            Languages.AddRange(StaticLanguages);
            Guilds.AddRange(StaticGuilds.Where(g => Client.Guilds.Any(g1 => g1.Id == g.Id)));
            return Task.CompletedTask;
        }

        /// <summary>
        ///     A method that is called each time the client (bot) receives a command.
        /// </summary>
        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(Client, message);
            if (message == null) return;
            if (message.Author.IsBot) return;
            var guild = GetGuild(context.Guild.Id);
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
                            await context.Channel.SendMessageAsync(null, false,
                                EmbedHelper.BuildError(ILocale.Find(guild.Language),
                                    $"```{execResult.Exception.StackTrace}```"));
                }
            }
            else
            {
                var game = Games.FirstOrDefault(g => g.Channel.Id == context.Channel.Id);
                game?.HandleInput(message);
            }
        }

        /// <summary>
        ///     Get the guild by it's ID.
        /// </summary>
        /// <param name="id">The ID of the Discord Guild.</param>
        /// <returns>First found guild or null.</returns>
        private JsonGuild GetGuild(ulong id)
        {
            return Guilds.FirstOrDefault(g => g.Id == id);
        }
    }
}