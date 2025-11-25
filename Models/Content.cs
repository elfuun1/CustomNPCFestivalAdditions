using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFestivalAdditions.Models
{
    public class ContentModel
    {
        public string ContentID { get; set; } = "";
        public string Source { get; set; } = "";
        public bool HasConflicts { get; set; } = false;
        public bool Enabled { get; set; } = true;

        public ContentModel()
        {
            this.ContentID = string.Empty;
            this.Source = string.Empty;
            this.HasConflicts = false;
            this.Enabled = true;
        }
    }
}
