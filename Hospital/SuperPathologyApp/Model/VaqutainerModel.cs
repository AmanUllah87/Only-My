using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace SuperPathologyApp.Model
{
    public class VaqutainerModel:UnitModel 
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int VaqGroupId { get; set; }
        public string VaqGroupName { get; set; }
        public string ItemId { get; set; }
        public string ItemDesc { get; set; }
        public string MasterCode { get; set; }

    }
}
