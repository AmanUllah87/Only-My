using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Gateway.Diagnosis;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Diagnosis;
using SuperPathologyApp.Report;
using SuperPathologyApp.Report.DataSet;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace SuperPathologyApp.UI
{
    public partial class DiagnosisDueCollUi : Form
    {
        



        public DiagnosisDueCollUi()
        {
            InitializeComponent();
            
            
           
        }
       
        private void ClearText()
        {
          
            dueCollDateTextBox.Text = Hlp.GetServerDate().ToString("dd-MMM-yyyy");
            dueCollNoTextBox.Text = _gt.GetInvoiceNo(2);
            
            contactNoTextBox.Focus();
            contactNoTextBox.Text = "";
            ptNameTextBox.Text = "";
            ageTextBox.Text = "";
          
            consDrNameTextBox.Text = "";
          
            discountPcTextBox.Text = "";
         
          
            dueAmtTextBox.Text = "0";
            totDiscountTextBox.Text = "0";
            balanceTextBox.Text = "0";
            paidAmtTextBox.Text = "0";
            remarksTextBox.Text = "";
         
        }


        public void Gridwidth(int no)
        {
            switch (no)
            {
                case 1:
                    dataGridView4.Columns[0].Width = 85;
                    dataGridView4.Columns[1].Width = 75;
                    dataGridView4.Columns[2].Width = 130;
                    dataGridView4.Columns[3].Width = 60;
                    break;
               


            }
            dataGridView4.RowHeadersVisible = false;
            dataGridView4.CurrentCell = null;

           
            dataGridView4.EnableHeadersVisualStyles = false;
            dataGridView4.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
     
          

            dataGridView4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView4.AllowUserToResizeRows = false;







        }
   
      
    
        private void GridWidth(DataGridView dataGrid)
        {
            dataGrid.Columns[0].Width = 175;
            dataGrid.Columns[1].Width = 220;
            dataGrid.Columns[2].Width = 60;
            dataGrid.Columns[3].Width = 180;
            dataGrid.Columns[15].HeaderText = @"Test Name";
            dataGrid.Columns[4].Visible = false;
            dataGrid.Columns[5].Visible = false;
            dataGrid.Columns[6].Visible = false;
            dataGrid.Columns[7].Visible = false;
            dataGrid.Columns[8].Visible = false;
            dataGrid.Columns[9].Visible = false;
            dataGrid.Columns[10].Visible = false;
            dataGrid.Columns[11].Visible = false;
            dataGrid.Columns[12].Visible = false;
            dataGrid.Columns[14].Visible = false;
            dataGrid.Columns[13].Visible = false;

            
            dataGrid.RowHeadersVisible = false;
            dataGrid.CurrentCell = null;
            dataGrid.Columns[3].ReadOnly = false;
            //dataGrid.Columns[4].ReadOnly = false;

            foreach (DataGridViewColumn dc in dataGrid.Columns)
            {
                dc.ReadOnly = !dc.Index.Equals(1);
            }
            dataGrid.Columns[2].ReadOnly = false;
            // ReSharper disable once PossibleNullReferenceException
            dataGrid.Columns["Result"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //    dataGrid.Columns["Result"].DefaultCellStyle.Font =dataGrid.Columns["Result"].DefaultCellStyle.Font,FontStyle.Bold;



        }
   
      
   
       


    

      


        

 

     

        private void ageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

  

  




    

      

      

        private void saveAndPrintButton_Click(object sender, EventArgs e)
        {
            //string ff = _reportFileName;




           




















            
           

           



     
        }

        readonly DbConnection db = new DbConnection();


    
        
        
      

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView4.CurrentRow != null)
            {
                string invNo = dataGridView4.CurrentRow.Cells[0].Value.ToString();
               // LabReportquery = "SELECT " + testNameForView + " AS TestName, * FROM VW_GET_LAB_REPORT_VIEW WHERE InvNo='" + invNo + "' AND InvDate='" + invDateDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND IsPrint=1   ";
            }
         //   var dt = new FrmReportViewer(_reportFileName,LabReportquery);
          //  dt.ShowDialog();
        }

    
        private void ReportPrintUi_Click(object sender, EventArgs e)
        {
            
        }

        private void ReportPrintUi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                saveAndPrintButton.PerformClick();
            }
        }

      

        private void ptNameTextBox_Enter(object sender, EventArgs e)
        {
            ptNameTextBox.BackColor = Hlp.EnterFocus();
           // helpPanel.Visible = false;

        }

        private void ptNameTextBox_Leave(object sender, EventArgs e)
        {
            ptNameTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void ageTextBox_Enter(object sender, EventArgs e)
        {
            ageTextBox.BackColor = Hlp.EnterFocus();

        }

        private void ageTextBox_Leave(object sender, EventArgs e)
        {
            ageTextBox.BackColor = Hlp.LeaveFocus();

        }


        private void contactNoTextBox_Enter(object sender, EventArgs e)
        {
            contactNoTextBox.BackColor = Hlp.EnterFocus();
            HelpDataGridLoadByDueList(dataGridView2,contactNoTextBox.Text);
            textInfo = "1";
        }

        private void contactNoTextBox_Leave(object sender, EventArgs e)
        {
            contactNoTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void ptNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (ptNameTextBox.Text == "")
                {
                    ptNameTextBox.Focus();
                }
                else
                {
                    ageTextBox.Focus();
                }
            }
        }


       
      
      

        private void remarksTextBox_Enter(object sender, EventArgs e)
        {
            remarksTextBox.BackColor = Hlp.EnterFocus();
        }

        private void remarksTextBox_Leave(object sender, EventArgs e)
        {
            remarksTextBox.BackColor = Hlp.LeaveFocus();
        }
        DiagnosisDueCollGateway _gt = new DiagnosisDueCollGateway();
      

        private void contactNoTextBox_TextChanged(object sender, EventArgs e)
        {
            HelpDataGridLoadByDueList(dataGridView2, contactNoTextBox.Text);
        }


        private string textInfo = "";
        private void contactNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode==Keys.Down)
            {
                textInfo = "1";
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

                switch (textInfo)
                {
                    case "1":
                        //contactNoTextBox.Text = gccode;
                        var dt = _gt.GetDueList(Convert.ToInt32(gccode), "");
                        if (dt.Rows.Count > 0)
                        {
                            billNoTextBox.Text = dt.Rows[0]["BillNo"].ToString();
                            billDateTextBox.Text =Convert.ToDateTime(dt.Rows[0]["BillDate"].ToString()).ToString("dd-MMM-yyyy");
                            ptNameTextBox.Text = dt.Rows[0]["PatientName"].ToString();
                            ageTextBox.Text = _gt.FncReturnFielValueLab("tb_BILL_MASTER","Id='"+ gccode +"'","Age");
                            int consId = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "Id='" + gccode + "'", "UnderDrId"));
                            consDrNameTextBox.Text = _gt.FncReturnFielValueLab("tb_DOCTOR", "Id='" + consId + "'", "Name");
                            balanceTextBox.Text = dt.Rows[0]["Balance"].ToString();
                            contactNoTextBox.Text =_gt.FncReturnFielValueLab("tb_BILL_MASTER","Id='"+ gccode +"'","MobileNo");
                            discountPcTextBox.Focus();
                        }
                    
                    
                    
                    
                    
                    
                    
                    //var dt = _pt.GetRegisterPatientList("", Convert.ToInt32(gccode));
                        //if (dt.Rows.Count > 0)
                        //{
                        //    ptNameTextBox.Text = dt.Rows[0]["Name"].ToString();
                        
                        //    DateTime dob = Convert.ToDateTime(dt.Rows[0]["Dob"]);
                        //    ageTextBox.Text = Hlp.CalculateAgeByDob(dob);
                        
                        //}
                    
                        break;
     
                    case "test":
                        //testCodeTextBox.Text = gccode;
                        //consDrNameTextBox.Text = gcdesc;
                      //  AddDataToGrid();

                        break;

                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

    

        private void HelpDataGridLoadByDueList(DataGridView dg,string search)
        {
            dg.DataSource = null;

            dg.DataSource = _gt.GetDueList(0, search);
           // Hlp.GridColor(dg);
            dg.Columns[0].Visible = false;
            dg.Columns[1].Width = 80;
            dg.Columns[2].Width = 80;
            dg.Columns[3].Width = 160;
            dg.Columns[4].Width = 100;
            dg.Columns[5].Width= 80;
            if (dg.Rows.Count>0)
            {
                dg.Rows[0].Selected = false;
            }
            

        }
 
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       

     

       

        

        private void CalculateTotal()
        {




            //double totAmt = 0;
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    totAmt += Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
            //}
            //totalAmtTextBox.Text = totAmt.ToString();
            //if (totDiscountTextBox.Text=="")
            //{
            //    totDiscountTextBox.Text = @"0";
            //}
            
            //paidAmtTextBox.Text =Convert.ToString(totAmt - Convert.ToDouble(totDiscountTextBox.Text));
            //dueAmtTextBox.Text= "0";



            //_gt.GridColor(dataGridView1);








        }

      

        private void discountPcTextBox_Enter(object sender, EventArgs e)
        {
            discountPcTextBox.BackColor = Hlp.EnterFocus();
        }

        private void discountPcTextBox_Leave(object sender, EventArgs e)
        {
            discountPcTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void discountPcTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (discountPcTextBox.Text.Length==0)
                {
                    paidAmtTextBox.SelectAll();
                    paidAmtTextBox.Focus();
                }
                else
                {
                    discountTypeComboBox.Focus();
                }

            }
        }

        private void discountTypeComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                switch (discountTypeComboBox.SelectedIndex)
                {
                    case 0:
                        //double totDiscount = Convert.ToDouble(totalAmtTextBox.Text)*0.01*Convert.ToDouble(discountPcTextBox.Text);
                        //totDiscountTextBox.Text = totDiscount.ToString();
                        break;
                    case 1:
                      //  totDiscountTextBox.Text = discountPcTextBox.Text;
                        break;

                }
                
                
                
                
                
                
                CalculateTotal();
                
                discountFromComboBox.Focus();
            }
        }

        private void discountFromComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                UpdateHnrAmount();
              
            }
        }

        private void UpdateHnrAmount()
        {

            switch (discountTypeComboBox.SelectedIndex)
            {
                case 1://TK
                    double totDisc =Math.Round(Convert.ToDouble(balanceTextBox.Text)*0.01*Convert.ToDouble(discountPcTextBox.Text));
                    totDiscountTextBox.Text = totDisc.ToString();
                    break;
                case 0://%
                    totDiscountTextBox.Text = discountPcTextBox.Text;
                    break;

            }
            
            
            
            
            
            
            
            
            
            double totHnrAmt = Convert.ToDouble(_gt.FncReturnFielValueLab("tb_BILL_DETAIL", "BillNo='" + billNoTextBox.Text + "' ", "Isnull(SUM(HnrAmt),0)"));
            double lessThisInvoice = 0;

            if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "' AND LessFrom='Doctor'"))
            {
                lessThisInvoice = Convert.ToDouble(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "' AND LessFrom='Doctor'", "Isnull(LessAmt,0)"));
            }
            
            
            
            totHnrAmt -= lessThisInvoice;

            switch (discountFromComboBox.SelectedIndex)
            {
                case 0://Doctor
                    if (Convert.ToDouble(totDiscountTextBox.Text) > totHnrAmt)
                    {
                        MessageBox.Show(@"Maximum discount is:" + totHnrAmt, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        discountPcTextBox.Focus();
                        discountPcTextBox.SelectAll();
                        return;
                    }
                    else
                    {
                        if (totDiscountTextBox.Text == ""){totDiscountTextBox.Text = "0";}

                        paidAmtTextBox.Text = Convert.ToString(Convert.ToDouble(balanceTextBox.Text) - Convert.ToDouble(totDiscountTextBox.Text));
                        if (Convert.ToDouble(paidAmtTextBox.Text) < 0)
                        {
                            discountPcTextBox.SelectAll();
                            discountPcTextBox.Focus();
                        }
                        else
                        {
                            paidAmtTextBox.Focus();
                            paidAmtTextBox.SelectAll();
                        }
                    }
                    break;
                case 1://Company
                    if (totDiscountTextBox.Text == ""){totDiscountTextBox.Text = "0";}
                    paidAmtTextBox.Text = Convert.ToString(Convert.ToDouble(balanceTextBox.Text) - Convert.ToDouble(totDiscountTextBox.Text));
                    if (Convert.ToDouble(paidAmtTextBox.Text)<0)
                    {
                        discountPcTextBox.SelectAll();
                        discountPcTextBox.Focus();
                    }
                    else
                    {
                        paidAmtTextBox.Focus();
                        paidAmtTextBox.SelectAll();
                    }
                    break;

            }












        }

        private void paidAmtTextBox_TextChanged(object sender, EventArgs e)
        {
            if (paidAmtTextBox.Text=="")
            {
                paidAmtTextBox.Text = "0";
            }
            if (totDiscountTextBox.Text == "")
            {
                totDiscountTextBox.Text = "0";
            }            
            
            double dueAmt = Convert.ToDouble(balanceTextBox.Text) - Convert.ToDouble(totDiscountTextBox.Text) -Convert.ToDouble(paidAmtTextBox.Text);
            if (dueAmt < 0)
            {
                paidAmtTextBox.SelectAll();
            }
            else
            {
               dueAmtTextBox.Text = dueAmt.ToString();
            }
        }

        private void saveAndPrintButton_Click_1(object sender, EventArgs e)
        {
            if (CheckSuccess())
            {
                var aMdl = new BillModel();
                aMdl.BillNo = billNoTextBox.Text;
                aMdl.BillId =Convert.ToInt32(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + aMdl.BillNo + "'", "Id"));
               




                if (discountPcTextBox.Text == ""||discountPcTextBox.Text == "0")
                {
                    aMdl.LessFrom = "";
                    aMdl.LessType = "";
                }
                else
                {
                    aMdl.LessFrom = discountFromComboBox.Text;
                    aMdl.LessType = discountTypeComboBox.Text;
                    aMdl.LessPc =Convert.ToDouble(discountPcTextBox.Text);
                }
                
                
                
              //  aMdl.TotalAmt =Convert.ToDouble(totalAmtTextBox.Text);

                aMdl.TotalLessAmt = Convert.ToDouble(totDiscountTextBox.Text);
                aMdl.CollAmt = Convert.ToDouble(paidAmtTextBox.Text);
                aMdl.Remarks =remarksTextBox.Text;












                string msg = _gt.SaveDueColl(aMdl);
                if (msg == _gt.SaveSuccessMessage)
                {
                    MessageBox.Show(_gt.SaveSuccessMessage, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearText();
                    PrintInvoice(aMdl.BillId.ToString());

                }


            }
        }

        private bool CheckSuccess()
        {
            bool isChecked = true;

            if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER","BillNo='"+ billNoTextBox.Text +"'")==false)
            {
                MessageBox.Show(@"Invalid Bill No.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                contactNoTextBox.Focus();
                isChecked = false;
            }
            if (Convert.ToDouble(dueAmtTextBox.Text)<0)
            {
                MessageBox.Show(@"Invalid Bill Amount", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                paidAmtTextBox.Focus();
                isChecked = false;
            }
            
            if (Convert.ToDouble(paidAmtTextBox.Text) >Convert.ToDouble(balanceTextBox.Text))
            {
                MessageBox.Show(@"Invalid Amount", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                paidAmtTextBox.Focus();
                isChecked = false;
            }
            



            return isChecked;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        private void searchTextBox_Enter(object sender, EventArgs e)
        {
            textInfo = "enter";
            dataGridView2.DataSource = _gt.GetDueCollList(Convert.ToDateTime(dueCollDateTextBox.Text),searchTextBox.Text);
          //  Hlp.GridColor(dataGridView2);
            if (dataGridView2.Rows.Count>0)
            {
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].Width = 80;
                dataGridView2.Columns[2].Width = 80;
                dataGridView2.Columns[3].Width = 180;
                dataGridView2.Columns[4].Width = 90;
                dataGridView2.Columns[5].Width = 70;
                dataGridView2.Columns[2].Visible = false;
              //  helpPanel.Location = new Point(12, 169);
                helpPanel.Visible = true;
               
                    dataGridView2.Rows[0].Selected = false;
                
            }
        }

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentCell.Selected)
            {
                if (dataGridView2.CurrentRow != null)
                {
                    string invNo = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                    PrintInvoice(invNo);
                }
            }
        }

        private void PrintInvoice(string invNo)
        {
            string query = "EXEC Sp_Get_InvoicePrint '" + invNo + "',''";
            var dt = new LabReqViewer("Bill", query, "Invoice", "Sp_Get_InvoicePrint","");
            dt.ShowDialog();
            //helpPanel.Visible = false;
        }

        private void paidAmtTextBox_Enter(object sender, EventArgs e)
        {
            paidAmtTextBox.BackColor = Hlp.EnterFocus();
        }

        private void paidAmtTextBox_Leave(object sender, EventArgs e)
        {
            paidAmtTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void DiagnosisDueCollUi_Load(object sender, EventArgs e)
        {
          //  Hlp.GridColor(dataGridView1);
            dueCollDateTextBox.Text = Hlp.GetServerDate().ToString("dd-MMM-yyyy");
           // genderComboBox.SelectedIndex = 0;
            discountFromComboBox.SelectedIndex = 1;
            discountTypeComboBox.SelectedIndex = 0;
            dueCollNoTextBox.Text = _gt.GetInvoiceNo(2);
            this.Location = new Point(126, 48);
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Delete)
            {
                if (textInfo=="enter")
                {
                    if (MessageBox.Show(@"Do you want to cancel this due collection?", @"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        string invNo = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                        _gt.DeleteInsertLab(@"INSERT INTO  DEL_RECORD_OF_BILL_DELETE  (BillNo, BillDate, BillTime, RegId, PatientName, MobileNo, Address, Age, Sex, RefDrId, UnderDrId, TotalAmt, LessAmt, LessFrom, CollAmt, Remarks, PostedBy,ModuleName,Status,MasterId,PcName,IpAddress) SELECT a.TrNo, a.TrDate, a.TrTime, b.RegId, b.PatientName, b.MobileNo, b.Address, b.Age, b.Sex, b.RefDrId, b.UnderDrId, a.TotalAmt,   a.LessAmt, a.LessFrom, a.TotalAmt , a.Remarks, '" + Hlp.UserName + "','Due-Coll','Pending',b.Id,'" + Environment.UserName + "','" + Hlp.IpAddress() + "'  FROM tb_DUE_COLL a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id WHERE a.TrNo='" + invNo + "'");                        
                        //_gt.DeleteInsertLab("DELETE FROM tb_DUE_COLL WHERE TrNo='" + invNo + "'");
                        //_gt.DeleteInsertLab("DELETE FROM tb_BILL_LEDGER WHERE TrNo='" + invNo + "'");
                        MessageBox.Show(@"Due collection cancel request success.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        searchTextBox.Focus();
                    }
                } 
            }
        }

        private void discountTypeComboBox_Enter(object sender, EventArgs e)
        {
            discountTypeComboBox.BackColor = Hlp.EnterFocus();
        }

        private void discountTypeComboBox_Leave(object sender, EventArgs e)
        {
            discountTypeComboBox.BackColor = Hlp.LeaveFocus();

            if (discountTypeComboBox.SelectedIndex == -1)
            {
                discountTypeComboBox.SelectedIndex = 0;
            }
        }


        private void discountFromComboBox_Enter(object sender, EventArgs e)
        {
            discountFromComboBox.BackColor = Hlp.EnterFocus();
        }

        private void discountFromComboBox_Leave(object sender, EventArgs e)
        {
            discountFromComboBox.BackColor = Hlp.LeaveFocus();


        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            textInfo = "enter";
            dataGridView2.DataSource = _gt.GetDueCollList(DateTime.Now, searchTextBox.Text);
            //  Hlp.GridColor(dataGridView2);
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].Width = 80;
                dataGridView2.Columns[2].Width = 80;
                dataGridView2.Columns[3].Width = 180;
                dataGridView2.Columns[4].Width = 90;
                dataGridView2.Columns[5].Width = 70;
                dataGridView2.Columns[2].Visible = false;
                //  helpPanel.Location = new Point(12, 169);
                helpPanel.Visible = true;

                dataGridView2.Rows[0].Selected = false;

            }
        }
    }
}
