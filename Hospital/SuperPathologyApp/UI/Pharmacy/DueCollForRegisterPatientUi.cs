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
    public partial class DueCollForRegisterPatientUi : Form
    {
        public DueCollForRegisterPatientUi()
        {
            InitializeComponent();
        }

        readonly DueCollForRegisterPatientGateway _DCFI = new DueCollForRegisterPatientGateway();
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
       
        PatientGateway _pt = new PatientGateway();
        private void HelpDataGridLoadByContactNo(DataGridView dg)
        {

            dg.DataSource = _pt.GetRegisterPatientList(contactNoTextBox.Text, 0);
            Hlp.GridFirstRowDeselect(dg);
            // Hlp.GridColor(dg);
            dg.Columns[0].Visible = false;
            dg.Columns[1].Width = 90;
            dg.Columns[2].Width = 90;
            dg.Columns[3].Width = 200;
            dg.Columns[4].Width = 180;
            dg.Columns[5].Visible = false;
            dg.Columns[6].Visible = false;



        }
        private void contactNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (contactNoTextBox.TextLength != 11)
                {
                    contactNoTextBox.Focus();
                    return;
                }



                if (_gt.FnSeekRecordNewLab("tb_PATIENT", "ContactNo='" + contactNoTextBox.Text + "'") == true)
                {
                    helpPanel.Visible = true;
                    HelpDataGridLoadByContactNo(dataGridView2);
                }
                else
                {
                    ptNameTextBox.Focus();
                }

            }


            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                //textInfo = "1";
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
                        var dt = _pt.GetRegisterPatientList("", Convert.ToInt32(gccode));
                        if (dt.Rows.Count > 0)
                        {
                            admNotextBox.Text = gccode;
                            ptNameTextBox.Text = dt.Rows[0]["Name"].ToString();
                            sexTextBox.Text = dt.Rows[0]["Sex"].ToString();
                            addressTextBox.Text = dt.Rows[0]["Address"].ToString();
                            contactNoTextBox.Text = dt.Rows[0]["ContactNo"].ToString();
                            collAmttextBox.Focus();
                            helpPanel.Visible = false;
                            GetDueInvoiceData();           
                        }

                                   
                        
                        
                       
                      
                       // helpPanel.Visible = false;
                        break;
         
 


                }

            }
        }

        private void GetDueInvoiceData()
        {
            double totAmt = 0;
            var data = _DCFI.GetDueInvoiceDataByRegId(Convert.ToInt32(admNotextBox.Text));
            if (data.Count>0)
            {
                dataGridView1.Rows.Clear();
                foreach (var item in data)
                {
                    dataGridView1.Rows.Add(item.BillId, item.BillNo, item.BillDate.ToString("dd-MMM-yyyy"), item.TotalAmt, 0, item.TotalAmt,0);
                    totAmt += item.TotalAmt;
                }
            }
            totAmountTextBox.Text = totAmt.ToString();
            CalculateTotal();
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

     
        private string CalculateTotal()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[6].Value = 0;
            }           
            double totColl = Convert.ToDouble(Hlp.IsNumeric(collAmttextBox.Text) ? collAmttextBox.Text : "0");

            
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (totColl > 0)
                {
                    double netAmt = Convert.ToDouble(Hlp.IsNumeric(dataGridView1.Rows[i].Cells[5].Value.ToString()) ? dataGridView1.Rows[i].Cells[5].Value : "0");
                    if (netAmt <= totColl)
                    {
                        dataGridView1.Rows[i].Cells[6].Value = netAmt.ToString();
                        totColl -= netAmt;
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[6].Value = totColl;
                        totColl = 0;
                    }

                }
                
                

            }

           
           
          
            
           
            if (dataGridView1.Rows.Count>0)
            {
                dataGridView1.CurrentCell.Selected = false;
            }
           
            
            return "ok";


          

        }

        

        private void contactNoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
           // e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if ( dataGridView1.CurrentCell.ColumnIndex == 4) //Desired Column
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
            dataGridView1.Rows.Clear();
            dueCollDateTextBox.Text = Hlp.GetServerDate().ToString("dd-MMM-yyyy");
            
            contactNoTextBox.Text = "";
            admNotextBox.Text = "0";
            ptNameTextBox.Text = "";
            sexTextBox.Text = "";
            addressTextBox.Text = "";
            consDrNameTextBox.Text = "";
            remarksTextBox.Text = "";
            discounttextBox.Text = "";
            totAmount.Text = "";
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
                    Patient = new PatientModel()
                    {
                        RegId = Convert.ToInt32(admNotextBox.Text)
                    },
                };
                var mdl = new List<BillModel>();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    mdl.Add(new BillModel()
                    {
                        BillId = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value.ToString()),
                        BillNo = dataGridView1.Rows[i].Cells[1].Value.ToString(),
                        BillDate = Convert.ToDateTime(dataGridView1.Rows[i].Cells[2].Value.ToString()),
                        LessPc = Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value.ToString()),
                        CollAmt = Convert.ToDouble(dataGridView1.Rows[i].Cells[6].Value.ToString()),
                       
                    });
                }
                aMdl.Bill = mdl;


                aMdl.Remarks = remarksTextBox.Text;
                aMdl.TotLessAmt = Convert.ToDouble(Hlp.IsNumeric(discounttextBox.Text) ? discounttextBox.Text : "0");



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

            if (_gt.FnSeekRecordNewLab("tb_Patient","Id='"+ admNotextBox.Text +"'") == false)
            {
                MessageBox.Show(@"Input your patient name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                contactNoTextBox.Focus();
                isChecked = false;
            }
            if (dataGridView1.Rows.Count<1)
            {
                MessageBox.Show(@"No Data For Save", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                contactNoTextBox.Focus();
                isChecked = false;
            }
            if (Hlp.StringToDouble(discounttextBox.Text) == 0 && Hlp.StringToDouble(collAmttextBox.Text) == 0)
            {
                MessageBox.Show(@"Invalid Amount Plz Check", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            string query = "SELECT * FROM V_Reg_PH_Due_Coll_List WHERE TrNo    ='" + billId + "'";


            var dt = new PharmacyReportViewer("in_ph_due_coll", query, "Medicine Due Coll", "V_Reg_PH_Due_Coll_List", "Due Coll");
                dt.Show();
            
            
        }
        private void GridWidth(DataGridView dataGrid)
        {
            foreach (DataGridViewColumn dc in dataGrid.Columns)
            {
                dc.ReadOnly = !dc.Index.Equals(1);
            }
            dataGrid.Columns[4].ReadOnly = false;



            dataGridView1.Columns[4].DefaultCellStyle.BackColor = Hlp.GridHightLightColumn();
           // dataGridView1.Columns[6].DefaultCellStyle.BackColor = Hlp.GridHightLightColumn();


        }
   
        private void IndoorTestAddUi_Load(object sender, EventArgs e)
        {
            ClearText();
            GridWidth(dataGridView1);
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
            GridWidth(dataGridView1);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void collAmttextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cellValueDidNotWork = false;
                CalculateTotal();
            }
        }
        private bool cellValueDidNotWork=false;
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //cellValueDidNotWork = true;
            if (cellValueDidNotWork)
            {
                if (dataGridView1.CurrentCell == null)
                {
                    return;
                }
                if (dataGridView1.CurrentCell.ColumnIndex == 4)
                {
                    try
                    {
                        collAmttextBox.Text = "";
                        int rowIndex = dataGridView1.CurrentCell.RowIndex;
                        double charge = Convert.ToDouble(dataGridView1.Rows[rowIndex].Cells[3].Value);
                        double lessAmt = Convert.ToDouble(dataGridView1.Rows[rowIndex].Cells[4].Value);
                        dataGridView1.Rows[rowIndex].Cells[5].Value = Math.Round(charge - lessAmt);
                        double totLess = 0, totGridCollAmt = 0;
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            totLess += Convert.ToDouble(Hlp.IsNumeric(dataGridView1.Rows[i].Cells[4].Value.ToString()) ? dataGridView1.Rows[i].Cells[4].Value : "0");
                            dataGridView1.Rows[i].Cells[6].Value = "0";
                            totGridCollAmt += Convert.ToDouble(Hlp.IsNumeric(dataGridView1.Rows[i].Cells[6].Value.ToString()) ? dataGridView1.Rows[i].Cells[6].Value : "0");
                        }
                        discounttextBox.Text = totLess.ToString();
                        collAmttextBox.Text = totGridCollAmt.ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
  
            

        }

        private void collAmttextBox_TextChanged(object sender, EventArgs e)
        {
            //CalculateTotal();
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            cellValueDidNotWork = true;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            cellValueDidNotWork = true;
        }

        private void DueCollForIndoorUi_Load(object sender, EventArgs e)
        {
            ClearText();
            GridWidth(dataGridView1);
        }

        private void DueCollForRegisterPatientUi_Load(object sender, EventArgs e)
        {
            ClearText();
            GridWidth(dataGridView1);
        }
    }
}
