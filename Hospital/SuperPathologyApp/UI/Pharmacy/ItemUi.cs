using System;
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Gateway.Indoor;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Indoor;
using SuperPathologyApp.Model.Pharmacy;

namespace SuperPathologyApp.UI.Indoor
{
    public partial class ItemUi : Form
    {
        readonly ItemGateway _gt = new ItemGateway();
        public ItemUi()
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


        private void drCodeTextBox_Leave(object sender, EventArgs e)
        {
            ptIdTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void drNameTextBox_Enter(object sender, EventArgs e)
        {
            genericNameTextBox.BackColor = Hlp.EnterFocus();
        }

        private void drNameTextBox_Leave(object sender, EventArgs e)
        {
            genericNameTextBox.BackColor = DbConnection.LeaveFocus(); 
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
            if (retailPriceTextBox.Text==""||retailPriceTextBox.Text=="0")
            {
                MessageBox.Show(@"Please Add Sales Price", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                retailPriceTextBox.Focus();
                return;
            }  

            if (nameTextBox.Text == "")
            {
                MessageBox.Show(@"Please Add Item Name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                nameTextBox.Focus();
                return;
            }


            var mdl = new ItemModel()
            {
                Id = Convert.ToInt32(ptIdTextBox.Text),
                Name = nameTextBox.Text,
                GenericName = genericNameTextBox.Text,
                Group = new GroupModel() { Id = (int)groupComboBox.SelectedValue },
                Supplier = new SupplierModel() { Id = Convert.ToInt32(suppIdTextBox.Text)},
                SalePrice=Convert.ToDouble(retailPriceTextBox.Text),
                WholeSalePrice = Convert.ToDouble(Hlp.IsNumeric(wholeSalePrice.Text) ? wholeSalePrice.Text : "0"),
                ReOrderQty = Convert.ToInt32(Hlp.IsNumeric(reorderQtyTextBox.Text) ? reorderQtyTextBox.Text : "0"),
                IsDiscItem=(int)discountedItemStatusComboBox.SelectedIndex,
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
                        nameTextBox.Focus();
                        break;
                    case "item":
                        ptIdTextBox.Text = gccode;
                        nameTextBox.Text = gcdesc;
                       var data=_gt.GetItemList(Convert.ToInt32(gccode), "",0);
                        if (data.Rows.Count>0)
                        {
                            groupComboBox.SelectedValue = data.Rows[0]["GroupId"].ToString();
                            genericNameTextBox.Text = data.Rows[0]["GenericName"].ToString();
                            retailPriceTextBox.Text = data.Rows[0]["SalePrice"].ToString();
                            wholeSalePrice.Text = data.Rows[0]["WholeSalePrice"].ToString();
                            reorderQtyTextBox.Text = data.Rows[0]["ReOrderQty"].ToString();
                            discountedItemStatusComboBox.SelectedIndex =Convert.ToInt32(data.Rows[0]["IsDiscItem"]);
                            suppIdTextBox.Text = data.Rows[0]["SuppId"].ToString();
                            suppNameTextBox.Text = data.Rows[0]["SuppName"].ToString();
                            nameTextBox.Focus();
                        }
                        
                        
                        
                        break;

                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        

        private void ClearText()
        {
            dataGridView1.DataSource = _gt.GetItemList(0, "",Convert.ToInt32(Hlp.IsNumeric(suppIdTextBox.Text)?suppIdTextBox.Text:"0"));
            
            Gridwidth(1);
            Hlp.GridFirstRowDeselect(dataGridView1);
            genericNameTextBox.Text = "";
            nameTextBox.Text = "";
            retailPriceTextBox.Text = "";
            ptIdTextBox.Text = "0";
            nameTextBox.Focus();
            discountedItemStatusComboBox.SelectedIndex = 1;
            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_ph_ITEM_GROUP Order by Id", groupComboBox);

        }

        private void addressTextBox_Enter(object sender, EventArgs e)
        {
            genericNameTextBox.BackColor = Hlp.EnterFocus();
        }

        private void addressTextBox_Leave(object sender, EventArgs e)
        {
            genericNameTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void contactNoTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }

       

        private void genderComboBox_KeyDown(object sender, KeyEventArgs e)
        {

        }


      

        private void bedNoTextBox_Enter(object sender, EventArgs e)
        {
            nameTextBox.BackColor = Hlp.EnterFocus();
        }

        private void bedNoTextBox_Leave(object sender, EventArgs e)
        {
            nameTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void bedNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                textinfo = "bed";
                dataGridView1.Rows[0].Selected = true;
                dataGridView1.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                genericNameTextBox.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        
       

        private void SupplierUi_Load(object sender, EventArgs e)
        {
            ClearText();
            nameTextBox.Focus();    
        }

        private void nameTextBox_Enter(object sender, EventArgs e)
        {
            nameTextBox.BackColor = Hlp.EnterFocus();
            PopulatedItemGridBySuppId();
            textinfo = "item";
        }

        private void PopulatedItemGridBySuppId()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name,GenericName,GroupName,SalePrice,ReOrderQty AS R_Qty FROM V_ph_ITEM_LIST WHERE SuppId='" + suppIdTextBox.Text + "' AND (convert(varchar,Id)+Name+GenericName) LIKE '%" + nameTextBox.Text + "%'");
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
            nameTextBox.BackColor = Hlp.LeaveFocus();
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
            if (groupComboBox.Text=="--Select--")
            {
                return;
            }
            
            
            if (_gt.FnSeekRecordNewLab("tb_ph_ITEM_GROUP", "Name='" + groupComboBox.Text + "'") == false)
            {
                _gt.DeleteInsertLab("INSERT INTO tb_ph_ITEM_GROUP(Name)VALUES('" + groupComboBox.Text + "')");
                MessageBox.Show(@"Added Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_ph_ITEM_GROUP Order by Id", groupComboBox);
            }
            else
            {
                MessageBox.Show(@"Group Name Already Exist", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



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

 

       
    }
}
