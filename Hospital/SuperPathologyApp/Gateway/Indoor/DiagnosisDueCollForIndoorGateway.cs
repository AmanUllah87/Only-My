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

namespace SuperPathologyApp.Gateway.Indoor
{
    class DiagnosisDueCollForIndoorGateway:DbConnection
    {
        public List<BillModel> GetDueInvoiceDataByAdmId(int admId)
        {
            try
            {
                var mdl = new List<BillModel>();
                ConLab.Open();
                string query = @"SELECT a.MasterId,b.BillNo,b.BillDate,b.PatientName,b.Age,b.Sex,b.MobileNo,c.Name As ConsDrName,c.Id AS ConsDrId,
Isnull(SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt),0) AS Balance,b.TotalAmt AS BillAmt,b.RefDrId,d.Name AS RefDrName,b.AdmId
FROM tb_BILL_LEDGER a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id
INNER JOIN tb_Doctor c ON b.UnderDrId=c.Id
INNER JOIN tb_Doctor d ON b.RefDrId=d.Id WHERE b.AdmId='" + admId + "' GROUP BY a.MasterId,b.PatientName,b.Age,b.Sex,b.MobileNo,b.BillDate,b.BillNo,c.Name,c.Id,b.TotalAmt,b.RefDrId,d.Name,b.AdmId HAVING SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt)>0 ";
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
                aMdl.TrNo = GetInvoiceNo(2, _trans, ConLab);

                double totColl = 0;
                foreach (var item in aMdl.Bill){totColl += item.CollAmt;}


                const string query = "INSERT INTO tb_DUE_COLL (TrNo, TrDate, MasterId, TotalAmt, LessAmt, LessFrom, Remarks, PostedBy,LessPc,LessType,TrTime,PcName,IpAddress,AdmId)VALUES(@TrNo, @TrDate, @MasterId, @TotalAmt, @LessAmt, @LessFrom, @Remarks, @PostedBy,@LessPc,@LessType,@TrTime,@PcName,@IpAddress,@AdmId)";
                var cmd = new SqlCommand(query, ConLab, _trans);
                cmd.Parameters.AddWithValue("@TrNo", aMdl.TrNo);
                cmd.Parameters.AddWithValue("@TrDate", Hlp.GetServerDate(ConLab, _trans).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MasterId", 0);

                cmd.Parameters.AddWithValue("@TotalAmt", totColl);
                cmd.Parameters.AddWithValue("@LessAmt", aMdl.TotLessAmt);
                cmd.Parameters.AddWithValue("@LessFrom", "Company");
                cmd.Parameters.AddWithValue("@Remarks", aMdl.Remarks);
                cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);

                cmd.Parameters.AddWithValue("@LessPc", aMdl.TotLessAmt);
                cmd.Parameters.AddWithValue("@LessType", "Tk");
                cmd.Parameters.AddWithValue("@TrTime", DateTime.Now.ToString("hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@PcName", Environment.UserName);
                cmd.Parameters.AddWithValue("@IpAddress", Hlp.IpAddress());
                //
                cmd.Parameters.AddWithValue("@AdmId", aMdl.Admission.AdmId);

                cmd.ExecuteNonQuery();

                foreach (var item in aMdl.Bill)
                {
                    Hlp.InsertIntoLedger(aMdl.TrNo, Hlp.GetServerDate(ConLab, _trans), item.BillId, 0, item.LessPc, 0, item.CollAmt, Hlp.UserName, _trans, ConLab);
                }

              
                int diagnosisBill = Convert.ToInt32(FncReturnFielValueLab("tb_in_FIXED_ID", "Name='Diagnosis Bill'", "Id", _trans, ConLab));
                var mdl = Hlp.GetRegAndBedId(aMdl.Admission.AdmId, ConLab, _trans);
                Hlp.InsertIntoPatientLedger(aMdl.TrNo, Hlp.GetServerDate(ConLab, _trans), mdl.Patient.RegId, aMdl.Admission.AdmId, mdl.Bed.BedId, diagnosisBill, "Diagnosis Bill", 0, 1, 0, aMdl.TotLessAmt, totColl, 0,0, 0, 0, 0, ConLab, _trans);

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

                string query = @"SELECT a.TrNo,a.TrDate,b.PtName AS  PatientName,b.ContactNo MobileNo,a.TotalAmt,b.BedName AS Bed
FROM tb_DUE_COLL  a INNER JOIN V_Admission_List b ON a.AdmId=b.Id 
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
