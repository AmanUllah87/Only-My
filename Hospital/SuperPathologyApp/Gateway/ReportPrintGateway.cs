using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
 
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using System.Windows.Forms;
using SuperPathologyApp.Model.Diagnosis;
using SuperPathologyApp.Gateway.Reagent;
using SuperPathologyApp.Model.Reagent;

namespace SuperPathologyApp.Gateway
{
    internal class ReportPrintGateway : DbConnection
    {
        public BillModel GetMasterData(int masterId)
        {
            try
            {
               
                var mdl = new BillModel();
                string query = @"EXEC Sp_Get_InvoicePrint "+ masterId +",''";
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    mdl.BillDate = Convert.ToDateTime(rdr["BillDate"]);
                    mdl.PatientName = rdr["PatientName"].ToString();
                    mdl.MobileNo = rdr["MobileNo"].ToString();
                    mdl.Age = rdr["Age"].ToString();
                    mdl.Sex = rdr["Sex"].ToString();
                    mdl.Remarks = rdr["Remarks"].ToString();
                    mdl.ConsDrModel = new DoctorModel() { DrId = Convert.ToInt32(rdr["UnderDrId"]), Name = rdr["ConsDr"].ToString() };
                    mdl.RefDrModel = new DoctorModel() { DrId = Convert.ToInt32(rdr["RefDrId"]), Name = rdr["RefDr"].ToString() };

                        
                }
                rdr.Close();
                ConLab.Close();
                return mdl;
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

        public List<TestCodeModel> GetDetailsData(int masterId,string groupName)
        {
            try
            {
                string cond = "";
                if (groupName != "COMMON REPORT")
                {
                    cond += " AND b.ReportFileName='" + groupName + "'";
                }
                if (groupName=="MICROBIOLOGY")
                {
                    cond = " AND b.ReportFileName='MICROBIOLOGY' OR b.ReportFileName='TOP'";
                }








                var list = new List<TestCodeModel>();
                string query = @"SELECT a.MasterId,a.TestId,b.Name AS TestName,b.ReportFileName,a.PrintStatus  FROM tb_BILL_DETAIL a INNER JOIN tb_TestChart b ON a.TestId=b.Id WHERE a.MasterId=" + masterId + " "+ cond +" AND b.VaqName!='Imaging'";

                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestCodeModel()
                    {
                        TestCode = rdr["TestId"].ToString(),
                        TestName = rdr["TestName"].ToString(),
                        ReportFileName = rdr["ReportFileName"].ToString(),
                        ReportPrintStatus = rdr["PrintStatus"].ToString()
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
        public  string GetLabNo(SqlTransaction tran, SqlConnection con)
        {
            string labNo=DateTime.Now.ToString("yy") + FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ReportNo", tran, con).PadLeft(7, '0');
            DeleteInsertLab("Update tb_MASTER_INFO SET ReportNo=ReportNo+1", tran, con);
            return labNo;
        }

        public string Save(ReportModel mdl, SqlTransaction trans, SqlConnection con)
        {
            try
            {
                #region ReportNo
                bool isFoundReportNo = false;

                string rptNo = "";
                foreach (var item in mdl.TestParam)
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
                    foreach (var item in mdl.TestParam)
                    {
                        item.ReportNo = reportNo;
                    }
                }
                else
                {
                    foreach (var item in mdl.TestParam)
                    {
                        item.ReportNo = rptNo;
                    }
                }
                #endregion


                foreach (var model in mdl.TestParam)
                {
                    DeleteInsertLab("DELETE FROM tb_REPORT_DETAILS WHERE MasterId='" + mdl.MasterId + "' AND TestChartId='" + model.TestchartId + "' AND MachineParam='" + model.MachineParam + "' ", trans, con);
                }
                DeleteInsertLab("DELETE FROM tb_REPORT_MASTER WHERE MasterId='" + mdl.MasterId + "' AND ReportNo='" + mdl.TestParam[0].ReportNo + "'", trans, con);
               
                foreach (var testCodeModel in mdl.TestParam)
                {
                    testCodeModel.IsPrint = 1;
                    testCodeModel.MstId = mdl.MasterId;
                }
                DataTable dt = ConvertListDataTable(mdl.TestParam);
                var objbulk = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, trans) { DestinationTableName = "tb_REPORT_DETAILS" };
                    objbulk.ColumnMappings.Add("MstId", "MasterId");
                    objbulk.ColumnMappings.Add("TestchartId", "TestChartId");
                    objbulk.ColumnMappings.Add("MachineParam", "MachineParam");
                    objbulk.ColumnMappings.Add("ReportParam", "ReportParam");
                    objbulk.ColumnMappings.Add("ReportingGroup", "ReportingGroup");
                    objbulk.ColumnMappings.Add("DefaultResult", "Result");
                    objbulk.ColumnMappings.Add("NormalRange", "NormalRange");
                    objbulk.ColumnMappings.Add("Unit", "Unit");
                    objbulk.ColumnMappings.Add("GroupSl", "GroupSl");
                    objbulk.ColumnMappings.Add("ParamSl", "ParamSl");
                    objbulk.ColumnMappings.Add("IsBold", "IsBold");
                    objbulk.ColumnMappings.Add("ReportNo", "ReportNo");
                    objbulk.ColumnMappings.Add("IsPrint", "IsPrint");

                    objbulk.WriteToServer(dt);





                //string rtf = "";
                //foreach (var paramModel in mdl.TestParam)
                //{
                //    if (FnSeekRecordNewLab("tb_REPORT_COMMENT_DEFAULT", "TestId='"+ paramModel.TestchartId +"'",trans,con))
                //    {
                //        rtf = FncReturnFielValueLab("tb_REPORT_COMMENT_DEFAULT","TestId='" + paramModel.TestchartId + "'", "Name",trans,con);
                //    }
                //}


               

                    const string query = "INSERT INTO tb_REPORT_MASTER( MasterId, ReportNo, LeftDrName, LeftDrDegree, MiddleDrName, MiddleDrDegree, RightDrName, RightDrDegree, Comment,  PostedBy,RtfFile)VALUES( @MasterId, @ReportNo, @LeftDrName, @LeftDrDegree, @MiddleDrName, @MiddleDrDegree, @RightDrName, @RightDrDegree, @Comment,  @PostedBy,@RtfFile)";
                var cmd = new SqlCommand(query, con,trans);
                    cmd.Parameters.AddWithValue("@MasterId", mdl.MasterId);
                    cmd.Parameters.AddWithValue("@ReportNo", mdl.TestParam[0].ReportNo);
                    cmd.Parameters.AddWithValue("@LeftDrName", mdl.LeftDoctor.Name);
                    cmd.Parameters.AddWithValue("@LeftDrDegree", mdl.LeftDoctor.Degree);
                    cmd.Parameters.AddWithValue("@MiddleDrName", mdl.MiddleDoctor.Name);
                    cmd.Parameters.AddWithValue("@MiddleDrDegree", mdl.MiddleDoctor.Degree);
                    cmd.Parameters.AddWithValue("@RightDrName", mdl.RightDoctor.Name);
                    cmd.Parameters.AddWithValue("@RightDrDegree", mdl.RightDoctor.Degree);
                    cmd.Parameters.AddWithValue("@Comment", mdl.TestParam[0].Comment);
                    cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);
                    cmd.Parameters.AddWithValue("@RtfFile", "");
                
                    cmd.ExecuteNonQuery();


             //   ConLab.Close();







                    var distinctItems = mdl.TestParam.GroupBy(x => x.TestchartId).Select(y => y.First());
                    var _gt=new ReagentMachineSetupGateway();
                    foreach (var item in distinctItems)
                    {
                        var outReagent = _gt.ChildCodeDetails(item.TestchartId.ToString(),trans,con);
                        foreach (var itm in outReagent)
                        {

                            double balQty = itm.Qty;
                            var stcMod = GetStockByPurchasePrice(itm.ReagentId,trans,con);
                            foreach (var itemModel in stcMod)
                            {
                                if (balQty > 0)
                                {
                                    double ouQty = balQty <= itemModel.Qty ? balQty : itemModel.Qty;
                                    if (ouQty < 0)
                                    {
                                        return "Can Not Save Problem In-Product Id:" + itm.ReagentId;
                                    }
                                    _gt.DeleteInsertLab("DELETE FROM tb_reagent_STOCK_LEDGER WHERE MasterId='"+ mdl.MasterId +"' AND ItemId='"+ itm.ReagentId +"'",trans,con);
                                    Hlp.InsertIntoStockLedgerReagent(mdl.MasterId, rptNo, Hlp.GetServerDate(), itm.ReagentId, itemModel.Price, 0, 0, ouQty, "Report", itemModel.LotNo, itemModel.ExpireDate, trans, con);
                                    balQty -= ouQty;
                                }
                            }
                        }
                    }







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
        public List<TestParamModel> GetDataFromMicrobiologyGrowthParameterDefinitionByTestCode()
        {
            try
            {
                var list = new List<TestParamModel>();
                string query = @"select * FROM tb_TESTCHART_PARAM_MICRO_GROWTH   Order By SlNo";
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestParamModel()
                    {
                        DrugName = rdr["DrugName"].ToString(),
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

        public List<TestParamModel> GetDataFromParameterDefinitionByTestCode(int testChartId)
        {
            try
            {
                var list = new List<TestParamModel>();
                string query = @"select * FROM tb_TESTCHART_PARAM WHERE TestChartId=" + testChartId + "   Order By GroupSl,ParamSl"; 
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestParamModel()
                    {
                        MachineParam = rdr["MachineParam"].ToString(),
                        ReportParam = rdr["ReportParam"].ToString(),
                        ReportingGroup = rdr["ReportingGroup"].ToString(),
                        NormalRange = rdr["NormalRange"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        GroupSl = Convert.ToInt32(rdr["GroupSl"]),
                        ParamSl = Convert.ToInt32(rdr["ParamSl"]),
                        DefaultResult = rdr["DefaultResult"].ToString(),
                        IsBold = Convert.ToInt32(rdr["IsBold"]),
                        LowerVal = Convert.ToDouble(rdr["LowerVal"]),
                        UpperVal = Convert.ToDouble(rdr["UpperVal"]),
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
                throw;
            }
        }









        public TestCodeModel GetDataFromMachineDataDtls(int masterId, int testCode,string paramName)
        {
            var mdl = new TestCodeModel();
            string query = "";
            
            
            
            if (FnSeekRecordNewLab("tb_REPORT_DETAILS", "MasterId=" + masterId + " AND TestChartId='" + testCode + "' AND MachineParam='" + paramName + "' "))
            {
                query = "SELECT TOP 1 Isnull(Result,'') AS Result,Isnull(ReportNo,'') AS ReportNo FROM tb_REPORT_DETAILS WHERE MasterId=" + masterId + " AND TestChartId='" + testCode + "'  AND MachineParam='" + paramName + "'  ORDER BY Id desc";
            }
            else
            {
                query = "SELECT TOP 1 Isnull(Result,'') AS Result,Isnull(ReportNo,'') AS ReportNo FROM tb_REPORT_DETAILS WHERE MasterId=" + masterId + " AND MachineParam='" + paramName + "'  ORDER BY Id desc";
            }
            
            

            ConLab.Open();
            var cmd = new SqlCommand(query, ConLab);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                mdl.Result = rdr["Result"].ToString();
                mdl.ReportNo = rdr["ReportNo"].ToString();
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
            catch (Exception exception)
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
            catch (Exception exception)
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


        private List<ReagentModel> GetStockByPurchasePrice(int itemId,SqlTransaction trans,SqlConnection con)
        {
            try
            {
                var list = new List<ReagentModel>();
                string query = "";
                query = @"SELECT a.ItemId,a.PurchasePrice,a.BalQty,a.LotNo,a.ExpireDate 
                        FROM V_reagent_Curr_Stock a WHERE a.ItemId=" + itemId + " GROUP BY a.ItemId,a.PurchasePrice,a.BalQty,a.LotNo,a.ExpireDate Order by a.ExpireDate";

                var cmd = new SqlCommand(query,con,trans);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new ReagentModel()
                    {
                        Price = Convert.ToDouble(rdr["PurchasePrice"]),
                        Qty = Convert.ToDouble(rdr["BalQty"]),
                        ExpireDate=Convert.ToDateTime(rdr["ExpireDate"]),
                        LotNo = rdr["LotNo"].ToString(),
                    });
                }
                rdr.Close();
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

     
    }
}
