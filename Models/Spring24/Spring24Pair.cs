using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNPCFestivalAdditions.Models.Spring24
{
    public class Spring24Pair : ContentModel
    {
        public bool IsPositionStrict { get; set; } = false;
        public Spring24Pair() 
        { 
        this.IsPositionStrict = false;
        }
    }
}
