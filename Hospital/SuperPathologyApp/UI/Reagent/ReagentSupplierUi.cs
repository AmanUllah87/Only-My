using System;
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Gateway.Indoor;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Indoor;
using SuperPathologyApp.Model.Pharmacy;
using SuperPathologyApp.Gateway.Reagent;

namespace SuperPathologyApp.UI.Pharmacy
{
    public partial class ReagentSupplierUi : Form
    {
        readonly ReagentSupplierGateway _gt = new ReagentSupplierGateway();
        public ReagentSupplierUi()
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
            addressTextBox.BackColor = Hlp.EnterFocus();
        }

        private void drNameTextBox_Leave(object sender, EventArgs e)
        {
            addressTextBox.BackColor = DbConnection.LeaveFocus(); 
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
               
            if (addressTextBox.Text == "")
            {
                MessageBox.Show(@"Please Add Floor No", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                addressTextBox.Focus();
                return;
            }

            if (nameTextBox.Text == "")
            {
                MessageBox.Show(@"Please Add Name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                nameTextBox.Focus();
                return;
            }
         

            var mdl = new SupplierModel()
            {
                Id = Convert.ToInt32(ptIdTextBox.Text ),
                Name = nameTextBox.Text,
                Address = addressTextBox.Text,
                MobileNo= contactNoTextBox.Text,
              
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
                    case "bed":
                        
                        ptIdTextBox.Text = gccode;
                        nameTextBox.Text = gcdesc;
                        var data=_gt.GetSupplierList(Convert.ToInt32(gccode), "");
                        if (data.Rows.Count>0)
                        {
                            addressTextBox.Text = data.Rows[0]["Address"].ToString();
                            nameTextBox.Text = data.Rows[0]["Name"].ToString();
                            contactNoTextBox.Text = data.Rows[0]["ContactNo"].ToString();
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
            dataGridView1.DataSource = _gt.GetSupplierList( 0,"");
            
            Gridwidth(1);
            addressTextBox.Text = "";
            nameTextBox.Text = "";
            contactNoTextBox.Text = "";
            ptIdTextBox.Text = "0";
            nameTextBox.Focus();
        


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
                addressTextBox.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        
       

        private void SupplierUi_Load(object sender, EventArgs e)
        {
              
        }

        private void nameTextBox_Enter(object sender, EventArgs e)
        {
            nameTextBox.BackColor = Hlp.EnterFocus();
        }

        private void addressTextBox_Enter_1(object sender, EventArgs e)
        {
            addressTextBox.BackColor = Hlp.EnterFocus();
        }

        private void addressTextBox_Leave_1(object sender, EventArgs e)
        {
            addressTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            nameTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void ReagentSupplierUi_Load(object sender, EventArgs e)
        {
            ClearText();
            nameTextBox.Focus();  
        }

 

       
    }
}
