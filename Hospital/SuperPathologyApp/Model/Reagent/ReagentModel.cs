using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Model.Reagent
{
    public class ReagentModel
    {
        public int ReagentId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string DeptName { get; set; }
        public string LotNo { get; set; }

        public int ReorderLevel { get; set; }
        public double Price { get; set; }
        public double Qty { get; set; }
        
        public int IsExpire { get; set; }
        public DateTime ExpireDate { get; set; }




    }
}
