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
    class GroupSetupGateway:DbConnection
    {


        public DataTable GetTestCodeList(string text)
        {

            try
            {
                string query = "", condition = "";
                if (text != "")
                {
                    condition = " WHERE (CONVERT(varchar(10),Id)+Name)  like '%" + text + "%'";
                }

                query = @"SELECT Id,Name FROM tb_TESTCHART  " + condition + " Order By Id";

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

        public DataTable GetList(string searchString,string fieldName)
        {

            try
            {
                string query = "", condition = "";
                if (searchString != "")
                {
                    condition = " AND  "+ fieldName +"  like '%" + searchString + "%'";
                }




                query = @"SELECT distinct "+ fieldName +",'' AS Blank FROM tb_TESTCHART_PARAM WHERE LEN("+ fieldName +")>0";

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
        internal string Save(VaqutainerModel mdl)
        {
            try
            {
                if (FnSeekRecordNewLab("tb_VacutainerSetup","ItemId='"+ mdl.ItemId +"'") )
                {
                    DeleteInsertLab("DELETE FROM tb_VacutainerSetup WHERE ItemId='"+ mdl.ItemId  +"'");
                }

                const string query = "INSERT INTO tb_VacutainerSetup(GroupId, VaqGroupId, ItemId, ItemDesc, MasterCode) VALUES (@GroupId, @VaqGroupId, @ItemId, @ItemDesc, @MasterCode)";
                var cmd = new SqlCommand(query, ConLab);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@GroupId", mdl.GroupId);
                cmd.Parameters.AddWithValue("@VaqGroupId", mdl.VaqGroupId);
                cmd.Parameters.AddWithValue("@ItemId", mdl.ItemId);
                cmd.Parameters.AddWithValue("@ItemDesc", mdl.ItemDesc);
                cmd.Parameters.AddWithValue("@MasterCode", mdl.MasterCode );
                ConLab.Open();
                cmd.ExecuteNonQuery();
                ConLab.Close();
                return SaveSuccessMessage; 
            
            }
            catch (Exception ex)
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                };
                return  ex.Message ;
            }
        }




        public DataTable GetListOfVaq(string code)
        {
            try
            {
                string query = @"SELECT * FROM VW_Get_Vaq_GroupName  WHERE ItemId='"+ code +"'";
                ConLab.Open();
                var da = new SqlDataAdapter(query, ConLab);
                var ds = new DataSet();
                da.Fill(ds, "Foreigner");
                var table = ds.Tables["Foreigner"];
                ConLab.Close();
                return table;
            }
            catch (Exception )
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                };
                throw;
            }
        }





      

    }
}
