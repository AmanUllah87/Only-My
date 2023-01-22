using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Gateway.Diagnosis;

namespace SuperPathologyApp.UI.Diagnosis
{
    public partial class DigitalVdUi : Form
    {
        public DigitalVdUi()
        {
            InitializeComponent();
        }
        DigitalVdGateway _gt=new DigitalVdGateway();
        ReportPrintGateway _rpt=new ReportPrintGateway();
        private string _textInfo = "";
        private void billNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
               


                GetDataByInvNo(billNoTextBox.Text);
                
                
                
               
            }
        }

        private void GetDataByInvNo(string billNo)
        {

            if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "BillNo='" + billNo + "'"))
            {
                billNoTextBox.Text = billNo;
                billDate.Value = Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + billNo + "'", "BillDate"));
                int masterId = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + billNo + "'", "Id"));
                var dataM = _rpt.GetMasterData(masterId);
                ptNameTextBox.Text = dataM.PatientName;
                ageTextBox.Text = dataM.Age;
                sexTextBox1.Text = dataM.Sex;
                remarksTextBox.Text = dataM.Remarks;
                refDrIdTextBox.Text = dataM.RefDrModel.DrId.ToString();
                refDrName.Text = dataM.RefDrModel.Name;
            }







            var data = _gt.GetDataByInvNo(billNo);
            dataGridView2.Rows.Clear();
            double refAmt = 0, drOneAmt = 0, drTwoAmt = 0, drThreeAmt = 0, paidAmt = 0, less = 0;
            string remarks = "";
            foreach (var model in data.TestChartModel)
            {
                if (_gt.FnSeekRecordNewLab("tb_DigitalVd", "BillNo='" + billNoTextBox.Text + "' AND BillDate='" + billDate.Value.ToString("yyyy-MM-dd") + "'"))
                {
                    var dt = _gt.Search("SELECT * FROM tb_DigitalVd WHERE BillNo='" + billNo + "' AND BillDate='" + billDate.Value.ToString("yyyy-MM-dd") + "' AND DeptName='" + model.Name + "'");
                    if (dt.Rows.Count > 0)
                    {
                        refAmt = Convert.ToDouble(dt.Rows[0]["RefAmt"]);
                        drOneAmt = Convert.ToDouble(dt.Rows[0]["DrOneAmt"]);
                        drTwoAmt = Convert.ToDouble(dt.Rows[0]["DrTwoAmt"]);
                        drThreeAmt = Convert.ToDouble(dt.Rows[0]["DrThreeAmt"]);
                        paidAmt = Convert.ToDouble(dt.Rows[0]["Paid"]);
                        less= Convert.ToDouble(dt.Rows[0]["Less"]);

                        remarks = dt.Rows[0]["Remarks"].ToString();
                        refDrIdTextBox.Text = dt.Rows[0]["RefId"].ToString();
                        drOneTextBox.Text = dt.Rows[0]["DrOneId"].ToString();
                        drTwoTextBox.Text = dt.Rows[0]["DrTwoId"].ToString();
                        drThreeTextBox.Text = dt.Rows[0]["DrThreeName"].ToString();

                    }
                }


                //double price = Convert.ToDouble(dataGridView2.Rows[rowIndex].Cells[1].Value);
                //double less = Convert.ToDouble(dataGridView2.Rows[rowIndex].Cells[2].Value);
                //double refDrAmt = Convert.ToDouble(dataGridView2.Rows[rowIndex].Cells[3].Value);
                //double drOneAmt = Convert.ToDouble(dataGridView2.Rows[rowIndex].Cells[4].Value);
                //double drTwoAmt = Convert.ToDouble(dataGridView2.Rows[rowIndex].Cells[5].Value);
                //double paidAmt = Convert.ToDouble(dataGridView2.Rows[rowIndex].Cells[7].Value);

                double balance = Math.Round(model.Charge - less - refAmt - drOneAmt - drTwoAmt - drThreeAmt- paidAmt);







                
                dataGridView2.Rows.Add(model.Name, model.Charge, less, refAmt, drOneAmt, drTwoAmt,drThreeAmt, balance, paidAmt,remarks);
            }



            refDrName.Text = _gt.FncReturnFielValueLab("tb_Doctor", "Id='" + refDrIdTextBox.Text + "'", "Name");
            drOneName.Text = _gt.FncReturnFielValueLab("tb_Doctor", "Id='" + drOneTextBox.Text + "'", "Name");
            drTwoName.Text = _gt.FncReturnFielValueLab("tb_Doctor", "Id='" + drTwoTextBox.Text + "'", "Name");





            dataGridView2.CurrentCell = null; 
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentCell == null)
            {
                return;
            }

            if (dataGridView2.CurrentCell.ColumnIndex == 2 || dataGridView2.CurrentCell.ColumnIndex == 3 || dataGridView2.CurrentCell.ColumnIndex == 4 || dataGridView2.CurrentCell.ColumnIndex == 5 || dataGridView2.CurrentCell.ColumnIndex == 6 || dataGridView2.CurrentCell.ColumnIndex == 8)
            {

                try
                {
                    int rowIndex = dataGridView2.CurrentCell.RowIndex;
                    double price = Convert.ToDouble(dataGridView2.Rows[rowIndex].Cells[1].Value);
                    double less = Convert.ToDouble(dataGridView2.Rows[rowIndex].Cells[2].Value);
                    double refDrAmt = Convert.ToDouble(dataGridView2.Rows[rowIndex].Cells[3].Value);
                    double drOneAmt = Convert.ToDouble(dataGridView2.Rows[rowIndex].Cells[4].Value);
                    double drTwoAmt = Convert.ToDouble(dataGridView2.Rows[rowIndex].Cells[5].Value);
                    double drThreeAmt = Convert.ToDouble(dataGridView2.Rows[rowIndex].Cells[6].Value);

                    double paidAmt = Convert.ToDouble(dataGridView2.Rows[rowIndex].Cells[8].Value);

                    double balance = Math.Round(price - less - refDrAmt - drOneAmt - drTwoAmt - drThreeAmt - paidAmt);
                    dataGridView2.Rows[rowIndex].Cells[7].Value = Math.Round(balance);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

            }
        }

        private void DigitalVdUi_Load(object sender, EventArgs e)
        {
            GridResize();
            ClearText();

        }

        private void GridResize()
        {
            foreach (DataGridViewColumn dc in dataGridView2.Columns)
            {
                dc.ReadOnly = !dc.Index.Equals(1);
            }
            dataGridView2.Columns[2].ReadOnly = false;
            dataGridView2.Columns[3].ReadOnly = false;
            dataGridView2.Columns[4].ReadOnly = false;
            dataGridView2.Columns[5].ReadOnly = false;
            dataGridView2.Columns[6].ReadOnly = false;

            dataGridView2.Columns[8].ReadOnly = false;
            dataGridView2.Columns[9].ReadOnly = false;



        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        readonly DiagnosisBillGateway _diag = new DiagnosisBillGateway();
        private void ClearText()
        {
            billNoTextBox.Text = "";
            ptNameTextBox.Text = "";
            ageTextBox.Text = "";
            sexTextBox1.Text = "";
            remarksTextBox.Text = "";
            refDrName.Text = "";
            drOneName.Text = "";
            drTwoName.Text = "";

            refDrIdTextBox.Text = "0";
            drOneTextBox.Text = "0";
            drTwoTextBox.Text = "0";
            billNoTextBox.Focus();
            dataGridView2.Rows.Clear();
            drThreeTextBox.Text = "";

            //textInfo = "enter";

            LoadInvGrid();



        }

        private void LoadInvGrid()
        {
            dataGridView1.DataSource = _diag.GetInvoiceListForDigitalVd(billDate.Value, billNoTextBox.Text, "enter");

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns[0].Width = 80;
                dataGridView1.Columns[1].Width = 75;
                dataGridView1.Columns[2].Width = 100;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Width = 55;

                Hlp.GridFirstRowDeselect(dataGridView1);
            }

        }

        private void billNoTextBox_Enter(object sender, EventArgs e)
        {
            billNoTextBox.BackColor = Hlp.EnterFocus();
            LoadInvGrid();
        }

        private void billNoTextBox_Leave(object sender, EventArgs e)
        {
            billNoTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void refDrIdTextBox_Enter(object sender, EventArgs e)
        {
            refDrIdTextBox.BackColor = Hlp.EnterFocus();
            HelpDataGridLoadByRefDr();
            _textInfo = "refDr";
        }

        private void refDrIdTextBox_Leave(object sender, EventArgs e)
        {
            refDrIdTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void drOneTextBox_Enter(object sender, EventArgs e)
        {
            drOneTextBox.BackColor = Hlp.EnterFocus();
            HelpDataGridLoadByDrOne();
            _textInfo = "drOne";
        }

        private void drOneTextBox_Leave(object sender, EventArgs e)
        {
            drOneTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void drTwoTextBox_Enter(object sender, EventArgs e)
        {
            drTwoTextBox.BackColor = Hlp.EnterFocus();
            HelpDataGridLoadByDrTwo();
            _textInfo = "drTwo";

        }

        private void drTwoTextBox_Leave(object sender, EventArgs e)
        {
            drTwoTextBox.BackColor = Hlp.LeaveFocus();
        }
        private void HelpDataGridLoadByRefDr()
        {
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name FROM tb_DOCTOR", refDrIdTextBox.Text, "(convert(varchar,Id)+Name)");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 290;
        }
        private void HelpDataGridLoadByDrOne()
        {
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name FROM tb_DOCTOR", drOneTextBox.Text, "(convert(varchar,Id)+Name)");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 290;
        }
        private void HelpDataGridLoadByDrTwo()
        {
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name FROM tb_DOCTOR", drTwoTextBox.Text, "(convert(varchar,Id)+Name)");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 290;
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string gccode = "";
                string gcdesc = "";
                gccode = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[0].Value.ToString();
                gcdesc = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[1].Value.ToString();

                switch (_textInfo)
                {

                    case "refDr":
                        refDrIdTextBox.Text = gccode;
                        refDrName.Text = gcdesc;
                        drOneTextBox.Focus();
                        break;
                    case "drOne":
                        drOneTextBox.Text = gccode;
                        drOneName.Text = gcdesc;
                        drTwoTextBox.Focus();
                        break;
                    case "drTwo":
                        drTwoTextBox.Text = gccode;
                        drTwoName.Text = gcdesc;
                        drThreeTextBox.Focus();
                        break;
                    default:
                        GetDataByInvNo(gccode);
                        break;
                }
            }
        }

        private void refDrIdTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                dataGridView1.Focus();
            }
        }

        private void drOneTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                dataGridView1.Focus();
            }
        }

        private void drTwoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                dataGridView1.Focus();
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER","BillNo='"+ billNoTextBox.Text +"'")==false)
            {
                MessageBox.Show(@"Please add Valid Bill No.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                billNoTextBox.Focus();
                return;
            }

            if (dataGridView2.Rows.Count<1)
            {
                MessageBox.Show(@"Please add Valid Bill No.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                billNoTextBox.Focus();
                return;
            }

            try
            {
                _gt.DeleteInsertLab("DELETE FROM tb_DigitalVd WHERE BillNo='" + billNoTextBox.Text + "'");
                for (int i = 0; i < dataGridView2.Rows.Count-1; i++)
                {
                    string deptName = dataGridView2.Rows[i].Cells[0].Value == null ? "" : dataGridView2.Rows[i].Cells[0].Value.ToString();
                    string price = dataGridView2.Rows[i].Cells[1].Value == null ? "" : dataGridView2.Rows[i].Cells[1].Value.ToString();
                    string less = dataGridView2.Rows[i].Cells[2].Value == null ? "" : dataGridView2.Rows[i].Cells[2].Value.ToString();
                    string refDrAmt = dataGridView2.Rows[i].Cells[3].Value == null ? "" : dataGridView2.Rows[i].Cells[3].Value.ToString();
                    string drOneAmt = dataGridView2.Rows[i].Cells[4].Value == null ? "" : dataGridView2.Rows[i].Cells[4].Value.ToString();
                    string drTwoAmt = dataGridView2.Rows[i].Cells[5].Value == null ? "" : dataGridView2.Rows[i].Cells[5].Value.ToString();
                    string drThreeAmt = dataGridView2.Rows[i].Cells[6].Value == null ? "" : dataGridView2.Rows[i].Cells[6].Value.ToString();
                    string balance = dataGridView2.Rows[i].Cells[7].Value == null ? "" : dataGridView2.Rows[i].Cells[7].Value.ToString();
                    string paid = dataGridView2.Rows[i].Cells[8].Value == null ? "" : dataGridView2.Rows[i].Cells[8].Value.ToString();
                    string remarks = dataGridView2.Rows[i].Cells[9].Value == null ? "" : dataGridView2.Rows[i].Cells[9].Value.ToString();
                    _gt.DeleteInsertLab("INSERT INTO tb_DigitalVd(BillNo, BillDate, DeptName, Price, Less, RefAmt, RefId, DrOneAmt, DrOneId, DrTwoAmt, DrTwoId, Balance, Paid, Remarks,  PostedBy, PcName, IpAddress,DrThreeAmt,DrThreeName)VALUES('" + billNoTextBox.Text + "', '" + billDate.Value.ToString("yyyy-MM-dd") + "', '" + deptName + "', '" + price + "', '" + less + "', '" + refDrAmt + "', '" + refDrIdTextBox.Text + "', '" + drOneAmt + "', '" + drOneTextBox.Text + "', '" + drTwoAmt + "', '" + drTwoTextBox.Text + "', '" + balance + "', '" + paid + "', '" + remarks + "', '" + Hlp.UserName + "', '" + Environment.UserName + "', '" + Hlp.IpAddress() + "','" + drThreeAmt + "','" + drThreeTextBox.Text + "')");

                }
                MessageBox.Show(@"Save Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearText();

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }








        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count>0)
            {
                GetDataByInvNo(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString());
            }
        }

        private void refDrIdTextBox_TextChanged(object sender, EventArgs e)
        {
            HelpDataGridLoadByRefDr();
            _textInfo = "refDr";
        }

        private void drOneTextBox_TextChanged(object sender, EventArgs e)
        {
            HelpDataGridLoadByDrOne();
            _textInfo = "drOne";
        }

        private void drTwoTextBox_TextChanged(object sender, EventArgs e)
        {
            HelpDataGridLoadByDrTwo();
            _textInfo = "drTwo";
        }

    }
}
