using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperPathologyApp.Gateway.DB_Helper;

namespace SuperPathologyApp.Gateway
{
    class ExpenseVoucherGateway:DbConnection
    {
        public DataTable GetTestCodeList(string text,int deptId,int subDeptId,string type)
        {

            try
            {
                string query = "", condition = " AND a.Type='"+ type +"'";

                if (deptId!=0)
                {
                    condition += " AND b.DeptId="+ deptId +"";
                }

                if (subDeptId != 0)
                {
                    condition += " AND a.SubDeptId=" + subDeptId+ "";
                }                
                if (text != "")
                {
                    condition += " AND a.Name like '%" + text + "%'";
                }




                query = @"SELECT a.Code,c.Name As DeptName,b.Name AS SubDeptName,a.Name AS ExpenseName 
FROM tb_ac_HEAD a LEFT JOIN tb_ac_SUB_DEPARTMENT b ON a.SubDeptId=b.Id
LEFT JOIN tb_ac_DEPARTMENT c ON b.DeptId=c.Id
 WHERE 1=1  " + condition + " Order By a.Code";

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

        internal DataTable GetTestCodeList(DateTime dateTime, string searchString,string type)
        {
            try
            {
                string query = "", condition = "";

                condition += " AND TrDate='"+ dateTime.ToString("yyyy-MM-dd") +"'";
                if (searchString != "")
                {
                    condition = "";
                    condition += " AND (CONVERT(varchar,TrDate)+TrNo) LIKE '%"+ searchString +"%'";
                }




                if (type=="Expense")
                {
                    query = @"SELECT TrDate,TrNo,SUM(Amount) AS Amount  FROM V_Expense_List WHERE 1=1 " + condition + " GROUP BY TrDate,TrNo Order by TrDate,TrNo";    
                }
                else
                {
                    query = @"SELECT TrDate,TrNo,SUM(Amount) AS Amount  FROM V_Income_List WHERE 1=1 " + condition + " GROUP BY TrDate,TrNo Order by TrDate,TrNo";    
                }

                










                ConLab.Open();
                var da = new SqlDataAdapter(query, ConLab);
                var ds = new DataSet();
                da.Fill(ds, "pCode");
                var table = ds.Tables["pCode"];
                ConLab.Close();
                return table;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
