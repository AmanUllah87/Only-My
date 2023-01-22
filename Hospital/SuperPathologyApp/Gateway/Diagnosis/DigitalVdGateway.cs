using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Diagnosis;

namespace SuperPathologyApp.Gateway.Diagnosis
{
    class DigitalVdGateway:DbConnection
    {

        internal BillModel GetDataByInvNo(string billNo)
        {
            try
            {
                var aMdl = new BillModel();
                var mdl = new List<TestChartModel>();
                string query = @"SELECT a.BillNo,a.BillDate,d.Id,d.Name,SUM(b.charge) AS Amt
                                FROM V_StrickerData  a INNER JOIN tb_TESTCHART b 
                                ON a.TestId=b.Id INNER JOIN tb_SubProject c ON b.SubProjectId=c.Id
                                INNER JOIN tb_MainProject d ON c.MainProjectId=d.Id WHERE a.BillNo='"+ billNo +"' GROUP BY a.BillNo,a.BillDate,d.Id,d.Name";
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    mdl.Add(new TestChartModel()
                    {
                        Charge = Convert.ToDouble(rdr["Amt"].ToString()),
                        Name = rdr["Name"].ToString(),
                    });
                
                }
                aMdl.TestChartModel = mdl;
                rdr.Close();
                ConLab.Close();

                return aMdl;

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
