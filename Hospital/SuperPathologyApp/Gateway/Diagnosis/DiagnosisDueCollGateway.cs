using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model.Diagnosis;

namespace SuperPathologyApp.Gateway.Diagnosis
{
    class DiagnosisDueCollGateway:DbConnection
    {
        private SqlTransaction _trans;
      

        internal DataTable GetDueList(int id, string searchString)
        {
            try
            {
                string cond = "";
                if (id!= 0)
                {
                    cond += " AND MasterId="+ id +"";
                }

                if (searchString != "")
                {
                    cond += "AND (BillNo+PatientName+MobileNo) like '%'+'" + searchString + "'+'%'";
                }

                string query = @"SELECT MasterId,BillNo,BillDate,PatientName,MobileNo,Balance FROM V_Due_Invoice_List WHERE 1=1  " + cond + "";

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
                    cond = "AND (b.PatientName+a.TrNo+b.MobileNo+b.BillNo) like '%'+'" + searchString + "'+'%'";
                }
                else
                {
                    cond = " AND a.TrDate='" + dateTime.ToString("yyyy-MM-dd") + "'";
                }

                string query = @"SELECT a.MasterId, a.TrNo,a.TrDate,b.PatientName,b.MobileNo,a.TotalAmt,b.BillNo FROM tb_DUE_COLL  a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id  " + cond + " Order by a.TrNo Desc";

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
                const string query = "INSERT INTO tb_DUE_COLL (TrNo, TrDate, MasterId, TotalAmt, LessAmt, LessFrom, Remarks, PostedBy,LessPc,LessType,TrTime,PcName,IpAddress)VALUES(@TrNo, FORMAT(getdate(),'yyyy-MM-dd'), @MasterId, @TotalAmt, @LessAmt, @LessFrom, @Remarks, @PostedBy,@LessPc,@LessType,@TrTime,@PcName,@IpAddress)";
                var cmd = new SqlCommand(query, ConLab, _trans);
                cmd.Parameters.AddWithValue("@TrNo", aMdl.BillNo);
                cmd.Parameters.AddWithValue("@TrDate", Hlp.GetServerDate(ConLab,_trans).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@MasterId", aMdl.BillId);
              
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

                Hlp.InsertIntoLedger(aMdl.BillNo, Hlp.GetServerDate(ConLab, _trans), aMdl.BillId, 0, aMdl.TotalLessAmt, 0, aMdl.CollAmt, Hlp.UserName, _trans, ConLab);


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
