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
    public partial class DoctorSetupUi : Form
    {
        LabDoctorGateway _gt = new LabDoctorGateway();
        public DoctorSetupUi()
        {
            InitializeComponent();
        }


        private void frmDoctorSetup_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _gt.GetDoctorList(0, drCodeTextBox.Text);
            Gridwidth(1);
            GridColor(dataGridView1);


            doctorTypeComboBox.Items.Add("Left");
            doctorTypeComboBox.Items.Add("Middle");
            doctorTypeComboBox.Items.Add("Right");
            doctorTypeComboBox.SelectedIndex = 0;




        }
        private void GridColor(DataGridView dg)
        {
            int i = 0;
            while (i < dg.Rows.Count)
            {
                dg.Rows[i].DefaultCellStyle.BackColor = Color.Azure;
                i += 2;
            }
        }
        public void Gridwidth(int no)
        {
            switch (no)
            {
                case 1:
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Width = 180;
                    dataGridView1.Columns[2].Width = 232;
                    break;

            }
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.CurrentCell = null;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeRows = false;







        }
    

        private void drCodeTextBox_Leave(object sender, EventArgs e)
        {
            drCodeTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void drNameTextBox_Enter(object sender, EventArgs e)
        {
            drNameTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void drNameTextBox_Leave(object sender, EventArgs e)
        {
            drNameTextBox.BackColor = DbConnection.LeaveFocus(); 
        }

        private void drDetailsRichTextBox_Enter(object sender, EventArgs e)
        {
            drDetailsRichTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void drDetailsRichTextBox_Leave(object sender, EventArgs e)
        {
            drDetailsRichTextBox.BackColor = DbConnection.LeaveFocus();
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
        }

  

        private void button2_Click(object sender, EventArgs e)
        {
            if (drNameTextBox.Text == "")
            {
                MessageBox.Show(@"Please Add Doctor Name");
                return;
            }

            int drId = 0;
            if (drCodeTextBox.Text!="")
            {
                drId =Convert.ToInt32(drCodeTextBox.Text);
            }


            var mdl = new DoctorModel
            {
                DrId =drId,
                Name = drNameTextBox.Text,
                Address = drDetailsRichTextBox.Text,
                ContactNo = doctorTypeComboBox.Text
            };

            string msg = "";
            
            if (_gt.FnSeekRecordNewLab("tb_LAB_DOCTOR","Id="+ mdl.DrId +"")==false)
	        {
                 msg = _gt.Save(mdl);
	        }
            else
            {
               
                 msg = _gt.Save(mdl);
               
            }

            Clear();
            MessageBox.Show(msg, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);












               
            
          
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();
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
                        drDetailsRichTextBox.Text = _gt.FncReturnFielValueLab("tb_LAB_DOCTOR", "Id=" + drCodeTextBox.Text + "", "Degree");
                        doctorTypeComboBox.Text = _gt.FncReturnFielValueLab("tb_LAB_DOCTOR", "Id=" + drCodeTextBox.Text + "", "Position");
                        
                        drDetailsRichTextBox.Focus();
                        break;
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            drCodeTextBox.Text = "";
            drNameTextBox.Text = "";
            drDetailsRichTextBox.Text = "";
            doctorTypeComboBox.SelectedIndex = 0;
            drNameTextBox.Focus();
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
                        _gt.DeleteInsertLab("DELETE tb_LAB_DOCTOR  WHERE Id=" + drId + "");
                        MessageBox.Show(@"Delete Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }







            }


        }
    }
}
