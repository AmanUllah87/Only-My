using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.VisualBasic;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Gateway.Pharmacy;
using SuperPathologyApp.Model.Pharmacy;
using SuperPathologyApp.Report;
using SuperPathologyApp.Report.DataSet;

namespace SuperPathologyApp.UI.Reagent
{
    public partial class ReagentPurchaseUi : Form
    {
        public ReagentPurchaseUi()
        {
            InitializeComponent();

        }
       
        private void ClearText()
        {
            Hlp.GridColor(dataGridView1);
            billDateTextBox.Text = Hlp.GetServerDate().ToString("dd-MMM-yyyy");
           
          
            billNoTextBox.Text = _gt.GetInvoiceNo(9);
           
            receiptNoTextBox.Text = "";
          
            dataGridView1.Rows.Clear();
           
            totalItemTextBox.Text = "0";
            totalAmountTextBox.Text = "0";
           
            netPayableTextBox.Text = "0";
            paidAmtTextBox.Text = "0";
            lessAmtTextBox.Text = "0";
            remarksTextBox.Text = "";
            suppNameTextBox.Text = "";
            suppIdTextBox.Text = "0";
            saveAndPrintButton.Text = "Save && &Print";
         
          
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
                case 6:
                    dataGridView1.RowHeadersVisible = false;
                    dataGridView1.CurrentCell = null;
                    dataGridView1.EnableHeadersVisualStyles = false;
                    dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                    dataGridView1.AllowUserToResizeRows = false;

                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Width = 200;
                    dataGridView1.Columns[2].Width = 345;

                    break;


            }
            dataGridView4.RowHeadersVisible = false;
            dataGridView4.CurrentCell = null;

           
            dataGridView4.EnableHeadersVisualStyles = false;
            dataGridView4.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
          

            dataGridView4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView4.AllowUserToResizeRows = false;







        }
   
      
    
        private void GridWidth(DataGridView dataGrid)
        {

            dataGrid.Columns[0].Visible = false;
            dataGrid.Columns[1].Width = itemNameTextBox.Width - 2;
            dataGrid.Columns[2].Width = unitTextBox.Width - 1;
            dataGrid.Columns[3].Width = QtyTextBox.Width-1;
          
            dataGrid.Columns[4].Width = unitPriceTextBox.Width-1;
            dataGrid.Columns[5].Width = unitTotalPriceTextBox.Width - 1;
            dataGrid.Columns[6].Width = lotNoTextBox.Width - 1;
            dataGrid.Columns[7].Width = expireDateTimePicker.Width-2;

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

        private void remarksTextBox_Enter(object sender, EventArgs e)
        {
            remarksTextBox.BackColor = Hlp.EnterFocus();

        }
        private void remarksTextBox_Leave(object sender, EventArgs e)
        {
            remarksTextBox.BackColor = Hlp.LeaveFocus();
        }
        ReagentPurchaseGateway _gt = new ReagentPurchaseGateway();
  
        private string textInfo = "";
    
        private void dataGridView2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string gccode = "";
                string gcdesc = "";
                gccode = helpDg.Rows[helpDg.CurrentCell.RowIndex - 1].Cells[0].Value.ToString();
                gcdesc = helpDg.Rows[helpDg.CurrentCell.RowIndex - 1].Cells[1].Value.ToString();

                switch (textInfo)
                {
                    case "supp":
                        suppIdTextBox.Text = gccode;
                        suppNameTextBox.Text = gcdesc;
                        itemNameTextBox.Focus();
                        break;
                    case "item":
                        itemIdTextBox.Text = gccode;
                        itemNameTextBox.Text = gcdesc;
                        unitPriceTextBox.Text = _gt.FncReturnFielValueLab("tb_REAGENT", "Id='" + gccode + "'", "LastPPrice");
                        QtyTextBox.Focus();
                        break;

                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

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

        private void AddDataToGrid()
        {
            if (_gt.FnSeekRecordNewLab("tb_REAGENT", "Id='" + itemIdTextBox.Text + "'") == false)
            {
                MessageBox.Show(@"Invalid Name", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_gt.IsDuplicate(itemIdTextBox.Text, dataGridView1) == false)
            {
               
                dataGridView1.Rows.Add(itemIdTextBox.Text, itemNameTextBox.Text,unitTextBox.Text,  QtyTextBox.Text, unitPriceTextBox.Text,unitTotalPriceTextBox.Text,lotNoTextBox.Text,expireDateTimePicker.Value.ToString("yyyy-MM-dd"));
                itemIdTextBox.Text = "";
                dataGridView1.CurrentCell.Selected = false;
                helpPanel.Visible = false;
                ClearGridText();
            }
            else
            {
                MessageBox.Show(@"Duplicate Name Found", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                itemIdTextBox.Text = "0";
                itemNameTextBox.Focus();
                return;
            }
        }

        private void ClearGridText()
        {
            itemNameTextBox.Text = "";
            QtyTextBox.Text = "";
            unitPriceTextBox.Text = "";
            lotNoTextBox.Text = "";
            unitTotalPriceTextBox.Text = "";
         
            itemNameTextBox.Focus();

            totalItemTextBox.Text = dataGridView1.Rows.Count.ToString();

            double totAmt = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                totAmt += Convert.ToDouble(dataGridView1.Rows[i].Cells[5].Value);
              
               
            }
            totalAmountTextBox.Text = Math.Round(totAmt, 2).ToString();
           
            netPayableTextBox.Text =Math.Round(totAmt,2).ToString();
        }
      

        private void saveAndPrintButton_Click_1(object sender, EventArgs e)
        {
            if (CheckSuccess())
            {
                var aMdl = new PurchaseModel
                {
                    BillNo=billNoTextBox.Text,
                    ReceiptNo = receiptNoTextBox.Text,
                    ReceiptDate = receiptDateTimePicker.Value,
                    Supplier = new SupplierModel() { Id =Convert.ToInt32(suppIdTextBox.Text)},
                    TotalItem =Convert.ToDouble(Hlp.StringToDouble(totalItemTextBox.Text)),
                    TotAmount = Convert.ToDouble(Hlp.StringToDouble(totalAmountTextBox.Text)),
                    NetAmount= Convert.ToDouble(Hlp.StringToDouble(netPayableTextBox.Text)),
                  
                    TotalLess= Convert.ToDouble(Hlp.StringToDouble(lessAmtTextBox.Text)),
                    TotalPaid= Convert.ToDouble(Hlp.StringToDouble(paidAmtTextBox.Text)),
                    Remarks = remarksTextBox.Text,
                };

                var mdl = new List<ItemModel>();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    mdl.Add(new ItemModel()
                    {
                        Id = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value.ToString()),
                        Qty= Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[3].Value.ToString())),
                        PurchasePrice = Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[4].Value.ToString())),
                        UnitTotal= Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[5].Value.ToString())),
                        LotNo = dataGridView1.Rows[i].Cells[6].Value.ToString(),
                        ExpireDate= Convert.ToDateTime(dataGridView1.Rows[i].Cells[7].Value.ToString()),
                    });
                }
                aMdl.ItemModels = mdl;

                string msg=_gt.Save(aMdl,saveAndPrintButton.Text);
                if (msg == _gt.SaveSuccessMessage)
                {
                    MessageBox.Show(_gt.SaveSuccessMessage, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearText();
                    //_gt.DeleteInsertLab("Update tb_BILL_MASTER SET  LastPrintPc='" + Environment.MachineName + "' WHERE Id='" + aMdl.BillId + "'");
                    //PrintInvoice(aMdl.BillId, "''");
                    receiptNoTextBox.Focus();
                }
                else
                {
                    MessageBox.Show(msg, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

      

        private bool CheckSuccess()
        {
            bool isChecked = true;

            if (_gt.FnSeekRecordNewLab("tb_ph_SUPPLIER", "Id='" + suppIdTextBox.Text + "'") == false)
            {
                MessageBox.Show(@"Invalid Supplier!!!Please Check.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                suppNameTextBox.Focus();
                isChecked = false;
            }
            if (dataGridView1.Rows.Count==0)
            {
                MessageBox.Show(@"No Item For Save ", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                itemNameTextBox.Focus();
                isChecked = false;
            }



            return isChecked;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        private void searchTextBox_Enter(object sender, EventArgs e)
        {
            searchTextBox.BackColor = Hlp.EnterFocus();

            textInfo = "enter";
            helpDg.DataSource = null;
            helpDg.DataSource = _gt.GetInvoiceList(Convert.ToDateTime(billDateTextBox.Text), searchTextBox.Text, "enter");

            if (helpDg.Rows.Count > 0)
            {
                helpDg.Columns[0].Width = 80;
                helpDg.Columns[1].Width = 80;
                helpDg.Columns[2].Width = 80;

                helpDg.Columns[3].Width = 280;
                helpDg.Columns[4].Width = 100;
                helpDg.Columns[5].Width = 100;

                //helpPanel.Location = new Point(12, 169);
                helpPanel.Visible = true;
                Hlp.GridFirstRowDeselect(helpDg);
            }
        }

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (helpDg.CurrentCell.Selected)
            {
                if (helpDg.CurrentRow != null)
                {
                    string invNo = helpDg.CurrentRow.Cells[0].Value.ToString();
                    int billId =Convert.ToInt32(_gt.FncReturnFielValueLab("tb_BILL_MASTER","BillNo='"+ invNo +"'","Id"));
                    _gt.DeleteInsertLab("Update tb_BILL_MASTER SET  LastPrintPc='" + Environment.MachineName + "' WHERE Id='" + billId + "'");
                    PrintInvoice(billId,"''");
                }
            }
        }

        readonly ReportDocument _rprt=new ReportDocument();
        private void AutoPrintInvoice(string reportFileName, string query, string dataSetName, string title)
        {
            try
            {
                string comName = _gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ComName");
                string comAddress = _gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "Address");

                _gt.ConLab.Open();
                string path = Application.StartupPath;
                path = path + @"\Report\File\Diagnosis\";
               


                _rprt.Load(path + "" + reportFileName + ".rpt");
                //  _rprt.Load(path + "" + _reportFileName + ".rdlc");

                var cmd = new SqlCommand(query, _gt.ConLab);
                var sda = new SqlDataAdapter(cmd);
                var ds = new GroupReportDS();
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

        private void PrintInvoice(int invNo,string groupName)
        {
            string query = "EXEC Sp_Get_InvoicePrint " + invNo + "," + groupName + "";
            var dt = new LabReqViewer("Bill", query, "Invoice", "Sp_Get_InvoicePrint", "");
            dt.Show();
            
            helpPanel.Visible = false;
        }

        private void paidAmtTextBox_Enter(object sender, EventArgs e)
        {
            paidAmtTextBox.BackColor = Hlp.EnterFocus();
            helpPanel.Visible = false;
        }

        private void paidAmtTextBox_Leave(object sender, EventArgs e)
        {
            paidAmtTextBox.BackColor = Hlp.LeaveFocus();
        }

      

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
            }
        }

        private void billNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='" + Hlp.UserName + "' AND  ParentName='Pharmacy' AND PermisionName='Purchase-Edit' "))
                {
                    if (_gt.FnSeekRecordNewLab("tb_ph_PURCHASE_MASTER", "BillNo='" + billNoTextBox.Text + "' "))
                    {
                        GetInvoiceDataForEdit(billNoTextBox.Text);
                     
                        saveAndPrintButton.Text = "&Update";
                    }
                }
                else
                {
                    MessageBox.Show(@"You need permission to do this task.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }


           
            
            
        }

        private void GetInvoiceDataForEdit(string invNo)
        {
            try
            {
                var aMdl = _gt.GetInvoiceDataForEdit(invNo);


                receiptNoTextBox.Text = aMdl.ReceiptNo;
                receiptDateTimePicker.Value = aMdl.ReceiptDate;

                suppIdTextBox.Text = aMdl.Supplier.Id.ToString();
                suppNameTextBox.Text = _gt.FncReturnFielValueLab("tb_ph_Supplier","Id='"+ suppIdTextBox.Text +"'","Name");

                totalItemTextBox.Text = aMdl.TotalItem.ToString();
                totalAmountTextBox.Text = aMdl.TotAmount.ToString();
               
                lessAmtTextBox.Text = aMdl.TotalLess.ToString();
                paidAmtTextBox.Text = aMdl.TotalPaid.ToString();
                netPayableTextBox.Text = aMdl.NetAmount.ToString();
                
                
                remarksTextBox.Text = aMdl.Remarks;


                dataGridView1.Rows.Clear();
                foreach (var item in aMdl.ItemModels)
                {
                   
                    
                      //Id = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value.ToString()),
                      //  Qty= Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[2].Value.ToString())),
                      //  PurchasePrice = Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[3].Value.ToString())),
                      //  BQty = Convert.ToDouble(Hlp.IsNumeric(dataGridView1.Rows[i].Cells[4].Value.ToString())?dataGridView1.Rows[i].Cells[4].Value.ToString():"0"), 
                      //  UnitTotal= Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[5].Value.ToString())),
                      //  VatPc = Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[6].Value.ToString())),
                      //  Vat = Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[7].Value.ToString())),
                      //  TaxPc = Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[8].Value.ToString())),
                      //  Tax= Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[9].Value.ToString())),
                      //  Tp = Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[10].Value.ToString())),
                      //  ExpireDate= Convert.ToDateTime(dataGridView1.Rows[i].Cells[11].Value.ToString()),
                    dataGridView1.Rows.Add(item.Id, item.Name, item.Qty, item.PurchasePrice, item.BQty, item.UnitTotal, item.VatPc, item.Vat,item.TaxPc,item.Tax,item.Tp,item.ExpireDate);
                }
                dataGridView1.CurrentCell.Selected = false;





                
               
             



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

      
  

        

     

     

      

     

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            helpDg.DataSource = _gt.GetInvoiceList(DateTime.Now, searchTextBox.Text, "change");
        
            if (helpDg.Rows.Count > 0)
            {
                helpDg.Columns[0].Width = 80;
                helpDg.Columns[1].Width = 80;
                helpDg.Columns[2].Width = 80;

                helpDg.Columns[3].Width = 280;
                helpDg.Columns[4].Width = 100;
                helpDg.Columns[5].Width = 100;

              //  helpPanel.Location = new Point(12, 169);
                helpPanel.Visible = true;
                Hlp.GridFirstRowDeselect(helpDg);
            }
        }


        private void paidAmtTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                remarksTextBox.Focus();
            }
        }


        private void remarksTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                saveAndPrintButton.Focus();
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

 



  
      

       



   

        private void saveAndPrintButton_Enter(object sender, EventArgs e)
        {
            saveAndPrintButton.BackColor = Color.Coral;
        }



        private void saveAndPrintButton_Leave(object sender, EventArgs e)
        {
            saveAndPrintButton.BackColor = Color.Green;
        }

        private void searchTextBox_Leave(object sender, EventArgs e)
        {
            searchTextBox.BackColor = Hlp.LeaveFocus();

        }

  

        private void MedicinePurchaseUi_Load(object sender, EventArgs e)
        {
            

        }

        private void suppIdTextBox_Enter(object sender, EventArgs e)
        {
            suppIdTextBox.BackColor = Hlp.EnterFocus();

        }

        private void suppIdTextBox_Leave(object sender, EventArgs e)
        {
            suppIdTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void itemNameTextBox_Enter(object sender, EventArgs e)
        {
            itemNameTextBox.BackColor = Hlp.EnterFocus();
            textInfo = "item";
            helpPanel.Visible = false;
            LoadDataGridByItem();

        }

        private void suppNameTextBox_Enter(object sender, EventArgs e)
        {
            suppNameTextBox.BackColor = Hlp.EnterFocus();
            textInfo = "supp";
            helpPanel.Visible = true;
            LoadDataGridBySupplier();

        }

        private void LoadDataGridBySupplier()
        {
            helpDg.DataSource = null;
            helpDg.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name,Address FROM tb_REAGENT_SUPPLIER", suppNameTextBox.Text, "(convert(varchar,Id)+Name) ");
            Hlp.GridFirstRowDeselect(helpDg);
            helpDg.Columns[0].Width = 100;
            helpDg.Columns[1].Width = 350;
            helpDg.Columns[2].Width = 270;
        }
        private void LoadDataGridByItem()
        {
            
           

            helpDg.DataSource = null;
            helpDg.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name,Type, DeptName,LastPrice FROM tb_REAGENT  WHERE 1=1 AND (Name+DeptName) LIKE '%" + itemNameTextBox.Text + "%' ");
            Hlp.GridFirstRowDeselect(helpDg);
            helpDg.Columns[0].Width = 60;
            helpDg.Columns[1].Width = 240;
            helpDg.Columns[2].Width = 100;
            helpDg.Columns[3].Width = 130;
            helpDg.Columns[4].Width = 100;


        }



        private void suppNameTextBox_Leave(object sender, EventArgs e)
        {
            suppNameTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void suppNameTextBox_TextChanged(object sender, EventArgs e)
        {
            LoadDataGridBySupplier();
        }

        private void suppNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Down||e.KeyCode==Keys.Up)
            {
                helpDg.Focus();
                helpDg.Rows[0].Cells[0].Selected = true;
            }
        }

        private void itemNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode==Keys.Enter)
            {
                if (itemNameTextBox.Text.Length > 0)
                {
                    if (helpDg.Rows.Count > 0)
                    {

                        if (helpDg.Rows[0].Cells[0].Value == null)
                        {
                            return;
                        }

                        itemNameTextBox.Text = helpDg.Rows[0].Cells[1].Value.ToString();
                        itemIdTextBox.Text = helpDg.Rows[0].Cells[0].Value.ToString();
                        unitPriceTextBox.Text = _gt.FncReturnFielValueLab("tb_REAGENT", "Id='" + itemIdTextBox.Text + "'", "LastPPrice");

                        QtyTextBox.Focus();

                    }
                }
                else
                {
                    if (dataGridView1.Rows.Count>0)
                    {
                        lessAmtTextBox.Focus();
                    }
                }


            }
            if (e.KeyCode==Keys.Down||e.KeyCode==Keys.Up)
            {
                helpPanel.Visible = true;
                if (helpDg.Rows.Count>0)
                {
                    helpDg.Rows[0].Cells[1].Selected = true;
                    helpDg.Focus();

                }
            }


            
            
        
        }

        private void expireDateTimePicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (_gt.FnSeekRecordNewLab("tb_REAGENT", "Id='" + itemIdTextBox.Text + "'"))
                {
                    AddDataToGrid();
                    return;
                }
                
            }
            
            
        }

        private void QtyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) )
            {
                e.Handled = true;
            }

          
        }

        private void unitPriceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void bonusQtyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void vatTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void commisionTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void QtyTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (QtyTextBox.Text == "" || QtyTextBox.Text == "0")
                {
                    return;
                }
                else { unitPriceTextBox.Focus(); }
            }
        }

        private void unitPriceTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double unitPrice = Convert.ToDouble(QtyTextBox.Text) * Convert.ToDouble(unitPriceTextBox.Text);
                unitTotalPriceTextBox.Text = Math.Round(unitPrice, 2).ToString();


                unitTotalPriceTextBox.Focus();
            }
        }

      

      
    
       

      
        private void receiptNoTextBox_Enter(object sender, EventArgs e)
        {
            receiptNoTextBox.BackColor= Hlp.EnterFocus();
            helpPanel.Visible = false;
        }

        private void receiptNoTextBox_Leave(object sender, EventArgs e)
        {
            receiptNoTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void receiptNoTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                receiptDateTimePicker.Focus();

            }
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

                suppNameTextBox.Focus();
            }
        }

        private void itemNameTextBox_TextChanged(object sender, EventArgs e)
        {
            LoadDataGridByItem();
            helpPanel.Visible = true;
        }

        private void itemNameTextBox_Leave(object sender, EventArgs e)
        {
            itemNameTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void QtyTextBox_Enter(object sender, EventArgs e)
        {
            QtyTextBox.BackColor = Hlp.EnterFocus();
        }

        private void QtyTextBox_Leave(object sender, EventArgs e)
        {
            QtyTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void unitPriceTextBox_Enter(object sender, EventArgs e)
        {
            unitPriceTextBox.BackColor = Hlp.EnterFocus();
            unitPriceTextBox.SelectAll();

        }

        private void unitPriceTextBox_Leave(object sender, EventArgs e)
        {
            unitPriceTextBox.BackColor = Hlp.LeaveFocus();
        }

       

       

      
        private void unitTotalPriceTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void unitTotalPriceTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
               
                unitPriceTextBox.Text =Math.Round((Convert.ToDouble(Hlp.IsNumeric(unitTotalPriceTextBox.Text)?unitTotalPriceTextBox.Text:"0") / Convert.ToDouble(QtyTextBox.Text)),2).ToString();
                lotNoTextBox.Focus();
            }
        }

        private void lessAmtTextBox_Enter(object sender, EventArgs e)
        {
            lessAmtTextBox.BackColor = Hlp.EnterFocus();
            helpPanel.Visible = false;
        }

        private void lessAmtTextBox_Leave(object sender, EventArgs e)
        {
            lessAmtTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void lessAmtTextBox_TextChanged(object sender, EventArgs e)
        {
            dueAmtTextBox.Text =Math.Round(Convert.ToDouble(Hlp.IsNumeric(netPayableTextBox.Text)?netPayableTextBox.Text:"0") - Convert.ToDouble(Hlp.IsNumeric(lessAmtTextBox.Text)?lessAmtTextBox.Text:"0"),2).ToString();
        }

        private void lessAmtTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void paidAmtTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void paidAmtTextBox_TextChanged(object sender, EventArgs e)
        {
            double netAmt = Math.Round(Convert.ToDouble(Hlp.IsNumeric(netPayableTextBox.Text) ? netPayableTextBox.Text : "0") - Convert.ToDouble(Hlp.IsNumeric(lessAmtTextBox.Text) ? lessAmtTextBox.Text : "0"), 2);
            dueAmtTextBox.Text =Math.Round(netAmt -Convert.ToDouble(Hlp.IsNumeric(paidAmtTextBox.Text) ? paidAmtTextBox.Text : "0"),2).ToString();

            if (Convert.ToDouble(dueAmtTextBox.Text)<0)
            {
                paidAmtTextBox.Text = "0";
                paidAmtTextBox.Focus();
                paidAmtTextBox.SelectAll();
            }
        }

        private void lessAmtTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                paidAmtTextBox.Focus();
            }
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
                    case "supp":
                        suppIdTextBox.Text = gccode;
                        suppNameTextBox.Text = gcdesc;
                        itemNameTextBox.Focus();
                        break;
                    case "item":
                        itemIdTextBox.Text = gccode;
                        itemNameTextBox.Text = gcdesc;
                        unitPriceTextBox.Text = _gt.FncReturnFielValueLab("tb_ph_ITEM", "Id='" + gccode + "'", "LastPPrice");
                        QtyTextBox.Focus();
                        break;

                }

            }
        }

        private void helpDg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (textInfo == "enter")
                {
                    string invNo = helpDg.CurrentRow.Cells[0].Value.ToString();
                    int masterId = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_ph_PURCHASE_MASTER", "BillNo='" + invNo + "'", "Id"));
                    if (_gt.FnSeekRecordNewLab("tb_ph_DUE_PAYMENT", "MasterId=" + masterId + ""))
                    {
                        MessageBox.Show(@"This bill has due collection.Please check.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {

                        if (MessageBox.Show(@"Do you want to request for cancel this bill?", @"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='" + Hlp.UserName + "' AND ParentName='Pharmacy' AND PermisionName='Purchase-Delete'"))
                            {
                                _gt.DeleteInsertLab("INSERT INTO  DEL_RECORD_OF_BILL_DELETE  (BillNo, BillDate, BillTime, RegId, PatientName, MobileNo, Address, Age, Sex, RefDrId, UnderDrId, TotalAmt, LessAmt, LessFrom, CollAmt, Remarks, PostedBy,ModuleName,Status,MasterId,PcName,IpAddress) SELECT a.BillNo, a.BillDate, '', 0, b.Name, b.ContactNo, b.Address, '', '', 0, 0, a.NetAmt, a.TotLess, '', a.TotPaid, a.Remarks, '" + Hlp.UserName + "','Pharmacy-Purchase','Pending'," + masterId + ",'" + Environment.UserName + "','" + Hlp.IpAddress() + "'  FROM tb_ph_PURCHASE_MASTER a INNER JOIN tb_ph_SUPPLIER b ON a.SuppId=b.Id WHERE a.Id="+ masterId +"");
                                MessageBox.Show(@"Purchase Bill cancel request success.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                searchTextBox.Focus();
                            }
                            else
                            {
                                MessageBox.Show(@"You need permission to do this task.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }


                        }
                    }
                }



            }
        }

        private void ReagentPurchaseUi_Load(object sender, EventArgs e)
        {
            Hlp.GridColor(dataGridView1);
            billDateTextBox.Text = Hlp.GetServerDate().ToString("dd-MMM-yyyy");


            billNoTextBox.Text = _gt.GetInvoiceNo(9);
            //dataGridView1.Columns[3].ReadOnly = false;
            foreach (DataGridViewColumn dc in dataGridView1.Columns)
            {
                dc.ReadOnly = !dc.Index.Equals(6);
            }


            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");

            GridWidth(dataGridView1);

            expireDateTimePicker.Value.AddMonths(2);
        }

        private void lotNoTextBox_Enter(object sender, EventArgs e)
        {
            lotNoTextBox.BackColor = Hlp.EnterFocus();
        }

        private void lotNoTextBox_Leave(object sender, EventArgs e)
        {
            lotNoTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void lotNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                expireDateTimePicker.Focus();
            }
        }
  
    }
}
