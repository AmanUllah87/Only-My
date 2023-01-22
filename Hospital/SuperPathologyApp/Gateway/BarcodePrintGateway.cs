using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Emit;
using SuperPathologyApp.Gateway.DB_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using SuperPathologyApp.Model;
using SuperPathologyApp.UI;

namespace SuperPathologyApp.Gateway
{
    class BarcodePrintGateway:DbConnection
    {
        private SqlTransaction _trans;
        public string TransferDataFromOtherDbToLabDbByInvNoAndDate(int masterId)
        {
            try
            {
                string invNo = FncReturnFielValueLab("tb_BILL_MASTER", "Id=" + masterId + "", "BillNO");
                var data = GeetOtherSoftwareData(masterId);
                
                string vaqNotConfigure = "";
                foreach (var testCodeModel in data)
                {
                    string code = testCodeModel.TestCode;

                    if (FnSeekRecordNewLab("tb_TESTCHART", "Id='" + testCodeModel.TestCode + "' AND VaqName=''"))
                    {
                        if (vaqNotConfigure == "")
                        {
                            vaqNotConfigure = code;
                        }
                        else
                        {
                            vaqNotConfigure += "," + code;
                        }
                        testCodeModel.TestCode = "";
                    }


                }
                if (vaqNotConfigure!="")
                {
                    Hlp.Message = "Code Need To Be Configured: " + vaqNotConfigure;    
                }

                data.RemoveAll(r => r.TestCode == "");
                data.RemoveAll(r => r.TestCode == null);
                if (data.Count<1)
                {
                    return "No Data Found For Print";
                }

               
               


                ConLab.Open();
                _trans = ConLab.BeginTransaction();


                foreach (var mdl in data)
                {

                    string reportFileName = FncReturnFielValueLab("tb_TESTCHART", "Id ='" + mdl.TestCode + "'", "ReportFileName", _trans, ConLab);
                    string vaqName = FncReturnFielValueLab("tb_TESTCHART", "Id ='" + mdl.TestCode + "'", "VaqName", _trans, ConLab);
                    if (FnSeekRecordNewLab("tb_LAB_STRICKER_INFO", "MasterId=" + masterId + " AND TestId='" + mdl.TestCode + "'", _trans, ConLab) == false)
                    {
                        InsertLabSampleInvestigation(invNo,masterId,"",mdl.TestCode,reportFileName,vaqName,reportFileName,_trans,ConLab);
                    }
                }
                var dataForLabSampleInvestigation = GetGroupwiseDataFromLabInvDetail(masterId,_trans,ConLab);
                int additionalSampleNo = 1;

                foreach (var mdl in dataForLabSampleInvestigation)
                {
                    string labNo = invNo + Hlp.GetCharacterByNo(additionalSampleNo); ;
                    DeleteInsertLab("Update tb_LAB_STRICKER_INFO  SET SampleNo='" + labNo + "' WHERE MasterId='" + masterId + "'  AND  VaqName='" + mdl.VaqName + "'", _trans);
                    additionalSampleNo += 1;
                }

                DeleteInsertLab("Update tb_BILL_DETAIL  SET SampleNO=b.SampleNO FROM tb_BILL_DETAIL  a INNER JOIN tb_LAB_STRICKER_INFO b ON a.MasterId=b.MasterId AND a.TestId=b.TestId AND  a.MasterId='" + masterId + "'  ", _trans, ConLab);

                _trans.Commit();
                ConLab.Close();
                return "Success";

            }
            catch (Exception exception)
            {
                _trans.Rollback();
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                return exception.Message;
            }
        }

       
        private IEnumerable<TestCodeModel> GetGroupwiseDataFromLabInvDetail(int masterId,SqlTransaction trans,SqlConnection con)
        {
            try
            {
                var list = new List<TestCodeModel>();
               // string query = @"SELECT VaqName,MasterId FROM tb_LAB_STRICKER_INFO WHERE MasterId ='" + masterId + "' AND VaqName Not In('IMAGING')   Group By MasterId,VaqName Order By VaqName";
                string query = @"SELECT VaqName,MasterId FROM tb_LAB_STRICKER_INFO WHERE MasterId ='" + masterId + "'    Group By MasterId,VaqName Order By VaqName";
                //ConLab.Open();
                var cmd = new SqlCommand(query, con,trans);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestCodeModel()
                    {
                        VaqName = rdr["VaqName"].ToString(),
                        //TestCode = rdr["TestId"].ToString(),
                    });
                }
                rdr.Close();
                //ConLab.Close();




                return list;

            }
            catch (Exception )
            {
               
                throw;
            }
        }
     
        public List<TestCodeModel> GeetOtherSoftwareData(int masterId)
        {
            try
            {
                var list = new List<TestCodeModel>();
                string query = @"SELECT * FROM V_StrickerData WHERE MasterId= " + masterId + " ";
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestCodeModel()
                    {
                        TestCode = rdr["TestId"].ToString(),
                        TestName = rdr["TestName"].ToString(),
                    });
                }
                rdr.Close();
                ConLab.Close();

                return list;

            }
            catch (Exception exception)
            {
                if (ConOther.State == ConnectionState.Open)
                {
                    ConOther.Close();
                }
                throw;
            }
        }
        public int InsertInvMaster(string invNo,DateTime invDate,string ptName,string ptAge,string ptSex,string ptMobileNo,string drCode,string drName,string bedNo,string ptStatus,string ptId,SqlTransaction trans,SqlConnection con )
        {
            try
            {
                const string query = @"INSERT INTO tb_InvMaster (InvNo, InvDate, PatientName, Age, Sex, MobileNo, DrCode, DrName, BedNo, PtStatus, PtYear, EntryTime,PatientId) OUTPUT INSERTED.ID VALUES (@InvNo, @InvDate, @PatientName, @Age, @Sex, @MobileNo, @DrCode, @DrName, @BedNo, @PtStatus, @PtYear, @EntryTime,@PatientId)";
                var cmd = new SqlCommand(query, con,trans);
                cmd.Parameters.AddWithValue("@InvNo", invNo);
                cmd.Parameters.AddWithValue("@InvDate", invDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PatientName", ptName);
                cmd.Parameters.AddWithValue("@Age", ptAge);
                cmd.Parameters.AddWithValue("@Sex", ptSex);
                cmd.Parameters.AddWithValue("@MobileNo", ptMobileNo);
                cmd.Parameters.AddWithValue("@DrCode", drCode);
                cmd.Parameters.AddWithValue("@DrName", drName);
                cmd.Parameters.AddWithValue("@BedNo", bedNo);
                cmd.Parameters.AddWithValue("@PtStatus", ptStatus);
                cmd.Parameters.AddWithValue("@PtYear", invDate.ToString("yyyy"));
                cmd.Parameters.AddWithValue("@EntryTime", Hlp.GetServerDate(con,trans).ToString("F"));
                cmd.Parameters.AddWithValue("@PatientId", ptId);

                


                //cmd.ExecuteNonQuery();
                var masterId = (int) cmd.ExecuteScalar();
                return masterId;

            }
            catch (Exception )
            {
                return 0;
            }
        }
        public void InsertInvDetails(int masterId, string code, string shortDesc, string groupName,string vaqName, string labNo, SqlTransaction trans, SqlConnection con)
        {
            try
            {
                const string query = @"INSERT INTO tb_InvDetails (MasterId,Code, ShortDesc, GroupName, LabNo, PtYear,VaqName) VALUES (@MasterId, @Code, @ShortDesc, @GroupName, @LabNo, @PtYear,@VaqName)";
                var cmd = new SqlCommand(query, con, trans);
                cmd.Parameters.AddWithValue("@MasterId", masterId);
                cmd.Parameters.AddWithValue("@Code", code);
                cmd.Parameters.AddWithValue("@ShortDesc", shortDesc);
                cmd.Parameters.AddWithValue("@GroupName", groupName);
                cmd.Parameters.AddWithValue("@LabNo", labNo);
                cmd.Parameters.AddWithValue("@PtYear", Hlp.GetServerDate(con,trans).ToString("yyyy"));
                cmd.Parameters.AddWithValue("@VaqName", vaqName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception )
            {

                throw;
            }
        }
        public void InsertLabSampleInvestigation(string invNo, int masterId, string sampleNo, string testId, string reportFileName, string vaqName, string groupName, SqlTransaction trans, SqlConnection con)
        {

          


            try
            {

                const string query = @"INSERT INTO tb_LAB_STRICKER_INFO (MasterId,InvNo, SampleNo, TestId,  ReportFileName, VaqName,   HOSTNAME) VALUES (@MasterId,@InvNo, @SampleNo, @testId,  @ReportFileName, @VaqName,  @HOSTNAME)";
                var cmd = new SqlCommand(query, con, trans);
                cmd.Parameters.AddWithValue("@MasterId", masterId);
                cmd.Parameters.AddWithValue("@SampleNo", sampleNo);
                cmd.Parameters.AddWithValue("@TestId", testId);
                cmd.Parameters.AddWithValue("@ReportFileName", reportFileName);
                cmd.Parameters.AddWithValue("@VaqName", vaqName);

                cmd.Parameters.AddWithValue("@HOSTNAME", Environment.MachineName);
                cmd.Parameters.AddWithValue("@InvNo", invNo);

                cmd.ExecuteNonQuery();
            }
            catch (Exception exception)
            {

                throw;
            }
        }
        internal List<TestCodeModel> GetDataFromLabTable(string invNo, DateTime invDate)
        {
            try
            {
                var list = new List<TestCodeModel>();
                string query = @"SELECT Labno,Shortdesc FROM tb_InvDetails WHERE InvNo='" + invNo + "' AND InvDate='" + invDate.ToString("yyyy-MM-dd") + "'";
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestCodeModel()
                    {
                        SampleNo = rdr["Labno"].ToString(),
                        TestName = rdr["Shortdesc"].ToString(),
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
        internal List<TestCodeModel>  GetDataFromLabSmapleInvestigation(int masterId)
        {
            try
            {
                var list = new List<TestCodeModel>();
                string query = @"SELECT a.MasterId,a.SampleNo,
STUFF((SELECT ',' +  CONVERT(varchar, m.Name) FROM tb_TESTCHART m INNER JOIN tb_BILL_DETAIL n ON m.Id=n.TestId AND n.MasterId=a.MasterId WHERE n.SampleNo=a.SampleNo  AND n.Valid=1   FOR XML PATH('')), 1, 1, '') FullDesc ,(SELECT TOP 1 CollStatus FROM tb_LAB_STRICKER_INFO WHERE MasterId=a.MasterId AND SampleNo=a.SampleNo)AS CollStatus
 FROM tb_BILL_DETAIL a 
WHERE a.MasterId='" + masterId + "' AND a.Valid=1 AND LEN(a.SampleNo)>5 AND SampleNo NOT IN (SELECT SampleNo FROM tb_LAB_STRICKER_INFO WHERE MasterId=a.MasterId AND TestId=a.TestId AND VaqName='Imaging') Group by a.MasterId,a.SampleNo";             
                
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestCodeModel()
                    {
                        SampleNo = rdr["SampleNo"].ToString(),
                        TestName = rdr["FullDesc"].ToString(),
                        SampleCollectionStatus = rdr["CollStatus"].ToString(),
                    });
                }
                rdr.Close();
                ConLab.Close();
                return list;
            }
            catch (Exception exception)
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                return new List<TestCodeModel>();
            } 
        }
        internal TestCodeModel GetPatientDetails(int masterId)
        {
            try
            {
                var list = new TestCodeModel();
                string query = @"SELECT PatientName,Age,Sex,'' AS BedNo,UnderDrName,BillDate,MobileNo FROM V_Bill_Register WHERE MasterId=" + masterId + "";
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.PtName = rdr["PatientName"].ToString();
                    list.PtAge = rdr["Age"].ToString();
                    list.PtSex = rdr["Sex"].ToString();
                    list.DrName = rdr["UnderDrName"].ToString();
                    list.BedNo = rdr["BedNo"].ToString();
                    list.PtInvDate =Convert.ToDateTime(rdr["BillDate"]);
                    list.PtMobileNo = rdr["MobileNo"].ToString();
                }
                rdr.Close();
                ConLab.Close();
                return list;
            }
            catch (Exception exception)
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                return new TestCodeModel();
            }
        }
    }
}
