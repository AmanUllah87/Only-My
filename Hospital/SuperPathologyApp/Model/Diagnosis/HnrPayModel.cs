using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Model.Diagnosis
{
    public class HnrPayModel
    {
       // RefDrId,MasterId,BillNo,BillDate,PatientName,TestName,Charge,DueAmt AS InvDue,HnrAmt,DrLess,HnrToPay,IsPaidHnr

        public int RefDrId { get; set; }
        public string RefDrName { get; set; }
        public int UnderDrId { get; set; }
        public string UnderDrName { get; set; }

        public int MasterId { get; set; }
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string PatientName { get; set; }
        public string TestName { get; set; }
        public double Charge { get; set; }
        public double InvDue { get; set; }
        public double HnrAmt { get; set; }
        public double DrLess { get; set; }
        public double HnrToPay { get; set; }
        public int IsPaidHnr { get; set; }

    }
}
