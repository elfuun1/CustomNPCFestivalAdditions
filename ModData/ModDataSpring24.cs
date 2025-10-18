using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFestivalAdditions.ModData
{
    public class Spring24PairWhitelist
    {
        public IContentPack? SourceContentPack { get; }
        public string ContentID { get; set; }
        public string Source { get; set; }
        public string UpperDancerName { get; set; }
        public string LowerDancerName { get; set; }
        public bool IsPositionStrict { get; set; }
        public bool HasConflicts { get; set; }
        public bool Enabled { get; set; }
        public NPC UpperDancer { get => Game1.getCharacterFromName(UpperDancerName); }
        public NPC LowerDancer { get => Game1.getCharacterFromName(LowerDancerName); }
        public Spring24PairWhitelist(Spring24PairWhitelist pairWhitelist)
        {
            this.Source = pairWhitelist.Source;
            if (pairWhitelist.Source.Equals("ContentPack") && pairWhitelist.SourceContentPack != null)
            {
                this.SourceContentPack = pairWhitelist.SourceContentPack;
                this.ContentID = $"{pairWhitelist.SourceContentPack.Manifest.UniqueID}/{pairWhitelist.SourceContentPack.Manifest.Version.ToString()}/Spring24CharacterBlacklist/{UpperDancerName}&{LowerDancerName}";
            }
            else { this.ContentID = $"{pairWhitelist.Source}/{DateTime.Now.ToString()}/Spring24CharacterBlacklist/{UpperDancerName}&{LowerDancerName}"; }
            this.UpperDancerName = pairWhitelist.UpperDancerName;
            this.LowerDancerName = pairWhitelist.LowerDancerName;
            this.IsPositionStrict = pairWhitelist.IsPositionStrict;
            this.HasConflicts = pairWhitelist.HasConflicts;
            this.Enabled = pairWhitelist.Enabled;
        }
        public Spring24PairWhitelist(string source, string upperDancerName, string lowerDancerName, bool isPositionStrict)
        {
            ContentID = $"{source}/{DateTime.Now.ToString()}/Spring24CharacterBlacklist/{upperDancerName}&{lowerDancerName}";
            Source = source;
            UpperDancerName = upperDancerName;
            LowerDancerName = lowerDancerName;
            IsPositionStrict = isPositionStrict;
            HasConflicts = false;
            Enabled = true;
        }
        /*public Spring24CharacterWhitelist (ModData.Content content)
        {
            this.Source = "ContentPack";
            this.SourceContentPack = content.ContentPack;
            this.ContentID = $"{content.ContentPack.Manifest.UniqueID}/{content.ContentPack.Manifest.Version.ToString()}/Spring24CharacterBlacklist/{UpperDancerName}&{LowerDancerName}";
            this.UpperDancerName = content.Fields.ToString();
            this.LowerDancerName = content.Fields.ToString();
            this.IsPositionStrict = bool.Parse(content.Fields.ToString());
            this.HasConflicts = false;
            this.Enabled = true;

        } */
    }
    public class Spring24PairBlacklist
    {
        public IContentPack? SourceContentPack { get; }
        public string ContentID { get; set; }
        public string Source { get; set; }
        public string UpperDancerName { get; set; }
        public string LowerDancerName { get; set; }
        public bool IsPositionStrict { get; set; }
        public bool HasConflicts { get; set; }
        public bool Enabled { get; set; }
        public NPC UpperDancer { get => Game1.getCharacterFromName(UpperDancerName); }
        public NPC LowerDancer { get => Game1.getCharacterFromName (LowerDancerName); }
        public Spring24PairBlacklist (Spring24PairBlacklist pairBlacklist)
        {
            this.Source = pairBlacklist.Source;
            if (pairBlacklist.Source.Equals("ContentPack") && pairBlacklist.SourceContentPack != null)
            {
                this.SourceContentPack = pairBlacklist.SourceContentPack;
                this.ContentID = $"{pairBlacklist.SourceContentPack.Manifest.UniqueID}/{pairBlacklist.SourceContentPack.Manifest.Version.ToString()}/Spring24CharacterBlacklist/{UpperDancerName}&{LowerDancerName}";
            }
            else { this.ContentID = $"{pairBlacklist.Source}/{DateTime.Now.ToString()}/Spring24CharacterBlacklist/{UpperDancerName}&{LowerDancerName}"; }
            this.UpperDancerName = pairBlacklist.UpperDancerName ;
            this.LowerDancerName = pairBlacklist.LowerDancerName ;
            this.IsPositionStrict = pairBlacklist.IsPositionStrict ;
            this.HasConflicts = pairBlacklist.HasConflicts ;
            this.Enabled = pairBlacklist.Enabled ;
        }
        public Spring24PairBlacklist(string source, string upperDancerName, string lowerDancerName, bool isPositionStrict)
        {
            ContentID = $"{source}/{DateTime.Now.ToString()}/Spring24CharacterBlacklist/{upperDancerName}&{lowerDancerName}";
            Source = source;
            UpperDancerName = upperDancerName;
            LowerDancerName = lowerDancerName;
            IsPositionStrict = isPositionStrict;
            HasConflicts = false;
            Enabled = true;
        } 
    }
    public class Spring24CharacterBlacklist
    {
        public IContentPack? SourceContentPack { get; }
        public string ContentID { get; set; }
        public string Source { get; set; }
        public string CharacterName { get; set; }
        public bool HasConflicts { get; set; }
        public bool Enabled { get; set; }
        public NPC Character { get => Game1.getCharacterFromName(CharacterName); }
        public Spring24CharacterBlacklist(Spring24CharacterBlacklist charBlacklist)
        {
            if (charBlacklist.Source.Equals("ContentPack") && charBlacklist.SourceContentPack != null)
            {
                this.SourceContentPack = charBlacklist.SourceContentPack;
                this.ContentID = $"{charBlacklist.SourceContentPack.Manifest.UniqueID}/{charBlacklist.SourceContentPack.Manifest.Version.ToString()}/Spring24CharacterBlacklist/{CharacterName}";
            }
            else { this.ContentID = $"{charBlacklist.Source}/{DateTime.Now.ToString()}/Spring24CharacterBlacklist/{CharacterName}"; }
            this.Source = charBlacklist.Source;
            this.CharacterName = charBlacklist.CharacterName;
            this.HasConflicts = charBlacklist.HasConflicts;
            this.Enabled = charBlacklist.Enabled;
        }
        public Spring24CharacterBlacklist(string source, string characterName)
        {
            ContentID = $"{source}/{DateTime.Now.ToString()}/Spring24CharacterBlacklist/{characterName}";
            Source = source;
            CharacterName = characterName;
            HasConflicts = false;
            Enabled = true;
        }
    }
    public class Spring24Sprite
    {
        public IContentPack SourceContentPack { get; }
        public string Source { get; set; }
        public string ContentID { get; set; }
        public string SpritesheetName { get; set; }
        public string SpritesheetPath { get; set; }
        public bool SpriteEnabled { get; set; }
        public string CharacterName { get; set; }
        public NPC Character { get => Game1.getCharacterFromName(CharacterName);}
        public Spring24Sprite(Spring24Sprite sprite)
        {

            this.SourceContentPack = sprite.SourceContentPack;
            this.ContentID = $"{sprite.Source}/{sprite.SourceContentPack.Manifest.UniqueID}/{sprite.SourceContentPack.Manifest.Version.ToString()}/Spring24Sprite/{CharacterName}";
            this.SpritesheetName = sprite.SpritesheetName;
            this.SpritesheetPath = sprite.SpritesheetPath;
            this.SpriteEnabled = sprite.SpriteEnabled;
            this.CharacterName = sprite.CharacterName;
        }
    }
    internal class ModDataSpring24
    {
        public static IMonitor Monitor;
        public static IModHelper Helper;
        public static ModConfig Config;

        public static void Initialize(IMonitor monitor, ModConfig config, IModHelper helper)
        {
            Monitor = monitor;
            Helper = helper;
            Config = config;
        }
        public static void updateModDataSpring24PairBlackList(List<NPC> actors)
        {
            string[] pairBlackList = Config.CNFAspring24PairBlacklist.Split(", ");

            Lookup<string, string> pairBlackListLookup = (Lookup<string, string>)pairBlackList.Select(pair => pair.Split(" & ")).Where(parts => parts.Length == 2).ToLookup(parts => parts[0], parts => parts[1]);
            foreach (NPC target in actors)
            {
                if (pairBlackListLookup.Contains(target.Name))
                {
                    foreach (string blackListedChar in pairBlackListLookup[target.Name])
                    {
                        if (!ModDataGeneric.tryGetModData(target, "elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList").Split(", ").Contains($"{blackListedChar}"))
                        {
                            if (target.modData["elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList"].Equals(""))
                            {
                                target.modData[$"elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList"] = $"{blackListedChar}";
                            }
                            else
                            {
                                target.modData[$"elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList"] += $", {blackListedChar}";
                            }
                            Monitor.Log($"Appending new blacklisted pair member to {target.Name}'s ModData: {blackListedChar}");
                        }
                    }
                }
                if (!ModData.ModDataGeneric.tryGetModData(target, "elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList").Equals(""))
                {
                    Monitor.Log($"{target.Name} will not dance with the following NPCs, per blacklist: {ModDataGeneric.tryGetModData(target, "elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList")}");
                }
                continue;
            }
        }

        //Mod data reset
        public static void clearSpring24CharBlacklist(NPC target)
        {
            target.modData.Remove("elfuun.CustomNPCFestivalAdditions/isSpring24BlackListed");
        }
        public static void clearSpring24PairBlacklist(NPC target)
        {
            target.modData.Remove("elfuun.CustomNPCFestivalAdditions/Spring24PairBlackList");
        }
        public static void clearSpring24CustomSprites(NPC target)
        {
            target.modData.Remove("elfuun.CustomNPCFestivalAdditions/hasSpring24Sprite");
        }
        public static void clearAllCNFASpring24(NPC target)
        {
            clearSpring24CharBlacklist(target);
            clearSpring24PairBlacklist(target);
            clearSpring24CustomSprites(target);
        }
    }
}
