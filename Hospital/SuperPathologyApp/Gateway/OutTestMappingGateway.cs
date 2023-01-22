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
    class OutTestMappingGateway : DbConnection
    {
        public DataTable GetOutTestMappingList()
        {

            try
            {
                string query = @"SELECT a.TestCode,c.ShortDesc AS TestName,b.Name AS HospName 
                                FROM tb_OutTestMappingWithHospital a LEFT JOIN tb_Hospital b ON a.HospitalId=b.Id
                                LEFT JOIN tb_LabInvestigationChart C ON a.TestCode=C.PCode   Order By a.TestCode";

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

        
        internal string Save(string pCode,int hospId)
        {
            try
            {
                if (FnSeekRecordNewLab("tb_OutTestMappingWithHospital", "TestCode='" + pCode + "'"))
                {
                    DeleteInsertLab("DELETE FROM tb_OutTestMappingWithHospital WHERE TestCode='" + pCode + "'");
                }
                const string query = "INSERT INTO tb_OutTestMappingWithHospital(HospitalId, TestCode) VALUES (@HospitalId, @TestCode)";
                var cmd = new SqlCommand(query, ConLab);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@HospitalId", hospId);
                cmd.Parameters.AddWithValue("@TestCode", pCode);
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

    }
}
