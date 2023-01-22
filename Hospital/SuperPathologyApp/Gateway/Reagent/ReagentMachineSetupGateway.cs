using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Reagent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperPathologyApp.Gateway.Reagent
{
    class ReagentMachineSetupGateway : DbConnection
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
            catch (Exception)
            {

                throw;
            }
        }
        internal string Save(DataGridView dataGrid, string pCode)
        {
            try
            {
                ConLab.Open();
                for (int i = 0; i < dataGrid.Rows.Count; i++)
                {
                    const string query = "INSERT INTO tb_reagent_TEST_BRIDGE(TestId,ReagentId,Qty) VALUES (@TestId,@ReagentId,@Qty)";
                    var cmd = new SqlCommand(query, ConLab);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TestId", pCode);
                    cmd.Parameters.AddWithValue("@ReagentId", dataGrid.Rows[i].Cells[0].Value.ToString());
                    cmd.Parameters.AddWithValue("@Qty", dataGrid.Rows[i].Cells[2].Value.ToString());
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
                return ex.Message;
            }
        }
        internal List<ReagentModel> ChildCodeDetails(string masterCode)
        {
            try
            {
                var lists = new List<ReagentModel>();
                string query = @"SELECT a.TestId,a.ReagentId,b.Name AS ReagentName,a.Qty FROM tb_reagent_TEST_BRIDGE a INNER JOIN tb_REAGENT b ON a.ReagentId=b.Id WHERE a.TestId='"+ masterCode +"'";
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    lists.Add(new ReagentModel()
                    {
                        ReagentId =Convert.ToInt32(rdr["ReagentId"]),
                        Name= rdr["ReagentName"].ToString(),
                        Qty = Convert.ToDouble(rdr["Qty"].ToString()),
                    });
                }


                rdr.Close();
                ConLab.Close();
                return lists;
            }
            catch (Exception)
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                };
                throw;
            }
        }
        internal List<ReagentModel> ChildCodeDetails(string masterCode,SqlTransaction trans,SqlConnection con)
        {
            try
            {
                var lists = new List<ReagentModel>();
                string query = @"SELECT a.TestId,a.ReagentId,b.Name AS ReagentName,a.Qty FROM tb_reagent_TEST_BRIDGE a INNER JOIN tb_REAGENT b ON a.ReagentId=b.Id WHERE a.TestId='" + masterCode + "'";
               
                var cmd = new SqlCommand(query, con,trans);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    lists.Add(new ReagentModel()
                    {
                        ReagentId = Convert.ToInt32(rdr["ReagentId"]),
                        Name = rdr["ReagentName"].ToString(),
                        Qty = Convert.ToDouble(rdr["Qty"].ToString()),
                    });
                }


                rdr.Close();
              
                return lists;
            }
            catch (Exception)
            {
               
                throw;
            }
        }


        internal DataTable GetParamNameByTestCode(string masterCode)
        {
            try
            {
                string query = "", condition = "";
                if (masterCode != "")
                {
                    condition = " WHERE (CONVERT(varchar(10),Id)+Name)  like '%" + masterCode + "%'";
                }


                query = @"SELECT Id,Name FROM tb_reagent "+ condition +"";



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
            catch (Exception)
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
                if (masterCode != "")
                {
                    cond = " AND Result Like '%" + masterCode + "%'";
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
                if (lists.Count == 1)
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
