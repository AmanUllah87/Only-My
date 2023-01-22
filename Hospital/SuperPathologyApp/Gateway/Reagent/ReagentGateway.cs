using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model.Reagent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Gateway.Reagent
{
    public class ReagentGateway : DbConnection
    {

        internal string Save(ReagentModel mdl)
        {
            try
            {
              
              
                string query = "";
                query = FnSeekRecordNewLab("tb_REAGENT", "Id=" + mdl.ReagentId + "") ? "Update tb_REAGENT SET Name=@Name, Type=@Type, ReorderLevel=@ReorderLevel, DeptName=@DeptName,  IsExpire=@IsExpire,  LastUpdateDtls=@LastUpdateDtls  WHERE Id=@Id" : @"INSERT INTO tb_REAGENT(Name, Type, ReorderLevel, DeptName,  IsExpire,  LastUpdateDtls) VALUES (@Name, @Type, @ReorderLevel, @DeptName,  @IsExpire,  @LastUpdateDtls)";
                var cmd = new SqlCommand(query, ConLab);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", mdl.ReagentId);
                cmd.Parameters.AddWithValue("@Name", mdl.Name);
                cmd.Parameters.AddWithValue("@Type", mdl.Type );
                cmd.Parameters.AddWithValue("@ReorderLevel", mdl.ReorderLevel );
                cmd.Parameters.AddWithValue("@IsExpire", mdl.IsExpire);
                cmd.Parameters.AddWithValue("@DeptName", mdl.DeptName);
                cmd.Parameters.AddWithValue("@LastUpdateDtls", Hlp.UserName+":"+DateTime.Now.ToString());



                ConLab.Open();
                    cmd.ExecuteNonQuery();
                ConLab.Close();

                return SaveSuccessMessage;
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

        internal DataTable GetTestCodeList(int id, string searchString)
        {
            try
            {
                string cond = "", vaqCond = "";
                if (searchString != "0")
                {
                    cond = "AND (convert(varchar,Id)+Name) like '%'+'" + searchString + "'+'%'";
                }
                if (id != 0)
                {
                    cond = "AND Id=" + id + "";
                }






                string query = @"SELECT Id,Name,Type,DeptName,ReorderLevel,IsExpire FROM tb_REAGENT WHERE 1=1  " + cond + " " + vaqCond + " Order by id";

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

    


    }
}
