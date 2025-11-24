using CustomNPCFestivalAdditions.ModData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFestivalAdditions.Models.Spring24
{
    public class Spring24CharBlacklist
    {
        public string ContentID { get; set; }
        public string Source { get; set; }
        public string CharacterName { get; set; }
        public bool HasConflicts { get; set; }
        public bool Enabled { get; set; }

        public Spring24CharBlacklist()
        {
            this.Source = "";
            this.ContentID = "";
            this.CharacterName = "";
            this.HasConflicts = false;
            this.Enabled = true;
        }
        public Spring24CharBlacklist(Spring24CharBlacklist charBlacklist)
        {
            this.Source = charBlacklist.Source;
            this.ContentID = charBlacklist.ContentID;
            this.CharacterName = charBlacklist.CharacterName;
            this.HasConflicts = charBlacklist.HasConflicts;
            this.Enabled = charBlacklist.Enabled;
        }
        public Spring24CharBlacklist(string source, string charName)
        {
            this.Source = source;
            this.CharacterName = charName;
            this.HasConflicts = false;
            this.Enabled = true;
            if (source == "Terminal" || source == "GUI")
            {
                this.ContentID = $"Spring24CharBlacklist_{source}_{DateTime.Now.ToString()}_{charName}";
            }
            else
            {
                this.ContentID = $"Spring24CharBlacklist_{source}_{charName}";
            }    
        }
        public Spring24CharBlacklist(RawContent content)
        {
            if(content.ContentType == "Spring24CharBlacklist" 
                && content.ContentFields.CharacterName != null)
            {
                this.Source = "ContentPack";
                this.ContentID = $"Spring24CharBlacklist_{content.ContentPack.Manifest.UniqueID}_{content.ContentPack.Manifest.Version.ToString()}_{content.ContentFields.CharacterName}";
                this.CharacterName = content.ContentFields.CharacterName;
                this.HasConflicts = false;
                this.Enabled = true;
            }
            else { throw new ArgumentNullException(); }
        }
    }
    public class ModData_Spring24CharBlacklist
    {
        public List<Spring24CharBlacklist> Data { get; set; }
        public ModData_Spring24CharBlacklist() 
        {
            this.Data = new List<Spring24CharBlacklist>();
        }
    }
}
