using System;
using System.Linq;
using Discord.Commands;
using Diswords.Locales;

namespace Diswords.Core.Helpers
{
    /// <summary>
    ///     Advanced Discord Context specifically for this bot.
    ///     Contains useful stuff like the client and the language.
    /// </summary>
    public class AdvancedContext : ModuleBase<SocketCommandContext>
    {
        /// <summary>
        ///     "Typing state".
        ///     <para>In Discord, it will show that the bot is "typing".</para>
        /// </summary>
        private IDisposable _typing;

        /// <summary>
        ///     Diswords Client.
        /// </summary>
        protected DiswordsClient Client { get; private set; }

        /// <summary>
        ///     Language of the client.
        /// </summary>
        protected ILocale Locale { get; private set; }

        /// <summary>
        ///     Method executed before the command.
        /// </summary>
        /// <param name="command">Discord.NET thing</param>
        /// <exception cref="Exception">Thrown if the client was not found.</exception>
        protected override void BeforeExecute(CommandInfo command)
        {
            //Find the client by it's ID
            //Diswords Client's ID is just Discord Bot's user ID.
            Client = DiswordsClient.Find(Context.Client.CurrentUser.Id);
            if (Client == default)
                throw new Exception("Diswords: Something went wrong..\nDiswordsClient was not found.");
            //Find the language of the server.
            Locale = ILocale.Find(Client.Guilds.FirstOrDefault(x => x.Id == Context.Guild.Id)?.Language);
            //Start "typing".
            _typing = Context.Channel.EnterTypingState();
            base.BeforeExecute(command);
        }

        /// <summary>
        ///     Method executed after the command.
        /// </summary>
        /// <param name="command">Discord.NET thing</param>
        protected override void AfterExecute(CommandInfo command)
        {
            //Stop "typing".
            _typing.Dispose();
            base.AfterExecute(command);
        }
    }
}