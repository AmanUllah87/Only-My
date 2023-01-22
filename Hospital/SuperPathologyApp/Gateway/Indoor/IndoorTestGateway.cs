using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model.Indoor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPathologyApp.Gateway.Indoor
{
    class IndoorTestGateway : DbConnection
    {
        private SqlTransaction _trans;
        internal string Save(ReleaseModel mAdm, string comeFrom)
        {
            try
            {
                ConLab.Open();
                _trans = ConLab.BeginTransaction();
                mAdm.TrNo = Hlp.GetAutoIncrementVal("TrNo", "tb_in_TEST_ADD_LIST", 6, ConLab, _trans);

                var mdl = Hlp.GetRegAndBedId(mAdm.Admission.AdmId, ConLab, _trans);

                    foreach (var item in mAdm.Test)
                    {
                        DeleteInsertLab(@"INSERT INTO tb_in_TEST_ADD_LIST(TrNo, TrDate, AdmId, BedId, TestId, TestName, Charge, Unit, TotCharge, Remarks, PostedBy, IpAddress)VALUES
                            ('" + mAdm.TrNo + "', '" + Hlp.GetServerDate(ConLab, _trans).ToString("yyyy-MM-dd") + "', '" + mAdm.Admission.AdmId + "', '" + mdl.Bed.BedId + "', '" + item.TestId + "',  '" + item.Name + "',  '" + item.Charge + "',  '" + item.Unit + "',  '" + item.TotCharge + "',  '" + mAdm.Remarks + "',  '" + Hlp.UserName + "',  '" + Hlp.IpAddress() + "')", _trans, ConLab);
                        Hlp.InsertIntoPatientLedger(mAdm.TrNo, Hlp.GetServerDate(ConLab, _trans), mdl.Patient.RegId, mAdm.Admission.AdmId, mdl.Bed.BedId, item.TestId, item.Name, item.Charge, (int)item.Unit, item.TotCharge, item.LessAmt, 0, 0,0, 0, item.DefaulHonouriam, item.DrId, ConLab, _trans);
                    }

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
        internal DataTable GetInvoiceList(DateTime dateTime, string searchString, string comeFrom)
        {
            try
            {
                string cond = "";
                if (searchString != "")
                {
                    cond = "AND (b.PtName+a.TrNo+b.ContactNo) like '%" + searchString + "%'";
                }
                if (comeFrom == "enter")
                {
                    cond += " AND a.TrDate='" + dateTime.ToString("yyyy-MM-dd") + "'";
                }
                string query = @"SELECT a.TrNo AS BillNo,a.TrDate AS  BillDate,b.PtName As PatientName,b.ContactNo AS  MobileNo,SUM(a.Charge*a.Unit) AS TotAmt
FROM tb_in_TEST_ADD_LIST a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id WHERE 1=1 " + cond + " GROUP BY a.TrNo,a.TrDate,b.PtName ,b.ContactNo  Order by a.TrNo Desc";
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
                ConLab.Close();
                throw;
            }
        }
        
    }
}
