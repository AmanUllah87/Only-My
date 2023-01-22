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
using SuperPathologyApp.Gateway.Pharmacy;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Diagnosis;
using SuperPathologyApp.Model.Pharmacy;
using SuperPathologyApp.Report;
using SuperPathologyApp.Report.DataSet;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using SuperPathologyApp.Model.Indoor;

namespace SuperPathologyApp.UI
{
    public partial class MedicineSalesUi : Form
    {
       



        public MedicineSalesUi()
        {
            InitializeComponent();
            
            
           
        }
       
        private void ClearText()
        {
            Hlp.GridColor(dataGridView1);
            billDateTextBox.Text = Hlp.GetServerDate().ToString("dd-MMM-yyyy");
            genderComboBox.SelectedIndex = 0;
          
            discountTypeComboBox.SelectedIndex = 1;
            billNoTextBox.Text = _gt.GetInvoiceNo(4);
            itemNameTextBox.Focus();
            contactNoTextBox.Text = "";
            ptNameTextBox.Text = "";
            yearTextBox.Text = "";
            addressTextBox.Text = "";
          
            dataGridView1.Rows.Clear();
            discountPcTextBox.Text = "";
            totalAmountTextBox.Text = "0";
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
            regIdTextBox.Text = "";
            totalItemTextBox.Text = "0";
            paidAmtTextBox.Enabled = true;
            discountPcTextBox.Enabled = true;


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
            dataGrid.Columns[0].Visible = false;
            dataGrid.Columns[1].Width = itemNameTextBox.Width-qtyTextBox.Width-8;
            dataGrid.Columns[2].Width = salesPriceTextBox.Width-1;
            dataGrid.Columns[3].Width = qtyTextBox.Width-1;
            dataGrid.Columns[4].Width = salesPriceTextBox.Width-3 ;

            dataGrid.Columns[5].Visible = false;

            
            dataGrid.RowHeadersVisible = false;
            dataGrid.ColumnHeadersVisible = false;
            dataGrid.CurrentCell = null;
          //  dataGrid.Columns[3].ReadOnly = false;
            //dataGrid.Columns[4].ReadOnly = false;

            foreach (DataGridViewColumn dc in dataGrid.Columns)
            {
                dc.ReadOnly = !dc.Index.Equals(1);
            }
            //dataGrid.Columns[2].ReadOnly = false;
            //// ReSharper disable once PossibleNullReferenceException
            //dataGrid.Columns["Result"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
            





        }

        private void UpdateHideShow()
        {
            if (_gt.FnSeekRecordNewLab("tb_MASTER_INFO", "UseIndoor=0"))
            {
                label31.Visible = false;
                admNoTextBox.Visible = false;
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
           


            itemNameTextBox.BackColor = Hlp.EnterFocus();
            helpPanel.Visible = false;
        }

        private void testCodeTextBox_Leave(object sender, EventArgs e)
        {
            itemNameTextBox.BackColor = Hlp.LeaveFocus();

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
        SalesGateway _gt = new SalesGateway();
        PatientGateway _pt = new PatientGateway();
      

        private void contactNoTextBox_TextChanged(object sender, EventArgs e)
        {
            
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
           

            if (e.KeyCode==Keys.Enter)
            {
                if (contactNoTextBox.TextLength!=11)
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
                string currQty = "";
                gccode = dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex - 1].Cells[0].Value.ToString();
                gcdesc = dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex - 1].Cells[1].Value.ToString();

                if (textInfo=="item")
                {
                    currQty = dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex - 1].Cells[4].Value.ToString();
                }

                switch (textInfo)
                {
                    case "1":
                        contactNoTextBox.Text = gcdesc;
                        var dt = _pt.GetRegisterPatientList("", Convert.ToInt32(gccode));
                        if (dt.Rows.Count > 0)
                        {
                            regIdTextBox.Text = gccode;
                            ptNameTextBox.Text = dt.Rows[0]["Name"].ToString();
                            genderComboBox.Text = dt.Rows[0]["Sex"].ToString();
                            addressTextBox.Text = dt.Rows[0]["Address"].ToString();
                            DateTime dob = Convert.ToDateTime(dt.Rows[0]["Dob"]);
                            CalculateAgeByDob(dob);
                            itemNameTextBox.Focus();
                            helpPanel.Visible = false;
                            admNoTextBox.Text = "";
                        }
                       
                        break;
                  
                    case "item":
                        itemIDTextBox.Text = gccode;
                        itemNameTextBox.Text = gcdesc;
                        salesPriceTextBox.Text = _gt.FncReturnFielValueLab("tb_ph_item", "Id='" + gccode + "'", "SalePrice");
                        lblQty.Text ="Current Qty:"+ currQty;
                    //AddDataToGrid();
                        qtyTextBox.Focus();
                        qtyTextBox.SelectAll();
                        break;
                    case "Indoor":
                        admNoTextBox.Text = gccode;
                        var ds = Hlp.GetRegAndBedId(Convert.ToInt32(gccode));
                        ptNameTextBox.Text = ds.Patient.Name;
                        genderComboBox.Text = ds.Patient.Sex;
                        addressTextBox.Text = ds.Patient.Address;
                        contactNoTextBox.Text = ds.Patient.ContactNo;
                        regIdTextBox.Text = "";
                        try
                        {
                            string[] ssize = ds.Patient.Age.Split(null);
                            yearTextBox.Text = ssize[0].Replace("Y","").Trim();
                            monthTextBox.Text = ssize[1].Replace("M", "").Trim();
                            dayTextBox.Text = ssize[2].Replace("D", "").Trim();

                        }
                        catch (Exception){}
                        itemNameTextBox.Focus();
                        ChangeUiForIndoorPt();

                        break;


                }

            }
        }

        private void ChangeUiForIndoorPt()
        {
            //discountPcTextBox.Enabled = false;
            //paidAmtTextBox.Enabled = false;
        }

      
        private void ChangeUiForOutdoorPt()
        {
            paidAmtTextBox.Text = "";
            admNoTextBox.Text = "";
            paidAmtTextBox.Enabled = true;
         
            discountPcTextBox.Enabled = true;
            discountTypeComboBox.Enabled = true;
         
        }


        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

       
        public void HelpDataGridLoadByRefDr(DataGridView dg,string search)
        {
            

        }
        public void HelpDataGridLoadByTest(DataGridView dg, string search)
        {
          


        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       

    
        private void testCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void AddDataToGrid()
        {


            if (_gt.FnSeekRecordNewLab("tb_ph_item","Id='"+ itemIDTextBox.Text +"'")==false)
            {
                MessageBox.Show(@"Invalid Item Name", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (_gt.FnSeekRecordNewLab("V_StockList","ItemId='"+ itemIDTextBox.Text +"' AND BalQty<"+ Hlp.StringToDouble(qtyTextBox.Text) +""))
            {
                MessageBox.Show(@"Invalid Qty.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }




            int itemId = Convert.ToInt32(itemIDTextBox.Text);
            if (_gt.IsDuplicate(itemIDTextBox.Text,dataGridView1)==false)
            {
                double unitTotal = Convert.ToDouble(salesPriceTextBox.Text)*Convert.ToDouble(qtyTextBox.Text);
                dataGridView1.Rows.Add(itemId,itemNameTextBox.Text, salesPriceTextBox.Text, qtyTextBox.Text, unitTotal, 0);
                itemNameTextBox.Text = "";
                qtyTextBox.Text = "";
                salesPriceTextBox.Text = "";
                itemIDTextBox.Text = "0";
                dataGridView1.CurrentCell.Selected = false;
                helpPanel.Visible = false;
                itemNameTextBox.Focus();
            }
            else
            {
                MessageBox.Show(@"Duplicate Name Found", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                itemNameTextBox.SelectAll();
                return;
            }

            CalculateTotal();



        }
        private string CalculateTotal()
        {

            double totAmt = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                totAmt += Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[4].Value.ToString()));
            }

           
            totalAmountTextBox.Text = totAmt.ToString();
            if (totDiscountTextBox.Text == "")
            {
                totDiscountTextBox.Text = @"0";
            }

         

            
            
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


            totalItemTextBox.Text = dataGridView1.Rows.Count.ToString();
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

     

   
       

     

       

        private string UpdateDiscTotal(string comefrom)
        {
            switch (discountTypeComboBox.SelectedIndex)
            {
                case 1:
                    double totDiscount = Convert.ToDouble(totalAmountTextBox.Text) * 0.01 * Convert.ToDouble(Hlp.IsNumeric(discountPcTextBox.Text)?discountPcTextBox.Text:"0");
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
                    dataGridView1.Rows[i].Cells[5].Value = 0;
                }
                else
                {
                    double gridAmt = Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value);
                    double hnrLess = Math.Round(gridAmt * Convert.ToDouble(totDiscountTextBox.Text) / Convert.ToDouble(totalAmountTextBox.Text), 4);
                    dataGridView1.Rows[i].Cells[5].Value = hnrLess;
                }
            }
            
            dueAmtTextBox.Text =Math.Round(Convert.ToDouble(Hlp.StringToDouble(totalAmountTextBox.Text)) -Convert.ToDouble(Hlp.StringToDouble(totDiscountTextBox.Text))).ToString();
            paidAmtTextBox.Text = dueAmtTextBox.Text;
            dueAmtTextBox.Text = "0";
            paidAmtTextBox.Focus();



        }

       


        private void paidAmtTextBox_TextChanged(object sender, EventArgs e)
        {
            if (paidAmtTextBox.Text=="")
            {
                paidAmtTextBox.Text = "0";
            }

            double dueAmt =  Convert.ToDouble(Hlp.StringToDouble(totalAmountTextBox.Text)) - Convert.ToDouble(Hlp.StringToDouble(totDiscountTextBox.Text)) -Convert.ToDouble(Hlp.StringToDouble(paidAmtTextBox.Text));
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
                var aMdl = new PurchaseModel
                {
                    //   Supplier = new SupplierModel() { Id = Convert.ToInt32(suppIdTextBox.Text) },
                    TotalItem = Convert.ToDouble(Hlp.StringToDouble(totalItemTextBox.Text)),
                    TotAmount = Convert.ToDouble(Hlp.StringToDouble(totalAmountTextBox.Text)),
                    NetAmount = Convert.ToDouble(Hlp.StringToDouble(totalAmountTextBox.Text)),
                    TotalLess = Convert.ToDouble(Hlp.StringToDouble(totDiscountTextBox.Text)),
                    TotalPaid = Convert.ToDouble(Hlp.StringToDouble(paidAmtTextBox.Text)),
                    Remarks = remarksTextBox.Text,
                    Admission = new AdmissionModel()
                    {
                        AdmId = Convert.ToInt32(Hlp.IsNumeric(admNoTextBox.Text) ? admNoTextBox.Text : "0"),
                    },
                    PatientsModel = new PatientModel()
                    {
                        RegId = Convert.ToInt32(Hlp.IsNumeric(regIdTextBox.Text) ? regIdTextBox.Text : "0"),
                    },

                };


                var mdl = new List<ItemModel>();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    mdl.Add(new ItemModel()
                    {
                        Id = Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value.ToString()),
                        Qty = Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[3].Value.ToString())),
                        SalePrice = Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[2].Value.ToString())),
                        UnitTotal = Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[4].Value.ToString())),
                        LessAmt = Convert.ToDouble(Hlp.StringToDouble(dataGridView1.Rows[i].Cells[5].Value.ToString())),
                    });
                }
                aMdl.ItemModels = mdl;

                string msg = _gt.Save(aMdl, saveAndPrintButton.Text);
                if (msg == _gt.SaveSuccessMessage)
                {
                    MessageBox.Show(_gt.SaveSuccessMessage, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearText();
                    //_gt.DeleteInsertLab("Update tb_BILL_MASTER SET  LastPrintPc='" + Environment.MachineName + "' WHERE Id='" + aMdl.BillId + "'");
                    itemNameTextBox.Focus();
                    PrintInvoice(aMdl.BillId, "''");
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

            if (dataGridView1.Rows.Count<1)
            {
                MessageBox.Show(@"No Item For Save",@"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                itemNameTextBox.Focus();
                isChecked = false;
            }
            if (Hlp.StringToDouble(dueAmtTextBox.Text)<0)
            {
                MessageBox.Show(@"Invalid Due Amount ", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                paidAmtTextBox.Focus();
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

            if (Hlp.StringToDouble(dueAmtTextBox.Text) >0 )
            {
                if (_gt.FnSeekRecordNewLab("tb_in_ADMISSION", "Id='" + admNoTextBox.Text + "'") == false)
                {
                    if (_gt.FnSeekRecordNewLab("tb_PATIENT", "Id='" + regIdTextBox.Text + "'") == false)
                    {
                        MessageBox.Show(@"Due Amount Remaining If Patient Only Register Or Indoor", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        paidAmtTextBox.Focus();
                        isChecked = false;
                    }
                }
            }



            return isChecked;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
           


            if (admNoTextBox.Text !="")
            {
                if (_gt.FnSeekRecordNewLab("tb_in_ADMISSION", "Id='" + admNoTextBox.Text + "'"))
                {
                    var dt = new PharmacyReportViewer("ph_Sales_bill_all", "SELECT * FROM V_ph_Sales_BIll WHERE AdmId='" + admNoTextBox.Text + "'", "Invoice", "V_ph_Sales_BIll", "");
                    dt.Show();
                }
            }
            else if (regIdTextBox.Text !="")
            {
                if (_gt.FnSeekRecordNewLab("tb_PATIENT", "Id='" + regIdTextBox.Text + "'"))
                {
                    var dt = new PharmacyReportViewer("ph_Sales_dtls_regCust", "SELECT * FROM V_ph_Sales_BIll_Reg_Customer WHERE CustId='" + regIdTextBox.Text + "'", "Invoice", "V_ph_Sales_BIll_Reg_Customer", "");
                    dt.Show();
                }
            }
            
            
            




        }

        private void GetAllInvoiceByAdmNo(int admId)
        {
            var dt = new PharmacyReportViewer("ph_Sales_bill_all", "SELECT * FROM V_ph_Sales_BIll WHERE AdmId=" + admId + "", "Invoice", "V_ph_Sales_BIll", "");
            dt.Show();
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

            //if (dataGridView2.CurrentCell.Selected)
            //{
            //    if (dataGridView2.CurrentRow != null)
            //    {
            //        string invNo = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            //        int billId = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_ph_SALES_MASTER", "BillNo='" + invNo + "'", "Id"));
            //        PrintInvoice(billId,"''");
            //    }
            //}
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

            var dt = new PharmacyReportViewer("ph_Sales_bill", "SELECT * FROM V_ph_Sales_BIll WHERE Id=" + invNo + "", "Invoice", "V_ph_Sales_BIll", "");
            dt.ShowDialog();
            
        
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

       
        private void billNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='"+ Hlp.UserName +"' AND  ParentName='Diagnosis' AND PermisionName='Bill-Edit' "))
                {
                    if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "' AND AdmId=0"))
                    {
                        GetInvoiceDataForEdit(billNoTextBox.Text);
                        itemNameTextBox.Focus();
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
                //var aMdl = _gt.GetInvoiceDataForEdit(invNo);

                //ptNameTextBox.Text = aMdl.PatientName;
             
                //genderComboBox.Text=aMdl.Sex ;
                //addressTextBox.Text=aMdl.Address;
                //contactNoTextBox.Text=aMdl.MobileNo;


            


                //if (aMdl.LessType != "")
                //{
                //    discountTypeComboBox.Text = aMdl.LessType;
                //}
                //if (aMdl.LessPc != 0)
                //{
                //    discountPcTextBox.Text = aMdl.LessPc.ToString();
                //}



             
                //totalAmountTextBox.Text = aMdl.TotalAmt.ToString();
                //totDiscountTextBox.Text=aMdl.TotalLessAmt.ToString();
                //paidAmtTextBox.Text=aMdl.CollAmt.ToString();
                //remarksTextBox.Text=aMdl.Remarks;


                //dataGridView1.Rows.Clear();
                //foreach (var item in aMdl.TestChartModel)
                //{
                //    dataGridView1.Rows.Add(item.TestId, item.Name, item.Charge, item.DefaulHonouriam, item.HnrLess, item.HnrToPay);
                //}
                //dataGridView1.CurrentCell.Selected = false;


                //yearTextBox.Text = aMdl.Age.Substring(0,2);
                //if (aMdl.Age.Length > 2 && aMdl.Age.Length <= 8)
                //{
                //    try
                //    {
                //        monthTextBox.Text = aMdl.Age.Substring(4, 2);    
                //    }
                //    catch (Exception)
                //    {
                //        monthTextBox.Text="00";
                //    }
                //}

                //if (aMdl.Age.Length > 8 )
                //{
                //    try
                //    {
                //        dayTextBox.Text = aMdl.Age.Substring(8, 2);
                //    }
                //    catch (Exception)
                //    {
                //        dayTextBox.Text = "00";
                //    }

                //}


                
                
                //UpdateDateOfBirth();
             



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
               // dataGridView2.Columns[5].Width = 100;

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
            if (e.KeyCode == Keys.Enter)
            {

                saveAndPrintButton.Focus();
            }



        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
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
            if (chkAllIndoor.Checked)
            {
                dataGridView2.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,AdmNo,AdmDate,PtName,ContactNo,BedName As Bed FROM V_Admission_List WHERE  (convert(varchar,Id)+PtName+BedName+ContactNo) LIKE '%" + admNoTextBox.Text + "%'");
            }
            else {
                dataGridView2.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,AdmNo,AdmDate,PtName,ContactNo,BedName As Bed FROM V_Admission_List WHERE ReleaseStatus=0 AND (convert(varchar,Id)+PtName+BedName+ContactNo) LIKE '%" + admNoTextBox.Text + "%'");
            
            }
            
            
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
                HelpDataGridLoadByPatientId();
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


       
        private void saveAndPrintButton_Enter(object sender, EventArgs e)
        {
            saveAndPrintButton.BackColor = Color.Coral;
        }

      

        private void saveAndPrintButton_Leave(object sender, EventArgs e)
        {
            saveAndPrintButton.BackColor = Color.Green;
        }

        private void searchTextBox_Leave(object sender, EventArgs e)
        {
            searchTextBox.BackColor = Hlp.LeaveFocus();

        }

     

        private void MedicineSalesUi_Load(object sender, EventArgs e)
        {
            Hlp.GridColor(dataGridView1);
            billDateTextBox.Text = Hlp.GetServerDate().ToString("dd-MMM-yyyy");
            genderComboBox.SelectedIndex = 0;
        
            discountTypeComboBox.SelectedIndex = 0;
            billNoTextBox.Text = _gt.GetInvoiceNo(4);
         

            //if (_gt.FnSeekRecordNewLab("tb_MASTER_INFO", "UseFinancialService=1"))
            //{
            //    var data = _gt.GetFinancial();
            //    dataGridView3.Rows.Clear();
            //    foreach (var mdl in data)
            //    {
            //        dataGridView3.Rows.Add(mdl.MachineId, mdl.MachineName, "");
            //    }

            //    Hlp.GridColor(dataGridView3);
            //    dataGridView3.Columns["financialAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //}


         
            foreach (DataGridViewColumn dc in dataGridView1.Columns)
            {
                dc.ReadOnly = !dc.Index.Equals(6);
            }


            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");



            UpdateHideShow();
            GridWidth(dataGridView1);
            discountTypeComboBox.SelectedIndex = 1;
            itemNameTextBox.Focus();

        }

        private void itemNameTextBox_Enter(object sender, EventArgs e)
        {
            textInfo = "item";
            itemNameTextBox.BackColor = Hlp.EnterFocus();
            helpPanel.Visible = false;
            LoadGridViewByName();
        }

        private void itemNameTextBox_Leave(object sender, EventArgs e)
        {
            itemNameTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void itemNameTextBox_TextChanged(object sender, EventArgs e)
        {
            LoadGridViewByName();
            helpPanel.Visible = true;
        }

        private void LoadGridViewByName()
        {
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = Hlp.LoadDbByQuery(0, "SELECT * FROM V_StockList WHERE (Name+GenericName) LIKE '%"+ itemNameTextBox.Text +"%'");
            Hlp.GridFirstRowDeselect(dataGridView2);
            dataGridView2.Columns[0].Width = 60;
            dataGridView2.Columns[1].Width = 230;
            dataGridView2.Columns[2].Width = 130;
            dataGridView2.Columns[3].Width = 75;
            dataGridView2.Columns[4].Width = 70;
        }

        private void qtyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                AddDataToGrid();
            }
        }

        private void qtyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void discountPcTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (discountPcTextBox.Text=="")
                {
                    totDiscountTextBox.Text = "0";
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
            if (e.KeyCode==Keys.Enter)
            {
                switch (discountTypeComboBox.SelectedIndex)
                {
                    case 1:
                        double totDiscount = Convert.ToDouble(totalAmountTextBox.Text) * 0.01 * Convert.ToDouble(discountPcTextBox.Text);
                        totDiscountTextBox.Text = totDiscount.ToString();
                        break;
                    case 0:
                        totDiscountTextBox.Text = discountPcTextBox.Text;
                        break;
                }
                UpdateLessIntoGrid();  
            }
            
            
       
        }

        private void paidAmtTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void itemNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                if (_gt.FnSeekRecordNewLab("tb_TestChart", "Id='" + itemNameTextBox.Text + "'"))
                {
                    AddDataToGrid();
                    return;
                }


                if (itemNameTextBox.Text.Length > 0)
                {
                    if (dataGridView2.Rows.Count > 0)
                    {

                        if (dataGridView2.Rows[0].Cells[0].Value == null)
                        {
                            return;

                        }

                        itemNameTextBox.Text = dataGridView2.Rows[0].Cells[0].Value.ToString();
                        if (_gt.FnSeekRecordNewLab("tb_TestChart", "Id='" + itemNameTextBox.Text + "'"))
                        {
                            AddDataToGrid();
                            return;
                        }
                    }
                }




                if (dataGridView1.Rows.Count > 0)
                {
                    if (_gt.FnSeekRecordNewLab("tb_in_Admission", "Id='" + admNoTextBox.Text + "'"))
                    {
                        paidAmtTextBox.Focus();
                    }
                    else
                    {
                        discountPcTextBox.Focus();
                    }
                    
                }
                else
                {
                    itemNameTextBox.Focus();
                }


            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                helpPanel.Visible = true;
                textInfo = "item";
                dataGridView2.CurrentRow.Selected = true;
                dataGridView2.Focus();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ClearText();
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string invNo = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            int billId = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_ph_SALES_MASTER", "BillNo='" + invNo + "'", "Id"));
            PrintInvoice(billId, "''");

        }
      

        

        // ,
    }
}
