using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model.Pharmacy;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace SuperPathologyApp.Gateway.Reagent
{
    class ReagentSupplierGateway : DbConnection
    {
        private SqlTransaction _trans;
        internal string Save(SupplierModel mdl)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                string query = "";
                query = FnSeekRecordNewLab("tb_REAGENT_SUPPLIER", "Id=" + mdl.Id + "", _trans, ConLab) ? "Update tb_REAGENT_SUPPLIER SET Name=@Name, Address=@Address,ContactNo=@ContactNo WHERE Id=@Id" : @"INSERT INTO tb_REAGENT_SUPPLIER(Name, Address,ContactNo) VALUES (@Name, @Address,@ContactNo)";
                var cmd = new SqlCommand(query, ConLab,_trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", mdl.Id);
                cmd.Parameters.AddWithValue("@Name", mdl.Name);
                cmd.Parameters.AddWithValue("@Address", mdl.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ContactNo", mdl.MobileNo ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();

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

        internal DataTable GetSupplierList(int id, string searchString)
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
                string query = @"SELECT * FROM tb_REAGENT_SUPPLIER WHERE 1=1  " + cond + "";

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
