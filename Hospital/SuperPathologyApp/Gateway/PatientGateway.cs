using SuperPathologyApp.Gateway.DB_Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperPathologyApp.Model;

namespace SuperPathologyApp.Gateway
{
    public class PatientGateway : DbConnection
    {


        internal DataTable GetRegisterPatientList(string searchString, int regId)
        {
            try
            {
                string cond = "",regCond="";
                if (searchString != "")
                {
                    cond += " AND (ContactNo+Name+Address) like '%" + searchString + "%'";
                }
                if (regId!= 0)
                {
                    regCond = " AND Id="+ regId +"";
                }

                string query = @"SELECT Top 30 Id,ContactNo,RegNo, Name, Address,Sex,Dob  FROM tb_Patient WHERE 1=1 " + cond + " " + regCond + " Order by Id ";

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
        public string Save(PatientModel mdl)
        {

            mdl.RegNo ="R"+ FncReturnFielValueLab("tb_PATIENT", "1=1", "ISNULL(CONVERT(INT,MAX(RIGHT(RegNo,6)))+1,1)").PadLeft(6,'0'); 
            
            
            
            
            
            try
            {

                string query = @"INSERT INTO tb_PATIENT(RegNo, Name, Address, ContactNo, Sex, Dob, PostedBy) VALUES (@RegNo, @Name, @Address, @ContactNo, @Sex, @Dob, @PostedBy)";
                var cmd = new SqlCommand(query, ConLab);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@RegNo", mdl.RegNo);
                cmd.Parameters.AddWithValue("@Name", mdl.Name);
                cmd.Parameters.AddWithValue("@Address", mdl.Address);
                cmd.Parameters.AddWithValue("@ContactNo", mdl.ContactNo);
                cmd.Parameters.AddWithValue("@Sex", mdl.Sex);
                cmd.Parameters.AddWithValue("@Dob", mdl.Dob);
                cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);

                ConLab.Open();
                    cmd.ExecuteNonQuery();
                ConLab.Close();

                return Hlp.SaveMessage;
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
        public string Update(PatientModel mdl)
        {
            try
            {






                string query = @"Update tb_PATIENT SET  Name=@Name, Address=@Address, ContactNo=@ContactNo, Sex=@Sex, Dob=@Dob, PostedBy=@PostedBy WHERE Id=@Id";
                var cmd = new SqlCommand(query, ConLab);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", mdl.PatientId);

                cmd.Parameters.AddWithValue("@Name", mdl.Name);
                cmd.Parameters.AddWithValue("@Address", mdl.Address);
                cmd.Parameters.AddWithValue("@ContactNo", mdl.ContactNo);
                cmd.Parameters.AddWithValue("@Sex", mdl.Sex);
                cmd.Parameters.AddWithValue("@Dob", mdl.Dob);
                cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);

                ConLab.Open();
                cmd.ExecuteNonQuery();
                ConLab.Close();

                return Hlp.UpdateMessage;
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
    }
}
