using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Model.Indoor
{
    public class BedModel
    {
        public int BedId { get; set; }
        public string Name { get; set; }
        public string Floor { get; set; }
        public string BedType { get; set; }
        public double Charge { get; set; }
        public double SrvCharge { get; set; }
        public int BookStatus { get; set; }

        public DepartmentModel Department { get; set; }
    
    
    }
}
