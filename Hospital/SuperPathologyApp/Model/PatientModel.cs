using System;

namespace SuperPathologyApp.Model
{
    public class PatientModel
    {
        public int RegId { get; set; }
        public int PatientId { get; set; }
        public string RegNo { get; set; }
        public string Name { get; set; }
        public string Spouse { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Sex { get; set; }
        public string Age { get; set; }
        public DateTime Dob { get; set; }

    }
}
