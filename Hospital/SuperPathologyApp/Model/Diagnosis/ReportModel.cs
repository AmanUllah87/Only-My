using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Model.Diagnosis
{
    class ReportModel
    {
       
        public string Specimen { get; set; }
        public ReportDoctorModel LeftDoctor { get; set; }
        public ReportDoctorModel MiddleDoctor { get; set; }
        public ReportDoctorModel RightDoctor { get; set; }
        public int MasterId { get; set; }
        public string PostedBy { get; set; }
        public List<TestParamModel> TestParam { get; set; }

    }
}
