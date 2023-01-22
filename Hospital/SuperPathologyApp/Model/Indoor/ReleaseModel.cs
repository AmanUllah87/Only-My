using SuperPathologyApp.Model.Diagnosis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Model.Indoor
{
    class ReleaseModel
    {
        public int InBillId { get; set; }
        public string TrNo { get; set; }
        public DateTime TrDate { get; set; }
        public AdmissionModel Admission { get; set; }
        public PatientModel Patient { get; set; }
        public BedModel Bed { get; set; }
        public List<TestChartModel> Test { get; set; }
        public List<MachineModel> FinancialModel { get; set; }
        public double TotAmt { get; set; }
        public double TotLessAmt { get; set; }

        public string Remarks { get; set; }

        public double TotCollAmt { get; set; }
        public double TotRtnAmt { get; set; }
        public double ExtraLessAmt { get; set; }
        public double AdvanceAmt { get; set; }
        public double DueAmt { get; set; }

        public List<BillModel> Bill { get; set; }




    }
}
