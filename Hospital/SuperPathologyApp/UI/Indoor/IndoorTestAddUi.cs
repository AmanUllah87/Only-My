using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperPathologyApp.Gateway.Indoor;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Indoor;
using SuperPathologyApp.Report;

namespace SuperPathologyApp.UI.Indoor
{
    public partial class IndoorTestAddUi : Form
    {
        public IndoorTestAddUi()
        {
            InitializeComponent();
        }

        readonly IndoorTestGateway _rls = new IndoorTestGateway();
        private void contactNoTextBox_TextChanged(object sender, EventArgs e)
        {
            HelpDataGridLoadByPatientId();
            helpPanel.Visible = true;
        }
        private void HelpDataGridLoadByPatientId()
        {
            //dataGridView2.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,AdmNo,AdmDate,PtName AS Name,ContactNo,BedName As Bed FROM V_Admission_List ", contactNoTextBox.Text, "(convert(varchar,Id)+PtName+BedName+ContactNo+AdmNo)");
            dataGridView2.DataSource = Hlp.LoadDbByQuery(0,"SELECT Id,AdmNo,AdmDate,PtName AS Name,ContactNo,BedName As Bed FROM V_Admission_List  WHERE ReleaseStatus=0 AND (convert(varchar,Id)+PtName+BedName+ContactNo+AdmNo) LIKE '%"+ contactNoTextBox.Text +"%'");

            Hlp.GridFirstRowDeselect(dataGridView2);
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Width = 65;
            dataGridView2.Columns[2].Width = 75;
            dataGridView2.Columns[3].Width = 160;
            dataGridView2.Columns[4].Visible = false;
            dataGridView2.Columns[5].Width = 70;
            dataGridView2.Columns[5].Visible = false;

        }
        string _textInfo = "";
        private void contactNoTextBox_Enter(object sender, EventArgs e)
        {
            _textInfo = "contact";
            contactNoTextBox.BackColor = Hlp.EnterFocus();
        }

        private void contactNoTextBox_Leave(object sender, EventArgs e)
        {
            contactNoTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void contactNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Down||e.KeyCode==Keys.Up)
            {
                dataGridView2.Rows[0].Selected = true;
                dataGridView2.Focus();
            }
        }

        private void dataGridView2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string gccode = "";
                string gcdesc = "";
                gccode = dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex - 1].Cells[0].Value.ToString();
                gcdesc = dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex - 1].Cells[1].Value.ToString();

                switch (_textInfo)
                {
                    case "contact":
                        contactNoTextBox.Text = gcdesc;
                        admNotextBox.Text = gccode;
                        var dt = Hlp.GetRegAndBedId(Convert.ToInt32(gccode));
                        ptNameTextBox.Text = dt.Patient.Name;
                        sexTextBox.Text = dt.Patient.Sex;
                        addressTextBox.Text = dt.Patient.Address;
                     
                        consDrNameTextBox.Text = dt.UnderDoctor.Name;
                        admDateTextBox.Text = dt.AdmDate.ToString("yyyy-MM-dd");
                        admTimeTextBox.Text = dt.AdmTime;
                        bedNotextBox.Text = dt.Bed.Name;
                        floorNotextBox.Text = dt.Bed.Floor;
                        departmentTextBox.Text = dt.Bed.Department.Name;
                        admNotextBox.Text = gccode;
                      
                      
                        
                        
                        CalculateTotal();
                        testCodeTextBox.Focus();
                       // helpPanel.Visible = false;
                        break;
                    case "test":
                        testCodeTextBox.Text = gccode;
                        AddDataToGrid();
                        break;
                    case "doctor":
                        drNameTextBox.Text = gccode;
                        AddDataToGrid();
                        break;
                    case "bill":
                        drNameTextBox.Text = gccode;
                        AddDataToGrid();
                        break;



                }

            }
        }

        private void testCodeTextBox_Enter(object sender, EventArgs e)
        {
            testCodeTextBox.BackColor = Hlp.EnterFocus();
            _textInfo = "test";
            HelpDataGridLoadByTest(dataGridView2, testCodeTextBox.Text);
        }

        private void testCodeTextBox_Leave(object sender, EventArgs e)
        {
            testCodeTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void testCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            HelpDataGridLoadByTest(dataGridView2, testCodeTextBox.Text);
        }
        TestChartGateway _gt = new TestChartGateway();
        public void HelpDataGridLoadByTest(DataGridView dg, string search)
        {
            helpPanel.Visible = true;
            dg.DataSource = null;
           
            dg.DataSource = _gt.GetTestCodeList(0, search, 0);
            Hlp.GridFirstRowDeselect(dg);
            dg.Columns.OfType<DataGridViewColumn>().ToList().ForEach(col => col.Visible = false);
          
            dg.Columns[0].Width = 65;
            dg.Columns[1].Width = 170;
            dg.Columns[2].Width = 65;
            dg.Columns[0].Visible = true;
            dg.Columns[1].Visible = true;
            dg.Columns[2].Visible = true;


        }

        private void testCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                if (_gt.FnSeekRecordNewLab("tb_TestChart", "Id='" + testCodeTextBox.Text + "'"))
                {
                    AddDataToGrid();
                    return;
                }


                if (testCodeTextBox.Text.Length > 0)
                {
                    if (dataGridView2.Rows.Count > 0)
                    {
                        if (dataGridView2.Rows[0].Cells[0].Value == null)
                        {
                            return;
                        }
                        testCodeTextBox.Text = dataGridView2.Rows[0].Cells[0].Value.ToString();
                        if (_gt.FnSeekRecordNewLab("tb_TestChart", "Id='" + testCodeTextBox.Text + "'"))
                        {
                            AddDataToGrid();
                            return;
                        }
                    }
                }
               
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                _textInfo = "test";
                dataGridView2.Rows[0].Selected = true;
                //dataGridView2.CurrentCell.Selected = true;
                dataGridView2.Focus();
            }
        }

        private void AddDataToGrid()
        {
            string drName = "";
            if (drNameTextBox.Text=="")
            {
                if (_gt.FnSeekRecordNewLab("tb_TestChart", "Id='" + testCodeTextBox.Text + "'"))
                {
                    if (_gt.FnSeekRecordNewLab("tb_TestChart", "Id='" + testCodeTextBox.Text + "' AND IsDoctor=1"))
                    {
                        drNameTextBox.Visible = true;
                        drNameTextBox.Focus();
                        return;
                    }
                }
            }
            else
            {
                if (_gt.FnSeekRecordNewLab("tb_DOCTOR", "Id='" + drNameTextBox.Text + "'")==false)
                {
                    MessageBox.Show(@"Invalid Dr Name Please Check", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    drNameTextBox.Focus();
                    return;
                }
                drName = _gt.FncReturnFielValueLab("tb_Doctor", "Id='" + drNameTextBox.Text + "'", "Name");

            }








            string descName = _gt.FncReturnFielValueLab("tb_TestChart", "Id='" + testCodeTextBox.Text + "'", "Name");
            string charge = _gt.FncReturnFielValueLab("tb_TestChart", "Id='" + testCodeTextBox.Text + "'", "Charge");

            if (_gt.FnSeekRecordNewLab("tb_Doctor","Id='"+ drNameTextBox.Text +"'"))
            {

                if (drName != "")
                {
                    descName = descName + "(" + drName + ")";
                }
                dataGridView1.Rows.Add(testCodeTextBox.Text, descName, charge, 1, charge, 0, charge, charge, drNameTextBox.Text);
                testCodeTextBox.Text = "";
                drNameTextBox.Text = "";
                drNameTextBox.Visible = false;
                dataGridView1.CurrentCell.Selected = false;
               // helpPanel.Visible = false;
                testCodeTextBox.Focus();

            }

            else 
            {

                if (_gt.IsDuplicate(testCodeTextBox.Text, dataGridView1) == false)
                {
                    dataGridView1.Rows.Add(testCodeTextBox.Text, descName, charge, 1, charge, 0, charge, 0, drNameTextBox.Text);
                    testCodeTextBox.Text = "";
                    drNameTextBox.Text = "";
                    drNameTextBox.Visible = false;
                    dataGridView1.CurrentCell.Selected = false;
                  //  helpPanel.Visible = false;
                    testCodeTextBox.Focus();
                }
                else
                {
                    MessageBox.Show(@"Duplicate Name Found", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    testCodeTextBox.SelectAll();
                    return;
                    
                }
            }




           


            CalculateTotal();

           
           

        }
        private string CalculateTotal()
        {

            double totCharge = 0, totLess = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                totCharge += Convert.ToDouble(Hlp.IsNumeric(dataGridView1.Rows[i].Cells[4].Value.ToString())?dataGridView1.Rows[i].Cells[4].Value:"0");
                totLess += Convert.ToDouble(Hlp.IsNumeric(dataGridView1.Rows[i].Cells[5].Value.ToString()) ? dataGridView1.Rows[i].Cells[5].Value : "0");

                if (_gt.FnSeekRecordNewLab("tb_TESTCHART","Id='"+ dataGridView1.Rows[i].Cells[0].Value.ToString() +"' AND ChangeCharge=1"))
                {
                    dataGridView1.Rows[i].Cells[2].Style.BackColor = Hlp.GridHightLightColumn();
                }
               

            
            
            }

            totalTextBox.Text = totCharge.ToString();
           
          
            
         //   dataGridView1.Columns[5].DefaultCellStyle.BackColor = Color.Yellow;
            dataGridView1.Columns[3].DefaultCellStyle.BackColor = Color.Yellow;
            if (dataGridView1.Rows.Count>0)
            {
                dataGridView1.CurrentCell.Selected = false;
            }
           
            
            return "ok";


          

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell==null)
            {
                return;
            }

            if (dataGridView1.CurrentCell.ColumnIndex == 3 || dataGridView1.CurrentCell.ColumnIndex == 5 || dataGridView1.CurrentCell.ColumnIndex == 2)
            {

                try
                {
                   





                    int rowIndex = dataGridView1.CurrentCell.RowIndex;
                    double qty = Convert.ToDouble(dataGridView1.Rows[rowIndex].Cells[3].Value);
                    double charge = Convert.ToDouble(dataGridView1.Rows[rowIndex].Cells[2].Value);
                    double lessAmt = Convert.ToDouble(dataGridView1.Rows[rowIndex].Cells[5].Value);
                    double totCharge = Math.Round(qty * charge);
                    dataGridView1.Rows[rowIndex].Cells[4].Value = totCharge;
                    dataGridView1.Rows[rowIndex].Cells[6].Value = Math.Round(totCharge - lessAmt);


                    
                    
                    if (_gt.FnSeekRecordNewLab("tb_TestChart", "Id='" + dataGridView1.Rows[rowIndex].Cells[0].Value + "' AND IsDoctor=1"))
                    {
                        dataGridView1.Rows[rowIndex].Cells[7].Value = Math.Round(totCharge - lessAmt);
                    }
                   



                    CalculateTotal();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                
            }
            
            




        }

        private void contactNoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
           // e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (dataGridView1.CurrentCell.ColumnIndex == 3 || dataGridView1.CurrentCell.ColumnIndex == 5 || dataGridView1.CurrentCell.ColumnIndex == 4) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }
        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)
                 && e.KeyChar != '.')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void drNameTextBox_Enter(object sender, EventArgs e)
        {
            drNameTextBox.BackColor = Hlp.EnterFocus();
            _textInfo = "doctor";
            HelpDataGridLoadByRefDr(dataGridView2,drNameTextBox.Text);

        }
        public void HelpDataGridLoadByRefDr(DataGridView dg, string search)
        {
            var dr = new DoctorGateway();
            dg.DataSource = null;

            dg.DataSource = dr.GetDoctorList(0, search);
            Hlp.GridFirstRowDeselect(dg);
            //  Hlp.GridColor(dg);
            dg.Columns[0].Width = 70;
            dg.Columns[1].Width = 300;
            dg.Columns[2].Visible = false;
            dg.Columns[3].Visible = false;

            dg.Columns[4].Visible = false;
            dg.Columns[5].Visible = false;
            dg.Columns[6].Visible = false;
            dg.Columns[7].Visible = false;
            dg.Columns[8].Visible = false;


        }

        private void drNameTextBox_Leave(object sender, EventArgs e)
        {
            drNameTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void drNameTextBox_TextChanged(object sender, EventArgs e)
        {
            HelpDataGridLoadByRefDr(dataGridView2, drNameTextBox.Text);
        }

        private void drNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                _textInfo = "doctor";
                dataGridView2.Rows[0].Selected = true;
                dataGridView2.Focus();
            }
        }

        private void ReleaseUi_Load(object sender, EventArgs e)
        {
          //  double ad =  Hlp.IsNumeric("aa") ? 10 : 0;
            
            
            ClearText();
        }

        private void ClearText()
        {
            dataGridView1.Rows.Clear();
           // ReleaseDateTextBox.Text = Hlp.GetServerDate().ToString("dd-MMM-yyyy");
            
            contactNoTextBox.Text = "";
            admNotextBox.Text = "0";
            ptNameTextBox.Text = "";
            sexTextBox.Text = "";
            addressTextBox.Text = "";
         
            consDrNameTextBox.Text = "";
            admDateTextBox.Text = "";
            admTimeTextBox.Text = "";
            bedNotextBox.Text = "";
            floorNotextBox.Text = "";
            totalTextBox.Text = "";
            remarksTextBox.Text = "";
            departmentTextBox.Text = "";
            contactNoTextBox.Focus();
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void saveAndPrintButton_Click(object sender, EventArgs e)
        {
            if (CheckSuccess())
            {
                var aMdl = new ReleaseModel()
                {
                    Admission= new AdmissionModel()
                    {
                        AdmId =Convert.ToInt32(admNotextBox.Text)
                    },
                };
                var mdl = new List<TestChartModel>();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    mdl.Add(new TestChartModel()
                    {
                        TestId = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value.ToString()),
                        Name =dataGridView1.Rows[i].Cells[1].Value.ToString(),
                        Charge = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value.ToString()),
                        Unit= Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value.ToString()),
                        TotCharge= Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value.ToString()),
                        LessAmt = Convert.ToDouble(dataGridView1.Rows[i].Cells[5].Value.ToString()),
                        NetTotAmt= Convert.ToDouble(dataGridView1.Rows[i].Cells[6].Value.ToString()),
                        DefaulHonouriam= Convert.ToDouble(dataGridView1.Rows[i].Cells[7].Value.ToString()),
                        DrId = Convert.ToInt32(Hlp.IsNumeric(dataGridView1.Rows[i].Cells[8].Value.ToString())?dataGridView1.Rows[i].Cells[8].Value.ToString():"0"),
                    });
                }
                aMdl.Test = mdl;
                aMdl.TotAmt = Convert.ToDouble(totalTextBox.Text);
               
                aMdl.Remarks = remarksTextBox.Text;
               



                string msg = _rls.Save(aMdl, saveAndPrintButton.Text);
                if (msg == _gt.SaveSuccessMessage)
                {

                    MessageBox.Show(_gt.SaveSuccessMessage, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearText();
                    //_gt.DeleteInsertLab("Update tb_BILL_MASTER SET  LastPrintPc='" + Environment.MachineName + "' WHERE Id='" + aMdl.BillId + "'");
                    PrintIndoorBill(aMdl.TrNo);
                }
                else
                {
                    MessageBox.Show(msg, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }

           

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
        private bool CheckSuccess()
        {
            bool isChecked = true;

            if (_gt.FnSeekRecordNewLab("tb_in_ADMISSION","Id='"+ admNotextBox.Text +"'") == false)
            {
                MessageBox.Show(@"Input your patient name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                contactNoTextBox.Focus();
                isChecked = false;
            }
            if (dataGridView1.Rows.Count<1)
            {
                MessageBox.Show(@"Please Add Some Procedure For Save", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                contactNoTextBox.Focus();
                isChecked = false;
            }

           

            return isChecked;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (dataGridView1.CurrentCell == null)
                {
                    return;
                }
                dataGridView1.Rows.RemoveAt(this.dataGridView1.CurrentCell.RowIndex);
                CalculateTotal();
            }


            if (e.KeyCode==Keys.Down||e.KeyCode==Keys.Up)
            {
                if (_gt.FnSeekRecordNewLab("tb_TestChart", "Id='" + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value + "' AND ChangeCharge=1"))
                {
                    dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].ReadOnly = false;
                    return;
                }
            }





        }

        private void searchTextBox_Enter(object sender, EventArgs e)
        {
            _textInfo = "bill";
            dataGridView2.DataSource = _rls.GetInvoiceList(DateTime.Now, searchTextBox.Text, "enter");
            // Hlp.GridColor(dataGridView2);
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Columns[0].Width = 80;
                dataGridView2.Columns[1].Width = 80;
                dataGridView2.Columns[2].Width = 200;
                dataGridView2.Columns[3].Visible = false;
                dataGridView2.Columns[4].Visible = false;
                dataGridView2.Columns[5].Visible = false;

               // helpPanel.Location = new Point(12, 169);
                helpPanel.Visible = true;
                Hlp.GridFirstRowDeselect(dataGridView2);
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView2.DataSource = _rls.GetInvoiceList(DateTime.Now, searchTextBox.Text, "change");
            Hlp.GridFirstRowDeselect(dataGridView2);
            // Hlp.GridColor(dataGridView2);
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Columns[0].Width = 80;
                dataGridView2.Columns[1].Width = 80;
                dataGridView2.Columns[2].Width = 200;
                dataGridView2.Columns[3].Visible = false;
                dataGridView2.Columns[4].Visible = false;
                dataGridView2.Columns[5].Visible = false;

                //helpPanel.Location = new Point(12, 169);
                helpPanel.Visible = true;
                Hlp.GridFirstRowDeselect(dataGridView2);
            }
        }

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentCell.Selected)
            {
                if (dataGridView2.CurrentRow != null)
                {
                    string invNo = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                    PrintIndoorBill(invNo);
                }
            }
        }

        private void PrintIndoorBill(string billId)
        {
            string query = "SELECT * FROM V_in_TestAdd_Bill WHERE BillNo='" + billId + "'";


            var dt = new IndoorReportViewer("in_TestAddBill", query, "Invoice Test Add", "V_in_TestAdd_Bill", "Indoor");
                dt.Show();
            
            
        }
        private void GridWidth(DataGridView dataGrid)
        {
            foreach (DataGridViewColumn dc in dataGrid.Columns)
            {
                dc.ReadOnly = !dc.Index.Equals(1);
            }
            dataGrid.Columns[3].ReadOnly = false;
           

            dataGrid.Columns[1].ReadOnly = true;

            // ReSharper disable once PossibleNullReferenceException
            dataGrid.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            // dataGrid.Columns["Result"].DefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold);


        }
   
        private void IndoorTestAddUi_Load(object sender, EventArgs e)
        {
            ClearText();
            GridWidth(dataGridView1);
        }

        private void searchTextBox_Enter_1(object sender, EventArgs e)
        {
            _textInfo = "bill";
            dataGridView2.DataSource = _rls.GetInvoiceList(DateTime.Now, searchTextBox.Text, "enter");
            // Hlp.GridColor(dataGridView2);
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Columns[0].Width = 65;
                dataGridView2.Columns[1].Width = 75;
                dataGridView2.Columns[2].Width = 160;
                dataGridView2.Columns[3].Visible = false;
                dataGridView2.Columns[4].Visible = false;


                // helpPanel.Location = new Point(12, 169);
                helpPanel.Visible = true;
                Hlp.GridFirstRowDeselect(dataGridView2);
            }
        }

        private void searchTextBox_TextChanged_1(object sender, EventArgs e)
        {
            _textInfo = "bill";
            dataGridView2.DataSource = _rls.GetInvoiceList(DateTime.Now, searchTextBox.Text, "change");
            // Hlp.GridColor(dataGridView2);
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Columns[0].Width = 65;
                dataGridView2.Columns[1].Width = 75;
                dataGridView2.Columns[2].Width = 160;
                dataGridView2.Columns[3].Visible = false;
                dataGridView2.Columns[4].Visible = false;


                // helpPanel.Location = new Point(12, 169);
                helpPanel.Visible = true;
                Hlp.GridFirstRowDeselect(dataGridView2);
            }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Delete)
            {
               string  gccode = dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[0].Value.ToString();

               _gt.DeleteInsertLab("DELETE FROM tb_IN_PATIENT_LEDGER WHERE trNo='"+ gccode +"'");
               _gt.DeleteInsertLab("DELETE FROM tb_in_TEST_ADD_LIST WHERE trNo='" + gccode + "'");
                
                MessageBox.Show(@"Delete Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (_gt.FnSeekRecordNewLab("tb_TestChart", "Id='" + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value + "' AND ChangeCharge=1"))
            {
                dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].ReadOnly = false;
                return;
            }
        }
    }
}
