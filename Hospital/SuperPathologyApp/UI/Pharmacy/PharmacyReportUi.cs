using System;
using System.Windows.Forms;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Report;

namespace SuperPathologyApp.UI.Diagnosis
{
    public partial class PharmacyReportUi : Form
    {
        public PharmacyReportUi()
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
           


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (suppIdTestBox.Text != "")
            {
                cond += " AND SuppId=" + suppIdTestBox.Text + "";
            }

            string query = "SELECT * FROM V_ph_Supplier_Due_List WHERE 1=1 "+ cond +"";
            var dt = new PharmacyReportViewer("ph_Supplier_due", query, "Date upto " + DateTime.Now.ToString("yyyy-MM-dd"), "V_ph_Supplier_Due_List", "Supplier Due List");
            dt.Show();

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string cond = "", drName = "";
            if (suppIdTestBox.Text != "")
            {
                cond += " AND RefDrId=" + suppIdTestBox.Text + "";
                drName = "(" + drNameTextBox.Text + ")";
            }

            string query = "SELECT * FROM V_Due_Collection_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("DueCollList", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Due_Collection_List", "Due Collection List " + drName);
            dt.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM V_Daily_Collection_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "'";
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

            string query = "SELECT * FROM V_in_Counted_List_Of_Tested_Item WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + " ";
            var dt = new LabReqViewer("in_ItemCountList", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_in_Counted_List_Of_Tested_Item", "Counted Item List(Indoor)");
            dt.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string cond = "";
            //if (suppIdTestBox.Text != "")
            //{
            //    cond += " AND UnderDrId=" + suppIdTestBox.Text + "";
            //}



            string query = "SELECT * FROM V_ph_Gross_Profit WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + " ";
            var dt = new PharmacyReportViewer("ph_gross_profit", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_ph_Gross_Profit", "Gross Profit");
            dt.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string cond = "";
            //if (suppIdTestBox.Text!="")
            //{
            //    cond += " AND UnderDrId=" + suppIdTestBox.Text + "";
            //}

            string query = "SELECT * FROM V_ph_Daily_Collection_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + " ";
            var dt = new PharmacyReportViewer("ph_daily_collection", query, "Daily Collection", "V_ph_Daily_Collection_List", "");
            dt.Show();

        }

        private void button7_Click(object sender, EventArgs e)
        {


            string query = "SELECT * FROM V_ph_Due_Invoice_List";
            var dt = new PharmacyReportViewer("ph_Current_Due", query, "Date ad " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_ph_Due_Invoice_List", "Current Due(Pharmacy)");
            dt.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {

            string cond = "";
            if (userNameComboBox.SelectedIndex != 0)
            {
                cond += " AND PostedBy='" + userNameComboBox.Text + "'";
            }


            string query = "SELECT * FROM V_ph_Purchase_List WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new PharmacyReportViewer("ph_purchaseDetails", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_ph_Purchase_List", "Daily Purchase List");
            dt.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM DEL_RECORD_OF_BILL_DELETE WHERE Convert(Date,EntryDate) Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "'";
            var dt = new LabReqViewer("DeleteRecord", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "DEL_RECORD_OF_BILL_DELETE", "Daily Delete List");
            dt.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (testIdTextBox.Text != "")
            {
                cond += " AND TestId=" + testIdTextBox.Text + "";
            }

            string query = "SELECT * FROM V_in_Counted_List_Of_Tested_Item WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + " ";
            var dt = new IndoorReportViewer("in_ItemCountList", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_in_Counted_List_Of_Tested_Item", "Counted Item List(Indoor)");
            dt.Show();
        }


        readonly DiagnosisBillUi _gBillUi=new DiagnosisBillUi();
        private void drIdTestBox_TextChanged(object sender, EventArgs e)
        {
            GridLoadDoctor();
        }

        private void GridLoadDoctor()
        {

            dataGridView2.DataSource = null;
            dataGridView2.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name FROM tb_ph_Supplier", suppIdTestBox.Text, "(convert(varchar,Id)+Name)");
            Hlp.GridFirstRowDeselect(dataGridView2);
            dataGridView2.Columns[0].Width = 70;
            dataGridView2.Columns[1].Width = 360;

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
                        suppIdTestBox.Text = gccode;
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
            if (suppIdTestBox.Text != "")
            {
                cond += " AND SuppId='" + suppIdTestBox.Text + "'";
            }

            string query = "SELECT * FROM V_daily_purchase_summary WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new PharmacyReportViewer("ph_daily_purchase_list", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_daily_purchase_summary", "Daily Purchase List");
            dt.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string cond = "",test="";
            if (testIdTextBox.Text != "")
            {
                cond += " AND TestId=" + testIdTextBox.Text + "";
            }
            if (suppIdTestBox.Text != "")
            {
                cond += " AND RefDrId=" + suppIdTestBox.Text + "";
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
            if (suppIdTestBox.Text != "")
            {
                cond += " AND RefDrId=" + suppIdTestBox.Text + "";
                drName = "("+ drNameTextBox.Text +")";
            }
            
            string query = "SELECT * FROM V_Due_Collection_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' "+ cond +"";
            var dt = new LabReqViewer("DueCollList", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Due_Collection_List", "Due Collection List "+drName);
            dt.Show();
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            string cond = "", drName = "";
            if (suppIdTestBox.Text != "")
            {
                cond += " AND SuppId=" + suppIdTestBox.Text + "";
                //drName = "(" + drNameTextBox.Text + ")";
            }
            else
            {
                drName = "";
            }

            string query = "SELECT * FROM V_ph_Daily_Payment_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new PharmacyReportViewer("ph_supplier_payment_list", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_ph_Daily_Payment_List", "Due Payment List");
            dt.Show();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (suppIdTestBox.Text != "")
            {
                cond += " AND UnderDrId=" + suppIdTestBox.Text + "";
            }

            string query = "SELECT * FROM V_Doctor_Honoriam_UnderDrwise WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("DoctorComisionDtls_UnderDr", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Doctor_Honoriam_UnderDrwise", "Doctor Comision Under Doctor");
            dt.Show();
        }

        private void button15_Click(object sender, EventArgs e)
        {
                string cond = "";
                if (suppIdTestBox.Text != "")
                {
                    cond += " AND RefId=" + suppIdTestBox.Text + "";
                }

                string query = "SELECT * FROM V_Digital_Vd WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
                var dt = new LabReqViewer("DigitalVd", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Digital_Vd", "Doctor Comision");
                dt.Show();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (suppIdTestBox.Text != "")
            {
                cond += " AND DrOneId=" + suppIdTestBox.Text + " AND DrOneAmt>0";
            }

            string query = "SELECT * FROM V_Digital_Vd WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("DigitalVdDrOne", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Digital_Vd", "Doctor Comision");
            dt.Show();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (suppIdTestBox.Text != "")
            {
                cond += " AND DrTwoId=" + suppIdTestBox.Text + " AND DrTwoAmt>0";
            }

            string query = "SELECT * FROM V_Digital_Vd WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("DigitalVdDrTwo", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Digital_Vd", "Doctor Comision");
            dt.Show();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (suppIdTestBox.Text != "")
            {
                cond += " AND  DrThreeAmt>0";
            }

            string query = "SELECT * FROM V_Digital_Vd WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("DigitalVdDrThree", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Doctor_Honoriam", "Doctor Comision");
            dt.Show();
        }

        private void IndoorReportUi_Load(object sender, EventArgs e)
        {
            _gt.LoadComboBox("SELECT Distinct 0 AS Id, UserName AS Description FROM tb_USER_PRIVILEGE  ", userNameComboBox);
            // imagingStatusComboBox.SelectedIndex = 0;

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string cond = "";
            //if (suppIdTestBox.Text!="")
            //{
            //    cond += " AND UnderDrId=" + suppIdTestBox.Text + "";
            //}

            string query = "SELECT * FROM V_ph_Daily_Collection_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + " ";
            var dt = new PharmacyReportViewer("ph_daily_collection_userwise", query, "Daily Collection", "V_ph_Daily_Collection_List", "");
            dt.Show();
        }

        private void PharmacyReportUi_Load(object sender, EventArgs e)
        {
            _gt.LoadComboBox("SELECT Distinct 0 AS Id, UserName AS Description FROM tb_USER_PRIVILEGE  ", userNameComboBox);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (suppIdTestBox.Text != "")
            {
                cond += " AND SuppID=" + suppIdTestBox.Text + "";
            }

            string query = "SELECT * FROM V_ph_Curr_Stock WHERE 1=1 " + cond + " ";
            var dt = new PharmacyReportViewer("ph_current_stock", query, "Current Stock as On:"+dtDateToDateTimePicker.Value.ToString("dd-MMM-yyyy"), "V_ph_Curr_Stock", "");
            dt.Show();

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            string cond = "";
            if (suppIdTestBox.Text != "")
            {
                cond += " AND  SuppId='"+ suppIdTestBox.Text +"'";
            }

            string query = "SELECT * FROM V_ph_Purchase_Return_BIll WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new PharmacyReportViewer("ph_purchase_return_bill_all", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_ph_Purchase_Return_BIll", "Supplier Return List");
            dt.Show();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            string cond = "";


            string query = "SELECT * FROM V_ph_indoor_return WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + " ";
            var dt = new PharmacyReportViewer("in_ph_return", query, "Daily Return", "V_ph_indoor_return", "");
            dt.Show();
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            string cond = "";


            string query = "SELECT * FROM V_ph_indoor_return WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + " ";
            var dt = new PharmacyReportViewer("in_ph_return_userwise", query, "Daily Return", "V_ph_indoor_return", "");
            dt.Show();
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            string cond = "";
            //if (suppIdTestBox.Text != "")
            //{
            //    cond += " AND UnderDrId=" + suppIdTestBox.Text + "";
            //}



            string query = "SELECT * FROM V_ph_Gross_Profit WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + " ";
            var dt = new PharmacyReportViewer("ph_gross_profitS", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_ph_Gross_Profit", "Gross Profit");
            dt.Show();
        }
    }
}
