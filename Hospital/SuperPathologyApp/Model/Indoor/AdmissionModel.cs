using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Model.Indoor
{
    public class AdmissionModel
    {
        public int AdmId { get; set; }
        public string AdmNo { get; set; }
        public DateTime AdmDate { get; set; }
        public string AdmTime { get; set; }
        public string EmergencyContact { get; set; }
        
        public string ChiefComplain { get; set; }
        

        public BedModel Bed { get; set; }
        public PatientModel Patient { get; set; }
        public DoctorModel RefDoctor { get; set; }
        public DoctorModel UnderDoctor { get; set; }
        public double AdmissionCharge { get; set; }
        public double AdvanceAmt { get; set; }
        public string AdvanceAmtTrNo { get; set; }
        public DepartmentModel Department { get; set; }

    }
}
