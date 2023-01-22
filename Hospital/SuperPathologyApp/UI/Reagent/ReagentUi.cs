using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Gateway.Reagent;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Reagent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SuperPathologyApp.UI.Reagent
{
    public partial class ReagentUi : Form
    {
        public ReagentUi()
        {
            InitializeComponent();
        }
        ReagentGateway _gt = new ReagentGateway();
    

     




        private void button1_Click(object sender, EventArgs e)
        {
           

        }

        private void button10_Click(object sender, EventArgs e)
        {
            




        }

      

        private void frmGroupSetup_Activated(object sender, EventArgs e)
        {
           // testCodeTextBox.Focus(); 
        }

        private void groupNameComboBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

      
        public void RefreshText() 
        {

            //dataGridView2.DataSource = _gt.GetTestCodeList("");
         
       //     _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_Group Order By Id Desc", groupNameComboBox);
         //   _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_VaqGroup Order By Id Desc", vaqGroupNameComboBox);










        }

        private void groupNameComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = true;

            if (e.KeyChar == 13)
            {
               // vaqGroupNameComboBox.Focus();
                

            }

        }

  

        private void masterCodeTextBox_Enter(object sender, EventArgs e)
        {

        }

  

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            

          
        }



        private void vaqGroupNameComboBox_Leave(object sender, EventArgs e)
        {
            itemNameTextBox.BackColor = DbConnection.LeaveFocus();
        }


        private void masterCodeTextBox_Leave(object sender, EventArgs e)
        {
            itemNameTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void masterCodeTextBox_Enter_1(object sender, EventArgs e)
        {
            itemNameTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void testCodeTextBox_TextChanged(object sender, EventArgs e)
        {
           // dataGridView3.DataSource = _gt.GetTestCodeList(testCodeTextBox.Text);
           // _gt.GridColor(dataGridView3);
        }



        private void masterCodeTextBox_TextChanged(object sender, EventArgs e)
        {
           
            HelpDataGridLoadByReagent(dataGridView1, itemNameTextBox.Text);
        }

    

        private void testCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            


        }

   
        private void groupNameComboBox_KeyDown_1(object sender, KeyEventArgs e)
        {
           
        }

        private void vaqGroupNameComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                itemNameTextBox.Focus();
            }
        }

   



        private void testNameTextBox_Enter(object sender, EventArgs e)
        {
            itemNameTextBox.BackColor = Hlp.EnterFocus();
        }

        private void testNameTextBox_Leave(object sender, EventArgs e)
        {
            itemNameTextBox.BackColor = Hlp.LeaveFocus();

        }

       


        private void reportFileNameTextBox_Enter(object sender, EventArgs e)
        {
            reorderLevelTextBox.BackColor = Hlp.EnterFocus();
        }

        private void reportFileNameTextBox_Leave(object sender, EventArgs e)
        {
            reorderLevelTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (itemNameTextBox.Text == "")
            {
                MessageBox.Show(@"Please add Reagent Name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                itemNameTextBox.Focus();
                return;
            }

           
            

           
            var mdl = new ReagentModel
            {
                ReagentId =Convert.ToInt32(idTextBox.Text),
                Name = itemNameTextBox.Text,
                Type = typeComboBox.Text,
                DeptName= deptNameComboBox.Text,
                IsExpire = isExpireomboBox.SelectedIndex,
                ReorderLevel =Convert.ToInt32( reorderLevelTextBox.Text),

            };

            
            MessageBox.Show(_gt.Save(mdl), @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDg1("",2);
            idTextBox.Text = "0";
           

        }

        private void TestChartUi_Load(object sender, EventArgs e)
        {
            
        
        
        }

        private void LoadDg1(string search,int isVaq)
        {
            dataGridView1.DataSource = _gt.GetTestCodeList(0, search);
            Hlp.GridColor(dataGridView1);
          
        }

        string txtInfo = "";
        private void testNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Down)
            {
                txtInfo = "1";
                dataGridView1.Focus();
            }

            if (e.KeyCode == Keys.Enter)
            {
                if (itemNameTextBox.Text =="")
                {
                    MessageBox.Show(@"Invalid test name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    itemNameTextBox.Focus();
                    return;
                }
                
            }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string gccode = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[0].Value.ToString();
                 string gcdesc = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[1].Value.ToString();

                switch (txtInfo)
                {
                    case "1":
                        idTextBox.Text = gccode;
                        itemNameTextBox.Text = gcdesc;
                        GetTestById(idTextBox.Text);
                        itemNameTextBox.Focus();
                        break;
                   

                }

            }
        }

        private void GetTestById(string testId)
        {
            try
            {
                var dt = _gt.GetTestCodeList(Convert.ToInt32(testId), "");
                if (dt.Rows.Count > 0)
                {
                    idTextBox.Text= Convert.ToInt32(dt.Rows[0]["Id"]).ToString();
                    itemNameTextBox.Text= dt.Rows[0]["Name"].ToString();
                    typeComboBox.Text= dt.Rows[0]["Type"].ToString();
                    deptNameComboBox.Text= dt.Rows[0]["DeptName"].ToString();
                    isExpireomboBox.SelectedValue = Convert.ToInt32(dt.Rows[0]["IsExpire"]);
                    reorderLevelTextBox.Text = dt.Rows[0]["ReorderLevel"].ToString();
                }
              
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    

       
     

        private void giveDiscountComboBox_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void isVaqComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               deptNameComboBox.Focus();
            }
        }

        private void subProjectNameComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                reorderLevelTextBox.Focus();
            }
        }

        private void reportFileNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.Focus();
            }
        }

        private void defaultHnouriamPcTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
               
            }
        }

        private void honouriamStatusComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
              
            }
        }

        private void btnClear_Click(object sender, EventArgs e)//Aman
        {
            itemNameTextBox.Text = "";
            idTextBox.Text = "";
            reorderLevelTextBox.Text = "";
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                string id = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                if (_gt.FnSeekRecordNewLab("tb_BILL_DETAIL", "TestId='" + id + "'"))
                {
                    MessageBox.Show(@"This test has transaction can not delete!!", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='" + Hlp.UserName + "' AND ParentName='Setup' AND PermisionName='Test-Delete'"))
                    {
                        _gt.DeleteInsertLab("DELETE tb_TESTCHART WHERE  Id='" + id + "'");
                        _gt.DeleteInsertLab("DELETE tb_TESTCHART_PARAM WHERE  TestChartId='" + id + "'");
                        this.dataGridView1.Rows.RemoveAt(this.dataGridView1.CurrentCell.RowIndex);
                        MessageBox.Show(@"Test delete success.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(@"You need permision to do this task!!", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                    
                    
                    
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //if (_gt.FnSeekRecordNewLab("tb_TESTCHART_VAQ","Name='"+ vaqNameComboBox.Text +"'")==false)
            //{
            //    _gt.DeleteInsertLab("INSERT INTO tb_TESTCHART_VAQ(Name)VALUES('"+ vaqNameComboBox.Text  +"')");
            //    _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_TESTCHART_VAQ Order By Id", vaqNameComboBox);

            //}
        }

        private void dataGridView2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

     

        private void Reagent_Load(object sender, EventArgs e)
        {
          
            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_SubProject Order By Id", deptNameComboBox);
            deptNameComboBox.SelectedIndex = 0;
            typeComboBox.SelectedIndex = 1;
            txtInfo = "1";
            isExpireomboBox.SelectedIndex = 0;

            HelpDataGridLoadByReagent(dataGridView1, itemNameTextBox.Text);


        
        }
        public void HelpDataGridLoadByReagent(DataGridView dg, string search)
        {
            dg.DataSource = null;
            //var _gt = new TestChartGateway();
            dg.DataSource = _gt.GetTestCodeList(0, search);
            Hlp.GridFirstRowDeselect(dg);
            dg.Columns.OfType<DataGridViewColumn>().ToList().ForEach(col => col.Visible = false);
            // Hlp.GridColor(dg);
            dg.Columns[0].Width = 80;
            dg.Columns[1].Width = 200;
            dg.Columns[2].Width = 100;
            dg.Columns[3].Width = 100;


            dg.Columns[0].Visible = true;
            dg.Columns[1].Visible = true;
            dg.Columns[2].Visible = true;
            dg.Columns[3].Visible = true;


        }

        private void itemNameTextBox_TextChanged(object sender, EventArgs e)
        {
            HelpDataGridLoadByReagent(dataGridView1, itemNameTextBox.Text);
        }
  
    }
}
