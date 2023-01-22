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
    public partial class DiagnosisBillUi : Form
    {
        



        public DiagnosisBillUi()
        {
            InitializeComponent();
            
            
           
        }
       
        private void ClearText()
        {
            Hlp.GridColor(dataGridView1);
            billDateTextBox.Text = Hlp.GetServerDate().ToString("dd-MMM-yyyy");
            genderComboBox.SelectedIndex = 0;
            discountFromComboBox.SelectedIndex = 0;
            discountTypeComboBox.SelectedIndex = 0;
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
            discountPcTextBox.Text = "";
            totalAmtTextBox.Text = "0";
            totDiscountTextBox.Text = "0";
            paidAmtTextBox.Text = "0";
            dueAmtTextBox.Text = "0";
            remarksTextBox.Text = "";
            monthTextBox.Text = "";
            yearTextBox.Text = "";
            dayTextBox.Text = "";
            saveAndPrintButton.Text = "Save && &Print";
            paidAmtTextBox.ReadOnly = false;
            admNoTextBox.Text = "";
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
            Hlp.GridColor(dataGridView1);
            billDateTextBox.Text = Hlp.GetServerDate().ToString("dd-MMM-yyyy");
            genderComboBox.SelectedIndex = 0;
            discountFromComboBox.SelectedIndex = 1;
            discountTypeComboBox.SelectedIndex = 0;
            billNoTextBox.Text = _gt.GetInvoiceNo(1);
            amPmComboBox.SelectedIndex = 0;
            timeComboBox.SelectedIndex = 0;


            if (_gt.FnSeekRecordNewLab("tb_DEFAULT_DELIVERY_DATETIME","HostName='"+ Environment.MachineName +"'"))
            {
                deliverydateTimePicker.Value = Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_DEFAULT_DELIVERY_DATETIME", "HostName='"+ Environment.MachineName +"'", "Date"));
                timeComboBox.Text = _gt.FncReturnFielValueLab("tb_DEFAULT_DELIVERY_DATETIME", "HostName='" + Environment.MachineName + "'", "TimeNumber");
                amPmComboBox.Text = _gt.FncReturnFielValueLab("tb_DEFAULT_DELIVERY_DATETIME", "HostName='" + Environment.MachineName + "'", "TimeAmPm");
            }
            else
            {
                _gt.DeleteInsertLab("INSERT INTO tb_DEFAULT_DELIVERY_DATETIME(Date,TimeNumber,TimeAmPm,HostName)VALUES('" + Hlp.GetServerDate().ToString("yyyy-MM-dd") + "','" + timeComboBox.Text + "','" + amPmComboBox.Text + "','" + Environment.MachineName + "')");
            }


            if (_gt.FnSeekRecordNewLab("tb_MASTER_INFO", "RemarksInGrid=0"))
            {
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[1].Width = 437;
            }




            if (_gt.FnSeekRecordNewLab("tb_MASTER_INFO", "UseFinancialService=1"))
            {
                var data = _gt.GetFinancial();
                dataGridView3.Rows.Clear();
                foreach (var mdl in data)
                {
                    dataGridView3.Rows.Add(mdl.MachineId, mdl.MachineName,"");
                }

                Hlp.GridColor(dataGridView3);
                dataGridView3.Columns["financialAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }


            //dataGridView1.Columns[3].ReadOnly = false;
            foreach (DataGridViewColumn dc in dataGridView1.Columns)
            {
                dc.ReadOnly = !dc.Index.Equals(6);
            }


           lblTime.Text= DateTime.Now.ToString("hh:mm:ss tt");



           UpdateHideShow();





        }

        private void UpdateHideShow()
        {
            if (_gt.FnSeekRecordNewLab("tb_MASTER_INFO", "UseIndoor=0"))
            {
                label31.Visible = false;
                admNoTextBox.Visible = false;
            }
            if (_gt.FnSeekRecordNewLab("tb_MASTER_INFO", "IsCheckPrintNo=0"))
            {
                lblPrintNo.Visible = false;
                printNoTextBox.Visible = false;
               // remarksTextBox.Size = new Size(393, 105);
              //  remarksTextBox.Location = new Point(53, 422);
            }




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

        private void testCodeTextBox_Enter(object sender, EventArgs e)
        {
            //if (_gt.FnSeekRecordNewLab("tb_DOCTOR","Id='"+ refDrCodeTextBox.Text +"'")==false)
            //{
            //    MessageBox.Show("Invalid Doctor Please Check", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //if (_gt.FnSeekRecordNewLab("tb_DOCTOR", "Id='" + consDrCodeTextBox.Text + "'") == false)
            //{
            //    MessageBox.Show("Invalid Doctor Please Check", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}



            testCodeTextBox.BackColor = Hlp.EnterFocus();
            helpPanel.Visible = false;
        }

        private void testCodeTextBox_Leave(object sender, EventArgs e)
        {
            testCodeTextBox.BackColor = Hlp.LeaveFocus();

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
        DiagnosisBillGateway _gt = new DiagnosisBillGateway();
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
                        testCodeTextBox.Focus();
                        break;
                    case "test":
                        testCodeTextBox.Text = gccode;
                        //consDrNameTextBox.Text = gcdesc;
                        AddDataToGrid();
                        testCodeTextBox.Text = "";
                        testCodeTextBox.Focus();
                        break;
                    case "Indoor":
                        admNoTextBox.Text = gccode;
                        var ds = Hlp.GetRegAndBedId(Convert.ToInt32(gccode));
                        ptNameTextBox.Text = ds.Patient.Name;
                        genderComboBox.Text = ds.Patient.Sex;
                        addressTextBox.Text = ds.Patient.Address;
                        contactNoTextBox.Text = ds.Patient.ContactNo;
                        refDrCodeTextBox.Text = ds.RefDoctor.DrId.ToString();
                        refDrNameTextBox.Text = ds.RefDoctor.Name;
                        consDrCodeTextBox.Text = ds.UnderDoctor.DrId.ToString();
                        consDrNameTextBox.Text = ds.UnderDoctor.Name;


                        try
                        {
                            string[] ssize = ds.Patient.Age.Split(null);
                            yearTextBox.Text = ssize[0].Replace("Y","").Trim();
                            monthTextBox.Text = ssize[1].Replace("M", "").Trim();
                            dayTextBox.Text = ssize[2].Replace("D", "").Trim();

                        }
                        catch (Exception){}
                       



                        testCodeTextBox.Focus();
                        //ChangeUiForIndoorPt();

                        break;


                }

            }
        }

        private void ChangeUiForIndoorPt()
        {
            paidAmtTextBox.Text = "0";
            paidAmtTextBox.Enabled = false;
            discountFromComboBox.Enabled = false;
            discountPcTextBox.Enabled = false;
            discountTypeComboBox.Enabled = false;

            refDrNameTextBox.Enabled = false;
            refDrCodeTextBox.Enabled = false;
            consDrNameTextBox.Enabled = false;
            consDrCodeTextBox.Enabled = false;

        }
        private void ChangeUiForOutdoorPt()
        {
            paidAmtTextBox.Text = "";
            admNoTextBox.Text = "";
            paidAmtTextBox.Enabled = true;
            discountFromComboBox.Enabled = true;
            discountPcTextBox.Enabled = true;
            discountTypeComboBox.Enabled = true;
            refDrNameTextBox.Enabled = true;
            refDrCodeTextBox.Enabled = true;
            consDrNameTextBox.Enabled = true;
            consDrCodeTextBox.Enabled = true;

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
            dg.Columns[8].Visible = false;
            dg.Columns[9].Visible = false;


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
                    testCodeTextBox.Focus();
                }
                else
                {
                    consDrCodeTextBox.Text = refDrCodeTextBox.Text;
                    consDrNameTextBox.Text = refDrNameTextBox.Text;
                    testCodeTextBox.Focus();
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

                if (_gt.FnSeekRecordNewLab("tb_TestChart", "Id='" + testCodeTextBox.Text + "'"))
                {
                    AddDataToGrid();
                    return;
                }
                
                
                if (testCodeTextBox.Text.Length> 0)
                {
                    if (dataGridView2.Rows.Count>0)
                    {

                        if (dataGridView2.Rows[0].Cells[0].Value==null)
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
                
                
                

                if (dataGridView1.Rows.Count>0)
                {
                    discountPcTextBox.Focus();
                }
                else
                {
                    testCodeTextBox.Focus();    
                }


            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                textInfo = "test";
                textInfo = "test";
                HelpDataGridLoadByTest(dataGridView2, testCodeTextBox.Text);
                helpPanel.Visible = true;
                helpPanel.Location = new Point(12, 232);









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







            int testCodeId = 0;
            if (_gt.IsDuplicate(testCodeTextBox.Text,dataGridView1)==false)
            {
                testCodeId =Convert.ToInt32(testCodeTextBox.Text);
                string descName = _gt.FncReturnFielValueLab("tb_TestChart", "Id='" + testCodeTextBox.Text + "'", "Name");
                string charge = _gt.FncReturnFielValueLab("tb_TestChart", "Id='" + testCodeTextBox.Text + "'", "Charge");
                int subProjectId = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_TestChart", "Id='" + testCodeTextBox.Text + "'", "SubProjectId"));
                string maxDiscount = "0";

                if (_gt.FnSeekRecordNewLab("tb_DrWiseHonorium", "DrId='" + refDrCodeTextBox.Text + "' AND SubProjectId=" + subProjectId + ""))
                {
                    string type = _gt.FncReturnFielValueLab("tb_DrWiseHonorium", "DrId='" + refDrCodeTextBox.Text + "' AND SubProjectId=" + subProjectId + "", "Type");
                    maxDiscount = _gt.FncReturnFielValueLab("tb_DrWiseHonorium", "DrId='" + refDrCodeTextBox.Text + "' AND SubProjectId=" + subProjectId + "", "HonouriamAmt");
                    if (type == "%")
                    {
                        double amt = Convert.ToDouble(charge) * Convert.ToDouble(maxDiscount) * 0.01;
                        maxDiscount = amt.ToString();
                    }

                }
                else
                {
                    maxDiscount = _gt.FncReturnFielValueLab("tb_TestChart", "Id='" + testCodeTextBox.Text + "'", "MaxDiscount");
                }
                



                dataGridView1.Rows.Add(testCodeTextBox.Text, descName, charge, maxDiscount, 0, maxDiscount);
                testCodeTextBox.Text = "";
                dataGridView1.CurrentCell.Selected = false;
                helpPanel.Visible = false;

            }
            else
            {
                MessageBox.Show(@"Duplicate Name Found", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                testCodeTextBox.SelectAll();
                return;
            }


            if (_gt.FnSeekRecordNewLab("tb_MASTER_INFO","TakeVaqPrice=1"))
            {
                var testChart = new TestChartGateway();
                var getVaqName = testChart.GetVaqListByTestId(testCodeId);
                int i = dataGridView1.Rows.Count;
                foreach (var item in getVaqName)
                {
                    if (_gt.IsDuplicate(item.VaqId.ToString(), dataGridView1) == false)
                    {
                        dataGridView1.Rows.Add(item.VaqId, _gt.FncReturnFielValueLab("tb_TESTCHART", "Id=" + item.VaqId + "", "Name"), item.Charge, 0, 0, 0);
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.PaleTurquoise;

                    }


                    i++;
                }
            }


            CalculateTotal();



        }
        private string CalculateTotal()
        {

            double totAmt = 0, withoutVaqtotAmt = 0, maxDisc = 0, totLess = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                totAmt += Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                maxDisc += Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);
                totLess += Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value);
            }

            if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='" + Hlp.UserName + "' AND PermisionName='Bill-Can Give Full Less' AND ParentName='Diagnosis'") == false)
            {
                if (totLess > maxDisc)
                {
                    MessageBox.Show(@"Maximum discount is:" + maxDisc, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    discountPcTextBox.Focus();
                    return "exit";
                }
            }





            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (_gt.FnSeekRecordNewLab("tb_TESTCHART", "Id='" + dataGridView1.Rows[i].Cells[0].Value + "' AND IsVaqItem=0"))
                {
                    withoutVaqtotAmt += Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                }
            }

            amountWithVaqTextBox.Text = totAmt.ToString();
            if (totDiscountTextBox.Text == "")
            {
                totDiscountTextBox.Text = @"0";
            }

            totalAmtTextBox.Text = withoutVaqtotAmt.ToString();

            
            
            if (_gt.FnSeekRecordNewLab("tb_in_ADMISSION", "Id='" + admNoTextBox.Text + "'") == false)
            {
                paidAmtTextBox.Text = Convert.ToString(totAmt - Convert.ToDouble(totDiscountTextBox.Text));
                dueAmtTextBox.Text = "0";
            }
            else {
                paidAmtTextBox.Text = "0";
                dueAmtTextBox.Text = Convert.ToString(totAmt - Convert.ToDouble(totDiscountTextBox.Text));
               // dueAmtTextBox.Text = totAmt.ToString();
            }

           



            //_gt.GridColor(dataGridView1);



            return "ok";




        }
        private string CalculateTotalJustForSave()
        {

            double totAmt = 0,  maxDisc = 0, totLess = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                totAmt += Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                maxDisc += Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);
                totLess += Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value);
            }

            if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS","UserName='" + Hlp.UserName +"' AND PermisionName='Bill-Can Give Full Less' AND ParentName='Diagnosis'") == false)
            {
                if (totLess > maxDisc)
                {
                    MessageBox.Show(@"Maximum discount is:" + maxDisc, @"Information", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    discountPcTextBox.Focus();
                    return "exit";
                }
            }
            return "ok";
        }

     

        private void testCodeTextBox_TextChanged(object sender, EventArgs e)
        
        {
            textInfo = "test";
            HelpDataGridLoadByTest(dataGridView2, testCodeTextBox.Text);
            helpPanel.Visible = true;
            helpPanel.Location = new Point(12, 232);

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

                UpdateDiscTotal("");
                
            }
        }

        private string UpdateDiscTotal(string comefrom)
        {
            switch (discountTypeComboBox.SelectedIndex)
            {
                case 1:
                    double totDiscount = Convert.ToDouble(totalAmtTextBox.Text) * 0.01 * Convert.ToDouble(discountPcTextBox.Text);
                    totDiscountTextBox.Text = totDiscount.ToString();
                    break;
                case 0:
                    totDiscountTextBox.Text = discountPcTextBox.Text;
                    break;

            }
            UpdateLessIntoGrid();

            if (comefrom=="Paid")
            {
                CalculateTotal();
                paidAmtTextBox.Focus();
                return "ok";
            }
            else if (CalculateTotal() == "ok")
            {
                discountFromComboBox.Focus();
                return "ok";
            }
            else if (CalculateTotal() == "Exit")
            {
                discountPcTextBox.Focus();
                return "exit";
            }
            else
            {
                discountPcTextBox.Focus();
                return "exit";
            }
        }

        private void UpdateLessIntoGrid()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (discountPcTextBox.Text=="")
                {
                    dataGridView1.Rows[i].Cells[4].Value = 0;
                }
                else
                {
                    if (discountFromComboBox.Text == "Doctor")
                    {
                        double gridAmt = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                        double hnrLess = Math.Round(gridAmt * Convert.ToDouble(totDiscountTextBox.Text) / Convert.ToDouble(amountWithVaqTextBox.Text), 2);
                        double hnrAmt = Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);

                        dataGridView1.Rows[i].Cells[4].Value = hnrLess;
                        dataGridView1.Rows[i].Cells[5].Value = Math.Round(hnrAmt - hnrLess);
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells[4].Value = 0;
                        double hnrAmt = Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);
                        dataGridView1.Rows[i].Cells[5].Value = hnrAmt;
                    
                    }
                    
                    
                    
                }
            }
        }

        private void discountFromComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                paidAmtTextBox.Focus();
                UpdateHnrAmount();
            }
        }

        private void UpdateHnrAmount()
        {
            double totHnrAmt = 0;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                totHnrAmt += Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);
            }
            
            switch (discountFromComboBox.SelectedIndex)
            {
                case 0://Doctor
                    if (Convert.ToDouble(totDiscountTextBox.Text)>totHnrAmt)
                    {
                        MessageBox.Show(@"Maximum discount is:"+totHnrAmt, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        discountPcTextBox.Focus();
                        discountPcTextBox.SelectAll();
                    }
                    else
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            double gridAmt=Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                            double hnrLess =Math.Round(gridAmt*Convert.ToDouble(totDiscountTextBox.Text)/Convert.ToDouble(totalAmtTextBox.Text),2);
                            double hnrPay=Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value)-hnrLess;
                            dataGridView1.Rows[i].Cells[4].Value = hnrLess;
                            dataGridView1.Rows[i].Cells[5].Value = hnrPay;

                        }
                        paidAmtTextBox.Focus();
                        paidAmtTextBox.SelectAll();
                    }


                    break;
                case 1://Company
                        paidAmtTextBox.Focus();
                        paidAmtTextBox.SelectAll();
                    break;

                case 2://Both
                    
                    
                    double discountAmt = Convert.ToDouble(_gt.IsNumeric(totDiscountTextBox.Text)?totDiscountTextBox.Text:"0");
                    if (discountAmt>0)
                    {
                        discountAmt = discountAmt / 2;
                    }
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        double gridAmt = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                        double hnrLess = Math.Round(gridAmt * Convert.ToDouble(discountAmt) / Convert.ToDouble(totalAmtTextBox.Text), 2);
                        double hnrPay = Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value) - hnrLess;
                        if (_gt.FnSeekRecordNewLab("tb_TESTCHART", "Id='" + dataGridView1.Rows[i].Cells[0].Value + "' AND IsVaqItem=0"))
                        {
                            dataGridView1.Rows[i].Cells[4].Value = hnrLess;
                            dataGridView1.Rows[i].Cells[5].Value = hnrPay;
                        
                        }
                    }
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        totHnrAmt += Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);
                    }
                    if (totHnrAmt<discountAmt)
                    {
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            double gridAmt = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                            double hnrLess = Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);
                            double hnrPay = 0;
                            dataGridView1.Rows[i].Cells[4].Value = hnrLess;
                            dataGridView1.Rows[i].Cells[5].Value = hnrPay;
                        }
                    }
                    paidAmtTextBox.Focus();
                    paidAmtTextBox.SelectAll();
                    break;
            }
        }

        private void paidAmtTextBox_TextChanged(object sender, EventArgs e)
        {
            if (paidAmtTextBox.Text=="")
            {
                paidAmtTextBox.Text = "0";
            }
            
            
            double dueAmt = Convert.ToDouble(Hlp.IsNumeric(totalAmtTextBox.Text)?totalAmtTextBox.Text:"0") - Convert.ToDouble(Hlp.IsNumeric(totDiscountTextBox.Text)?totDiscountTextBox.Text:"0") -Convert.ToDouble(Hlp.IsNumeric( paidAmtTextBox.Text)?paidAmtTextBox.Text:"0");
            if (dueAmt<0)
            {
                CalculateTotal();
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
                var aMdl = new BillModel
                {
                    PatientName = ptNameTextBox.Text,
                    Sex = genderComboBox.Text,
                    Address = addressTextBox.Text,
                    MobileNo = contactNoTextBox.Text,
                    Dob = dobDateTimePicker.Value,
                    BillNo = billNoTextBox.Text,
                    DeliveryDate = deliverydateTimePicker.Value,
                    DeliveryTimeAmPm = amPmComboBox.Text,
                    DeliveryNumber = timeComboBox.Text,
 
                };

                aMdl.RegId= Hlp.InsertIntoPatient(aMdl.PatientName,aMdl.Address,aMdl.MobileNo,aMdl.Sex,aMdl.Dob);

                if (yearTextBox.Text!="0")
                {
                    aMdl.Age = yearTextBox.Text + "Y ";
                }
                if (monthTextBox.Text != "0")
                {
                    aMdl.Age += monthTextBox.Text.PadLeft(2,'0') + "M ";
                }
                if (dayTextBox.Text != "0")
                {
                    aMdl.Age += dayTextBox.Text.PadLeft(2,'0') + "D";
                }

                aMdl.RefDrModel=new DoctorModel()
                {
                    DrId =  Convert.ToInt32(refDrCodeTextBox.Text),
                };
                aMdl.ConsDrModel = new DoctorModel()
                {
                    DrId = Convert.ToInt32(consDrCodeTextBox.Text),
                };
                aMdl.Admission = new AdmissionModel()
                {
                    AdmId = Convert.ToInt32(Hlp.IsNumeric(admNoTextBox.Text)?admNoTextBox.Text:"0"),
                };


                if (discountPcTextBox.Text == "" || discountPcTextBox.Text == "0")
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
                if (amountWithVaqTextBox.Text=="")
                {
                    amountWithVaqTextBox.Text = "0";
                }
                
                
                aMdl.TotalAmt =Convert.ToDouble(amountWithVaqTextBox.Text);

                aMdl.TotalLessAmt = Convert.ToDouble(totDiscountTextBox.Text);
                aMdl.CollAmt = Convert.ToDouble(paidAmtTextBox.Text);
                aMdl.Remarks =remarksTextBox.Text;

                var mdl = new List<TestChartModel>();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {

                   
                    mdl.Add(new TestChartModel()
                    {
                        TestId = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value.ToString()),
                        Charge = Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value.ToString()),
                        DefaulHonouriam = Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value.ToString()),
                        
                        
                        HnrLess = Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value.ToString()), 
                        HnrToPay= Convert.ToDouble(dataGridView1.Rows[i].Cells[5].Value.ToString()),
                        GridRemarks = dataGridView1.Rows[i].Cells[6].Value == null ? "" : dataGridView1.Rows[i].Cells[6].Value.ToString(),

                    });
                }
                aMdl.TestChartModel = mdl;



                if (_gt.FnSeekRecordNewLab("tb_MASTER_INFO", "UseFinancialService=1"))
                {
                    var fncMdl = new List<MachineModel>();
                    for (int i = 0; i < dataGridView3.Rows.Count; i++)
                    {
                        string  amtstr = dataGridView3.Rows[i].Cells[2].Value == null ? "" : dataGridView3.Rows[i].Cells[2].Value.ToString();
                        if (_gt.IsNumeric(amtstr))
                        {
                            fncMdl.Add(new MachineModel()
                            {
                                MachineId = Convert.ToInt32(dataGridView3.Rows[i].Cells[0].Value.ToString()),
                                Amount = Convert.ToDouble(dataGridView3.Rows[i].Cells[2].Value.ToString()),
                            });    
                        }
                        
                    }
                    aMdl.FinancialModel = fncMdl;
                }



                string msg=_gt.SaveInvoice(aMdl,saveAndPrintButton.Text);
                if (msg == _gt.SaveSuccessMessage)
                {
                    
                    MessageBox.Show(_gt.SaveSuccessMessage, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearText();
                    _gt.DeleteInsertLab("Update tb_BILL_MASTER SET  LastPrintPc='" + Environment.MachineName +"' WHERE Id='" + aMdl.BillId + "'");
                    PrintInvoice(aMdl.BillId,"''");
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
            //if (contactNoTextBox.Text.Length <= 10)//Aman
            //{
            //    MessageBox.Show(@"Contact No is less then 11", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    contactNoTextBox.Focus();
            //    isChecked = false;
            //}
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

            if (discountPcTextBox.Text.Length >0)
            {
                if (totDiscountTextBox.Text=="0")
                {
                    MessageBox.Show(@"Please check discount", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    discountPcTextBox.Focus();
                    isChecked = false;
                }
            }
            if (CalculateTotalJustForSave() == "exit")
            {
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
            dataGridView2.DataSource = _gt.GetInvoiceList(Convert.ToDateTime(billDateTextBox.Text), searchTextBox.Text, "enter");
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
                    string invNo = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                    int billId =Convert.ToInt32(_gt.FncReturnFielValueLab("tb_BILL_MASTER","BillNo='"+ invNo +"'","Id"));
                    _gt.DeleteInsertLab("Update tb_BILL_MASTER SET  LastPrintPc='" + Environment.MachineName + "' WHERE Id='" + billId + "'");
                    PrintInvoice(billId,"''");
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
                if (_gt.FnSeekRecordNewLab("tb_MASTER_INFO", "IsCheckPrintNo=1"))
                {
                    int noOfPrint =Hlp.IsNumeric(printNoTextBox.Text)?Convert.ToInt32(printNoTextBox.Text):0;
                    if (noOfPrint>0)
                    {
                        for (int i = 0; i < noOfPrint-1; i++)
                        {
                            AutoPrintInvoice("Bill", query, "Sp_Get_InvoicePrint", "Invoice");
                        }
                    }
                }
            
            
            
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

            UpdateDiscTotal("Paid");









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


            //if (e.KeyCode == Keys.ControlKey)
            //{
            //    int rowIndex = dataGridView1.CurrentCell.RowIndex;
            //  //  int colIndex = dataGridView1.CurrentCell.ColumnIndex;

            //    Show_Combobox(rowIndex, 6);
            //}
        }

        private void Show_Combobox(int iRowIndex, int iColumnIndex)
        {
            _gt.LoadComboBoxForDefault("SELECT 0 AS Id,Name AS Description FROM tb_DOCTOR", defaultResultComboBox);


            int x = 0;
            int y = 0;
            int Width = 0;
            int height = 0;
            Rectangle rect = default(Rectangle);
            rect = dataGridView1.GetCellDisplayRectangle(iColumnIndex, iRowIndex, false);
            x = rect.X + dataGridView1.Left;
            y = rect.Y + dataGridView1.Top;
            Width = rect.Width;
            height = rect.Height;
            defaultResultComboBox.SetBounds(x, y, Width, height);
            defaultResultComboBox.Visible = true;
            defaultResultComboBox.Focus();
            defaultResultComboBox.DroppedDown = true;
            //defaultResultComboBox.Focus();
        }


        private void billNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='"+ Hlp.UserName +"' AND  ParentName='Diagnosis' AND PermisionName='Bill-Edit' "))
                {
                    //if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "' AND AdmId=0"))
                    //{
                    //    //------------------------Bill edit Only for Digital--------------
                    //    DateTime maxInvDate = Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "1=1", "MAX(BillDate)"));
                    //    DateTime invDate = Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "'", "BillDate"));
                    //    if (invDate < maxInvDate)
                    //    {
                    //        MessageBox.Show(@"You can not edit invoice in backdate", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //        return;
                    //    }
                    //    //------------------------------------------------------------------
                    //    GetInvoiceDataForEdit(billNoTextBox.Text);
                    //    testCodeTextBox.Focus();
                    //    saveAndPrintButton.Text = "&Update";
                    //}


                    if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "'"))
                    {
                        //------------------------Bill edit Only for Digital--------------
                        DateTime maxInvDate = Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "1=1", "MAX(BillDate)"));
                        DateTime invDate = Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "'", "BillDate"));
                        if (invDate < maxInvDate)
                        {
                            MessageBox.Show(@"You can not edit invoice in backdate", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        //------------------------------------------------------------------
                        GetInvoiceDataForEdit(billNoTextBox.Text);
                        testCodeTextBox.Focus();
                        saveAndPrintButton.Text = "&Update";
                    }









                }
                else
                {
                    MessageBox.Show(@"You need permission to do this task.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }


           
            
            
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
                admNoTextBox.Text = aMdl.Admission.AdmId.ToString();


                if (aMdl.LessFrom!="")
                {
                    discountFromComboBox.Text = aMdl.LessFrom;
                }
                if (aMdl.LessType != "")
                {
                    discountTypeComboBox.Text = aMdl.LessType;
                }
                if (aMdl.LessPc != 0)
                {
                    discountPcTextBox.Text = aMdl.LessPc.ToString();
                }



                totalAmtTextBox.Text=aMdl.TotalAmt.ToString();
                amountWithVaqTextBox.Text = aMdl.TotalAmt.ToString();
                totDiscountTextBox.Text=aMdl.TotalLessAmt.ToString();
                paidAmtTextBox.Text=aMdl.CollAmt.ToString();
                remarksTextBox.Text=aMdl.Remarks;


                dataGridView1.Rows.Clear();
                foreach (var item in aMdl.TestChartModel)
                {
                    dataGridView1.Rows.Add(item.TestId, item.Name, item.Charge, item.DefaulHonouriam, item.HnrLess, item.HnrToPay);
                }
                dataGridView1.CurrentCell.Selected = false;


                if (_gt.FnSeekRecordNewLab("tb_in_admission","Id='"+ aMdl.Admission.AdmId +"'")==false)
                {
                    yearTextBox.Text = aMdl.Age.Substring(0, 2);
                    if (aMdl.Age.Length > 2 && aMdl.Age.Length <= 8)
                    {
                        try
                        {
                            monthTextBox.Text = aMdl.Age.Substring(4, 2);
                        }
                        catch (Exception)
                        {
                            monthTextBox.Text = "00";
                        }
                    }

                    if (aMdl.Age.Length > 8)
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
                    UpdateDateOfBirth();
                }

               


                
                
              
             



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

        private void discountTypeComboBox_Enter(object sender, EventArgs e)
        {
            discountTypeComboBox.BackColor = Hlp.EnterFocus();
        }

        private void discountTypeComboBox_Leave(object sender, EventArgs e)
        {
            discountTypeComboBox.BackColor = Hlp.LeaveFocus();
            if (discountTypeComboBox.SelectedIndex==-1)
            {
                discountTypeComboBox.SelectedIndex = 0;
            }

        }

        private void discountTypeComboBox_DragEnter(object sender, DragEventArgs e)
        {
            discountFromComboBox.BackColor = Hlp.EnterFocus();
        }

        private void discountFromComboBox_Enter(object sender, EventArgs e)
        {
            discountFromComboBox.BackColor = Hlp.EnterFocus();
        }

        private void discountFromComboBox_Leave(object sender, EventArgs e)
        {

            discountFromComboBox.BackColor = Hlp.LeaveFocus();
            if (discountFromComboBox.SelectedIndex == -1)
            {
                discountFromComboBox.SelectedIndex = 1;
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
            if (e.KeyCode==Keys.F1)
            {
                if (_gt.FnSeekRecordNewLab("tb_MASTER_INFO", "UseFinancialService=1"))
                {
                    panel2.Visible = true;
                    dataGridView3.Rows[0].Cells[2].Selected = true;
                    dataGridView3.Focus();
                }
            }







        }

        private void dayTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {
            _gt.DeleteInsertLab("DELETE FROM tb_DEFAULT_DELIVERY_DATETIME WHERE HostName='" + Environment.MachineName + "'");
            _gt.DeleteInsertLab("INSERT INTO tb_DEFAULT_DELIVERY_DATETIME(Date,TimeNumber,TimeAmPm,HostName)VALUES('" + Hlp.GetServerDate().ToString("yyyy-MM-dd") + "','" + timeComboBox.Text + "','" + amPmComboBox.Text + "','" + Environment.MachineName + "')");
            MessageBox.Show(@"Save Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count>0)
            {
                double gridAmt = 0;
                for (int i = 0; i < dataGridView3.Rows.Count; i++)
                {
                    gridAmt += Convert.ToDouble(dataGridView3.Rows[i].Cells[2].Value == "" ? "0" : dataGridView3.Rows[i].Cells[2].Value.ToString());
                }
                paidAmtTextBox.ReadOnly = true;
                paidAmtTextBox.Text = gridAmt.ToString();
            }
            

        }

        private void dataGridView3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.F1)
            {
                remarksTextBox.Focus();
                panel2.Visible = false;
            }
        }

        private void remarksTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                if (_gt.FnSeekRecordNewLab("tb_MASTER_INFO", "UseFinancialService=1"))
                {
                    panel2.Visible = true;
                    dataGridView3.Rows[0].Cells[2].Selected = true;
                    dataGridView3.Focus();
                }
            }
            if (e.KeyCode==Keys.Enter)
            {
                if (printNoTextBox.Visible == true)
                {
                    printNoTextBox.Focus();
                }
                else
                {
                    saveAndPrintButton.Focus();
                }
            }



        }

        private void label29_Click(object sender, EventArgs e)
        {
            
            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                dataGridView3.Rows[i].Cells[2].Value = "" ;
            }
            paidAmtTextBox.ReadOnly = false;
           
            panel2.Visible = false;
            paidAmtTextBox.SelectAll();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void discountPcTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_gt.IsNumeric(discountPcTextBox.Text))
            {
                if ((discountPcTextBox.Text =="0")||(discountPcTextBox.Text ==""))
                {
                    UpdateDiscTotal("");
                    discountPcTextBox.Text = ""; 
                }
               

    
            }
            
        }

 

        private void Edit_Click(object sender, EventArgs e)
        {
            var grp = new UpdatePateintInfo();
            grp.Show();
        }

        private void contactNoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

      

        private void admNoTextBox_Enter(object sender, EventArgs e)
        {
            admNoTextBox.BackColor = Hlp.EnterFocus();
            HelpDataGridLoadByPatientId();
        }

        private void admNoTextBox_Leave(object sender, EventArgs e)
        {
            admNoTextBox.BackColor = Hlp.LeaveFocus();

        }


        private void HelpDataGridLoadByPatientId()
        {
            dataGridView2.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,AdmNo,AdmDate,PtName,ContactNo,BedName As Bed FROM V_Admission_List WHERE ReleaseStatus=0 AND (convert(varchar,Id)+PtName+BedName+ContactNo) LIKE '%"+ admNoTextBox.Text +"%'");
            Hlp.GridFirstRowDeselect(dataGridView2);
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Width = 70;
            dataGridView2.Columns[2].Width = 90;
            dataGridView2.Columns[3].Width = 200;
            dataGridView2.Columns[4].Width = 90;
            dataGridView2.Columns[5].Width = 90;
            helpPanel.Visible = true;
            textInfo = "Indoor";
        }

        private void admNoTextBox_TextChanged(object sender, EventArgs e)
        {
            if (admNoTextBox.Text.Length==0)
            {
                ChangeUiForOutdoorPt();
            }
        }

        private void admNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                textInfo = "Indoor";
                if (dataGridView2.CurrentCell!=null)
                {
                    dataGridView2.CurrentCell.Selected = true;

                }
                dataGridView2.Focus();
            }
        }

        private void printNoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void printNoTextBox_Enter(object sender, EventArgs e)
        {
            printNoTextBox.BackColor = Hlp.EnterFocus();
        }

        private void printNoTextBox_Leave(object sender, EventArgs e)
        {
            printNoTextBox.BackColor = Hlp.LeaveFocus();
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

        private void searchTextBox_Leave(object sender, EventArgs e)
        {
            searchTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void discountTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
       

        

        // ,
    }
}
