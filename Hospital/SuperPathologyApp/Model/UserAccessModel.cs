using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperPathologyApp.Model
{
    public class UserAccessModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ParentName { get; set; }
        public string ChildName { get; set; }
    }
}
