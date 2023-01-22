using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Diagnosis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SuperPathologyApp.Gateway
{
    class ParameterSetupGateway:DbConnection
    {

        SqlTransaction _trans;
        internal string SaveParameter(TestCodeModel mdl, DataGridView dataGridView3)
        {
            try
            {
                ConLab.Open();
                _trans= ConLab.BeginTransaction();

                if (FnSeekRecordNewLab("tb_TESTCHART_PARAM", "TestChartId='" + mdl.TestCode + "'", _trans, ConLab))
                {
                    DeleteInsertLab("DELETE FROM tb_TESTCHART_PARAM WHERE TestChartId='" + mdl.TestCode + "'", _trans);
                }
                DeleteInsertLab("UPDATE tb_TESTCHART SET Specimen='"+ mdl.SpecimenName +"' WHERE Id='" + mdl.TestCode + "'", _trans);



                for (int i = 0; i < dataGridView3.Rows.Count ; i++)
                {

                    string query = @"INSERT INTO tb_TESTCHART_PARAM(TestChartId, GroupSl, ParamSl, MachineParam, ReportParam, ReportingGroup, NormalRange, Unit, DefaultResult, IsBold, LowerVal, UpperVal) VALUES (@TestChartId, @GroupSl, @ParamSl, @MachineParam, @ReportParam, @ReportingGroup, @NormalRange, @Unit, @DefaultResult, @IsBold, @LowerVal, @UpperVal)";
                    var cmd = new SqlCommand(query, ConLab, _trans);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TestChartId", mdl.TestCode);

                    cmd.Parameters.AddWithValue("@GroupSl", dataGridView3.Rows[i].Cells[0].Value);
                    cmd.Parameters.AddWithValue("@ParamSl", dataGridView3.Rows[i].Cells[1].Value);
                    cmd.Parameters.AddWithValue("@MachineParam", dataGridView3.Rows[i].Cells[2].Value);
                    cmd.Parameters.AddWithValue("@ReportParam", dataGridView3.Rows[i].Cells[3].Value);


                    cmd.Parameters.AddWithValue("@NormalRange", dataGridView3.Rows[i].Cells[4].Value);
                    cmd.Parameters.AddWithValue("@Unit", dataGridView3.Rows[i].Cells[5].Value);
                    cmd.Parameters.AddWithValue("@ReportingGroup", dataGridView3.Rows[i].Cells[6].Value);

                    
                    cmd.Parameters.AddWithValue("@DefaultResult", dataGridView3.Rows[i].Cells[7].Value);
                    cmd.Parameters.AddWithValue("@IsBold", dataGridView3.Rows[i].Cells[8].Value);
                    cmd.Parameters.AddWithValue("@LowerVal", dataGridView3.Rows[i].Cells[9].Value);
                    cmd.Parameters.AddWithValue("@UpperVal", dataGridView3.Rows[i].Cells[10].Value);

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
        internal List<TestCodeModel> GetParameterListByItemId(string pCode)
        {
            try
            {
                var list = new List<TestCodeModel>();
                string query = "";
                query = @"SELECT * FROM tbl_LAB_PARAMETER_DEFINITION WHERE PCode=@pCode";
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                cmd.Parameters.AddWithValue("@pCode", pCode);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestCodeModel()
                    {
                        SpecimenName = rdr["Specimen"].ToString(),
                        ParameterTestName = rdr["AliasName"].ToString(),
                        ParameterName = rdr["ParameterName"].ToString(),
                        //Result = rdr["Result"].ToString(),
                        //Unit = rdr["Unit"].ToString(),
                        //NormalValue = rdr["NormalValue"].ToString(),
                        //GroupName = rdr["GroupName"].ToString(),
                        //GroupSlNo = Convert.ToInt32(rdr["GroupSlNo"]),
                        //ItemSlNo = Convert.ToInt32(rdr["ItemSlNo"]),
                        //PCode = pCode,
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
        internal List<TestParamModel> GetParameterDetailsByTestCode(string testCode)
        {
            try
            {
                var list = new List<TestParamModel>();
                ConLab.Open();
                string query = @" SELECT a.TestChartId,b.Name, b.Specimen, a.GroupSl,a.ParamSl, a.MachineParam,a.ReportParam, a.NormalRange, a.Unit, a.DefaultResult, a.ReportingGroup, a.IsBold,a.LowerVal,a.UpperVal   FROM tb_TESTCHART_PARAM a INNER JOIN tb_TESTCHART b ON a.TestChartId=b.id  WHERE TestChartId='" + testCode + "' Order By GroupSl, ParamSl";
                var cmd =new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestParamModel()
                    {
                        Specimen = rdr["Specimen"].ToString(),
                        TestchartId = Convert.ToInt32(rdr["TestChartId"].ToString()),
                        Name = rdr["Name"].ToString(),
                        GroupSl = Convert.ToInt32(rdr["GroupSl"].ToString()),
                        ParamSl = Convert.ToInt32(rdr["ParamSl"].ToString()),
                        MachineParam = rdr["MachineParam"].ToString(),
                        ReportParam = rdr["ReportParam"].ToString(),
                        NormalRange= rdr["NormalRange"].ToString(),
                        Unit = rdr["Unit"].ToString(),
                        DefaultResult = rdr["DefaultResult"].ToString(),
                        ReportingGroup = rdr["ReportingGroup"].ToString(),
                        IsBold = Convert.ToInt32(rdr["IsBold"].ToString()),
                        UpperVal = Convert.ToDouble(rdr["UpperVal"].ToString()),
                        LowerVal = Convert.ToDouble(rdr["LowerVal"].ToString()),
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
    }
}
