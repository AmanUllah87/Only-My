using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
 
using System.Windows.Forms;

namespace SuperPathologyApp.UI
{
    public partial class ChannelSetupUi : Form
    {

        ChannelSetupGateway _gt = new ChannelSetupGateway();
        GroupSetupGateway gtTestCode = new GroupSetupGateway();
        public string textinfo = "";
        private bool IsEdit = false;


        public ChannelSetupUi()
        {
            InitializeComponent();
        }

        private void testCodeTextBox_Enter(object sender, EventArgs e)
        {
            testCodeTextBox.BackColor = DbConnection.EnterFocus();
           
        }

       

        

        private void testCodeTextBox_Leave(object sender, EventArgs e)
        {
            testCodeTextBox.BackColor = DbConnection.LeaveFocus();
        }

        

    

        private void calculativeNumberTextBox_Enter(object sender, EventArgs e)
        {
            calculativeNumberTextBox.BackColor = DbConnection.EnterFocus();  
        }


        private void unitComboBox_Enter(object sender, EventArgs e)
        {
            groupNameComboBox.BackColor = DbConnection.EnterFocus();  
        }

        private void specimenComboBox_Enter(object sender, EventArgs e)
        {
            machineNameComboBox.BackColor = DbConnection.EnterFocus();  
        }

    

        private void specimenComboBox_Leave(object sender, EventArgs e)
        {
            machineNameComboBox.BackColor = DbConnection.LeaveFocus();  
        }

        private void unitComboBox_Leave(object sender, EventArgs e)
        {
            groupNameComboBox.BackColor = DbConnection.LeaveFocus();  
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
                case 2:
                    dataGridView3.Columns[0].Width = 100;
                    dataGridView3.Columns[1].Width = 90;
                    dataGridView3.Columns[2].Width = 90;
                    dataGridView3.Columns[3].Width = 100;
                    dataGridView3.Columns[4].Width = 150;
                   
                    //dataGridView3.Columns[8].Width = 45;
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

     

        private void PopulateGridViewData()
        {
            dataGridView3.Rows.Clear();
            var data = _gt.GetParameterDetailsByTestCode(testCodeTextBox.Text,machineNameComboBox.Text);
                    //machineNameComboBox.Text = data[0].MachineName;
            foreach (var mdl in data)
            {
                dataGridView3.Rows.Add(mdl.ParameterName, mdl.ParameterTestName, mdl.PtType, mdl.MachineName, mdl.ReportingGroupName);
            }
            GridColor(dataGridView3);

            dataGridView3.RowHeadersVisible = false;
            dataGridView3.CurrentCell = null;



            dataGridView3.Columns[0].HeaderText = @"ReagentName";
            dataGridView3.Columns[1].HeaderText = @"Parameter"; ;
            dataGridView3.Columns[2].HeaderText = @"ParamType"; ;
            dataGridView3.Columns[3].HeaderText = @"MachineName"; ;
            dataGridView3.Columns[4].HeaderText = @"GroupName"; ;







        }

        private void testCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                textinfo = "1";
                dataGridView1.Focus();
            }

            if (e.KeyCode==Keys.Enter)
            {
                if (_gt.FnSeekRecordNewLab("ChannelDefination ", "Pcode='" + testCodeTextBox.Text + "'"))
                {
                    dataGridView3.Rows.Clear();
                    PopulateGridViewData();
                    machineNameComboBox.Focus();
                }
                else
                {
                    MessageBox.Show(@"Invalid Item Please Check", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);  
                }
            }



        }

        private void testCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = gtTestCode.GetTestCodeList(testCodeTextBox.Text);
            GridColor(dataGridView1);
        }




     


     
        private void parameterNameTextBox_Enter(object sender, EventArgs e)
        {
            reagentNameTextBox.BackColor = DbConnection.EnterFocus();
            dataGridView1.DataSource = _gt.GetChannelData("");
            GridColor(dataGridView1);









        }



        private void parameterNameTextBox_Leave(object sender, EventArgs e)
        {
            reagentNameTextBox.BackColor = DbConnection.LeaveFocus();
        }


 


        private void specimenComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            


        }
        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (testCodeTextBox.Text != "")
            {
               
               
                if (IsEdit==false)
                {
                    if (_gt.IsDuplicate(reagentNameTextBox.Text, dataGridView3) == false)
                    {
                        MessageBox.Show(@"Already Exist Item Please Check", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        dataGridView3.Rows.Add(reagentNameTextBox.Text, parameterNameTextBox.Text, parameterType.Text, machineNameComboBox.Text, groupNameComboBox.Text);
                    }
                }
                else
                {
                    if (IsEdit)
                    {
                        dataGridView3.CurrentRow.Cells[0].Value = reagentNameTextBox.Text;
                        dataGridView3.CurrentRow.Cells[1].Value = parameterNameTextBox.Text;
                        dataGridView3.CurrentRow.Cells[2].Value = parameterType.Text;
                        dataGridView3.CurrentRow.Cells[3].Value = machineNameComboBox.Text;
                        dataGridView3.CurrentRow.Cells[4].Value = groupNameComboBox.Text;
                        IsEdit = false;
                    }
                }
                GridColor(dataGridView3);
               // MessageBox.Show(@"Added Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearAdd();
            }
        }

        private void ClearAdd()
        {
            reagentNameTextBox.Text = "";
            parameterNameTextBox.Text = "";

            reagentNameTextBox.Focus();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (testCodeTextBox.Text == "")
            {
                MessageBox.Show(@"Please Add Test Code", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (machineNameComboBox.Text == "")
            {
                MessageBox.Show(@"Please Add MachineName", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dataGridView3.Rows.Count < 1)
            {
                MessageBox.Show(@"Please Add Parameter Name", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var mdl = new TestCodeModel
            {
                TestCode = testCodeTextBox.Text,
                TestName = testNameTextBox.Text,
                MachineName= machineNameComboBox.Text
            };
            string msg = _gt.SaveParameter(mdl, dataGridView3);
            MessageBox.Show(msg, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                switch (textinfo)
                {
                    case "1":
                        testCodeTextBox.Text = gccode;
                        testNameTextBox.Text = gcdesc;
                        dataGridView3.Rows.Clear();
                        if (_gt.FnSeekRecordNewLab("ChannelDefination ", "Pcode='" + testCodeTextBox.Text + "'"))
                        {
                            PopulateGridViewData();
                        }
                        machineNameComboBox.Focus();
                        break;
                    case "2":
                        reagentNameTextBox.Text = gccode;
                        parameterNameTextBox.Focus();
                        break;
                    case "3":
                        parameterNameTextBox.Text = gccode;
                        groupNameComboBox.Focus();
                        break;
                }
            }
        }

        private void ParameterSetupUi_Activated(object sender, EventArgs e)
        {
            testCodeTextBox.Focus();
        }

        private void ChannelSetupUi_Load(object sender, EventArgs e)
        {
            _gt.LoadComboBox("SELECT distinct 0 AS Id, MachineName AS Description FROM ChannelDefination", machineNameComboBox);
            _gt.LoadComboBox("SELECT Distinct 0 AS Id,GroupName AS  Description FROM ChannelDefination", groupNameComboBox);
            
            dataGridView1.DataSource = gtTestCode.GetTestCodeList("");
            Gridwidth(1);
            Gridwidth(2);
            GridColor(dataGridView1);
            testCodeTextBox.Focus();
        }

        private void reagentNameTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _gt.GetChannelData(reagentNameTextBox.Text);
            GridColor(dataGridView1);
        }

        private void parameterNameTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _gt.GetParamData(parameterNameTextBox.Text );
            GridColor(dataGridView1);

        }

        private void parameterNameTextBox_Enter_1(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _gt.GetParamData("");
            GridColor(dataGridView1);

        }

        private void reagentNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                textinfo = "2";
                dataGridView1.Focus();
            }
        }

        private void parameterNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                textinfo = "3";
                dataGridView1.Focus();
            }
        }

        private void groupNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupNameComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                parameterType.Focus();
            }
            
        }

        private void parameterType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAdd.Focus();
            }
        }

        private void machineNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var data= _gt.GetChannelListByMachineName(machineNameComboBox.Text);

            //dataGridView3.Rows.Clear();
            //foreach (var mdl in data)
            //{
            //    dataGridView3.Rows.Add(mdl.PCode, mdl.ParameterName, mdl.ParameterTestName, mdl.PtType, mdl.ReportingGroupName);
            //}
            //GridColor(dataGridView3);

            //dataGridView3.RowHeadersVisible = false;
            //dataGridView3.CurrentCell = null;

            //dataGridView3.Columns[0].Width = 80;
            //dataGridView3.Columns[1].Width = 100;

            //dataGridView3.Columns[0].HeaderText =@"PCode";
            //dataGridView3.Columns[1].HeaderText = @"Reagent"; ;
            //dataGridView3.Columns[2].HeaderText = @"Parameter"; ;
            //dataGridView3.Columns[4].HeaderText = @"GroupName"; ;

            //dataGridView3.Columns[1].Width = 45;


        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

       
        private void dataGridView3_DoubleClick(object sender, EventArgs e)
        {
            IsEdit = true;


            string reagentName = dataGridView3.CurrentRow.Cells[0].Value.ToString();
            string parametreName = dataGridView3.CurrentRow.Cells[1].Value.ToString();
            string groupName = dataGridView3.CurrentRow.Cells[4].Value.ToString();
            string paramType = dataGridView3.CurrentRow.Cells[2].Value.ToString();





            groupNameComboBox.Text = groupName;
            reagentNameTextBox.Text = reagentName;
            parameterNameTextBox.Text = parametreName;
            parameterType.Text = paramType;

        }


        private void dataGridView3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                int rowindex = dataGridView3.CurrentCell.RowIndex;
                if (dataGridView3.CurrentRow != null)
                {
                  //  int id = Convert.ToInt32(dataGridView3.CurrentRow.Cells[0].Value);
                    dataGridView3.Rows.RemoveAt(rowindex);
                }
            }
        }

        private void machineNameComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                reagentNameTextBox.Focus();
                PopulateGridViewData();
                
              
            }
        }
       
    }
}
