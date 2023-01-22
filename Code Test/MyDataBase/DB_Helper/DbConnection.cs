using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyDataBase.DB_Helper;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace MyDataBase.DB_Helper
{
    internal class DbConnection
    {
        
        //public SqlConnection ConLab = new SqlConnection(@"Data Source = SERVER2; Initial Catalog = SuperPathDb_Digital; User ID = sa; Password=Admin12345");
        static readonly string PathAddressLabSoft = Application.StartupPath + "\\dbConnection.txt";
        static readonly string[] LinesLabSoft = File.ReadAllLines(@"" + PathAddressLabSoft + "");
        public SqlConnection ConLab = new SqlConnection(@"server=" + hlp.GetDecryptedDataForDb(LinesLabSoft[0]) + ";Database=" + hlp.GetDecryptedDataForDb(LinesLabSoft[1]) + ";User ID=" + hlp.GetDecryptedDataForDb(LinesLabSoft[2]) + ";Password=" + hlp.GetDecryptedDataForDb(LinesLabSoft[3]) + "");
        public string DeleteSuccessMessage = "Delete Success";
        public string DatabaseName = hlp.GetDecryptedDataForDb(LinesLabSoft[1]);



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

                if (ConLab.State == ConnectionState.Open)//using System.Data;
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
    }
}
