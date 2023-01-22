using System;

using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Gateway.Pharmacy;


namespace SuperPathologyApp.UI.Pharmacy
{
    public partial class StockRelatedReportUi : Form
    {
        public StockRelatedReportUi()
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
           
        }
     


      
        private void receiptDateTimePicker_Enter(object sender, EventArgs e)
        {
            receiptDateTimePicker.BackColor = Hlp.EnterFocus();

        }

        private void receiptDateTimePicker_Leave(object sender, EventArgs e)
        {
            receiptDateTimePicker.BackColor = Hlp.LeaveFocus();

        }

        private void receiptDateTimePicker_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

              
            }
        }

      

        private void saveAndPrintButton_Click(object sender, EventArgs e)
        {

            
            
            string cond = "";
            string query = "";
            switch (reportTypeComboBox.Text)
            {
                case "CurrentStock":
                        if (itemIdTextBox.Text != "")
                        {
                            cond += " WHERE a.ItemId='" + itemIdTextBox.Text + "'";
                        }
                        query = @"Select a.ItemId,b.Name AS ItemName,b.SuppId, c.Name AS SuppName,  AVG(a.PurchasePrice) AS PurchasePrice,SUM(a.InQty-a.OutQty) AS BalQty FROM tb_ph_STOCK_LEDGER a INNER JOIN  tb_ph_ITEM  b  ON a.ItemId=b.Id  INNER JOIN tb_ph_SUPPLIER c ON b.SuppId=c.Id "+ cond +" GROUP BY  a.ItemId,b.Name,b.SuppId, c.Name"; 
                        helpDg.DataSource = null;
                        helpDg.DataSource = Hlp.LoadDbByQuery(0, query);
                        Hlp.GridFirstRowDeselect(helpDg);
                        helpDg.Columns[0].Width = 70;
                        helpDg.Columns[1].Width = 250;
                        helpDg.Columns[2].Width = 70;
                        helpDg.Columns[3].Width = 180;
                        helpDg.Columns[4].Width = 80;
                        helpDg.Columns[5].Width = 80;
                    break;
                case "CurrentDue":


                    if (suppIdTextBox.Text != "")
                    {
                        cond += " WHERE SuppId='" + suppIdTextBox.Text + "'";
                    }
                    query = @"Select * FROM V_ph_Supplier_Due_List " + cond + "";
                    helpDg.DataSource = null;
                    helpDg.DataSource = Hlp.LoadDbByQuery(0, query);
                    Hlp.GridFirstRowDeselect(helpDg);
                    helpDg.Columns[0].Width = 70;
                    helpDg.Columns[1].Width = 250;
                    helpDg.Columns[2].Width = 180;
                    helpDg.Columns[3].Width = 80;
                    helpDg.Columns[4].Width = 80;
                    helpDg.Columns[5].Width = 80;
                    break;












            }
            



        }

       

        private void itemIdTextBox_TextChanged(object sender, EventArgs e)
        {
            textInfo = "item";
            helpDg.DataSource = Hlp.LoadDbByQuery(0, "SELECT a.Id,a.Name,c.Name AS GroupName, a.GenericName,b.Name AS SuppName FROM tb_ph_ITEM a INNER JOIN tb_ph_SUPPLIER b ON a.SuppId=b.Id INNER JOIN tb_ph_ITEM_GROUP  c ON a.GroupId=c.Id  WHERE 1=1 AND (a.Name+a.GenericName+b.Name) LIKE '%" + itemIdTextBox.Text + "%' ");
            Hlp.GridFirstRowDeselect(helpDg);
            helpDg.Columns[0].Width = 60;
            helpDg.Columns[1].Width = 240;
            helpDg.Columns[2].Width = 100;
            helpDg.Columns[3].Width = 130;
            helpDg.Columns[4].Width = 200;
        }

        private void helpDg_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string gccode = "";
                string gcdesc = "";
                gccode = helpDg.Rows[helpDg.CurrentCell.RowIndex - 1].Cells[0].Value.ToString();
                gcdesc = helpDg.Rows[helpDg.CurrentCell.RowIndex - 1].Cells[1].Value.ToString();

                switch (textInfo)
                {
                   
                    case "item":
                        itemIdTextBox.Text = gccode;
                        itemNameTextBox.Text = gcdesc;
                        break;

                }

            }
        }

        private void itemIdTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                helpDg.Focus();
            }
        }
  
    }
}
