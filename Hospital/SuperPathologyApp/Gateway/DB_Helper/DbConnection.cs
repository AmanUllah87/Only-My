using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Windows.Forms;
using SuperPathologyApp.Gateway.DB_Helper;

namespace SuperPathologyApp.Gateway.DB_Helper
{
    public class DbConnection
    {
        
        
        
        
        static readonly string PathAddressLabSoft = Application.StartupPath + "\\dbConnection.txt";
        static readonly string[] LinesLabSoft = File.ReadAllLines(@"" + PathAddressLabSoft + "");
        public SqlConnection ConLab = new SqlConnection(@"server=" + Hlp.GetDecryptedDataForDb(LinesLabSoft[0]) + ";Database=" + Hlp.GetDecryptedDataForDb(LinesLabSoft[1]) + ";User ID=" + Hlp.GetDecryptedDataForDb(LinesLabSoft[2]) + ";Password=" + Hlp.GetDecryptedDataForDb(LinesLabSoft[3]) + "");
       
        public SqlConnection ConOther = new SqlConnection(@"server=192.168.19.16;Database=HPM;User ID=BBCL;Password=bbcl");
        public string ReportQuery = "";
        public string SaveSuccessMessage = "Saved Success";
        public string UpdateSuccessMessage = "Update Success";
        public string DeleteSuccessMessage = "Delete Success";
        public string AlreadyExistMessage = "This Name Already Exists";


        public DataTable Search(string Sql)
        {
            DataTable dataSet = new DataTable();
            new SqlDataAdapter(Sql, ConLab).Fill(dataSet);
            return dataSet;
        }
        public DataSet SearchDs(string Sql)
        {
            DataSet dataSet = new DataSet();
            new SqlDataAdapter(Sql, ConLab).Fill(dataSet);
            return dataSet;
        }

        public void GridColor(DataGridView dg)
        {
            int i = 0;
            while (i < dg.Rows.Count)
            {
                dg.Rows[i].DefaultCellStyle.BackColor = Color.Azure;
                i += 2;
            }
        }
        public  bool IsNumeric(string number)
        {
           
            try
            {
                double dbl = Convert.ToDouble(number);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        public string DeleteInsertLab(string id)
        {
            try
            {
                ConLab.Open();
                string autoId = id;
                var command = new SqlCommand(autoId, ConLab);
                command.ExecuteNonQuery();
                return DeleteSuccessMessage;
            }
            catch (Exception ex)
            {
               
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                throw new Exception(ex.Message);
            }
            finally
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
            }
        }
        public string DeleteInsertLab(string id, SqlTransaction trans)
        {
            try
            {
                //ConLab.Open();
                string autoId = id;
                var command = new SqlCommand(autoId, ConLab, trans);
                command.ExecuteNonQuery();
                return DeleteSuccessMessage;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    //  ConLab.Close();
                }
            }
        }
        public string DeleteInsertLab(string id, SqlTransaction trans, SqlConnection con)
        {
            try
            {
                //ConLab.Open();
                string autoId = id;
                var command = new SqlCommand(autoId, con, trans);
                command.ExecuteNonQuery();
                return DeleteSuccessMessage;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (ConLab.State == ConnectionState.Open)
                {
                    //  ConLab.Close();
                }
            }
        }

        public bool FnSeekRecordNewLab(string tableName, string condition)
        {
            try
            {
                string query = @"Select * from " + tableName + " WHERE " + condition + "";
                var cmd = new SqlCommand(query, ConLab);
                ConLab.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                bool isExist = rdr.HasRows;
                rdr.Close();
                ConLab.Close();
                return isExist;
            }
            catch (Exception )
            {
                if (ConLab.State == ConnectionState.Open)
                {
                      ConLab.Close();
                }
                return false;
            }
           
        }
        public bool FnSeekRecordNewOther(string tableName, string condition)
        {
            string query = @"Select * from " + tableName + " WHERE " + condition + "";
            var cmd = new SqlCommand(query, ConOther);
            ConOther.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            bool isExist = rdr.HasRows;
            rdr.Close();
            ConOther.Close();
            return isExist;
        }
        public bool FnSeekRecordNewLab(string tableName, string condition, SqlTransaction transaction, SqlConnection con)
        {
            string query = @"Select * from " + tableName + " WHERE " + condition + "";
            var cmd = new SqlCommand(query, con, transaction);
            SqlDataReader rdr = cmd.ExecuteReader();
            bool isExist = rdr.HasRows;
            rdr.Close();
            return isExist;
        }
        public bool FnSeekRecordNewOther(string tableName, string condition, SqlTransaction transaction)
        {
            string query = @"Select * from " + tableName + " WHERE " + condition + "";
            var cmd = new SqlCommand(query, ConOther, transaction);
            SqlDataReader rdr = cmd.ExecuteReader();
            bool isExist = rdr.HasRows;
            rdr.Close();
            return isExist;
        }
        public string FncReturnFielValueLabOpenCon(string tableName, string condition, string fieldName)
        {
            string returnfield = "";
            string query = @"SELECT  " + fieldName + " AS Description FROM " + tableName + " WHERE " + condition + "";
            var cmd = new SqlCommand(query, ConLab);
            //ConLab.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                returnfield = rdr["Description"].ToString();
            }
            rdr.Close();
            //ConLab.Close();
            return returnfield;
        }
        public string FncReturnFielValueLab(string tableName, string condition, string fieldName)
        {
            try
            {
                string returnfield = "";
                string query = @"SELECT  " + fieldName + " AS Description FROM " + tableName + " WHERE " + condition + "";
                var cmd = new SqlCommand(query, ConLab);
                ConLab.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    returnfield = rdr["Description"].ToString();
                }
                rdr.Close();
                ConLab.Close();
                return returnfield;

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
        public string FncReturnFielValueLab(string tableName, string condition, string fieldName, SqlTransaction trans)
        {
            string returnfield = "";
            string query = @"SELECT  " + fieldName + " AS Description FROM " + tableName + " WHERE " + condition + "";
            var cmd = new SqlCommand(query, ConLab, trans);

            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                returnfield = rdr["Description"].ToString();
            }
            rdr.Close();

            return returnfield;
        }
        public string FncReturnFielValueLab(string tableName, string condition, string fieldName, SqlTransaction trans, SqlConnection con)
        {
            string returnfield = "";
            string query = @"SELECT  " + fieldName + " AS Description FROM " + tableName + " WHERE " + condition + "";
            var cmd = new SqlCommand(query, con, trans);

            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                returnfield = rdr["Description"].ToString();
            }
            rdr.Close();

            return returnfield;
        }
        public string FncReturnFielValueOther(string tableName, string condition, string fieldName)
        {
            string returnfield = "";
            string query = @"SELECT TOP 1 " + fieldName + " AS Description FROM " + tableName + " WHERE " + condition + "";
            var cmd = new SqlCommand(query, ConOther);
            ConOther.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                returnfield = rdr["Description"].ToString();
            }
            rdr.Close();
            ConOther.Close();
            return returnfield;
        }
        public string FncReturnFielValueOther(string tableName, string condition, string fieldName, SqlTransaction trans)
        {
            string returnfield = "";
            string query = @"SELECT  " + fieldName + " AS Description FROM " + tableName + " WHERE " + condition + "";
            var cmd = new SqlCommand(query, ConOther, trans);
            //ConOther.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                returnfield = rdr["Description"].ToString();
            }
            rdr.Close();
            //ConOther.Close();
            return returnfield;
        }
        public void LoadComboBoxForDefault(string query, ComboBox comboBoxName)
        {
            try
            {
                var dt = new DataTable();
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var da = new SqlDataAdapter { SelectCommand = cmd };
                da.Fill(dt);
                comboBoxName.DisplayMember = "Description";
                comboBoxName.ValueMember = "Id";
                //DataRow dr = dt.NewRow();
               // dt.Rows.InsertAt(dr, 0);
                comboBoxName.DataSource = dt;
                ConLab.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void LoadComboBox(string query, ComboBox comboBoxName)
        {
            try
            {
                var dt = new DataTable();
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var da = new SqlDataAdapter { SelectCommand = cmd };
                da.Fill(dt);
                comboBoxName.DisplayMember = "Description";
                comboBoxName.ValueMember = "Id";
                DataRow dr = dt.NewRow();
                dr[0] = 0;
                dr[1] = "--Select--";
                dt.Rows.InsertAt(dr, 0);
                comboBoxName.DataSource = dt;
                ConLab.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void LoadComboBoxSingleItem(string query, ComboBox comboBoxName)
        {
            try
            {
                var dt = new DataTable();
                ConLab.Open();
                var cmd = new SqlCommand(query, ConLab);
                var da = new SqlDataAdapter { SelectCommand = cmd };
                da.Fill(dt);
                comboBoxName.DisplayMember = "Description";
                comboBoxName.ValueMember = "Description";
                DataRow dr = dt.NewRow();
                dr[0] = 0;
                dr[1] = "--Select--";
                dt.Rows.InsertAt(dr, 0);
                comboBoxName.DataSource = dt;
                ConLab.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public int GetMaxId(string tableName, string fieldName)
        {
            int slNo = 0;
            string sql = @"SELECT  Isnull(MAX(" + fieldName + "),0)+1 AS TrNo  FROM " + tableName + " ";
            ConLab.Open();
            var aCommand = new SqlCommand(sql, ConLab);
            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                slNo = Convert.ToInt32(aReader["TrNo"].ToString());
            }
            ConLab.Close();
            return slNo;
        }
        public string GetInvoiceNo(string tableName, string fieldName,string onChange)
        {
            string  slNo = "";
            string sql = @"SELECT RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) + RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT("+ fieldName +", 4))),0)+ 1), 4) AS InvNo FROM "+ tableName +"  WHERE YEAR("+ onChange +")=YEAR(GetDate())  ";
            ConLab.Open();
            var aCommand = new SqlCommand(sql, ConLab);
               SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                slNo = aReader["InvNo"].ToString();
            }
            ConLab.Close();
            return slNo;
        }
        public string GetInvoiceNo(string tableName, string fieldName, string onChange, SqlConnection con, SqlTransaction tran)
        {
            string slNo = "";
            string sql = @"SELECT RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) + RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(" + fieldName + ", 4))),0)+ 1), 4) AS InvNo FROM " + tableName + "  WHERE YEAR(" + onChange + ")=YEAR(GetDate())  ";
            var aCommand = new SqlCommand(sql, con,tran);
            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                slNo = aReader["InvNo"].ToString();
            }
            aReader.Close();
            return slNo;
        }









        public static Color EnterFocus()
        {
            return Color.PaleGoldenrod;
        }
        public static Color LeaveFocus()
        {
            return Color.White;
        }
        public static void ToCsV(DataGridView dGv, string filename)
        {
            string stOutput = "";
            string sHeaders = "";

            for (int j = 0; j < dGv.Columns.Count; j++)
                sHeaders = sHeaders.ToString() + Convert.ToString(dGv.Columns[j].HeaderText) + "\t";
            stOutput += sHeaders + "\r\n";
            for (int i = 0; i < dGv.RowCount; i++)
            {
                string stLine = "";
                for (int j = 0; j < dGv.Rows[i].Cells.Count; j++)
                    stLine = stLine.ToString() + Convert.ToString(dGv.Rows[i].Cells[j].Value) + "\t";
                stOutput += stLine + "\r\n";
            }
            Encoding utf16 = Encoding.GetEncoding(1254);
            byte[] output = utf16.GetBytes(stOutput);
            var fs = new FileStream(filename, FileMode.Create);
            var bw = new BinaryWriter(fs);
            bw.Write(output, 0, output.Length); //write the encoded file
            bw.Flush();
            bw.Close();
            fs.Close();
        }
     
        internal Boolean IsDuplicate(string lcCode, DataGridView dataGrid)
        {
            try
            {
                for (int j = 0; j < dataGrid.Rows.Count; j++)
                {
                    string lcPdCode = dataGrid.Rows[j].Cells[0].Value.ToString();
                    if (lcCode == lcPdCode)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
           
        }
        public DataTable ConvertListDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    
        public string GetEncryptedData(string password)
        {
            var array = password.ToCharArray();
            return array.Select(chara => Encoding.ASCII.GetBytes(chara.ToString())).Select(n => (char)Convert.ToInt32(n[0] + 3)).Aggregate("", (current, character) => current + character.ToString());
        }
        public string GetDecryptedData(string password)
        {
            var array = password.ToCharArray();
            return array.Select(chara => Encoding.ASCII.GetBytes(chara.ToString())).Select(n => (char)Convert.ToInt32(n[0] - 3)).Aggregate("", (current, character) => current + character.ToString());
        }

     


        public string GetInvoiceNo(int stockType, SqlTransaction trans,SqlConnection con)
        {
            string lcsql = @"Exec SP_GET_INVOICENO " + stockType + "";
            // Con.Open();
            var aCommand = new SqlCommand(lcsql, con, trans);
            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                lcsql = aReader["InvNo"].ToString();
            }
            // Con.Close();
            aReader.Close();
            return lcsql;
        }
        public string GetInvoiceNo(int stockType)
        {
            string lcsql = @"Exec SP_GET_INVOICENO " + stockType + "";
            ConLab.Open();
            var aCommand = new SqlCommand(lcsql, ConLab);
            SqlDataReader aReader = aCommand.ExecuteReader();
            while (aReader.Read())
            {
                lcsql = aReader["InvNo"].ToString();
            }
            ConLab.Close();
            aReader.Close();
            return lcsql;
        }


        public string GetRtnNo()
        {
            return FncReturnFielValueLab("tb_BILL_RETURN", "YEAR(TrDate)=YEAR(GetDate()) AND MONTH(TrDate)=MONTH(GetDate())", "RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) + RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(TrNo, 4))),0)+ 1), 4)");
        }



    }
}
