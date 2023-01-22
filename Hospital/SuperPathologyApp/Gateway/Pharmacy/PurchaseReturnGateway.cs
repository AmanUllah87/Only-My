using System;
using System.Data;
using System.Data.SqlClient;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model.Pharmacy;
using System.Collections.Generic;

namespace SuperPathologyApp.Gateway.Pharmacy
{
    class PurchaseReturnGateway : DbConnection
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
                    double prevAmt = Convert.ToDouble(FncReturnFielValueLab("tb_ph_PURCHASE_MASTER", "Id='" + aMdl.BillId + "'", "TotAmt", _trans, ConLab));
                    int prevItem = Convert.ToInt32(FncReturnFielValueLab("tb_ph_PURCHASE_MASTER", "Id='" + aMdl.BillId + "'", "TotItem", _trans, ConLab));
                    string prevName = FncReturnFielValueLab("tb_ph_Supplier", "Id='" + aMdl.Supplier.Id+ "'", "Name", _trans, ConLab);

                   Hlp.InsertIntoEditRecord(aMdl.BillNo, aMdl.BillDate, prevName, prevAmt, aMdl.TotAmount, prevItem, aMdl.ItemModels.Count, 0, 0, "Pharmacy-Purchase", _trans, ConLab);


                }
                else
                {
                    aMdl.BillNo = GetInvoiceNo(7, _trans, ConLab);
                    aMdl.BillDate = Hlp.GetServerDate(ConLab, _trans);
                }


                string query = "";

                if (comeFrom == "&Update")
                {
                    query = "UPDATE tb_ph_PURCHASE_MASTER SET  BillNo=@BillNo,BillDate=@BillDate,ReceiptNo=@ReceiptNo,ReceiptDate=@ReceiptDate,SuppId=@SuppId,TotItem=@TotItem,TotAmt=@TotAmt,NetAmt=@NetAmt,TotVat=@TotVat,TotComision=@TotComision,TotLess=@TotLess,TotPaid=@TotPaid,Remarks=@Remarks,PostedBy=@PostedBy,PcName=@PcName,IpAddress=@IpAddress  WHERE Id=" + aMdl.BillId + "";
                }
                else
                {
                    query = @"INSERT INTO tb_ph_PURCHASE_RETURN_MASTER(BillNo, BillDate, SuppId, TotItem, TotAmt, Remarks, PostedBy,  PcName, IpAddress)OUTPUT INSERTED.ID VALUES(@BillNo, FORMAT(getdate(),'yyyy-MM-dd'), @SuppId, @TotItem, @TotAmt, @Remarks, @PostedBy,  @PcName, @IpAddress)";
                }

                if (aMdl.Remarks==null)
                {
                    aMdl.Remarks = "";
                }


                var cmd = new SqlCommand(query, ConLab, _trans);
                cmd.Parameters.AddWithValue("@BillNo", aMdl.BillNo);
                cmd.Parameters.AddWithValue("@BillDate", Hlp.GetServerDate(ConLab, _trans).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@SuppId", aMdl.Supplier.Id);
                cmd.Parameters.AddWithValue("@TotItem", aMdl.TotalItem);
                cmd.Parameters.AddWithValue("@TotAmt", aMdl.TotAmount);
                cmd.Parameters.AddWithValue("@Remarks", aMdl.Remarks ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);
                cmd.Parameters.AddWithValue("@PcName", Environment.UserName);
                cmd.Parameters.AddWithValue("@IpAddress", Hlp.IpAddress());

                int masterId = 0;
                if (comeFrom == "&Update")
                {
                    cmd.ExecuteNonQuery();
                    DeleteInsertLab("DELETE FROM tb_ph_PURCHASE_RETURN_MASTER WHERE MasterId=" + aMdl.BillId + "", _trans, ConLab);
                    DeleteInsertLab("DELETE FROM tb_ph_PURCHASE_LEDGER WHERE MasterId=" + aMdl.BillId + " AND TrNo='" + aMdl.BillNo + "'", _trans, ConLab);
                    DeleteInsertLab("DELETE FROM tb_ph_PURCHASE_RETURN_DETAIL WHERE MasterId=" + aMdl.BillId + " AND ModuleName='Bill'", _trans, ConLab);
                    DeleteInsertLab("DELETE FROM tb_ph_STOCK_LEDGER WHERE MasterId=" + aMdl.BillId + " AND Module='Purchase'", _trans, ConLab);

                }
                else
                {
                    masterId = (int)cmd.ExecuteScalar();
                    aMdl.BillId = masterId;
                }
                foreach (var model in aMdl.ItemModels)
                {
                    DeleteInsertLab("INSERT INTO tb_ph_PURCHASE_RETURN_DETAIL(MasterId, ItemId, Qty, UnitPrice, UnitTotal)VALUES(" + aMdl.BillId + ", '" + model.Id + "', '" + model.Qty + "', '" + model.PurchasePrice + "', '" + model.UnitTotal + "')", _trans, ConLab);
                    Hlp.InsertIntoStockLedgerPharmacy(aMdl.BillId, aMdl.BillNo, aMdl.BillDate, model.Id, model.PurchasePrice, model.PurchasePrice, 0, model.Qty, "Purchase-Return",0,0, _trans, ConLab);
                }
                Hlp.InsertIntoPurchaseLedgerPharmacy(aMdl.Supplier.Id, aMdl.BillNo, aMdl.BillDate, aMdl.BillId, 0, 0, aMdl.TotAmount, 0, Hlp.UserName, _trans, ConLab);




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

        internal DataTable GetInvoiceList(DateTime dateTime, string searchString, string comeFrom)
        {
            try
            {
                string cond = "";
                if (searchString != "")
                {
                    cond = "AND (b.Name+a.BillNo) like '%" + searchString + "%'";
                }
                if (comeFrom == "enter")
                {
                    cond += " AND a.BillDate='" + dateTime.ToString("yyyy-MM-dd") + "'";
                }
                string query = @"SELECT a.BillNo,a.BillDate,b.Name AS SupplierName,a.PostedBy, a.TotAmt 
FROM tb_ph_PURCHASE_RETURN_MASTER a INNER JOIN tb_ph_SUPPLIER b ON a.SuppId=b.Id 
  " + cond + " Order by a.BillNo Desc";
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

        internal PurchaseModel GetInvoiceDataForEdit(string invNo)
        {
            int masterId = Convert.ToInt32(FncReturnFielValueLab("tb_ph_PURCHASE_MASTER", "BillNo='" + invNo + "'", "Id"));

            var aMdl = new PurchaseModel();
            var mdl = new List<ItemModel>();
            ConLab.Open();
            string query = @"EXEC Sp_Get_InvoicePrint " + masterId + ",''";
            var cmd = new SqlCommand("SELECT * FROM V_ph_Purchase_List WHERE BillNo='"+ invNo +"'", ConLab);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                aMdl.ReceiptNo = rdr["ReceiptNo"].ToString();
                aMdl.ReceiptDate = Convert.ToDateTime(rdr["ReceiptDate"]);
                aMdl.Supplier = new SupplierModel() { Id = Convert.ToInt32(rdr["SuppId"].ToString()) };
                aMdl.TotalItem = Convert.ToDouble(rdr["TotItem"]);
                aMdl.TotAmount = Convert.ToDouble(rdr["TotAmt"]);
                aMdl.NetAmount = Convert.ToDouble(rdr["NetAmt"]);
                aMdl.TotalVat = Convert.ToDouble(rdr["TotVat"]);
                aMdl.TotalComision = Convert.ToDouble(rdr["TotComision"]);
                aMdl.TotalLess = Convert.ToDouble(rdr["TotLess"]);
                aMdl.TotalPaid = Convert.ToDouble(rdr["TotPaid"]);
                aMdl.Remarks = rdr["Remarks"].ToString();

                mdl.Add(new ItemModel()
                {
                    Id = Convert.ToInt32(rdr["ItemId"]),
                    Name=rdr["ItemName"].ToString(),
                    Qty = Convert.ToDouble(rdr["Qty"]),
                    PurchasePrice = Convert.ToDouble(rdr["UnitPrice"]),
                    BQty = Convert.ToDouble(rdr["BQty"]),
                    UnitTotal = Convert.ToDouble(rdr["UnitTotal"]),
                    VatPc = Convert.ToDouble(rdr["VatPc"]),
                    Vat = Convert.ToDouble(rdr["VatAmt"]),
                    TaxPc = Convert.ToDouble(rdr["ComPc"]),
                    Tax = Convert.ToDouble(rdr["ComAmt"]),
                    Tp = Convert.ToDouble(rdr["Tp"]),
                    ExpireDate = Convert.ToDateTime(rdr["ExpireDate"]),
                });

                aMdl.ItemModels = mdl;


            }
            rdr.Close();
            ConLab.Close();
            return aMdl;


        }
        
     


        
        
    }
}
