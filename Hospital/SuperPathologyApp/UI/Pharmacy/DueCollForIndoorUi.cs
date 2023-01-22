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
using SuperPathologyApp.Gateway.Pharmacy;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Indoor;
using SuperPathologyApp.Report;
using SuperPathologyApp.Model.Diagnosis;

namespace SuperPathologyApp.UI.Pharmacy
{
    public partial class DueCollForIndoorUi : Form
    {
        public DueCollForIndoorUi()
        {
            InitializeComponent();
        }

        readonly DueCollForIndoorGateway _DCFI = new DueCollForIndoorGateway();
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
                        admNotextBox.Text = gccode;
                        dueAmtTextBox.Text = _gt.FncReturnFielValueLab("V_ph_Due_Invoice_List", "AdmId='"+ gccode +"' AND Status='Indoor' ","ROUND(DueAmt,2)");
                        totalLessTextBox.Focus();
                        
                        
                       
                      
                       // helpPanel.Visible = false;
                        break;
         
 


                }

            }
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

              

               
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                _textInfo = "test";
                dataGridView2.Rows[0].Selected = true;
                //dataGridView2.CurrentCell.Selected = true;
                dataGridView2.Focus();
            }
        }

     
      

        

        private void contactNoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
           // e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
           
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
           
            dueCollDateTextBox.Text = Hlp.GetServerDate().ToString("dd-MMM-yyyy");
            
            contactNoTextBox.Text = "";
            admNotextBox.Text = "0";
            ptNameTextBox.Text = "";
            sexTextBox.Text = "";
            addressTextBox.Text = "";
            consDrNameTextBox.Text = "";
            remarksTextBox.Text = "";
           
            dueCollNoTextBox.Text =_gt.GetInvoiceNo(5);
            collAmttextBox.Text = "";
            contactNoTextBox.Focus();
           
        }

        private void saveAndPrintButton_Click(object sender, EventArgs e)
        {
            if (CheckSuccess())
            {
                var aMdl = new ReleaseModel()
                {
                    Admission = new AdmissionModel()
                    {
                        AdmId = Convert.ToInt32(admNotextBox.Text)
                    },
                };
                var mdl = new List<BillModel>();

                aMdl.Bill = mdl;


                aMdl.Remarks = remarksTextBox.Text;
                aMdl.TotLessAmt = Convert.ToDouble(Hlp.IsNumeric(totalLessTextBox.Text) ? totalLessTextBox.Text : "0");
                aMdl.TotCollAmt = Convert.ToDouble(Hlp.IsNumeric(collAmttextBox.Text) ? collAmttextBox.Text : "0");



                string msg = _DCFI.Save(aMdl);
                if (msg == _gt.SaveSuccessMessage)
                {

                    MessageBox.Show(_gt.SaveSuccessMessage, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearText();
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
            if (collAmttextBox.Text =="")
            {
                MessageBox.Show(@"Input your Coll.Amount", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                collAmttextBox.Focus();
                isChecked = false;
            }
            if (Hlp.StringToDouble(dueAmtTextBox.Text) <0)
            {
                MessageBox.Show(@"Invalid Amount", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                collAmttextBox.Focus();
                isChecked = false;
            }

           

            return isChecked;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        


        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    cellValueDidNotWork=true;
            //}
        }

        private void searchTextBox_Enter(object sender, EventArgs e)
        {
            _textInfo = "bill";
            //dataGridView2.DataSource = _DCFI.GetInvoiceList(DateTime.Now, searchTextBox.Text, "enter");
            //// Hlp.GridColor(dataGridView2);
            //if (dataGridView2.Rows.Count > 0)
            //{
            //    dataGridView2.Columns[0].Width = 80;
            //    dataGridView2.Columns[1].Width = 80;
            //    dataGridView2.Columns[2].Width = 200;
            //    dataGridView2.Columns[3].Visible = false;
            //    dataGridView2.Columns[4].Visible = false;
            //    dataGridView2.Columns[5].Visible = false;

            //   // helpPanel.Location = new Point(12, 169);
            //    helpPanel.Visible = true;
            //    Hlp.GridFirstRowDeselect(dataGridView2);
            //}
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            //dataGridView2.DataSource = _DCFI.GetInvoiceList(DateTime.Now, searchTextBox.Text, "change");
            //Hlp.GridFirstRowDeselect(dataGridView2);
            //// Hlp.GridColor(dataGridView2);
            //if (dataGridView2.Rows.Count > 0)
            //{
            //    dataGridView2.Columns[0].Width = 80;
            //    dataGridView2.Columns[1].Width = 80;
            //    dataGridView2.Columns[2].Width = 200;
            //    dataGridView2.Columns[3].Visible = false;
            //    dataGridView2.Columns[4].Visible = false;
            //    dataGridView2.Columns[5].Visible = false;

            //    //helpPanel.Location = new Point(12, 169);
            //    helpPanel.Visible = true;
            //    Hlp.GridFirstRowDeselect(dataGridView2);
            //}
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
            string query = "SELECT * FROM V_In_PH_Due_Coll_List WHERE TrNo    ='" + billId + "'";


            var dt = new PharmacyReportViewer("in_ph_due_coll", query, "Medicine Due Coll", "V_In_PH_Due_Coll_List", "Due Coll");
                dt.Show();
            
            
        }
        private void GridWidth(DataGridView dataGrid)
        {
            foreach (DataGridViewColumn dc in dataGrid.Columns)
            {
                dc.ReadOnly = !dc.Index.Equals(1);
            }
            dataGrid.Columns[4].ReadOnly = false;



        }
   
        private void IndoorTestAddUi_Load(object sender, EventArgs e)
        {
            ClearText();
          
        }

        private void searchTextBox_Enter_1(object sender, EventArgs e)
        {
            _textInfo = "bill";
            dataGridView2.DataSource = _DCFI.GetDueCollList(DateTime.Now, searchTextBox.Text);
            // Hlp.GridColor(dataGridView2);
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Columns[0].Width = 90;
                dataGridView2.Columns[1].Width = 75;
                dataGridView2.Columns[2].Width = 130;
                dataGridView2.Columns[3].Visible = false;
                dataGridView2.Columns[4].Visible = false;
                dataGridView2.Columns[5].Width = 50;


                // helpPanel.Location = new Point(12, 169);
                helpPanel.Visible = true;
                Hlp.GridFirstRowDeselect(dataGridView2);
            }
        }

        private void searchTextBox_TextChanged_1(object sender, EventArgs e)
        {
            _textInfo = "bill";
            dataGridView2.DataSource = _DCFI.GetDueCollList(DateTime.Now, searchTextBox.Text);
            // Hlp.GridColor(dataGridView2);
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Columns[0].Width = 90;
                dataGridView2.Columns[1].Width = 75;
                dataGridView2.Columns[2].Width = 130;
                dataGridView2.Columns[3].Visible = false;
                dataGridView2.Columns[4].Visible = false;
                dataGridView2.Columns[5].Width = 50;


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
               MessageBox.Show(@"Delete Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void DiagnosisDueCollForIndoorUi_Load(object sender, EventArgs e)
        {
            ClearText();
         
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void collAmttextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                remarksTextBox.Focus();
               
            }
        }
     
     
        private void collAmttextBox_TextChanged(object sender, EventArgs e)
        {
            balAmtTextBox.Text = (Hlp.StringToDouble(dueAmtTextBox.Text) - Hlp.StringToDouble(totalLessTextBox.Text) - Hlp.StringToDouble(collAmttextBox.Text)).ToString();
        }


        private void DueCollForIndoorUi_Load(object sender, EventArgs e)
        {
            ClearText();
           
        }

        private void collAmttextBox_Enter(object sender, EventArgs e)
        {
            collAmttextBox.BackColor = Hlp.EnterFocus();
        }

        private void collAmttextBox_Leave(object sender, EventArgs e)
        {
            collAmttextBox.BackColor = Hlp.LeaveFocus();
        }

        private void remarksTextBox_Enter(object sender, EventArgs e)
        {
            remarksTextBox.BackColor = Hlp.EnterFocus();
        }

        private void remarksTextBox_Leave(object sender, EventArgs e)
        {
            remarksTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void remarksTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                saveAndPrintButton.Focus();
            }
        }

        private void totalLessTextBox_Enter(object sender, EventArgs e)
        {
            totalLessTextBox.BackColor = Hlp.EnterFocus();

        }

        private void totalLessTextBox_Leave(object sender, EventArgs e)
        {
            totalLessTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void totalLessTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                collAmttextBox.Focus();
                collAmttextBox.SelectAll();
            }
        }

        private void totalLessTextBox_TextChanged(object sender, EventArgs e)
        {
            balAmtTextBox.Text = (Hlp.StringToDouble(dueAmtTextBox.Text) - Hlp.StringToDouble(totalLessTextBox.Text)).ToString();
            collAmttextBox.Text = balAmtTextBox.Text;
        }
    }
}
