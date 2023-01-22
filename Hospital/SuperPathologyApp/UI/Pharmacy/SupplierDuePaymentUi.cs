using System;
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Gateway.Indoor;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Indoor;
using SuperPathologyApp.Model.Pharmacy;
using SuperPathologyApp.Gateway.Pharmacy;

namespace SuperPathologyApp.UI.Indoor
{
    public partial class SupplierDuePaymentUi : Form
    {
        readonly SupplierDuePaymentGateway _gt = new SupplierDuePaymentGateway();
        public SupplierDuePaymentUi()
        {
            InitializeComponent();
        }

        

        public void Gridwidth(int no)
        {
            switch (no)
            {
                case 1:
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Width = 300;
                    dataGridView1.Columns[2].Width = 250;
                    dataGridView1.Columns[3].Width = 130;
                    dataGridView1.Columns[4].Visible = false;
                   
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

  

        private void button2_Click(object sender, EventArgs e)
        {
            if (_gt.FnSeekRecordNewLab("tb_ph_SUPPLIER","Id='"+ suppIdTextBox.Text +"'")==false)
            {
                MessageBox.Show(@"Please Add Supplier Name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                suppIdTextBox.Focus();
                return;
            }
            if (paidAmtTextBox.Text==""||paidAmtTextBox.Text=="0")
            {
                MessageBox.Show(@"Please Add Some Money", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                paidAmtTextBox.Focus();
                return;
            }  

         
            var mdl = new PurchaseModel()
            {
              
             
                Supplier = new SupplierModel() { Id = Convert.ToInt32(suppIdTextBox.Text)},
                TotalPaid=Convert.ToDouble(paidAmtTextBox.Text),
               
            };
            string msg = _gt.Save(mdl);
           
            
            MessageBox.Show(msg, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearText();

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
                    case "supp":
                        suppIdTextBox.Text = gccode;
                        suppNameTextBox.Text = gcdesc;
                        addressTextBox.Text = _gt.FncReturnFielValueLab("tb_ph_SUPPLIER","Id='"+ gccode +"'","Address");
                        dueAmtTextBox.Text = _gt.FncReturnFielValueLab("V_ph_Supplier_Due_List", "SuppId='" + gccode + "'", "DueAmt");
                        paidAmtTextBox.Focus();
                        break;

                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearText();
        }


        ItemGateway itm = new ItemGateway();
        private void ClearText()
        {
            dataGridView1.DataSource = itm.GetItemList(0, "", 0);
            
            Gridwidth(1);
            Hlp.GridFirstRowDeselect(dataGridView1);
          
            paidAmtTextBox.Text = "";
           

        }

        private void addressTextBox_Enter(object sender, EventArgs e)
        {
            addressTextBox.BackColor = Hlp.EnterFocus();
        }

        private void addressTextBox_Leave(object sender, EventArgs e)
        {
            addressTextBox.BackColor = Hlp.LeaveFocus();
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

        private void button1_Click(object sender, EventArgs e)
        {
            
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
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name,GenericName,GroupName,SalePrice,ReOrderQty AS R_Qty FROM V_ph_ITEM_LIST WHERE SuppId='" + suppIdTextBox.Text + "' AND (convert(varchar,Id)+Name+GenericName) LIKE '%" + suppIdTextBox.Text + "%'");
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
            suppIdTextBox.BackColor = Hlp.LeaveFocus();

        }
        private string textInfo="";
        private void suppIdTextBox_Enter(object sender, EventArgs e)
        {
            suppIdTextBox.BackColor = Hlp.EnterFocus();
            PopulatedSupplierGridView();
            textinfo="supp";
        }

        private void PopulatedSupplierGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name,Address FROM tb_ph_SUPPLIER WHERE 1=1 AND (convert(varchar,Id)+Name) LIKE '%"+ suppIdTextBox.Text +"%'");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 90;
            dataGridView1.Columns[1].Width = 350;
            dataGridView1.Columns[2].Width = 200;

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
            PopulatedSupplierGridView();
            textinfo = "supp";
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
            ClearText();
        }

        private void paidAmtTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Hlp.StringToDouble(paidAmtTextBox.Text) > 0) 
            {
                remAmountTextBox.Text = (Hlp.StringToDouble(dueAmtTextBox.Text) - Hlp.StringToDouble(paidAmtTextBox.Text)).ToString();
                if (Hlp.StringToDouble(remAmountTextBox.Text )<0)
                {
                    paidAmtTextBox.Text = "0";
                    paidAmtTextBox.SelectAll();
                }
            
            }

        }

        private void paidAmtTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                remarksTextBox.Focus();
            }
        }

        private void remarksTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.Focus();
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

 

       
    }
}
