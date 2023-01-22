using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Model
{
    public class TestChartModel
    {
        public int TestId { get; set; }
        public string Name { get; set; }
        public double Charge { get; set; }
        public double Unit { get; set; }
        public double TotCharge { get; set; }
        public double LessAmt { get; set; }
        public double RtnAmt { get; set; }

        public double NetTotAmt { get; set; }
        public double GridCollAmt { get; set; }
            

        public double DefaulHonouriam{ get; set; }
        public int IsGiveDiscount { get; set; }
        public int IsVaqItem { get; set; }
        public int IsDoctorItem { get; set; }
        public int IsIndoorItem { get; set; }
        public int IsChangeCharge { get; set; }
        public int DrId { get; set; }
        public string ReportFileName { get; set; }
        public string VaqName { get; set; }
        public string GridRemarks { get; set; }

        public double HnrLess { get; set; }
        public double HnrToPay { get; set; }
        public double ReportFee { get; set; }


        public SubProjectModel SubProject { get; set; }
        public List<VacutainerModel> Vaq { get; set; }
    }
}
