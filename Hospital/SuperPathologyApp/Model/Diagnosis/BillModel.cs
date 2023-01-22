using SuperPathologyApp.Model.Indoor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Model.Diagnosis
{
    public class BillModel
    {
        public int RegId { get; set; }
        public int BillId { get; set; }
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string PatientName { get; set; }
        public DateTime Dob { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public double TotalAmt { get; set; }
        public double TotalLessAmt { get; set; }

        public string LessFrom { get; set; }
        public double CollAmt { get; set; }
        public double DueAmt { get; set; }
        public string Remarks { get; set; }
        public string PostedBy { get; set; }
        public double LessPc { get; set; }
        public string LessType { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryNumber { get; set; }
        public string DeliveryTimeAmPm { get; set; }
        public int BillStatus { get; set; }
  


        public DoctorModel RefDrModel { get; set; }
        public DoctorModel ConsDrModel { get; set; }
        public List<TestChartModel> TestChartModel { get; set; }
        public List<MachineModel> FinancialModel { get; set; }

        public AdmissionModel Admission { get; set; }
        public BillReturnModel BillReturn { get; set; }




      //  RefDrId, UnderDrId




    }
}
