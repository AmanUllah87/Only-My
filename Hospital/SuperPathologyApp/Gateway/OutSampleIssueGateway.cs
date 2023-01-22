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
    class OutSampleIssueGateway : DbConnection
    {
        public DataTable GetOutTestList(DateTime dt)
        {
            try
            {
                string query = @"SELECT distinct a.IssueNo,a.SampleNo,a.IssueDate,b.Name AS HospName
                                        FROM tb_Invoice_Sample_Issue  a LEFT JOIN tb_Hospital b ON a.HospId=b.Id WHERE a.IssueDate='" + dt.ToString("yyyy-MM-dd") +"'";
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
                    ConLab.Close();
                }
                throw;
            }
        }


        internal string Save(string sampleNo,int hospId, DataGridView dataGridView1)
        {
            try
            {
                string invNo = GetInvoiceNo("tb_Invoice_Sample_Issue", "IssueNo","");
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DeleteInsertLab("INSERT INTO tb_Invoice_Sample_Issue(IssueNo, SampleNo, HospId, TestCode, UserName)VALUES('" + invNo + "','" + sampleNo + "'," + hospId + ",'" + dataGridView1.Rows[i].Cells[0].Value + "','" + Hlp.UserName + "')");
                }
                return SaveSuccessMessage; 
            }
            catch (Exception ex)
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                return  ex.Message ;
            }
        }


     
    }
}
