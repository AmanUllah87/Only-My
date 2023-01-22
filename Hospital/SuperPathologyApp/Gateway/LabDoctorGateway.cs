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
    class LabDoctorGateway : DbConnection
    {
        private SqlTransaction _trans;
        internal string Save(DoctorModel mLabDoctor)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                string query = "";
                query = FnSeekRecordNewLab("tb_LAB_DOCTOR", "Id=" + mLabDoctor.DrId + "", _trans, ConLab) ? "Update tb_LAB_DOCTOR SET Name=@Name, Degree=@Degree,Position=@Position WHERE Id=@Id" : @"INSERT INTO tb_LAB_DOCTOR(Name, Degree,Position) VALUES (@Name, @Degree,@Position)";
                var cmd = new SqlCommand(query, ConLab,_trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", mLabDoctor.DrId);
                cmd.Parameters.AddWithValue("@Name", mLabDoctor.Name);
                cmd.Parameters.AddWithValue("@Degree", mLabDoctor.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Position", mLabDoctor.ContactNo ?? (object)DBNull.Value);
                cmd.ExecuteNonQuery();
                _trans.Commit();
                ConLab.Close();

                if (mLabDoctor.DrId == 0)
                {
                    return SaveSuccessMessage;
                }
                else
                {
                    return UpdateSuccessMessage;
                }


               // return SaveSuccessMessage;
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
                string query = @"SELECT * FROM tb_LAB_DOCTOR WHERE 1=1  " + cond + "";

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
