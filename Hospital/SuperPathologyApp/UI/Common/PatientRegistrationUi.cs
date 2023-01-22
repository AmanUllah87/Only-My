using System;
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;

namespace SuperPathologyApp.UI.Common
{
    public partial class PatientRegistrationUi : Form
    {
        PatientGateway _gt = new PatientGateway();
        public PatientRegistrationUi()
        {
            InitializeComponent();
        }

        

        public void Gridwidth(int no)
        {
            switch (no)
            {
                case 1:
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Width = 100;
                    dataGridView1.Columns[2].Width = 80;
                    dataGridView1.Columns[3].Width = 200;
                    dataGridView1.Columns[4].Width = 100;
                    dataGridView1.Columns[5].Width = 60;
                    dataGridView1.Columns[6].Width = 90;

                   // dataGridView1.Columns[7].Visible = false;


                    break;

            }
          
        }


        private void drCodeTextBox_Leave(object sender, EventArgs e)
        {
            ptIdTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void drNameTextBox_Enter(object sender, EventArgs e)
        {
            NameTextBox.BackColor = Hlp.EnterFocus();
        }

        private void drNameTextBox_Leave(object sender, EventArgs e)
        {
            NameTextBox.BackColor = DbConnection.LeaveFocus(); 
        }

       
       

        private void frmDoctorSetup_Activated(object sender, EventArgs e)
        {
            NameTextBox.Focus();
        }

        private void drNameTextBox_TextChanged(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = _gt.GetRegisterPatientList(0, drNameTextBox.Text);
            //Gridwidth(1);
        }
        public string textinfo = "";
        private void drNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.KeyCode == Keys.Enter)
            {
                genderComboBox.Focus();
            }
        }

  

        private void button2_Click(object sender, EventArgs e)
        {
            if (NameTextBox.Text == "")
            {
                MessageBox.Show(@"Please Add Patient Name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                NameTextBox.Focus();
                return;
            }

            var mdl = new PatientModel()
            {
                PatientId = Convert.ToInt32(ptIdTextBox.Text ),
                Name = NameTextBox.Text,
                Address = addressTextBox.Text,
                ContactNo = contactNoTextBox.Text,
                Dob = dtDobTimePicker.Value,
                Sex = genderComboBox.Text,
                
            };
            string msg = "";
            msg = _gt.FnSeekRecordNewLab("tb_PATIENT","Id='"+ mdl.PatientId +"'") ? _gt.Update(mdl) : _gt.Save(mdl);
            
            
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
                    case "test":
                        ptIdTextBox.Text = gccode;
                        contactNoTextBox.Text = gcdesc;

                        NameTextBox.Text = _gt.FncReturnFielValueLab("tb_PATIENT", "Id=" + ptIdTextBox.Text + "", "Name");
                        addressTextBox.Text = _gt.FncReturnFielValueLab("tb_PATIENT", "Id=" + ptIdTextBox.Text + "", "Address");
                        genderComboBox.Text = _gt.FncReturnFielValueLab("tb_PATIENT", "Id=" + ptIdTextBox.Text + "", "Sex");
                        dtDobTimePicker.Value =Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_PATIENT", "Id=" + ptIdTextBox.Text + "", "Dob"));
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
            dataGridView1.DataSource = _gt.GetRegisterPatientList("", 0);
            Gridwidth(1);
            Hlp.GridColor(dataGridView1);
          
            NameTextBox.Text = "";
            addressTextBox.Text = "";
            contactNoTextBox.Text = "";
            ptIdTextBox.Text = "0";
            contactNoTextBox.Focus();
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

        private void PatientRegistrationUi_Load(object sender, EventArgs e)
        {
            ClearText();
        }

        private void contactNoTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _gt.GetRegisterPatientList(contactNoTextBox.Text, 0);
            Gridwidth(1);
            Hlp.GridColor(dataGridView1);
        }

        private void contactNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                textinfo = "test";
                dataGridView1.Rows[0].Selected = true;

                dataGridView1.Focus();
            }
            if (e.KeyCode==Keys.Enter)
            {
                NameTextBox.Focus();
            }

        }

        private void genderComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dtDobTimePicker.Focus();
            }

        }

        private void dtDobTimePicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                addressTextBox.Focus();
            }

        }

        private void genderComboBox_Enter(object sender, EventArgs e)
        {
            genderComboBox.BackColor = Hlp.EnterFocus();
        }

        private void genderComboBox_Leave(object sender, EventArgs e)
        {
            genderComboBox.BackColor = Hlp.LeaveFocus();
        }

        private void dtDobTimePicker_Enter(object sender, EventArgs e)
        {
            dtDobTimePicker.BackColor = Hlp.EnterFocus();
        }

        private void dtDobTimePicker_Leave(object sender, EventArgs e)
        {
            dtDobTimePicker.BackColor = Hlp.LeaveFocus();
        }

 

       
    }
}
