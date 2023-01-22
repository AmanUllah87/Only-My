using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Indoor;
using System.IO;
using System.Diagnostics;
using CrystalDecisions.CrystalReports.Engine;
using SuperPathologyApp.Report.DataSet;

namespace SuperPathologyApp.Gateway.DB_Helper
{
    public static class Hlp
    {
        public static string UserName = "Rakib";
        public static string Message = "";
        public static bool IsNursStation =false;
        public static string GroupName = "";
        public static bool AutoPrint = false;
        public static string SaveMessage = "Save Success";
        public static string UpdateMessage = "Update Success";
        public static DateTime ServerDate = DateTime.Now;
        public static string InvoiceNoComeFromIndividualSearch = "";
        public static bool IsComeFromIndividualSearch;
        public static string ReportHeaderName="";
        public static string FirstFormVal = "";
        public static ReportDocument _rprt = new ReportDocument();

        public static int PhCode = 13508;

        public static bool IsIndoor = false;

        public static double StringToDouble(string number)
        {
            if (IsNumeric(number))
            {
                return Convert.ToDouble(number);
            }
            else
            {
                return 0;
            }
        }
        public static DateTime GetServerDate()
        {
            DbConnection _gt = new DbConnection();
            return Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "1=1", "GetDate()"));
        }
        public static DateTime GetServerDate(SqlConnection con,SqlTransaction tran)
        {
            DbConnection _gt = new DbConnection();
            return Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "1=1", "GetDate()",tran,con));
        }
        public static string GetCharacterByNo(int number)
        {
            string character = "";
            switch (number)
            {
                case 1:
                    character = "A";
                    break;
                case 2:
                    character = "B";
                    break;
                case 3:
                    character = "C";
                    break;
                case 4:
                    character = "D";
                    break;
                case 5:
                    character = "E";
                    break;
                case 6:
                    character = "F";
                    break;
                case 7:
                    character = "G";
                    break;
                case 8:
                    character = "H";
                    break;
                case 9:
                    character = "I";
                    break;
                case 10:
                    character = "J";
                    break;
                case 11:
                    character = "K";
                    break;
                case 12:
                    character = "L";
                    break;
                case 13:
                    character = "M";
                    break;
                case 14:
                    character = "N";
                    break;
                case 15:
                    character = "O";
                    break;

                default:
                    break;
            }

            return character;

        }
        public static int GetNumberByCharacter(string character)
        {
            int number = 0;
            switch (character)
            {
                case "A":
                    number = 1;
                    break;
                case "B":
                    number = 2;
                    break;
                case "C":
                    number = 3;
                    break;
                case "D":
                    number = 4;
                    break;
                case "E":
                    number = 5;
                    break;
                case "F":
                    number = 6;
                    break;
                case "G":
                    number = 7;
                    break;
                case "H":
                    number = 8;
                    break;
                case "I":
                    number = 9;
                    break;
                case "J":
                    number = 10;
                    break;
                case "K":
                    number = 11;
                    break;
                case "L":
                    number = 12;
                    break;
                case "M":
                    number = 13;
                    break;
                case "N":
                    number = 14;
                    break;
                case "O":
                    number = 15;
                    break;


                default:
                    break;
            }

            return number;

        }
        public static void AutoPrintInvoice(string reportFileName,string fileLoc, string query, string dataSetName, string title)
        {
            DbConnection _gt = new DbConnection();  
            try
            {
                string comName = _gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ComName");
                string comAddress = _gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "Address");

                _gt.ConLab.Open();
                string path = Application.StartupPath;
                path = path + @"\Report\File\"+ fileLoc +"\\";

                _rprt.Load(path + "" + reportFileName + ".rpt");

                var cmd = new SqlCommand(query, _gt.ConLab);
                var sda = new SqlDataAdapter(cmd);
                var ds = new IndoorDs();
                sda.Fill(ds, dataSetName);
                _rprt.SetDataSource(ds);
                _rprt.SetParameterValue("lcComName", comName);
                _rprt.SetParameterValue("lcComAddress", comAddress);
                _rprt.SetParameterValue("lcDateRange", "");
                _rprt.SetParameterValue("lcTitle", title);
                _rprt.PrintToPrinter(1, true, 0, 0);
                _rprt.Dispose();
                _rprt.Close();

                _gt.ConLab.Close();
            }
            catch (Exception exception)
            {
                if (_gt.ConLab.State == ConnectionState.Open)
                {
                    _gt.ConLab.Close();
                }
                MessageBox.Show(exception.Message);
            }
        }


        public static string IpAddress()
        {
            return Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
        }

        public static void MakeFieldNumeric()
        { 
        
        
        }







        public static string GetAutoIncrementVal(string fieldName, string tableName, int length)
        {
            var gt = new DbConnection();
            string rtn =  gt.FncReturnFielValueLab(tableName, "1=1", "ISNULL(CONVERT(INT,MAX("+ fieldName +")+1),'1')").PadLeft(length, '0');
            return rtn;
        }
        public static string GetAutoIncrementVal(string fieldName, string tableName, int length,SqlConnection con,SqlTransaction tran)
        {
            var gt = new DbConnection();
            string rtn = gt.FncReturnFielValueLab(tableName, "1=1", "ISNULL(CONVERT(INT,MAX(" + fieldName + ")+1),'1')",tran,con).PadLeft(length, '0');
            return rtn;
        }






        public static string GetEncryptedDataForDb(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }
        public static string GetDecryptedDataForDb(string hexString)
        {
            try
            {
                var bytes = new byte[hexString.Length / 2];
                for (var i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }

                return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"

            }
            catch (Exception)
            {

                return hexString;
            }
            
        }



       
        static UnicodeEncoding _unicodeEncoding = new UnicodeEncoding();
        static RSACryptoServiceProvider _rSaCryptoServiceProvider = new RSACryptoServiceProvider();
       


        

        public static bool IsNumeric(string number)
        {
            
            try
            {
                double dbl = Convert.ToDouble(number);
                return true;
            }
            catch (Exception )
            {

                return false;
            }

        }
        public static Point GetPoint()
        {
            return new Point(228, 50);
        }
        
        public static Color EnterFocus()
        {
            return Color.Gold;
        }
        public static Color LeaveFocus()
        {
            return Color.White;
        }

        public static Color GridHightLightColumn()
        {
            return Color.Yellow;
        }



        public static void GridFirstRowDeselect(DataGridView dg)
        {
            if (dg.Rows.Count > 1)
            {
                dg.Rows[0].Selected = false;
                try
                {
                  //  dg.CurrentCell.Selected = false;
                }
                catch (Exception)
                {
                    ;
                }
               
            }           
        }

        public static void GridColor(DataGridView dg)
        {

            dg.RowHeadersVisible = false;
            dg.CurrentCell = null;
           // dg.EnableHeadersVisualStyles = false;
            //dg.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            //dg.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dg.AllowUserToResizeRows = false;
            //dg.CurrentCell.Selected = false;
  
            
            int i = 0;
            while (i < dg.Rows.Count)
            {
                dg.Rows[i].DefaultCellStyle.BackColor = Color.Azure;
                i += 2;
            }
        }

        internal static bool DataGridDuplicateCheck(string gccode, DataGridView dataGridView2)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                string dgCode = dataGridView2.Rows[i].Cells[0].Value.ToString();
                if (dgCode==gccode)
                {
                    return true;
                }
            }
            return false;
        }
        internal static string CalculateAgeByDob(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(PastYearDate).Hours;
            int Minutes = Now.Subtract(PastYearDate).Minutes;
            int Seconds = Now.Subtract(PastYearDate).Seconds;
            return String.Format("{0}Y{1}M{2}D",Years, Months, Days);
        }
        public static DateTime CalculateDob(DateTime currentDate, int ageyear , int agemonth,int ageday)
        {
            currentDate=currentDate.AddYears(-ageyear);
            currentDate=currentDate.AddMonths(-agemonth);
            currentDate=currentDate.AddDays(-ageday);
            return currentDate;
        }
        internal static void InsertIntoLedger(string trNo,DateTime trDate,int masterId,double salesAmt,double lessAmt,double rtnAmt,double collAmt,string postedBy,SqlTransaction trans,SqlConnection con)
        {
            const string query = "INSERT INTO tb_BILL_LEDGER (TrNo, TrDate, MasterId, SalesAmt, LessAmt, RtnAmt, CollAmt, PostedBy)VALUES(@TrNo, FORMAT(getdate(),'yyyy-MM-dd'), @MasterId, @SalesAmt, @LessAmt, @RtnAmt, @CollAmt, @PostedBy)";
            var cmd = new SqlCommand(query, con, trans);
            cmd.Parameters.AddWithValue("@TrNo", trNo);
            cmd.Parameters.AddWithValue("@TrDate", trDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@MasterId", masterId);
            cmd.Parameters.AddWithValue("@SalesAmt", salesAmt);
            cmd.Parameters.AddWithValue("@LessAmt", lessAmt);
            cmd.Parameters.AddWithValue("@RtnAmt", rtnAmt);
            cmd.Parameters.AddWithValue("@CollAmt", collAmt);
            cmd.Parameters.AddWithValue("@PostedBy", postedBy);
            
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        
        internal static void InsertIntoPurchaseLedgerPharmacy(int suppId, string trNo, DateTime trDate, int masterId, double purchaseAmt, double lessAmt, double rtnAmt, double paymentAmt, string postedBy, SqlTransaction trans, SqlConnection con)
        {
            const string query = "INSERT INTO tb_ph_PURCHASE_LEDGER (TrNo, TrDate,SuppId, MasterId, PurchaseAmt, LessAmt, RtnAmt, PaymentAmt, PostedBy)VALUES(@TrNo,FORMAT(getdate(),'yyyy-MM-dd'),@SuppId, @MasterId, @PurchaseAmt, @LessAmt, @RtnAmt, @PaymentAmt, @PostedBy)";
            var cmd = new SqlCommand(query, con, trans);
            cmd.Parameters.AddWithValue("@TrNo", trNo);
            cmd.Parameters.AddWithValue("@TrDate", trDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@SuppId", suppId);
            cmd.Parameters.AddWithValue("@MasterId", masterId);
            cmd.Parameters.AddWithValue("@PurchaseAmt", purchaseAmt);
            cmd.Parameters.AddWithValue("@LessAmt", lessAmt);
            cmd.Parameters.AddWithValue("@RtnAmt", rtnAmt);
            cmd.Parameters.AddWithValue("@PaymentAmt", paymentAmt);
            cmd.Parameters.AddWithValue("@PostedBy", postedBy);
            
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                
                throw;
            }
        }











        internal static void InsertIntoSalesLedgerPharmacy(int custId, string trNo, DateTime trDate, int masterId, double salesAmt, double lessAmt, double rtnAmt, double collAmt, double lessAdjust, string postedBy, int admId, SqlTransaction trans, SqlConnection con)
        {
            const string query = "INSERT INTO tb_ph_SALES_LEDGER (TrNo, TrDate, CustId, MasterId, SalesAmt, LessAmt, RtnAmt, CollAmt, PostedBy,AdmId,LessAdjust)VALUES(@TrNo, FORMAT(getdate(),'yyyy-MM-dd'), @CustId, @MasterId, @SalesAmt, @LessAmt, @RtnAmt, @CollAmt, @PostedBy,@AdmId,@LessAdjust)";
            var cmd = new SqlCommand(query, con, trans);
            cmd.Parameters.AddWithValue("@TrNo", trNo);
            cmd.Parameters.AddWithValue("@TrDate", trDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@CustId", custId);
            cmd.Parameters.AddWithValue("@MasterId", masterId);
            cmd.Parameters.AddWithValue("@SalesAmt", salesAmt);
            cmd.Parameters.AddWithValue("@LessAmt", lessAmt);
            cmd.Parameters.AddWithValue("@RtnAmt", rtnAmt);
            cmd.Parameters.AddWithValue("@CollAmt", collAmt);
            cmd.Parameters.AddWithValue("@PostedBy", postedBy);
            cmd.Parameters.AddWithValue("@AdmId", admId);
            cmd.Parameters.AddWithValue("@LessAdjust", lessAdjust);
            

            
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                
                throw;
            }
        }









        internal static void InsertIntoStockLedgerPharmacy(int masterId, string trNo, DateTime trDate, int itemId, double purchasePrice, double salesPrice, double inQty, double outQty, string module,int custId,int indoorId, SqlTransaction trans, SqlConnection con)
        {
            const string query = "INSERT INTO tb_ph_STOCK_LEDGER (MasterId, TrNo, TrDate, ItemId, PurchasePrice, SalesPrice, InQty, OutQty, Module,CustId,AdmId)VALUES(@MasterId, @TrNo, FORMAT(getdate(),'yyyy-MM-dd'), @ItemId, @PurchasePrice, @SalesPrice, @InQty, @OutQty, @Module,@CustId,@AdmId)";
            var cmd = new SqlCommand(query, con, trans);
            cmd.Parameters.AddWithValue("@MasterId", masterId);
            cmd.Parameters.AddWithValue("@TrNo", trNo);
            cmd.Parameters.AddWithValue("@TrDate", trDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@ItemId", itemId);
            cmd.Parameters.AddWithValue("@PurchasePrice", purchasePrice);
            cmd.Parameters.AddWithValue("@SalesPrice", salesPrice);
            cmd.Parameters.AddWithValue("@InQty", inQty);
            cmd.Parameters.AddWithValue("@OutQty", outQty);
            cmd.Parameters.AddWithValue("@Module", module);
            cmd.Parameters.AddWithValue("@CustId", custId);
            cmd.Parameters.AddWithValue("@AdmId", indoorId);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }

            
        }







        internal static void InsertIntoEditRecord(string trNo, DateTime trDate, string patientName, double prevAmt, double currAmt, double prevTestNo, double currTestNo, int prevRefDrId, int currRefDrId, string moduleName, SqlTransaction trans, SqlConnection con)
        {
            string query = "INSERT INTO EDIT_RECORD_OF_BILL (TrNo, TrDate,  PatientName, PrevAmt, CurrAmt, PrevTestNo, CurrTestNo,  PrevRefDrId, CurrRefDrId, ModuleName,PostedBy,PcName,IpAddress)VALUES(@TrNo, FORMAT(getdate(),'yyyy-MM-dd'),  @PatientName, @PrevAmt, @CurrAmt, @PrevTestNo, @CurrTestNo,  @PrevRefDrId, @CurrRefDrId, @ModuleName,'" + Hlp.UserName + "',@PcName,@IpAddress)";
            var cmd = new SqlCommand(query, con, trans);
            cmd.Parameters.AddWithValue("@TrNo", trNo);
            cmd.Parameters.AddWithValue("@TrDate", trDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@PatientName", patientName);
            cmd.Parameters.AddWithValue("@PrevAmt", prevAmt);
            cmd.Parameters.AddWithValue("@CurrAmt", currAmt);
            cmd.Parameters.AddWithValue("@PrevTestNo", prevTestNo);
            cmd.Parameters.AddWithValue("@CurrTestNo", currTestNo);
            cmd.Parameters.AddWithValue("@PrevRefDrId", prevRefDrId);
            cmd.Parameters.AddWithValue("@CurrRefDrId", currRefDrId);
            cmd.Parameters.AddWithValue("@ModuleName", moduleName);
            cmd.Parameters.AddWithValue("@PcName", Environment.UserName);
            cmd.Parameters.AddWithValue("@IpAddress", Hlp.IpAddress());
            cmd.ExecuteNonQuery();
        }
        private static string GetRegNo()
        {
            var gt = new DbConnection();
            string rtn=  "R"+ gt.FncReturnFielValueLab("tb_PATIENT", "1=1", "ISNULL(CONVERT(INT,MAX(RIGHT(RegNo,6))+1),'1')").PadLeft(6,'0');
            return rtn;
        }
        private static string GetRegNo(SqlConnection con,SqlTransaction tran)
        {
            var gt = new DbConnection();
            string rtn = "R" + gt.FncReturnFielValueLab("tb_PATIENT", "1=1", "ISNULL(CONVERT(INT,MAX(RIGHT(RegNo,6))+1),'1')",tran,con).PadLeft(6, '0');
            return rtn;
        }

        internal static int InsertIntoPatient(string name, string address, string  contactNo, string sex, DateTime dob)
        {
            int regId = 0;
            var gt = new DbConnection();
            if (gt.FnSeekRecordNewLab("tb_Patient","ContactNo='"+ contactNo +"' AND Name='"+ name +"'")==false)
            {
                const string query = "INSERT INTO tb_Patient (RegNo, Name, Address, ContactNo, Sex, Dob, PostedBy)OUTPUT INSERTED.ID VALUES (@RegNo, @Name, @Address, @ContactNo, @Sex, @Dob, @PostedBy)";
                var cmd = new SqlCommand(query, gt.ConLab);
                cmd.Parameters.AddWithValue("@RegNo", GetRegNo());
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@ContactNo", contactNo);
                cmd.Parameters.AddWithValue("@Sex", sex);
                cmd.Parameters.AddWithValue("@Dob", dob.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);
                gt.ConLab.Open();
                regId=(int)cmd.ExecuteScalar();
                gt.ConLab.Close();
            }
            else
            {
                regId =Convert.ToInt32(gt.FncReturnFielValueLab("tb_Patient", "ContactNo='" + contactNo + "' AND Name='" + name + "'","Id"));
            }
            return regId;
        }
        internal static PatientModel GetAgeSexByInvNo(int masterId)
        {
            var gt = new DbConnection();
            int regId = Convert.ToInt32(gt.FncReturnFielValueLab("tb_BILL_MASTER", "Id=" + masterId + "", "RegId"));
            try
            {
                var mdl = new PatientModel();
                string query = @"SELECT DATEDIFF(YEAR, Dob, GETDATE()) AS Age,Sex  FROM tb_PATIENT WHERE Id="+ regId +"";
                gt.ConLab.Open();
                var cmd = new SqlCommand(query, gt.ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    mdl.Age = rdr["Age"].ToString();
                    mdl.Sex = rdr["Sex"].ToString();

                }

                rdr.Close();
                gt.ConLab.Close();
                return mdl;
            }
            catch (Exception exception)
            {

                if (gt.ConLab.State == ConnectionState.Open)
                {
                    gt.ConLab.Close();
                }
                throw;
            }
        }


        public static DataTable LoadDbByQuery(int id, string queryForDb)
        {
            var gt = new DbConnection();
            try
            {
                string cond = "";
                
                if (id != 0)
                {
                    cond = "AND Id=" + id + "";
                }
                string query = @"" + queryForDb + " " + cond + "";

                gt.ConLab.Open();
                var da = new SqlDataAdapter(query, gt.ConLab);
                var ds = new DataSet();
                da.Fill(ds, "pCode");
                var table = ds.Tables["pCode"];
                gt.ConLab.Close();
                return table;
            }
            catch (Exception exception)
            {
                if (gt.ConLab.State == ConnectionState.Open)
                {
                    gt.ConLab.Close();
                }
                throw;
            }
        }

        public static DataTable LoadDbByQuery(int id, string queryForDb,string searchString,string searchField)
       {
            var gt = new DbConnection();
            try
            {
                string cond = "";
                if (searchString != "")
                {
                    cond = "AND "+ searchField +" like '%'+'" + searchString + "'+'%'";
                }
                if (id != 0)
                {
                    cond = "AND Id=" + id + "";
                }
                string query = @"" + queryForDb + " WHERE 1=1  " + cond + "";

                gt.ConLab.Open();
                var da = new SqlDataAdapter(query, gt.ConLab);
                var ds = new DataSet();
                da.Fill(ds, "pCode");
                var table = ds.Tables["pCode"];
                gt.ConLab.Close();
                return table;
            }
            catch (Exception )
            {
                if (gt.ConLab.State == ConnectionState.Open)
                {
                    gt.ConLab.Close();
                }
                throw;
            }
        }






        public static AgeModel CalculateAgeByDobReturnAgeModel(DateTime Dob)
        {
            var ageMdl =new AgeModel();
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(PastYearDate).Hours;
            int Minutes = Now.Subtract(PastYearDate).Minutes;
            int Seconds = Now.Subtract(PastYearDate).Seconds;
           ageMdl.Year= Years.ToString().PadLeft(2, '0');
            ageMdl.Month = Months.ToString().PadLeft(2, '0');
            ageMdl.Day = Days.ToString().PadLeft(2, '0');
            return ageMdl;
        }




        public static string InsertIntoPatientLedger(string trNo,DateTime trDate,int regId,int admId,int bedId,int testId,string testName,double charge,int unit,double totCharge,double lessAmt,double collAmt,double rtnAmt,double extraLessAmt)
        {
                 var gt = new DbConnection();
                 const string query = "INSERT INTO tb_in_PATIENT_LEDGER (TrNo, TrDate, RegId, AdmId, BedId, TestId, TestName, Charge, Unit, TotCharge, LessAmt, CollAmt, RtnAmt, ExtraLessAmt) VALUES (@TrNo, @TrDate, @RegId, @AdmId, @BedId, @TestId, @TestName, @Charge, @Unit, @TotCharge, @LessAmt, @CollAmt, @RtnAmt, @ExtraLessAmt)";
                var cmd = new SqlCommand(query, gt.ConLab);
                cmd.Parameters.AddWithValue("@TrNo", GetRegNo());
                cmd.Parameters.AddWithValue("@TrDate", trDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@RegId", regId);
                cmd.Parameters.AddWithValue("@AdmId", admId);
                cmd.Parameters.AddWithValue("@BedId", bedId);
                cmd.Parameters.AddWithValue("@TestId", testId);
                cmd.Parameters.AddWithValue("@TestName", testName);
                cmd.Parameters.AddWithValue("@Charge", charge);
                cmd.Parameters.AddWithValue("@Unit", unit);
                cmd.Parameters.AddWithValue("@TotCharge", totCharge);
                cmd.Parameters.AddWithValue("@LessAmt", lessAmt);
                cmd.Parameters.AddWithValue("@CollAmt", collAmt);
                cmd.Parameters.AddWithValue("@RtnAmt", rtnAmt);
                cmd.Parameters.AddWithValue("@ExtraLessAmt", extraLessAmt);
               

                try
                {
                    gt.ConLab.Open();
                    cmd.ExecuteNonQuery();
                    gt.ConLab.Close();
                    return SaveMessage;
                }
                catch (Exception)
                {

                    throw;
                }



        }
        public static string InsertIntoPatientLedger(string trNo, DateTime trDate, int regId, int admId, int bedId, int testId, string testName, double charge, int unit, double totCharge, double lessAmt, double collAmt, double rtnAmt, double extraLessAmt,double lessAdjust, double drAmt,int drId, SqlConnection con, SqlTransaction tran)
        {
            const string query = "INSERT INTO tb_in_PATIENT_LEDGER (TrNo, TrDate, RegId, AdmId, BedId, TestId, TestName, Charge, Unit, TotCharge, LessAmt, CollAmt, RtnAmt, ExtraLessAmt,DrId,DrAmt,LessAdjust) VALUES (@TrNo, @TrDate, @RegId, @AdmId, @BedId, @TestId, @TestName, @Charge, @Unit, @TotCharge, @LessAmt, @CollAmt, @RtnAmt, @ExtraLessAmt,@DrId,@DrAmt,@LessAdjust)";
            var cmd = new SqlCommand(query, con,tran);
            cmd.Parameters.AddWithValue("@TrNo", trNo);
            cmd.Parameters.AddWithValue("@TrDate", trDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@RegId", regId);
            cmd.Parameters.AddWithValue("@AdmId", admId);
            cmd.Parameters.AddWithValue("@BedId", bedId);
            cmd.Parameters.AddWithValue("@TestId", testId);
            cmd.Parameters.AddWithValue("@TestName", testName);
            cmd.Parameters.AddWithValue("@Charge", charge);
            cmd.Parameters.AddWithValue("@Unit", unit);
            cmd.Parameters.AddWithValue("@TotCharge", totCharge);
            cmd.Parameters.AddWithValue("@LessAmt", lessAmt);
            cmd.Parameters.AddWithValue("@CollAmt", collAmt);
            cmd.Parameters.AddWithValue("@RtnAmt", rtnAmt);
            cmd.Parameters.AddWithValue("@ExtraLessAmt", extraLessAmt);
            cmd.Parameters.AddWithValue("@LessAdjust", lessAdjust);
            //

            cmd.Parameters.AddWithValue("@DrId", drId);
            cmd.Parameters.AddWithValue("@DrAmt", drAmt);

            try
            {
                cmd.ExecuteNonQuery();
                return SaveMessage;
               
            }
            catch (Exception ex)
            {
                Hlp.UpdateLog(ex.Message);
                throw;
            }
          
        }

        public static string InsertIntoAdvanceColl(string trNo, DateTime trDate, int regId, int admId, int bedId, double amount)
        {
            var gt = new DbConnection();
            const string query = "INSERT INTO tb_in_ADVANCE_COLLECTION (TrNo, TrDate, RegId, AdmId, BedId, Amount, PostedBy, PcName, IpAddress) VALUES (@TrNo, @TrDate, @RegId, @AdmId, @BedId, @Amount, @PostedBy, @PcName, @IpAddress)";
            var cmd = new SqlCommand(query, gt.ConLab);
            cmd.Parameters.AddWithValue("@TrNo", Hlp.GetAutoIncrementVal("TrNo", "tb_in_ADVANCE_COLLECTION",6));
            cmd.Parameters.AddWithValue("@TrDate", trDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@RegId", regId);
            cmd.Parameters.AddWithValue("@AdmId", admId);
            cmd.Parameters.AddWithValue("@BedId", bedId);
            cmd.Parameters.AddWithValue("@Amount", amount);
            cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);
            cmd.Parameters.AddWithValue("@PcName", Environment.UserName);
            cmd.Parameters.AddWithValue("@IpAddress", Hlp.IpAddress());
           



            try
            {
                gt.ConLab.Open();
                cmd.ExecuteNonQuery();
                gt.ConLab.Close();



                return SaveMessage;

            }
            catch (Exception ex)
            {
                Hlp.UpdateLog(ex.Message);
                throw;
            }
            
            
            
            
            
            
            
            
            
            
            
            
        }
        public static string InsertIntoAdvanceColl(string trNo, DateTime trDate, int regId, int admId, int bedId, double amount, SqlConnection con, SqlTransaction tran)
        {
           
            const string query = "INSERT INTO tb_in_ADVANCE_COLLECTION (TrNo, TrDate, RegId, AdmId, BedId, Amount, PostedBy, PcName, IpAddress) VALUES (@TrNo, @TrDate, @RegId, @AdmId, @BedId, @Amount, @PostedBy, @PcName, @IpAddress)";
            var cmd = new SqlCommand(query, con,tran);
            cmd.Parameters.AddWithValue("@TrNo", trNo);
            cmd.Parameters.AddWithValue("@TrDate", trDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@RegId", regId);
            cmd.Parameters.AddWithValue("@AdmId", admId);
            cmd.Parameters.AddWithValue("@BedId", bedId);
            cmd.Parameters.AddWithValue("@Amount", amount);
            cmd.Parameters.AddWithValue("@PostedBy", Hlp.UserName);
            cmd.Parameters.AddWithValue("@PcName", Environment.UserName);
            cmd.Parameters.AddWithValue("@IpAddress", Hlp.IpAddress());


            try
            {
                cmd.ExecuteNonQuery();
                return SaveMessage;

            }
            catch (Exception)
            {

                throw;
            }

        }


       

        private static void UpdateLog(string message)
        {
            try
            {
                File.AppendAllText(Application.StartupPath + "\\ErrorLog.txt", DateTime.Now + @":::::" + message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        internal static AdmissionModel GetRegAndBedId(int admId)
        {
            var gt = new DbConnection();
            try
            {
                var mdl = new AdmissionModel();
                string query = @"SELECT RegId,BedId,PtName,PtAddress,PtSex,AdmDate,AdmTime,RefDrName,UnderDrName,DeptName,BedName,Floor,ChiefComplain,PtAge,ContactNo,RefId,UnderDrId  FROM V_Admission_List WHERE  ReleaseStatus=0 AND Id=" + admId + "";
                gt.ConLab.Open();
                var cmd = new SqlCommand(query, gt.ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {


                    mdl.Patient = new PatientModel()
                    {
                        RegId = Convert.ToInt32(rdr["RegId"]),
                        Name = rdr["PtName"].ToString(),
                        Address = rdr["PtAddress"].ToString(),
                        Sex = rdr["PtSex"].ToString(),
                        Age = rdr["PtAge"].ToString(),
                        ContactNo = rdr["ContactNo"].ToString(),
                    };
                    mdl.Bed = new BedModel()
                    {
                        BedId = Convert.ToInt32(rdr["BedId"]),
                        Name = rdr["BedName"].ToString(),
                        Floor = rdr["Floor"].ToString(),
                        Department = new DepartmentModel() { Name=rdr["DeptName"].ToString()},
                    };
                    mdl.RefDoctor = new DoctorModel()
                    {
                        DrId = Convert.ToInt32(rdr["RefId"]),
                        Name = rdr["RefDrName"].ToString(),
                    };
                    mdl.UnderDoctor = new DoctorModel()
                    {
                        DrId=Convert.ToInt32(rdr["UnderDrId"]),
                        Name = rdr["UnderDrName"].ToString(),
                    };
                    mdl.AdmDate = Convert.ToDateTime(rdr["AdmDate"]);
                    mdl.AdmTime = rdr["AdmTime"].ToString();
                    mdl.ChiefComplain= rdr["ChiefComplain"].ToString();


                }

                rdr.Close();
                gt.ConLab.Close();
                return mdl;
            }
            catch (Exception ex)
            {
                Hlp.UpdateLog(ex.Message);
                if (gt.ConLab.State == ConnectionState.Open)
                {
                    gt.ConLab.Close();
                }
                throw;
            }
        }
        internal static AdmissionModel GetRegAndBedId(int admId, SqlConnection ConLab, SqlTransaction _trans)
        {
           
            try
            {
                var mdl = new AdmissionModel();
                string query = @"SELECT RegId,BedId,PtName,PtAddress,BedName,PtSex  FROM V_Admission_List WHERE   Id=" + admId + "";
             
                var cmd = new SqlCommand(query, ConLab,_trans);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {


                    mdl.Patient = new PatientModel()
                    {
                        RegId = Convert.ToInt32(rdr["RegId"]),
                        Name = rdr["PtName"].ToString(),
                        Address = rdr["PtAddress"].ToString(),
                        Sex = rdr["PtSex"].ToString(),
                    };
                    mdl.Bed = new BedModel()
                    {
                        BedId = Convert.ToInt32(rdr["BedId"]),
                        Name = rdr["BedName"].ToString()
                    };
                }

                rdr.Close();
               
                return mdl;
            }
            catch (Exception ex)
            {
                Hlp.UpdateLog(ex.Message);
                if (ConLab.State == ConnectionState.Open)
                {
                    ConLab.Close();
                }
                throw;
            }
        }




        internal static void InsertIntoPurchaseLedgerReagent(int suppId, string trNo, DateTime trDate, int masterId, double purchaseAmt, double lessAmt, double rtnAmt, double paymentAmt, string postedBy, SqlTransaction trans, SqlConnection con)
        {
            const string query = "INSERT INTO tb_reagent_PURCHASE_LEDGER (TrNo, TrDate,SuppId, MasterId, PurchaseAmt, LessAmt, RtnAmt, PaymentAmt, PostedBy)VALUES(@TrNo,FORMAT(getdate(),'yyyy-MM-dd'),@SuppId, @MasterId, @PurchaseAmt, @LessAmt, @RtnAmt, @PaymentAmt, @PostedBy)";
            var cmd = new SqlCommand(query, con, trans);
            cmd.Parameters.AddWithValue("@TrNo", trNo);
            cmd.Parameters.AddWithValue("@TrDate", trDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@SuppId", suppId);
            cmd.Parameters.AddWithValue("@MasterId", masterId);
            cmd.Parameters.AddWithValue("@PurchaseAmt", purchaseAmt);
            cmd.Parameters.AddWithValue("@LessAmt", lessAmt);
            cmd.Parameters.AddWithValue("@RtnAmt", rtnAmt);
            cmd.Parameters.AddWithValue("@PaymentAmt", paymentAmt);
            cmd.Parameters.AddWithValue("@PostedBy", postedBy);
            
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                
                throw;
            }
        }





        internal static void InsertIntoStockLedgerReagent(int masterId, string trNo, DateTime trDate, int itemId, double purchasePrice, double salesPrice, double inQty, double outQty, string module, string lotNo,DateTime expireDate, SqlTransaction trans, SqlConnection con)
        {
            const string query = "INSERT INTO tb_reagent_STOCK_LEDGER (MasterId, TrNo, TrDate, ItemId, PurchasePrice, SalesPrice, InQty, OutQty, Module,LotNo,ExpireDate)VALUES(@MasterId, @TrNo, FORMAT(getdate(),'yyyy-MM-dd'), @ItemId, @PurchasePrice, @SalesPrice, @InQty, @OutQty, @Module,@LotNo,@ExpireDate)";
            var cmd = new SqlCommand(query, con, trans);
            cmd.Parameters.AddWithValue("@MasterId", masterId);
            cmd.Parameters.AddWithValue("@TrNo", trNo);
            cmd.Parameters.AddWithValue("@TrDate", trDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@ItemId", itemId);
            cmd.Parameters.AddWithValue("@PurchasePrice", purchasePrice);
            cmd.Parameters.AddWithValue("@SalesPrice", salesPrice);
            cmd.Parameters.AddWithValue("@InQty", inQty);
            cmd.Parameters.AddWithValue("@OutQty", outQty);
            cmd.Parameters.AddWithValue("@Module", module);
            cmd.Parameters.AddWithValue("@LotNo", lotNo);
            cmd.Parameters.AddWithValue("@ExpireDate", expireDate.ToString("yyyy-MM-dd"));


            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                
                throw;
            }

            
        }
        public static string Rc4(string input, string key)
        {
            var result = new StringBuilder();
            int x, y, j = 0;
            var box = new int[256];
            for (int i = 0; i < 256; i++)
                box[i] = i;
            for (int i = 0; i < 256; i++)
            {
                j = (key[i % key.Length] + box[i] + j) % 256;
                x = box[i];
                box[i] = box[j];
                box[j] = x;
            }
            for (int i = 0; i < input.Length; i++)
            {
                y = i % 256;
                j = (box[y] + j) % 256;
                x = box[y];
                box[y] = box[j];
                box[j] = x;
                result.Append((char)(input[i] ^ box[(box[y] + box[j]) % 256]));
            }
            return result.ToString();
        }
        public static string Encrypt(string source, string key)
        {
            using (var tripleDesCryptoService = new TripleDESCryptoServiceProvider())
            {
                using (var hashMd5Provider = new MD5CryptoServiceProvider())
                {
                    byte[] byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                    tripleDesCryptoService.Key = byteHash;
                    tripleDesCryptoService.Mode = CipherMode.ECB;//CBC, CFB
                    byte[] bytes = Encoding.Unicode.GetBytes(source);
                    return Convert.ToBase64String(tripleDesCryptoService.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
                }
            }
        }
        public static string Decrypt(string encrypt, string key)
        {
            try
            {
                using (var tripleDesCryptoService = new TripleDESCryptoServiceProvider())
                {
                    using (var hashMd5Provider = new MD5CryptoServiceProvider())
                    {
                        byte[] byteHash = hashMd5Provider.ComputeHash(Encoding.UTF8.GetBytes(key));
                        tripleDesCryptoService.Key = byteHash;
                        tripleDesCryptoService.Mode = CipherMode.ECB;//CBC, CFB
                        byte[] byteBuff = Convert.FromBase64String(encrypt);
                        return Encoding.Unicode.GetString(tripleDesCryptoService.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
                    }
                }

            }
            catch (Exception)
            {
                return "";
            }
        }

        static byte[] Encrypt(byte[] data, RSAParameters rsaKey, bool fOaep)
        {
            byte[] encryptedData;
            using (var rSaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                rSaCryptoServiceProvider.ImportParameters(rsaKey);
                encryptedData = rSaCryptoServiceProvider.Encrypt(data, fOaep);
            }
            return encryptedData;
        }

        static byte[] Decrypt(byte[] data, RSAParameters rsaKey, bool fOaep)
        {
            byte[] decryptedData;
            using (var rSaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                rSaCryptoServiceProvider.ImportParameters(rsaKey);
                decryptedData = rSaCryptoServiceProvider.Decrypt(data, fOaep);
            }
            return decryptedData;
        }

    }
}
