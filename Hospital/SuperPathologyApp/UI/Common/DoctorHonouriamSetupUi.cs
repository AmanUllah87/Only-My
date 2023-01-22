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
    public partial class DoctorHonouriamSetupUi : Form
    {
        DoctorHonouriamSetupGateway _gt = new DoctorHonouriamSetupGateway();
        public DoctorHonouriamSetupUi()
        {
            InitializeComponent();
        }

        


        private void drCodeTextBox_Leave(object sender, EventArgs e)
        {
        }
        public string textinfo = "";

  

        private void button2_Click(object sender, EventArgs e)
        {
            if (_gt.FnSeekRecordNewLab("tb_Doctor", "Id='" + drIDTextBox.Text + "'") == false)
            {
                MessageBox.Show(@"Invalid Docotor Name Please Check", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var lists = new List<SubProjectModel>();

            for (int i = 0; i < dataGridView2.Rows.Count-1; i++)
            {
                //string result = dataGridView2.Rows[i].Cells[3].Value == null ? "" : dataGridView2.Rows[i].Cells[3].Value.ToString();
                    lists.Add(new SubProjectModel()
                    {
                        SubProjectId = Convert.ToInt32(dataGridView2.Rows[i].Cells[0].Value),
                        HnrAmt = Convert.ToDouble(dataGridView2.Rows[i].Cells[2].Value),
                        Type = dataGridView2.Rows[i].Cells[3].Value.ToString(),
                    });
            }

            var mdl = new DoctorModel
            {
                DrId =Convert.ToInt32(drIDTextBox.Text),
                SubProject = lists,

            };

            string msg =  _gt.Save(mdl);
            if (msg == "Saved Success")
            {
                MessageBox.Show(msg, @"Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                drIDTextBox.Text = "";
                drNameTextBox.Text = "";
                dataGridView2.Rows.Clear();
                drNameTextBox.Focus();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            drIDTextBox.Text = "";
            drNameTextBox.Text = "";
            dataGridView2.Rows.Clear();
            drNameTextBox.Focus();
        }

   

       
        private void DoctorSetupUiNew_Load(object sender, EventArgs e)
        {
            

        
            


        }

        private void addressTextBox_Enter(object sender, EventArgs e)
        {
            
        }

        private void addressTextBox_Leave(object sender, EventArgs e)
        {
            
        }


        private void contactNoTextBox_Enter(object sender, EventArgs e)
        {
           
        }

        private void contactNoTextBox_Leave(object sender, EventArgs e)
        {
           
        }

        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            
        }

        private void dataGridView2_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //if (e.ColumnIndex == 2) 
            //{
            //    int i;

            //    if (!int.TryParse(Convert.ToString(e.FormattedValue), out i))
            //    {
            //        e.Cancel = true;
            //    }
            //}
        }
        bool isFlag = false;
        private void DoctorHonouriamSetupUi_Load(object sender, EventArgs e)
        {
        }

        private void GetProjectList(int drId)
        {
            dataGridView2.Rows.Clear();

            var data = _gt.GetProjectList(drId);
            foreach (var mdl in data)
            {
                dataGridView2.Rows.Add(mdl.SubProjectId, mdl.Name, mdl.HnrAmt, mdl.Type);
            }

            Hlp.GridColor(dataGridView2);
           // drIdComboBox.SelectedIndex = 0;


            //
            dataGridView2.Columns[1].ReadOnly = true;
            dataGridView2.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.Columns[0].ReadOnly = true;
        }

        private void drIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void drIdComboBox_Enter(object sender, EventArgs e)
        {
            isFlag = true;
        }

        private void drIdComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                isFlag = true;
                if (isFlag)
                {
                    GetProjectList(Convert.ToInt32(drIDTextBox.Text));
                    isFlag = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                GetProjectList(Convert.ToInt32(drIDTextBox.Text));

        }

        private void drNameTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _gt.GetDoctorList(0, drNameTextBox.Text);
            Gridwidth(1);
        }
        public void Gridwidth(int no)
        {
            switch (no)
            {
                case 1:
                    dataGridView1.Columns[0].Width = 70;
                    dataGridView1.Columns[1].Width = 400;
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[3].Visible = false;

                    dataGridView1.Columns[4].Visible = false;
                    dataGridView1.Columns[5].Visible = false;
                    dataGridView1.Columns[6].Visible = false;
                    dataGridView1.Columns[7].Visible = false;
                    dataGridView1.Columns[8].Visible = false;

                    break;

            }

        }

        private void drNameTextBox_Enter(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _gt.GetDoctorList(0, drNameTextBox.Text);
            Gridwidth(1);
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
                        drIDTextBox.Text = gccode;
                        drNameTextBox.Text = gcdesc;
                        GetProjectList(Convert.ToInt32(drIDTextBox.Text));

                        break;
                }

            }
        }

        private void drNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                textinfo = "1";
                dataGridView1.Focus();
            }
        }
    }
}
