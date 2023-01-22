using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
 
using System.Windows.Forms;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;

namespace SuperPathologyApp.Gateway
{
    class ParentTestSetupGateway:DbConnection
    {

        private SqlTransaction _trans;
        internal string Save(TestCodeModel mdl, DataGridView dataGridView1)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                DeleteInsertLab("DELETE FROM tb_MasterCodeSetup WHERE MasterPCode='" + mdl.TestCode + "'",_trans);
                for (int i = 0; i < dataGridView1.Rows.Count ; i++)
                {
                    const string query = @"INSERT INTO tb_MasterCodeSetup(MasterPCode, MasterDesc, ChildCode, ChildDesc) VALUES (@MasterPCode, @MasterDesc, @ChildCode, @ChildDesc)";
                    var cmd = new SqlCommand(query, ConLab, _trans);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@MasterPCode", mdl.TestCode);
                    cmd.Parameters.AddWithValue("@MasterDesc", mdl.TestName);
                    cmd.Parameters.AddWithValue("@ChildCode", dataGridView1.Rows[i].Cells[0].Value.ToString());
                    cmd.Parameters.AddWithValue("@ChildDesc", dataGridView1.Rows[i].Cells[1].Value.ToString());;
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

        internal List<TestCodeModel> ChildCodeDetails(string masterCode)
        {
            try
            {
                var lists = new List<TestCodeModel>();
                string query = @"SELECT ChildCode,ChildDesc FROM tb_MasterCodeSetup WHERE MasterPCode='"+ masterCode +"' Order By Id";
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    lists.Add(new TestCodeModel()
                    {
                        PCode = rdr["ChildCode"].ToString(),
                        ItemDesc = rdr["ChildDesc"].ToString(),
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
    }
}
