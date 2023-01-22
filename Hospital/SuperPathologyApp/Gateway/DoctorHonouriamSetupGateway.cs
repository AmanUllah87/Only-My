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
    class DoctorHonouriamSetupGateway : DbConnection
    {
        private SqlTransaction _trans;
        internal string Save(DoctorModel mLabDoctor)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                DeleteInsertLab("DELETE FROM tb_DrWiseHonorium WHERE DrId='"+ mLabDoctor.DrId +"'",_trans,ConLab);
                foreach (var model in mLabDoctor.SubProject)
                {
                    string query = @"INSERT INTO tb_DrWiseHonorium(DrId, SubProjectId,HonouriamAmt,Type,UserName) VALUES (@DrId, @SubProjectId,@HonouriamAmt,@Type,'"+ Hlp.UserName +"')";
                    var cmd = new SqlCommand(query, ConLab, _trans);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@DrId", mLabDoctor.DrId);
                    cmd.Parameters.AddWithValue("@SubProjectId", model.SubProjectId);
                    cmd.Parameters.AddWithValue("@HonouriamAmt", model.HnrAmt);
                    cmd.Parameters.AddWithValue("@Type", model.Type);
                    cmd.ExecuteNonQuery();
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
                    cond = "AND Name like '%'+'" + searchString + "'+'%'";
                }
                if (id != 0)
                {
                    cond = "AND Id=" + id + "";
                }
                string query = @"SELECT * FROM tb_Doctor WHERE Valid=1  " + cond + "";

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
                throw;
            }
        }
        
        internal List<DoctorModel> GetLabDoctor(string searchString,string type)
        {
            try
            {
                var lists = new List<DoctorModel>();
                string cond = " AND Type='"+ type +"'";
                if (searchString != "")
                {
                    cond += "AND Name like '%'+'" + searchString + "'+'%'";
                }
                string query = @"SELECT * FROM tb_DoctorSetup WHERE IsShow=1  " + cond + "";

                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new DoctorModel()
                    {
                        DrId =Convert.ToInt32(rdr["Id"]),
                       
                    });
                }
                rdr.Close();
                ConLab.Close();
                return lists;
            }
            catch (Exception )
            {
                throw;
            }
        }
        
        internal string Update(DoctorModel mLabDoctor)
        {
            try
            {
                ConLab.Open();
                const string query = @"UPDATE tb_DoctorSetup SET  Details=@DrDetails,Name=@DrName,Type=@DrType WHERE Id=@Id";
                var cmd = new SqlCommand(query, ConLab);
                cmd.Parameters.Clear();
                //cmd.Parameters.AddWithValue("@Id", mLabDoctor.DrCode);
                //cmd.Parameters.AddWithValue("@DrDetails", mLabDoctor.Details);
                //cmd.Parameters.AddWithValue("@DrName", mLabDoctor.DrName);
                //cmd.Parameters.AddWithValue("@DrType", mLabDoctor.DrType);

                cmd.ExecuteNonQuery();
                ConLab.Close();
                return UpdateSuccessMessage;
            }
            catch (Exception exception)
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                return exception.Message;
            }
        }


        
        internal List<SubProjectModel> GetProjectList(int drId)
        {
            try
            {
                const string query = @"SELECT Id,Name,HnrAmt,Type FROM tb_SubProject Order by Id";
                var lists = new List<SubProjectModel>();
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new SubProjectModel()
                    {
                        SubProjectId = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
                        HnrAmt = Convert.ToDouble(rdr["HnrAmt"]),
                        Type = rdr["Type"].ToString(),
                    });
                }
                rdr.Close();
                ConLab.Close();

                
                if (FnSeekRecordNewLab("tb_DrWiseHonorium","DrId="+ drId +""))
	            {
                    foreach (var item in lists)
                    {
                        if (FnSeekRecordNewLab("tb_DrWiseHonorium","DrId="+ drId +" AND SubProjectId="+ item.SubProjectId +""))
                        {
                            item.HnrAmt =Convert.ToDouble(FncReturnFielValueLab("tb_DrWiseHonorium", "DrId=" + drId + " AND SubProjectId=" + item.SubProjectId + "", "HonouriamAmt"));
                            item.Type = FncReturnFielValueLab("tb_DrWiseHonorium", "DrId=" + drId + " AND SubProjectId=" + item.SubProjectId + "", "Type");
                        }
                    }
	            }
                return lists;
            }
            catch (Exception exception)
            {
                throw;
            }
        }
    }
}
