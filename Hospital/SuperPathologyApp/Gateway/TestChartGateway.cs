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
    class TestChartGateway : DbConnection
    {
        private SqlTransaction _trans;
        internal string Save(DoctorModel mLabDoctor)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                string query = "";
                query = FnSeekRecordNewLab("tb_Doctor", "Id=" + mLabDoctor.DrId + "", _trans, ConLab) ? "Update tb_Doctor SET Name=@Name, Address=@Address,ContactNo=@ContactNo,TakeCom=@TakeCom ,UserName=@UserName WHERE Id=@Id" : @"INSERT INTO tb_Doctor(Name, Address,ContactNo,TakeCom,UserName) VALUES (@Name, @Address,@ContactNo,@TakeCom,@UserName)";
                var cmd = new SqlCommand(query, ConLab,_trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", mLabDoctor.DrId);
                cmd.Parameters.AddWithValue("@Name", mLabDoctor.Name);
                cmd.Parameters.AddWithValue("@Address", mLabDoctor.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ContactNo", mLabDoctor.ContactNo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TakeCom", mLabDoctor.TakeCommision);
                cmd.Parameters.AddWithValue("@UserName", Hlp.UserName);
                
                cmd.ExecuteNonQuery();
                _trans.Commit();
                ConLab.Close();
                
                return SaveSuccessMessage;
            }
            catch (Exception exception)
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    ConLab.Close();
                }
                return exception.Message;
            }
        }

        internal DataTable GetTestCodeList(int id, string searchString,int isVaq)
        {
            try
            {
                string cond = "",vaqCond="";
                if (searchString != "0")
                {
                    cond = "AND (convert(varchar,Id)+Name+convert(varchar,Charge)) like '%'+'" + searchString + "'+'%'";
                }
                if (id != 0)
                {
                    cond = "AND Id=" + id + "";
                }
                if (isVaq!=2)
                {
                    vaqCond = " AND IsVaqItem=" + isVaq + "";
                }

               




                string query = @"SELECT * FROM tb_TestChart WHERE 1=1  " + cond + " "+ vaqCond +" Order by id";

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




        internal string Save(TestChartModel mdl)
        {
            string rtnMsg = "";
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                string query = "";
                query = FnSeekRecordNewLab("tb_TestChart", "Id=" + mdl.TestId + "", _trans, ConLab) ? "Update tb_TestChart SET Name=@Name, Charge=@Charge,MaxDiscount=@DefaultHonouriam,IsDiscountItem=@IsDiscountItem ,SubProjectId=@SubProjectId,IsVaqItem=@IsVaqItem,UserName=@UserName,ReportFileName=@ReportFileName,VaqName=@VaqName,ReportFee=@ReportFee,IsDoctor=@IsDoctor,ChangeCharge=@ChangeCharge WHERE Id=@Id" : @"INSERT INTO tb_TestChart(Name, Charge, MaxDiscount, IsDiscountItem, SubProjectId, IsVaqItem, UserName, ReportFileName,VaqName,ReportFee,IsDoctor,ChangeCharge) OUTPUT INSERTED.ID VALUES (@Name, @Charge, @DefaultHonouriam, @IsDiscountItem, @SubProjectId, @IsVaqItem, @UserName, @ReportFileName,@VaqName,@ReportFee,@IsDoctor,@ChangeCharge)";
                var cmd = new SqlCommand(query, ConLab, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Id", mdl.TestId);
                cmd.Parameters.AddWithValue("@Name", mdl.Name);
                cmd.Parameters.AddWithValue("@Charge", mdl.Charge);
                cmd.Parameters.AddWithValue("@DefaultHonouriam", mdl.DefaulHonouriam);
                cmd.Parameters.AddWithValue("@IsDiscountItem", mdl.IsGiveDiscount);
                cmd.Parameters.AddWithValue("@SubProjectId", mdl.SubProject.SubProjectId);
                cmd.Parameters.AddWithValue("@IsVaqItem", mdl.IsVaqItem);
                cmd.Parameters.AddWithValue("@ReportFileName", mdl.ReportFileName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@UserName", Hlp.UserName);
                cmd.Parameters.AddWithValue("@VaqName", mdl.VaqName);
                cmd.Parameters.AddWithValue("@ReportFee", mdl.ReportFee);
                cmd.Parameters.AddWithValue("@IsDoctor", mdl.IsDoctorItem);
                cmd.Parameters.AddWithValue("@ChangeCharge", mdl.IsChangeCharge);

                
                var testId = 0;
                
                if (mdl.TestId == 0)
                {
                    testId = (int)cmd.ExecuteScalar();
                    rtnMsg = SaveSuccessMessage;
                }
                else
                {
                    cmd.ExecuteNonQuery();
                    testId = mdl.TestId;
                    rtnMsg = UpdateSuccessMessage;
                }


                DeleteInsertLab("DELETE FROM tb_TESTCHART_VAQ_FOR_BILL WHERE TestId=" + testId + "", _trans, ConLab);
                foreach (var item in mdl.Vaq)
                {
                    query = "INSERT INTO tb_TESTCHART_VAQ_FOR_BILL(TestId, VaqId, Charge, UserName)VALUES(@TestId, @VaqId, @Charge, @UserName)";
                    cmd = new SqlCommand(query, ConLab, _trans);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@TestId", testId);
                    cmd.Parameters.AddWithValue("@VaqId", item.VaqId);
                    cmd.Parameters.AddWithValue("@Charge", item.Charge);
                    cmd.Parameters.AddWithValue("@UserName", Hlp.UserName);
                    cmd.ExecuteNonQuery();
                }
                _trans.Commit();
                ConLab.Close();

                return rtnMsg;
            }
            catch (Exception exception)
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    _trans.Rollback();
                    ConLab.Close();
                }
                return exception.Message;
            }
        }

        internal List<VacutainerModel> GetVaqListByTestId(int id)
        {
            try
            {
                var lists = new List<VacutainerModel>();
                string query = @"SELECT * FROM tb_TESTCHART_VAQ_FOR_BILL WHERE TestId=" + id + " Order by Id";
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new VacutainerModel
                    {
                        VaqId = Convert.ToInt32(rdr["VaqId"]),
                        Charge = Convert.ToDouble(rdr["Charge"]),

                    });
                }
                
              
                rdr.Close();
                ConLab.Close();
                return lists;
            }
            catch (Exception )
            {
                throw;
            }
        }

    }
}
