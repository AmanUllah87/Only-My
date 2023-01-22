using System;
using System.Data;
using System.Data.SqlClient;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Indoor;

namespace SuperPathologyApp.Gateway.Indoor
{
    class AdmissionGateway : DbConnection
    {
        private SqlTransaction _trans;
        internal string Save(AdmissionModel mAdm,string comeFrom)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                string query = "";

                if (comeFrom=="&Save")
                {
                    mAdm.AdmNo = GetInvoiceNo("tb_IN_ADMISSION", "AdmNo", "AdmDATE", ConLab, _trans);
                    query = @"INSERT INTO tb_IN_ADMISSION(DeptId, AdmNo, AdmDate, AdmTime, PtName, PtSex, PtAge, PtAddress, SpouseName, ContactNo, ChiefComplain, RefId, UnderDrId, BedId, AdmCharge, PostedBy, PcName, IpAddress,EmgrContact,RegId)OUTPUT INSERTED.ID VALUES(@DeptId, @AdmNo, @AdmDate, @AdmTime, @PtName, @PtSex, @PtAge, @PtAddress, @SpouseName, @ContactNo, @ChiefComplain, @RefId, @UnderDrId, @BedId, @AdmCharge, @PostedBy, @PcName, @IpAddress,@EmgrContact,@RegId)";
                }
                else
                {
                    query = @"UPDATE tb_IN_ADMISSION SET DeptId=@DeptId,   PtName=@PtName, PtSex=@PtSex, PtAge=@PtAge, PtAddress=@PtAddress, SpouseName=@SpouseName, ContactNo=@ContactNo, ChiefComplain=@ChiefComplain, RefId=@RefId, UnderDrId=@UnderDrId, BedId=@BedId, AdmCharge=@AdmCharge,  PcName=@PcName, IpAddress=@IpAddress,EmgrContact=@EmgrContact,RegId=@RegId  WHERE Id=@AdmId";
                    string prevName = FncReturnFielValueLab("tb_in_Admission", "Id='" + mAdm.AdmId + "'", "PtName", _trans, ConLab);
                    Hlp.InsertIntoEditRecord(mAdm.AdmNo, Hlp.GetServerDate(ConLab, _trans), prevName, 0, 0, 0, 0, 0, 0, "Admission", _trans, ConLab);
                }

                var cmd = new SqlCommand(query, ConLab, _trans);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@AdmId", mAdm.AdmId);
                cmd.Parameters.AddWithValue("@RegId", mAdm.Patient.RegId);

                cmd.Parameters.AddWithValue("@DeptId", mAdm.Department.DeptId);
                cmd.Parameters.AddWithValue("@AdmNo", mAdm.AdmNo);
                cmd.Parameters.AddWithValue("@AdmDate", Hlp.GetServerDate(ConLab, _trans).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@AdmTime", Hlp.GetServerDate(ConLab, _trans).ToShortTimeString());
                cmd.Parameters.AddWithValue("@PtName", mAdm.Patient.Name);
                cmd.Parameters.AddWithValue("@PtSex", mAdm.Patient.Sex);
                cmd.Parameters.AddWithValue("@PtAge", mAdm.Patient.Age);
                cmd.Parameters.AddWithValue("@PtAddress", mAdm.Patient.Address);

                cmd.Parameters.AddWithValue("@SpouseName", mAdm.Patient.Spouse);
                cmd.Parameters.AddWithValue("@ContactNo", mAdm.Patient.ContactNo);
                cmd.Parameters.AddWithValue("@EmgrContact", mAdm.EmergencyContact);
                
                cmd.Parameters.AddWithValue("@ChiefComplain", mAdm.ChiefComplain);
                cmd.Parameters.AddWithValue("@RefId", mAdm.RefDoctor.DrId);
                cmd.Parameters.AddWithValue("@UnderDrId", mAdm.UnderDoctor.DrId);
                cmd.Parameters.AddWithValue("@BedId", mAdm.Bed.BedId);
                cmd.Parameters.AddWithValue("@AdmCharge", mAdm.AdmissionCharge);

                cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);
                cmd.Parameters.AddWithValue("@PcName", Environment.UserName);
                cmd.Parameters.AddWithValue("@IpAddress", Hlp.IpAddress());






                string rtnMessage = "";
                if (comeFrom == "&Save")
                {
                    mAdm.AdmId = (int)cmd.ExecuteScalar();
                    int admissionFeeId = Convert.ToInt32(FncReturnFielValueLab("tb_in_FIXED_ID", "Name='Admission Fee'", "Id", _trans, ConLab));
                    Hlp.InsertIntoPatientLedger(mAdm.AdmNo, Hlp.GetServerDate(ConLab, _trans), mAdm.Patient.RegId, mAdm.AdmId, mAdm.Bed.BedId, admissionFeeId, "Admission Fee", mAdm.AdmissionCharge, 1, mAdm.AdmissionCharge, 0, 0,0, 0, 0, 0, 0, ConLab, _trans);
                    if (mAdm.AdvanceAmt > 0)
                    {
                        mAdm.AdvanceAmtTrNo = Hlp.GetAutoIncrementVal("TrNo", "tb_in_ADVANCE_COLLECTION", 6, ConLab, _trans);
                        int advanceCollId = Convert.ToInt32(FncReturnFielValueLab("tb_in_FIXED_ID", "Name='Advance Collection'", "Id", _trans, ConLab));
                        Hlp.InsertIntoPatientLedger(mAdm.AdvanceAmtTrNo, Hlp.GetServerDate(ConLab, _trans), mAdm.Patient.RegId, mAdm.AdmId, mAdm.Bed.BedId, advanceCollId, "Advance Collection", 0, 1, 0, 0, mAdm.AdvanceAmt, 0,0, 0, 0, 0, ConLab, _trans);
                        Hlp.InsertIntoAdvanceColl(mAdm.AdvanceAmtTrNo, Hlp.GetServerDate(ConLab, _trans), mAdm.Patient.RegId, mAdm.AdmId, mAdm.Bed.BedId, mAdm.AdvanceAmt, ConLab, _trans);
                        DeleteInsertLab("Update tb_IN_ADMISSION SET TrNo='" + mAdm.AdvanceAmtTrNo + "' WHERE Id='" + mAdm.AdmId + "'", _trans, ConLab);
                    }

                    DeleteInsertLab("Update tb_in_BED SET BookStatus=1 WHERE Id=" + mAdm.Bed.BedId + "", _trans, ConLab);
                    rtnMessage = SaveSuccessMessage;

                }
                else
                {
                    cmd.ExecuteNonQuery();
                    rtnMessage=UpdateSuccessMessage;
                }
      
                
                
                
                
                

                _trans.Commit();
                ConLab.Close();

                return rtnMessage;
               

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
        internal DataTable GetBedList(int id, string searchString)
        {
            try
            {
                string cond = "";
                if (searchString != "0")
                {
                    cond = "AND (convert(varchar,Id)+Name+Floor+BedType+DeptName) like '%'+'" + searchString + "'+'%'";
                }
                if (id != 0)
                {
                    cond = "AND Id=" + id + "";
                }
                string query = @"SELECT * FROM V_IN_BED_WITH_DEPT WHERE 1=1  " + cond + "";

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
                    _trans.Rollback();
                    ConLab.Close();
                }
                throw;
            }
        }


        internal AdmissionModel GetAdmissionDataForEdit(string admNo)
        {
            int masterId = Convert.ToInt32(FncReturnFielValueLab("tb_in_ADMISSION", "AdmNo='" + admNo + "'", "Id"));
            var aMdl = new AdmissionModel();
          
            ConLab.Open();
            string query = @"SELECT * FROM V_Admission_List WHERE Id='"+ masterId +"'";
            var cmd = new SqlCommand(query, ConLab);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
               
                aMdl.EmergencyContact = rdr["EmgrContact"].ToString();
                aMdl.ChiefComplain = rdr["ChiefComplain"].ToString();
                aMdl.Patient = new PatientModel()
                {
                   PatientId=Convert.ToInt32(rdr["Id"]),
                    Name = rdr["PtName"].ToString(),
                    Age = rdr["PtAge"].ToString(),
                    ContactNo = rdr["ContactNo"].ToString(),
                    Sex = rdr["PtSex"].ToString(),
                    Address = rdr["PtAddress"].ToString(),
                    Spouse = rdr["SpouseName"].ToString(),
                };




                aMdl.RefDoctor = new DoctorModel() 
                { 
                    DrId = Convert.ToInt32(rdr["RefId"].ToString()),
                    Name = rdr["RefDrName"].ToString(),
                };

                aMdl.UnderDoctor = new DoctorModel()
                {
                    DrId = Convert.ToInt32(rdr["UnderDrId"].ToString()),
                    Name = rdr["UnderDrName"].ToString(),
                };


                aMdl.Bed = new BedModel() 
                { 
                    BedId = Convert.ToInt32(rdr["BedId"].ToString()),
                    Name = rdr["BedName"].ToString(),
                    Floor = rdr["Floor"].ToString(),
                    Charge = Convert.ToDouble(rdr["BedCharge"]),
                };

                aMdl.Department = new DepartmentModel() 
                { 
                    DeptId= Convert.ToInt32(rdr["DeptId"].ToString()),
                };

                aMdl.AdmissionCharge = Convert.ToDouble(rdr["AdmCharge"].ToString());
                aMdl.AdmDate = Convert.ToDateTime(rdr["AdmDate"].ToString());
             

                


            }
            rdr.Close();
            ConLab.Close();
            return aMdl;





        }
        
        
        
            
        
        internal string GetAdmissionNo()
        {
            throw new NotImplementedException();
        }
    }
}
