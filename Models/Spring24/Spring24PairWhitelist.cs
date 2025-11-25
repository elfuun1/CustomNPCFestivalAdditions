using CustomNPCFestivalAdditions.ModData;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFestivalAdditions.Models.Spring24
{
    public class Spring24PairWhitelist : Spring24Pair
    {
        public string UpperDancerName { get; set; }
        public string LowerDancerName { get; set; }
        public Spring24PairWhitelist()
        {
            this.Source = "";
            this.ContentID = "";
            this.UpperDancerName = "";
            this.LowerDancerName = "";
            this.IsPositionStrict = false;
            this.HasConflicts = false;
            this.Enabled = true;
        }
        public Spring24PairWhitelist(Spring24PairWhitelist pairWhitelist)
        {
            this.Source = pairWhitelist.Source;
            this.ContentID = pairWhitelist.ContentID;
            this.UpperDancerName = pairWhitelist.UpperDancerName;
            this.LowerDancerName = pairWhitelist.LowerDancerName;
            this.IsPositionStrict = pairWhitelist.IsPositionStrict;
            this.HasConflicts = pairWhitelist.HasConflicts;
            this.Enabled = pairWhitelist.Enabled;
        }
        public Spring24PairWhitelist(string source, string upperDancerName, string lowerDancerName, bool? isPositionStrict)
        {
            this.ContentID = $"Spring24PairBlacklist_{source}_{DateTime.Now.ToString()}_{upperDancerName}_{lowerDancerName}";
            this.Source = source;
            this.UpperDancerName = upperDancerName;
            this.LowerDancerName = lowerDancerName;
            this.IsPositionStrict = isPositionStrict ?? false;
            this.HasConflicts = false;
            this.Enabled = true;
        }
        public Spring24PairWhitelist(RawContent content)
        {
            if (content.ContentType == "Spring24PairWhitelist"
                && content.ContentFields.UpperDancerName != null
                && content.ContentFields.LowerDancerName != null)
            {
                this.ContentID = $"Spring24PairWhitelist_{content.ContentPack.Manifest.UniqueID.ToString()}_{content.ContentPack.Manifest.Version.ToString()}_{content.ContentFields.UpperDancerName}_{content.ContentFields.LowerDancerName}";
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
    public class ModData_Spring24PairWhitelist
    {
        public List<Spring24PairWhitelist> Data { get; set; }
        public ModData_Spring24PairWhitelist()
        {
            this.Data = new List<Spring24PairWhitelist>();
        }
    }
}
