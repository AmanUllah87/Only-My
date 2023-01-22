using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
 

namespace SuperPathologyApp.Gateway
{
    class DoctorGateway:DbConnection
    {
        private SqlTransaction _trans;
        internal string Save(DoctorModel mLabDoctor)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                string query = "";
                query = FnSeekRecordNewLab("tb_Doctor", "Id=" + mLabDoctor.DrId + "", _trans, ConLab) ? "Update tb_Doctor SET Name=@Name, Address=@Address,ContactNo=@ContactNo,TakeCom=@TakeCom ,UserName=@UserName,MpoId=@MpoId WHERE Id=@Id" : @"INSERT INTO tb_Doctor(Name, Address,ContactNo,TakeCom,UserName,MpoId) VALUES (@Name, @Address,@ContactNo,@TakeCom,@UserName,@MpoId)";
                var cmd = new SqlCommand(query, ConLab,_trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", mLabDoctor.DrId);
                cmd.Parameters.AddWithValue("@Name", mLabDoctor.Name);
                cmd.Parameters.AddWithValue("@Address", mLabDoctor.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ContactNo", mLabDoctor.ContactNo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TakeCom", mLabDoctor.TakeCommision);
                cmd.Parameters.AddWithValue("@UserName", Hlp.UserName);
                cmd.Parameters.AddWithValue("@MpoId", mLabDoctor.Mpo.Id);
                
                
                cmd.ExecuteNonQuery();


                if (mLabDoctor.TakeCommision==0)
                {
                    DeleteInsertLab("DELETE FROM tb_DrWiseHonorium WHERE DrId='"+ mLabDoctor.DrId +"'",_trans,ConLab);
                }


                _trans.Commit();
                ConLab.Close();
                
                return SaveSuccessMessage;
            }
            catch (Exception exception)
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    ConLab.Close();
                }
                return exception.Message;
            }
        }

        internal DataTable GetDoctorList(int id, string searchString)
        {
            try
            {
                string cond = "";
                if (searchString != "0")
                {
                    cond = "AND (convert(varchar,Id)+Name) like '%'+'" + searchString + "'+'%'";
                }
                if (id != 0)
                {
                    cond = "AND Id=" + id + "";
                }
                string query = @"SELECT * FROM tb_Doctor WHERE 1=1  " + cond + "";

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
