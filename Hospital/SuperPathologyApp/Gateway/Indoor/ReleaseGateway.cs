using System.Data;
using System.Data.SqlClient;
using SuperPathologyApp.Gateway.DB_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperPathologyApp.Model.Indoor;
using SuperPathologyApp.Model;

namespace SuperPathologyApp.Gateway.Indoor
{
    class ReleaseGateway:DbConnection
    {
        private SqlTransaction _trans;
        

        public ReleaseModel GetLedgerDataByAdmId(int admId)
        {
           

            var aMdl = new ReleaseModel();
            var mdl = new List<TestChartModel>();
            ConLab.Open();


            string query = @"SELECT TestId,TestName,Charge,SUM(Unit) AS Unit,SUM(TotCharge) AS TotCharge,SUM(LessAmt) AS LessAmt,DrId,SUM(DrAmt) AS DrAmt 
from tb_IN_PATIENT_LEDGER WHERE AdmId='" + admId + "' AND TestId NOT IN (SELECT Id FROM tb_in_FIXED_ID) GROUP BY TestId,TestName,Charge,DrId  UNION ALL  SELECT TestId,TestName,SUM(TotCharge-CollAmt-LessAmt-RtnAmt) AS Charge,1 AS Unit,SUM(TotCharge-CollAmt-LessAmt-RtnAmt) AS TotCharge,SUM(LessAmt) AS LessAmt,DrId, SUM(DrAmt) AS DrAmt   from tb_IN_PATIENT_LEDGER  WHERE AdmId='" + admId + "' AND TestId IN (SELECT Id FROM tb_in_FIXED_ID WHERE TestName<>'Advance Collection' and iD!=" + Hlp.PhCode + ") GROUP BY TestId,TestName,DrId  HAVING (SUM(TotCharge-CollAmt-LessAmt-RtnAmt))>0 UNION ALL SELECT TestId,TestName,SUM(TotCharge+LessAdjust)-SUM(CollAmt+LessAmt+RtnAmt) AS Charge,1 AS Unit,SUM(TotCharge+LessAdjust)-SUM(CollAmt+LessAmt+RtnAmt) AS TotCharge,0 AS LessAmt,DrId, 0 AS DrAmt   from tb_IN_PATIENT_LEDGER  WHERE AdmId='" + admId + "' AND TestId =" + Hlp.PhCode + " GROUP BY TestId,TestName,DrId HAVING SUM(TotCharge+LessAdjust)-SUM(CollAmt+LessAmt+RtnAmt)>0 Order by TestName ";

            

            var cmd = new SqlCommand(query, ConLab);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                mdl.Add(new TestChartModel()
                {
                    TestId = Convert.ToInt32(rdr["TestId"].ToString()),
                    Name = rdr["TestName"].ToString(),
                    Charge = Convert.ToDouble(rdr["Charge"].ToString()),
                    Unit = Convert.ToDouble(rdr["Unit"].ToString()),
                    TotCharge = Convert.ToDouble(rdr["TotCharge"].ToString()),
                    LessAmt = Convert.ToDouble(rdr["LessAmt"].ToString()),
                    DrId = Convert.ToInt32(rdr["DrId"].ToString()),
                    DefaulHonouriam= Convert.ToDouble(rdr["DrAmt"].ToString()),
                });
                aMdl.Test = mdl;
            }
            rdr.Close();
            ConLab.Close();
            return aMdl;
        }
        public string GetReleaseNo(SqlTransaction trans, SqlConnection con)
        {
            string lcsql = @"Exec [S_ReleaseNo]";
            // Con.Open();
            var aCommand = new SqlCommand(lcsql, con, trans);
            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                lcsql = aReader["InvNo"].ToString();
            }
            // Con.Close();
            aReader.Close();
            return lcsql;
        }
        internal DataTable GetInvoiceList(DateTime dateTime, string searchString, string comeFrom)
        {
            try
            {
                string cond = "";
                if (searchString != "")
                {
                    cond = "AND (b.PtName+a.BillNo+b.ContactNo) like '%" + searchString + "%'";
                }
                if (comeFrom == "enter")
                {
                    cond += " AND a.BillDate='" + dateTime.ToString("yyyy-MM-dd") + "'";
                }
                string query = @"SELECT a.BillNo,a.BillDate,b.PtName As PatientName,b.ContactNo AS  MobileNo,a.TotalAmt,a.Remarks 
FROM tb_in_BILL_MASTER a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id WHERE 1=1 " + cond + " Order by a.BillNo Desc";
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
        
        internal string SaveInvoice(ReleaseModel aMdl, string comeFrom)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                var mdl = Hlp.GetRegAndBedId(aMdl.Admission.AdmId, ConLab, _trans);



                if (comeFrom == "&Update")
                {
                    //aMdl.BillId = Convert.ToInt32(FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + aMdl.BillNo + "'", "Id", _trans, ConLab));
                    //aMdl.BillNo = FncReturnFielValueLab("tb_BILL_MASTER", "Id='" + aMdl.BillId + "'", "BillNo", _trans, ConLab);
                    //aMdl.BillDate = Convert.ToDateTime(FncReturnFielValueLab("tb_BILL_MASTER", "Id='" + aMdl.BillId + "'", "BillDate", _trans, ConLab));
                    //double prevAmt = Convert.ToDouble(FncReturnFielValueLab("tb_BILL_MASTER", "Id='" + aMdl.BillId + "'", "TotalAmt", _trans, ConLab));
                    //int prevTestNo = Convert.ToInt32(FncReturnFielValueLab("tb_BILL_DETAIL", "MasterId='" + aMdl.BillId + "'", "COUNT(MasterId)", _trans, ConLab));
                    //string prevName = FncReturnFielValueLab("tb_BILL_MASTER", "Id='" + aMdl.BillId + "'", "PatientName", _trans, ConLab);
                    //int prevDrId = Convert.ToInt32(FncReturnFielValueLab("tb_BILL_MASTER", "Id='" + aMdl.BillId + "'", "RefDrId", _trans, ConLab));

                    //Hlp.InsertIntoEditRecord(aMdl.BillNo, aMdl.BillDate, prevName, prevAmt, aMdl.TotalAmt, prevTestNo, aMdl.TestChartModel.Count, prevDrId, aMdl.RefDrModel.DrId, "Bill", _trans, ConLab);


                }
                else
                {
                    aMdl.TrNo = GetReleaseNo(_trans, ConLab);
                    aMdl.TrDate = Hlp.GetServerDate(ConLab, _trans);
                }


                string query = "";

                if (comeFrom == "&Update")
                {
                    query = "UPDATE tb_BILL_MASTER SET  BillDate=@BillDate, RegId=@RegId, PatientName=@PatientName, MobileNo=@MobileNo, Address=@Address, Age=@Age, Sex=@Sex, RefDrId=@RefDrId, UnderDrId=@UnderDrId, TotalAmt=@TotalAmt, LessAmt=@LessAmt, LessFrom=@LessFrom, CollAmt=@CollAmt, Remarks=@Remarks, LessType=@LessType,LessPc=@LessPc,BillTime=@BillTime,DeliveryDate=@DeliveryDate,DeliveryNumber=@DeliveryNumber,DeliveryTimeAmPm=@DeliveryTimeAmPm WHERE Id=" + aMdl.InBillId + "";
                }
                else
                {
                    query = "INSERT INTO tb_IN_BILL_MASTER (BillNo, BillDate, BillTime, AdmId, RegId, BedId, TotalAmt, TotalDiscount, AdvanceAmt, PaidAmt, Remarks, PostedBy, PcName, IpAddress)OUTPUT INSERTED.ID VALUES(@BillNo, @BillDate, @BillTime, @AdmId, @RegId, @BedId, @TotalAmt, @TotalDiscount, @AdvanceAmt, @PaidAmt, @Remarks, @PostedBy, @PcName, @IpAddress)";
                }

                var cmd = new SqlCommand(query, ConLab, _trans);
                cmd.Parameters.AddWithValue("@BillNo", aMdl.TrNo);
                cmd.Parameters.AddWithValue("@BillDate", aMdl.TrDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@BillTime", DateTime.Now.ToString("hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@AdmId", aMdl.Admission.AdmId);
                cmd.Parameters.AddWithValue("@RegId", mdl.Patient.RegId);
                cmd.Parameters.AddWithValue("@BedId", mdl.Bed.BedId);
                cmd.Parameters.AddWithValue("@TotalAmt", aMdl.TotAmt);
                cmd.Parameters.AddWithValue("@TotalDiscount", aMdl.TotLessAmt);
                cmd.Parameters.AddWithValue("@AdvanceAmt", aMdl.AdvanceAmt);
                cmd.Parameters.AddWithValue("@PaidAmt", aMdl.TotCollAmt);
                cmd.Parameters.AddWithValue("@Remarks", aMdl.Remarks ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);
                cmd.Parameters.AddWithValue("@PcName", Environment.UserName);
                cmd.Parameters.AddWithValue("@IpAddress", Hlp.IpAddress());
                int masterId = 0;
                if (comeFrom == "&Update")
                {
                    //cmd.ExecuteNonQuery();
                    //DeleteInsertLab("DELETE FROM tb_BILL_DETAIL WHERE MasterId=" + aMdl.BillId + "", _trans, ConLab);
                    //DeleteInsertLab("DELETE FROM tb_BILL_LEDGER WHERE MasterId=" + aMdl.BillId + " AND TrNo='" + aMdl.BillNo + "'", _trans, ConLab);
                    //DeleteInsertLab("DELETE FROM tb_FINANCIAL_COLLECTION WHERE MasterId=" + aMdl.BillId + " AND ModuleName='Bill'", _trans, ConLab);
                }
                else
                {
                    masterId = (int)cmd.ExecuteScalar();
                    aMdl.InBillId = masterId;
                }

              

                foreach (var model in aMdl.Test)
                {
                    DeleteInsertLab("INSERT INTO tb_in_BILL_DETAIL(MasterId, BillNo, TestId,TestName, Charge, Unit, TotCharge, LessAmt, ExtraLess, DrAmt, DrId)VALUES(" + aMdl.InBillId + ", '" + aMdl.TrNo + "', '" + model.TestId + "','" + model.Name + "', '" + model.Charge + "', '" + model.Unit + "', '" + model.TotCharge + "','" + model.LessAmt + "', 0, '" + model.DefaulHonouriam + "', '" + model.DrId + "')", _trans, ConLab);
                }

                int releaseTimeCollId = Convert.ToInt32(FncReturnFielValueLab("tb_in_FIXED_ID", "Name='Release Time Collection'", "Id", _trans, ConLab));

                double collAmt = 0, rtnAmt = 0;
                if (aMdl.DueAmt<0)
                {
                    collAmt = 0;
                    rtnAmt = Math.Abs(aMdl.DueAmt);
                }
                else
                {
                    collAmt = aMdl.TotCollAmt;
                }

                Hlp.InsertIntoPatientLedger(aMdl.TrNo, Hlp.GetServerDate(ConLab, _trans), mdl.Patient.RegId, aMdl.Admission.AdmId, mdl.Bed.BedId, releaseTimeCollId, "Release Time Collection", 0, 1, 0, aMdl.TotLessAmt, collAmt, rtnAmt, 0,0, 0, 0, ConLab, _trans);

               
                   
                    foreach (var model in aMdl.Test)
                    {
                       
                        if (model.DefaulHonouriam>0)
                        {
                            DeleteInsertLab("INSERT INTO tb_DOCTOR_LEDGER(MasterId, DrId, TestId, Charge, HnrAmt,LessAmt,ModuleName)VALUES(" + aMdl.InBillId + ", '" + model.DrId + "', '" + model.TestId + "', '" + model.Charge + "', '" + (model.Charge-model.LessAmt) + "', '"+ model.LessAmt +"','Indoor')", _trans, ConLab);
                        }
                    }


                    if (aMdl.FinancialModel != null)
                    {
                        if (aMdl.FinancialModel.Count > 0)
                        {
                            foreach (var data in aMdl.FinancialModel)
                            {
                                DeleteInsertLab("INSERT INTO tb_FINANCIAL_COLLECTION(MasterId, FinancialId, Amount, ModuleName)VALUES(" + aMdl.InBillId + ", '" + data.MachineId + "', '" + data.Amount + "', 'Indoor-Bill')", _trans, ConLab);
                            }
                        }
                    }
                    DeleteInsertLab("Update tb_in_ADMISSION SET ReleaseStatus=1 WHERE Id=" + aMdl.Admission.AdmId + "",_trans,ConLab);
                    DeleteInsertLab("Update tb_in_BED SET BookStatus=0 WHERE Id=" + mdl.Bed.BedId + "", _trans, ConLab);
                
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
    }
}
