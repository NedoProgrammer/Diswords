using Discord;
using Diswords.Locales;

namespace Diswords.Core.Helpers
{
    /// <summary>
    ///     The class that helps build success/warning/error Discord embeds.
    /// </summary>
    public static class EmbedHelper
    {
        /// <summary>
        ///     Build an error embed.
        /// </summary>
        /// <param name="locale">Language of the bot (used for getting the error message name)</param>
        /// <param name="text">The description of the embed.</param>
        /// <returns>A beautiful Discord Embed</returns>
        public static Embed BuildError(ILocale locale, string text)
        {
            return new EmbedBuilder().WithColor(Color.Red).WithTitle(locale.Error).WithDescription(text).Build();
        }

        /// <summary>
        ///     Build a warning embed.
        /// </summary>
        /// <param name="locale">Language of the bot (used for getting the error message name)</param>
        /// <param name="text">The description of the embed.</param>
        /// <returns>A beautiful Discord Embed</returns>
        public static Embed BuildWarning(ILocale locale, string text)
        {
            return new EmbedBuilder().WithColor(Color.Gold).WithTitle(locale.Warning).WithDescription(text).Build();
        }

        /// <summary>
        ///     Build a success embed.
        /// </summary>
        /// <param name="locale">Language of the bot (used for getting the error message name)</param>
        /// <param name="text">The description of the embed.</param>
        /// <returns>A beautiful Discord Embed</returns>
        public static Embed BuildSuccess(ILocale locale, string text)
        {
            return new EmbedBuilder().WithColor(Color.Green).WithTitle(locale.Success).WithDescription(text).Build();
        }
    }
}