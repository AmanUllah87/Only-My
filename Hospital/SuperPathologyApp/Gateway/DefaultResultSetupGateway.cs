using System.Windows.Forms;
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
    class DefaultResultSetupGateway : DbConnection
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
        internal string Save(DataGridView dataGrid,string pCode)
        {
            try
            {
                ConLab.Open();
                for (int i = 0; i < dataGrid.Rows.Count; i++)
                {
                    const string query = "INSERT INTO tb_TESTCHART_DEF_RESULT(TestChartId,MachineParam,Result) VALUES (@TestChartId,@MachineParam,@Result)";
                    var cmd = new SqlCommand(query, ConLab);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TestChartId", pCode);
                    cmd.Parameters.AddWithValue("@MachineParam", dataGrid.Rows[i].Cells[0].Value.ToString());
                    cmd.Parameters.AddWithValue("@Result", dataGrid.Rows[i].Cells[1].Value.ToString());
                    cmd.ExecuteNonQuery();
                }
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
        internal List<TestCodeModel> ChildCodeDetails(string masterCode)
        {
            try
            {
                var lists = new List<TestCodeModel>();
                string query = @"SELECT MachineParam,Result FROM tb_TESTCHART_DEF_RESULT WHERE TestChartId='" + masterCode + "' Order By Id";
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    lists.Add(new TestCodeModel()
                    {
                        ParameterName = rdr["MachineParam"].ToString(),
                        Result = rdr["Result"].ToString(),
                    });
                }


                rdr.Close();
                ConLab.Close();
                return lists;
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
        internal DataTable GetParamNameByTestCode(string masterCode)
        {
            try
            {
                string query = @"SELECT distinct 0 AS Id,MachineParam As AliasName FROM tb_TESTCHART_PARAM WHERE  TestChartId='" + masterCode + "'";
              


                ConLab.Open();
                var da = new SqlDataAdapter(query, ConLab);
                var ds = new DataSet();
                da.Fill(ds, "pCode");
                var table = ds.Tables["pCode"];
                ConLab.Close();
                return table;






                //rdr.Close();
                //ConLab.Close();
                //if (lists.Count == 1)
                //{
                //    lists.Add(new UnitModel() { UnitName = "", UnitId = 0 });
                //}
                //return lists;
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
        internal List<UnitModel> GetAllDefaultResult(string masterCode)
        {
            try
            {
                string cond = "";
                if (masterCode!="")
                {
                    cond = " AND Result Like '%"+ masterCode +"%'";
                }
                var lists = new List<UnitModel>();
                string query = @"SELECT distinct 0 AS Id,Result FROM tb_TESTCHART_DEF_RESULT WHERE  1=1 " + cond + "";
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new UnitModel()
                    {
                        UnitId = Convert.ToInt32(rdr["Id"]),
                        UnitName = rdr["Result"].ToString(),
                    });
                }


                rdr.Close();
                ConLab.Close();
                if (lists.Count==1)
                {
                    lists.Add(new UnitModel() { UnitName = "", UnitId = 0 });
                }
               
                return lists;
            }
            catch (Exception ex)
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
