using CustomNPCFestivalAdditions.ModData;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFestivalAdditions.Models.Spring24
{
    public class Spring24PairBlacklist
    {
        public string ContentID { get; set; }
        public string Source { get; set; }
        public string UpperDancerName { get; set; }
        public string LowerDancerName { get; set; }
        public bool IsPositionStrict { get; set; }
        public bool HasConflicts { get; set; }
        public bool Enabled { get; set; }
        public Spring24PairBlacklist()
        {
            this.Source = "";
            this.ContentID = "";
            this.UpperDancerName = "";
            this.LowerDancerName = "";
            this.IsPositionStrict = false;
            this.HasConflicts = false;
            this.Enabled = true;
        }
        public Spring24PairBlacklist(Spring24PairBlacklist pairBlacklist)
        {
            this.Source = pairBlacklist.Source;
            this.ContentID = pairBlacklist.ContentID;
            this.UpperDancerName = pairBlacklist.UpperDancerName;
            this.LowerDancerName = pairBlacklist.LowerDancerName;
            this.IsPositionStrict = pairBlacklist.IsPositionStrict;
            this.HasConflicts = pairBlacklist.HasConflicts;
            this.Enabled = pairBlacklist.Enabled;
        }
        public Spring24PairBlacklist(string source, string upperDancerName, string lowerDancerName, bool isPositionStrict)
        {
            this.Source = source;
            if (source == "ModConfig")
            {
                this.ContentID = $"ModConfig_Spring24PairBlacklist_{upperDancerName}_{lowerDancerName}";
            }
            if (source == "Terminal" || source == "GUI")
            {
                this.ContentID = $"{source}_{DateTime.Now.ToString()}_Spring24PairBlacklist_{upperDancerName}_{lowerDancerName}";
            }
            this.UpperDancerName = upperDancerName;
            this.LowerDancerName = lowerDancerName;
            this.IsPositionStrict = isPositionStrict;
            this.HasConflicts = false;
            this.Enabled = true;
        }
        public Spring24PairBlacklist(RawContent content)
        {
            if (content.ContentType == "Spring24PairBlacklist"
                && content.ContentFields.UpperDancerName != null
                && content.ContentFields.LowerDancerName != null)
            {
                this.ContentID = $"{content.ContentPack.Manifest.UniqueID}_{content.ContentPack.Manifest.Version.ToString()}_Spring24PairBlacklist_{content.ContentFields.UpperDancerName}_{content.ContentFields.LowerDancerName}";
                this.Source = "ContentPack";
                this.UpperDancerName = content.ContentFields.UpperDancerName;
                this.LowerDancerName = content.ContentFields.LowerDancerName;
                this.IsPositionStrict = content.ContentFields.IsPositionStrict ?? false;
                this.HasConflicts = false;
                this.Enabled = true;
            }
            else { throw new ArgumentNullException(); }
        }
    }

    public class ModData_Spring24PairBlacklist
    {
        public List<Models.Spring24.Spring24PairBlacklist> Data { get; set; }

        public ModData_Spring24PairBlacklist()
        {
            this.Data = new List<Spring24PairBlacklist>();
        }
    }
}
