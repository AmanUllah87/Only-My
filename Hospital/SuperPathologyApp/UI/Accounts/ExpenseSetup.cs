using System;
using System.Drawing;
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;

namespace SuperPathologyApp.UI.Accounts
{
    public partial class ExpenseSetup : Form
    {

        readonly ExpenseVoucherGateway _gtTestCode = new ExpenseVoucherGateway();
        public string Textinfo = "";
        private bool _isEdit = false;


        public ExpenseSetup()
        {
            InitializeComponent();
        }

        private void testCodeTextBox_Enter(object sender, EventArgs e)
        {
            expenseNameTextBox.BackColor = DbConnection.EnterFocus();
           
        }

       

        

        private void testCodeTextBox_Leave(object sender, EventArgs e)
        {
            expenseNameTextBox.BackColor = DbConnection.LeaveFocus();
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
                    dataGridView1.Columns[0].Width = 50;
                    dataGridView1.Columns[1].Width = 303;
                    break;
               
             

            }
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.CurrentCell = null;
            
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeRows = false;
           
            
            //dataGridView3.EnableHeadersVisualStyles = false;
            //dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            //dataGridView3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dataGridView3.AllowUserToResizeRows = false;



        }

     

       

        private void testCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                Textinfo = "1";
                dataGridView1.Focus();
            }

            //if (e.KeyCode==Keys.Enter)
            //{
            //    if (_gt.FnSeekRecordNewLab("ChannelDefination ", "Pcode='" + expenseNameTextBox.Text + "'"))
            //    {
            //        dataGridView3.Rows.Clear();
            //        PopulateGridViewData();
                   
            //    }
            //    else
            //    {
            //        MessageBox.Show(@"Invalid Item Please Check", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);  
            //    }
            //}



        }

        private void GridShow(string search,int deptId,int subDeptId,string type)
        {
            if (departmentComboBox.Text != "" || departmentComboBox.Text != "--Select--")
            {
                dataGridView1.DataSource = _gtTestCode.GetTestCodeList(search, deptId,subDeptId,type);
            }
            else
            {
                dataGridView1.DataSource = _gtTestCode.GetTestCodeList(expenseNameTextBox.Text, 0,0,type);
            }
            GridColor(dataGridView1);
            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns[1].Width = 120;
            dataGridView1.Columns[2].Width = 130;
            dataGridView1.Columns[3].Width = 200;
            dataGridView1.Rows[0].Selected = false;

            //dataGridView1.Columns[4].Visible = false;
            //dataGridView1.Columns[5].Visible = false;
            //dataGridView1.Columns[6].Visible = false;
            //dataGridView1.Columns[7].Visible = false;
        }




     


     
       

      

 


        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (expenseNameTextBox.Text != "")
            {
                
               
                if (_isEdit==false)
                {
                    //if (_gt.IsDuplicate(reagentNameTextBox.Text, dataGridView3) == false)
                    //{
                    //    MessageBox.Show(@"Already Exist Item Please Check", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //}
                    //else
                    //{
                    //   // dataGridView3.Rows.Add(reagentNameTextBox.Text, parameterNameTextBox.Text, parameterType.Text, machineNameComboBox.Text, groupNameComboBox.Text);
                    //}
                }
                else
                {
                    if (_isEdit)
                    {
                        //dataGridView3.CurrentRow.Cells[0].Value = reagentNameTextBox.Text;
                        //dataGridView3.CurrentRow.Cells[1].Value = parameterNameTextBox.Text;
                        //dataGridView3.CurrentRow.Cells[2].Value = parameterType.Text;
                        //dataGridView3.CurrentRow.Cells[3].Value = machineNameComboBox.Text;
                        //dataGridView3.CurrentRow.Cells[4].Value = groupNameComboBox.Text;
                        _isEdit = false;
                    }
                }
              
               // MessageBox.Show(@"Added Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearAdd();
            }
        }

        private void ClearAdd()
        {
            //reagentNameTextBox.Text = "";
            //parameterNameTextBox.Text = "";

            //reagentNameTextBox.Focus();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (expenseNameTextBox.Text == "")
            {
                MessageBox.Show(@"Please Add Test Code", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
          
           
            var mdl = new TestCodeModel
            {
                TestCode = expenseNameTextBox.Text,
               
            };
            //string msg = _gtTestCode.SaveParameter(mdl, dataGridView3);
            //MessageBox.Show(msg, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void button2_Click(object sender, EventArgs e)
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

                switch (Textinfo)
                {
                    case "1":
                        subDeptComboBox.SelectedValue = _gtTestCode.FncReturnFielValueLab("tb_ac_HEAD", "Code='" + gccode + "'", "SubDeptId");
                        departmentComboBox.SelectedValue = _gtTestCode.FncReturnFielValueLab("tb_ac_SUB_DEPARTMENT", "Id='"+ subDeptComboBox.SelectedValue +"'", "DeptId");
                        expenseNameTextBox.Text = _gtTestCode.FncReturnFielValueLab("tb_ac_HEAD", "Code='" + gccode + "'", "Name");
                        acCodeTextBox.Text = gccode;
                        expenseNameTextBox.Focus();
                        button4.Text = "Update";

                        break;
                   
                }
            }
        }

    
     
    
       
        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

       
      

        

        

        private void ExpenseVoucher_Load(object sender, EventArgs e)
        {
            _gtTestCode.LoadComboBox("SELECT Id, Name AS Description FROM tb_ac_DEPARTMENT", departmentComboBox);
            _gtTestCode.LoadComboBox("SELECT Id,Name AS Description FROM tb_ac_SUB_DEPARTMENT Order By Id", subDeptComboBox);

            GridShow(expenseNameTextBox.Text, Convert.ToInt32(departmentComboBox.SelectedValue), Convert.ToInt32(subDeptComboBox.SelectedValue),typeComboBox.Text);
            typeComboBox.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (departmentComboBox.Text == "--Select--" || departmentComboBox.Text == "")
            {
                return;
            }

            if (_gtTestCode.FnSeekRecordNewLab("tb_ac_DEPARTMENT", "Name='" + departmentComboBox.Text.Trim() + "'") == false)
            {
                _gtTestCode.DeleteInsertLab("INSERT INTO tb_ac_DEPARTMENT(Id,Name)VALUES('" + Hlp.GetAutoIncrementVal("Id", "tb_ac_DEPARTMENT", 2) + "','" + departmentComboBox.Text.Trim() + "')");
                MessageBox.Show(@"Added Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _gtTestCode.LoadComboBox("SELECT Id,Name AS Description FROM tb_ac_DEPARTMENT Order By Id", departmentComboBox);

            }
            else
            {
                MessageBox.Show(@"Already exists please check.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_gtTestCode.FnSeekRecordNewLab("tb_ac_DEPARTMENT", "Id='" + Convert.ToInt32(departmentComboBox.SelectedValue) + "'") == false)
            {
                MessageBox.Show(@"Please select a valid department", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_gtTestCode.FnSeekRecordNewLab("tb_ac_SUB_DEPARTMENT", "Id='" + Convert.ToInt32(subDeptComboBox.SelectedValue) + "'") == false)
            {
                MessageBox.Show(@"Please select a valid sub department", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (button4.Text=="Update")
            {
                if (_gtTestCode.FnSeekRecordNewLab("tb_ac_HEAD","Code='"+ acCodeTextBox.Text +"'")==false)
                {
                    MessageBox.Show(@"Invalid Code", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    departmentComboBox.Focus();
                    return;
                }
                
                if (_gtTestCode.FnSeekRecordNewLab("tb_ac_HEAD", "DeptId='" + Convert.ToInt32(departmentComboBox.SelectedValue) + "' AND SubDeptId='" + Convert.ToInt32(subDeptComboBox.SelectedValue) + "' AND Type='" + typeComboBox.Text + "' AND Code='"+ acCodeTextBox.Text +"'"))
                {
                    _gtTestCode.DeleteInsertLab("UPDATE tb_ac_HEAD SET Name='" + expenseNameTextBox.Text.Trim() + "' WHERE DeptId='" + Convert.ToInt32(departmentComboBox.SelectedValue) + "' AND SubDeptId='" + Convert.ToInt32(subDeptComboBox.SelectedValue) + "' AND Type='" + typeComboBox.Text + "' AND Code='" + acCodeTextBox.Text + "'");
                    MessageBox.Show(@"Update Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    acCodeTextBox.Text = "";
                    return;
                }
            }









            if (_gtTestCode.FnSeekRecordNewLab("tb_ac_HEAD", "DeptId='" + Convert.ToInt32(departmentComboBox.SelectedValue) + "' AND SubDeptId='" + Convert.ToInt32(subDeptComboBox.SelectedValue) + "' AND Name='" + expenseNameTextBox.Text + "' AND Type='" + typeComboBox.Text + "'") == false)
            {
                var expId = Hlp.GetAutoIncrementVal("Id", "tb_ac_HEAD", 2);
                var deptId = Convert.ToInt32(departmentComboBox.SelectedValue).ToString().PadLeft(2, '0');
                var subdeptId = Convert.ToInt32(subDeptComboBox.SelectedValue).ToString().PadLeft(2, '0');
                var code = deptId + subdeptId + expId;
                _gtTestCode.DeleteInsertLab("INSERT INTO tb_ac_HEAD(Id,DeptId,SubDeptId,Name,Code,Type)VALUES('" + expId + "','" + Convert.ToInt32(departmentComboBox.SelectedValue) + "','" + Convert.ToInt32(subDeptComboBox.SelectedValue) + "','" + expenseNameTextBox.Text.Trim() + "','" + code + "','"+ typeComboBox.Text +"')");
                MessageBox.Show(@"Added Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(@"Already exists please check.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (subDeptComboBox.Text == "--Select--" || subDeptComboBox.Text == "")
            {
                return;
            }
            if (_gtTestCode.FnSeekRecordNewLab("tb_ac_DEPARTMENT", "Id='" + Convert.ToInt32(departmentComboBox.SelectedValue) + "'") == false)
            {
                MessageBox.Show(@"Please select a valid department", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_gtTestCode.FnSeekRecordNewLab("tb_ac_SUB_DEPARTMENT", "DeptId=" + Convert.ToInt32(departmentComboBox.SelectedValue) + " AND  Name='" + subDeptComboBox.Text.Trim() + "'") == false)
            {
                _gtTestCode.DeleteInsertLab("INSERT INTO tb_ac_SUB_DEPARTMENT(Id,DeptId,Name)VALUES('" + Hlp.GetAutoIncrementVal("Id", "tb_ac_SUB_DEPARTMENT", 2) + "','" + Convert.ToInt32(departmentComboBox.SelectedValue) + "','" + subDeptComboBox.Text.Trim() + "')");
                MessageBox.Show(@"Added Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _gtTestCode.LoadComboBox("SELECT Id,Name AS Description FROM tb_ac_SUB_DEPARTMENT Order By Id", subDeptComboBox);

            }
            else
            {
                MessageBox.Show(@"Already exists please check.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void expenseNameTextBox_TextChanged(object sender, EventArgs e)
        {
            GridShow(expenseNameTextBox.Text, Convert.ToInt32(departmentComboBox.SelectedValue), Convert.ToInt32(subDeptComboBox.SelectedValue),typeComboBox.Text);
        }

        private void departmentComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (departmentComboBox.SelectedIndex!=0)
            {
                _gtTestCode.LoadComboBox("SELECT Id,Name AS Description FROM tb_ac_SUB_DEPARTMENT WHERE DeptId='"+ departmentComboBox.SelectedValue +"' Order By Id", subDeptComboBox);
                GridShow(expenseNameTextBox.Text, Convert.ToInt32(departmentComboBox.SelectedValue), Convert.ToInt32(subDeptComboBox.SelectedValue),typeComboBox.Text);
            }
        }

        private void subDeptComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (subDeptComboBox.SelectedIndex != 0)
            {
                GridShow(expenseNameTextBox.Text, Convert.ToInt32(departmentComboBox.SelectedValue), Convert.ToInt32(subDeptComboBox.SelectedValue),typeComboBox.Text);
            }
        }
       
    }
}
