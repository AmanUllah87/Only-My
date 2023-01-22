using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model.Diagnosis;

namespace SuperPathologyApp.Gateway.Indoor
{
    class IndoorDueCollGateway : DbConnection
    {
        private SqlTransaction _trans;
      

        internal DataTable GetDueList(int id, string searchString)
        {
            try
            {
                string cond = "";
                if (id!= 0)
                {
                    cond += " AND AdmId="+ id +"";
                }

                if (searchString != "")
                {
                    cond += "AND (BillNo+PtName+ContactNo) like '%'+'" + searchString + "'+'%'";
                }

                string query = @"SELECT AdmId AS MasterId, BillNo AS ReleaseNo,ReleaseDate,PtName AS PatientName,ContactNo AS  MobileNo,DueAmt AS Balance FROM V_in_DueList  WHERE 1=1  " + cond + "";

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


        internal DataTable GetDueCollList(DateTime dateTime, string searchString)
        {
            try
            {
                string cond = "";
                if (searchString != "")
                {
                    cond = "AND (b.PtName+a.TrNo+b.ContactNo) like '%'+'" + searchString + "'+'%'";
                }
                else
                {
                    cond = " AND a.TrDate='" + dateTime.ToString("yyyy-MM-dd") + "'";
                }

                string query = @"SELECT a.AdmId , a.TrNo,a.TrDate,b.PtName AS  PatientName,b.ContactNo AS MobileNo,a.TotalAmt FROM tb_DUE_COLL  a  INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id   " + cond + " Order by a.TrNo Desc  "; 
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




        internal string SaveDueColl(BillModel aMdl)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                aMdl.BillNo = GetInvoiceNo(2, _trans, ConLab);
                const string query = "INSERT INTO tb_DUE_COLL (TrNo, TrDate, MasterId, TotalAmt, LessAmt, LessFrom, Remarks, PostedBy,LessPc,LessType,TrTime,PcName,IpAddress,AdmId)VALUES(@TrNo, @TrDate, @MasterId, @TotalAmt, @LessAmt, @LessFrom, @Remarks, @PostedBy,@LessPc,@LessType,@TrTime,@PcName,@IpAddress,@AdmId)";
                var cmd = new SqlCommand(query, ConLab, _trans);
                cmd.Parameters.AddWithValue("@TrNo", aMdl.BillNo);
                cmd.Parameters.AddWithValue("@TrDate", Hlp.GetServerDate(ConLab, _trans).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MasterId", 0);
                cmd.Parameters.AddWithValue("@AdmId", aMdl.BillId);
                cmd.Parameters.AddWithValue("@TotalAmt", aMdl.CollAmt);
                cmd.Parameters.AddWithValue("@LessAmt", aMdl.TotalLessAmt);
                cmd.Parameters.AddWithValue("@LessFrom", aMdl.LessFrom);
                cmd.Parameters.AddWithValue("@Remarks", aMdl.Remarks);
                cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);

                cmd.Parameters.AddWithValue("@LessPc", aMdl.LessPc);
                cmd.Parameters.AddWithValue("@LessType", aMdl.LessType ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TrTime", DateTime.Now.ToString("hh:mm:ss tt"));
                cmd.Parameters.AddWithValue("@PcName", Environment.UserName);
                cmd.Parameters.AddWithValue("@IpAddress", Hlp.IpAddress());




                cmd.ExecuteNonQuery();
                var mdl = Hlp.GetRegAndBedId(aMdl.BillId, ConLab, _trans);
                int dueCollId = Convert.ToInt32(FncReturnFielValueLab("tb_in_FIXED_ID", "Name='Due Collection'", "Id", _trans, ConLab));
                Hlp.InsertIntoPatientLedger(aMdl.BillNo, Hlp.GetServerDate(ConLab, _trans), mdl.Patient.RegId, aMdl.BillId, mdl.Bed.BedId, dueCollId, "Due Collection", 0, 1, 0, aMdl.TotalLessAmt, aMdl.CollAmt, 0, 0,0, 0, 0, ConLab, _trans);


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
