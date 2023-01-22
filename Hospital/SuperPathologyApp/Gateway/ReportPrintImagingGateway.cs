using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;

namespace SuperPathologyApp.Gateway
{
    internal class ReportPrintImagingGateway : DbConnection
    {
        public List<TestCodeModel> GetInvoiceDetailByInvNo(int masterId,string labNo)
        {
            try
            {
                string cond = "";
                if (labNo!="")
                {
                    cond="AND b.LabNo='"+ labNo +"'";
                }
                var list = new List<TestCodeModel>();
                string query = @"SELECT a.BillDate As  InvDate,a.BillNo AS  InvNo,a.PatientName,a.MobileNo,a.Sex,a.Age,b.TestId AS  Code,c.Name AS ShortDesc,
a.UnderDrId AS  DrCode,d.Name AS  DrName,c.ReportFileName As  GroupName,b.PrintStatus  AS  PrintNo,'' AS LabNo,'' AS PtStatus  
FROM tb_BILL_MASTER a LEFT JOIN tb_BILL_DETAIL b ON a.Id=b.MasterId
INNER JOIN tb_TESTCHART c ON b.TestId=c.Id
INNER JOIN tb_DOCTOR d ON a.UnderDrId=d.Id
 WHERE a.Id=" + masterId + " AND c.SubProjectId IN (12,13,14,15,16,18,19,20) ";
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestCodeModel()
                    {
                        SampleNo = rdr["LabNo"].ToString(),
                        PtInvDate = Convert.ToDateTime(rdr["InvDate"]),
                        PtInvNo = rdr["InvNo"].ToString(),
                        PtName = rdr["PatientName"].ToString(),
                        PtTelephoneNo = rdr["MobileNo"].ToString(),
                        PtSex = rdr["Sex"].ToString(),
                        PtAge = rdr["Age"].ToString(),
                        PCode = rdr["Code"].ToString(),
                        ItemDesc = rdr["ShortDesc"].ToString(),
                        DrCode = rdr["DrCode"].ToString(),
                        DrName = rdr["DrName"].ToString(),
                        GroupName = rdr["GroupName"].ToString(),
                        PrintNo = Convert.ToInt32(rdr["PrintNo"]),
                        PtType = rdr["PtStatus"].ToString(),
                    });
                }
                rdr.Close();
                ConLab.Close();
                return list;
            }
            catch (Exception )
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                throw;
            }
        }
        //public List<TestCodeModel> GetTestCodeByGroupName(string groupName, string invNo, DateTime invDate)
        public List<TestCodeModel> GetTestCodeByGroupName(string groupName, int masterId)
        {
            try
            {
                var list = new List<TestCodeModel>();
                string query = "";
                //query = @"SELECT Distinct Code,GroupName FROM tb_InvDetails WHERE GroupName='" + groupName + "' AND InvNo='" + invNo + "' AND InvDate='" + invDate.ToString("yyyy-MM-dd") + "'";
                //query = @"SELECT Distinct Code,GroupName,LabNo FROM tb_InvDetails WHERE GroupName='" + groupName + "' AND MasterId='" + masterId + "'";
                query = @"SELECT Pcode AS Code,VaqGroup AS GroupName,LabNo FROM VW_Sample_Process_Tracking WHERE  MasterId='" + masterId + "' AND VaqGroup='" + groupName + "' AND ReceiveInLabStatus='Collected' AND ReportProcessStatus='Pending'";
                
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestCodeModel()
                    {
                        TestCode = rdr["Code"].ToString(),
                        GroupName = rdr["GroupName"].ToString(),
                        SampleNo = rdr["LabNo"].ToString(),
                    });
                }
                rdr.Close();
                ConLab.Close();
                return list;
            }
            catch (Exception )
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                throw;
            }
        }
        private string GetLabNo(SqlTransaction tran, SqlConnection con)
        {
            string labNo=DateTime.Now.ToString("yy") + FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ReportNo", tran, con).PadLeft(7, '0');
            DeleteInsertLab("Update tb_MASTER_INFO SET ReportNo=ReportNo+1", tran, con);
            return labNo;
        }

        public string Save(List<TestCodeModel> mdl,SqlTransaction trans,SqlConnection con)
        {
            try
            {
                bool isFoundReportNo = false;
                string rptNo = "";
                foreach (var item in mdl)
	            {
                    rptNo = item.ReportNo == null ? "" : item.ReportNo;
                    if (rptNo!="")
	                {
                        isFoundReportNo = true;
                        goto rpt;
	                }
	            }
                rpt:
                if (isFoundReportNo == false)
                {
                    string reportNo = GetLabNo(trans, con);
                    foreach (var item in mdl)
                    {
                        item.ReportNo = reportNo;
                    }
                }
                else
                {
                    foreach (var item in mdl)
                    {
                        item.ReportNo = rptNo;
                    }
                }



                
                foreach (var model in mdl)
                {
                    if (FnSeekRecordNewLab("tb_MachineDataImagingDtls", "MasterId='" + model.MasterId + "'AND Parameter='" + model.ParameterName + "' AND LabNo='" + model.SampleNo + "' ", trans, con))
                    {
                        DeleteInsertLab("DELETE FROM tb_MachineDataImagingDtls WHERE MasterId='" + model.MasterId + "' AND Parameter='" + model.ParameterName + "' AND LabNo='" + model.SampleNo + "' ", trans, con);
                    }
                }
                DeleteInsertLab("DELETE FROM tb_MachineDataImagingMaster WHERE ReportNo='" + mdl[0].ReportNo + "' AND MasterId='" + mdl[0].MasterId + "'", trans, con);
               
                foreach (var testCodeModel in mdl)
                {
                    testCodeModel.UserName = Hlp.UserName;
                    testCodeModel.IsPrint = 1;
                }

               // ConLab.Open();
                DataTable dt = ConvertListDataTable(mdl);

                var objbulk = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, trans) { DestinationTableName = "tb_MachineDataImagingDtls" };

                objbulk.ColumnMappings.Add("MasterId", "MasterId");
                objbulk.ColumnMappings.Add("PtInvNo", "InvNo");
                objbulk.ColumnMappings.Add("PtInvDate", "InvDate");
                objbulk.ColumnMappings.Add("SampleNo", "LabNo");
                objbulk.ColumnMappings.Add("ReportNo", "ReportNo");
                objbulk.ColumnMappings.Add("TestCode", "TestCode");
                objbulk.ColumnMappings.Add("ParameterName", "Parameter");
                objbulk.ColumnMappings.Add("Result", "Result");
                objbulk.ColumnMappings.Add("ParameterSlNo", "SlNo");
                objbulk.ColumnMappings.Add("UserName", "UserName");
                objbulk.ColumnMappings.Add("IsBold", "IsBold");
                objbulk.ColumnMappings.Add("IsPrint", "IsPrint");
               
                objbulk.WriteToServer(dt);

                const string query = "INSERT INTO tb_MachineDataImagingMaster(MasterId,InvNo,Invdate,ReportNo, CheckedByName, CheckedByDegree, ConsultantName, ConsultantDegree, LabInchargeName, LabInchargeDegree, Comments,ReportHeaderName)VALUES(@MasterId,@InvNo,@Invdate,@ReportNo, @CheckedByName, @CheckedByDegree, @ConsultantName, @ConsultantDegree, @LabInchargeName, @LabInchargeDegree, @Comments,@ReportHeaderName)";
                var cmd = new SqlCommand(query, con,trans);
                cmd.Parameters.AddWithValue("@MasterId", mdl[0].MasterId);
                cmd.Parameters.AddWithValue("@InvNo", mdl[0].PtInvNo);
                cmd.Parameters.AddWithValue("@Invdate", mdl[0].PtInvDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@ReportNo", mdl[0].ReportNo);
                cmd.Parameters.AddWithValue("@CheckedByName", mdl[0].CheckedByName);
                cmd.Parameters.AddWithValue("@CheckedByDegree", mdl[0].CheckedByDegree);
                cmd.Parameters.AddWithValue("@ConsultantName", mdl[0].ConsultantName);
                cmd.Parameters.AddWithValue("@ConsultantDegree", mdl[0].ConsultantDegree);
                cmd.Parameters.AddWithValue("@LabInchargeName", mdl[0].LabInchargeName);
                cmd.Parameters.AddWithValue("@LabInchargeDegree", mdl[0].LabInchargeDegree);
                cmd.Parameters.AddWithValue("@Comments", mdl[0].CommentsInv);
                cmd.Parameters.AddWithValue("@ReportHeaderName", Hlp.ReportHeaderName);

                cmd.ExecuteNonQuery();


             //   ConLab.Close();
                return SaveSuccessMessage;


            }
            catch (Exception exception)
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                return exception.Message;
            }
        }
        public List<TestCodeModel> GetDataFromParameterDefinitionByTestCode(int masterId,string pCode,string groupName)
        {
            try
            {
                string cond = "";
                var list = new List<TestCodeModel>();
                if (pCode!="")
                {
                    cond = "AND b.Code='"+ pCode +"'";
                }
                //string query =@"select   b.Code,a.Id AS InvNo,b.LabNo,c.ParamName,c.SlNo,c.Result,c.IsBold,c.Specimen,b.ShortDEsc  From tb_Invmaster a ,tb_InvDetails b ,tb_Parameter_Definition_Imaging c  where a.Id=b.MasterId AND  b.Code=c.TestCode  and  a.Id=" + masterId + "  AND b.GroupName='" + groupName + "'  " + cond + "  AND  EXISTS (SELECT SampleNo AS LabNo FROM tb_LabSampleStatusInfo m WHERE ReceiveInLabStatus='Collected' AND m.SampleNo=b.LabNo) Order By c.SlNo";

                string query =@"select   b.Code,a.Id AS InvNo,b.LabNo,c.ParamName,c.SlNo,c.Result,c.IsBold,c.Specimen,b.ShortDEsc  From tb_Invmaster a ,tb_InvDetails b ,tb_Parameter_Definition_Imaging c  where a.Id=b.MasterId AND  b.Code=c.TestCode  and  a.Id=" + masterId + "  AND b.GroupName='" + groupName + "'  " + cond + "   Order By c.SlNo";
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestCodeModel()
                    {
                        SpecimenName = rdr["Specimen"].ToString(),
                        ParameterSlNo = Convert.ToInt32(rdr["SlNo"]),
                        ParameterName = rdr["ParamName"].ToString(),
                        Result = rdr["Result"].ToString(),
                        IsBold = Convert.ToInt32(rdr["IsBold"]),
                        TestCode = rdr["Code"].ToString(),
                        SampleNo = rdr["LabNo"].ToString(),
                        TestName = rdr["ShortDesc"].ToString(),
                    });
                }
                rdr.Close();
                ConLab.Close();
                return list;

            }
            catch (Exception )
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                throw;
            }
        }
        public TestCodeModel GetDataFromMachineDataDtls(int masterId, string testCode,string  sampleNo,string paramName)
        {
            var mdl = new TestCodeModel();
            string query = "SELECT TOP 1 Isnull(Result,'') AS Result,Isnull(ReportNo,'') AS ReportNo,Isnull(LabNo,'') AS LabNo FROM tb_MachineDataImagingDtls WHERE MasterId=" + masterId + "  AND Parameter='" + paramName + "' AND LabNo='" + sampleNo + "' ORDER BY Id desc";

            ConLab.Open();
            var cmd = new SqlCommand(query, ConLab);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                mdl.Result = rdr["Result"].ToString();
                mdl.ReportNo = rdr["ReportNo"].ToString();
                //mdl.ReportFileName = rdr["ReportFileName"].ToString();
                mdl.SampleNo = rdr["LabNo"].ToString();
            }
            rdr.Close();
            ConLab.Close();
            return mdl;
        }

        internal DataTable GetInvoiceList(DateTime date)
        {
            try
            {
                string query = @"SELECT InvNo,InvDate,PatientName,Age  FROM tb_invMASTER WHERE InvDate='"+ date.ToString("yyyy-MM-dd") +"' Order by Id Desc ";

                ConLab.Open();
                var da = new SqlDataAdapter(query, ConLab);
                var ds = new DataSet();
                da.Fill(ds, "pCode");
                var table = ds.Tables["pCode"];
                ConLab.Close();
                return table;
            }
            catch (Exception )
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                throw;
            }
        }



        internal DataTable GetCultureDefaultParameter(string paramName,string searchString)
        {
            try
            {
                string cond = "";
                if (searchString != "")
                {
                    cond += " AND Result like '%"+ searchString +"%'";
                }

                string query = @"SELECT Id,Result FROM tb_DefaultResultSetupCulture WHERE IsShow=1 AND parameter='"+ paramName +"' "+ cond +" Order by Id ";

                ConLab.Open();
                var da = new SqlDataAdapter(query, ConLab);
                var ds = new DataSet();
                da.Fill(ds, "pCode");
                var table = ds.Tables["pCode"];
                ConLab.Close();
                return table;
            }
            catch (Exception )
            {


                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }

                throw;
            }
        }

        internal DataTable GetCultureDrugName(string searchString)
        {
            try
            {
                string cond = "";
                if (searchString != "")
                {
                    cond += " AND Parameter like '%" + searchString + "%'";
                }

                string query = @"SELECT Id,Parameter FROM tb_Parameter_Definition_Microbiology WHERE 1=1 " + cond + " Order by Id ";

                ConLab.Open();
                var da = new SqlDataAdapter(query, ConLab);
                var ds = new DataSet();
                da.Fill(ds, "pCode");
                var table = ds.Tables["pCode"];
                ConLab.Close();
                return table;
            }
            catch (Exception exception)
            {


                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }

                throw;
            }
        }

     
    }
}
