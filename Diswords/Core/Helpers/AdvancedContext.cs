using System;
using System.Linq;
using Discord.Commands;
using Diswords.Locales;

namespace Diswords.Core
{
    public class AdvancedContext : ModuleBase<SocketCommandContext>
    {
        private IDisposable _typing;
        protected DiswordsClient Client { get; private set; }
        protected ILocale Locale { get; private set; }

        protected override void BeforeExecute(CommandInfo command)
        {
            Client = DiswordsClient.Find(Context.Client.CurrentUser.Id);
            if (Client == default)
                throw new Exception("Diswords: Something went wrong..\nDiswordsClient was not found.");
            Locale = ILocale.Find(Client.Guilds.FirstOrDefault(x => x.Id == Context.Guild.Id)?.Language);
            _typing = Context.Channel.EnterTypingState();
            base.BeforeExecute(command);
        }

        protected override void AfterExecute(CommandInfo command)
        {
            _typing.Dispose();
            base.AfterExecute(command);
        }
    }
}