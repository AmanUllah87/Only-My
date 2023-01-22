using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace SuperPathologyApp.Model
{
    public class ReportingGroupModel:SpecimenModel 
    {
        public int ReportingGroupId { get; set; }
        public string ReportingGroupName { get; set; }
    }
}
