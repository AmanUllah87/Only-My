using System;
using System.Windows.Forms;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Report;

namespace SuperPathologyApp.UI.Diagnosis
{
    public partial class DiagnosisReportUi : Form
    {
        public DiagnosisReportUi()
        {
            InitializeComponent();
        }

        readonly DbConnection _gt = new DbConnection();
        
        private void btnReceiveInDeliveryCounter_Click(object sender, EventArgs e)
        {

            string query = "EXEC Sp_Sales_ledger '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "','" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "'";
            var dt = new LabReqViewer("BillLedger", query, "Date from "+dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd")+" and "+dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "Sp_Sales_ledger","Sales Ledger");
            dt.Show();

        }

       

        private void DiagnosisReportUi_Load(object sender, EventArgs e)
        {
            _gt.LoadComboBox("SELECT Distinct 0 AS Id, UserName AS Description FROM tb_USER_PRIVILEGE  ", userNameComboBox);
            _gt.LoadComboBox("SELECT Id, Name AS Description FROM tb_MPO  ", mpoComboBox);




        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cond = "", drName = "";
            if (drIdTestBox.Text != "")
            {
                cond += " AND RefDrId=" + drIdTestBox.Text + "";
                drName = "(" + drNameTextBox.Text + ")";
            }
            else
            {
                drName = "";
            }

            //string query = "SELECT * FROM V_Due_Invoice_List WHERE 1=1 "+ cond +"";
            string query = "SELECT * FROM V_Due_Invoice_List WHERE BillDate between' " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("DueListRefDr", query, "Date upto " + DateTime.Now.ToString("yyyy-MM-dd"), "V_Due_Invoice_List", "Due List "+drName);
            dt.Show();

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string cond = "", drName = "";
            if (drIdTestBox.Text != "")
            {
                cond += " AND RefDrId=" + drIdTestBox.Text + "";
                drName = "(" + drNameTextBox.Text + ")";
            }

            string query = "SELECT * FROM V_Due_Collection_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("DueCollList", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Due_Collection_List", "Due Collection List " + drName);
            dt.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM V_Daily_Collection_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "'  AND CollAmt<>0";
            var dt = new LabReqViewer("DailyCollection", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Daily_Collection_List", "Daily Collection List");
            dt.Show();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (testIdTextBox.Text != "")
            {
                cond += " AND TestId=" + testIdTextBox.Text + "";
            }
            
            string query = "SELECT * FROM V_Counted_List_Of_Tested_Item WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' "+ cond +" ";
            var dt = new LabReqViewer("ItemCountList", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Counted_List_Of_Tested_Item", "Counted Item List");
            dt.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (drIdTestBox.Text != "")
            {
                cond += " AND UnderDrId=" + drIdTestBox.Text + "";
            }
            
            
            
            string query = "SELECT * FROM V_Bill_Register WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' "+ cond +" ";
            var dt = new LabReqViewer("BillRegister", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Bill_Register", "Bill Register");
            dt.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (drIdTestBox.Text!="")
            {
                cond += " AND UnderDrId=" + drIdTestBox.Text + "";
            }
            
            string query = "SELECT * FROM V_Bill_Register WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' "+ cond +" Order By SalesAmt";
            var dt = new LabReqViewer("TopContribution", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Bill_Register", "Doctor Contribution");
            dt.Show();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (drIdTestBox.Text != "")
            {
                cond += " AND RefDrId=" + drIdTestBox.Text + "";
            }

            string query = "SELECT * FROM V_Doctor_Honoriam WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' "+ cond +"";
            var dt = new LabReqViewer("DoctorComisionDtls", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Doctor_Honoriam", "Doctor Comision");
            dt.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {

            string cond = "";
            if (userNameComboBox.SelectedIndex != 0)
            {
                cond += " AND PostedBy='" + userNameComboBox.Text + "'";
            }
            
            
            string query = "SELECT * FROM V_Daily_Collection_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' "+ cond +"";
            var dt = new LabReqViewer("DailyCollectionUserWise", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Daily_Collection_List", "Daily Collection List");
            dt.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM DEL_RECORD_OF_BILL_DELETE WHERE Convert(Date,EntryDate) Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "'" + "AND AuthorizedBy  !='NULL'";//AAA
            var dt = new LabReqViewer("DeleteRecord", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "DEL_RECORD_OF_BILL_DELETE", "Daily Delete List");
            dt.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM V_Counted_List_Of_Tested_Item WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd")  + "'  ";
            var dt = new LabReqViewer("GroupCountList", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Counted_List_Of_Tested_Item", "Counted Item List (Groupwise)");
            dt.Show();
        }


        readonly DiagnosisBillUi _gBillUi=new DiagnosisBillUi();
        private void drIdTestBox_TextChanged(object sender, EventArgs e)
        {
            GridLoadDoctor();
        }

        private void GridLoadDoctor()
        {
            _gBillUi.HelpDataGridLoadByRefDr(dataGridView2, drIdTestBox.Text);
            dataGridView2.Columns[0].Width = 50;
            dataGridView2.Columns[1].Width = 360;
            dataGridView2.Columns[2].Visible = false;
            dataGridView2.Columns[7].Visible = false;
            dataGridView2.Columns[8].Visible = false;
        }

        private void drIdTestBox_Enter(object sender, EventArgs e)
        {
            GridLoadDoctor();

        }

        private void testIdTextBox_TextChanged(object sender, EventArgs e)
        {
            GridTestName();
        }

        private void GridTestName()
        {
            _gBillUi.HelpDataGridLoadByTest(dataGridView2, testIdTextBox.Text);
            dataGridView2.Columns[0].Width = 50;
            dataGridView2.Columns[1].Width = 260;
            dataGridView2.Columns[2].Width = 100;
            

        }

        private void testIdTextBox_Enter(object sender, EventArgs e)
        {
            GridTestName();
        }

        private string textInfo = "";
        private void drIdTestBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up )
            {
                textInfo = "Dr";
                dataGridView2.Focus();
            }
        }

        private void testIdTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                textInfo = "Test";
                dataGridView2.Focus();
            }
        }

        private void dataGridView2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string gccode = "";
                string gcdesc = "";
                gccode = dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex - 1].Cells[0].Value.ToString();
                gcdesc = dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex - 1].Cells[1].Value.ToString();

                switch (textInfo)
                {
                    case "Dr":
                        drIdTestBox.Text = gccode;
                        drNameTextBox.Text = gcdesc;
                        
                        break;
                    case "Test":
                        testIdTextBox.Text = gccode;
                        testNameTextBox.Text = gcdesc;
                        break;
                }

            }
        }

        private void button12_Click(object sender, EventArgs e)
        {

            string cond = "";
            if (userNameComboBox.SelectedIndex != 0)
            {
                cond += " AND PostedBy='" + userNameComboBox.Text + "'";
            }


            string query = "SELECT * FROM V_Daily_Collection_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("DailyCollectionUserWiseSummary", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Daily_Collection_List", "Daily Collection List");
            dt.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string cond = "",test="";
            if (testIdTextBox.Text != "")
            {
                cond += " AND TestId=" + testIdTextBox.Text + "";
            }
            if (drIdTestBox.Text != "")
            {
                cond += " AND RefDrId=" + drIdTestBox.Text + "";
            }
            if (testIdTextBox.Text=="")
            {
                test = "Test Wise Doctor(All)";
            }
            else
            {
                test = "Test Wise Doctor(" + testNameTextBox.Text + ")";
            }






            string query = "SELECT * FROM V_Test_Wise_Doctor WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("TestwiseDoctor", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Test_Wise_Doctor", test);
            dt.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string cond = "",drName="";
            if (drIdTestBox.Text != "")
            {
                cond += " AND RefDrId=" + drIdTestBox.Text + "";
                drName = "("+ drNameTextBox.Text +")";
            }
            
            string query = "SELECT * FROM V_Due_Collection_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' "+ cond +"";
            var dt = new LabReqViewer("DueCollList", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Due_Collection_List", "Due Collection List "+drName);
            dt.Show();
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            string cond = "", drName = "";
            if (drIdTestBox.Text != "")
            {
                cond += " AND ConsDrId=" + drIdTestBox.Text + "";
                drName = "(" + drNameTextBox.Text + ")";
            }
            else
            {
                drName = "";
            }

            //string query = "SELECT * FROM V_Due_Invoice_List WHERE 1=1 " + cond + "";
            string query = "SELECT * FROM V_Due_Invoice_List WHERE BillDate between' " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("DueListConsDr", query, "Date upto " + DateTime.Now.ToString("yyyy-MM-dd"), "V_Due_Invoice_List", "Due List " + drName);
            dt.Show();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (drIdTestBox.Text != "")
            {
                cond += " AND UnderDrId=" + drIdTestBox.Text + "";
            }

            string query = "SELECT * FROM V_Doctor_Honoriam_UnderDrwise WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("DoctorComisionDtls_UnderDr", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Doctor_Honoriam_UnderDrwise", "Doctor Comision Under Doctor");
            dt.Show();
        }

        private void button15_Click(object sender, EventArgs e)
        {
                string cond = "";
                if (drIdTestBox.Text != "")
                {
                    cond += " AND RefId=" + drIdTestBox.Text + "";
                }

                string query = "SELECT * FROM V_Digital_Vd WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
                var dt = new LabReqViewer("DigitalVd", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Digital_Vd", "Doctor Comision");
                dt.Show();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (drIdTestBox.Text != "")
            {
                cond += " AND DrOneId=" + drIdTestBox.Text + " AND DrOneAmt>0";
            }

            string query = "SELECT * FROM V_Digital_Vd WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("DigitalVdDrOne", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Digital_Vd", "Doctor Comision");
            dt.Show();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (drIdTestBox.Text != "")
            {
                cond += " AND DrTwoId=" + drIdTestBox.Text + " AND DrTwoAmt>0";
            }

            string query = "SELECT * FROM V_Digital_Vd WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("DigitalVdDrTwo", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Digital_Vd", "Doctor Comision");
            dt.Show();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (drIdTestBox.Text != "")
            {
                cond += " AND  DrThreeAmt>0";
            }

            string query = "SELECT * FROM V_Digital_Vd WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("DigitalVdDrThree", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Doctor_Honoriam", "Doctor Comision");
            dt.Show();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (mpoComboBox.SelectedIndex != 0)
            {
                cond += " AND MpoId=" + mpoComboBox.SelectedValue + "";
            }

            string query = "SELECT * FROM V_MpoContribution WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + " Order By SalesAmt";
            var dt = new LabReqViewer("mpoContribution", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_MpoContribution", "Mpo Contribution");
            dt.Show();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM V_SUMMARY WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "'";
            var dt = new LabReqViewer("DailyIncomeExpense", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_SUMMARY", "Daily Income Expense Summary");
            dt.Show();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (drIdTestBox.Text != "")
            {
                cond += " AND RefDrId=" + drIdTestBox.Text + "";
            }

            string query = "SELECT * FROM V_Hnr_Summary WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("HonouriamSummary", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Hnr_Summary", "Doctor Comision(Summary)");
            dt.Show();
        }

        private void DeptWiseSummarySort_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM V_SUMMARY WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "'";
            var dt = new LabReqViewer("DailyIncomeExpenseShort", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_SUMMARY", "Daily Income Expense Summary");
            dt.Show();
        }
    }
}
