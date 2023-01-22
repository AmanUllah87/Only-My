using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
 
using System.Windows.Forms;

namespace SuperPathologyApp.Gateway
{
    class ChannelSetupGateway : DbConnection
    {



        SqlTransaction _trans;
        internal string SaveParameter(TestCodeModel mdl, DataGridView dataGridView3)
        {
            try
            {
                ConLab.Open();
                _trans= ConLab.BeginTransaction();

                if (FnSeekRecordNewLab("Channeldefination", "PCode='" + mdl.TestCode + "' AND MachineName='"+ mdl.MachineName +"'", _trans, ConLab))
                {
                    DeleteInsertLab("DELETE FROM Channeldefination WHERE PCode='" + mdl.TestCode + "' AND MachineName='" + mdl.MachineName + "'", _trans);
                }


                for (int i = 0; i < dataGridView3.Rows.Count ; i++)
                {

                    string query = @"INSERT INTO Channeldefination(Pcode, ShortDesc, Parameter,  AliasName   ,ParameterType, MachineName,GroupName) VALUES (@Pcode, @ShortDesc, @Parameter,  @AliasName   ,@ParameterType, @MachineName,@GroupName)";
                    var cmd = new SqlCommand(query, ConLab, _trans);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Pcode", mdl.TestCode);
                    cmd.Parameters.AddWithValue("@ShortDesc", mdl.TestName);
                    cmd.Parameters.AddWithValue("@Parameter", dataGridView3.Rows[i].Cells[0].Value);
                    cmd.Parameters.AddWithValue("@AliasName", dataGridView3.Rows[i].Cells[1].Value);
                    cmd.Parameters.AddWithValue("@ParameterType", dataGridView3.Rows[i].Cells[2].Value);
                    cmd.Parameters.AddWithValue("@MachineName", dataGridView3.Rows[i].Cells[3].Value);
                    cmd.Parameters.AddWithValue("@GroupName", dataGridView3.Rows[i].Cells[4].Value);
                    cmd.ExecuteNonQuery();
                }


                _trans.Commit();
                ConLab.Close();
                return SaveSuccessMessage;

            }
            catch (Exception ex)
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    ConLab.Close();
                };
                return ex.Message;
            }
        }
   
        internal List<TestCodeModel> GetParameterDetailsByTestCode(string testCode,string machineName)
        {
            try
            {
                string cond = "";
                if (machineName!="--Select--")
                {
                    cond += " AND MachineName='" + machineName + "'";
                }


                if (testCode != "")
                {
                    cond += " AND PCode='" + testCode + "'";
                }
                
                
                
                var list = new List<TestCodeModel>();
                ConLab.Open();
                string query = @"SELECT Parameter,AliasName,ParameterType,MachineName,GroupName FROM Channeldefination WHERE  1=1 "+ cond +"";
                var cmd =new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestCodeModel()
                    {
                        ParameterName = rdr["Parameter"].ToString(),
                        ParameterTestName = rdr["AliasName"].ToString(),
                        PtType = rdr["ParameterType"].ToString(),
                        MachineName = rdr["MachineName"].ToString(),
                        ReportingGroupName = rdr["GroupName"].ToString(),
                    });
                }
                rdr.Close();
                ConLab.Close();
                return list;
            }
            catch (Exception )
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }    
                throw;
            }
        }

        public DataTable GetChannelData(string text)
        {

            try
            {
                string query = "", condition = "";
                if (text != "")
                {
                    condition = "WHERE (Parameter) like '%" + text + "%'";
                }

                query = @"SELECT Distinct Parameter AS Reagent,'' AS 'NA' FROM Channeldefination  " + condition + " Order By Parameter";

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
        public DataTable GetParamData(string text)
        {

            try
            {
                string query = "", condition = "";
                if (text != "")
                {
                    condition = "WHERE (AliasName) like '%" + text + "%'";
                }

                query = @"SELECT Distinct AliasName AS Parameter,'' AS 'NA' FROM Channeldefination  " + condition + " Order By AliasName";

                ConLab.Open();
                var da = new SqlDataAdapter(query, ConLab);
                var ds = new DataSet();
                da.Fill(ds, "pCode");
                var table = ds.Tables["pCode"];
                ConLab.Close();
                return table;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        internal List<TestCodeModel> GetChannelListByMachineName(string text)
        {
            try
            {

                if (text!="--Select--")
                {

                    var list = new List<TestCodeModel>();
                    ConLab.Open();
                    string query = @"SELECT Pcode,Parameter,AliasName,ParameterType,MachineName,GroupName FROM Channeldefination WHERE  MachineName='" + text + "'";
                    var cmd = new SqlCommand(query, ConLab);
                    var rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        list.Add(new TestCodeModel()
                        {
                            PCode = rdr["Pcode"].ToString(),
                            ParameterName = rdr["Parameter"].ToString(),
                            ParameterTestName = rdr["AliasName"].ToString(),
                            PtType = rdr["ParameterType"].ToString(),
                            MachineName = rdr["MachineName"].ToString(),
                            ReportingGroupName = rdr["GroupName"].ToString(),
                        });
                    }
                    rdr.Close();
                    ConLab.Close();
                    return list;
                }

                return new List<TestCodeModel>();


            }
            catch (Exception )
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                throw;
            }
        }
    }
}
