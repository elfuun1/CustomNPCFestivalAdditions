using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;

namespace CustomNPCFestivalAdditions
{
    internal class Tokens
    {
        IMonitor Monitor;
        IModHelper Helper;
        //Spring24 related tokens
        //Dance Partner token for year
        public void createSpring24PartnerToken(NPC upperDancer, NPC? lowerDancer)
        {
            int currentYear = Game1.year;
            if (lowerDancer == null) { return; }
            if (!upperDancer.modData.ContainsKey($"elfuun.CustomNPCFestivalAdditions/Spring24Partner_Year_{currentYear}"))
            {
                upperDancer.modData[$"elfuun.CustomNPCFestivalAdditions/Spring24Partner_Year_{currentYear}"] = lowerDancer.Name;
            }
            if (lowerDancer.modData.ContainsKey($"elfuun.CustomNPCFestivalAdditions/Spring24Partner_Year_{currentYear}"))
            {
                lowerDancer.modData[$"elfuun.CustomNPCFestivalAdditions/Spring24Partner_Year_{currentYear}"] = upperDancer.Name;
            }
        }
        public void createSpring24DanceToken(string upperDancer, string? lowerDancer)
        {
            int currentYear = Game1.year;
            if (lowerDancer == null) { return; }

            NPC upper = Game1.getCharacterFromName(upperDancer);
            if (!upper.modData.ContainsKey($"elfuun.CustomNPCFestivalAdditions/Spring24Partner_Year_{currentYear}"))
            {
                upper.modData[$"elfuun.CustomNPCFestivalAdditions/Spring24Partner_Year_{currentYear}"] = lowerDancer;
            }

            NPC lower = Game1.getCharacterFromName(lowerDancer);
            if (lower.modData.ContainsKey($"elfuun.CustomNPCFestivalAdditions/Spring24Partner_Year_{currentYear}"))
            {
                lower.modData[$"elfuun.CustomNPCFestivalAdditions/Spring24Partner_Year_{currentYear}"] = upperDancer;
            }
        }
        public void createSpring24PositionToken(NPC upperDancer, NPC? lowerDancer)
        {
            int currentYear = Game1.year;
        }
    }
}
