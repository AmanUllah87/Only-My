using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace SuperPathologyApp.Model
{
    class MasterSetupModel
    {
        public int UseBarcodeForSample { get; set; }
        public string ComName { get; set; }
        public string Address { get; set; }
        public double IpdSampleNo { get; set; }
        public double OpdSampleNo { get; set; }
        public double ReportNo { get; set; }
        public double HistoSampleNo { get; set; }

    }
}
