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
using SuperPathologyApp.Model;
using SuperPathologyApp.Report;
using SuperPathologyApp.Report.DataSet;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;


namespace SuperPathologyApp.UI
{
    public partial class ReportPrintImagingUi : Form
    {
        string _invNo = "";
       
        readonly ReportPrintImagingGateway _gt = new ReportPrintImagingGateway();
        public static string  LabReportquery = "";
        public static string LabReportFileName = "";
        private bool _isSaved=true;
        private string _reportFileName = "";
        private  string _testCode = "";
        List<TestCodeModel> testCodeList=new List<TestCodeModel>();
        private string testNameForView = "";

        private bool IsClickSaveButton = true;



        public ReportPrintImagingUi()
        {
            InitializeComponent();
            
            
            if (Hlp.GroupName == "")
            {
                FourDigitTextBox.Focus();
            }
            else
            {
                sampleNoTextBox.Focus();
            }
        }

        private void ClearText()
        {
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
            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView3.EnableHeadersVisualStyles = false;
            dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;


            dataGridView4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView4.AllowUserToResizeRows = false;

        }
   
        private void GetInvoiceDetailByInvNo()
        {
            testCodeList = _gt.GetInvoiceDetailByInvNo(masterId,sampleNoTextBox.Text);

            if (testCodeList.Count > 0)
            {
                dataGridView3.Rows.Clear();
                dataGridView1.Rows.Clear();

                PopulateMasterData(testCodeList);
                
                var distinctItems = testCodeList.GroupBy(x => x.GroupName).Select(y => y.First());
                foreach (var codeModel in distinctItems)
                {
                    if (codeModel != null) dataGridView3.Rows.Add(codeModel.GroupName);    
                }
                dataGridView3.Rows[0].Selected = false;
                _gt.GridColor(dataGridView3);

                int i = 0;
                testCodeList = testCodeList.GroupBy(x => x.PCode).Select(y => y.First()).ToList();
                
                foreach (var codeModel in testCodeList)
                {
                    if (codeModel.PrintNo==1)
                    {
                        /////////Printed
                        dataGridView1.Rows.Add(codeModel.PCode, codeModel.ItemDesc, 1, codeModel.SampleNo, codeModel.GroupName);
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.DarkRed;
                    }
                    else if (codeModel.PrintNo == 2)
                    {
                        /////////Saved
                        dataGridView1.Rows.Add(codeModel.PCode, codeModel.ItemDesc, 0, codeModel.SampleNo, codeModel.GroupName);
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.BlanchedAlmond;
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.DarkRed;
                    }
                    else
                    {
                        dataGridView1.Rows.Add(codeModel.PCode, codeModel.ItemDesc, 0, codeModel.SampleNo, codeModel.GroupName);
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.DarkRed;
                    }
                    
                    i++;
                }
                dataGridView1.Rows[0].Selected = false;
                //_gt.GridColor(dataGridView1);
            }
            else {

                ClearText();
                dataGridView3.Rows.Clear();
                dataGridView1.Rows.Clear();
                        
            }
        }

        private void PopulateMasterData(List<TestCodeModel> testCodeList)
        {
            invDateDateTimePicker.Value = testCodeList[0].PtInvDate;
            patientNameTextBox.Text = testCodeList[0].PtName;
            contactNoTextBox.Text = testCodeList[0].PtTelephoneNo;
            ageTextBox.Text = testCodeList[0].PtAge;
            sexTextBox.Text = testCodeList[0].PtSex;
            drCodeTextBox.Text = testCodeList[0].DrCode;
            drNameTextBox.Text = testCodeList[0].DrName;
            patientTypeTextBox.Text =testCodeList[0].PtType;
        }

    
        private void GridWidth(DataGridView dataGrid)
        {
            dataGrid.Columns[0].Visible = false;
            dataGrid.Columns[1].Width = 150;
            dataGrid.Columns[2].Width = 50;
            dataGrid.Columns[3].Width = 600;

           
            
            dataGrid.RowHeadersVisible = false;
            dataGrid.CurrentCell = null;
            dataGrid.Columns[1].ReadOnly = true;
            dataGrid.Columns[2].ReadOnly = true;


        }
   
      
   
        private void button1_Click(object sender, EventArgs e)
        {
            return;
            if (dataGridView2.Rows.Count < 1)
            {
                MessageBox.Show(@"No Data Found To View", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Hlp.AutoPrint = false;
            
            
            
            string invNo = invNoFirstPartTextBox.Text  + FourDigitTextBox.Text;
            var mdl = new List<TestCodeModel>();

            string testNameForRpt = "";
            string testNameInGrid = "";
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                string tstName = dataGridView2.Rows[i].Cells[17].Value.ToString();
                if (testNameInGrid != tstName)
                {
                    testNameInGrid = dataGridView2.Rows[i].Cells[17].Value.ToString();
                    if (testNameForRpt == "")
                    {
                        testNameForRpt = dataGridView2.Rows[i].Cells[17].Value.ToString();
                    }
                    else
                    {
                        testNameForRpt = testNameForRpt + "," + dataGridView2.Rows[i].Cells[17].Value.ToString();
                    }
                }
            }
            testNameForView = testNameForRpt;

            
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                string result = dataGridView2.Rows[i].Cells[1].Value == null ? "" : dataGridView2.Rows[i].Cells[1].Value.ToString();
                string parameterTestName = dataGridView2.Rows[i].Cells[0].Value == null? "": dataGridView2.Rows[i].Cells[0].Value.ToString();
                string unitName = dataGridView2.Rows[i].Cells[2].Value == null? "": dataGridView2.Rows[i].Cells[2].Value.ToString();
                string normalValue = dataGridView2.Rows[i].Cells[3].Value == null? "": dataGridView2.Rows[i].Cells[3].Value.ToString();
                string machineName = dataGridView2.Rows[i].Cells[6].Value == null? "": dataGridView2.Rows[i].Cells[6].Value.ToString();
                string headerName = dataGridView2.Rows[i].Cells[7].Value == null? "": dataGridView2.Rows[i].Cells[7].Value.ToString();
                int groupSlNo = dataGridView2.Rows[i].Cells[8].Value == null? 0:Convert.ToInt32(dataGridView2.Rows[i].Cells[8].Value.ToString());
                int parameterSlNo = dataGridView2.Rows[i].Cells[9].Value == null? 0: Convert.ToInt32(dataGridView2.Rows[i].Cells[9].Value.ToString());
                string parameterName = dataGridView2.Rows[i].Cells[10].Value == null? "": dataGridView2.Rows[i].Cells[10].Value.ToString();
                string reportingGroupName = dataGridView2.Rows[i].Cells[11].Value == null? "": dataGridView2.Rows[i].Cells[11].Value.ToString();
                int isBold = dataGridView2.Rows[i].Cells[12].Value == null? 0: Convert.ToInt32(dataGridView2.Rows[i].Cells[12].Value.ToString());
                string testCode = dataGridView2.Rows[i].Cells[13].Value == null? "": dataGridView2.Rows[i].Cells[13].Value.ToString();
                string groupName = dataGridView2.Rows[i].Cells[14].Value == null? "": dataGridView2.Rows[i].Cells[14].Value.ToString();
                string sampleNo = dataGridView2.Rows[i].Cells[16].Value == null? "": dataGridView2.Rows[i].Cells[16].Value.ToString();
                string specimen = _gt.FncReturnFielValueLab("tb_Parameter_Definition", "TestCode='" + testCode + "'","Specimen");
                string collTime = _gt.FncReturnFielValueLab("tb_LabSampleStatusInfo", "SampleNo='" + sampleNo + "'", "CollTime");
                string collUser = _gt.FncReturnFielValueLab("tb_LabSampleStatusInfo", "SampleNo='" + sampleNo + "'", "CollUser");

               



                mdl.Add(new TestCodeModel()
                    {
                        PtName = patientNameTextBox.Text,
                        PtAge = ageTextBox.Text,
                        PtSex = sexTextBox.Text,
                        DrName = drNameTextBox.Text,
                        ParameterTestName = parameterTestName,
                        Result = result,
                        UnitName = unitName,
                        NormalValue = normalValue,
                        MachineName = machineName,
                        HeaderName = headerName,
                        GroupSlNo = groupSlNo,
                        ParameterSlNo = parameterSlNo,
                        ParameterName = parameterName,
                        ReportingGroupName = reportingGroupName,
                        IsBold = isBold,
                        TestCode = testCode,
                        GroupName = groupName,
                        SampleNo = sampleNo,
                        MasterId = masterId,
                        PtInvNo = invNo,
                        PtInvDate = invDateDateTimePicker.Value,
                        ReportNo = "0",
                        CommentsInv = commentsRichTextBox.Text,
                        UserName = collUser,
                        TestName = testNameForView,
                        SpecimenName = specimen,
                        SampleCollectionTime = collTime,
                        PrintBy = Hlp.UserName,
                        //SampleCollectionUserName = collUser,
                    });

            }


           
          

            #region
            foreach (TestCodeModel model in mdl)
            {
                if ((checkedByComboBox.Text == @"--Select--"))
                {
                    model.CheckedByName = "";
                    model.CheckedByDegree = "";
                }
                else
                {
                    model.CheckedByName = checkedByComboBox.Text;
                    model.CheckedByDegree = _gt.FncReturnFielValueLab("tb_DoctorSetup", "Id=" + checkedByComboBox.SelectedValue + "", "Details");
                }
            }
            foreach (TestCodeModel model in mdl)
            {
                if ((consultantComboBox.Text == @"--Select--"))
                {
                    model.ConsultantName = "";
                    model.ConsultantDegree = "";
                }
                else
                {
                    model.ConsultantName = consultantComboBox.Text;
                    model.ConsultantDegree = _gt.FncReturnFielValueLab("tb_DoctorSetup", "Id=" + consultantComboBox.SelectedValue + "", "Details");
                }
            }
            foreach (TestCodeModel model in mdl)
            {
                if ((labInchargeComboBox.Text == @"--Select--"))
                {
                    model.LabInchargeName = "";
                    model.LabInchargeDegree = "";
                }
                else
                {
                    model.LabInchargeName = labInchargeComboBox.Text;
                    model.LabInchargeDegree= _gt.FncReturnFielValueLab("tb_DoctorSetup", "Id=" + labInchargeComboBox.SelectedValue + "", "Details");
                }
            }

            #endregion

            _gt.DeleteInsertLab("DELETE FROM A_TMP_Lab_ReportView");
            DataTable dt = _gt.ConvertListDataTable(mdl);

            var objbulk = new SqlBulkCopy(_gt.ConLab) { DestinationTableName = "A_TMP_Lab_ReportView" };
            objbulk.ColumnMappings.Add("PtInvNo", "InvNo");
            objbulk.ColumnMappings.Add("PtInvDate", "InvDate");
            objbulk.ColumnMappings.Add("SampleNo", "LabNo");

            objbulk.ColumnMappings.Add("PtName", "PatientName");
            objbulk.ColumnMappings.Add("PtAge", "Age");
            objbulk.ColumnMappings.Add("PtSex", "Sex");
            objbulk.ColumnMappings.Add("DrName", "DrName");

            objbulk.ColumnMappings.Add("TestCode", "TestCode");
            objbulk.ColumnMappings.Add("ParameterName", "Parameter");
            objbulk.ColumnMappings.Add("ParameterTestName", "AliasNo");
            objbulk.ColumnMappings.Add("Result", "Result");
            objbulk.ColumnMappings.Add("UnitName", "Unit");
            objbulk.ColumnMappings.Add("NormalValue", "NormalValue");

            objbulk.ColumnMappings.Add("ReportingGroupName", "ReportingGroupName");
            objbulk.ColumnMappings.Add("GroupSlNo", "GroupSlNo");
            objbulk.ColumnMappings.Add("ParameterSlNo", "SerialNo");
           
            objbulk.ColumnMappings.Add("UserName", "UserName");
            objbulk.ColumnMappings.Add("HeaderName", "ReportHeaderName");
            objbulk.ColumnMappings.Add("IsBold", "IsBold");

            objbulk.ColumnMappings.Add("ConsultantName", "ConsultantName");
            objbulk.ColumnMappings.Add("ConsultantDegree", "ConsultantDegree");
            objbulk.ColumnMappings.Add("CheckedByName", "CheckedByName");
            objbulk.ColumnMappings.Add("CheckedByDegree", "CheckedByDegree");

            objbulk.ColumnMappings.Add("LabInchargeName", "LabInchargeName");
            objbulk.ColumnMappings.Add("LabInchargeDegree", "LabInchargeDegree");


            objbulk.ColumnMappings.Add("GroupName", "ReportFileName");
            objbulk.ColumnMappings.Add("TestName", "TestName");
            objbulk.ColumnMappings.Add("SpecimenName", "Specimen");

            objbulk.ColumnMappings.Add("SampleCollectionTime", "CollTime");
            objbulk.ColumnMappings.Add("PrintBy", "PrintBy");


            _gt.ConLab.Open();
                objbulk.WriteToServer(dt);
            _gt.ConLab.Close();

            LabReportquery = "SELECT * FROM A_TMP_Lab_ReportView WHERE InvNo='" + invNo + "' AND InvDate='" + invDateDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND ReportFileName='" + _reportFileName + "' AND PrintBy='"+ Hlp.UserName +"' ";
            var gl = new FrmReportViewer(_reportFileName,LabReportquery);
            gl.ShowDialog();


            
        }


        private void Show_Combobox(int iRowIndex, int iColumnIndex, string testCode, string paramName)
        {
            _gt.LoadComboBoxForDefault("SELECT 0 AS Id,Result AS Description FROM tb_DefaultResultSetup WHERE PCode='" + testCode + "' AND  Name='" + paramName + "'", defaultResultComboBox);
            
            
            int x = 0;
            int y = 0;
            int Width = 0;
            int height = 0;
            Rectangle rect = default(Rectangle);
            rect = dataGridView2.GetCellDisplayRectangle(iColumnIndex, iRowIndex, false);
            x = rect.X + dataGridView2.Left;
            y = rect.Y + dataGridView2.Top;
            Width = rect.Width;
            height = rect.Height;
            defaultResultComboBox.SetBounds(x, y, Width, height);
            defaultResultComboBox.Visible = true;
            defaultResultComboBox.Focus();
            defaultResultComboBox.DroppedDown= true;
            //defaultResultComboBox.Focus();
        }

        private void defaultResultComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //defaultResultComboBox.Visible = false;
            //dataGridView2.CurrentCell = dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[1];
            //dataGridView2.CurrentCell.Value = defaultResultComboBox.Text;
            //dataGridView2.Focus();
        }

        private int masterId = 0;
        private void FourDigitTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetDataForInvoicePrint();
            }
        }

        private void GetDataForInvoicePrint()
        {
            _invNo = invNoFirstPartTextBox.Text + FourDigitTextBox.Text;
            dataGridView2.Rows.Clear();
            totalCountTextBox.Text = @"0";


            if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "BillNo='" + _invNo + "'"))
            {
                invDateDateTimePicker.Value = Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + _invNo + "'", "BillDate"));
                masterId = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillDate='" + invDateDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND BillNo='" + _invNo + "' ", "Id"));
            }
            else
            {
                masterId = 0;
            }

            var otherData = new BarcodePrintGateway();
           // var data = otherData.GeetOtherSoftwareDataForImaging(_invNo, invDateDateTimePicker.Value);
           // UpdateInvMasterAndDetails(data);


            if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "Id='" + masterId + "'"))
            {
                GetInvoiceDetailByInvNo();
            }
            else
            {
                MessageBox.Show(@"No Data Found On This Id", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FourDigitTextBox.Focus();
            }
        }

      

        private void patientNameTextBox_Enter(object sender, EventArgs e)
        {
            patientNameTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void FourDigitTextBox_Enter(object sender, EventArgs e)
        {
            FourDigitTextBox.BackColor = DbConnection.EnterFocus();
            sampleNoTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void FourDigitTextBox_Leave(object sender, EventArgs e)
        {
            FourDigitTextBox.BackColor = DbConnection.LeaveFocus();
        }

 

        private void ageTextBox_Enter(object sender, EventArgs e)
        {
            ageTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void ageTextBox_Leave(object sender, EventArgs e)
        {
            ageTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void patientNameTextBox_Leave(object sender, EventArgs e)
        {
            patientNameTextBox.BackColor = DbConnection.LeaveFocus();
        }





        private void commentsRichTextBox_Enter(object sender, EventArgs e)
        {
            commentsRichTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void commentsRichTextBox_Leave(object sender, EventArgs e)
        {
            commentsRichTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private SqlTransaction _trans;
 

        private void Save(string invNo)
        {

            if (patientNameTextBox.Text == @"")
            {
                MessageBox.Show(@"Invalid Patient Name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _isSaved = false;
                return;
            }
            if (drNameTextBox.Text == @"")
            {
                MessageBox.Show(@"Invalid Doctor Name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _isSaved = false;
                return;
            }

            var inv = new BarcodePrintGateway();
            
            try
            {
                inv.ConLab.Open();
                _trans = inv.ConLab.BeginTransaction();

                if (dataGridView2.Rows.Count == 0)
                {
                    _isSaved = false;
                    return;
                }


                #region dataGridView
                var mdl = new List<TestCodeModel>();
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                        mdl.Add(new TestCodeModel()
                        {
                            ParameterSlNo = Convert.ToInt32(dataGridView2.Rows[i].Cells[0].Value),
                            ParameterName = dataGridView2.Rows[i].Cells[1].Value.ToString(),
                            Result = dataGridView2.Rows[i].Cells[3].Value == null ? "" : dataGridView2.Rows[i].Cells[3].Value.ToString(),
                            IsBold = Convert.ToInt32(dataGridView2.Rows[i].Cells[4].Value),
                            TestCode = dataGridView2.Rows[i].Cells[5].Value.ToString(),
                            SampleNo = dataGridView2.Rows[i].Cells[7].Value.ToString(),
                            MasterId = masterId,
                            PtInvNo = invNo,
                            PtInvDate = invDateDateTimePicker.Value,
                            ReportNo = dataGridView2.Rows[i].Cells[6].Value == null ? "" : dataGridView2.Rows[i].Cells[6].Value.ToString(),
                            CommentsInv = commentsRichTextBox.Text,
                        });
          
                }
                if (mdl.Count<1)
                {
                    MessageBox.Show(@"No Result Found For Save", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _isSaved = false;
                    return;
                }
                #endregion


 
                #region consultant
                foreach (TestCodeModel model in mdl)
                {
                    if ((checkedByComboBox.Text == @"--Select--"))
                    {
                        model.CheckedByName = "";
                        model.CheckedByDegree = "";
                    }
                    else
                    { 
                        model.CheckedByName = checkedByComboBox.Text;
                        model.CheckedByDegree = _gt.FncReturnFielValueLab("tb_DoctorSetup", "Id=" + checkedByComboBox.SelectedValue + "", "Details", _trans, inv.ConLab);
                    }
                }
                foreach (TestCodeModel model in mdl)
                {
                    if ((consultantComboBox.Text == @"--Select--"))
                    {
                        model.ConsultantName = "";
                        model.ConsultantDegree = "";
                    }
                    else
                    {
                        model.ConsultantName = consultantComboBox.Text;
                        model.ConsultantDegree = _gt.FncReturnFielValueLab("tb_DoctorSetup", "Id=" + consultantComboBox.SelectedValue + "", "Details", _trans, inv.ConLab);
                    }
                }
                foreach (TestCodeModel model in mdl)
                {
                    if ((labInchargeComboBox.Text == @"--Select--"))
                    {
                        model.LabInchargeName = "";
                        model.LabInchargeDegree = "";
                    }
                    else
                    {
                        model.LabInchargeName = labInchargeComboBox.Text;
                        model.LabInchargeDegree = _gt.FncReturnFielValueLab("tb_DoctorSetup", "Id=" + labInchargeComboBox.SelectedValue + "", "Details", _trans, inv.ConLab);
                    }
                }

                #endregion 


                _gt.Save(mdl,_trans,inv.ConLab);
                _isSaved = true;



             
                _trans.Commit();
                inv.ConLab.Close();

                if (IsClickSaveButton == false)
                {
                    foreach (var model in mdl)
                    {
                        if (model.Result.Length>0)
                        {
                            _gt.DeleteInsertLab("Update tb_InvDetails SET PrintNo=1 WHERE MasterId='" + masterId + "'  AND Code='" + model.TestCode + "'");
                            _gt.DeleteInsertLab("Update tb_LabSampleStatusInfo SET CollStatus='Collected',CollTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',CollUser='" + Hlp.UserName + "',SendStatus='Send',SendTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',SendUser='" + Hlp.UserName + "',ReceiveInLabStatus='Collected',ReceiveInLabTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',ReceiveInLabUser='" + Hlp.UserName + "',ReportProcessStatus='Processed',ReportProcessTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',ReportProcessUser='" + Hlp.UserName + "',  ReportprintStatus='Printed',ReportPrintUser='" + Hlp.UserName + "',ReportPrintTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',ReportNo='" + model.ReportNo + "',FinalStatus='Report Process',RDeliveryDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE MasterId='" + masterId + "'  AND TestCode='" + model.TestCode + "' ");
                        }
                    }
                }
                else
                {
                    foreach (var model in mdl)
                    {
                        if (model.Result.Length > 0)
                        {
                            _gt.DeleteInsertLab("Update tb_InvDetails SET PrintNo=2 WHERE MasterId='" + masterId +"'  AND Code='" + model.TestCode + "' AND PrintNo<>1");
                            _gt.DeleteInsertLab("Update tb_LabSampleStatusInfo SET FinalStatus='Saved' WHERE MasterId='" + masterId + "'  AND TestCode='" + model.TestCode + "' AND ReportprintStatus='Pending'");

                        }
                    }
                }

            

               

                ClearText();
                dataGridView2.Rows.Clear();
                commentsRichTextBox.Text = "";
                manualSampleNoTextBox.Text = "";
                dataGridView3.Focus();

                if (dataGridView3.Rows.Count>1)
                {
                    dataGridView3.CurrentCell.Selected = true;
                }
 

            }
            catch (Exception exception)
            {

                _trans.Rollback();
                if (inv.ConLab.State == ConnectionState.Open)
                {
                    inv.ConLab.Close();
                }


                MessageBox.Show(exception.Message);
            }
            
            
            
        }



        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


    

        private void patientNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sexTextBox.Focus();
            }
        }

        private void ageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                drCodeTextBox.Focus();
                //button3.Focus();
            }
        }

  

  




        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count == 0)
            {
                MessageBox.Show(@"No Item Found For Save", @"Information", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (_gt.FnSeekRecordNewLab("tb_MachineDataDtls","InvNo='" + FourDigitTextBox.Text + "' AND InvDate='" +invDateDateTimePicker.Value.ToString("yyyy-MM-dd") + "' ") == false)
            {
                MessageBox.Show(@"Invalid Lab No", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            try
            {


                _gt.ConLab.Open();
                _trans = _gt.ConLab.BeginTransaction();



                var mdl = new List<TestCodeModel>();
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    mdl.Add(new TestCodeModel()
                    {
                        PtInvNo = FourDigitTextBox.Text,
                        PtInvDate = invDateDateTimePicker.Value,
                        SampleNo = "",
                        GroupName = dataGridView2.Rows[i].Cells[14].Value.ToString(),
                        TestCode = dataGridView2.Rows[i].Cells[13].Value.ToString(),
                        ParameterTestName = dataGridView2.Rows[i].Cells[0].Value.ToString(),
                        ParameterName = dataGridView2.Rows[i].Cells[10].Value.ToString(),
                        Result = dataGridView2.Rows[i].Cells[1].Value.ToString(),
                        UnitName = dataGridView2.Rows[i].Cells[2].Value.ToString(),
                        NormalValue = dataGridView2.Rows[i].Cells[3].Value.ToString(),

                        ReportingGroupName = dataGridView2.Rows[i].Cells[11].Value.ToString(),
                        GroupSlNo = Convert.ToInt32(dataGridView2.Rows[i].Cells[8].Value),
                        ParameterSlNo = Convert.ToInt32(dataGridView2.Rows[i].Cells[9].Value),


                        //DrCode = drCodeTextBox.Text ,
                        //DrName= drNameTextBox.Text,
                        MachineName = dataGridView2.Rows[i].Cells[6].Value.ToString(),
                        HeaderName = dataGridView2.Rows[i].Cells[7].Value.ToString(),
                        IsBold = Convert.ToInt32(dataGridView2.Rows[i].Cells[12].Value),

                        ConsultantName = consultantComboBox.Text,
                        ConsultantDegree = _gt.FncReturnFielValueLab("tb_DoctorSetup","Id=" + consultantComboBox.SelectedValue + "", "Details",_trans,_gt.ConLab),
                        CheckedByName = checkedByComboBox.Text,
                        CheckedByDegree = _gt.FncReturnFielValueLab("tb_DoctorSetup", "Id=" + checkedByComboBox.SelectedValue + "", "Details", _trans, _gt.ConLab),
                        CommentsInv = commentsRichTextBox.Text

                    });
                }


                foreach (TestCodeModel model in mdl)
                {
                    if ((checkedByComboBox.Text == @"--Select--"))
                    {
                        model.CheckedByName = "";
                        model.CheckedByDegree= "";
                    }
                }
                foreach (TestCodeModel model in mdl)
                {
                    if ((consultantComboBox.Text == @"--Select--"))
                    {
                        model.ConsultantName = "";
                        model.ConsultantDegree = "";
                    }
                }
                double totCount = 0;
                foreach (TestCodeModel dataN in mdl)
                {
                    switch (dataN.ParameterName)
                    {
                        case "NEUT%":
                        case "LYMPH%":
                        case "MONO%":
                        case "BAS%":
                        case "EO%":
                            totCount += Convert.ToDouble(dataN.Result);
                            break;
                    }
                }
                if (totCount < 100)
                {
                    MessageBox.Show(@"Total count must be 100", @"Information", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    _isSaved = false;
                    return;
                }
                _gt.Save(mdl,_trans,_gt.ConLab);


                _trans.Commit();
                _gt.ConLab.Close();
                ClearText();
                dataGridView2.Rows.Clear();
                FourDigitTextBox.Focus();
            }
            catch (Exception exception)
            {
                _trans.Rollback();
                if (_gt.ConLab.State == ConnectionState.Open)
                {
                    _gt.ConLab.Close();
                }

                MessageBox.Show(exception.Message);
            }
        }
        private void UpdateDataGrid()
        {

            //Thread.Sleep(500);

            double wbc = 0,neut=0,lym=0,mono=0,eos=0,baso=0;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                string paramName = dataGridView2.Rows[i].Cells[10].Value.ToString();
                switch (paramName)
                {
                    case "WBC":
                        wbc = _gt.IsNumeric(dataGridView2.Rows[i].Cells[1].Value.ToString()) ? Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value.ToString()) : 0;
                        break;
                    case "NEU%":
                        neut = _gt.IsNumeric(dataGridView2.Rows[i].Cells[1].Value.ToString()) ? Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value.ToString()) : 0;
                        dataGridView2.Rows[i].Cells[1].Value = neut.ToString().PadLeft(2, '0');
                        break;
                    case "LYM%":
                        lym = _gt.IsNumeric(dataGridView2.Rows[i].Cells[1].Value.ToString()) ? Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value.ToString()) : 0;
                        dataGridView2.Rows[i].Cells[1].Value = lym.ToString().PadLeft(2, '0');
                        break;
                    case "MON%":
                        mono = _gt.IsNumeric(dataGridView2.Rows[i].Cells[1].Value.ToString()) ? Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value.ToString()) : 0;
                        dataGridView2.Rows[i].Cells[1].Value = mono.ToString().PadLeft(2, '0');
                        break;
                    case "EOS%":
                        eos = _gt.IsNumeric(dataGridView2.Rows[i].Cells[1].Value.ToString()) ? Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value.ToString()) : 0;
                        dataGridView2.Rows[i].Cells[1].Value = eos.ToString().PadLeft(2, '0');
                        break;
                    case "BAS%":
                        baso = _gt.IsNumeric(dataGridView2.Rows[i].Cells[1].Value.ToString()) ? Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value.ToString()) : 0;
                        dataGridView2.Rows[i].Cells[1].Value = baso.ToString().PadLeft(2, '0');
                        break;

                }
            }
            //for (int i = 0; i < dataGridView2.Rows.Count; i++)
            //{
            //    string paramName = dataGridView2.Rows[i].Cells[10].Value.ToString();
            //    if (paramName == "TCE")
            //    {
            //        if (_gt.IsNumeric(dataGridView2.Rows[i].Cells[1].Value.ToString()))
            //        {
            //            dataGridView2.Rows[i].Cells[1].Value = Math.Round(wbc / 100 * eos, 0);
            //        }
            //    }
            //}
           
            
            double totCount = neut + lym + mono + eos+baso;
            totalCountTextBox.Text = totCount.ToString();
            if (totCount != 100)
            {
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    dataGridView2.Rows[i].Cells[1].Style.BackColor = Color.White;
                    string paramName = dataGridView2.Rows[i].Cells[10].Value.ToString();
                    switch (paramName)
                    {
                        case "NEU%":
                        case "LYM%":
                        case "MON%":
                        case "EOS%":
                        case "BAS%":
                            dataGridView2.Rows[i].Cells[1].Style.BackColor = Color.LightBlue;
                            break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    dataGridView2.Rows[i].Cells[1].Style.BackColor = Color.White;
                }
            }

        }

  

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
          
        }



        private void button4_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (totalCountTextBox.Text=="")
            {
                totalCountTextBox.Text = "0";
            }
           
            
            int diffCount = Convert.ToInt32(totalCountTextBox.Text);
            if (diffCount > 0)
            {
                if (diffCount != 100)
                {
                    MessageBox.Show(@"Diffential Count Must Be 100", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }


            IsClickSaveButton = true;
            if (dataGridView2.Rows.Count < 1)
            {
                MessageBox.Show(@"No Data Found To Save", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (Hlp.GroupName == "")
            {
                if (FourDigitTextBox.Text == "")
                {
                    return;
                }
            }
            else
            {
                if (sampleNoTextBox.Text == "")
                {
                    return;
                }

            }

       

            string invNo = invNoFirstPartTextBox.Text + FourDigitTextBox.Text;
            if (_gt.FnSeekRecordNewLab("VW_Sample_Process_Tracking", "InvNo='" + invNo + "'") == false)
            {
                return;
            }

            // string invNo = invNoFirstPartTextBox.Text  + FourDigitTextBox.Text;

            Save(invNo);

            if (Hlp.GroupName =="")
            {
                GetInvoiceDetailByInvNo();
                FourDigitTextBox.Focus();
                FourDigitTextBox.Select(3,4);
            }
            else
            {
                sampleNoTextBox.Focus();
                sampleNoTextBox.Select(0,11);
                
            }
            
            

        }

        private void saveAndPrintButton_Click(object sender, EventArgs e)
        {

            IsClickSaveButton = false;

            if (dataGridView2.Rows.Count < 1)
            {
                MessageBox.Show(@"No Data Found To Save", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

 

            string invNo =  FourDigitTextBox.Text;
            if (_gt.FnSeekRecordNewLab("VW_Sample_Process_Tracking","InvNo='"+ invNo +"'")==false)
            {
                return;
            }


          

            #region sampleNoForPrint
            var listOfSampleNo = new List<UnitModel>();
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                string smplNo = dataGridView2.Rows[i].Cells[7].Value == null ? "" : dataGridView2.Rows[i].Cells[7].Value.ToString();
                listOfSampleNo.Add(new UnitModel()
                {
                    UnitName = smplNo,
                });
            }
            var distSampleNo = listOfSampleNo.Select(x => x.UnitName).Distinct();
            string sampleNoInGrid = "";
            foreach (var sDl in distSampleNo)
            {
                if (sampleNoInGrid=="")
                {
                    sampleNoInGrid = "('" + sDl;
                }
                else
                {
                    sampleNoInGrid += "','" + sDl;
                }
            }
            sampleNoInGrid += "')";
            
            #endregion

            Save(invNo);
            
            
            string cond = "";

            Hlp.AutoPrint = autoPrintCheckBox.Checked;

            if (_isSaved)
            {
                string testCond = "";
                if (_testCode!="")
                {
                    testCond = "AND TestCode='"+ _testCode +"'";
                }


                string smplCond = "";
                if (sampleNoInGrid!="")
                {
                    smplCond += " AND LabNo IN "+sampleNoInGrid;
                }

                LabReportquery = "SELECT '" + testNameForView + "' AS TestName,'" + Hlp.UserName + "' AS PrintBy,  * FROM VW_GET_IMAGING_REPORT_VIEW WHERE InvDate='" + invDateDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND MasterId=" + masterId + " " + testCond + " " + cond + " " + smplCond + " AND IsPrint=1 ";
                if (Hlp.AutoPrint)
                {
                    PrintWithOutShow();
                }
                else
                {
                    var dk = new GroupReportViewer(_reportFileName, LabReportquery, "", "VW_GET_IMAGING_REPORT_VIEW", "");
                    dk.ShowDialog();
                }



            }

            if (Hlp.GroupName == "")
            {
                GetInvoiceDetailByInvNo();
                FourDigitTextBox.Focus();
                FourDigitTextBox.Select(6, 3);
            }
            else
            {
                sampleNoTextBox.Focus();
                sampleNoTextBox.Select(0, 11);
            }


            specimenTextBoxNew.Text = "";


        }
        readonly DbConnection db = new DbConnection();
        private void PrintWithOutShow()
        {

            try
            {
                var rprt = new ReportDocument();

                string comName = db.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ComName");
                string comAddress = db.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "Address");


                db.ConLab.Open();
                string path = Application.StartupPath;
                path = path + @"\Report\File\";

                rprt.Load(path + "" + _reportFileName + ".rpt");
                var cmd = new SqlCommand(LabReportquery, db.ConLab);
                var sda = new SqlDataAdapter(cmd);
                var ds = new GroupReportDS();
                sda.Fill(ds, "VW_GET_IMAGING_REPORT_VIEW");
                rprt.SetDataSource(ds);

                rprt.SetParameterValue("lcComName", comName);
                rprt.SetParameterValue("lcComAddress", comAddress);
                rprt.SetParameterValue("lcDateRange", Hlp.UserName);
                rprt.SetParameterValue("lcTitle", "");
                rprt.PrintToPrinter(1, true, 0, 0);

                db.ConLab.Close();

            }
            catch (Exception exception)
            {
                if (db.ConLab.State == ConnectionState.Open)
                {
                    db.ConLab.Close();
                }
                MessageBox.Show(exception.Message);
            }


        }
        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView4.CurrentRow != null)
            {
                string invNo = dataGridView4.CurrentRow.Cells[0].Value.ToString();
                LabReportquery = "SELECT " + testNameForView + " AS TestName, * FROM VW_GET_LAB_REPORT_VIEW WHERE InvNo='" + invNo + "' AND InvDate='" + invDateDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND IsPrint=1   ";
            }
            var dt = new FrmReportViewer(_reportFileName,LabReportquery);
            dt.ShowDialog();
        }

        private void dataGridView4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show(@"Do you want to delete this LabNo?", @"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
                else
                {
                    if (dataGridView4.CurrentRow != null)
                    {
                        string invNo = dataGridView4.CurrentRow.Cells[0].Value.ToString();
                        DateTime invDate = Convert.ToDateTime(dataGridView4.CurrentRow.Cells[1].Value.ToString());
                        _gt.DeleteInsertLab("DELETE FROM tb_InvMaster WHERE InvNo='" + invNo + "' AND InvDate='" + invDate.ToString("yyyy-MM-dd") + "'");
                        _gt.DeleteInsertLab("DELETE FROM tb_InvDetails WHERE InvNo='" + invNo + "' AND InvDate='" + invDate.ToString("yyyy-MM-dd") + "'");
                        _gt.DeleteInsertLab("DELETE FROM tb_MachineDataDtls WHERE InvNo='" + invNo + "' AND InvDate='" + invDate.ToString("yyyy-MM-dd") + "'");
                    }
                }
            }
        }

      

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
      

        }

      
       
        private void GetParameterDataByCode(int mstId,string pCode, string groupName,string sampleNo)
        {
                specimenTextBoxNew.Text = "";

                Hlp.ReportHeaderName = _gt.FncReturnFielValueLab("tb_Group", "Name='" + groupName + "'", "HeaderName");    

                var data = _gt.GetDataFromParameterDefinitionByTestCode(mstId, pCode, groupName);
                foreach (var mdl in data)
                {
                    // endGrid:
                    var savedData = _gt.GetDataFromMachineDataDtls(mstId, mdl.TestCode, mdl.SampleNo, mdl.ParameterName);
                    if ((savedData.Result != "") && (savedData.Result != null))
                    {
                        mdl.Result = savedData.Result;
                    }
                    mdl.ReportNo = savedData.ReportNo != "" ? savedData.ReportNo : "";

                    dataGridView2.Rows.Add(mdl.ParameterSlNo,mdl.ParameterName,":", mdl.Result, mdl.IsBold,  mdl.TestCode, mdl.ReportNo, mdl.SampleNo);
                    string specimen = _gt.FncReturnFielValueLab("tb_Parameter_Definition_Imaging", "Testcode='" + mdl.TestCode + "'", "Specimen");
                    specimenTextBoxNew.Text = specimen;

                }
                
                
                
                //}
          
            
           

                if (pCode=="")
                {
                    commentsRichTextBox.Text = "";
                    commentsRichTextBox.Text= _gt.FncReturnFielValueLab("tb_MachineDataDtls", "MasterId=" + masterId + " AND ReportFileName='" + groupName + "'", "Comments");
                }
                else if (pCode != "")
                {
                    commentsRichTextBox.Text = "";
                    commentsRichTextBox.Text = _gt.FncReturnFielValueLab("tb_MachineDataDtls", "MasterId=" + masterId + " AND ReportFileName='" + groupName + "' AND TestCode='"+ pCode +"' ", "Comments");
                }


            //  commentsRichTextBox.Text =_gt.FncReturnFielValueLab("")





                //GridWidth(dataGridView2);
                _gt.GridColor(dataGridView2);
        }

   



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                _testCode = dataGridView1.SelectedCells[0].Value.ToString();
                _reportFileName = dataGridView1.SelectedCells[4].Value.ToString();
               
                dataGridView2.Rows.Clear();
                _gt.DeleteInsertLab("DELETE FROM A_Imaging_Report_X_Ray_Ultra");
                _gt.DeleteInsertLab("INSERT INTO A_Imaging_Report_X_Ray_Ultra(InvNo, InvDate, Name, Sex, Age, DrName, TestName)VALUES('" + FourDigitTextBox.Text + "', '" + invDateDateTimePicker.Value.ToString("yyyy-MM-dd") + "', '" + patientNameTextBox.Text + "', '" + sexTextBox.Text + "', '" + ageTextBox.Text + "', '" + drNameTextBox.Text + "', '" + dataGridView1.SelectedCells[1].Value + "')");
                string destinationFolder = Application.StartupPath + "\\Saved_Report\\" + FourDigitTextBox.Text + "_" + _reportFileName + ".doc";
                string sourceFileName = Application.StartupPath + "\\ImagingWordReportFile\\" + _reportFileName + ".doc";



                
                try
                {
                    if (File.Exists(destinationFolder) == false)
                    {
                        File.Copy(sourceFileName, destinationFolder, true);
                    }
                    Process.Start(destinationFolder);
                    //_gt.DeleteInsertLab("Update tb_LabSampleStatusInfo SET CollStatus='Collected',CollTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',CollUser='" + Hlp.UserName + "',SendStatus='Send',SendTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',SendUser='" + Hlp.UserName + "',ReceiveInLabStatus='Collected',ReceiveInLabTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',ReceiveInLabUser='" + Hlp.UserName + "',ReportProcessStatus='Processed',ReportProcessTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',ReportProcessUser='" + Hlp.UserName + "',  ReportprintStatus='Printed',ReportPrintUser='" + Hlp.UserName + "',ReportPrintTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',ReportNo='" + reportNo + "',FinalStatus='Report Process',RDeliveryDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE MasterId='" + masterId + "'  AND TestCode='" + _testCode + "' ");
                    _gt.DeleteInsertLab("Update tb_BILL_DETAIL SET PrintStatus=1 WHERE MasterId='" + masterId + "'  AND TestId='" + _testCode + "'");

                    if (_gt.FnSeekRecordNewLab("tb_DOCTOR", "ReportUserName='" + Hlp.UserName + "'"))
                    {
                        double reportFee = Convert.ToDouble(_gt.FncReturnFielValueLab("tb_TESTCHART", "Id='" + _testCode + "'", "ReportFee"));
                        double charge = Convert.ToDouble(_gt.FncReturnFielValueLab("tb_TESTCHART", "Id='" + _testCode + "'", "Charge"));
                        int drId = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_DOCTOR", "ReportUserName='" + Hlp.UserName + "'", "Id"));
                        _gt.DeleteInsertLab("INSERT INTO tb_DOCTOR_LEDGER(MasterId, DrId, TestId, Charge, HnrAmt,LessAmt)VALUES(" + masterId + ", '" + drId + "', '" + _testCode + "', '" + charge + "', '" + reportFee + "', '0')");
                    }

                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void dataGridView3_KeyDown(object sender, KeyEventArgs e)
        {
           

            if (e.KeyCode==Keys.Enter)
            {
                e.Handled = true;
                if (dataGridView3.SelectedCells.Count > 0)
                {
                    dataGridView2.Rows.Clear();
                    totalCountTextBox.Text = "0";
                    _reportFileName = dataGridView3.SelectedCells[0].Value.ToString();
                    GetParameterDataByCode(masterId, "", _reportFileName,"");
                   // CalculateDifferentialCount();

                    var testName = (from mDl in testCodeList where mDl.GroupName == _reportFileName select new TestCodeModel() { ItemDesc = mDl.ItemDesc }).ToList();
                    string pTestName = "";
                    foreach (var model in testName)
                    {
                        if (pTestName == "")
                        {
                            pTestName = model.ItemDesc;
                        }
                        else
                        {
                            pTestName += "," + model.ItemDesc;
                        }

                    }
                    testNameForView = pTestName;

                }
            }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode==Keys.Left)
            //{
            //    dataGridView3.Focus();
            //    if (dataGridView2.CurrentRow != null)
            //    {
            //        dataGridView2.CurrentRow.Selected = false;
            //        if (dataGridView3.CurrentRow != null) dataGridView3.CurrentRow.Selected = true;
            //    }
            //}
            if (e.KeyCode==Keys.ControlKey)
            {
              int rowIndex=  dataGridView2.CurrentCell.RowIndex;
              string testCode = dataGridView2.Rows[rowIndex].Cells[13].Value.ToString();
              string parameter = dataGridView2.Rows[rowIndex].Cells[10].Value.ToString();// dataGridView2.SelectedCells[10].Value.ToString();
                if (_gt.FnSeekRecordNewLab("tb_DefaultResultSetup","PCode='"+ testCode +"' AND Name='"+ parameter +"'"))
                {
                    Show_Combobox(rowIndex, 1, testCode, parameter);   
                }
            }
            if (e.KeyCode == Keys.Delete)
            {
                //int rowIndex = dataGridView2.CurrentCell.RowIndex;
                //string parameter = dataGridView2.Rows[rowIndex].Cells[10].Value.ToString();
                //string labNo = dataGridView2.Rows[rowIndex].Cells[16].Value.ToString();// dataGridView2.SelectedCells[16].Value.ToString();
                //_gt.DeleteInsertLab("UPDATE tb_MachineDataDtls SET IsPrint=0 WHERE  MasterId=" + masterId + " AND Parameter='"+ parameter +"' AND LabNo='"+ labNo +"' ");
                this.dataGridView2.Rows.RemoveAt(this.dataGridView2.CurrentCell.RowIndex);
            }



         
            if ((e.KeyCode==Keys.Down)||(e.KeyCode==Keys.Up))
            {
                int rowIndex = dataGridView2.CurrentCell.RowIndex;

               
                
                



               




            }
            
            
            
            //  fncCalculatePrmResult();


           






        }

        private string lcParameter = "";
        private string lcPcode = "";






      
        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    string code = dataGridView2.SelectedCells[13].Value.ToString();
            //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //    {
            //        string gdCode =  dataGridView1.Rows[i].Cells[0].Value.ToString();
            //        if (gdCode==code)
            //        {
            //            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Gold;
            //            dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.DarkRed;
            //        }
            //        else
            //        {
            //            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
            //            dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.DarkRed;
            //        }
            //    }
            //}
            //catch (Exception exception)
            //{
            //    return;
            //}

            

        }

        private void defaultResultComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                defaultResultComboBox.Visible = false;
                dataGridView2.CurrentCell = dataGridView2.Rows[dataGridView2.CurrentCell.RowIndex].Cells[1];
                dataGridView2.CurrentCell.Value = defaultResultComboBox.Text;
               // dataGridView2.Rows[10].Selected = true;
                dataGridView2.Focus();
            }
        }
        
        private void dataGridView3_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //if (e.RowIndex < 0 || e.ColumnIndex < 0)
            //    return;
            //if (e.ColumnIndex == 0) // Also you can check for specific row by e.RowIndex
            //{
            //    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~(DataGridViewPaintParts.ContentForeground));
            //    var r = e.CellBounds;
            //    r.Inflate(-4, -4);
            //    e.Graphics.FillRectangle(Brushes.Gold, r);
            //    e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
            //    e.Handled = true;
            //}
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _gt.DeleteInsertLab("Update tb_DefaultLabDoctorSetting SET LabIncharge=" + labInchargeComboBox.SelectedValue + " WHERE MachineName='" + System.Environment.MachineName + "'");
            MessageBox.Show(@"Update success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void button2_Click_2(object sender, EventArgs e)
        {
            _gt.DeleteInsertLab("Update tb_DefaultLabDoctorSetting SET CheckedBy=" + checkedByComboBox.SelectedValue + " WHERE MachineName='" + System.Environment.MachineName + "'");
            MessageBox.Show(@"Update success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            _gt.DeleteInsertLab("Update tb_DefaultLabDoctorSetting SET Consultant=" + consultantComboBox.SelectedValue + " WHERE MachineName='" + System.Environment.MachineName + "'");
            MessageBox.Show(@"Update success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void sampleNoTextBox_Enter(object sender, EventArgs e)
        {
            sampleNoTextBox.BackColor = Hlp.EnterFocus();
            if (Hlp.GroupName=="")
            {
                FourDigitTextBox.Focus();
            }
            if (Hlp.GroupName != "")
            {
                FourDigitTextBox.ReadOnly = true;
            }
        }

        private void sampleNoTextBox_Leave(object sender, EventArgs e)
        {
            sampleNoTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void sampleNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (_gt.FnSeekRecordNewLab("tb_LabSampleStatusInfo","SampleNo='"+ sampleNoTextBox.Text +"'"))
                {
                    totalCountTextBox.Text = "0"; 
                    masterId =Convert.ToInt32(_gt.FncReturnFielValueLab("tb_LabSampleStatusInfo","SampleNo='" + sampleNoTextBox.Text + "'", "MasterId"));
                    FourDigitTextBox.Text = _gt.FncReturnFielValueLab("tb_InvMaster","Id=" + masterId + "", "InvNo");
                    testCodeList = _gt.GetInvoiceDetailByInvNo(masterId,sampleNoTextBox.Text);
                    if (testCodeList.Count>0)
                    {
                        PopulateMasterData(testCodeList);
                        GetInvoiceDetailByInvNo();
                    }
                    
                    dataGridView2.Rows.Clear();
                    _reportFileName = _gt.FncReturnFielValueLab("tb_LabSampleStatusInfo", "SampleNo='" + sampleNoTextBox.Text + "'", "VaqGroup");
                    //dataGridView3.SelectedCells[0].Value.ToString();
                    var testName = (from mDl in testCodeList where mDl.GroupName == _reportFileName select new TestCodeModel() { ItemDesc = mDl.ItemDesc }).ToList();
                    string pTestName = "";
                    foreach (var model in testName)
                    {
                        if (pTestName == "")
                        {
                            pTestName = model.ItemDesc;
                        }
                        else
                        {
                            pTestName += "," + model.ItemDesc;
                        }

                    }
                    testNameForView = pTestName;
                    GetParameterDataByCode(masterId, "", _reportFileName,sampleNoTextBox.Text );

                    return;
                }
                MessageBox.Show(@"Invalid Sample Id.", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            //if (dataGridView2.Rows.Count>2)
            //{
            //    int rowIndex = dataGridView2.CurrentCell.RowIndex;
            //    string testName = dataGridView2.Rows[rowIndex].Cells[0].Value.ToString();
            //    if (testName == "Atypical Cells")
            //    {
            //        dataGridView2.Rows[rowIndex].Cells[0].ReadOnly = false;
            //    }
            //}


        }
        BarcodePrintGateway _gtBarcode = new BarcodePrintGateway();
       

        private void FourDigitTextBox_TextChanged(object sender, EventArgs e)
        {
            if (FourDigitTextBox.TextLength != 9)
            {
                dataGridView1.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView2.Rows.Clear();
            }
        }

        private void sampleNoTextBox_TextChanged(object sender, EventArgs e)
        {
            if (sampleNoTextBox.TextLength != 9)
            {
                dataGridView1.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView2.Rows.Clear();
            }
        }

        private void specimenTextBoxNew_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode==Keys.Enter)
            {
                var lists = new List<UnitModel>();
                if (dataGridView2.Rows.Count>0)
                {
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        string testCode = dataGridView2.Rows[i].Cells[13].Value.ToString();
                        if (_gt.FnSeekRecordNewLab("tb_Parameter_Definition", "Testcode='" + testCode + "'"))
                        {
                            _gt.DeleteInsertLab("UPDATE tb_Parameter_Definition SET Specimen='" + specimenTextBoxNew.Text + "' WHERE Testcode='" + testCode + "'");
                            MessageBox.Show(@"Update Success",@"Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            goto endloop;
                        }
                    }
                }
            endloop:;
            }
        }


        private KeyMessageFilter m_filter = null;
        private void ReportPrintUsgUi_Load(object sender, EventArgs e)
        {

            m_filter = new KeyMessageFilter(this);
            Application.AddMessageFilter(m_filter);




            //_gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_DoctorSetup WHERE Type='Checked By' Order By Id ", checkedByComboBox);
            //_gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_DoctorSetup WHERE Type='Consultant' Order By Id ", consultantComboBox);
            //_gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_DoctorSetup WHERE Type='Lab Incharge' Order By Id ", labInchargeComboBox);


            GridWidth(dataGridView2);
            invNoFirstPartTextBox.Text = "";//DateTime.Now.ToString("yyyy").Substring(2,2)+DateTime.Now.ToString("MM").PadLeft(2, '0');
            //FourDigitTextBox.Focus();
          //  dataGridView4.DataSource = _gt.GetInvoiceList(invDateDateTimePicker.Value);
           // Gridwidth(1);
           // _gt.GridColor(dataGridView4);
            ClearText();

            //if (Hlp.GroupName == "")
            //{
            //    FourDigitTextBox.Focus();
            //}
            //else
            //{
            //    sampleNoTextBox.Focus();
            //}

            FourDigitTextBox.Focus();


            //if (_gt.FnSeekRecordNewLab("tb_DefaultLabDoctorSetting", "MachineName='" + System.Environment.MachineName + "'") == false)
            //{
            //    _gt.DeleteInsertLab("INSERT INTO tb_DefaultLabDoctorSetting(CheckedBy, Consultant, LabInCharge, MachineName)VALUES(0, 0, 0, '" + System.Environment.MachineName + "')");
            //}



            //checkedByComboBox.SelectedValue = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_DefaultLabDoctorSetting", "MachineName='" + System.Environment.MachineName + "'", "CheckedBy"));
            //consultantComboBox.SelectedValue = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_DefaultLabDoctorSetting", "MachineName='" + System.Environment.MachineName + "'", "Consultant"));
            //labInchargeComboBox.SelectedValue = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_DefaultLabDoctorSetting", "MachineName='" + System.Environment.MachineName + "'", "LabInCharge"));


            autoPrintCheckBox.Checked = true;

            //if (Hlp.IsComeFromIndividualSearch)
            //{
            //    FourDigitTextBox.Text = Hlp.InvoiceNoComeFromIndividualSearch;
            //    GetDataForInvoicePrint();
            //    FourDigitTextBox.Focus();
            //}
           
            FourDigitTextBox.Text = "IN" + DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString().PadLeft(2, '0');
            FourDigitTextBox.Select(FourDigitTextBox.Text.Length, 0);
           // FourDigitTextBox.Focus();

            if (_gt.FnSeekRecordNewLab("tb_DOCTOR", "ReportUserName='"+ Hlp.UserName +"'"))
            {
                lblDrName.Text = _gt.FncReturnFielValueLab("tb_DOCTOR", "ReportUserName='" + Hlp.UserName + "'", "Name");
            }




        }

       
    }
}
