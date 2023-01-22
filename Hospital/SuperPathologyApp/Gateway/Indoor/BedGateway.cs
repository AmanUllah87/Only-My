using System;
using System.Data;
using System.Data.SqlClient;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Indoor;

namespace SuperPathologyApp.Gateway.Indoor
{
    class BedGateway : DbConnection
    {
        private SqlTransaction _trans;
        internal string Save(BedModel mBed)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                string query = "";
                query = FnSeekRecordNewLab("tb_IN_BED", "Id=" + mBed.BedId + "", _trans, ConLab) ? "" +
                        "Update tb_IN_BED SET Name=@Name, Floor=@Floor,BedType=@BedType,Charge=@Charge ,SrvCharge=@SrvCharge,PostedBy=@PostedBy,DeptId=@DeptId WHERE Id=@Id" : @"INSERT INTO tb_IN_BED(Name, Floor, BedType, Charge, SrvCharge,  PostedBy,DeptId) VALUES (@Name, @Floor, @BedType, @Charge, @SrvCharge,  @PostedBy,@DeptId)";
                var cmd = new SqlCommand(query, ConLab,_trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", mBed.BedId);
                cmd.Parameters.AddWithValue("@Name", mBed.Name);
                cmd.Parameters.AddWithValue("@Floor", mBed.Floor ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@BedType", mBed.BedType ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Charge", mBed.Charge);
                cmd.Parameters.AddWithValue("@SrvCharge", mBed.SrvCharge);
                cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);
                cmd.Parameters.AddWithValue("@DeptId", mBed.Department.DeptId);
                
                
                cmd.ExecuteNonQuery();
                _trans.Commit();
                ConLab.Close();

                if (mBed.BedId!=0)
                {
                    return UpdateSuccessMessage;
                }
                else
                {
                    return SaveSuccessMessage;    
                }

                
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

        internal DataTable GetBedList(int id, string searchString)
        {
            try
            {
                string cond = "";
                if (searchString != "0")
                {
                    cond = "AND (convert(varchar,Id)+Name+Floor+BedType+DeptName) like '%'+'" + searchString + "'+'%'";
                }
                if (id != 0)
                {
                    cond = "AND Id=" + id + "";
                }
                string query = @"SELECT * FROM V_IN_BED_WITH_DEPT WHERE 1=1  " + cond + "";

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
