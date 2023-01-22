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
    public partial class ParameterSetupUi : Form
    {

        ParameterSetupGateway _gt = new ParameterSetupGateway();
        GroupSetupGateway gtTestCode = new GroupSetupGateway();
        public string textinfo = "";
        private bool IsEdit = false;
        
        
        public ParameterSetupUi()
        {
            InitializeComponent();
        }

        private void testCodeTextBox_Enter(object sender, EventArgs e)
        {
            testCodeTextBox.BackColor = DbConnection.EnterFocus();
            dataGridView1.DataSource = gtTestCode.GetTestCodeList(testCodeTextBox.Text);
            GridColor(dataGridView1);
            Gridwidth(1);
            Gridwidth(2);
          
        }

       

        

        private void testCodeTextBox_Leave(object sender, EventArgs e)
        {
            testCodeTextBox.BackColor = DbConnection.LeaveFocus();
        }

        

        private void unitNameComboBox_Enter(object sender, EventArgs e)
        {
            unitNameComboBox.BackColor = DbConnection.EnterFocus();
            textinfo = "Unit";
            dataGridView1.DataSource = gtTestCode.GetList(unitNameComboBox.Text, textinfo);
            GridColor(dataGridView1);
            dataGridView1.Columns[0].Width = 250;
        }

        private void unitNameComboBox_Leave(object sender, EventArgs e)
        {
            unitNameComboBox.BackColor = DbConnection.LeaveFocus();  
        }

        private void resultTextBox_Enter(object sender, EventArgs e)
        {
            resultTextBox.BackColor = DbConnection.EnterFocus();
            textinfo = "DefaultResult";
            dataGridView1.DataSource = gtTestCode.GetList(resultTextBox.Text, textinfo);
            GridColor(dataGridView1);
            dataGridView1.Columns[0].Width = 250;
        }

        private void calculativeNumberTextBox_Enter(object sender, EventArgs e)
        {
            calculativeNumberTextBox.BackColor = DbConnection.EnterFocus();  
        }

        private void normalValueRichTextBox_Enter(object sender, EventArgs e)
        {
            normalValueRichTextBox.BackColor = DbConnection.EnterFocus();  
        }

        private void unitComboBox_Enter(object sender, EventArgs e)
        {
            reportingGroupNameComboBox.BackColor = DbConnection.EnterFocus();  
        }

        private void specimenComboBox_Enter(object sender, EventArgs e)
        {
           // specimenComboBox.BackColor = DbConnection.EnterFocus();  
        }

        private void groupSerialTextBox_Enter(object sender, EventArgs e)
        {
            groupSerialTextBox.BackColor = DbConnection.EnterFocus();  
        }

        private void parameterSerialTextBox_Enter(object sender, EventArgs e)
        {
            parameterSerialTextBox.BackColor = DbConnection.EnterFocus();
            dataGridView1.DataSource = gtTestCode.GetTestCodeList(testCodeTextBox.Text);
            GridColor(dataGridView1);
            textinfo = "MP";
        }

        private void parameterSerialTextBox_Leave(object sender, EventArgs e)
        {
            parameterSerialTextBox.BackColor = DbConnection.LeaveFocus();  
        }

        private void groupSerialTextBox_Leave(object sender, EventArgs e)
        {
            groupSerialTextBox.BackColor = DbConnection.LeaveFocus();  
        }

        private void specimenComboBox_Leave(object sender, EventArgs e)
        {
           // specimenComboBox.BackColor = DbConnection.LeaveFocus();  
        }

        private void unitComboBox_Leave(object sender, EventArgs e)
        {
            reportingGroupNameComboBox.BackColor = DbConnection.LeaveFocus();  
        }

        private void normalValueRichTextBox_Leave(object sender, EventArgs e)
        {
            normalValueRichTextBox.BackColor = DbConnection.LeaveFocus();  
        }

        private void resultTextBox_Leave(object sender, EventArgs e)
        {
            resultTextBox.BackColor = DbConnection.LeaveFocus();  
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
        private void frmParameterSetup_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = gtTestCode.GetTestCodeList("");
            Gridwidth(1);
            Gridwidth(2);
            GridColor(dataGridView1);

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
                    dataGridView3.Columns[0].Width = 35;
                    dataGridView3.Columns[1].Width = 35;
                    dataGridView3.Columns[2].Width = 80;
                    dataGridView3.Columns[3].Width = 130;
                    dataGridView3.Columns[4].Width = 200;
                    dataGridView3.Columns[5].Width = 100;
                    dataGridView3.Columns[6].Width = 120;
                    dataGridView3.Columns[7].Width = 100;
                    dataGridView3.Columns[8].Width = 50;
                    dataGridView3.Columns[9].Width = 70;
                    dataGridView3.Columns[10].Width = 70;

                    //dataGridView3.Columns[8].Width = 45;
                    break;

            }
          //  dataGridView1.RowHeadersVisible = false;
            dataGridView1.CurrentCell = null;
            
          //  dataGridView1.EnableHeadersVisualStyles = false;
          ////  dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
          //  dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
          //  dataGridView1.AllowUserToResizeRows = false;
          //  dataGridView3.EnableHeadersVisualStyles = false;
          ////  dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
          //  dataGridView3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
          //  dataGridView3.AllowUserToResizeRows = false;



        }

     

        private void PopulateGridViewData()
        {
            var data = _gt.GetParameterDetailsByTestCode(testCodeTextBox.Text);
            specimenTextBox.Text = data[0].Specimen;


            foreach (var mdl in data)
            {
                dataGridView3.Rows.Add(mdl.GroupSl, mdl.ParamSl, mdl.MachineParam, mdl.ReportParam, mdl.NormalRange, mdl.Unit,mdl.ReportingGroup, mdl.DefaultResult, mdl.IsBold,mdl.LowerVal,mdl.UpperVal);
            }
            GridColor(dataGridView3);

            dataGridView3.RowHeadersVisible = false;
            dataGridView3.CurrentCell = null;











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
                if (_gt.FnSeekRecordNewLab("tb_TESTCHART_PARAM", "TestChartId='" + testCodeTextBox.Text + "'"))
                {
                    dataGridView3.Rows.Clear();
                    PopulateGridViewData();
                    //specimenComboBox.Focus();
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

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
           
        }

        private void btnMachineSave_Click(object sender, EventArgs e)
        {
            


        }

       

     

        private void machineNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        

        private void parameterNameTextBox_Enter(object sender, EventArgs e)
        {
            textinfo = "MachineParam";
            parameterNameTextBox.BackColor = DbConnection.EnterFocus();
            dataGridView1.DataSource = gtTestCode.GetList(parameterNameTextBox.Text, textinfo);
            GridColor(dataGridView1);
            dataGridView1.Columns[0].Width = 250;
            dataGridView1.Columns[1].Visible = false;


        }

        private void parameterNameTextBox_Leave(object sender, EventArgs e)
        {
            parameterNameTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

     

        private void machineNameComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void specimenComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                parameterNameTextBox.Focus();
            }
        }

        private void ClearAdd()
        {
            //parameterNameTextBox.Text = "";
            //parameterTestNameTextBox.Text = "";
            isBoldCheckBox.Checked = false;
            //resultTextBox.Text = "";
            //unitNameComboBox.Text = "";
            //normalValueRichTextBox.Text = "";
            parameterNameTextBox.Focus();
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
                        if (_gt.FnSeekRecordNewLab("tb_TESTCHART_PARAM", "TestChartId='" + testCodeTextBox.Text + "'"))
                        {
                            PopulateGridViewData();
                        }
                       // specimenComboBox.Focus();
                        break;
                    case"ReportParam":
                        parameterTestNameTextBox.Text = gccode;
                        dataGridView3.Rows.Clear();
                        unitNameComboBox.Focus();
                        break;
                    case "Unit":
                        unitNameComboBox.Text = gccode;
                        dataGridView3.Rows.Clear();
                        resultTextBox.Focus();
                        break;
                    case "DefaultResult":
                        resultTextBox.Text = gccode;
                        dataGridView3.Rows.Clear();
                        normalValueRichTextBox.Focus();
                        break;
                    case "MachineParam":
                        parameterNameTextBox.Text = gccode;
                        dataGridView3.Rows.Clear();
                        parameterTestNameTextBox.Focus();
                        break;







                }
            }
        }
        private void dataGridView3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                int rowindex = dataGridView3.CurrentCell.RowIndex;
                if (dataGridView3.CurrentRow != null)
                {
                    int id = Convert.ToInt32(dataGridView3.CurrentRow.Cells[0].Value);
                    dataGridView3.Rows.RemoveAt(rowindex);
                }
            }
        }
        private void dataGridView3_DoubleClick(object sender, EventArgs e)
        {
           




            //int rowindex = dataGridView3.CurrentCell.RowIndex;
            //if (dataGridView3.CurrentRow != null)
            //{
            //    int id = Convert.ToInt32(dataGridView3.CurrentRow.Cells[0].Value);
            //    dataGridView3.Rows.RemoveAt(rowindex);
            //}
        }
        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }
        private void ParameterSetupUi_Activated(object sender, EventArgs e)
        {
            testCodeTextBox.Focus();
        }
        private void button13_Click(object sender, EventArgs e)
        {
           
        }
        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (testCodeTextBox.Text != "")
            {
                int a = 0;
                if (isBoldCheckBox.Checked)
                {
                    a = 1;
                }

                if (IsEdit == false)
                {
                    if (_gt.IsDuplicate(parameterNameTextBox.Text, dataGridView3))
                    {
                        MessageBox.Show(@"Already Exist Item Please Check", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        dataGridView3.Rows.Add(groupSerialTextBox.Text, parameterSerialTextBox.Text, parameterNameTextBox.Text, parameterTestNameTextBox.Text, normalValueRichTextBox.Text, unitNameComboBox.Text, reportingGroupNameComboBox.Text, resultTextBox.Text, a,lowerValTextBox.Text,upperValTextBox.Text);
                    }
                }
                else
                {
                    if (IsEdit)
                    {
                        dataGridView3.CurrentRow.Cells[0].Value = groupSerialTextBox.Text;
                        dataGridView3.CurrentRow.Cells[1].Value = parameterSerialTextBox.Text;
                        dataGridView3.CurrentRow.Cells[2].Value = parameterNameTextBox.Text;
                        dataGridView3.CurrentRow.Cells[3].Value = parameterTestNameTextBox.Text;
                        dataGridView3.CurrentRow.Cells[4].Value = normalValueRichTextBox.Text;
                        dataGridView3.CurrentRow.Cells[5].Value = unitNameComboBox.Text;
                        dataGridView3.CurrentRow.Cells[6].Value = reportingGroupNameComboBox.Text;
                        dataGridView3.CurrentRow.Cells[7].Value = resultTextBox.Text;
                        dataGridView3.CurrentRow.Cells[8].Value = a;
                        dataGridView3.CurrentRow.Cells[9].Value = lowerValTextBox.Text;
                        dataGridView3.CurrentRow.Cells[10].Value = upperValTextBox.Text;

                        IsEdit = false;
                    }
                }
                GridColor(dataGridView3);
                MessageBox.Show(@"Added Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearAdd();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (testCodeTextBox.Text == "")
            {
                MessageBox.Show(@"Please Add Test Code", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (specimenTextBox.Text == "")
            {
                MessageBox.Show(@"Please Add Specimen", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MachineName = machineNameComboBox.Text,
                HeaderName = reportHeaderTextBox.Text,
                SpecimenName = specimenTextBox.Text
            };
            string msg = _gt.SaveParameter(mdl, dataGridView3);
            MessageBox.Show(msg, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView3_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            


        }

        private void parameterNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                dataGridView1.Focus();
            }
        }

        private void reportingGroupNameComboBox_Enter(object sender, EventArgs e)
        {
            reportingGroupNameComboBox.BackColor = DbConnection.EnterFocus();
            textinfo = "ReportingGroup";
            dataGridView1.DataSource = gtTestCode.GetList(reportingGroupNameComboBox.Text, textinfo);
            GridColor(dataGridView1);
            dataGridView1.Columns[0].Width = 250;
        }

        private void reportingGroupNameComboBox_Leave(object sender, EventArgs e)
        {
            reportingGroupNameComboBox.BackColor = DbConnection.LeaveFocus();
        }

        private void parameterTestNameTextBox_Enter(object sender, EventArgs e)
        {
            parameterTestNameTextBox.BackColor = DbConnection.EnterFocus();
            textinfo = "ReportParam";
            dataGridView1.DataSource = gtTestCode.GetList(parameterTestNameTextBox.Text, textinfo);
            GridColor(dataGridView1);
            dataGridView1.Columns[0].Width = 250;
        }

        private void parameterTestNameTextBox_Leave(object sender, EventArgs e)
        {
            parameterTestNameTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void parameterNameTextBox_Leave_1(object sender, EventArgs e)
        {
            parameterNameTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void parameterNameTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = gtTestCode.GetList(parameterNameTextBox.Text, textinfo);
            GridColor(dataGridView1);

        }

        private void parameterTestNameTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = gtTestCode.GetList(parameterTestNameTextBox.Text, textinfo);
            GridColor(dataGridView1);
        }

        private void unitNameComboBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = gtTestCode.GetList(unitNameComboBox.Text, textinfo);
            GridColor(dataGridView1);
        }

        private void resultTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = gtTestCode.GetList(resultTextBox.Text, textinfo);
            GridColor(dataGridView1);

        }

        private void reportingGroupNameComboBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = gtTestCode.GetList(reportingGroupNameComboBox.Text, textinfo);
            GridColor(dataGridView1);

        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            IsEdit = true;


            string gSl = dataGridView3.CurrentRow.Cells[0].Value.ToString();
            string pSl = dataGridView3.CurrentRow.Cells[1].Value.ToString();
            string parametreName = dataGridView3.CurrentRow.Cells[2].Value.ToString();
            string testName = dataGridView3.CurrentRow.Cells[3].Value.ToString();
            string normalValue = dataGridView3.CurrentRow.Cells[4].Value.ToString();
            string unitName = dataGridView3.CurrentRow.Cells[5].Value.ToString();
            string reportingGroup = dataGridView3.CurrentRow.Cells[6].Value.ToString();
            string result = dataGridView3.CurrentRow.Cells[7].Value.ToString();
            int isBold = Convert.ToInt32(dataGridView3.CurrentRow.Cells[8].Value.ToString());
            string lowerVal = dataGridView3.CurrentRow.Cells[9].Value.ToString();
            string upperVal = dataGridView3.CurrentRow.Cells[10].Value.ToString();




            groupSerialTextBox.Text = gSl;
            parameterSerialTextBox.Text = pSl;
            reportingGroupNameComboBox.Text = reportingGroup;
            resultTextBox.Text = result;
            parameterNameTextBox.Text = parametreName;
            normalValueRichTextBox.Text = normalValue;
            unitNameComboBox.Text = unitName;
            isBoldCheckBox.Checked = Convert.ToBoolean(isBold);
            parameterTestNameTextBox.Text = testName;
            lowerValTextBox.Text = lowerVal;
            upperValTextBox.Text = upperVal;
            testCodeTextBox.Focus();
        }
    }
}
