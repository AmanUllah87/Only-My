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
    class DueCollForIndoorGateway : DbConnection
    {
        public List<BillModel> GetDueInvoiceDataByAdmId(int admId)
        {
            try
            {
                var mdl = new List<BillModel>();
                ConLab.Open();
                string query = @"SELECT a.MasterId,b.BillNo,b.BillDate,c.PtName AS PatientName,c.PtAge AS Age,c.PtSex AS Sex,c.ContactNo AS MobileNo,Isnull(SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt),0) AS Balance,b.TotAmt AS BillAmt,b.AdmId
FROM tb_ph_SALES_LEDGER a INNER JOIN tb_ph_SALES_MASTER b ON a.MasterId=b.Id AND b.AdmId<>0
INNER JOIN tb_in_Admission c ON b.AdmId=c.Id
WHERE b.AdmId='"+ admId +"' GROUP BY a.MasterId,c.PtName,c.PtAge,c.PtSex,c.ContactNo,b.BillDate,b.BillNo,b.TotAmt,b.AdmId HAVING SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt)>0";
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

              //  double totColl = aMdl.Bill.Sum(item => item.CollAmt);


                const string query = "INSERT INTO tb_ph_DUE_COLL ( TrNo, TrDate, MasterId, CollectionAmt, LessAmt, Remarks, PostedBy, PcName, IpAddress, AdmId)VALUES(@TrNo, @TrDate, @MasterId, @CollectionAmt, @LessAmt, @Remarks, @PostedBy, @PcName, @IpAddress, @AdmId)";
                var cmd = new SqlCommand(query, ConLab, _trans);
                cmd.Parameters.AddWithValue("@TrNo", aMdl.TrNo);
                cmd.Parameters.AddWithValue("@TrDate", Hlp.GetServerDate(ConLab, _trans).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MasterId", 0);

                cmd.Parameters.AddWithValue("@CollectionAmt", aMdl.TotCollAmt);
                cmd.Parameters.AddWithValue("@LessAmt", aMdl.TotLessAmt);
                cmd.Parameters.AddWithValue("@Remarks", aMdl.Remarks);
                cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);
                cmd.Parameters.AddWithValue("@PcName", Environment.UserName);
                cmd.Parameters.AddWithValue("@IpAddress", Hlp.IpAddress());
                cmd.Parameters.AddWithValue("@AdmId", aMdl.Admission.AdmId);
                cmd.ExecuteNonQuery();


                Hlp.InsertIntoSalesLedgerPharmacy(0, aMdl.TrNo, Hlp.GetServerDate(ConLab, _trans), aMdl.Admission.AdmId, 0, aMdl.TotLessAmt, 0, aMdl.TotCollAmt,0, Hlp.UserName, aMdl.Admission.AdmId, _trans, ConLab);


                int medicineBill = Convert.ToInt32(FncReturnFielValueLab("tb_in_FIXED_ID", "Name='Medicine Bill'", "Id", _trans, ConLab));
                var mdl = Hlp.GetRegAndBedId(aMdl.Admission.AdmId, ConLab, _trans);
                Hlp.InsertIntoPatientLedger(aMdl.TrNo, Hlp.GetServerDate(ConLab, _trans), mdl.Patient.RegId, aMdl.Admission.AdmId, mdl.Bed.BedId, medicineBill, "Medicine Bill", 0, 1, 0, aMdl.TotLessAmt, aMdl.TotCollAmt, 0, 0,0, 0, 0, ConLab, _trans);

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
                    cond = "AND (b.PtName+a.TrNo+b.ContactNo+b.BedName) like '%'+'" + searchString + "'+'%'";
                }
                else
                {
                    cond = " AND a.TrDate='" + dateTime.ToString("yyyy-MM-dd") + "'";
                }

                string query = @"SELECT a.TrNo,a.TrDate,b.PtName AS  PatientName,b.ContactNo MobileNo,a.CollectionAmt AS TotalAmt,b.BedName AS Bed
FROM tb_ph_DUE_COLL  a INNER JOIN V_Admission_List b ON a.AdmId=b.Id 
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
