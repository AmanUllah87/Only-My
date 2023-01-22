using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Diagnosis;

namespace SuperPathologyApp.Gateway.Diagnosis
{
    public class DiagnosisBillReturnGateway : DbConnection
    {

        private SqlTransaction _trans;
        internal string SaveInvoice(BillModel aMdl,string comeFrom)
        {
            try
            {
                aMdl.BillNo = GetRtnNo();
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                
                foreach (var model in aMdl.TestChartModel)
                {
                    if (model.RtnAmt>0)
                    {
                        DeleteInsertLab("INSERT INTO tb_BILL_RETURN(TrNo, TrDate, MasterId, InvoiceValue, TotalReturn, ItemId, Charge, LessAmt, RtnAmt, PostedBy, PcName, IpAddress,CashRtn,DueAdjust)VALUES('" + aMdl.BillNo + "', FORMAT(getdate(),'yyyy-MM-dd'), '" + aMdl.BillId + "', '" + aMdl.TotalAmt + "', '" + aMdl.BillReturn.TotalReturnAmt + "', '" + model.TestId + "', '" + model.Charge + "', '" + model.LessAmt + "', '" + model.RtnAmt + "', '" + Hlp.UserName + "', '" + Environment.UserName + "', '" + Hlp.IpAddress() + "', '" + aMdl.BillReturn.CashReturnAmt + "', '" + aMdl.BillReturn.DueAdjustAmt + "')", _trans, ConLab);
                    }
                }
            

                Hlp.InsertIntoLedger(aMdl.BillNo, Hlp.GetServerDate(ConLab,_trans), aMdl.BillId, 0, 0, aMdl.BillReturn.TotalReturnAmt, 0, Hlp.UserName, _trans, ConLab);

            





                _trans.Commit();
                ConLab.Close();

                
                return SaveSuccessMessage;
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

     

        internal DataTable GetInvoiceList(DateTime dateTime, string searchString,string comeFrom)
        {
            try
            {
                string cond = "";
                if (searchString != "")
                {
                    cond = "AND (PatientName+BillNo+MobileNo+Remarks) like '%" + searchString + "%'";
                }
                if (comeFrom=="enter")
                {
                    cond += " AND BillDate='"+ dateTime.ToString("yyyy-MM-dd") +"'";
                }
                string query = @"SELECT BillNo,BillDate,PatientName,MobileNo,TotalAmt,Remarks FROM tb_BILL_MASTER WHERE IsShow=1  " + cond + " Order by BillNo Desc";
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
                ConLab.Close();
                throw;
            }
        }
        internal DataTable GetInvoiceListForDigitalVd(DateTime dateTime, string searchString, string comeFrom)
        {
            try
            {
                string cond = "";
                if (searchString != "")
                {
                    cond = "AND (PatientName+BillNo+MobileNo+Remarks) like '%" + searchString + "%'";
                }
                if (comeFrom == "enter")
                {
                    cond += " AND BillDate='" + dateTime.ToString("yyyy-MM-dd") + "'";
                }
                string query = @"SELECT BillNo,BillDate,PatientName,MobileNo,TotalAmt,Remarks,CASE WHEN  ISNULL((SELECT TOP 1 BillNo FROM tb_DigitalVd WHERE BillNo=tb_BILL_MASTER.BillNo),'DUE') ='DUE' THEN 'DUE' ELSE 'PAID' END AS STATUS
                        FROM tb_BILL_MASTER WHERE IsShow=1  " + cond + " Order by BillNo Desc";
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
                ConLab.Close();
                throw;
            }
        }

        internal BillModel GetInvoiceDataForEdit(string invNo)
        {
            int masterId =Convert.ToInt32(FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + invNo + "'", "Id"));

            var aMdl = new BillModel();
            var mdl = new List<TestChartModel>();
            ConLab.Open();
            string query = @"EXEC Sp_Get_InvoicePrint "+ masterId +",''";
            var cmd = new SqlCommand(query, ConLab);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                aMdl.PatientName = rdr["PatientName"].ToString();
                aMdl.Age = rdr["Age"].ToString();
                aMdl.Sex = rdr["Sex"].ToString(); 
                aMdl.Address = rdr["Address"].ToString(); 
                aMdl.MobileNo = rdr["MobileNo"].ToString(); 


                aMdl.RefDrModel = new DoctorModel()
                {
                    DrId = Convert.ToInt32(rdr["RefDrId"].ToString()),
                    Name=rdr["RefDr"].ToString(),
                };
                aMdl.ConsDrModel = new DoctorModel()
                {
                    DrId = Convert.ToInt32(rdr["UnderDrId"].ToString()),
                    Name = rdr["ConsDr"].ToString(),
                };


                aMdl.LessFrom = rdr["LessFrom"].ToString();
                aMdl.LessType = rdr["LessType"].ToString();
                aMdl.LessPc = Convert.ToDouble(rdr["LessPc"].ToString());



                aMdl.TotalAmt = Convert.ToDouble(rdr["TotalAmt"].ToString());

                aMdl.TotalLessAmt = Convert.ToDouble(rdr["LessAmt"].ToString());
                aMdl.CollAmt = Convert.ToDouble(rdr["CollAmt"].ToString());
                aMdl.Remarks = rdr["Remarks"].ToString();



               
                    mdl.Add(new TestChartModel()
                    {

                        TestId = Convert.ToInt32(rdr["TestId"].ToString()),
                        Charge = Convert.ToDouble(rdr["Charge"].ToString()),
                        Name=rdr["TestName"].ToString(),
                        DefaulHonouriam = Convert.ToDouble(rdr["HnrAmt"].ToString()),
                        HnrLess = Convert.ToDouble(rdr["DrLess"].ToString()),
                        HnrToPay = Convert.ToDouble(rdr["TotHnrAmt"].ToString()),
                        LessAmt = Convert.ToDouble(rdr["BillTimeLess"].ToString()),

                    });
               
                aMdl.TestChartModel = mdl;


            }
            rdr.Close();
            ConLab.Close();
            return aMdl;





        }

        internal List<MachineModel> GetFinancial()
        {
            var mdl = new List<MachineModel>();
            ConLab.Open();
            var cmd = new SqlCommand("SELECT Id,Name FROM tb_FINANCIAL_SERVICE", ConLab);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                mdl.Add(new MachineModel()
                {
                    MachineId =Convert.ToInt32(rdr["Id"]),
                    MachineName = rdr["Name"].ToString(),
                });
            }
            rdr.Close();
            ConLab.Close();
            return mdl;
        }
    }
}
