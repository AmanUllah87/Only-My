using System;
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Gateway.Indoor;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Indoor;
using SuperPathologyApp.Model.Pharmacy;
using SuperPathologyApp.Gateway.Pharmacy;
using System.Collections.Generic;

namespace SuperPathologyApp.UI.Indoor
{
    public partial class SalesReturnUi : Form
    {
        readonly SalesReturnGateway _gt = new SalesReturnGateway();
        public SalesReturnUi()
        {
            InitializeComponent();
        }

        

        public void Gridwidth(int no)
        {
            switch (no)
            {
                case 2:
                    dataGridView2.Columns[0].Width= 80;
                    dataGridView2.Columns[1].Width = itemNameTextBox.Width-160;
                    dataGridView2.Columns[2].Width = 78;
                    dataGridView2.Columns[3].Width = 82;
                    dataGridView2.Columns[4].Width = salesPriceTextBox.Width-2;

                //dataGridView1.Columns[4].Visible = false;
                   
                    break;

            }
          
        }


    



        private void drNameTextBox_TextChanged(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = _gt.GetRegisterPatientList(0, drNameTextBox.Text);
            //Gridwidth(1);
        }
        public string textinfo = "";
        private void drNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

  

   


        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string gccode = "";
                string gcdesc = "";

                gccode = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[0].Value.ToString();
                gcdesc = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[1].Value.ToString();
             
                switch (textinfo)
                {
                    case "indoor":
                        gcdesc = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[3].Value.ToString();
                        indoorIdTextBox.Text = gccode;
                        ptNameTextBox.Text = gcdesc;
                        itemNameTextBox.Focus();
                        LoadGridByIndoorId();


                        break;
                    case "item":
                        string pPrice = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[3].Value.ToString();
                        string itemId = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[1].Value.ToString();
                        string itemName = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[2].Value.ToString();
                        string sPrice= dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[4].Value.ToString();
                        string qty = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[5].Value.ToString();


                        itemIdTextBox.Text = itemId;
                        itemNameTextBox.Text = itemName;
                        pPriceTextBox.Text = pPrice;
                        salesPriceTextBox.Text = sPrice;
                        qtyTextBox.Text = qty;
                        qtyTextBox.Focus();
                        qtyTextBox.SelectAll();
                        
                        break;


                }

            }
        }

        private void LoadGridByIndoorId()
        {
            string query = @"SELECT a.AdmId, a.ItemId,b.Name AS ItemName, a.PurchasePrice,a.SalesPrice,SUM(a.OutQty-a.InQty) AS BalQty 
FROM tb_ph_STOCK_LEDGER a LEFT JOIN tb_ph_ITEM b ON a.ItemId=b.Id WHERE a.AdmId='"+ indoorIdTextBox.Text  +"' AND b.Name Like '%" + itemNameTextBox.Text + "%' GROUP BY a.ItemId,a.AdmId, a.PurchasePrice,a.SalesPrice,b.Name HAVING SUM(a.OutQty-a.InQty)<>0";
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, query);
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[2].Width = 270;
            dataGridView1.Columns[3].Width = 70;
            dataGridView1.Columns[4].Width = 70;
            dataGridView1.Columns[5].Width = 70;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearText();
        }


        ItemGateway itm = new ItemGateway();
        private void ClearText()
        {
            dataGridView2.Rows.Clear();
            totalAmountTextBox.Text = "0";
            totalItemTextBox.Text = "0";
            dataGridView1.DataSource = itm.GetItemList(0, "", 0);
            Gridwidth(1);
            Hlp.GridFirstRowDeselect(dataGridView1);
            billNoTextBox.Text = _gt.GetInvoiceNo(8);
            indoorIdTextBox.Text = "";
            ptNameTextBox.Text = "";
         //   dueAmtTextBox.Text = "";
            custNameTextBox.Text = "";
            refDateTextBox.Text = "";
            indoorIdTextBox.Focus();
            billDateTextBox.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            helpPanel.Visible = true;
        }

        private void addressTextBox_Enter(object sender, EventArgs e)
        {
            custNameTextBox.BackColor = Hlp.EnterFocus();
        }

        private void addressTextBox_Leave(object sender, EventArgs e)
        {
            custNameTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void contactNoTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }

       

        private void genderComboBox_KeyDown(object sender, KeyEventArgs e)
        {

        }


  

        private void bedNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                textinfo = "bed";
                dataGridView1.Rows[0].Selected = true;
                dataGridView1.Focus();
            }

        }

        
       

        private void SupplierUi_Load(object sender, EventArgs e)
        {
            ClearText();
         
        }

        private void nameTextBox_Enter(object sender, EventArgs e)
        {
        
            PopulatedItemGridBySuppId();
            textinfo = "item";
        }

        private void PopulatedItemGridBySuppId()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name,GenericName,GroupName,SalePrice,ReOrderQty AS R_Qty FROM V_ph_ITEM_LIST WHERE SuppId='" + indoorIdTextBox.Text + "' AND (convert(varchar,Id)+Name+GenericName) LIKE '%" + indoorIdTextBox.Text + "%'");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 200;
            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Columns[3].Width = 80;
            dataGridView1.Columns[4].Width = 80;
            dataGridView1.Columns[5].Width = 70;


        }

      

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
           
        }

        private void ItemUi_Load(object sender, EventArgs e)
        {
            ClearText();
        }

        private void retailPriceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void wholeSalePrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {
            
            
            //if (_gt.FnSeekRecordNewLab("tb_ph_ITEM_GROUP", "Name='" + groupComboBox.Text + "'") == false)
            //{
            //    _gt.DeleteInsertLab("INSERT INTO tb_ph_ITEM_GROUP(Name)VALUES('" + groupComboBox.Text + "')");
            //    MessageBox.Show(@"Added Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_ph_ITEM_GROUP Order by Id", groupComboBox);
            //}
            //else
            //{
            //    MessageBox.Show(@"Group Name Already Exist", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}



        }

       

        private void suppIdTextBox_Leave(object sender, EventArgs e)
        {
            indoorIdTextBox.BackColor = Hlp.LeaveFocus();

        }
        private string textInfo="";
        private void suppIdTextBox_Enter(object sender, EventArgs e)
        {
            indoorIdTextBox.BackColor = Hlp.EnterFocus();
            PopulatedIndoorGridView();
            textinfo="indoor";
            helpPanel.Visible = true;
        }

        private void PopulatedIndoorGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,AdmNo,AdmDate,PtName,ContactNo,BedName As Bed FROM V_Admission_List WHERE ReleaseStatus=0 AND (convert(varchar,Id)+PtName+BedName+ContactNo) LIKE '%" + indoorIdTextBox.Text + "%'");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 75;
            dataGridView1.Columns[2].Width = 75;
            dataGridView1.Columns[3].Width = 205;
            dataGridView1.Columns[4].Width = 110;


        }

        private void nameTextBox_Enter_1(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Hlp.EnterFocus();


        }

        private void nameTextBox_Leave_1(object sender, EventArgs e)
        {
            ((TextBox)sender).BackColor = Hlp.LeaveFocus();
        }

        private void discountedItemStatusComboBox_Enter(object sender, EventArgs e)
        {
            ((ComboBox)sender).BackColor = Hlp.EnterFocus();
        }

        private void discountedItemStatusComboBox_Leave(object sender, EventArgs e)
        {
            ((ComboBox)sender).BackColor = Hlp.LeaveFocus();
        }

        private void suppIdTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void suppIdTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Down||e.KeyCode==Keys.Up)
            {
                if (dataGridView1.Rows.Count>0)
                {
                    dataGridView1.Rows[0].Selected = true;
                    dataGridView1.Focus();
                }
            }
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            PopulatedItemGridBySuppId();
            textinfo = "item";
        }

        private void nameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Down||e.KeyCode==Keys.Up)
            {
                dataGridView1.Focus();
                if (dataGridView1.Rows.Count>0)
                {
                    dataGridView1.Rows[0].Selected = true;
                }
            }
        }

        private void SupplierDuePaymentUi_Load(object sender, EventArgs e)
        {
            
        }


      

        private void remarksTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                saveAndPrintButton.Focus();
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _gt.GetInvoiceList(DateTime.Now, "", "enter");
            Hlp.GridFirstRowDeselect(dataGridView1);
     
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _gt.GetInvoiceList(DateTime.Now, "", "");
            Hlp.GridFirstRowDeselect(dataGridView1);
     
        }

        private void SupplierReturnUi_Load(object sender, EventArgs e)
        {
            
        }

        private void itemNameTextBox_Enter(object sender, EventArgs e)
        {
            itemNameTextBox.BackColor = Hlp.EnterFocus();
            if (_gt.FnSeekRecordNewLab("tb_in_Admission", "Id='" + indoorIdTextBox.Text + "'"))
            {
                LoadGridByIndoorId();
            }

            helpPanel.Visible = false;
        }

        private void itemNameTextBox_Leave(object sender, EventArgs e)
        {
            itemNameTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void qtyTextBox_Enter(object sender, EventArgs e)
        {
            qtyTextBox.BackColor = Hlp.EnterFocus();

        }

        private void qtyTextBox_Leave(object sender, EventArgs e)
        {
            qtyTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void itemNameTextBox_TextChanged(object sender, EventArgs e)
        {
            textinfo = "item";
            if (_gt.FnSeekRecordNewLab("tb_in_Admission","Id='"+ indoorIdTextBox.Text +"'"))
            {
                LoadGridByIndoorId();
            }
            
            helpPanel.Visible = true;
        }
        private void LoadGridViewByName()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT ItemId,ItemName,BalQty,PurchasePrice FROM V_ph_Curr_Stock WHERE SuppId='" + indoorIdTextBox.Text + "' AND BalQty<>0 AND ItemName LIKE '%" + itemNameTextBox.Text + "%'");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 100;
            dataGridView1.Columns[1].Width = 260;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 100;
        }

        private void itemNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Down||e.KeyCode==Keys.Up)
            {
                textinfo = "item";
                helpPanel.Visible = true;
                dataGridView1.Rows[0].Cells[2].Selected = true;
                dataGridView1.Focus();

            }
            if (e.KeyCode==Keys.Enter)
            {
                if (dataGridView2.Rows.Count>0)
                {
                    discountPcTextBox.Focus();
                }
            }



        }

        private void qtyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void qtyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddDataToGrid();
                Gridwidth(2);
            }
        }
        private void AddDataToGrid()
        {


            if (_gt.FnSeekRecordNewLab("tb_ph_item", "Id='" + itemIdTextBox.Text + "'") == false)
            {
                MessageBox.Show(@"Invalid Item Name", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            


            int itemId = Convert.ToInt32(itemIdTextBox.Text);
            if (IsDuplicate(itemIdTextBox.Text,salesPriceTextBox.Text, dataGridView2) == false)
            {
                double unitTotal = Convert.ToDouble(salesPriceTextBox.Text) * Convert.ToDouble(qtyTextBox.Text);
                double avgLess = 0;
                if (masterId!=0)
                {
                    double salesLess =Convert.ToDouble( _gt.FncReturnFielValueLab("tb_ph_SALES_DETAIL", "MasterId='" + masterId + "' AND ItemId='"+ itemIdTextBox.Text +"'","SUM(LessAmt)"));
                    double qtySales = Convert.ToDouble(_gt.FncReturnFielValueLab("tb_ph_SALES_DETAIL", "MasterId='" + masterId + "' AND ItemId='" + itemIdTextBox.Text + "'", "SUM(Qty)"));
                    avgLess = salesLess / qtySales;
                }
                double lessAdj = avgLess * Convert.ToDouble(qtyTextBox.Text);

                dataGridView2.Rows.Add(itemId, itemNameTextBox.Text, salesPriceTextBox.Text, qtyTextBox.Text, unitTotal,pPriceTextBox.Text,lessAdj);
                itemNameTextBox.Text = "";
                qtyTextBox.Text = "";
                salesPriceTextBox.Text = "";
                itemIdTextBox.Text = "0";
                dataGridView2.CurrentCell.Selected = false;
                helpPanel.Visible = false;
                itemNameTextBox.Focus();
            }
            else
            {
                MessageBox.Show(@"Duplicate Name Found", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                itemNameTextBox.SelectAll();
                return;
            }

            CalculateTotal();



        }
        private void CalculateTotal()
        {
            double totAmt = 0,totLess=0;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                totAmt += Convert.ToDouble(Hlp.StringToDouble(dataGridView2.Rows[i].Cells[4].Value.ToString()));
                totLess += Convert.ToDouble(Hlp.StringToDouble(dataGridView2.Rows[i].Cells[6].Value.ToString()));

            }
            totalAmountTextBox.Text = totAmt.ToString();
            totalItemTextBox.Text = dataGridView2.Rows.Count.ToString();
            lessAdjustAmtTextBox.Text = totLess.ToString();

        }

        private Boolean IsDuplicate(string lcCode, string pPrice, DataGridView dataGrid)
        {
            try
            {
                for (int j = 0; j < dataGrid.Rows.Count; j++)
                {
                    string lcPdCode = dataGrid.Rows[j].Cells[0].Value.ToString();
                    string lcGridPPrice = dataGrid.Rows[j].Cells[3].Value.ToString();

                    if (lcCode == lcPdCode)
                    {
                        if (pPrice == lcGridPPrice)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }

        }

        private void saveAndPrintButton_Click(object sender, EventArgs e)
        {
            if (indoorIdTextBox.Text!="")
            {
                refDateTextBox.Text = "";
                salesInvNoTextBox.Text = "";
                if (_gt.FnSeekRecordNewLab("tb_in_ADMISSION", "Id='" + indoorIdTextBox.Text + "' AND ReleaseStatus=0") == false)
                {
                    MessageBox.Show(@"Invalid Indoor Id Please Check", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    indoorIdTextBox.Focus();
                    return;
                }
            }
         
            if (dataGridView2.Rows.Count==0)
            {
                MessageBox.Show(@"Please Add Item", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                itemNameTextBox.Focus();
                return;
            }








            var mdl = new PurchaseModel()
            {
                PatientsModel = new PatientModel() { RegId = Convert.ToInt32(Hlp.IsNumeric(custIdTextBox.Text) ? custIdTextBox.Text : "0") },
                Admission = new AdmissionModel() { AdmId = Convert.ToInt32(Hlp.IsNumeric(indoorIdTextBox.Text) ? indoorIdTextBox.Text : "0") },

                TotAmount = Convert.ToDouble(totalAmountTextBox.Text),
                TotalItem = Convert.ToDouble(totalItemTextBox.Text),
                TotalLess = Convert.ToDouble(Hlp.StringToDouble(lessAdjustAmtTextBox.Text)),
                ReceiptNo=salesInvNoTextBox.Text,
            };
            var mdlItem = new List<ItemModel>();

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                mdlItem.Add(new ItemModel()
                {
                    Id = Convert.ToInt32(dataGridView2.Rows[i].Cells[0].Value.ToString()),
                    Qty = Convert.ToDouble(Hlp.StringToDouble(dataGridView2.Rows[i].Cells[3].Value.ToString())),
                    PurchasePrice = Convert.ToDouble(Hlp.StringToDouble(dataGridView2.Rows[i].Cells[5].Value.ToString())),
                    UnitTotal = Convert.ToDouble(Hlp.StringToDouble(dataGridView2.Rows[i].Cells[4].Value.ToString())),
                    SalePrice= Convert.ToDouble(Hlp.StringToDouble(dataGridView2.Rows[i].Cells[2].Value.ToString())),

                });
            }
            mdl.ItemModels = mdlItem;


            string msg = _gt.Save(mdl,saveAndPrintButton.Text);
            MessageBox.Show(msg, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (msg==_gt.SaveSuccessMessage)
            {
                ClearText();
            }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (dataGridView2.CurrentCell == null)
                {
                    return;
                }


                dataGridView2.Rows.RemoveAt(this.dataGridView2.CurrentCell.RowIndex);
                    CalculateTotal();
               







            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        private void remarksTextBox_Enter(object sender, EventArgs e)
        {
            remarksTextBox.BackColor = Hlp.EnterFocus();
            helpPanel.Visible = false;
        }

        private void remarksTextBox_Leave(object sender, EventArgs e)
        {
            remarksTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void searchTextBox_Enter(object sender, EventArgs e)
        {
            searchTextBox.BackColor = Hlp.EnterFocus();

            textInfo = "enter";
            dataGridView2.DataSource = _gt.GetInvoiceList(DateTime.Now, searchTextBox.Text, "enter");
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView1.Columns[0].Width = 80;
                dataGridView1.Columns[1].Width = 80;
                dataGridView1.Columns[2].Width = 230;
                dataGridView1.Columns[3].Width = 90;
                dataGridView1.Columns[4].Width = 80;
                helpPanel.Visible = true;
                Hlp.GridFirstRowDeselect(dataGridView1);
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            textInfo = "enter";
            dataGridView2.DataSource = _gt.GetInvoiceList(DateTime.Now, searchTextBox.Text, "change");
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView1.Columns[0].Width = 80;
                dataGridView1.Columns[1].Width = 80;
                dataGridView1.Columns[2].Width = 230;
                dataGridView1.Columns[3].Width = 90;
                dataGridView1.Columns[4].Width = 80;
                helpPanel.Visible = true;
                Hlp.GridFirstRowDeselect(dataGridView1);
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (textInfo == "enter")
                {
                    string invNo = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                    int masterId = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_ph_PURCHASE_RETURN_MASTER", "BillNo='" + invNo + "'", "Id"));
                    

                        if (MessageBox.Show(@"Do you want to request for cancel this return bill?", @"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='" + Hlp.UserName + "' AND ParentName='Pharmacy' AND PermisionName='Bill-Return-DELETE'"))
                            {
                                _gt.DeleteInsertLab("INSERT INTO  DEL_RECORD_OF_BILL_DELETE  (BillNo, BillDate, BillTime, RegId, PatientName, MobileNo, Address, Age, Sex, RefDrId, UnderDrId, TotalAmt, LessAmt, LessFrom, CollAmt, Remarks, PostedBy,ModuleName,Status,MasterId,PcName,IpAddress) SELECT a.BillNo, a.BillDate, '', '', b.Name AS SuppName, b.ContactNo, b.Address, '', '', 0, 0, a.TotAmt, 0, '', 0, a.Remarks, '" + Hlp.UserName + "','Pharmacy-PurchaseReturn','Pending'," + masterId + ",'" + Environment.UserName + "','" + Hlp.IpAddress() + "' FROM tb_ph_PURCHASE_RETURN_MASTER a INNER JOIN tb_ph_SUPPLIER b ON a.SuppId=b.Id WHERE a.Id=" + masterId + " ");

                                MessageBox.Show(@"Purchase return cancel request success.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SalesReturnUi_Load(object sender, EventArgs e)
        {
            ClearText();
            discountTypeComboBox.SelectedIndex = 0;
        }

        private void indoorIdTextBox_TextChanged(object sender, EventArgs e)
        {
            PopulatedIndoorGridView();
            textinfo = "indoor";
        }

        private void custIdTextBox_TextChanged(object sender, EventArgs e)
        {
            PopulatedRegisterGridView();
            textinfo = "register";
        }

        private void PopulatedRegisterGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,AdmNo,AdmDate,PtName,ContactNo,BedName As Bed FROM V_Admission_List WHERE ReleaseStatus=0 AND (convert(varchar,Id)+PtName+BedName+ContactNo) LIKE '%" + indoorIdTextBox.Text + "%'");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 75;
            dataGridView1.Columns[2].Width = 75;
            dataGridView1.Columns[3].Width = 205;
            dataGridView1.Columns[4].Width = 110;
        }

        int masterId = 0;
        private void salesInvNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (_gt.FnSeekRecordNewLab("tb_ph_SALES_MASTER", "BillNo='" + salesInvNoTextBox.Text + "'") == false)
                {
                    MessageBox.Show(@"Invalid Bill No Please Check", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    salesInvNoTextBox.Focus();
                }
                else
                {
                    masterId =Convert.ToInt32(_gt.FncReturnFielValueLab("tb_ph_SALES_MASTER", "BillNo='" + salesInvNoTextBox.Text + "'","Id"));
                    itemNameTextBox.Focus();
                    refDateTextBox.Text =Convert.ToDateTime( _gt.FncReturnFielValueLab("tb_ph_SALES_MASTER", "Id='" + masterId + "'", "BillDate")).ToString("dd-MMM-yy");
                    PopulatedGridViewByInvNo(masterId);
                }
            }
        }

        private void PopulatedGridViewByInvNo(int masterId)
        {
            string query = @"SELECT a.MasterId, a.ItemId,b.Name AS ItemName, a.PurchasePrice,a.SalesPrice,SUM(a.OutQty-a.InQty) AS BalQty 
FROM tb_ph_STOCK_LEDGER a LEFT JOIN tb_ph_ITEM b ON a.ItemId=b.Id WHERE MasterId="+ masterId +" AND  b.Name Like '%" + itemNameTextBox.Text + "%' GROUP BY a.ItemId,a.MasterId, a.PurchasePrice,a.SalesPrice,b.Name HAVING SUM(a.OutQty-a.InQty)<>0";
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, query);
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[2].Width = 270;
            dataGridView1.Columns[3].Width = 70;
            dataGridView1.Columns[4].Width = 70;
            dataGridView1.Columns[5].Width = 70;
        }

        private void discountPcTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void discountTypeComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                switch (discountTypeComboBox.SelectedIndex)
                {
                    case 1:
                        double totDiscount = Convert.ToDouble(totalAmountTextBox.Text) * 0.01 * Convert.ToDouble(discountPcTextBox.Text);
                        lessAdjustAmtTextBox.Text = totDiscount.ToString();
                        break;
                    case 0:
                        lessAdjustAmtTextBox.Text = discountPcTextBox.Text;
                        break;
                }
                remarksTextBox.Focus();

            }
        }

        private void discountPcTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                discountTypeComboBox.Focus();
            }
        }

        private void discountPcTextBox_Enter(object sender, EventArgs e)
        {
            discountPcTextBox.BackColor = Hlp.EnterFocus();
        }

        private void discountPcTextBox_Leave(object sender, EventArgs e)
        {
            discountPcTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void discountTypeComboBox_Enter(object sender, EventArgs e)
        {
            discountTypeComboBox.BackColor = Hlp.EnterFocus();

        }

        private void discountTypeComboBox_Leave(object sender, EventArgs e)
        {
            discountTypeComboBox.BackColor = Hlp.LeaveFocus();
        }

        private void lessAdjustAmtTextBox_TextChanged(object sender, EventArgs e)
        {
            RtnAmount.Text = (float.Parse(totalAmountTextBox.Text) - float.Parse(lessAdjustAmtTextBox.Text)).ToString();//Aman
        }
    }
}
