using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model.Pharmacy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Gateway.Pharmacy
{
    class SupplierDuePaymentGateway : DbConnection
    {
        private SqlTransaction _trans;
        internal string Save(PurchaseModel aMdl)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();

               
                    aMdl.BillNo = GetInvoiceNo(6, _trans, ConLab);
                    aMdl.BillDate = Hlp.GetServerDate(ConLab, _trans);
               

                string query = "";


                query = @"INSERT INTO tb_ph_PURCHASE_DUE_PAYMENT(TrNo, TrDate, SuppId, PaymentAmt, LessAmt, Remarks, PostedBy, PcName, IpAddress)VALUES(@TrNo, FORMAT(getdate(),'yyyy-MM-dd'), @SuppId, @PaymentAmt, @LessAmt, @Remarks, @PostedBy, @PcName, @IpAddress)";
               

                var cmd = new SqlCommand(query, ConLab, _trans);
                cmd.Parameters.AddWithValue("@TrNo", aMdl.BillNo);
            
               

                cmd.Parameters.AddWithValue("@SuppId", aMdl.Supplier.Id);
                cmd.Parameters.AddWithValue("@PaymentAmt", aMdl.TotalPaid);
                cmd.Parameters.AddWithValue("@LessAmt", 0);

                cmd.Parameters.AddWithValue("@Remarks", aMdl.Remarks ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);
                cmd.Parameters.AddWithValue("@PcName", Environment.UserName);
                cmd.Parameters.AddWithValue("@IpAddress", Hlp.IpAddress());
                cmd.ExecuteNonQuery();
                    

                Hlp.InsertIntoPurchaseLedgerPharmacy(aMdl.Supplier.Id, aMdl.BillNo, aMdl.BillDate, 0, 0, 0, 0, aMdl.TotalPaid, Hlp.UserName, _trans, ConLab);




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
                    cond = "AND (b.Name+a.TrNo) like '%" + searchString + "%'";
                }
                if (comeFrom == "enter")
                {
                    cond += " AND a.TrDate='" + dateTime.ToString("yyyy-MM-dd") + "'";
                }
                string query = @"
SELECT a.TrNo AS BillNo,a.TrDate AS BillDate,b.Name AS SupplierName,a.PostedBy, a.PaymentAmt AS TotAmt 
FROM tb_ph_PURCHASE_DUE_PAYMENT a INNER JOIN tb_ph_SUPPLIER b ON a.SuppId=b.Id  WHERE 1=1 "+ cond +"  Order by a.TrNo Desc";
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
            var cmd = new SqlCommand("SELECT * FROM V_ph_Purchase_List WHERE BillNo='" + invNo + "'", ConLab);
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
                    Name = rdr["ItemName"].ToString(),
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
