using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Beautiful
{
    internal static class ForEacher1
    {
        public static void ForEach1()
        {
            var n = 100000;
            var vs = new BigInteger[n+1];
            vs[1] = 1;
            vs[2] = 1;

            BigInteger fn(int k)
            {
                if (k <= 2)
                    return vs[k];

                BigInteger v = 0;

                var c = 13;
                for (int i = k - 1; i >= 1 && c-->0; i-=2)
                {
                    v += vs[i];
                }

                return v;
            }

            for (var i = 1; i <= n; i++)
            {
                vs[i] = fn(i);
            }

            Debug.WriteLine(vs[n].ToString()[^6..^1]);
        }
    }
}
