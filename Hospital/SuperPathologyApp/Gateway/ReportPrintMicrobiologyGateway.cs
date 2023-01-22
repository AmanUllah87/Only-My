using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
 
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;

namespace SuperPathologyApp.Gateway
{
    internal class ReportPrintMicrobiologyGateway : DbConnection
    {
        public List<TestCodeModel> GetInvoiceDetailByInvNo(int masterId)
        {
            try
            {
                var list = new List<TestCodeModel>();
               // string query =@"SELECT a.InvDate,a.InvNo,a.PatientName,a.MobileNo,a.Sex,a.Age,b.Code,b.ShortDesc,a.DrCode,a.DrName,b.GroupName,b.PrintNo,b.LabNo  FROM tb_InvMaster a LEFT JOIN tb_InvDetails b ON a.Id=b.MasterId    WHERE a.Id=" + masterId + " AND EXISTS (SELECT SampleNo FROM tb_LabSampleStatusInfo  WHERE ReceiveInLabStatus='Collected' AND SampleNo=b.LabNo AND VaqGroup='Microbiology') ";
                string query =@"SELECT a.InvDate,a.InvNo,a.PatientName,a.MobileNo,a.Sex,a.Age,b.Code,b.ShortDesc,a.DrCode,a.DrName,b.GroupName,b.PrintNo,b.LabNo  FROM tb_InvMaster a LEFT JOIN tb_InvDetails b ON a.Id=b.MasterId    WHERE a.Id=" + masterId + " AND EXISTS (SELECT SampleNo FROM tb_LabSampleStatusInfo  WHERE  SampleNo=b.LabNo AND VaqGroup='Microbiology') ";

                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestCodeModel()
                    {
                        SampleNo = rdr["LabNo"].ToString(),
                        PtInvDate = Convert.ToDateTime(rdr["InvDate"]),
                        PtInvNo = rdr["InvNo"].ToString(),
                        PtName = rdr["PatientName"].ToString(),
                        PtTelephoneNo = rdr["MobileNo"].ToString(),
                        PtSex = rdr["Sex"].ToString(),
                        PtAge = rdr["Age"].ToString(),
                        PCode = rdr["Code"].ToString(),
                        ItemDesc = rdr["ShortDesc"].ToString(),
                        DrCode = rdr["DrCode"].ToString(),
                        DrName = rdr["DrName"].ToString(),
                        GroupName = rdr["GroupName"].ToString(),
                        PrintNo = Convert.ToInt32(rdr["PrintNo"]),
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
                return new List<TestCodeModel>();
            }
        }

        public string Save(List<TestCodeModel> mdl,SqlTransaction trans,SqlConnection con)
        {
            try
            {
                foreach (var model in mdl)
                {
                    if (FnSeekRecordNewLab("tb_MachineDataDtls_MicroDetail", "MasterId='" + model.MasterId + "' AND PCode='" + model.PCode + "'", trans, con))
                    {
                        DeleteInsertLab("DELETE FROM tb_MachineDataDtls_MicroDetail WHERE MasterId='" + model.MasterId + "' AND PCode='" + model.PCode + "'", trans, con);
                    }
                }

                DataTable dt = ConvertListDataTable(mdl);

                var objbulk = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, trans) { DestinationTableName = "tb_MachineDataDtls_MicroDetail" };

                objbulk.ColumnMappings.Add("ParameterTestName", "DrugName");
                objbulk.ColumnMappings.Add("ZoneSize", "ZoneSize");
                objbulk.ColumnMappings.Add("Enterpretation", "Enterpretation");
                objbulk.ColumnMappings.Add("PCode", "PCode");
                objbulk.ColumnMappings.Add("MasterId", "MasterId");

                objbulk.ColumnMappings.Add("Organism", "Organism");
                objbulk.ColumnMappings.Add("Colony", "ColonyCount");
                objbulk.ColumnMappings.Add("Incubation", "Incubation");
                objbulk.ColumnMappings.Add("SpecificTest", "SpecificTest");
                
                objbulk.WriteToServer(dt);
                return SaveSuccessMessage;


            }
            catch (Exception exception)
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConOther.Close();
                }
                return exception.Message;
            }
        }
        
        internal DataTable GetCultureDefaultParameter(string paramName,string searchString)
        {
            try
            {
                string cond = "";
                if (searchString != "")
                {
                    cond += " AND Result like '%"+ searchString +"%'";
                }

                string query = @"SELECT Id,Result FROM tb_DefaultResultSetupCulture WHERE IsShow=1 AND parameter='"+ paramName +"' "+ cond +" Order by Id ";

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

                throw ;
            }
        }

        internal DataTable GetCultureDrugName(string searchString)
        {
            try
            {
                string cond = "";
                if (searchString != "")
                {
                    cond += " AND  cast(Id AS Varchar(10))+ShortName LIKE  '%" + searchString + "%'";
                }
                //AND  cast(Id AS Varchar(10))+Parameter LIKE '%PIPE%'

                string query = @"SELECT Id,ShortName,Parameter FROM tb_Parameter_Definition_Microbiology WHERE 1=1 " + cond + " Order by Id ";

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


                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }

                throw;
            }
        }

        internal List<TestCodeModel> GetAllDrugName()
        {
            try
            {
                var list = new List<TestCodeModel>();
                string cond = "";
                string query = @"SELECT Id,Parameter,ShortName FROM tb_Parameter_Definition_Microbiology WHERE 1=1 " + cond + " Order by Id ";
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestCodeModel()
                    {
                        ItemId = rdr["Id"].ToString(),
                        ShortName = rdr["ShortName"].ToString(),
                        ParameterName = rdr["Parameter"].ToString(),
                    });
                }
                rdr.Close();
                ConLab.Close();
                return list;
            }
            catch (Exception)
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
