using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model.Diagnosis;
using SuperPathologyApp.Model.Indoor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Gateway.Pharmacy
{
    class DueCollForRegisterPatientGateway : DbConnection
    {
        public List<BillModel> GetDueInvoiceDataByRegId(int regId)
        {
            try
            {
                var mdl = new List<BillModel>();
                ConLab.Open();
                string query = @"SELECT a.MasterId,b.BillNo,b.BillDate,c.Name AS PatientName,'' AS Age,c.Sex AS Sex,c.ContactNo AS MobileNo,Isnull(SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt),0) AS Balance,b.TotAmt AS BillAmt,b.AdmId
FROM tb_ph_SALES_LEDGER a INNER JOIN tb_ph_SALES_MASTER b ON a.MasterId=b.Id AND b.AdmId=0
INNER JOIN tb_PATIENT c ON a.CustId=c.Id
WHERE a.CustId='"+ regId +"' GROUP BY a.MasterId,c.Name,c.Sex,c.ContactNo,b.BillDate,b.BillNo,b.TotAmt,b.AdmId HAVING SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt)>0";
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    mdl.Add(new BillModel()
                    {
                        BillId = Convert.ToInt32(rdr["MasterId"].ToString()),
                        BillNo = rdr["BillNo"].ToString(),
                        BillDate = Convert.ToDateTime(rdr["BillDate"].ToString()),
                        TotalAmt = Convert.ToDouble(rdr["Balance"].ToString()),
                    });
                }
                rdr.Close();
                ConLab.Close();
                return mdl;
            }
            catch (Exception)
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                throw;
            }
           

            
        }
        SqlTransaction _trans;
        internal string Save(ReleaseModel aMdl)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                aMdl.TrNo = GetInvoiceNo(5, _trans, ConLab);

                double totColl = aMdl.Bill.Sum(item => item.CollAmt);


                const string query = "INSERT INTO tb_ph_DUE_COLL ( TrNo, TrDate, MasterId, CollectionAmt, LessAmt, Remarks, PostedBy, PcName, IpAddress, CustId,AdmId)VALUES(@TrNo, @TrDate, @MasterId, @CollectionAmt, @LessAmt, @Remarks, @PostedBy, @PcName, @IpAddress, @AdmId,0)";
                var cmd = new SqlCommand(query, ConLab, _trans);
                cmd.Parameters.AddWithValue("@TrNo", aMdl.TrNo);
                cmd.Parameters.AddWithValue("@TrDate", Hlp.GetServerDate(ConLab, _trans).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MasterId", 0);

                cmd.Parameters.AddWithValue("@CollectionAmt", totColl);
                cmd.Parameters.AddWithValue("@LessAmt", aMdl.TotLessAmt);
                cmd.Parameters.AddWithValue("@Remarks", aMdl.Remarks);
                cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);
                cmd.Parameters.AddWithValue("@PcName", Environment.UserName);
                cmd.Parameters.AddWithValue("@IpAddress", Hlp.IpAddress());
                cmd.Parameters.AddWithValue("@AdmId", aMdl.Patient.RegId);
                cmd.ExecuteNonQuery();

                foreach (var item in aMdl.Bill)
                {
                    Hlp.InsertIntoSalesLedgerPharmacy(aMdl.Patient.RegId, aMdl.TrNo, Hlp.GetServerDate(ConLab, _trans), item.BillId, 0, item.LessPc, 0, item.CollAmt,0, Hlp.UserName, 0, _trans, ConLab);
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


        internal DataTable GetDueCollList(DateTime dateTime, string searchString)
        {
            try
            {
                string cond = "";
                if (searchString != "")
                {
                    cond = "AND (b.Name+a.TrNo+b.ContactNo+b.Address) like '%'+'" + searchString + "'+'%'";
                }
                else
                {
                    cond = " AND a.TrDate='" + dateTime.ToString("yyyy-MM-dd") + "'";
                }

                string query = @"SELECT a.TrNo,a.TrDate,b.Name AS  PatientName,b.ContactNo MobileNo,a.CollectionAmt AS TotalAmt,b.Address 
FROM tb_ph_DUE_COLL  a INNER JOIN tb_PATIENT b ON a.CustId=b.Id 
" + cond + " Order by a.TrNo Desc";

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
                    _trans.Rollback();
                    ConLab.Close();
                }
                throw;
            }
        }
    }
}
