using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.VisualBasic.CompilerServices;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Diagnosis;
using SuperPathologyApp.Report;
using SuperPathologyApp.Report.DataSet;

namespace SuperPathologyApp.UI.Lab
{
    public partial class ReportPrintUi : Form
    {
        string _invNo = "";
        readonly ReportPrintGateway _gt = new ReportPrintGateway();
        public static string  LabReportquery = "";
        public static string LabReportFileName = "";
        private bool _isSaved=true;
        private string _reportFileName = "";
        private  string _testCode = "";
        List<TestCodeModel> _testCodeList=new List<TestCodeModel>();
        private string _testNameForView = "";

        private bool _isClickSaveButton = true;

        
        
        public ReportPrintUi()
        {
            InitializeComponent();
           
            if (Hlp.GroupName == "")
            {
                FourDigitTextBox.Focus();
            }
            else
            {
               // sampleNoTextBox.Focus();
            }
        }
        private void frmReportPrint_Load(object sender, EventArgs e)
        {

        }

        private void ClearText()
        {
         //   patientNameTextBox.Text = "";
         //   ageTextBox.Text = "";
          //  drCodeTextBox.Text= "";
          //  drNameTextBox.Text = "";
           // commentsRichTextBox.Text = "";
         
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

            if (_masterId==0)
            {
                MessageBox.Show(@"Invalid InvoiceNo Please Check", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            
            var data = _gt.GetMasterData(_masterId);
            lblDate.Text = data.BillDate.ToString("dd-MMM-yyyy");
            lblPtName.Text = data.PatientName;
            lblAge.Text = data.Age;
            lblSex.Text = data.Sex;
            lblDrName.Text = data.ConsDrModel.Name;
            ptNameTextBox.Text = data.PatientName;
           
            ptSexTextBox.Text = data.Sex;
            drNameComboBox.SelectedValue  = data.ConsDrModel.DrId;




            
            var dtls = _gt.GetDetailsData(_masterId,Hlp.GroupName);

            if (dtls.Count > 0)
            {
                dataGridView3.Rows.Clear();
                dataGridView1.Rows.Clear();


                var distinctItems = dtls.GroupBy(x => x.ReportFileName).Select(y => y.First());
                int j = 0;
                foreach (var codeModel in distinctItems)
                {
                    if (codeModel != null) dataGridView3.Rows.Add(codeModel.ReportFileName);
                    dataGridView3.Rows[j].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dataGridView3.Rows[j].DefaultCellStyle.ForeColor = Color.DarkRed;
                    j++;
                }
                


         

               // _gt.GridColor(dataGridView3);

                int i = 0;
                _testCodeList = dtls.GroupBy(x => x.TestCode).Select(y => y.First()).ToList();
           
                
                foreach (var codeModel in _testCodeList)
                {
                    if (codeModel.ReportPrintStatus=="1")
                    {
                        /////////Printed
                        dataGridView1.Rows.Add(codeModel.TestCode, codeModel.TestName, 1, "", codeModel.ReportFileName);
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.DarkRed;

                    }
                    else if (codeModel.ReportPrintStatus == "1")
                    {
                        /////////Saved
                        dataGridView1.Rows.Add(codeModel.TestCode, codeModel.TestName, 1, "", codeModel.ReportFileName);
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.BlanchedAlmond;
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.DarkRed;

                    }
                    else
                    {
                        dataGridView1.Rows.Add(codeModel.TestCode, codeModel.TestName, 1, "", codeModel.ReportFileName);
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.DarkRed;

                    }
                    
                    i++;
                }
                dataGridView1.Rows[0].Selected = false;
                dataGridView3.Rows[0].Selected = false;
              
                //_gt.GridColor(dataGridView1);
            }
            else {

                ClearText();
                dataGridView3.Rows.Clear();
                dataGridView1.Rows.Clear();
                        
            }
        }

       

    
        private void GridWidth(DataGridView dataGrid)
        {
            //dataGrid.Columns[0].Width = 175;
            //dataGrid.Columns[1].Width = 220;
            //dataGrid.Columns[2].Width = 60;
            //dataGrid.Columns[3].Width = 180;
         
            //dataGrid.Columns[4].Visible = false;
            //dataGrid.Columns[5].Visible = false;
            //dataGrid.Columns[6].Visible = false;
            //dataGrid.Columns[7].Visible = false;
            //dataGrid.Columns[8].Visible = false;
            //dataGrid.Columns[9].Visible = false;
            //dataGrid.Columns[10].Visible = false;
            //dataGrid.Columns[11].Visible = false;
          
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
           // dataGrid.Columns["Result"].DefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold);


        


        }
   
      
   
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count < 1)
            {
                MessageBox.Show(@"No Data Found To View", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Hlp.AutoPrint = false;
            
            
            
            string invNo = FourDigitTextBox.Text;
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
            _testNameForView = testNameForRpt;

            
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
                        PtName = lblPtName.Text,
                        PtAge = lblAge.Text ,
                        PtSex = lblSex.Text,
                        DrName = lblDrName.Text ,
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
                        MasterId = _masterId,
                        PtInvNo = invNo,
                        PtInvDate =Convert.ToDateTime(lblDate.Text),
                        ReportNo = "0",
                        CommentsInv = commentsRichTextBox.Text,
                        UserName = collUser,
                        TestName = _testNameForView,
                        SpecimenName = specimen,
                        SampleCollectionTime = collTime,
                        PrintBy = Hlp.UserName,
                        //SampleCollectionUserName = collUser,
                    });
            }

            #region
            foreach (TestCodeModel model in mdl)
            {
                if ((leftDrComboBox.Text == @"--Select--"))
                {
                    model.CheckedByName = "";
                    model.CheckedByDegree = "";
                }
                else
                {
                    model.CheckedByName = leftDrComboBox.Text;
                    model.CheckedByDegree = _gt.FncReturnFielValueLab("tb_DoctorSetup", "Id=" + leftDrComboBox.SelectedValue + "", "Details");
                }
            }
            foreach (TestCodeModel model in mdl)
            {
                if ((middleDrComboBox.Text == @"--Select--"))
                {
                    model.ConsultantName = "";
                    model.ConsultantDegree = "";
                }
                else
                {
                    model.ConsultantName = middleDrComboBox.Text;
                    model.ConsultantDegree = _gt.FncReturnFielValueLab("tb_DoctorSetup", "Id=" + middleDrComboBox.SelectedValue + "", "Details");
                }
            }
            foreach (TestCodeModel model in mdl)
            {
                if ((rightDrComboBox.Text == @"--Select--"))
                {
                    model.LabInchargeName = "";
                    model.LabInchargeDegree = "";
                }
                else
                {
                    model.LabInchargeName = rightDrComboBox.Text;
                    model.LabInchargeDegree= _gt.FncReturnFielValueLab("tb_DoctorSetup", "Id=" + rightDrComboBox.SelectedValue + "", "Details");
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

            LabReportquery = "SELECT * FROM A_TMP_Lab_ReportView WHERE MasterId='" + _masterId + "'  AND ReportFileName='" + _reportFileName + "' AND PrintBy='"+ Hlp.UserName +"' ";
            var gl = new FrmReportViewer(_reportFileName,LabReportquery);
            gl.ShowDialog();


            
        }


        private void Show_Combobox(int iRowIndex, int iColumnIndex, string testCode, string paramName)
        {
            _gt.LoadComboBoxForDefault("SELECT 0 AS Id,Result AS Description FROM tb_TESTCHART_DEF_RESULT WHERE TestChartId='" + testCode + "' AND  MachineParam='" + paramName + "'", defaultResultComboBox);
            
            
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

        private int _masterId = 0;
        private void FourDigitTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            
            if (e.KeyCode == Keys.Enter)
            {
                GetDataForInvoicePrint();
            }
        }

        private void GetDataForInvoicePrint()
        {
            if (FourDigitTextBox.Text.Length<2)
            {
                return;
            }
            _invNo =  FourDigitTextBox.Text;

            dataGridView2.Rows.Clear();
          //  totalCountTextBox.Text = @"0";

            if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "BillNo='" + _invNo + "'"))
            {
                _masterId = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + _invNo + "'", "Id"));
                if (_gt.FnSeekRecordNewLab("V_Due_Invoice_List","MasterId="+ _masterId +" AND Balance>0"))
                {
                    if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE", "UserName='" + Hlp.UserName + "' AND ParentName='Lab' AND ChildName='Can Print Due Report'")==false)
                    {
                        double ratio = Convert.ToDouble(_gt.FncReturnFielValueLab("V_Due_Invoice_List", "MasterId=" + _masterId + "", "SUM((Balance*100)/BillAmt)"));
                        double ratioInDb = Convert.ToDouble(_gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ReportPrintDuePercentage"));
                        if (ratio > ratioInDb)
                        {
                            label6.Text = @"Due:" + _gt.FncReturnFielValueLab("V_Due_Invoice_List", "MasterId=" + _masterId + "", "Balance");
                            MessageBox.Show(@"You Can Not View Report When Due Is Bigger Than " + ratioInDb + "%", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            label6.Text = @"Due:" + _gt.FncReturnFielValueLab("V_Due_Invoice_List", "MasterId=" + _masterId + "", "Balance");
                        }
                    }

                }
                else
                {
                    label6.Text = @"FULL PAID";
                }
            }
            GetInvoiceDetailByInvNo();
            dataGridView2.Rows.Clear();

        }



        private SqlTransaction _trans;
 

        private void Save(string invNo)
        {

            

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
                var mdl = new List<TestParamModel>();
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    string result = dataGridView2.Rows[i].Cells[1].Value == null ? "" : dataGridView2.Rows[i].Cells[1].Value.ToString();
                    if (result.Trim() != "")
                    {
                        mdl.Add(new TestParamModel()
                        {
                            ReportParam = dataGridView2.Rows[i].Cells[0].Value.ToString(),
                            DefaultResult = result,
                            Unit = dataGridView2.Rows[i].Cells[2].Value.ToString(),
                            NormalRange = dataGridView2.Rows[i].Cells[3].Value.ToString(),
                            MachineParam = dataGridView2.Rows[i].Cells[4].Value.ToString(),
                            GroupSl = Convert.ToInt32(dataGridView2.Rows[i].Cells[6].Value),
                            ParamSl = Convert.ToInt32(dataGridView2.Rows[i].Cells[7].Value),
                            ReportingGroup = dataGridView2.Rows[i].Cells[8].Value.ToString(),
                            IsBold = Convert.ToInt32(dataGridView2.Rows[i].Cells[9].Value),
                            TestchartId = Convert.ToInt32(dataGridView2.Rows[i].Cells[10].Value),
                            ReportNo =  dataGridView2.Rows[i].Cells[11].Value == null ? "" : dataGridView2.Rows[i].Cells[11].Value.ToString(),
                            Comment=commentsRichTextBox.Text,
                        });
                    }
                }
                if (mdl.Count < 1)
                {
                    MessageBox.Show(@"No Result Found For Save", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _isSaved = false;
                    return;
                }
    
                var reportModel = new ReportModel();
                #endregion

                reportModel.TestParam = mdl;
                reportModel.MasterId = _masterId;
 
                #region consultant
                if (leftDrComboBox.SelectedIndex != 0)
                {
                    reportModel.LeftDoctor = new ReportDoctorModel()
                    {
                        Name = leftDrComboBox.Text,
                        Degree = _gt.FncReturnFielValueLab("tb_LAB_DOCTOR", "Id=" + leftDrComboBox.SelectedValue + "", "Degree", _trans, inv.ConLab)
                    };

                }
                else
                {
                    reportModel.LeftDoctor = new ReportDoctorModel()
                    {
                        Name = "",
                        Degree = "",
                    };
                }

                if (middleDrComboBox.SelectedIndex != 0)
                {
                    reportModel.MiddleDoctor = new ReportDoctorModel()
                    {
                        Name = middleDrComboBox.Text,
                        Degree = _gt.FncReturnFielValueLab("tb_LAB_DOCTOR", "Id=" + middleDrComboBox.SelectedValue + "", "Degree", _trans, inv.ConLab)
                    };
                }
                else {
                    reportModel.MiddleDoctor = new ReportDoctorModel()
                    {
                        Name = "",
                        Degree = "",
                    };
                }


                if (rightDrComboBox.SelectedIndex!=0)
                {
                    reportModel.RightDoctor = new ReportDoctorModel()
                    {
                        Name = rightDrComboBox.Text,
                        Degree = _gt.FncReturnFielValueLab("tb_LAB_DOCTOR", "Id=" + rightDrComboBox.SelectedValue + "", "Degree", _trans, inv.ConLab)
                    };
                }
                else
                {
                    reportModel.RightDoctor = new ReportDoctorModel()
                    {
                        Name = "",
                        Degree = "",
                    };
                }
                #endregion 

                string rtnMessage = _gt.Save(reportModel, _trans, inv.ConLab);
                if (rtnMessage == _gt.SaveSuccessMessage)
                {
                    _isSaved = true;
                }
                else
                {
                    MessageBox.Show(rtnMessage, @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _isSaved = false;
                }
    
                _trans.Commit();
                inv.ConLab.Close();

                if (_isClickSaveButton == false)
                {
                    foreach (var model in mdl)
                    {
                        if (model.DefaultResult.Length>0)
                        {
                            _gt.DeleteInsertLab("Update tb_BILL_DETAIL SET PrintStatus=1 WHERE MasterId='" + _masterId + "'  AND TestId='" + model.TestchartId + "'");
                           // _gt.DeleteInsertLab("Update tb_LabSampleStatusInfo SET   ReportprintStatus='Printed',ReportPrintUser='" + Hlp.UserName + "',ReportPrintTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',ReportNo='" + model.ReportNo + "',FinalStatus='ReportPrint',RDeliveryDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE MasterId='" + masterId + "'  AND TestCode='" + model.TestCode + "' ");
                        }
                    }
                }
                else
                {
                    foreach (var model in mdl)
                    {
                        if (model.DefaultResult.Length > 0)
                        {
                            _gt.DeleteInsertLab("Update tb_BILL_DETAIL SET PrintStatus=2 WHERE MasterId='" + _masterId + "'  AND TestId='" + model.TestchartId + "' AND PrintStatus<>1");
                        }
                    }
                }


                ClearText();
                dataGridView2.Rows.Clear();
                commentsRichTextBox.Text = "";
              
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

       

   

  

  




        private void button6_Click(object sender, EventArgs e)
        {
            _gt.DeleteInsertLab("Update tb_LAB_DOCTOR_DEFAULT SET [Middle]=" + middleDrComboBox.SelectedValue + " WHERE PcName='" + System.Environment.MachineName + "'");
            MessageBox.Show(@"Update success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private void UpdateDataGrid()
        {

            //Thread.Sleep(500);

            double wbc = 0,eos=0;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                string paramName = dataGridView2.Rows[i].Cells[10].Value.ToString();
                switch (paramName)
                {
                    case "WBC":
                        wbc = _gt.IsNumeric(dataGridView2.Rows[i].Cells[1].Value.ToString()) ? Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value.ToString()) : 0;
                        break;
                    case "EOS%":
                        eos = _gt.IsNumeric(dataGridView2.Rows[i].Cells[1].Value.ToString()) ? Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value.ToString()) : 0;
                        break;
                }
            }
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                string paramName = dataGridView2.Rows[i].Cells[10].Value.ToString();
                if (paramName == "TCE")
                {
                    dataGridView2.Rows[i].Cells[1].Value = Math.Round(wbc / 100 * eos, 2);
                }


            }


        }

  

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                CalculateDifferentialCount();
            }
 
        }



       
        private void saveAndPrintButton_Click(object sender, EventArgs e)
        {
            _isClickSaveButton = false;
            //CalculateDifferentialCount();

            if (dataGridView2.Rows.Count < 1)
            {
                MessageBox.Show(@"No Data Found To Save", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (totalCountTextBox.Text == "")
            {
                totalCountTextBox.Text = @"0";
            }
            double diffCount = Convert.ToDouble(totalCountTextBox.Text);
   
            if (diffCount > 0)
            {
                if (diffCount != 100)
                {
                    MessageBox.Show(@"Diffential Count Must Be 100", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            #region testName
            string testNameForRpt = "";
            string testNameInGrid = "";
            var listOfParamInGrid = new List<UnitModel>();
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                string result = dataGridView2.Rows[i].Cells[1].Value == null ? "" : dataGridView2.Rows[i].Cells[1].Value.ToString();
                //if (result.Trim() != "")
                //{
                    string tstName = dataGridView2.Rows[i].Cells[12].Value.ToString();
                    string mchnParam = dataGridView2.Rows[i].Cells[4].Value == null ? "" : dataGridView2.Rows[i].Cells[4].Value.ToString();
                    listOfParamInGrid.Add(new UnitModel()
                    {
                        UnitName = mchnParam,
                    });
                    if (testNameInGrid != tstName)
                    {
                        testNameInGrid = dataGridView2.Rows[i].Cells[12].Value.ToString();
                        if (testNameForRpt == "")
                        {
                            testNameForRpt = dataGridView2.Rows[i].Cells[12].Value.ToString();
                        }
                        else
                        {
                            testNameForRpt = testNameForRpt + "," + dataGridView2.Rows[i].Cells[12].Value.ToString();
                        }
                    }
               // }
            }
            _testNameForView = testNameForRpt;
            var distinctParam = listOfParamInGrid.Select(x => x.UnitName).Distinct();
            string paramInGrid = "";
            foreach (var sDl in distinctParam)
            {
                if (paramInGrid == "")
                {
                    paramInGrid = "('" + sDl;
                }
                else
                {
                    paramInGrid += "','" + sDl;
                }
            }
            paramInGrid += "')";
            string paramCond = " AND MachineParam IN"+paramInGrid;
            #endregion




            Save(FourDigitTextBox.Text);
            

            if (_isSaved)
            {
                string testCond = "";
                if (_testCode!="")
                {
                    testCond = "AND TestChartId='"+ _testCode +"'";
                }

                string reportHeader = _gt.FncReturnFielValueLab("tb_REPORT_HEADER_DEFAULT","ReportFileName='"+ _reportFileName +"'","HeaderName");
                LabReportquery = "SELECT '" + _testNameForView + "' AS TestName,'" + Hlp.UserName + "' AS PrintBy,'"+ reportHeader +"' AS ReportHeader,  * FROM V_REPORT WHERE MasterId=" + _masterId + " " + testCond + "  AND IsPrint=1  "+ paramCond +"";

                var ds = Getdata(LabReportquery);
                if (ds.Rows.Count<1)
                {
                    MessageBox.Show(@"No Item For Save", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (_reportFileName=="MICROBIOLOGY GROWTH")
                {
                    _reportFileName = "MICROBIOLOGY";
                }
                
                
                if (Hlp.AutoPrint)
                {
                    PrintWithOutShow();
                }
                else
                {
                    var dt = new LabReqViewer(_reportFileName, LabReportquery, "", "V_REPORT", "LabReport");
                    dt.ShowDialog();

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
              
            }


            //FourDigitTextBox.Focus();
            //FourDigitTextBox.Select(3, 4);
            //GetInvoiceDetailByInvNo();
        }

        readonly DbConnection db = new DbConnection();


        internal DataTable Getdata(string searchString)
        {
            try
            {


                _gt.ConLab.Open();
                var da = new SqlDataAdapter(searchString, _gt.ConLab);
                var ds = new DataSet();
                da.Fill(ds, "pCode");
                var table = ds.Tables["pCode"];
                _gt.ConLab.Close();
                return table;
            }
            catch (Exception exception)
            {
                throw;
            }
        }
        
        
        
        
        
        
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
                var ds = new DataSet1();
                sda.Fill(ds,"DT_LAB_REPORT_VIEW");
                rprt.SetDataSource(ds);

                rprt.SetParameterValue("lcComName", comName);
                rprt.SetParameterValue("lcComAddress",comAddress );
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
                LabReportquery = "SELECT " + _testNameForView + " AS TestName, * FROM VW_GET_LAB_REPORT_VIEW WHERE MasterId='" + _masterId + "' AND IsPrint=1   ";
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
        private void GetDataByGroupName(string rptFileName,string testCode)
        {
            dataGridView2.Rows.Clear();
            var data = _gt.GetDetailsData(_masterId,Hlp.GroupName).Where(n => n.ReportFileName == rptFileName);
            if (testCode!="")
            {
                data = data.Where(n => n.TestCode == testCode);
            }
            foreach (var item in data)
            {
                GetParameterDataByCode(_masterId,Convert.ToInt32(item.TestCode));
            }
            CalculateDifferentialCount();
        }
        double _hba1C = 0,_gluc=0,crea=0;
        private void CalculateDifferentialCount()
        {
            double totCount = 0.0;
            if (dataGridView2.Rows.Count>0)
            {
                int num2 = checked(dataGridView2.Rows.Count - 1);
                int i = 0;
                while (i <= num2)
                {
                    if (_gt.FnSeekRecordNewLab("tb_DIFF_COUNT_PARAM", "Param='" + dataGridView2.Rows[i].Cells[4].Value + "'"))
                    {
                        if (_gt.IsNumeric(dataGridView2.Rows[i].Cells[1].Value.ToString()))
                        {
                            totCount += Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value.ToString());
                            dataGridView2.Rows[i].Cells[1].Value = Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value).ToString().PadLeft(2, '0');
                        }
                    }
                    string param = dataGridView2.Rows[i].Cells[4].Value.ToString();
                    #region hba1c
                    if (param.ToUpper()=="HBA1C")
                    {
                        if (_gt.IsNumeric(dataGridView2.Rows[i].Cells[1].Value.ToString()))
                        {
                            _hba1C = Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value.ToString());
                        }
                    }
                    if (_hba1C>0)
                    {
                        if (param.ToUpper()=="EAG")
                        {
                            dataGridView2.Rows[i].Cells[1].Value =Math.Round((28.7*_hba1C) - 46.7);
                            _hba1C = 0;
                        }
                    }
                    #endregion
                    #region Gluc
                    if (param.ToUpper() == "GLUC")
                    {
                        if (_gt.IsNumeric(dataGridView2.Rows[i].Cells[1].Value.ToString()))
                        {
                            _gluc = Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value.ToString());    
                        }
                    }
                    if (_gluc > 0)
                    {
                        if (param.ToUpper() == "GLUC1")
                        {
                            dataGridView2.Rows[i].Cells[1].Value = Math.Round(_gluc/18,2);
                            _gluc = 0;
                        }
                    }
                    #endregion
                    #region creatinine
                    if (param.ToUpper() == "CREA")
                    {
                        if (_gt.IsNumeric(dataGridView2.Rows[i].Cells[1].Value.ToString()))
                        {
                            crea = Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value.ToString());    
                        }
                    }
                    if (crea > 0)
                    {
                        if (param.ToUpper() == "GFR")
                        {
                            var getAgeSex = Hlp.GetAgeSexByInvNo(_masterId);
                            double egf = (175*crea) - 1.154*Convert.ToInt32(getAgeSex.Age) - 0.203;
                            if (getAgeSex.Sex.ToUpper()=="FEMALE")
                            {
                                egf = egf*0.742;
                            }
                            dataGridView2.Rows[i].Cells[1].Value =Math.Round(egf);
                            crea = 0;
                        }
                    }
                    #endregion

                    checked { ++i; }
                }
                totalCountTextBox.Text = Conversions.ToString(totCount);
            }

          //  UpdateDataGrid();
        }
       
        private void GetParameterDataByCode(int mstId,int testChartId)
        {
            totalCountTextBox.Text = "0";
            var data = _gt.GetDataFromParameterDefinitionByTestCode(testChartId);
            string specimen = _gt.FncReturnFielValueLab("tb_TESTCHART", "Id=" + testChartId + "", "Specimen");
            string testName = _gt.FncReturnFielValueLab("tb_TESTCHART", "Id=" + testChartId + "", "Name");
          
            foreach (var mdl in data)
            {
                //string sign = _gt.FncReturnFielValueLab("tb_REPORT_CALCULATION", "TestId='" + testChartId + "' AND Parameter='" + mdl.MachineParam + "' AND GroupB<>'Result'", "Sign");
                //string groupB = _gt.FncReturnFielValueLab("tb_REPORT_CALCULATION", "TestId='" + testChartId + "' AND Parameter='" + mdl.MachineParam + "' AND GroupB<>'Result'", "GroupB");

                var savedData = _gt.GetDataFromMachineDataDtls(mstId, testChartId, mdl.MachineParam);
                if ((savedData.Result != "") && (savedData.Result != null))
                {
                    mdl.DefaultResult = savedData.Result;
                }
                mdl.ReportNo = savedData.ReportNo != "" ? savedData.ReportNo : "";
                dataGridView2.Rows.Add(mdl.ReportParam, mdl.DefaultResult, mdl.Unit, mdl.NormalRange, mdl.MachineParam, specimen, mdl.GroupSl, mdl.ParamSl, mdl.ReportingGroup, mdl.IsBold, testChartId, mdl.ReportNo, testName);

                string richtext = _gt.FncReturnFielValueLab("tb_REPORT_MASTER", "MasterId=" + _masterId + "  AND ReportNo='" + mdl.ReportNo + "' ", "Comment");
                if (richtext != "")
                {
                    commentsRichTextBox.Text = richtext;
                }
            }
                GridWidth(dataGridView2);
                _gt.GridColor(dataGridView2);
        }

     



     

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            

            commentsRichTextBox.Text = "";
            _testCode = "";
            if (dataGridView1.SelectedCells.Count > 0)
            {
                _reportFileName = dataGridView1.SelectedCells[4].Value.ToString();
                GetDataByGroupName(_reportFileName, dataGridView1.SelectedCells[0].Value.ToString());
            }

            if (dataGridView2.Rows.Count > 0)
            {

                dataGridView2.CurrentCell = dataGridView2.Rows[0].Cells[1];
                dataGridView2.BeginEdit(true);
                // dataGridView2.Rows[0].Cells[1].Selected = true;
            }












        }

        private void dataGridView3_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F11)
            //{
            //    saveAndPrintButton.PerformClick();
            //}


            //if (e.KeyCode == Keys.Right)
            //{
            //    dataGridView2.Focus();
            //    dataGridView2.CurrentRow.Selected = true;
            //    dataGridView2.SelectedCells[1].Selected = true;
            //}
            if (e.KeyCode==Keys.Enter)
            {
                e.Handled = true;
                _reportFileName = dataGridView3.SelectedCells[0].Value.ToString();
                GetDataByGroupName(_reportFileName,"");
                
                if (dataGridView2.Rows.Count>0)
                {
                    
                    dataGridView2.CurrentCell = dataGridView2.Rows[0].Cells[1];
                    dataGridView2.BeginEdit(true);
                   // dataGridView2.Rows[0].Cells[1].Selected = true;
                   
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
            if (e.KeyCode == Keys.ControlKey)
            {
                int rowIndex = dataGridView2.CurrentCell.RowIndex;
                string testCode = dataGridView2.Rows[rowIndex].Cells[10].Value.ToString();
                string parameter = dataGridView2.Rows[rowIndex].Cells[4].Value.ToString();// dataGridView2.SelectedCells[10].Value.ToString();
                if (_gt.FnSeekRecordNewLab("tb_TESTCHART_DEF_RESULT", "TestChartId='" + testCode + "' AND MachineParam='" + parameter + "'"))
                {
                    Show_Combobox(rowIndex, 1, testCode, parameter);
                }
            }
            if (e.KeyCode == Keys.Delete)
            {
                int rowIndex = dataGridView2.CurrentCell.RowIndex;
                string parameter = dataGridView2.Rows[rowIndex].Cells[10].Value.ToString();
                string labNo = dataGridView2.Rows[rowIndex].Cells[4].Value.ToString();// dataGridView2.SelectedCells[16].Value.ToString();
                _gt.DeleteInsertLab("UPDATE tb_REPORT_DETAILS SET IsPrint=0 WHERE  MasterId=" + _masterId + " AND MachineParam='" + parameter + "'  ");
                this.dataGridView2.Rows.RemoveAt(this.dataGridView2.CurrentCell.RowIndex);
            }
           



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
 

        private void button7_Click(object sender, EventArgs e)
        {

            _gt.DeleteInsertLab("Update tb_LAB_DOCTOR_DEFAULT SET [Right]='" + rightDrComboBox.SelectedValue + "' WHERE PcName='" + System.Environment.MachineName + "'");
            MessageBox.Show(@"Update success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
      
        private void button5_Click(object sender, EventArgs e)
        {
            _gt.DeleteInsertLab("Update tb_LAB_DOCTOR_DEFAULT SET [Left]='" + leftDrComboBox.SelectedValue + "' WHERE PcName='" + System.Environment.MachineName + "'");
            MessageBox.Show(@"Update success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

     


       

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

          
           

            if (dataGridView2.Rows.Count > 2)
            {
                int rowIndex = dataGridView2.CurrentCell.RowIndex;
                string testName= dataGridView2.Rows[rowIndex].Cells[0].Value.ToString();
                if (testName == "Others")
                {
                    dataGridView2.Rows[rowIndex].Cells[0].ReadOnly = false;
                }
                if (testName == "Others1")
                {
                    dataGridView2.Rows[rowIndex].Cells[0].ReadOnly = false;
                }
                if (testName == "Others2")
                {
                    dataGridView2.Rows[rowIndex].Cells[0].ReadOnly = false;
                }
                if (testName == "Others3")
                {
                    dataGridView2.Rows[rowIndex].Cells[0].ReadOnly = false;
                }
            }
        }
        BarcodePrintGateway _gtBarcode = new BarcodePrintGateway();
        private void button8_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Are you sure want to update patient details?", @"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }
            else
            {
                if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "BillNo='" + FourDigitTextBox.Text + "'"))
                {
                    _gt.DeleteInsertLab(@"INSERT INTO Update_Record_Of_Patient (BillNo, Name, DrName, Sex,  PostedBy)VALUES('" + FourDigitTextBox.Text + "', '" + lblPtName.Text + "', '" + lblDrName.Text  + "', '" + lblSex.Text  + "', '" + Hlp.UserName + "')");
                    _gt.DeleteInsertLab("UPDATE tb_BILL_MASTER SET PatientName='" + ptNameTextBox.Text + "',Sex='" + ptSexTextBox.Text + "',UnderDrId='"+ drNameComboBox.SelectedValue +"' WHERE BillNo='" + FourDigitTextBox.Text + "' ");
                    
                    MessageBox.Show(@"Update Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GetDataForInvoicePrint();
                    groupBox1.Hide();
                }
                else
                {
                    MessageBox.Show(@"Invalid InvoiceNo Or Date", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void FourDigitTextBox_TextChanged(object sender, EventArgs e)
        {
            if (FourDigitTextBox.TextLength != 10)
            {
                dataGridView1.Rows.Clear();
                dataGridView3.Rows.Clear();
                dataGridView2.Rows.Clear();
                commentsRichTextBox.Text = "";
            }
        }

     
        readonly DoctorGateway gt=new DoctorGateway();


        private string textInfo = "";

      

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

       

       

       

        private void button2_Click_1(object sender, EventArgs e)
        {
            GetDataForInvoicePrint();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _isClickSaveButton = true;
            if (dataGridView2.Rows.Count < 1)
            {
                MessageBox.Show(@"No Data Found To Save", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }



           

            Save(FourDigitTextBox.Text);

          
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            commentsRichTextBox.Text = "";
            _testCode = "";
            if (dataGridView3.SelectedCells.Count > 0)
            {
                _reportFileName = dataGridView3.SelectedCells[0].Value.ToString();
                GetDataByGroupName(_reportFileName,"");
            }
            //  CalculateDifferentialCount();

            if (dataGridView2.Rows.Count > 0)
            {

                dataGridView2.CurrentCell = dataGridView2.Rows[0].Cells[1];
                dataGridView2.BeginEdit(true);
                // dataGridView2.Rows[0].Cells[1].Selected = true;
            }
        }

       

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            


           

            int CellIndex = e.ColumnIndex;

            if (CellIndex==1)
            {
                //dataGridView2.Rows[Rowindex].Cells[CellIndex].Style.Font = new Font("Arial", 12, FontStyle.Bold);
                dataGridView2.Columns[1].DefaultCellStyle.Font =   new Font("Arial", 12, FontStyle.Bold);
            }


        }

       

        private void button3_Click(object sender, EventArgs e)
        {
            if (groupBox1.Visible)
            {
                groupBox1.Hide();
            }
            else {
                groupBox1.Show();
            }


        }

        private void ReportPrintUi_Load(object sender, EventArgs e)
        {
            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_LAB_DOCTOR WHERE Position='Left' Order By Id ", leftDrComboBox);
            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_LAB_DOCTOR WHERE Position='Middle' Order By Id ", middleDrComboBox);
            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_LAB_DOCTOR WHERE Position='Right' Order By Id ", rightDrComboBox);


            // GridWidth(dataGridView2);
            // invNoFirstPartTextBox.Text ="";//DateTime.Now.ToString("yyyy").Substring(2,2)+DateTime.Now.ToString("MM").PadLeft(2, '0');

            //ClearText();





            if (_gt.FnSeekRecordNewLab("tb_LAB_DOCTOR_DEFAULT", "PcName='" + System.Environment.MachineName + "'") == false)
            {
                _gt.DeleteInsertLab("INSERT INTO tb_LAB_DOCTOR_DEFAULT([Left], Middle, [Right], PcName)VALUES(0, 0, 0, '" + System.Environment.MachineName + "')");
            }



            leftDrComboBox.SelectedValue = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_LAB_DOCTOR_DEFAULT", "PcName='" + System.Environment.MachineName + "'", "[Left]"));
            middleDrComboBox.SelectedValue = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_LAB_DOCTOR_DEFAULT", "PcName='" + System.Environment.MachineName + "'", "[Middle]"));
            rightDrComboBox.SelectedValue = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_LAB_DOCTOR_DEFAULT", "PcName='" + System.Environment.MachineName + "'", "[Right]"));


            // autoPrintCheckBox.Checked = false;

            // if (Hlp.IsComeFromIndividualSearch)
            // {
            //     FourDigitTextBox.Text = Hlp.InvoiceNoComeFromIndividualSearch;
            //     GetDataForInvoicePrint();
            //     //FourDigitTextBox.SelectAll();
            // }


            label2.Text = Hlp.GroupName;

            int invNoDigit = Convert.ToString(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "1=1 Order by Id Desc", "TOP 1 BillNo")).Length;
            string firstTwoDigit = "";
            if (invNoDigit == 10)
            {
                firstTwoDigit = Convert.ToString(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "1=1 Order by Id Desc", "TOP 1 BillNo")).Substring(0, 2);
            }


            FourDigitTextBox.Text = firstTwoDigit + Hlp.GetServerDate().Year.ToString().Substring(2, 2) + Hlp.GetServerDate().Month.ToString().PadLeft(2, '0');
            FourDigitTextBox.Select(FourDigitTextBox.Text.Length, 0);

            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_DOCTOR Order By Id ", drNameComboBox);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

            DateTime invDate =Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_Bill_Master", "BillNo='" + FourDigitTextBox.Text + "'","BillDate"));
            //                        PtName = lblPtName.Text,
             //           PtAge = lblAge.Text ,
              //          PtSex = lblSex.Text,
                 //       DrName = lblDrName.Text ,

            _gt.DeleteInsertLab("DELETE FROM A_HeaderView");
            _gt.DeleteInsertLab(@"INSERT INTO [dbo].[A_HeaderView]([InvNo],[InvDate],[PtName],[Age],[Sex],[DrName])     VALUES
           ('" + FourDigitTextBox.Text + "','" + invDate.ToString("yyyy-MM-dd") + "','" + lblPtName.Text + "','" + lblAge.Text + "','" + lblSex.Text + "','" + lblDrName.Text + "')");



            LabReportquery = "SELECT * FROM A_HeaderView ";
            var gl = new LabReqViewer("Header", LabReportquery, "", "A_HeaderView", "LabReport");
            gl.ShowDialog();








        }


    }
}
