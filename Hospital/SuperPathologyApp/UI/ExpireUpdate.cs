using System;
using System.Drawing;
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;

namespace SuperPathologyApp.UI
{
    public partial class ExpireUpdate : Form
    {

        readonly ExpenseVoucherGateway _gtTestCode = new ExpenseVoucherGateway();
        public string Textinfo = "";
        private bool _isEdit = false;


        public ExpireUpdate()
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
                   
                    break;
               
             

            }
          
           
            
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








        private void button4_Click(object sender, EventArgs e)
        {




        }

  

        private void expenseNameTextBox_TextChanged(object sender, EventArgs e)
        {
           // GridShow(expenseNameTextBox.Text, Convert.ToInt32(departmentComboBox.SelectedValue), Convert.ToInt32(subDeptComboBox.SelectedValue));
        }

     

      

        private void ExpireUpdate_Load(object sender, EventArgs e)
        {

        }
       
    }
}
