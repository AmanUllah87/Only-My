using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Diagnosis;
using SuperPathologyApp.Model.Indoor;

namespace SuperPathologyApp.Gateway.Diagnosis
{
    public class DiagnosisBillGateway : DbConnection
    {

        private SqlTransaction _trans;
        internal string SaveInvoice(BillModel aMdl,string comeFrom)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                
                if (comeFrom == "&Update")
                {
                    aMdl.BillId = Convert.ToInt32(FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + aMdl.BillNo + "'", "Id",_trans,ConLab));
                    aMdl.BillNo = FncReturnFielValueLab("tb_BILL_MASTER", "Id='" + aMdl.BillId + "'", "BillNo", _trans, ConLab);
                    aMdl.BillDate = Convert.ToDateTime(FncReturnFielValueLab("tb_BILL_MASTER", "Id='" + aMdl.BillId + "'", "BillDate", _trans, ConLab));
                    double prevAmt= Convert.ToDouble(FncReturnFielValueLab("tb_BILL_MASTER", "Id='" + aMdl.BillId + "'", "TotalAmt",_trans,ConLab));
                    int prevTestNo = Convert.ToInt32(FncReturnFielValueLab("tb_BILL_DETAIL", "MasterId='" + aMdl.BillId + "'", "COUNT(MasterId)", _trans, ConLab));
                    string prevName = FncReturnFielValueLab("tb_BILL_MASTER", "Id='" + aMdl.BillId + "'", "PatientName",_trans, ConLab);
                    int prevDrId = Convert.ToInt32(FncReturnFielValueLab("tb_BILL_MASTER", "Id='" + aMdl.BillId + "'", "RefDrId", _trans, ConLab));
                    Hlp.InsertIntoEditRecord(aMdl.BillNo,aMdl.BillDate,prevName,prevAmt,aMdl.TotalAmt,prevTestNo,aMdl.TestChartModel.Count,prevDrId,aMdl.RefDrModel.DrId,"Bill",_trans,ConLab);

                }
                else
                {
                    aMdl.BillNo = GetInvoiceNo(1, _trans, ConLab);
                    aMdl.BillDate = Hlp.GetServerDate(ConLab,_trans);
                }

              
                string query = "";

                if (comeFrom == "&Update")
                {
                    query = "UPDATE tb_BILL_MASTER SET  BillDate=@BillDate, RegId=@RegId, PatientName=@PatientName, MobileNo=@MobileNo, Address=@Address, Age=@Age, Sex=@Sex, RefDrId=@RefDrId, UnderDrId=@UnderDrId, TotalAmt=@TotalAmt, LessAmt=@LessAmt, LessFrom=@LessFrom, CollAmt=@CollAmt, Remarks=@Remarks, LessType=@LessType,LessPc=@LessPc,BillTime=@BillTime,DeliveryDate=@DeliveryDate,DeliveryNumber=@DeliveryNumber,DeliveryTimeAmPm=@DeliveryTimeAmPm WHERE Id=" + aMdl.BillId + "";
                }
                else
                {
                    query = "INSERT INTO tb_BILL_MASTER (BillNo, BillDate, RegId, PatientName, MobileNo, Address, Age, Sex, RefDrId, UnderDrId, TotalAmt, LessAmt, LessFrom, CollAmt, Remarks, PostedBy,LessType,LessPc,BillTime,DeliveryDate,DeliveryNumber,DeliveryTimeAmPm,PcName,IpAddress,AdmId)OUTPUT INSERTED.ID VALUES(@BillNo, FORMAT(getdate(),'yyyy-MM-dd'), @RegId, @PatientName, @MobileNo, @Address, @Age, @Sex, @RefDrId, @UnderDrId, @TotalAmt, @LessAmt, @LessFrom, @CollAmt, @Remarks, @PostedBy,@LessType,@LessPc,FORMAT(GETDATE(),'hh:mm tt') ,@DeliveryDate,@DeliveryNumber,@DeliveryTimeAmPm,@PcName,@IpAddress,@AdmId)";
                }
                
                
                var cmd = new SqlCommand(query, ConLab, _trans);
                cmd.Parameters.AddWithValue("@BillNo", aMdl.BillNo);
                cmd.Parameters.AddWithValue("@BillDate", aMdl.BillDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@RegId", aMdl.RegId);
                cmd.Parameters.AddWithValue("@PatientName", aMdl.PatientName);
                cmd.Parameters.AddWithValue("@MobileNo", aMdl.MobileNo?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", aMdl.Address);
                cmd.Parameters.AddWithValue("@Age", aMdl.Age);
                cmd.Parameters.AddWithValue("@Sex", aMdl.Sex);
                cmd.Parameters.AddWithValue("@RefDrId", aMdl.RefDrModel.DrId);
                cmd.Parameters.AddWithValue("@UnderDrId", aMdl.ConsDrModel.DrId);
                cmd.Parameters.AddWithValue("@TotalAmt", aMdl.TotalAmt);
                cmd.Parameters.AddWithValue("@LessAmt", aMdl.TotalLessAmt);
                cmd.Parameters.AddWithValue("@LessFrom", aMdl.LessFrom);

                cmd.Parameters.AddWithValue("@CollAmt", aMdl.CollAmt);
                cmd.Parameters.AddWithValue("@Remarks", aMdl.Remarks ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);


                cmd.Parameters.AddWithValue("@DeliveryDate", aMdl.DeliveryDate);
                cmd.Parameters.AddWithValue("@DeliveryTimeAmPm", aMdl.DeliveryTimeAmPm);
                cmd.Parameters.AddWithValue("@DeliveryNumber", aMdl.DeliveryNumber);



                cmd.Parameters.AddWithValue("@LessPc", aMdl.LessPc);
                cmd.Parameters.AddWithValue("@LessType", aMdl.LessType ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@BillTime", DateTime.Now.ToString("hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@PcName", Environment.UserName);
                cmd.Parameters.AddWithValue("@IpAddress", Hlp.IpAddress());
                cmd.Parameters.AddWithValue("@AdmId", aMdl.Admission.AdmId);
             
                int masterId = 0;
                if (comeFrom == "&Update")
                {
                     cmd.ExecuteNonQuery();
                     DeleteInsertLab("DELETE FROM tb_BILL_DETAIL WHERE MasterId=" + aMdl.BillId + "", _trans, ConLab);
                     DeleteInsertLab("DELETE FROM tb_BILL_LEDGER WHERE MasterId=" + aMdl.BillId + " AND TrNo='" + aMdl.BillNo + "'", _trans, ConLab);
                     DeleteInsertLab("DELETE FROM tb_FINANCIAL_COLLECTION WHERE MasterId=" + aMdl.BillId + " AND ModuleName='Bill'", _trans, ConLab);
                     DeleteInsertLab("DELETE FROM tb_DOCTOR_LEDGER WHERE MasterId=" + aMdl.BillId + " AND ModuleName='Diagnosis'", _trans, ConLab);

                     DeleteInsertLab("DELETE FROM tb_in_PATIENT_LEDGER WHERE AdmId=" + aMdl.Admission.AdmId + " AND TrNo='"+ aMdl.BillNo +"'", _trans, ConLab);

                    //

                }
                else
                {
                    masterId = (int)cmd.ExecuteScalar();
                    aMdl.BillId = masterId;
                }
                foreach (var model in aMdl.TestChartModel)
                {
                    double gridLess = Math.Round(model.Charge * aMdl.TotalLessAmt / aMdl.TotalAmt, 4);
                    if (Double.IsNaN(gridLess))
                    {
                        gridLess = 0;
                    }
                    
                    if (model.GridRemarks==null)
                    {
                        model.GridRemarks = "";
                    }
                    DeleteInsertLab("INSERT INTO tb_BILL_DETAIL(MasterId, BillNo, TestId, Charge, Quantity, ActualCharge, HnrAmt, DrLess, TotHnrAmt,GridRemarks,BillTimeLess)VALUES(" + aMdl.BillId + ", '" + aMdl.BillNo + "', '" + model.TestId + "', '" + model.Charge + "', '1', 0,'" + model.DefaulHonouriam + "', '" + model.HnrLess + "', '" + model.HnrToPay + "', '" + model.GridRemarks + "','"+ gridLess +"')", _trans, ConLab);
                }
                #region commsion ledger

                if (FnSeekRecordNewLab("tb_DrWiseHonorium", "DrId='" + aMdl.RefDrModel.DrId + "'", _trans, ConLab))
                {
                    #region for drwiseHonouriam
                    double hnrLess = 0;
                    foreach (var model in aMdl.TestChartModel)
                    {
                        if (aMdl.LessFrom == "Doctor")
                        {
                            if (FnSeekRecordNewLab("tb_MASTER_INFO", "UnderDrComision=0", _trans, ConLab))
                            {
                                hnrLess = model.HnrLess;
                            }
                        }
                        int subProjectId = Convert.ToInt32(FncReturnFielValueLab("tb_TESTCHART", "Id='" + model.TestId + "'", "SubProjectId", _trans, ConLab));
                        double hnrAmt = 0;

                        try
                        {
                            hnrAmt = Convert.ToDouble(FncReturnFielValueLab("tb_DrWiseHonorium", "DrId='" + aMdl.RefDrModel.DrId + "' AND SubProjectId='" + subProjectId + "'", "CASE WHEN Type='%' THEN (" + model.Charge + "*0.01*HonouriamAmt) ELSE HonouriamAmt END", _trans, ConLab));
                        }
                        catch (Exception)
                        {
                            ;

                        }
                        DeleteInsertLab("INSERT INTO tb_DOCTOR_LEDGER(MasterId, DrId, TestId, Charge, HnrAmt,LessAmt,MODULENAME)VALUES(" + aMdl.BillId + ", '" + aMdl.RefDrModel.DrId + "', '" + model.TestId + "', '" + model.Charge + "', '" + hnrAmt + "', '" + hnrLess + "','Diagnosis')", _trans, ConLab);
                    }
                    #endregion
                }
                else {
                    foreach (var model in aMdl.TestChartModel)
                    {
                        double gridLess = Math.Round(model.Charge * aMdl.TotalLessAmt / aMdl.TotalAmt, 4);
                        if (Double.IsNaN(gridLess))
                        {
                            gridLess = 0;
                        }

                        if (model.GridRemarks == null)
                        {
                            model.GridRemarks = "";
                        }
                        DeleteInsertLab("INSERT INTO tb_DOCTOR_LEDGER(MasterId, DrId, TestId, Charge, HnrAmt,LessAmt,MODULENAME)VALUES(" + aMdl.BillId + ", '" + aMdl.RefDrModel.DrId + "', '" + model.TestId + "', '" + model.Charge + "', '" + model.DefaulHonouriam + "', '" + model.HnrLess + "','Diagnosis')", _trans, ConLab);
                    }
                }









                if (FnSeekRecordNewLab("tb_MASTER_INFO", "UnderDrComision=1", _trans, ConLab))
                {
                    if (FnSeekRecordNewLab("tb_DrWiseHonorium", "DrId='" + aMdl.ConsDrModel.DrId + "'", _trans, ConLab))
                    {
                        if (aMdl.ConsDrModel.DrId!=aMdl.RefDrModel.DrId)
                        {
                            foreach (var model in aMdl.TestChartModel)
                            {
                                if (aMdl.LessFrom != "Doctor")
                                {
                                    model.HnrLess = 0;
                                }
                                int subProjectId = Convert.ToInt32(FncReturnFielValueLab("tb_TESTCHART", "Id='" + model.TestId + "'", "SubProjectId", _trans, ConLab));

                                double hnrAmt = 0;

                                try
                                {
                                    hnrAmt = Convert.ToDouble(FncReturnFielValueLab("tb_DrWiseHonorium", "DrId='" + aMdl.ConsDrModel.DrId + "' AND SubProjectId='" + subProjectId + "'", "CASE WHEN Type='%' THEN (" + model.Charge + "*0.01*HonouriamAmt) ELSE HonouriamAmt  END", _trans, ConLab));
                                }
                                catch (Exception)
                                {
                                    ;
                                }
                                DeleteInsertLab("INSERT INTO tb_DOCTOR_LEDGER(MasterId, DrId, TestId, Charge, HnrAmt,LessAmt,MODULENAME)VALUES(" + aMdl.BillId + ", '" + aMdl.ConsDrModel.DrId + "', '" + model.TestId + "', '" + model.Charge + "', '" + hnrAmt + "', '" + model.HnrLess + "','Diagnosis')", _trans, ConLab);
                            }
                        }
                    }
                }
                #endregion
                Hlp.InsertIntoLedger(aMdl.BillNo, aMdl.BillDate, aMdl.BillId, aMdl.TotalAmt, aMdl.TotalLessAmt, 0, aMdl.CollAmt, Hlp.UserName, _trans, ConLab);

                if (aMdl.FinancialModel!=null)
                {
                    if (aMdl.FinancialModel.Count > 0)
                    {
                        foreach (var data in aMdl.FinancialModel)
                        {
                            DeleteInsertLab("INSERT INTO tb_FINANCIAL_COLLECTION(MasterId, FinancialId, Amount, ModuleName)VALUES(" + aMdl.BillId + ", '" + data.MachineId + "', '" + data.Amount + "', 'Bill')", _trans, ConLab);
                        }
                    }
                }

                if (FnSeekRecordNewLab("tb_in_ADMISSION","Id='"+ aMdl.Admission.AdmId +"'",_trans,ConLab))
                {
                    int diagnosisBill = Convert.ToInt32(FncReturnFielValueLab("tb_in_FIXED_ID", "Name='Diagnosis Bill'", "Id", _trans, ConLab));
                    var mdl = Hlp.GetRegAndBedId(aMdl.Admission.AdmId, ConLab, _trans);
                    Hlp.InsertIntoPatientLedger(aMdl.BillNo, Hlp.GetServerDate(ConLab,_trans), mdl.Patient.RegId, aMdl.Admission.AdmId, mdl.Bed.BedId, diagnosisBill, "Diagnosis Bill", aMdl.TotalAmt, 1, aMdl.TotalAmt, 0, 0, 0, 0, 0,0, 0, ConLab, _trans);
                }





                _trans.Commit();
                ConLab.Close();

                var brCode=new BarcodePrintGateway();
                brCode.TransferDataFromOtherDbToLabDbByInvNoAndDate(masterId);














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
                aMdl.Admission = new AdmissionModel()
                {
                    AdmId = Convert.ToInt32(rdr["AdmId"].ToString()),
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
