using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperPathologyApp.Model
{
    public class DoctorModel
    {
        public int DrId { get; set; }
        public string  Name { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public int TakeCommision { get; set; }
        public string ReportUserName { get; set; }
        public List<SubProjectModel> SubProject{ get; set; }
        public MpoModel Mpo { get; set; }
        
    }
}