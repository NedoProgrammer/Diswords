using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Linq;
using Diswords.Core.Helpers;

namespace Diswords.Core.Commands
{
    public class ShowCommands: AdvancedContext
    {
        [Command("commands")]
        public async Task PrintCommands()
        {
        	var embedBuilder = new EmbedBuilder().WithColor(Color.Orange).WithTitle(Locale.Commands).WithDescription(Locale.Help);
        	await Context.Channel.SendMessageAsync(null, false, embedBuilder.Build());
        }
    }
}
