using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
 
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper ;
using SuperPathologyApp.Model;

namespace SuperPathologyApp.UI
{
    public partial class DoctorSetupUiNew : Form
    {
        DoctorGateway _gt = new DoctorGateway();
        public DoctorSetupUiNew()
        {
            InitializeComponent();
        }

        

        public void Gridwidth(int no)
        {
            switch (no)
            {
                case 1:
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Width = 320;
                    dataGridView1.Columns[2].Width = 210;
                    dataGridView1.Columns[3].Width = 120;

                    dataGridView1.Columns[4].Visible= false;
                    dataGridView1.Columns[5].Visible = false;
                    dataGridView1.Columns[6].Visible = false;
                    dataGridView1.Columns[7].Visible = false;
                    dataGridView1.Columns[8].Visible = false;

                    break;

            }
          
        }


        private void drCodeTextBox_Leave(object sender, EventArgs e)
        {
            drCodeTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void drNameTextBox_Enter(object sender, EventArgs e)
        {
            drNameTextBox.BackColor = Hlp.EnterFocus();
        }

        private void drNameTextBox_Leave(object sender, EventArgs e)
        {
            drNameTextBox.BackColor = DbConnection.LeaveFocus(); 
        }

       
       

        private void frmDoctorSetup_Activated(object sender, EventArgs e)
        {
            drNameTextBox.Focus();
        }

        private void drNameTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _gt.GetDoctorList(0, drNameTextBox.Text);
            Gridwidth(1);
        }
        public string textinfo = "";
        private void drNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Down)
            {
                textinfo = "1";
                dataGridView1.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                addressTextBox.Focus();
            }
        }

  

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (drNameTextBox.Text == "")
            {
                MessageBox.Show(@"Please add Doctor name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                drNameTextBox.Focus();
                return;
            }
            if (drCodeTextBox.Text == "")
            {
                drCodeTextBox.Text = "0";
                return;
            }
            var mdl = new DoctorModel
            {
                DrId =Convert.ToInt32(drCodeTextBox.Text),
                Name = drNameTextBox.Text,
                Address = addressTextBox.Text,
                ContactNo = contactNoTextBox.Text,
                TakeCommision =(int)takeCommisionComboBox.SelectedIndex,
            };
            mdl.Mpo=new MpoModel()
            {
                Id =  Convert.ToInt32(mpoComboBox.SelectedValue),
            };

            string msg =  _gt.Save(mdl);
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
                    case "1":
                        drCodeTextBox.Text = gccode;
                        drNameTextBox.Text = gcdesc;

                        takeCommisionComboBox.SelectedIndex = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_Doctor", "Id=" + drCodeTextBox.Text + "", "TakeCom"));
                        addressTextBox.Text = _gt.FncReturnFielValueLab("tb_Doctor", "Id=" + drCodeTextBox.Text + "", "Address");
                        contactNoTextBox.Text = _gt.FncReturnFielValueLab("tb_Doctor", "Id=" + drCodeTextBox.Text + "", "ContactNo");
                        mpoComboBox.SelectedValue = _gt.FncReturnFielValueLab("tb_Doctor", "Id=" + drCodeTextBox.Text + "", "MpoId");
                       
                        break;
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.KeyCode==Keys.Delete)
            {
                if (MessageBox.Show(@"Do you want to delete this doctor?", @"Information", MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
                else
                {
                    if (dataGridView1.Rows.Count>1)
                    {
                        int drId =Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                        _gt.DeleteInsertLab("Delete tb_Doctor  WHERE Id=" + drId + "");//Aman
                      //  _gt.DeleteInsertLab("Update tb_Doctor SET Valid=0 WHERE Id="+ drId +"");
                        MessageBox.Show(@"Delete Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }







            }


        }



       
        private void DoctorSetupUiNew_Load(object sender, EventArgs e)
        {
            ClearText();
        }

        private void ClearText()
        {
            dataGridView1.DataSource = _gt.GetDoctorList(0, "");
            Gridwidth(1);
            Hlp.GridColor(dataGridView1);
            takeCommisionComboBox.SelectedIndex = 1;
            drNameTextBox.Text = "";
            addressTextBox.Text = "";
            contactNoTextBox.Text = "";
            drCodeTextBox.Text = "";
           // _gt.LoadComboBox("SELECT Distinct 0 AS Id,UserName AS Description FROM tb_USER_PRIVILEGE Order By UserName", reportDoctorUserNameTextBox);
            drNameTextBox.Focus();
            _gt.LoadComboBox("SELECT  Id, Name AS Description FROM tb_MPO  ", mpoComboBox);

        }

        private void addressTextBox_Enter(object sender, EventArgs e)
        {
            addressTextBox.BackColor = Hlp.EnterFocus();
        }

        private void addressTextBox_Leave(object sender, EventArgs e)
        {
            addressTextBox.BackColor = Hlp.LeaveFocus();
        }


        private void contactNoTextBox_Enter(object sender, EventArgs e)
        {
            contactNoTextBox.BackColor = Hlp.EnterFocus();
        }

        private void contactNoTextBox_Leave(object sender, EventArgs e)
        {
            contactNoTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            if (mpoComboBox.Text=="--Select--")
            {
                return;
            }
            if (_gt.FnSeekRecordNewLab("tb_MPO","Name='"+ mpoComboBox.Text +"'")==false)
            {
                _gt.DeleteInsertLab("INSERT INTO tb_MPO(Name)VALUES('"+ mpoComboBox.Text +"')");
                MessageBox.Show(@"Added Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _gt.LoadComboBox("SELECT  Id, Name AS Description FROM tb_MPO  ", mpoComboBox);
            }
        }

 

       
    }
}
