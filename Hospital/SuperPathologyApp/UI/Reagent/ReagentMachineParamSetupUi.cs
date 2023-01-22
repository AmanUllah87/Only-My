using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Gateway.Reagent;

namespace SuperPathologyApp.UI
{
    public partial class ReagentMachineParamSetupUi : Form
    {
        public ReagentMachineParamSetupUi()
        {
            InitializeComponent();
        }
        readonly ReagentMachineSetupGateway _gt = new ReagentMachineSetupGateway();
        private string textInfo = "";
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }




     //   BarcodePrintGateway _gtBarcode=new BarcodePrintGateway();
        private void button3_Click(object sender, EventArgs e)
        {
            if (_gt.FnSeekRecordNewLab("tb_TESTCHART", "Id='" + testCodeTextBox.Text + "'") == false)
            {
                MessageBox.Show(@"Invalid Test Code Please Check", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dataGridView1.Rows.Count<1)
            {
                return;
            }




            if (_gt.FnSeekRecordNewLab("tb_reagent_TEST_BRIDGE", "TestId='" + testCodeTextBox.Text + "'"))
                {
                    _gt.DeleteInsertLab("DELETE FROM tb_reagent_TEST_BRIDGE WHERE TestId='" + testCodeTextBox.Text + "'");
                }
                string msg=_gt.Save(dataGridView1,testCodeTextBox.Text);
                if (msg == "Saved Success")
                {
                    dataGridView1.Rows.Clear();
                    testCodeTextBox.Text = "";
                    parameterTextBox.Text = "";
                    testCodeTextBox.Focus();
                }

                MessageBox.Show(msg, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            

        }

      


        private void DefaultResultSetupUi_Load(object sender, EventArgs e)
        {
            testCodeTextBox.Focus();
            dataGridView3.DataSource = _gt.GetTestCodeList(testCodeTextBox.Text);
            GridWidthDataGridView1();
            _gt.GridColor(dataGridView3);
        }
        private void GridWidthDataGridView1()
        {





            dataGridView3.EnableHeadersVisualStyles = false;
            dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView3.AllowUserToResizeRows = false;
            dataGridView3.Columns[0].Width = 75;
            dataGridView3.Columns[1].Width = 240;
      



        }
        private void testCodeTextBox_Leave(object sender, EventArgs e)
        {
            testCodeTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void testCodeTextBox_Enter(object sender, EventArgs e)
        {
            testCodeTextBox.BackColor = Hlp.EnterFocus();
        }

        private void testCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView3.DataSource = _gt.GetTestCodeList(testCodeTextBox.Text);
            GridWidthDataGridView1();
            _gt.GridColor(dataGridView3);
        }

        private void testCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Down)
            {
                textInfo = "1";
                dataGridView3.Focus();
            }

            if (e.KeyCode == Keys.Enter)
            {
                if (_gt.FnSeekRecordNewLab("tb_DefaultResultSetup", "PCode='" + testCodeTextBox.Text + "'"))
                {
                    PopulateChildData();
                }
                parameterTextBox.Focus();
            }

        }

        private void dataGridView3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string gccode = "";
                string gcdesc = "";
                gccode = dataGridView3.Rows[dataGridView3.CurrentCell.RowIndex - 1].Cells[0].Value.ToString();
                gcdesc = dataGridView3.Rows[dataGridView3.CurrentCell.RowIndex - 1].Cells[1].Value.ToString();
                switch (textInfo)
                {
                    case "1":
                        testCodeTextBox.Text = gccode;
                        testNameTextBox.Text = gcdesc;

                        if (_gt.FnSeekRecordNewLab("tb_reagent_TEST_BRIDGE", "TestId='" + testCodeTextBox.Text + "'"))
                        {
                            PopulateChildData();
                        }
                        else
                        {
                            dataGridView1.Rows.Clear();
                        }

                        parameterTextBox.Focus();
                        break;
                    case "2":
                        parameterTextBox.Text = gcdesc;
                        paramIdtextBox.Text = gccode;
                        parameterTextBox.Focus();
                        break;
                    case "3":
                      
                        break;


                }

            }
        }

        private void PopulateChildData()
        {
            var data = _gt.ChildCodeDetails(testCodeTextBox.Text);
            dataGridView1.Rows.Clear();
            foreach (var codeModel in data)
            {
                dataGridView1.Rows.Add(codeModel.ReagentId, codeModel.Name,codeModel.Qty);
            }
            dataGridView1.CurrentCell.Selected = false;
            _gt.GridColor(dataGridView1);
        }

        private void parameterTextBox_Leave(object sender, EventArgs e)
        {
            parameterTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void parameterTextBox_Enter(object sender, EventArgs e)
        {
            parameterTextBox.BackColor = Hlp.EnterFocus();
            textInfo = "2";

            dataGridView3.DataSource = _gt.GetParamNameByTestCode(parameterTextBox.Text);
            dataGridView3.Columns[1].Width = 240;
            dataGridView3.Columns[0].Width= 75;

            _gt.GridColor(dataGridView3);

        }

        private void parameterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Down)
            {
                textInfo = "2";
                dataGridView3.Focus();
            }

            if (e.KeyCode==Keys.Enter)
            {
                if (_gt.FnSeekRecordNewLab("tb_REAGENT","Id='"+ paramIdtextBox.Text +"'")==false)
                {
                    MessageBox.Show(@"Invalid Reagent name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                
                if (_gt.IsDuplicate(paramIdtextBox.Text,  dataGridView1) == false)
                {
                    dataGridView1.Rows.Add(paramIdtextBox.Text, parameterTextBox.Text,1);
                }
              
                dataGridView1.CurrentCell.Selected = false;
                _gt.GridColor(dataGridView1);
                paramIdtextBox.Text = "0";
                parameterTextBox.Text = "";
            }

        }

        private void resulTextBox_Leave(object sender, EventArgs e)
        {
           

        }

        private void resulTextBox_Enter(object sender, EventArgs e)
        {
           
            PopulateDataByResult();

        }

        private void PopulateDataByResult()
        {
           

        }

        private void resulTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Down)
            {
                textInfo = "3";
                dataGridView3.Focus();
            }

            if (e.KeyCode==Keys.Enter)
            {
                
                
                
                
               // dataGridView1.Rows.Add(parameterTextBox.Text, resulTextBox.Text);
                
                
                
                
               
                dataGridView1.Rows[0].Selected = false;
                _gt.GridColor(dataGridView1);
  


            }


        
        }

        private void resulTextBox_TextChanged(object sender, EventArgs e)
        {
            PopulateDataByResult();
        }

        private void parameterTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView3.DataSource = _gt.GetParamNameByTestCode(parameterTextBox.Text);
            dataGridView3.Columns[1].Width = 240;
            dataGridView3.Columns[0].Width = 75;
            _gt.GridColor(dataGridView3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double pt = 17.8;//first param
            double cntr = 13.8;//second param
         
            
            double index = cntr / pt * 100;
            double ratio = pt / cntr;
            double inr = Math.Pow(pt / cntr, 1.03);
            
            
            
            
            MessageBox.Show(inr.ToString());
        }

        private void ReagentMachineParamSetupUi_Load(object sender, EventArgs e)
        {
            testCodeTextBox.Focus();
            dataGridView3.DataSource = _gt.GetTestCodeList(testCodeTextBox.Text);
            GridWidthDataGridView1();
            _gt.GridColor(dataGridView3);
            foreach (DataGridViewColumn dc in dataGridView1.Columns)
            {
                dc.ReadOnly = !dc.Index.Equals(2);
            }
            dataGridView1.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
    }
}
