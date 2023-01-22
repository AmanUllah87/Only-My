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
using SuperPathologyApp.Model.Indoor;

namespace SuperPathologyApp.UI
{
    public partial class DiagnosisBillReturnUi : Form
    {
       



        public DiagnosisBillReturnUi()
        {
            InitializeComponent();
            
            
           
        }
       
        private void ClearText()
        {
            Hlp.GridColor(dataGridView1);
            billDateTextBox.Text = Hlp.GetServerDate().ToString("dd-MMM-yyyy");
            genderComboBox.SelectedIndex = 0;

            billNoTextBox.Text = _gt.GetInvoiceNo(1);
            contactNoTextBox.Focus();
            contactNoTextBox.Text = "";
            ptNameTextBox.Text = "";
            yearTextBox.Text = "";
            addressTextBox.Text = "";
            refDrCodeTextBox.Text = "";
            refDrNameTextBox.Text = "";
            consDrCodeTextBox.Text = "";
            consDrNameTextBox.Text = "";
            dataGridView1.Rows.Clear();
          
           
            totDiscountTextBox.Text = "0";
            paidAmtTextBox.Text = "0";
            dueAmtTextBox.Text = "0";
            remarksTextBox.Text = "";
            monthTextBox.Text = "";
            yearTextBox.Text = "";
            dayTextBox.Text = "";
            saveAndPrintButton.Text = "Save && &Print";
            paidAmtTextBox.ReadOnly = false;
            dueAfterReturnTextBox.Text = "0";
            dueAmtTextBox.Text = "0";
            returnAmtTextBox.Text = "0";
            cashReturnTextBox.Text = "0";

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
                case 2:
                    dataGridView1.Columns[0].Width = 70;
                    dataGridView1.Columns[1].Width = 200;
                    dataGridView1.Columns[2].Width = 80;
                    dataGridView1.Columns[3].Width = 80;
                    dataGridView1.Columns[4].Width = 80;
                    dataGridView1.Columns[5].Width = 78;

                    break;
                case 6:
                    dataGridView1.RowHeadersVisible = false;
                    dataGridView1.CurrentCell = null;
                    dataGridView1.EnableHeadersVisualStyles = false;
                    dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                    dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                    dataGridView1.AllowUserToResizeRows = false;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Width = 200;
                    dataGridView1.Columns[2].Width = 345;

                    break;


            }
            dataGridView4.RowHeadersVisible = false;
            dataGridView4.CurrentCell = null;

           
            dataGridView4.EnableHeadersVisualStyles = false;
            dataGridView4.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
          

            dataGridView4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView4.AllowUserToResizeRows = false;







        }
   
      
    
     
   
      
   
        private void button1_Click(object sender, EventArgs e)
        {
            

            Hlp.AutoPrint = false;
            
            
            
          //  string invNo = invNoFirstPartTextBox.Text  + FourDigitTextBox.Text;
            var mdl = new List<TestCodeModel>();

           
         
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                //string result = dataGridView2.Rows[i].Cells[1].Value == null ? "" : dataGridView2.Rows[i].Cells[1].Value.ToString();
                //string parameterTestName = dataGridView2.Rows[i].Cells[0].Value == null? "": dataGridView2.Rows[i].Cells[0].Value.ToString();
                //string unitName = dataGridView2.Rows[i].Cells[2].Value == null? "": dataGridView2.Rows[i].Cells[2].Value.ToString();
                //string normalValue = dataGridView2.Rows[i].Cells[3].Value == null? "": dataGridView2.Rows[i].Cells[3].Value.ToString();
                //string machineName = dataGridView2.Rows[i].Cells[6].Value == null? "": dataGridView2.Rows[i].Cells[6].Value.ToString();
                //string headerName = dataGridView2.Rows[i].Cells[7].Value == null? "": dataGridView2.Rows[i].Cells[7].Value.ToString();
                //int groupSlNo = dataGridView2.Rows[i].Cells[8].Value == null? 0:Convert.ToInt32(dataGridView2.Rows[i].Cells[8].Value.ToString());
                //int parameterSlNo = dataGridView2.Rows[i].Cells[9].Value == null? 0: Convert.ToInt32(dataGridView2.Rows[i].Cells[9].Value.ToString());
                //string parameterName = dataGridView2.Rows[i].Cells[10].Value == null? "": dataGridView2.Rows[i].Cells[10].Value.ToString();
                //string reportingGroupName = dataGridView2.Rows[i].Cells[11].Value == null? "": dataGridView2.Rows[i].Cells[11].Value.ToString();
                //int isBold = dataGridView2.Rows[i].Cells[12].Value == null? 0: Convert.ToInt32(dataGridView2.Rows[i].Cells[12].Value.ToString());
                //string testCode = dataGridView2.Rows[i].Cells[13].Value == null? "": dataGridView2.Rows[i].Cells[13].Value.ToString();
                //string groupName = dataGridView2.Rows[i].Cells[14].Value == null? "": dataGridView2.Rows[i].Cells[14].Value.ToString();
                //string sampleNo = dataGridView2.Rows[i].Cells[16].Value == null? "": dataGridView2.Rows[i].Cells[16].Value.ToString();
                //string specimen = _gt.FncReturnFielValueLab("tb_Parameter_Definition", "TestCode='" + testCode + "'","Specimen");
                //string collTime = _gt.FncReturnFielValueLab("tb_LabSampleStatusInfo", "SampleNo='" + sampleNo + "'", "CollTime");
                //string collUser = _gt.FncReturnFielValueLab("tb_LabSampleStatusInfo", "SampleNo='" + sampleNo + "'", "CollUser");

               



                mdl.Add(new TestCodeModel()
                    {
                        PtName = ptNameTextBox.Text,
                     
              
                        //DrName = drNameTextBox.Text,
                        //ParameterTestName = parameterTestName,
                        //Result = result,
                        //UnitName = unitName,
                        //NormalValue = normalValue,
                        //MachineName = machineName,
                        //HeaderName = headerName,
                        //GroupSlNo = groupSlNo,
                        //ParameterSlNo = parameterSlNo,
                        //ParameterName = parameterName,
                        //ReportingGroupName = reportingGroupName,
                        //IsBold = isBold,
                        //TestCode = testCode,
                        //GroupName = groupName,
                        //SampleNo = sampleNo,
                        //MasterId = masterId,
                      
                        //UserName = collUser,
                        //TestName = testNameForView,
                        //SpecimenName = specimen,
                        //SampleCollectionTime = collTime,
                        //PrintBy = Hlp.UserName,
                        ////SampleCollectionUserName = collUser,
                    });

            }


           
          

           


            
        }


    

      


        

 

     

        private void ageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               
            }
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

        private void DiagnosisBillUi_Load(object sender, EventArgs e)
        {
            





        }

       
        private void CalculateAgeByDob(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(PastYearDate).Hours;
            int Minutes = Now.Subtract(PastYearDate).Minutes;
            int Seconds = Now.Subtract(PastYearDate).Seconds;
            yearTextBox.Text = Years.ToString().PadLeft(2,'0');
            monthTextBox.Text = Months.ToString().PadLeft(2, '0');
            dayTextBox.Text = Days.ToString().PadLeft(2, '0');
        }

      

      

        private void ptNameTextBox_Enter(object sender, EventArgs e)
        {
            ptNameTextBox.BackColor = Hlp.EnterFocus();
            helpPanel.Visible = false;

        }

        private void ptNameTextBox_Leave(object sender, EventArgs e)
        {
            ptNameTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void ageTextBox_Enter(object sender, EventArgs e)
        {
            yearTextBox.BackColor = Hlp.EnterFocus();

        }

        private void ageTextBox_Leave(object sender, EventArgs e)
        {
            yearTextBox.BackColor = Hlp.LeaveFocus();

        }



        private void refDrCodeTextBox_Enter(object sender, EventArgs e)
        {
            //textInfo = "2";
            refDrCodeTextBox.BackColor = Hlp.EnterFocus();
            textInfo = "refdr";
            helpPanel.Visible = true;
            HelpDataGridLoadByRefDr(dataGridView2, refDrCodeTextBox.Text);

        }

        private void refDrCodeTextBox_Leave(object sender, EventArgs e)
        {
            refDrCodeTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void consDrCodeTextBox_Enter(object sender, EventArgs e)
        {
            consDrCodeTextBox.BackColor = Hlp.EnterFocus();
            textInfo = "consDr";
            HelpDataGridLoadByRefDr(dataGridView2, consDrCodeTextBox.Text);
            helpPanel.Visible = true;
        }

        private void consDrCodeTextBox_Leave(object sender, EventArgs e)
        {
            consDrCodeTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void addressTextBox_Enter(object sender, EventArgs e)
        {
            addressTextBox.BackColor = Hlp.EnterFocus();
            helpPanel.Visible = false;
        }

        private void addressTextBox_Leave(object sender, EventArgs e)
        {
            addressTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void contactNoTextBox_Enter(object sender, EventArgs e)
        {
            contactNoTextBox.BackColor = Hlp.EnterFocus();
           
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
                    yearTextBox.Focus();
                }
            }
        }

        private void genderComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                addressTextBox.Focus();
            }
        }

        private void addressTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                refDrCodeTextBox.Focus();
            }
        }

       
        private void refDrCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (_gt.FnSeekRecordNewLab("tb_Doctor","Id='"+ refDrCodeTextBox.Text +"'"))
                {
                    refDrNameTextBox.Text = _gt.FncReturnFielValueLab("tb_Doctor", "Id='" + refDrCodeTextBox.Text + "'","Name");
                    consDrCodeTextBox.Focus();
                }
                else
                {
                    refDrCodeTextBox.Focus();
                }
                
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                textInfo = "refDr";
                dataGridView2.CurrentCell.Selected = true;
                dataGridView2.Focus();
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
        DiagnosisBillReturnGateway _gt = new DiagnosisBillReturnGateway();
        PatientGateway _pt = new PatientGateway();
        DoctorGateway _dr = new DoctorGateway();

        private void contactNoTextBox_TextChanged(object sender, EventArgs e)
        {
            //if (contactNoTextBox.Text.Length > 2)
            //{
            //    helpPanel.Visible = true;
            //   // helpPanel.Location = new Point(12, 53);
                
            //    HelpDataGridLoadByContactNo(dataGridView2);
            //}
            //else
            //{
            //    helpPanel.Visible = false;
            //}
        }

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
        private string textInfo = "";
        private void contactNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //if (contactNoTextBox.Text.Length > 2)
            //{
            //    helpPanel.Visible = true;
            //   // helpPanel.Location = new Point(12, 53);
            //}
            //else
            //{
            //    helpPanel.Visible = false;
            //}
           

            if (e.KeyCode==Keys.Enter)
            {
                if (contactNoTextBox.TextLength!=11)
                {
                    contactNoTextBox.Focus();
                    return;
                }
                
                
                //if (helpPanel.Visible==true)
                //{
                //    ptNameTextBox.Focus();
                //}

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
                        contactNoTextBox.Text = gcdesc;
                        var dt = _pt.GetRegisterPatientList("", Convert.ToInt32(gccode));
                        if (dt.Rows.Count > 0)
                        {
                            ptNameTextBox.Text = dt.Rows[0]["Name"].ToString();
                            genderComboBox.Text = dt.Rows[0]["Sex"].ToString();
                            addressTextBox.Text = dt.Rows[0]["Address"].ToString();
                            DateTime dob = Convert.ToDateTime(dt.Rows[0]["Dob"]);
                            CalculateAgeByDob(dob);
                        
                        }
                        refDrCodeTextBox.Focus();
                        break;
                    case "refDr":
                        refDrCodeTextBox.Text = gccode;
                        refDrNameTextBox.Text = gcdesc;
                        consDrCodeTextBox.Focus();
                        break;
                    case "consDr":
                        consDrCodeTextBox.Text = gccode;
                        consDrNameTextBox.Text = gcdesc;
                       
                        break;
                   



                }

            }
        }



        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void refDrCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            textInfo = "refdr";
            helpPanel.Visible = true;
          //  helpPanel.Location = new Point(12, 169);
            HelpDataGridLoadByRefDr(dataGridView2,refDrCodeTextBox.Text);
        }

        public void HelpDataGridLoadByRefDr(DataGridView dg,string search)
        {
            dg.DataSource = null;
          
            dg.DataSource = _dr.GetDoctorList(0, search);
            Hlp.GridFirstRowDeselect(dg);
          //  Hlp.GridColor(dg);
            dg.Columns[0].Width = 80;
            dg.Columns[1].Width = 200;
            dg.Columns[2].Width = 180;
            dg.Columns[3].Width = 100;
            
            dg.Columns[4].Visible = false;
            dg.Columns[5].Visible = false;
            dg.Columns[6].Visible = false;
            dg.Columns[7].Visible = false;


        }
        public void HelpDataGridLoadByTest(DataGridView dg, string search)
        {
            dg.DataSource = null;
            var _gt=new TestChartGateway();
            dg.DataSource = _gt.GetTestCodeList(0, search,0);
            Hlp.GridFirstRowDeselect(dg);
            dg.Columns.OfType<DataGridViewColumn>().ToList().ForEach(col => col.Visible = false);
           // Hlp.GridColor(dg);
            dg.Columns[0].Width = 100;
            dg.Columns[1].Width = 300;
            dg.Columns[2].Width = 150;
            dg.Columns[0].Visible = true;
            dg.Columns[1].Visible = true;
            dg.Columns[2].Visible = true;


        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void consDrCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            textInfo = "consDr";
            HelpDataGridLoadByRefDr(dataGridView2, consDrCodeTextBox.Text);
            helpPanel.Visible = true;
          //  helpPanel.Location = new Point(12, 169);
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       

        private void consDrCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (_gt.FnSeekRecordNewLab("tb_Doctor", "Id='" + consDrCodeTextBox.Text + "'"))
                {
                    consDrNameTextBox.Text = _gt.FncReturnFielValueLab("tb_Doctor", "Id='" + consDrCodeTextBox.Text + "'", "Name");
                  
                }
                else
                {
                    consDrCodeTextBox.Text = refDrCodeTextBox.Text;
                    consDrNameTextBox.Text = refDrNameTextBox.Text;
                  
                }
                
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                textInfo = "consDr";
                dataGridView2.CurrentCell.Selected = true;
                dataGridView2.Focus();
            }
        }

        private void testCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

               
                
               
                
                
                

               
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                textInfo = "test";
                dataGridView2.Rows[0].Selected = true;
                //dataGridView2.CurrentCell.Selected = true;
                dataGridView2.Focus();
            }
        }

        private void AddDataToGrid()
        {

            if (_gt.FnSeekRecordNewLab("tb_DOCTOR","Id='"+ refDrCodeTextBox.Text +"'")==false)
            {
                MessageBox.Show(@"Please Add Refered Name Please", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                refDrCodeTextBox.SelectAll();
                refDrCodeTextBox.Focus();
                return;
            }





         


            CalculateTotal();



        }
        private string CalculateTotal()
        {
            cashReturnTextBox.Text = "0";
            dueAfterReturnTextBox.Text = "0";
            double totAmt = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                totAmt += Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value);
            }
            returnAmtTextBox.Text = totAmt.ToString();

            double dueAfterRtn = Convert.ToDouble(Hlp.IsNumeric(dueAmtTextBox.Text) ? dueAmtTextBox.Text : "0") - totAmt;

            if (dueAfterRtn < 0)
            {
                cashReturnTextBox.Text = Math.Abs(dueAfterRtn).ToString();
                dueAfterReturnTextBox.Text = "0";
            }
            else
            {
                dueAfterReturnTextBox.Text = dueAfterRtn.ToString();
            }





            return "ok";


        }
      
     

     




     


  

       

        private void paidAmtTextBox_TextChanged(object sender, EventArgs e)
        {
            if (paidAmtTextBox.Text=="")
            {
                paidAmtTextBox.Text = "0";
            }
            
           
        }

        private void saveAndPrintButton_Click_1(object sender, EventArgs e)
        {
            if (CheckSuccess())
            {
                var aMdl = new BillModel
                {
                    BillNo = billNoTextBox.Text,
                    BillId=Convert.ToInt32(_gt.FncReturnFielValueLab("tb_BILL_MASTER","BillNo='"+ billNoTextBox.Text +"'","Id")),
                };


                aMdl.RefDrModel=new DoctorModel()
                {
                    DrId =  Convert.ToInt32(refDrCodeTextBox.Text),
                };
                aMdl.ConsDrModel = new DoctorModel()
                {
                    DrId = Convert.ToInt32(consDrCodeTextBox.Text),
                };
               


                
                aMdl.TotalAmt =Convert.ToDouble(amountWithVaqTextBox.Text);

                aMdl.TotalLessAmt = Convert.ToDouble(totDiscountTextBox.Text);
                aMdl.Remarks =remarksTextBox.Text;

               

                aMdl.BillReturn = new BillReturnModel()
                {
                   TotalReturnAmt = Convert.ToDouble(returnAmtTextBox.Text),
                   CashReturnAmt= Convert.ToDouble(cashReturnTextBox.Text),
                   DueAdjustAmt = Convert.ToDouble(Hlp.IsNumeric(dueAmtTextBox.Text)?dueAmtTextBox.Text:"0") - Convert.ToDouble(dueAfterReturnTextBox.Text),


                };



                var mdl = new List<TestChartModel>();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {

                   
                    mdl.Add(new TestChartModel()
                    {
                        TestId = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value.ToString()),
                        Charge = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value.ToString()),
                        LessAmt = Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value.ToString()),
                        RtnAmt = Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value.ToString()), 

                    });
                }
                aMdl.TestChartModel = mdl;






                string msg=_gt.SaveInvoice(aMdl,saveAndPrintButton.Text);
                if (msg == _gt.SaveSuccessMessage)
                {
                    
                    MessageBox.Show(_gt.SaveSuccessMessage, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearText();
                    //_gt.DeleteInsertLab("Update tb_BILL_MASTER SET  LastPrintPc='" + Environment.MachineName +"' WHERE Id='" + aMdl.BillId + "'");
                    //PrintInvoice(aMdl.BillId,"''");
                }
                else
                {
                    MessageBox.Show(msg, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }

      

        private bool CheckSuccess()
        {
            bool isChecked = true;

            if (ptNameTextBox.Text=="")
            {
                MessageBox.Show(@"Input your patient name",@"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ptNameTextBox.Focus();
                isChecked = false;
            }
            if (contactNoTextBox.Text == "")
            {
                MessageBox.Show(@"Input your patient contact", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                contactNoTextBox.Focus();
                isChecked = false;
            }

            if (_gt.FnSeekRecordNewLab("tb_Doctor","Id='"+ refDrCodeTextBox.Text +"'")==false)
            {
                MessageBox.Show(@"Invalid Doctor!!!Please Check.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                refDrCodeTextBox.Focus();
                isChecked = false;
            }
            if (_gt.FnSeekRecordNewLab("tb_Doctor", "Id='" + consDrCodeTextBox.Text + "'") == false)
            {
                MessageBox.Show(@"Invalid Doctor!!!Please Check.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                consDrCodeTextBox.Focus();
                isChecked = false;
            }

            if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "'") == false)
            {
                MessageBox.Show(@"Invalid Bill No.!!!Please Check.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                searchTextBox.Focus();
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
            searchTextBox.BackColor = Hlp.EnterFocus();

            textInfo = "enter";
            dataGridView2.DataSource = _gt.GetInvoiceList(DateTime.Now, searchTextBox.Text, "enter");
           // Hlp.GridColor(dataGridView2);
            if (dataGridView2.Rows.Count>0)
            {
                dataGridView2.Columns[0].Width = 80;
                dataGridView2.Columns[1].Width = 80;
                dataGridView2.Columns[2].Width = 230;
                dataGridView2.Columns[3].Width = 90;
                dataGridView2.Columns[4].Width = 80;
                helpPanel.Location = new Point(12, 169);
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
                    ClearText();
                    string invNo = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                    billNoTextBox.Text = invNo;
                    GetInvoiceDataForEdit(invNo);
                    helpPanel.Visible = false;
                }
            }
        }

        readonly ReportDocument _rprt=new ReportDocument();
        private void AutoPrintInvoice(string reportFileName, string query, string dataSetName, string title)
        {
            try
            {
                string comName = _gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ComName");
                string comAddress = _gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "Address");

                _gt.ConLab.Open();
                string path = Application.StartupPath;
                path = path + @"\Report\File\Diagnosis\";
               


                _rprt.Load(path + "" + reportFileName + ".rpt");
                //  _rprt.Load(path + "" + _reportFileName + ".rdlc");

                var cmd = new SqlCommand(query, _gt.ConLab);
                var sda = new SqlDataAdapter(cmd);
                var ds = new GroupReportDS();
                sda.Fill(ds, dataSetName);
                _rprt.SetDataSource(ds);
                _rprt.SetParameterValue("lcComName", comName);
                _rprt.SetParameterValue("lcComAddress", comAddress);
                _rprt.SetParameterValue("lcDateRange", "");
                _rprt.SetParameterValue("lcTitle", title);
                _rprt.PrintToPrinter(1, true, 0, 0);
                _rprt.Dispose();
                _rprt.Close();
               
                _gt.ConLab.Close();
            }
            catch (Exception exception)
            {
                if (_gt.ConLab.State == ConnectionState.Open)
                {
                    _gt.ConLab.Close();
                }
                MessageBox.Show(exception.Message);
            }
        }

        private void PrintInvoice(int invNo,string groupName)
        {
            string query = "EXEC Sp_Get_InvoicePrint " + invNo + "," + groupName + "";

            if (autoPrintCheckBox.Checked)
            {
                AutoPrintInvoice("Bill", query, "Sp_Get_InvoicePrint", "Invoice");
               
            
            
            
            }
            else
            {
                var dt = new LabReqViewer("Bill", query, "Invoice", "Sp_Get_InvoicePrint", "");
                dt.Show();
            }
            
            
            
            
            
            
            
            
            
            
            helpPanel.Visible = false;

            if (_gt.FnSeekRecordNewLab("tb_MASTER_INFO","GroupwiseBill=1"))
            {
                var billNo =new List<string>();
                var group= _gt.SearchDs(query);
                if (group.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <= group.Tables[0].Rows.Count - 1; i++)
                    {
                        billNo.Add(group.Tables[0].Rows[i]["SubProjectName"].ToString());
                    }
                }
                var groupwisePrint = billNo.Distinct().ToList();
                if (groupwisePrint.Count>1)
                {
                    foreach (var item in groupwisePrint)
                    {
                        if (item!="")
                        {
                            query = "EXEC Sp_Get_InvoicePrint " + invNo + ",'" + item + "'";


                            if (autoPrintCheckBox.Checked)
                            {
                                AutoPrintInvoice("BillGroup", query, "Sp_Get_InvoicePrint", "Invoice");
                            }
                            else
                            {
                                var dt = new LabReqViewer("BillGroup", query, "Invoice", "Sp_Get_InvoicePrint", item);
                                dt.Show();
                            }
                            
                            
                            
                            
                            
                            
                           
                        }
                    }
                }


            }



        
        
        }

        private void paidAmtTextBox_Enter(object sender, EventArgs e)
        {
            paidAmtTextBox.BackColor = Hlp.EnterFocus();

         






        }

        private void paidAmtTextBox_Leave(object sender, EventArgs e)
        {
            paidAmtTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (textInfo == "enter")
                {
                    string invNo = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                    int masterId = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + invNo + "'", "Id"));
                    if (_gt.FnSeekRecordNewLab("tb_DUE_COLL", "MasterId=" + masterId + ""))
                    {
                        MessageBox.Show(@"This bill has due collection.Please check.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {

                        if (MessageBox.Show(@"Do you want to request for cancel this bill?", @"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='" + Hlp.UserName + "' AND ParentName='Diagnosis' AND PermisionName='Bill-Delete'"))
                            {
                                _gt.DeleteInsertLab("INSERT INTO  DEL_RECORD_OF_BILL_DELETE  (BillNo, BillDate, BillTime, RegId, PatientName, MobileNo, Address, Age, Sex, RefDrId, UnderDrId, TotalAmt, LessAmt, LessFrom, CollAmt, Remarks, PostedBy,ModuleName,Status,MasterId,PcName,IpAddress) SELECT BillNo, BillDate, BillTime, RegId, PatientName, MobileNo, Address, Age, Sex, RefDrId, UnderDrId, TotalAmt, LessAmt, LessFrom, CollAmt, Remarks, '" + Hlp.UserName + "','Bill','Pending'," + masterId + ",'" + Environment.UserName + "','" + Hlp.IpAddress() + "' FROM tb_BILL_MASTER WHERE Id=" + masterId + "");
                                //_gt.DeleteInsertLab("DELETE FROM tb_BILL_MASTER WHERE Id=" + masterId + "");
                                //_gt.DeleteInsertLab("DELETE FROM tb_BILL_DETAIL WHERE MasterId=" + masterId + "");
                                //_gt.DeleteInsertLab("DELETE FROM tb_BILL_LEDGER WHERE MasterId=" + masterId + "");
                                MessageBox.Show(@"Bill cancel request success.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                searchTextBox.Focus();
                            }
                            else
                            {
                                MessageBox.Show(@"You need permission to do this task.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            
                        }
                    }
                }
                
                
                
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (dataGridView1.CurrentCell==null)
                {
                    return;
                }

                if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER","BillNo='" + billNoTextBox.Text + "'")==false)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                    CalculateTotal();
                    return;
                }
                if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='" + Hlp.UserName + "' AND permisionname='Bill-Can Remove Item When Edit'"))
                {
                    dataGridView1.Rows.RemoveAt(this.dataGridView1.CurrentCell.RowIndex);
                    CalculateTotal();
                }
                else
                {
                    MessageBox.Show(@"You need additional(Can Remove Item When Edit) previlage to do this task ", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }







            }


          
        }

    

        private void billNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode==Keys.Enter)
            //{
            //    if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='"+ Hlp.UserName +"' AND  ParentName='Diagnosis' AND PermisionName='Bill-Edit' "))
            //    {
            //        if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "' AND AdmId=0"))
            //        {
            //            GetInvoiceDataForEdit(billNoTextBox.Text);
            //            testCodeTextBox.Focus();
            //            saveAndPrintButton.Text = "&Update";
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show(@"You need permission to do this task.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}


           
            
            
        }

        private void GetInvoiceDataForEdit(string invNo)
        {
            try
            {
                var aMdl = _gt.GetInvoiceDataForEdit(invNo);

                ptNameTextBox.Text = aMdl.PatientName;
             
                genderComboBox.Text=aMdl.Sex ;
                addressTextBox.Text=aMdl.Address;
                contactNoTextBox.Text=aMdl.MobileNo;


                refDrCodeTextBox.Text = aMdl.RefDrModel.DrId.ToString();
                consDrCodeTextBox.Text = aMdl.ConsDrModel.DrId.ToString();
                refDrNameTextBox.Text = aMdl.RefDrModel.Name;
                consDrNameTextBox.Text = aMdl.ConsDrModel.Name;



           



                //totalAmtTextBox.Text=aMdl.TotalAmt.ToString();
                amountWithVaqTextBox.Text = aMdl.TotalAmt.ToString();
                totDiscountTextBox.Text=aMdl.TotalLessAmt.ToString();
                paidAmtTextBox.Text=aMdl.CollAmt.ToString();
                remarksTextBox.Text=aMdl.Remarks;
                dueAmtTextBox.Text = _gt.FncReturnFielValueLab("V_Due_Invoice_List", "BillNo='" + invNo + "'", "Balance");

                dataGridView1.Rows.Clear();
                foreach (var item in aMdl.TestChartModel)
                {
                    dataGridView1.Rows.Add(item.TestId, item.Name, item.Charge, item.LessAmt, 0, false);
                }
                dataGridView1.CurrentCell.Selected = false;


                yearTextBox.Text = aMdl.Age.Substring(0,2);
                if (aMdl.Age.Length > 2 && aMdl.Age.Length <= 8)
                {
                    try
                    {
                        monthTextBox.Text = aMdl.Age.Substring(4, 2);    
                    }
                    catch (Exception)
                    {
                        monthTextBox.Text="00";
                    }
                }

                if (aMdl.Age.Length > 8 )
                {
                    try
                    {
                        dayTextBox.Text = aMdl.Age.Substring(8, 2);
                    }
                    catch (Exception)
                    {
                        dayTextBox.Text = "00";
                    }

                }


                
                
              //  UpdateDateOfBirth();
             



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ageTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void yearTextBox_Enter(object sender, EventArgs e)
        {
            yearTextBox.BackColor = Hlp.EnterFocus();
        }

        private void yearTextBox_Leave(object sender, EventArgs e)
        {
            yearTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void monthTextBox_Enter(object sender, EventArgs e)
        {
            monthTextBox.BackColor = Hlp.EnterFocus();
        }

        private void monthTextBox_Leave(object sender, EventArgs e)
        {
            monthTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void dayTextBox_Enter(object sender, EventArgs e)
        {
            dayTextBox.BackColor = Hlp.EnterFocus();
        }

        private void dayTextBox_Leave(object sender, EventArgs e)
        {
            dayTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void yearTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                UpdateDateOfBirth();                
                monthTextBox.Focus();

            }

        }

        private void UpdateDateOfBirth()
        {
            if (yearTextBox.Text == "")
            {
                yearTextBox.Text = "0";
            }
            if (monthTextBox.Text == "")
            {
                monthTextBox.Text = "0";
            }
            if (dayTextBox.Text == "")
            {
                dayTextBox.Text = "0";
            }
            dobDateTimePicker.Value = Hlp.CalculateDob(DateTime.Now, Convert.ToInt32(yearTextBox.Text), Convert.ToInt32(monthTextBox.Text), Convert.ToInt32(dayTextBox.Text));

        }

        private void monthTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateDateOfBirth();
                dayTextBox.Focus();


            }

        }

        private void dayTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UpdateDateOfBirth();
                genderComboBox.Focus();

            }

        }

        private void yearTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void monthTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void dayTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView2.DataSource = _gt.GetInvoiceList(DateTime.Now, searchTextBox.Text,"change");
            Hlp.GridFirstRowDeselect(dataGridView2);
            // Hlp.GridColor(dataGridView2);
            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Columns[0].Width = 80;
                dataGridView2.Columns[1].Width = 80;
                dataGridView2.Columns[2].Width = 200;
                dataGridView2.Columns[3].Width = 90;
                dataGridView2.Columns[4].Width = 80;
                dataGridView2.Columns[5].Width = 100;

                helpPanel.Location = new Point(12, 169);
                helpPanel.Visible = true;
                Hlp.GridFirstRowDeselect(dataGridView2);
            }
        }

    


        private void genderComboBox_Enter(object sender, EventArgs e)
        {
            genderComboBox.BackColor = Hlp.EnterFocus();
        }

        private void genderComboBox_Leave(object sender, EventArgs e)
        {
            genderComboBox.BackColor = Hlp.LeaveFocus();
            if (genderComboBox.SelectedIndex == -1)
            {
                genderComboBox.SelectedIndex = 0;
            }
        }

        private void paidAmtTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                remarksTextBox.Focus();
            }

        }

        private void dayTextBox_TextChanged(object sender, EventArgs e)
        {

        }

      

        private void defaultResultComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                defaultResultComboBox.Visible = false;
                dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[6];
                dataGridView1.CurrentCell.Value = defaultResultComboBox.Text;
                // dataGridView2.Rows[10].Selected = true;
                dataGridView1.Focus();
            }
        }

     

        private void dataGridView3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.F1)
            {
                remarksTextBox.Focus();
               
            }
        }

        private void remarksTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode==Keys.Enter)
            {
                
                    saveAndPrintButton.Focus();
               
            }



        }

        private void label29_Click(object sender, EventArgs e)
        {
            
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void discountPcTextBox_TextChanged(object sender, EventArgs e)
        {
           
               

    
            
        }

 

        private void Edit_Click(object sender, EventArgs e)
        {
            var grp = new UpdatePateintInfo();
            grp.Show();
        }

        private void contactNoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

      

       



   


        private void printNoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

      
       

        private void saveAndPrintButton_Enter(object sender, EventArgs e)
        {
            saveAndPrintButton.BackColor = Color.Coral;
        }

        private void printNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                saveAndPrintButton.Focus();
            }
        }

        private void saveAndPrintButton_Leave(object sender, EventArgs e)
        {
            saveAndPrintButton.BackColor = Color.Green;
        }

        private void DiagnosisBillReturnUi_Load(object sender, EventArgs e)
        {
            Hlp.GridColor(dataGridView1);
            billDateTextBox.Text = Hlp.GetServerDate().ToString("dd-MMM-yyyy");
            genderComboBox.SelectedIndex = 0;

            billNoTextBox.Text = _gt.GetInvoiceNo(1);



            //dataGridView1.Columns[3].ReadOnly = false;
            foreach (DataGridViewColumn dc in dataGridView1.Columns)
            {
                dc.ReadOnly = !dc.Index.Equals(6);
            }


            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");

            Gridwidth(2);
            searchTextBox.Focus();
           
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null ? true : (!(bool)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value));

                if (Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == 1)
                {
                    double charge = Convert.ToDouble(dataGridView1.CurrentRow.Cells[2].Value);
                    double less = Convert.ToDouble(dataGridView1.CurrentRow.Cells[3].Value);
                    double rtnAmt = Math.Round(charge - less);
                    dataGridView1.CurrentRow.Cells[4].Value = rtnAmt.ToString();
                }
                else
                {
                    dataGridView1.CurrentRow.Cells[4].Value = "0";
                }
                CalculateTotal();
            }
        }

        private void searchTextBox_Leave(object sender, EventArgs e)
        {
            searchTextBox.BackColor = Hlp.LeaveFocus();

        }
       

        

        // ,
    }
}
