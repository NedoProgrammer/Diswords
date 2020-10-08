using System;

namespace Diswords.Core
{
    public class StringComparison
    {
        public static float LevenshteinDistance(string first, string second)
        {
            char[] sa;
            int n;
            int[] p; //'previous' cost array, horizontally
            int[] d; // cost array, horizontally
            int[] _d; //placeholder to assist in swapping p and d
            sa = first.ToCharArray();
            n = sa.Length;
            p = new int[n + 1];
            d = new int[n + 1];
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

                _d = p;
                p = d;
                d = _d;
            }


            return 1.0f - (float) p[n] / Math.Max(second.Length, sa.Length);
        }
    }
}