using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace SuperPathologyApp.Model
{
    public class TestCodeModel:ReportingGroupModel 
    {

        public int InvDtlId { get; set; }
        public int MasterId { get; set; }
        public string TestCode { get; set; }
        public string ReportFileName { get; set; }
        public string TestName { get; set; }
        public string SampleNo { get; set; }
        public string ParameterName { get; set; }
        public string ParameterTestName{ get; set; }

        public string PtInvNo { get; set; }
        public DateTime PtInvDate { get; set; }
        public string AYear { get; set; }
        public string PtName { get; set; }
        public string PtTelephoneNo { get; set; }
        public string PtSex { get; set; }
        public string PtAge { get; set; }
        public string PtGender { get; set; }
        public string PtMobileNo { get; set; }
        public string PtType { get; set; }


        public string BedNo { get; set; }
        
        public string PCode { get; set; }
        public string DrCode { get; set; }
        public string DrName { get; set; }



        public string ParentTestCode { get; set; }
        public string ParentTestName { get; set; }

        public int GroupSlNo { get; set; }
        public int ParameterSlNo { get; set; }
        public string NormalValue { get; set; }
        public int IsBold { get; set; }
        public string Result { get; set; }

        public string ConsultantName { get; set; }
        public string ConsultantDegree { get; set; }

        public string CheckedByName { get; set; }
        public string CheckedByDegree { get; set; }

        public string LabInchargeName { get; set; }
        public string LabInchargeDegree { get; set; }
        
        
        
        
        
        public string UserName { get; set; }

        public string ChildCode { get; set; }
        public string ChildDesc { get; set; }
        public string VaqName { get; set; }

        public string SampleCollectionStatus { get; set; }
        public string SampleCollectionTime { get; set; }
        public string SampleCollectionUserName { get; set; }

        public string SampleSendStatus { get; set; }
        public string SampleSendTime { get; set; }
        public string SampleSendUserName { get; set; }



        public string CommentsInv { get; set; }
        public int PrintNo { get; set; }

        public string LabNo { get; set; }
        public string ReportNo { get; set; }

        public string SampleReceiveInLabStatus { get; set; }
        public string SampleReceiveInLabStatusTime { get; set; }
        public string SampleReceiveInLabUserName { get; set; }

        public string SampleReportProcessStatus { get; set; }
        public string SampleReportProcessTime { get; set; }
        public string SampleReportProcessUserName { get; set; }

        public string ReportPrintStatus { get; set; }
        public string ReportPrintTime { get; set; }
        public string ReportPrintUserName { get; set; }

        public string ReportReceiveInDeliveryCounterStatus { get; set; }
        public string ReportReceiveInDeliveryCounterTime { get; set; }
        public string ReportReceiveInDeliveryCounterUserName { get; set; }

        public string DeliverToPatientStatus { get; set; }
        public string DeliverToPatientTime { get; set; }
        public string DeliverToPatientUserName { get; set; }


        public string ZoneSize { get; set; }
        public string Enterpretation { get; set; }

        public string HospitalName { get; set; }
        public string FinalStatus { get; set; }
        public string PtRegNo { get; set; }



        public string Organism { get; set; }
        public string Colony { get; set; }
        public string Incubation { get; set; }
        public string SpecificTest { get; set; }
        public int IsPrint { get; set; }
        public string ManualSampleNo { get; set; }
        public string PrintBy { get; set; }
        public string ShortName { get; set; }




    }
}
