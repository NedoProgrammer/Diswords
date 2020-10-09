using System;

namespace Diswords.Core.Helpers
{
    /// <summary>
    ///     Class for detecting if the word is gibberish.
    ///     <para>
    ///         It compares two string using the
    ///         <see href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein Distance</see>
    ///     </para>
    /// </summary>
    public static class StringComparison
    {
        /// <summary>
        ///     The comparing method. :D
        ///     <remarks>Thank you so much, stackoverflow..</remarks>
        /// </summary>
        /// <param name="first">The string that is being compared</param>
        /// <param name="second">The string to which the first string is compared to.</param>
        /// <returns></returns>
        public static float LevenshteinDistance(string first, string second)
        {
            var sa = first.ToCharArray();
            var n = sa.Length;
            var p = new int[n + 1];
            var d = new int[n + 1];
            var m = second.Length;
            if (n == 0 || m == 0) return n == m ? 1 : 0;
            int i;
            int j;
            for (i = 0; i <= n; i++) p[i] = i;
            for (j = 1; j <= m; j++)
            {
                var tJ = second[j - 1];
                d[0] = j;
                for (i = 1; i <= n; i++)
                {
                    var cost = sa[i - 1] == tJ ? 0 : 1;
                    d[i] = Math.Min(Math.Min(d[i - 1] + 1, p[i] + 1), p[i - 1] + cost);
                }

                var d2 = p;
                p = d;
                d = d2;
            }

            return 1.0f - (float) p[n] / Math.Max(second.Length, sa.Length);
        }
    }
}