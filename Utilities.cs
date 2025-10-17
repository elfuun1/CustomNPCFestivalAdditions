using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFestivalAdditions
{
    internal class Utilities
    {
        public static IMonitor Monitor;
        public static IModHelper Helper;
        public static ModConfig Config;

        public static void Initialize(IMonitor monitor, IModHelper helper, ModConfig config)
        {
            Monitor = monitor;
            Helper = helper;
            Config = config;
        }

        public static void Shuffle<T>(IList<T> list)
        {
            Random rnd = new Random();
            int endIndex = list.Count();

            while (endIndex > 1)
            {
                endIndex--;
                int rndIndex = rnd.Next(endIndex+1);

                T value = list[rndIndex];
                list[rndIndex] = list[endIndex];
                list[endIndex] = value;
            }
        }





    }
}
