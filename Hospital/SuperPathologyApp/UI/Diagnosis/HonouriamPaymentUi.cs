using System;

using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Gateway.Pharmacy;
using System.Collections.Generic;
using SuperPathologyApp.Model.Diagnosis;
using System.Data.SqlClient;


namespace SuperPathologyApp.UI.Pharmacy
{
    public partial class HonouriamPaymentUi : Form
    {
        public HonouriamPaymentUi()
        {
            InitializeComponent();

        }
       
        private void ClearText()
        {
           
          
        }


        public void Gridwidth(int no)
        {
            switch (no)
            {
                case 1:
                    dataGridView4.Columns[0].Width = 85;
                    dataGridView4.Columns[1].Width = 75;
                    dataGridView4.Columns[2].Width = 130;
                    dataGridView4.Columns[3].Width = 60;
                    break;

                case 2:
                    Hlp.GridFirstRowDeselect(helpDg);
                   
                    helpDg.Columns[0].Visible = false;
                    helpDg.Columns[1].Width = 80;
                    helpDg.Columns[2].Width = 70;
                    helpDg.Columns[3].Width = 140;
                    helpDg.Columns[4].Width = 150;

                    helpDg.Columns[5].Width = 70;
                    helpDg.Columns[6].Width = 150;
                    helpDg.Columns[7].Width = 70;
                    helpDg.Columns[8].Width = 150;


                    helpDg.Columns[9].Width = 65;
                    helpDg.Columns[10].Width = 65;
                    helpDg.Columns[11].Width = 65;
                    helpDg.Columns[12].Width = 65;
                    helpDg.Columns[13].Width = 65;
                    helpDg.Columns[14].Width = 65;
                    helpDg.RowHeadersVisible = false;
                    helpDg.CurrentCell = null;
                    break;


            }
            dataGridView4.RowHeadersVisible = false;
            dataGridView4.CurrentCell = null;

           
            dataGridView4.EnableHeadersVisualStyles = false;
            dataGridView4.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
          
            dataGridView4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView4.AllowUserToResizeRows = false;







        }
   
      
    
        private void GridWidth(DataGridView dataGrid)
        {

            dataGrid.Columns[0].Visible = false;
         
            dataGrid.ColumnHeadersVisible = false;
            dataGrid.CurrentCell = null;
        }

        readonly DbConnection db = new DbConnection();

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView4.CurrentRow != null)
            {
                string invNo = dataGridView4.CurrentRow.Cells[0].Value.ToString();
               // LabReportquery = "SELECT " + testNameForView + " AS TestName, * FROM VW_GET_LAB_REPORT_VIEW WHERE InvNo='" + invNo + "' AND InvDate='" + invDateDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND IsPrint=1   ";
            }
         //   var dt = new FrmReportViewer(_reportFileName,LabReportquery);
          //  dt.ShowDialog();
        }
    
        private void ReportPrintUi_Click(object sender, EventArgs e)
        {
            
        }

        private void ReportPrintUi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                saveAndPrintButton.PerformClick();
            }
        }

     
        PurchaseGateway _gt = new PurchaseGateway();
  
        private string textInfo = "";
    

        public void HelpDataGridLoadByTest(DataGridView dg, string search)
        {
            dg.DataSource = null;
            var _gt=new TestChartGateway();
            dg.DataSource = _gt.GetTestCodeList(0, search,0);
            Hlp.GridFirstRowDeselect(dg);
            dg.Columns.OfType<DataGridViewColumn>().ToList().ForEach(col => col.Visible = false);
           // Hlp.GridColor(dg);
            dg.Columns[0].Width = 100;
            dg.Columns[1].Width = 300;
            dg.Columns[2].Width = 150;
            dg.Columns[0].Visible = true;
            dg.Columns[1].Visible = true;
            dg.Columns[2].Visible = true;
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

       

     

       
  

        private void MedicinePurchaseUi_Load(object sender, EventArgs e)
        {
            reportTypeComboBox.SelectedIndex = 0;

        }

      

        private void LoadDataGridBySupplier()
        {
            panel2.Visible = true;

            textInfo = "1";
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name,Address FROM tb_DOCTOR WHERE  (convert(varchar,Id)+Name) LIKE '%" + drIdTextBox.Text + "%' Order by Id");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 320;
            dataGridView1.Columns[2].Width = 300;
        }
     


      
        private void receiptDateTimePicker_Enter(object sender, EventArgs e)
        {
            dtDateFromDateTimePicker.BackColor = Hlp.EnterFocus();

        }

        private void receiptDateTimePicker_Leave(object sender, EventArgs e)
        {
            dtDateFromDateTimePicker.BackColor = Hlp.LeaveFocus();

        }

        private void receiptDateTimePicker_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

              
            }
        }

      

        private void saveAndPrintButton_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            string cond = "";
            if (reportTypeComboBox.Text=="Paid")
            {
                cond += " AND IsPaidHnr=1";
            }
            else if (reportTypeComboBox.Text == "Due")
            {
                cond += " AND IsPaidHnr=0";
            }

            if (drIdTextBox.Text!="")
            {
                cond += " AND RefDrId="+ drIdTextBox.Text +"";
            }
            
            
            string query = " ";
            query = @"SELECT RefDrId,MasterId,BillNo,BillDate,PatientName,TestName,Charge,DueAmt AS InvDue,HnrAmt,DrLess,HnrToPay,IsPaidHnr,DrName,UnderDrId,UnderDrName FROM V_HNR_SUMMARY WHERE BillDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtdateTodateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";


            var list = new List<HnrPayModel>();
            _gt.ConLab.Open();
            var cmd = new SqlCommand(query, _gt.ConLab);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new HnrPayModel()
                {
                    RefDrId = Convert.ToInt32(rdr["RefDrId"]),
                    RefDrName = rdr["DrName"].ToString(),
                    UnderDrId = Convert.ToInt32(rdr["UnderDrId"]),
                    UnderDrName = rdr["UnderDrName"].ToString(),


                    MasterId = Convert.ToInt32(rdr["MasterId"]),

                    BillNo = rdr["BillNo"].ToString(),
                    BillDate = Convert.ToDateTime(rdr["BillDate"].ToString()),
                    PatientName = rdr["PatientName"].ToString(),
                    TestName = rdr["TestName"].ToString(),

                    Charge = Convert.ToDouble(rdr["Charge"]),
                    InvDue = Convert.ToDouble(rdr["InvDue"]),
                    HnrAmt = Convert.ToDouble(rdr["HnrAmt"]),
                    DrLess = Convert.ToDouble(rdr["DrLess"]),
                    HnrToPay = Convert.ToDouble(rdr["HnrToPay"]),
                    IsPaidHnr = Convert.ToInt32(rdr["IsPaidHnr"]),


                    
                });
            }
            rdr.Close();
            _gt.ConLab.Close();

            helpDg.Rows.Clear();

            foreach (var item in list)
            {
                helpDg.Rows.Add( item.MasterId, item.BillNo, item.BillDate.ToString("dd-MMM-yyyy"), item.PatientName, item.TestName,item.RefDrId,item.RefDrName,item.UnderDrId,item.UnderDrName,  item.Charge, item.InvDue, item.HnrAmt, item.DrLess, item.HnrToPay, item.IsPaidHnr);
            }
            Gridwidth(2);




            Hlp.GridColor(helpDg);
           

        }

       

        private void itemIdTextBox_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void helpDg_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void itemIdTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                helpDg.Focus();
            }
        }

        private void HonouriamPaymentUi_Load(object sender, EventArgs e)
        {
            reportTypeComboBox.SelectedIndex = 0;
            Gridwidth(2);
        }

        private void suppIdTextBox_TextChanged(object sender, EventArgs e)
        {
            DoctorGateway _gt = new DoctorGateway();
            panel2.Visible = true;
            dataGridView1.DataSource = _gt.GetDoctorList(0, drNameTextBox.Text);
            Gridwidth(1);
        }

        private void suppNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void drIdTextBox_TextChanged(object sender, EventArgs e)
        {
            LoadDataGridBySupplier();
            
      









        }

        private void drIdTextBox_Enter(object sender, EventArgs e)
        {
            LoadDataGridBySupplier();
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string gccode = "";
                string gcdesc = "";

                gccode = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[0].Value.ToString();
                gcdesc = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[1].Value.ToString();

                switch (textInfo)
                {
                    case "1":
                        drIdTextBox.Text = gccode;
                        drNameTextBox.Text = gcdesc;
                        panel2.Visible = false;
                        break;
                }

            }
        }

        private void drIdTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                textInfo  = "1";
                dataGridView1.Focus();
            }
        }

        private void helpDg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 14)
            {

                try
                {
                   // helpDg.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (helpDg.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null ? true : (!(bool)helpDg.Rows[e.RowIndex].Cells[e.ColumnIndex].Value));
                     bool isChecked = (bool)helpDg[e.ColumnIndex, e.RowIndex].EditedFormattedValue;
                     if (isChecked == false)
                     {
                         helpDg.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = true;
                         int masterId = (int)helpDg.CurrentRow.Cells[0].Value;
                         _gt.DeleteInsertLab("UPDATE tb_BILL_MASTER SET IsPaidHnr=1 WHERE Id=" + masterId + "");

                     }
                     else
                     {
                         helpDg.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = false;
                         int masterId = (int)helpDg.CurrentRow.Cells[0].Value;
                         _gt.DeleteInsertLab("UPDATE tb_BILL_MASTER SET IsPaidHnr=0 WHERE Id=" + masterId + "");
                     
                     }

                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
                
                
            }
        }
  
    }
}
