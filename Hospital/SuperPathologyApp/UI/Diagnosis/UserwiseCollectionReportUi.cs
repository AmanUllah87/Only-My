using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
 
using System.Windows.Forms;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Report;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace SuperPathologyApp.UI
{
    public partial class UserwiseCollectionReportUi : Form
    {
        public UserwiseCollectionReportUi()
        {
            InitializeComponent();
        }
        DbConnection _gt = new DbConnection();
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnReceiveInDeliveryCounter_Click(object sender, EventArgs e)
        {





            string query = "EXEC Sp_Sales_ledger '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "','" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "'";
            var dt = new LabReqViewer("BillLedger", query, "Date from "+dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd")+" and "+dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "Sp_Sales_ledger","Sales Ledger");
            dt.ShowDialog();

            
         
            
            
         
        }


        
        
        
        
     

        private void button1_Click_1(object sender, EventArgs e)
        {
           

            MessageBox.Show("Success");
        }

        private void AddReportUi_Load(object sender, EventArgs e)
        {
            //_gt.LoadComboBox("SELECT distinct 0 AS Id,MachineName AS Description FROM MachineDataDtls Order By MachineName", machineNameComboBox);

        }

        private void DiagnosisReportUi_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM V_Due_Invoice_List";
            var dt = new LabReqViewer("DueList", query, "Date upto " + DateTime.Now.ToString("yyyy-MM-dd"), "V_Due_Invoice_List", "Due List");
            dt.ShowDialog();

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string query = "SELECT * FROM V_Due_Collection_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "'";
            var dt = new LabReqViewer("DueCollList", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Due_Collection_List", "Due Collection List");
            dt.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM V_Daily_Collection_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "'";
            var dt = new LabReqViewer("DailyCollection", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Daily_Collection_List", "Daily Collection List");
            dt.ShowDialog();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM V_Counted_List_Of_Tested_Item WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "'";
            var dt = new LabReqViewer("ItemCountList", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Counted_List_Of_Tested_Item", "Counted Item List");
            dt.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM V_Bill_Register WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "'";
            var dt = new LabReqViewer("BillRegister", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Bill_Register", "Bill Register");
            dt.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM V_Bill_Register WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' Order By SalesAmt";
            var dt = new LabReqViewer("TopContribution", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Bill_Register", "Doctor Contribution");
            dt.ShowDialog();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //
            string query = "SELECT * FROM V_Doctor_Honoriam WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' ";
            var dt = new LabReqViewer("DoctorComisionDtls", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Doctor_Honoriam", "Doctor Comision");
            dt.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM V_Daily_Collection_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND PostedBy='"+ Hlp.UserName +"'";
            var dt = new LabReqViewer("DailyCollectionUserWise", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Daily_Collection_List", "Daily Collection List");
            dt.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM DEL_RECORD_OF_BILL_DELETE WHERE Convert(Date,EntryDate) Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "'";
            var dt = new LabReqViewer("DeleteRecord", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "DEL_RECORD_OF_BILL_DELETE", "Daily Delete List");
            dt.ShowDialog();
        }
    }
}
