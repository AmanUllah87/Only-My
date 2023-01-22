using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
 
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using SuperPathologyApp.UI;

namespace SuperPathologyApp.Gateway
{
    class SampleSearchGateway : DbConnection
    {
        internal List<TestCodeModel> GetListByParam(string testCode, string status,string sampleNo,string ptName,string invoiceNo,DateTime dateFrom,DateTime dateTo)
        {
            return new List<TestCodeModel>();
//            try
//            {
//                var list = new List<TestCodeModel>();
//                string query = "",cond="";


//                if (sampleNo!="")
//                {
//                    cond += " AND LabNo='"+ sampleNo +"'";
//                }
//                if (ptName != "")
//                {
//                    cond += " AND PatientName like '%"+ ptName +"%'";
//                }
//                if (invoiceNo != "")
//                {
//                    cond += " AND InvNo ='"+ invoiceNo +"'";
//                }

//                string statusString = "";
//                switch (status)
//                {
//                    case "Pending":
//                        statusString = " AND CollStatus='Pending'";
//                        break;
//                    case "Collected":
//                        statusString = " AND CollStatus='Collected'";
//                        break;
//                    case "ReceiveInLab":
//                        statusString = " AND ReceiveInLabStatus='Collected'";
//                        break;
//                    case "Print":
//                        statusString = " AND ReportPrintStatus='Collected'";
//                        break;
//                    case "Process":
//                        statusString = " AND ReportProcessStatus='Collected'";
//                        break;
//                    case "DeliveryToCounter":
//                        statusString = " AND ReportDeliveryToCounterStatus='Collected'";
//                        break;
//                    case "DeliveryToPatient":
//                        statusString = " AND DeliverToPatientStatus='Collected'";
//                        break;

//                }




//                query = @"SELECT  MasterId, InvNo, InvDate, LabNo, PatientName, Age, Sex, ShortDesc, CollStatus, CollTime, CollUser, 
//                        ReceiveInLabStatus, ReceiveInLabTime, ReceiveInLabUser, 
//                        ReportProcessStatus, ReportProcessTime, ReportProcessUser, ReportPrintStatus, ReportPrintTime, ReportPrintUser, ReportDeliveryToCounterStatus, ReportDeliveryToCounterTime, ReportDeliveryToCounterUser, DeliverToPatientStatus, DeliverToPatientTime, DeliverToPatientUser FROM VW_Sample_Process_Tracking WHERE InvDate Between '" + dateFrom.ToString("yyyy-MM-dd") + "' AND '" + dateTo.ToString("yyyy-MM-dd") + "' " + statusString + " ";
                
                
//                ConLab.Open();
//                var cmd = new SqlCommand(query, ConLab);
//                var rdr = cmd.ExecuteReader();
//                while (rdr.Read())
//                {
//                    list.Add(new TestCodeModel()
//                    {
//                        PtInvNo = rdr["InvNo"].ToString(),
//                        PtInvDate = Convert.ToDateTime(rdr["InvDate"].ToString()),
//                        LabNo = rdr["LabNo"].ToString(),
//                        PtName = rdr["PatientName"].ToString(),
//                        PtAge = rdr["Age"].ToString(),
//                        PtSex = rdr["Sex"].ToString(),
//                        ItemDesc = rdr["ShortDesc"].ToString(),

//                        SampleCollectionStatus = rdr["CollStatus"].ToString(),
//                        SampleCollectionUserName = rdr["CollUser"].ToString(),
//                        SampleCollectionTime = rdr["CollTime"].ToString(),

//                        SampleReceiveInLabStatus = rdr["ReceiveInLabStatus"].ToString(),
//                        SampleReceiveInLabStatusTime = rdr["ReceiveInLabTime"].ToString(),
//                        SampleReceiveInLabUserName = rdr["ReceiveInLabUser"].ToString(),

//                        SampleReportProcessStatus = rdr["ReportProcessStatus"].ToString(),
//                        SampleReportProcessTime = rdr["ReportProcessTime"].ToString(),
//                        SampleReportProcessUserName = rdr["ReportProcessUser"].ToString(),

//                        ReportPrintStatus = rdr["ReportPrintStatus"].ToString(),
//                        ReportPrintTime = rdr["ReportPrintTime"].ToString(),
//                        ReportPrintUserName = rdr["ReportPrintUser"].ToString(),

//                       // ReportDeliveryToCounterStatus = rdr["ReportDeliveryToCounterStatus"].ToString(),
//                       // ReportDeliveryToCounterTime = rdr["ReportDeliveryToCounterTime"].ToString(),
//                       // ReportDeliveryToCounterUserName = rdr["ReportDeliveryToCounterUser"].ToString(),
                        
//                        DeliverToPatientStatus = rdr["DeliverToPatientStatus"].ToString(),
//                        DeliverToPatientTime = rdr["DeliverToPatientTime"].ToString(),
//                        DeliverToPatientUserName = rdr["DeliverToPatientUser"].ToString(),
                    
//                    });
//                }
//                rdr.Close();
//                ConLab.Close();
//                return list;
//            }
//            catch (Exception exception)
//            {
//                if (ConLab.State == ConnectionState.Open)
//                {
//                    ConLab.Close();
//                }
//                throw;
//            }
        }
    }
}
