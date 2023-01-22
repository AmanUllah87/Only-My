using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
 
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using SuperPathologyApp.UI;

namespace SuperPathologyApp.Gateway
{
    class MasterSetupGateway:DbConnection
    {
        internal string Save(Model.MasterSetupModel aModel)
        {
            try
            {
                ConLab.Open();
                const string query = @"UPDATE tb_MASTER_INFO SET IsUsedBarcode=@IsUsedBarcode,ComName=@ComName,Address=@Address,ReportNo=@ReportNo,IpdSampleNo=@IpdSerialNo,OpdSampleNo=@OpdSerialNo,HistoSampleNo=@HistoSlNo";
                var cmd = new SqlCommand(query, ConLab);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IsUsedBarcode", aModel.UseBarcodeForSample);
                cmd.Parameters.AddWithValue("@ComName", aModel.ComName);
                cmd.Parameters.AddWithValue("@Address", aModel.Address);
                cmd.Parameters.AddWithValue("@IpdSerialNo", aModel.IpdSampleNo);
                cmd.Parameters.AddWithValue("@OpdSerialNo", aModel.OpdSampleNo);
                cmd.Parameters.AddWithValue("@ReportNo", aModel.ReportNo);
                cmd.Parameters.AddWithValue("@HistoSlNo", aModel.HistoSampleNo);
                




                cmd.ExecuteNonQuery();
                ConLab.Close();
                return UpdateSuccessMessage;
            }
            catch (Exception exception)
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                return exception.Message;
            }
        }

        internal MasterSetupModel GetCheckedValue()
        {
            try
            {
                var mdl = new MasterSetupModel();
                string query = "SELECT IsUsedBarcode,ComName,Address,IpdSampleNo,OpdSampleNo,ReportNo,HistoSampleNo FROM tb_MASTER_INFO";
                var cmd = new SqlCommand(query, ConLab);
                ConLab.Open();
                var rdr = cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    mdl.UseBarcodeForSample = Convert.ToInt32(rdr["IsUsedBarcode"]);
                    mdl.ComName = rdr["ComName"].ToString();
                    mdl.Address = rdr["Address"].ToString();
                    mdl.IpdSampleNo = Convert.ToDouble(rdr["IpdSampleNo"]);
                    mdl.OpdSampleNo = Convert.ToDouble(rdr["OpdSampleNo"]);
                    mdl.ReportNo = Convert.ToDouble(rdr["ReportNo"]);
                    mdl.HistoSampleNo = Convert.ToDouble(rdr["HistoSampleNo"]);

                }
                rdr.Close();
                ConLab.Close();
                return mdl;
            }
            catch (Exception )
            {

                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                return new MasterSetupModel();
            }
            
            





        }
    }
}
