using System;
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Gateway.Indoor;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Indoor;

namespace SuperPathologyApp.UI.Indoor
{
    public partial class BedUi : Form
    {
        readonly BedGateway _gt = new BedGateway();
        public BedUi()
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
                    dataGridView1.Columns[2].Width = 100;
                    dataGridView1.Columns[3].Width = 130;
                    dataGridView1.Columns[4].Width = 80;
                    dataGridView1.Columns[5].Width = 70;
                    dataGridView1.Columns[6].Visible = false;
                    dataGridView1.Columns[7].Visible = false;
                    dataGridView1.Columns[8].Width = 180;

                    break;

            }
          
        }


        private void drCodeTextBox_Leave(object sender, EventArgs e)
        {
            ptIdTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void drNameTextBox_Enter(object sender, EventArgs e)
        {
            floorNameTextBox.BackColor = Hlp.EnterFocus();
        }

        private void drNameTextBox_Leave(object sender, EventArgs e)
        {
            floorNameTextBox.BackColor = DbConnection.LeaveFocus(); 
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
                bedTypeComboBox.Focus();
            }
        }

  

        private void button2_Click(object sender, EventArgs e)
        {
            if (_gt.FnSeekRecordNewLab("tb_IN_DEPARTMENT","Id='"+ departmentComboBox.SelectedValue +"'")==false)
            {
                MessageBox.Show(@"Invalid Department", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                departmentComboBox.Focus();
                return;
            }


            if ((_gt.FnSeekRecordNewLab("tb_IN_BED","Name='"+ bedNoTextBox.Text +"'"))&&(ptIdTextBox.Text=="0"))
            {
                MessageBox.Show(@"Bed Name Already Exist", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bedNoTextBox.Focus();
                return;
            }
            
            
            if (bedNoTextBox.Text == "")
            {
                MessageBox.Show(@"Please Add Bed No", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bedNoTextBox.Focus();
                return;
            }
            if (floorNameTextBox.Text == "")
            {
                MessageBox.Show(@"Please Add Floor No", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                floorNameTextBox.Focus();
                return;
            }

            if (chargeTextBox.Text == "")
            {
                MessageBox.Show(@"Please Add Charge", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chargeTextBox.Focus();
                return;
            }
            if (serviceChargeTextBox.Text =="")
            {
                serviceChargeTextBox.Text = "0";
            }
            if (bedTypeComboBox.Text==@"--Select--")
            {
                MessageBox.Show(@"Please Bed Type", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bedTypeComboBox.Focus();
                return;
            }

            var mdl = new BedModel()
            {
                BedId = Convert.ToInt32(ptIdTextBox.Text ),
                Name = bedNoTextBox.Text,
                Floor = floorNameTextBox.Text,
                BedType= bedTypeComboBox.Text,
                Charge=Convert.ToDouble(chargeTextBox.Text),
                SrvCharge= Convert.ToDouble(serviceChargeTextBox.Text),
                Department = new DepartmentModel() { DeptId =Convert.ToInt32(departmentComboBox.SelectedValue)}
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
                        bedNoTextBox.Text = gcdesc;
                        var data=_gt.GetBedList(Convert.ToInt32(gccode), "");
                        if (data.Rows.Count>0)
                        {
                            floorNameTextBox.Text = data.Rows[0]["Floor"].ToString();
                            bedTypeComboBox.Text = data.Rows[0]["BedType"].ToString();
                            chargeTextBox.Text = data.Rows[0]["Charge"].ToString();
                            serviceChargeTextBox.Text = data.Rows[0]["SrvCharge"].ToString();
                            departmentComboBox.SelectedValue = data.Rows[0]["DeptId"].ToString();
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
            dataGridView1.DataSource = _gt.GetBedList( 0,"");
            Gridwidth(1);
           // Hlp.GridColor(dataGridView1);
          
            floorNameTextBox.Text = "";
            chargeTextBox.Text = "";
            bedNoTextBox.Text = "";
            serviceChargeTextBox.Text = "";
            ptIdTextBox.Text = "0";

            _gt.LoadComboBox("SELECT Distinct 0 AS Id,Name AS Description FROM tb_IN_BED_TYPE Order by Name", bedTypeComboBox);
            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_IN_DEPARTMENT Order by Id", departmentComboBox);
            bedNoTextBox.Focus();
        }

        private void addressTextBox_Enter(object sender, EventArgs e)
        {
            chargeTextBox.BackColor = Hlp.EnterFocus();
        }

        private void addressTextBox_Leave(object sender, EventArgs e)
        {
            chargeTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void contactNoTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }

       

        private void genderComboBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

      

        private void genderComboBox_Enter(object sender, EventArgs e)
        {
            bedTypeComboBox.BackColor = Hlp.EnterFocus();
        }

        private void genderComboBox_Leave(object sender, EventArgs e)
        {
            bedTypeComboBox.BackColor = Hlp.LeaveFocus();
        }

      

        private void bedNoTextBox_Enter(object sender, EventArgs e)
        {
            bedNoTextBox.BackColor = Hlp.EnterFocus();
        }

        private void bedNoTextBox_Leave(object sender, EventArgs e)
        {
            bedNoTextBox.BackColor = Hlp.LeaveFocus();
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
                floorNameTextBox.Focus();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bedTypeComboBox.Text !="--Select--")
            {
                if (_gt.FnSeekRecordNewLab("tb_IN_BED_TYPE", "Name='" + bedTypeComboBox.Text + "'") == false)
                {
                    _gt.DeleteInsertLab("INSERT INTO tb_IN_BED_TYPE(Name)VALUES('" + bedTypeComboBox.Text + "')");
                    _gt.LoadComboBox("SELECT Distinct 0 AS Id,Name AS Description FROM tb_IN_BED_TYPE Order by Name", bedTypeComboBox);
                }

            }
        }

        private void serviceChargeTextBox_Enter(object sender, EventArgs e)
        {
            serviceChargeTextBox.BackColor = Hlp.EnterFocus();
        }

        private void serviceChargeTextBox_Leave(object sender, EventArgs e)
        {
            serviceChargeTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void BedUi_Load(object sender, EventArgs e)
        {
            ClearText();
        }

 

       
    }
}
