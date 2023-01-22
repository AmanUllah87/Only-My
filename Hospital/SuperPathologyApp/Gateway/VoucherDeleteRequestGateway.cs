using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model.Diagnosis;

namespace SuperPathologyApp.Gateway
{
    class VoucherDeleteRequestGateway:DbConnection
    {
        internal List<BillModel> GetRequestBillList(DateTime dateFrom,DateTime dateTo,string projectName)
        {
            try
            {
                string query = @"SELECT MasterId,BillNo,BillDate,PatientName,TotalAmt,Case when Status='Pending' THEN 0 ELSE 1 END STATUS,PostedBy,AuthorizedDateTime FROM DEL_RECORD_OF_BILL_DELETE WHERE Convert(date,EntryDate) BETWEEN '" + dateFrom.ToString("yyyy-MM-dd") + "' AND '" + dateTo.ToString("yyyy-MM-dd") + "' AND ModuleName='"+ projectName +"'";
                var lists = new List<BillModel>();
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new BillModel()
                    {
                        BillId = Convert.ToInt32(rdr["MasterId"]),
                        PatientName = rdr["PatientName"].ToString(),
                        BillNo = rdr["BillNo"].ToString(),
                        BillDate = Convert.ToDateTime(rdr["BillDate"]),
                        TotalAmt = Convert.ToDouble(rdr["TotalAmt"]),
                        BillStatus = Convert.ToInt32(rdr["STATUS"]),
                        PostedBy= rdr["PostedBy"].ToString(),
                        DeliveryTimeAmPm = rdr["AuthorizedDateTime"].ToString()

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

        internal string Save(Model.DoctorModel mdl)
        {
            throw new NotImplementedException();
        }
    }
}
