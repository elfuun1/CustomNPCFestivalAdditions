using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFestivalAdditions
{
    public class ConfigurationValidator
    {
      public static bool nameInputValidator(string name)
        {
            if (string.IsNullOrEmpty(name)) 
            { return false; }
            if (!NPC.TryGetData(name, out CharacterData data).Equals(false))
            { return true; }
            else
            {  return false; }
        }

    }
}
