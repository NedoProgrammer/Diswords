using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Linq;
namespace Diswords.Core.Commands
{
    public class StopGame: AdvancedContext
    {
        [Command("stop")]
        public async Task Stop()
        {
            if(!Client.Games.Any(x => x.Creator.Id == Context.User.Id) && !(Context.User as IGuildUser).GuildPermissions.Administrator)
            {
                await Context.Channel.SendMessageAsync(Locale.NotEnoughPermissions);
                return;
            }
            var game = Client.Games.FirstOrDefault(x => x.Creator.Id == Context.User.Id);
            if(game != default)
                game.Stop();
        }
    }
}