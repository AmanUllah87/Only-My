using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model.Pharmacy;

namespace SuperPathologyApp.Gateway.Pharmacy
{
    class SalesGateway : DbConnection
    {
        private SqlTransaction _trans;
        internal string Save(PurchaseModel aMdl, string comeFrom)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();

                if (comeFrom == "&Update")
                {
                    aMdl.BillId = Convert.ToInt32(FncReturnFielValueLab("tb_ph_PURCHASE_MASTER", "BillNo='" + aMdl.BillNo + "'", "Id", _trans, ConLab));
                    aMdl.BillNo = FncReturnFielValueLab("tb_ph_PURCHASE_MASTER", "Id='" + aMdl.BillId + "'", "BillNo", _trans, ConLab);
                    aMdl.BillDate = Convert.ToDateTime(FncReturnFielValueLab("tb_ph_PURCHASE_MASTER", "Id='" + aMdl.BillId + "'", "BillDate", _trans, ConLab));
                    double prevAmt = Convert.ToDouble(FncReturnFielValueLab("tb_ph_PURCHASE_MASTER", "Id='" + aMdl.BillId + "'", "TotalAmt", _trans, ConLab));
                    int prevTestNo = Convert.ToInt32(FncReturnFielValueLab("tb_ph_PURCHASE_MASTER", "MasterId='" + aMdl.BillId + "'", "COUNT(MasterId)", _trans, ConLab));
                    string prevName = FncReturnFielValueLab("tb_ph_PURCHASE_MASTER", "Id='" + aMdl.BillId + "'", "PatientName", _trans, ConLab);
                    int prevDrId = Convert.ToInt32(FncReturnFielValueLab("tb_ph_PURCHASE_MASTER", "Id='" + aMdl.BillId + "'", "RefDrId", _trans, ConLab));

                   // Hlp.InsertIntoEditRecord(aMdl.BillNo, aMdl.BillDate, prevName, prevAmt, aMdl.TotalAmt, prevTestNo, aMdl.TestChartModel.Count, prevDrId, aMdl.RefDrModel.DrId, "Bill", _trans, ConLab);


                }
                else
                {
                    aMdl.BillNo = GetInvoiceNo(4, _trans, ConLab);
                    aMdl.BillDate = Hlp.GetServerDate(ConLab, _trans);
                }


                string query = "";

                if (comeFrom == "&Update")
                {
                    query = "UPDATE tb_ph_PURCHASE_MASTER SET  BillNo=@BillNo,BillDate=@BillDate,ReceiptNo=@ReceiptNo,ReceiptDate=@ReceiptDate,SuppId=@SuppId,TotItem=@TotItem,TotPrice=@TotPrice,TotVat=@TotVat,TotComision=@TotComision,TotLess=@TotLess,TotPaid=@TotPaid,Remarks=@Remarks,PostedBy=@PostedBy,PcName=@PcName,IpAddress=@IpAddress  WHERE Id=" + aMdl.BillId + "";
                }
                else
                {
                    query = @"INSERT INTO tb_ph_SALES_MASTER(BillNo, BillDate, CustId, TotItem, TotAmt, TotVat, TotLess, TotPaid, Remarks, PostedBy, PcName, IpAddress,AdmId)OUTPUT INSERTED.ID VALUES(@BillNo, FORMAT(getdate(),'yyyy-MM-dd'), @CustId, @TotItem, @TotAmt, @TotVat, @TotLess, @TotPaid, @Remarks, @PostedBy, @PcName, @IpAddress,@AdmId)";
                }

                var cmd = new SqlCommand(query, ConLab, _trans);
                cmd.Parameters.AddWithValue("@BillNo", aMdl.BillNo);
                cmd.Parameters.AddWithValue("@BillDate", aMdl.BillDate.ToString("yyyy-MM-dd"));


                cmd.Parameters.AddWithValue("@CustId", aMdl.PatientsModel.RegId);
                cmd.Parameters.AddWithValue("@TotItem", aMdl.TotalItem);
                cmd.Parameters.AddWithValue("@TotAmt", aMdl.TotAmount);
                cmd.Parameters.AddWithValue("@NetAmt", aMdl.NetAmount);
                cmd.Parameters.AddWithValue("@TotVat", aMdl.TotalVat);
                cmd.Parameters.AddWithValue("@TotLess", aMdl.TotalLess);
                cmd.Parameters.AddWithValue("@TotPaid", aMdl.TotalPaid);
                cmd.Parameters.AddWithValue("@Remarks", aMdl.Remarks ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);
                cmd.Parameters.AddWithValue("@PcName", Environment.UserName);
                cmd.Parameters.AddWithValue("@IpAddress", Hlp.IpAddress());
                cmd.Parameters.AddWithValue("@AdmId", aMdl.Admission.AdmId);
                

                if (comeFrom == "&Update")
                {
                    cmd.ExecuteNonQuery();
                    DeleteInsertLab("DELETE FROM tb_BILL_DETAIL WHERE MasterId=" + aMdl.BillId + "", _trans, ConLab);
                    DeleteInsertLab("DELETE FROM tb_BILL_LEDGER WHERE MasterId=" + aMdl.BillId + " AND TrNo='" + aMdl.BillNo + "'", _trans, ConLab);
                    DeleteInsertLab("DELETE FROM tb_FINANCIAL_COLLECTION WHERE MasterId=" + aMdl.BillId + " AND ModuleName='Bill'", _trans, ConLab);
                    DeleteInsertLab("DELETE FROM tb_DOCTOR_LEDGER WHERE MasterId=" + aMdl.BillId + " AND ModuleName='Diagnosis'", _trans, ConLab);

                }
                else
                {
                    var masterId = (int)cmd.ExecuteScalar();
                    aMdl.BillId = masterId;
                }
                foreach (var model in aMdl.ItemModels)
                {
                    double balQty = model.Qty;
                    var stcMod = GetStockByPurchasePrice(model.Id,_trans, ConLab);
                    foreach (var itemModel in stcMod)
                    {
                        if (balQty>0)
                        {
                            double ouQty = balQty <= itemModel.BQty ? balQty : itemModel.BQty;
                            if (ouQty<0)
                            {
                                return "Can Not Save Problem In-Product Id:"+model.Id;
                            }
                            
                            DeleteInsertLab("INSERT INTO tb_ph_SALES_DETAIL(MasterId, ItemId, PurchasePrice, Qty, UnitPrice, UnitTotal, VatAmt,LessAmt)VALUES(" + aMdl.BillId + ", '" + model.Id + "', '" + itemModel.PurchasePrice + "',  '" + ouQty + "','" + model.SalePrice + "', '" + model.UnitTotal + "', '" + model.Vat + "', '" + model.LessAmt + "')", _trans, ConLab);
                            Hlp.InsertIntoStockLedgerPharmacy(aMdl.BillId, aMdl.BillNo, aMdl.BillDate, model.Id, itemModel.PurchasePrice, model.SalePrice, 0, ouQty, "Sales",aMdl.PatientsModel.RegId,aMdl.Admission.AdmId, _trans, ConLab);
                            balQty -= ouQty;
                        }
                    }
                }
                Hlp.InsertIntoSalesLedgerPharmacy(aMdl.PatientsModel.RegId, aMdl.BillNo, aMdl.BillDate, aMdl.BillId, aMdl.TotAmount, aMdl.TotalLess, 0, aMdl.TotalPaid,0, Hlp.UserName,aMdl.Admission.AdmId, _trans, ConLab);


                if (FnSeekRecordNewLab("tb_in_ADMISSION", "Id='" + aMdl.Admission.AdmId + "'", _trans, ConLab))
                {
                    int medicineBill = Convert.ToInt32(FncReturnFielValueLab("tb_in_FIXED_ID", "Name='Medicine Bill'", "Id", _trans, ConLab));
                    var mdl = Hlp.GetRegAndBedId(aMdl.Admission.AdmId, ConLab, _trans);
                    Hlp.InsertIntoPatientLedger(aMdl.BillNo, Hlp.GetServerDate(ConLab, _trans), mdl.Patient.RegId, aMdl.Admission.AdmId, mdl.Bed.BedId, medicineBill, "Medicine Bill", aMdl.TotAmount, 1, aMdl.TotAmount, aMdl.TotalLess, aMdl.TotalPaid, 0,0, 0, 0, 0, ConLab, _trans);
                }


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

        private List<ItemModel> GetStockByPurchasePrice(int itemId, SqlTransaction trans, SqlConnection con)
        {
            try
            {
                var list = new List<ItemModel>();
                string query = "";
                query = @"SELECT a.ItemId,a.PurchasePrice,  SUM(a.InQty-a.OutQty) AS BalQty 
                        FROM tb_ph_STOCK_LEDGER a INNER JOIN  tb_ph_ITEM b ON a.ItemId=b.Id WHERE a.ItemId=" + itemId + " GROUP BY a.ItemId,a.PurchasePrice HAVING SUM(a.InQty-a.OutQty)>0";
                
                var cmd = new SqlCommand(query, con,trans);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new ItemModel()
                    {
                        PurchasePrice = Convert.ToDouble(rdr["PurchasePrice"]),
                        BQty = Convert.ToDouble(rdr["BalQty"]),
                    });
                }
                rdr.Close();
                return list;
            }
            catch (Exception )
            {
                throw;
            }
        }

        internal DataTable GetInvoiceList(DateTime dateTime, string searchString, string comeFrom)
        {
            try
            {
                string cond = "";
                if (searchString != "")
                {
                    cond = "AND (a.BillNo+Isnull(b.Name,'')) like '%" + searchString + "%'";
                }
                if (comeFrom == "enter")
                {
                    cond += " AND a.BillDate='" + dateTime.ToString("yyyy-MM-dd") + "'";
                }
                string query = @"SELECT a.BillNo,a.BillDate,CASE WHEN a.AdmId!=0 THEN c.PtName ELSE  Isnull(b.Name,'Honourable Customer') END AS CustName,
a.PostedBy, a.TotAmt 
FROM tb_ph_SALES_MASTER a LEFT JOIN tb_PATIENT b ON a.CustId=b.Id  
LEFT JOIN tb_in_ADMISSION c ON a.AdmId=c.Id
WHERE 1=1 "+ cond +" Order by a.BillNo Desc";
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
        
      
        
     


        
        
    }
}
