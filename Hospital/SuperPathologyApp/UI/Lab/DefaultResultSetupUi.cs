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

namespace SuperPathologyApp.UI
{
    public partial class DefaultResultSetupUi : Form
    {
        public DefaultResultSetupUi()
        {
            InitializeComponent();
        }
        readonly DefaultResultSetupGateway _gt = new DefaultResultSetupGateway();
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
            if (dataGridView1.Rows.Count<2)
            {
                return;
            }


            
            if (MessageBox.Show(@"Are you sure want to save default result?", @"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }
            else
            {
                if (_gt.FnSeekRecordNewLab("tb_TESTCHART_DEF_RESULT", "TestChartId='" + testCodeTextBox.Text + "'"))
                {
                    _gt.DeleteInsertLab("DELETE FROM tb_TESTCHART_DEF_RESULT WHERE TestChartId='" + testCodeTextBox.Text + "'");
                }
                string msg=_gt.Save(dataGridView1,testCodeTextBox.Text);
                if (msg == "Saved Success")
                {
                    dataGridView1.Rows.Clear();
                    testCodeTextBox.Text = "";
                    parameterTextBox.Text = "";
                    resulTextBox.Text = "";
                    testCodeTextBox.Focus();
                }

                MessageBox.Show(msg, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

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
            dataGridView3.Columns[0].Width = 85;
            dataGridView3.Columns[1].Width = 230;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeRows = false;










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

                        if (_gt.FnSeekRecordNewLab("tb_TESTCHART_DEF_RESULT", "TestChartId='" + testCodeTextBox.Text + "'"))
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
                        resulTextBox.Focus();
                        break;
                    case "3":
                        resulTextBox.Text = gcdesc;
                        resulTextBox.Focus();
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
                dataGridView1.Rows.Add(codeModel.ParameterName, codeModel.Result);
            }
            dataGridView1.Rows[0].Selected = false;
            _gt.GridColor(dataGridView1);
        }

        private void parameterTextBox_Leave(object sender, EventArgs e)
        {
            parameterTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void parameterTextBox_Enter(object sender, EventArgs e)
        {
            parameterTextBox.BackColor = Hlp.EnterFocus();


            dataGridView3.DataSource = _gt.GetParamNameByTestCode(testCodeTextBox.Text);
            dataGridView3.Columns[1].Width = 270;
            dataGridView3.Columns[0].Visible= false;
           // dataGridView3.Columns[2].Visible = false;
          //  dataGridView3.Columns[3].Visible = false;
         //   dataGridView3.Columns[4].Visible = false;

            //dataGridView3.CurrentCell.Selected = false;
            _gt.GridColor(dataGridView3);

        }

        private void parameterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Down)
            {
                textInfo = "2";
                dataGridView3.Focus();
            }
        }

        private void resulTextBox_Leave(object sender, EventArgs e)
        {
            resulTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void resulTextBox_Enter(object sender, EventArgs e)
        {
            resulTextBox.BackColor = Hlp.EnterFocus();
            PopulateDataByResult();

        }

        private void PopulateDataByResult()
        {
            dataGridView3.DataSource = _gt.GetAllDefaultResult(resulTextBox.Text);
            dataGridView3.Columns[1].Width = 270;
            dataGridView3.Columns[0].Visible = false;
            dataGridView3.Columns[2].Visible = false;
            dataGridView3.Columns[3].Visible = false;
            dataGridView3.Columns[4].Visible = false;

            //dataGridView3.CurrentCell.Selected = false;
            _gt.GridColor(dataGridView3);

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
                
                
                
                
                dataGridView1.Rows.Add(parameterTextBox.Text, resulTextBox.Text);
                
                
                
                
                resulTextBox.Text = "";
                resulTextBox.Focus();
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
            dataGridView3.DataSource = _gt.GetParamNameByTestCode(testCodeTextBox.Text);
            dataGridView3.Columns[1].Width = 270;
            dataGridView3.Columns[0].Visible = false;
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
    }
}
