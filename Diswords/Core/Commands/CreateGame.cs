using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Diswords.Core.Commands
{
    public class CreateGame : AdvancedContext
    {
        [Command("create")]
        public async Task Create()
        {
            await Context.Channel.SendMessageAsync(Locale.PleaseWait);
            if (Client.Languages.All(x => x.ShortName != Locale.Name))
                throw new Exception($"Diswords: Language {Locale.Name} was not found.");
            var game = new Game.Diswords(Context.Message.Author as IGuildUser,
                Client.Guilds.First(g => g.Id == Context.Guild.Id), Context.Channel as SocketTextChannel,
                Client.Languages.First(x => x.ShortName == Locale.Name));
            var word = game.Language.Words[new Random((int) DateTime.Now.Ticks).Next(game.Language.Words.Count)];
            game.SetWord(word, Client.Client.CurrentUser);
            Client.Games.Add(game);
            await Context.Channel.SendMessageAsync(string.Format(Locale.GameCreated, word));
        }
    }
}