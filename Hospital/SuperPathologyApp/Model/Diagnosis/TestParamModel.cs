using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Model.Diagnosis
{
    class TestParamModel
    {
        public int ParamId { get; set; }
        public int TestchartId { get; set; }
        public string Name { get; set; }
        public string Specimen { get; set; }

        public string MachineParam { get; set; }
        public string ReportParam { get; set; }
        public string DrugName { get; set; }
        public string ReportingGroup { get; set; }
        public string NormalRange { get; set; }
        public string Unit { get; set; }
        public int GroupSl { get; set; }
        public int ParamSl { get; set; }
        public string DefaultResult { get; set; }
        public int IsBold { get; set; }
        public double LowerVal { get; set; }
        public double UpperVal { get; set; }
        public string ReportNo { get; set; }
        public string Comment { get; set; }
        public int IsPrint { get; set; }
        public int MstId { get; set; }


    }
}
