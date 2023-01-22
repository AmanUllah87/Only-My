using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
 
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using SuperPathologyApp.Report;
using SuperPathologyApp.Report.DataSet;

namespace SuperPathologyApp.UI
{
    public partial class ReportPrintMicrobiologyUi : Form
    {
        string _invNo = "";
        string _pCode = "";
        string _sampleNo = "";
        string reportNo = "";

        readonly ReportPrintMicrobiologyGateway _gt = new ReportPrintMicrobiologyGateway();
        public static string  LabReportquery = "";
        public static string LabReportFileName = "";
        private bool _isSaved=true;
        private string _reportFileName = "";
      
        List<TestCodeModel> testCodeList=new List<TestCodeModel>();
        private int _textInfo = 0;



        public ReportPrintMicrobiologyUi(int textInfo)
        {
            _textInfo = textInfo;
            InitializeComponent();
            FourDigitTextBox.Focus();
        }
        private void frmReportPrint_Load(object sender, EventArgs e)
        {
            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_DoctorSetup WHERE Type='Checked By'  Order By Id ", checkedByComboBox);
            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_DoctorSetup WHERE Type='Consultant'  Order By Id ", consultantNameComboBox);
            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_DoctorSetup WHERE Type='Lab Incharge'  Order By Id ", labInchargecomboBox);
          

         //   invNoFirstPartTextBox.Text = DateTime.Now.ToString("yyyy").Substring(2,2)+DateTime.Now.ToString("MM").PadLeft(2, '0');
            FourDigitTextBox.Focus();
        
           Gridwidth(1);
           _gt.GridColor(dataGridView4);
          // ClearText();

           organismTextBox.Text = _gt.FncReturnFielValueLab("tb_DefaultResultSetupCulture", "Parameter='Organism' AND Id=34", "Result");
           colonyCountTextBox.Text = _gt.FncReturnFielValueLab("tb_DefaultResultSetupCulture", "Parameter='Colony Count' AND Id=1", "Result");
           incubationTextBox.Text = _gt.FncReturnFielValueLab("tb_DefaultResultSetupCulture", "Parameter='Incubation' AND Id=12", "Result");
           specificTextBox.Text = _gt.FncReturnFielValueLab("tb_DefaultResultSetupCulture", "Parameter='Specific Test' AND Id=50", "Result");

            this.Location = Hlp.GetPoint();


            if (_gt.FnSeekRecordNewLab("tb_DefaultLabDoctorSetting","MachineName='" + System.Environment.MachineName + "'")==false)
            {
                _gt.DeleteInsertLab("INSERT INTO tb_DefaultLabDoctorSetting(CheckedBy, Consultant, LabInCharge, MachineName)VALUES(0, 0, 0, '" + System.Environment.MachineName + "')");
            }


            if (_gt.FnSeekRecordNewLab("tb_DefaultLabDoctorSetting", "MachineName='" + System.Environment.MachineName + "' AND CheckedBy<>0"))
            {
                checkedByComboBox.SelectedValue = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_DefaultLabDoctorSetting", "MachineName='" + System.Environment.MachineName + "'", "CheckedBy"));
            }
            else
            {
                checkedByComboBox.SelectedIndex = 0;
            }
            if (_gt.FnSeekRecordNewLab("tb_DefaultLabDoctorSetting", "MachineName='" + System.Environment.MachineName + "' AND Consultant<>0"))
            {
                consultantNameComboBox.SelectedValue = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_DefaultLabDoctorSetting", "MachineName='" + System.Environment.MachineName + "'", "Consultant"));
            }
            else
            {
                consultantNameComboBox.SelectedIndex = 0;
            }
            if (_gt.FnSeekRecordNewLab("tb_DefaultLabDoctorSetting","MachineName='" + System.Environment.MachineName + "' AND LabInCharge<>0"))
            {
                labInchargecomboBox.SelectedValue =Convert.ToInt32(_gt.FncReturnFielValueLab("tb_DefaultLabDoctorSetting","MachineName='" + System.Environment.MachineName + "'", "LabInCharge"));
            }
            else
            {
                labInchargecomboBox.SelectedIndex = 0;
            }

            autoPrintCheckBox.Checked = true;




            EditDataGridView2();

          



        }

        private void EditDataGridView2()
        {
            PopulateDrugGridView();
            dataGridView2.Columns[0].ReadOnly = true;
            dataGridView2.Columns[1].ReadOnly = false;
            dataGridView2.Columns[2].ReadOnly = false;


            //foreach (DataGridViewColumn dc in dataGridView2.Columns)
            //{
            //    dc.ReadOnly = !dc.Index.Equals(1);
            //}
            dataGridView2.Columns["zoneSize"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.Columns["enterpretion"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


        }

        private void ClearText()
        {

            string code = drugTextBox.Text;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                string gridCode = dataGridView2.Rows[i].Cells[0].Value.ToString();
                if (gridCode==code)
                {
                    MessageBox.Show(@"Duplicate Drug", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    drugTextBox.Focus();
                    return;
                }
            }
            dataGridView2.Rows.Add(drugTextBox.Text, zoneSizeTextBox.Text, enterpretationTextBox.Text);

           // enterpretationTextBox.Focus();
            drugTextBox.Focus();
            drugTextBox.Text = "";
            enterpretationTextBox.Text = "";
            zoneSizeTextBox.Text = "";
            _gt.GridColor(dataGridView2);
            dataGridView2.CurrentRow.Selected = false;
         
        }


        public void Gridwidth(int no)
        {
            switch (no)
            {
                case 1:
                    //dataGridView4.Columns[0].Width = 85;
                    //dataGridView4.Columns[1].Width = 75;
                    //dataGridView4.Columns[2].Width = 130;
                    //dataGridView4.Columns[3].Width = 60;
                    break;



            }
            //dataGridView4.RowHeadersVisible = false;
            //dataGridView4.CurrentCell = null;

            //dataGridView4.EnableHeadersVisualStyles = false;
            //dataGridView4.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            //dataGridView4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dataGridView4.AllowUserToResizeRows = false;
            
            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView2.AllowUserToResizeRows = false;
            
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView3.EnableHeadersVisualStyles = false;
            dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;

       


        }
   
        private void GetInvoiceDetailByInvNo()
        {
            testCodeList = _gt.GetInvoiceDetailByInvNo(masterId);

            if (testCodeList.Count > 0)
            {
                dataGridView3.Rows.Clear();
                dataGridView1.Rows.Clear();

                invDateDateTimePicker.Value = testCodeList[0].PtInvDate;
                patientNameTextBox.Text = testCodeList[0].PtName;
                contactNoTextBox.Text = testCodeList[0].PtTelephoneNo;
                ageTextBox.Text = testCodeList[0].PtAge;
                sexTextBox.Text = testCodeList[0].PtSex;
                drCodeTextBox.Text = testCodeList[0].DrCode;
                drNameTextBox.Text = testCodeList[0].DrName;

                var distinctItems = testCodeList.GroupBy(x => x.GroupName).Select(y => y.First());
                foreach (var codeModel in distinctItems)
                {
                    if (codeModel != null) dataGridView3.Rows.Add(codeModel.GroupName);
                }
                dataGridView3.Rows[0].Selected = false;
                _gt.GridColor(dataGridView3);
                foreach (var codeModel in testCodeList)
                {
                    dataGridView1.Rows.Add(codeModel.PCode, codeModel.ItemDesc, codeModel.PrintNo, codeModel.SampleNo, codeModel.GroupName);
                }
                dataGridView1.Rows.Add("", "", 0, "","");
                dataGridView1.Rows[0].Selected = false;
                _gt.GridColor(dataGridView1);














            }
            else {

                ClearText();
                dataGridView3.Rows.Clear();
                dataGridView1.Rows.Clear();
                        
            }
        }

    
  
   
      
   
        private void button1_Click(object sender, EventArgs e)
        {
            Hlp.AutoPrint = autoPrintCheckBox.Checked;
            if (dataGridView2.Rows.Count < 1)
            {
                return;
            }
            if (FourDigitTextBox.Text == "")
            {
                return;
            }
            string invNo = FourDigitTextBox.Text;
            if (_gt.FnSeekRecordNewLab("VW_Sample_Process_Tracking", "InvNo='" + invNo + "'") == false)
            {
                return;
            }
            if (_sampleNo == "")
            {
                return;
            }
            Save(invNo);

            if (_isSaved)
            {
                LabReportquery = "SELECT * FROM VW_LAB_REPORT_MICROBIOLOGY WHERE MasterId='" + masterId + "' AND Code='" + _pCode + "'";
                string fileName = _gt.FncReturnFielValueLab("VW_Sample_Process_Tracking", "ReportNo='" + reportNo + "'", "VaqGroup");
                fileName = "Microbiology_Growth";
                if (fileName == "")
                {
                    fileName = "Microbiology_Growth";
                }
                var dt = new FrmReportViewer(fileName, LabReportquery, "VW_LAB_REPORT_MICROBIOLOGY");
                dt.ShowDialog();
            }
            _pCode = "";
            _sampleNo = "";
            FourDigitTextBox.Focus();
            FourDigitTextBox.Select(5, 4);
            
            
        }

      
     



        private int masterId = 0;
        private void FourDigitTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _invNo =  FourDigitTextBox.Text;
                if (_gt.FnSeekRecordNewLab("tb_invmaster", "InvNo='" + _invNo + "'"))
                {
                    invDateDateTimePicker.Value =Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_invmaster", "InvNo='" + _invNo + "'", "InvDate"));
                }
                
                
                if (_gt.FnSeekRecordNewLab("tb_invmaster","InvNo='"+ _invNo +"' AND InvDate='"+ invDateDateTimePicker.Value.ToString("yyyy-MM-dd") +"'"))
                {
                    masterId =Convert.ToInt32(_gt.FncReturnFielValueLab("tb_invmaster","InvNo='" + _invNo + "' AND InvDate='" + invDateDateTimePicker.Value.ToString("yyyy-MM-dd") + "'", "Id"));
                }
                if (_gt.FnSeekRecordNewLab("tb_InvMaster", "InvNo='" + _invNo + "' AND InvDate='" + invDateDateTimePicker.Value.ToString("yyyy-MM-dd") + "'"))
                {
                    GetInvoiceDetailByInvNo();
                
                }
                else
                {
                    MessageBox.Show(@"No Data Found On This Id");
                    FourDigitTextBox.Focus();
                }
                _gt.GridColor(dataGridView1);

               
               
                dataGridView2.Rows.Clear();
                EditDataGridView2();
                organismTextBox.Focus();
            }
        }

     
      

      

   


        private void patientNameTextBox_Enter(object sender, EventArgs e)
        {
            patientNameTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void FourDigitTextBox_Enter(object sender, EventArgs e)
        {
            FourDigitTextBox.BackColor = DbConnection.EnterFocus();
            panelCultureDefaultResult.Visible = false;
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



        private SqlTransaction _trans;


        ReportPrintGateway rptNo = new ReportPrintGateway();
        private void Save(string invNo)
        {
            if (dataGridView2.Rows.Count == 0)
            {
                MessageBox.Show(@"No Drug Found For Save", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _isSaved = false;
                return;
            }
  
            if (patientNameTextBox.Text == @"")
            {
                MessageBox.Show(@"Invalid patient name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _isSaved = false;
                return;
            }
            if (drNameTextBox.Text == @"")
            {
                MessageBox.Show(@"Invalid doctor name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _isSaved = false;
                return;
            }

            var inv = new BarcodePrintGateway();

            try
            {

                //reportNo = GetLabNo();
                inv.ConLab.Open();
                _trans = inv.ConLab.BeginTransaction();
                reportNo = rptNo.GetLabNo(_trans, inv.ConLab);
                if (UpdateMasterDb(inv.ConLab))
                {
                    var mdl = new List<TestCodeModel>();
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        string result = dataGridView2.Rows[i].Cells[2].Value == null? "": dataGridView2.Rows[i].Cells[2].Value.ToString();
                        if (result.Trim() != "")
                        {
                            mdl.Add(new TestCodeModel()
                            {
                                ParameterTestName = dataGridView2.Rows[i].Cells[0].Value.ToString(),
                                ZoneSize = dataGridView2.Rows[i].Cells[1].Value.ToString(),
                                Enterpretation = dataGridView2.Rows[i].Cells[2].Value.ToString(),
                                PCode = _pCode,
                                MasterId = masterId,
                                Organism = organismTextBox.Text,
                                Colony = colonyCountTextBox.Text,
                                Incubation = incubationTextBox.Text,
                                SpecificTest = specificTextBox.Text,
                            });
                        }
                    }


                    _gt.Save(mdl, _trans, inv.ConLab);
                    _isSaved = true;

                    _trans.Commit();
                    inv.ConLab.Close();

                }
                else
                {
                    MessageBox.Show(@"Result Not Saved!!!", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _isSaved = false;
                }



                    _gt.DeleteInsertLab("Update tb_LabSampleStatusInfo SET   ReportprintStatus='Printed',ReportPrintUser='" + Hlp.UserName + "',ReportPrintTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',ReportNo='" + reportNo + "',FinalStatus='ReportPrint' WHERE MasterId='" + masterId + "'  AND SampleNo='" + _sampleNo + "'");
                //    _gt.DeleteInsertLab("Update tb_MASTER_INFO SET ReportNo=ReportNo+1");
                    _gt.DeleteInsertLab("Update tb_InvDetails SET PrintNo=PrintNo+1 WHERE MasterId='" + masterId + "'  AND Code='" + _pCode + "'");
                //

                
                    dataGridView1.Rows.Clear();
                    
                    //dataGridView3.Focus();
                    //dataGridView3.CurrentCell.Selected = true;
 

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

        private bool UpdateMasterDb(SqlConnection con)
        {

            _gt.DeleteInsertLab("DELETE FROM tb_MachineDataDtls_MicroMaster WHERE MasterId=" + masterId + "",_trans,con);
            _gt.DeleteInsertLab("Update tb_LabSampleStatusInfo SET RdeliveryDate='"+ deliveryDateTimePicker.Value.ToString("yyyy-MM-dd") +"' WHERE MasterId=" + masterId + " AND SampleNo='"+ _sampleNo +"' AND TestCode='"+ _pCode +"'", _trans, con);
            //string reportNo = GetLabNo();
            try
            {
                string biochemistName = "";
                string biochemistDegree = "";
                string consultantName = "";
                string consultantDegree = "";

                string labInchargeName = "";
                string labInchargeDegree = "";


                if (checkedByComboBox.Text != @"--Select--")
                {
                    biochemistName = checkedByComboBox.Text;
                    biochemistDegree = _gt.FncReturnFielValueLab("tb_DoctorSetup", "Id=" + checkedByComboBox.SelectedValue + "", "Details", _trans, con);
                }

                if (consultantNameComboBox.Text != @"--Select--")
                {
                    consultantName = consultantNameComboBox.Text;
                    consultantDegree = _gt.FncReturnFielValueLab("tb_DoctorSetup", "Id=" + consultantNameComboBox.SelectedValue + "", "Details", _trans, con);
                }
                if (labInchargecomboBox.Text != @"--Select--")
                {
                    labInchargeName = labInchargecomboBox.Text;
                    labInchargeDegree = _gt.FncReturnFielValueLab("tb_DoctorSetup", "Id=" + labInchargecomboBox.SelectedValue + "", "Details", _trans, con);
                }




                string query = "INSERT INTO tb_MachineDataDtls_MicroMaster (MasterId, LabNo, ReportNo, Code, Organism, ColonyCount, Incubation, SpecficTest, UserName,  BiochemistName, BiochemistDegree, ConsultantName, ConsultantDegree, Specimen,Comments,LabInchargeName,LabInchargeDegree,TestName) VALUES (@MasterId, @LabNo, @ReportNo, @Code, @Organism, @ColonyCount, @Incubation, @SpecficTest, @UserName,  @BiochemistName, @BiochemistDegree, @ConsultantName, @ConsultantDegree, @Specimen,@Comments,@LabInchargeName,@LabInchargeDegree,@TestName)";
                var cmd = new SqlCommand(query, con,_trans);
                cmd.Parameters.AddWithValue("@MasterId", masterId);
                cmd.Parameters.AddWithValue("@LabNo", _sampleNo);
                cmd.Parameters.AddWithValue("@ReportNo", reportNo);
                cmd.Parameters.AddWithValue("@Code", _pCode);
                cmd.Parameters.AddWithValue("@Organism", organismTextBox.Text);
                cmd.Parameters.AddWithValue("@ColonyCount", colonyCountTextBox.Text);
                cmd.Parameters.AddWithValue("@Incubation", incubationTextBox.Text);
                cmd.Parameters.AddWithValue("@SpecficTest", specificTextBox.Text);
                cmd.Parameters.AddWithValue("@UserName", Hlp.UserName);
                cmd.Parameters.AddWithValue("@BiochemistName", biochemistName);//, , , 
                cmd.Parameters.AddWithValue("@BiochemistDegree", biochemistDegree);
                cmd.Parameters.AddWithValue("@ConsultantName", consultantName);
                cmd.Parameters.AddWithValue("@ConsultantDegree", consultantDegree);
                cmd.Parameters.AddWithValue("@LabInchargeName", labInchargeName);
                cmd.Parameters.AddWithValue("@LabInchargeDegree", labInchargeDegree);

                cmd.Parameters.AddWithValue("@Comments", commentsTextBox.Text);
                cmd.Parameters.AddWithValue("@Specimen", specimenNewTextBox.Text);// );
                cmd.Parameters.AddWithValue("@TestName", _gt.FncReturnFielValueLab("tb_LabInvestigationChart", "Pcode='" + _pCode + "'", "ShortDesc", _trans, con));// );


                cmd.ExecuteNonQuery();
               

                return true;

            }
            catch (Exception )
            {

                return false;
            }
        }



        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            _gt.DeleteInsertLab("Update tb_DefaultLabDoctorSetting SET CheckedBy=" + checkedByComboBox.SelectedValue + " WHERE MachineName='" + System.Environment.MachineName + "' ");
            MessageBox.Show(@"Update success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            _gt.DeleteInsertLab("Update tb_DefaultLabDoctorSetting SET Consultant=" + consultantNameComboBox.SelectedValue + " WHERE MachineName='" + System.Environment.MachineName + "'");
            MessageBox.Show(@"Update success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

  

  



       

      
    
        private void button4_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            string invNo = FourDigitTextBox.Text;

            Save(invNo);
            FourDigitTextBox.Focus();
            FourDigitTextBox.Select(4, 3);
        }

        private void saveAndPrintButton_Click(object sender, EventArgs e)
        {
            Hlp.AutoPrint = autoPrintCheckBox.Checked;
            if (dataGridView2.Rows.Count<1)
            {
                return;
            }
            if (_pCode.Length<2)
            {
                MessageBox.Show(@"Please Select Test ", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (FourDigitTextBox.Text =="")
            {
                return;
            }
            string invNo = FourDigitTextBox.Text;
            if (_gt.FnSeekRecordNewLab("VW_Sample_Process_Tracking","InvNo='"+ invNo +"'")==false)
            {
                return;
            }
            if (_sampleNo == "")
            {
                return;
            }
            Save(invNo);
            
            if (_isSaved)
            {
                LabReportquery = "SELECT * FROM VW_LAB_REPORT_MICROBIOLOGY WHERE MasterId='" + masterId + "' AND Code='" + _pCode + "'";
                string fileName = _gt.FncReturnFielValueLab("VW_Sample_Process_Tracking", "ReportNo='" + reportNo + "'", "VaqGroup");
                fileName = "Microbiology_Growth";
                if (fileName=="")
                {
                    fileName = "Microbiology_Growth";
                }
                var dt = new FrmReportViewer(fileName, LabReportquery, "VW_LAB_REPORT_MICROBIOLOGY");
                dt.ShowDialog();
            }
            _pCode = "";
            _sampleNo = "";
            dataGridView2.Rows.Clear();
            EditDataGridView2();
            FourDigitTextBox.Focus();
            FourDigitTextBox.Select(5, 4);
            
        }

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

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
                    string invNo = dataGridView4.CurrentRow.Cells[0].Value.ToString();
                    DateTime invDate = Convert.ToDateTime(dataGridView4.CurrentRow.Cells[1].Value.ToString());
                    _gt.DeleteInsertLab("DELETE FROM tb_InvMaster WHERE InvNo='" + invNo + "' AND InvDate='" + invDate.ToString("yyyy-MM-dd") + "'");
                    _gt.DeleteInsertLab("DELETE FROM tb_InvDetails WHERE InvNo='" + invNo + "' AND InvDate='" + invDate.ToString("yyyy-MM-dd") + "'");
                    _gt.DeleteInsertLab("DELETE FROM tb_MachineDataDtls WHERE InvNo='" + invNo + "' AND InvDate='" + invDate.ToString("yyyy-MM-dd") + "'");
                }
            }
        }

      

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
            if (dataGridView3.SelectedCells.Count > 0)
            {
                dataGridView2.Rows.Clear();
                _reportFileName = dataGridView3.SelectedCells[0].Value.ToString();
                var testCode =(from mDl in testCodeList where mDl.GroupName == _reportFileName select new TestCodeModel(){TestCode = mDl.PCode,SampleNo = mDl.SampleNo,}).ToList();

                foreach (var mDl in testCode)
                {
                    GetParameterDataByCode(mDl.TestCode, mDl.SampleNo);
                }

            }

        }


       
        private void GetParameterDataByCode(string testCode,string sampleNo)
        {
            
                //_gt.GridColor(dataGridView1);
                //if (_gt.FnSeekRecordNewLab("VW_Get_Vaq_GroupName", "ItemId='" + testCode + "'"))
                //{
                //    _reportFileName = _gt.FncReturnFielValueLab("VW_Get_Vaq_GroupName", "ItemId='" + testCode + "'", "GroupName");
                //    //_testCode = testCode;
                //}
                //else
                //{
                //    MessageBox.Show(@"Please Set VaqGroup In This TestCode:"+testCode, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                //var data = _gt.GetDataFromParameterDefinitionByTestCode(testCode);
               

                //if (data.Count==0)
                //{
                //    MessageBox.Show(@"No Parameter Found In This TestCode:" + testCode, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                //foreach (var mdl in data)
                //{
                //    var savedData = _gt.GetDataFromMachineDataDtls(masterId, testCode, sampleNo, mdl.ParameterName);
                //    if (savedData.Result!="")
                //    {
                //        mdl.Result = savedData.Result;
                //    }
                //    dataGridView2.Rows.Add(mdl.ParameterTestName, mdl.Result, mdl.UnitName, mdl.NormalValue, mdl.TestName, mdl.SpecimenName, mdl.MachineName, mdl.HeaderName, mdl.GroupSlNo, mdl.ParameterSlNo, mdl.ParameterName, mdl.ReportingGroupName, mdl.IsBold, mdl.TestCode, _reportFileName, "rptNo", sampleNo);
                //}

               
                //_gt.GridColor(dataGridView2);
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView1.SelectedCells.Count > 0)
            {
                GetDataFromDb();
            }

        }

        private void GetDataFromDb()
        {
            _pCode = dataGridView1.SelectedCells[0].Value.ToString();
            _sampleNo = dataGridView1.SelectedCells[3].Value.ToString();
            deliveryDateTimePicker.Value = _gt.FnSeekRecordNewLab("tb_LabSampleStatusInfo", "MasterId=" + masterId + " AND TestCode='" + _pCode + "' AND SampleNo='" + _sampleNo + "' AND RdeliveryDate IS NOT NULL") ? Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_LabSampleStatusInfo", "MasterId=" + masterId + " AND TestCode='" + _pCode + "' AND SampleNo='" + _sampleNo + "'", "RdeliveryDate")) : DateTime.Now;
            specimenNewTextBox.Text = _gt.FnSeekRecordNewLab("tb_MachineDataDtls_MicroMaster", "Code='" + _pCode + "' AND LabNo='" + _sampleNo + "'") ? _gt.FncReturnFielValueLab("tb_MachineDataDtls_MicroMaster", "Code='" + _pCode + "' AND LabNo='" + _sampleNo + "'", "Specimen") : _gt.FncReturnFielValueLab("tb_Parameter_Definition", "TestCode='" + _pCode + "'", "Specimen");
            
           dataGridView2.Rows.Clear();

            if (_gt.FnSeekRecordNewLab("tb_MachineDataDtls_MicroDetail","MasterId="+ masterId +" AND PCode='"+ _pCode +"'"))
            {
                
                var dataMain = GetDataFromMachineDataDtls_MicroMasterByCode(_pCode, masterId);


                 var list=_gt.GetAllDrugName();
                dataGridView2.Rows.Clear();
                 foreach (var model in list)
                 {
                     var data = GetDataFromMachineDataDtls_MicroDetailByCode(_pCode, masterId,model.ParameterName);     
                     
                     dataGridView2.Rows.Add(model.ParameterName, data.ZoneSize, data.Enterpretation);
                 }
                 _gt.GridColor(dataGridView2);
                 dataGridView2.CurrentRow.Selected = false;
                 



      

                incubationTextBox.Text = dataMain.Incubation;
                specificTextBox.Text = dataMain.SpecificTest;
                colonyCountTextBox.Text = dataMain.Colony;
                organismTextBox.Text = dataMain.Organism;

            }

            drugTextBox.Focus();

        }

        private TestCodeModel GetDataFromMachineDataDtls_MicroMasterByCode(string pCode, int mstId)
        {
            var list = new TestCodeModel();
            string query = "SELECT Organism,ColonyCount,Incubation,SpecificTest FROM tb_MachineDataDtls_MicroDetail WHERE PCode='" + pCode + "' AND MasterId=" + mstId + "";
            _gt.ConLab.Open();
            var cmd = new SqlCommand(query, _gt.ConLab);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                list.Organism = rdr["Organism"].ToString();
                list.Colony = rdr["ColonyCount"].ToString();
                list.Incubation= rdr["Incubation"].ToString();
                list.SpecificTest = rdr["SpecificTest"].ToString();

            }
            _gt.ConLab.Close();
            return list;
        }

        private TestCodeModel GetDataFromMachineDataDtls_MicroDetailByCode(string pCode, int mstId,string drugName)
        {
            var list = new TestCodeModel();
            string query = "SELECT DrugName,ZoneSize,Enterpretation From tb_MachineDataDtls_MicroDetail WHERE MasterId=" + mstId + " AND PCode='" + pCode + "' AND DrugName='"+ drugName +"'";
            _gt.ConLab.Open();
            var cmd = new SqlCommand(query, _gt.ConLab);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.ParameterTestName = rdr["DrugName"].ToString();
                list.ZoneSize = rdr["ZoneSize"].ToString();
                list.Enterpretation = rdr["Enterpretation"].ToString();

            }
            _gt.ConLab.Close();
            return list;
        }


     











        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView1.SelectedCells.Count > 0)
            {
                GetDataFromDb();
            }


        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //GetParameterDataByCode();

            if (dataGridView1.SelectedCells.Count > 0)
            {
                specificTextBox.Text = "";
                colonyCountTextBox.Text = "";
                incubationTextBox.Text = "";
                organismTextBox.Text ="";
               GetDataFromDb();
            }


           

        }

        private void dataGridView3_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.KeyCode == Keys.Right)
            {
                dataGridView2.Focus();
                dataGridView2.CurrentRow.Selected = true;
                dataGridView2.SelectedCells[1].Selected = true;
            }

            if (e.KeyCode==Keys.Enter)
            {
                e.Handled = true;
                if (dataGridView3.SelectedCells.Count > 0)
                {
                    dataGridView2.Rows.Clear();
                    _reportFileName = dataGridView3.SelectedCells[0].Value.ToString();
                    var testCode = (from mDl in testCodeList where mDl.GroupName == _reportFileName select new TestCodeModel() { TestCode = mDl.PCode, SampleNo = mDl.SampleNo, }).ToList();
                    foreach (var mDl in testCode)
                    {
                        GetParameterDataByCode(mDl.TestCode, mDl.SampleNo);
                    }

                }
            }
        }

      

        private void organismTextBoxForGrid_Leave(object sender, EventArgs e)
        {
            organismTextBoxForGrid.BackColor = DbConnection.LeaveFocus();
            //panelCultureDefaultResult.Visible = false;
        }

        private void organismTextBoxForGrid_Enter(object sender, EventArgs e)
        {
            _textInfo = 1;
            organismTextBoxForGrid.BackColor = DbConnection.EnterFocus();
            PopulateGridViewByType("Organism","");
            panelCultureDefaultResult.Visible = true;
        }

        private void PopulateGridViewByType(string paramName,string searchString)
        {
            dataGridView5.DataSource = _gt.GetCultureDefaultParameter(paramName, searchString);
            dataGridView5.EnableHeadersVisualStyles = false;
            dataGridView5.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView5.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView5.AllowUserToResizeRows = false;
            dataGridView5.Columns[0].Width = 50;
            dataGridView5.Columns[1].Width = 400;
            _gt.GridColor(dataGridView5);

            if (dataGridView5.CurrentRow != null)
            {
                dataGridView5.CurrentRow.Selected = false;
            }
            
        }

        private void organismTextBoxForGrid_TextChanged(object sender, EventArgs e)
        {
            PopulateGridViewByType("Organism", organismTextBoxForGrid.Text);
        }

        private void organismTextBoxForGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Down)
            {
                dataGridView5.CurrentRow.Selected = true;
                dataGridView5.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView5.CurrentRow!=null)
                {
                    string rslt = dataGridView5.Rows[0].Cells[1].Value.ToString();
                    if (organismTextBox.Text == "") { organismTextBox.Text = rslt; } else { organismTextBox.Text += @"," + rslt; }
                    panelCultureDefaultResult.Hide();
                    colonyCountTextBox.Focus();
                }
                else
                {
                    colonyCountTextBox.Focus();
                }
            }


        }

        private void dataGridView5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                string rslt = dataGridView5.SelectedCells[1].Value.ToString();
                switch (_textInfo)
                {
                    case 1:
                        //if (organismTextBox.Text==""){organismTextBox.Text = rslt;}else{organismTextBox.Text += @"," + rslt;}
                        organismTextBox.Text = rslt;
                        panelCultureDefaultResult.Hide();
                        colonyCountTextBox.Focus();
                        break;
                    case 2:
                        colonyCountTextBox.Text = rslt; 
                        panelCultureDefaultResult.Hide();
                         dataGridView1.CurrentRow.Selected = true;
                        dataGridView1.Focus();

                        break;
                    case 3:
                        incubationTextBox.Text = rslt;
                        
                       // dataGridView1.Focus(); 

                        PopulateGridViewByType("Specific Test", "");
                        panelCultureDefaultResult.Hide();
                        dataGridView1.CurrentRow.Selected = true;
                        dataGridView1.Focus();


                        break;
                    case 4:
                        specificTextBox.Text = rslt;
                        drugTextBox.Focus();
                        panelCultureDefaultResult.Hide();
                        break;

                }
               
            }
        }

        private void colonyCountTextBox_Enter(object sender, EventArgs e)
        {
            
            _textInfo = 2;
            colonyCountTextBox.BackColor = DbConnection.EnterFocus();
            PopulateGridViewByType("Colony Count", "");
            panelCultureDefaultResult.Visible = true;

        }

        private void colonyCountTextBox_Leave(object sender, EventArgs e)
        {
            colonyCountTextBox.BackColor = DbConnection.LeaveFocus();
            //panelCultureDefaultResult.Visible = false;
        }

        private void colonyCountTextBox_TextChanged(object sender, EventArgs e)
        {
            PopulateGridViewByType("Colony Count", colonyCountTextBox.Text);
        }

        private void colonyCountTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                dataGridView5.CurrentRow.Selected = true;
                dataGridView5.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView5.CurrentRow != null)
                {
                    string rslt = dataGridView5.Rows[0].Cells[1].Value.ToString();
                    colonyCountTextBox.Text = rslt;
                    panelCultureDefaultResult.Hide();
                    //incubationTextBox.Focus();
                    dataGridView1.CurrentRow.Selected = true;
                    dataGridView1.Focus();

                }
                else
                {
                    colonyCountTextBox.Focus();
                }
            }

        }

        private void incubationTextBox_Enter(object sender, EventArgs e)
        {
            _textInfo = 3;
            incubationTextBox.BackColor = DbConnection.EnterFocus();
            PopulateGridViewByType("Incubation", "");
            panelCultureDefaultResult.Visible = true;



        }

        private void incubationTextBox_Leave(object sender, EventArgs e)
        {
            incubationTextBox.BackColor = DbConnection.LeaveFocus();
            //panelCultureDefaultResult.Visible = false;
        }

        private void incubationTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                dataGridView5.CurrentRow.Selected = true;
                dataGridView5.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView5.CurrentRow != null)
                {
                    string rslt = dataGridView5.Rows[0].Cells[1].Value.ToString();
                    incubationTextBox.Text = rslt;
                    panelCultureDefaultResult.Hide();
                    dataGridView1.CurrentRow.Selected = true;
                    dataGridView1.Focus();
                }
                else
                {
                    specificTextBox.Focus();
                }
            }
        }

        private void incubationTextBox_TextChanged(object sender, EventArgs e)
        {
            PopulateGridViewByType("Incubation", incubationTextBox.Text);
        }

        private void specificTextBox_Leave(object sender, EventArgs e)
        {
            specificTextBox.BackColor = DbConnection.LeaveFocus();
            //panelCultureDefaultResult.Visible = false;
        }

        private void specificTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                dataGridView5.CurrentRow.Selected = true;
                dataGridView5.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView5.CurrentRow != null)
                {
                    string rslt = dataGridView5.Rows[0].Cells[1].Value.ToString();
                    specificTextBox.Text = rslt;
                    panelCultureDefaultResult.Hide();
                    drugTextBox.Focus();
                }
                else
                {
                    drugTextBox.Focus();
                }
            }
        }

        private void specificTextBox_Enter(object sender, EventArgs e)
        {
            _textInfo = 4;
            specificTextBox.BackColor = DbConnection.EnterFocus();
            PopulateGridViewByType("Specific Test", "");
            panelCultureDefaultResult.Visible = true;
        }

        private void specificTextBox_TextChanged(object sender, EventArgs e)
        {
            PopulateGridViewByType("Specific Test", specificTextBox.Text );
        }

        private void drugTextBox_Enter(object sender, EventArgs e)
        {
            panelCultureDefaultResult.Visible = false;
            drugTextBox.BackColor = DbConnection.EnterFocus();
            PopulateDrugGridView();
        }

        private void drugTextBox_Leave(object sender, EventArgs e)
        {
            drugTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void drugTextBox_KeyDown(object sender, KeyEventArgs e)
        {
          
            if (e.KeyCode == Keys.Enter)
            {

                if (drugTextBox.Text=="")
                {
                    if (dataGridView6.CurrentRow != null)
                    {
                        string rslt = dataGridView6.Rows[0].Cells[2].Value.ToString();
                        idTextBox.Text = dataGridView6.Rows[0].Cells[0].Value.ToString();
                        drugTextBox.Text = rslt;
                        enterpretationTextBox.Focus();
                    }
                    else
                    {
                        drugTextBox.Focus();
                    }
                }
                else
                {
                    if (_gt.FnSeekRecordNewLab("tb_Parameter_Definition_Microbiology", "Id='" + drugTextBox.Text + "'"))
                    {
                        idTextBox.Text = drugTextBox.Text;
                        drugTextBox.Text = _gt.FncReturnFielValueLab("tb_Parameter_Definition_Microbiology", "Id=" + drugTextBox.Text + "", "Parameter");
                        //idTextBox.Text = drugTextBox.Text;
                        zoneSizeTextBox.Focus();
                    }
                    else
                    {

                        if (dataGridView6.Rows.Count==1)
                        {
                            enterpretationTextBox.Focus();
                            return;
                        }


                        string rslt = dataGridView6.Rows[0].Cells[2].Value.ToString();
                        idTextBox.Text = dataGridView6.Rows[0].Cells[0].Value.ToString();
                        drugTextBox.Text = rslt;
                        zoneSizeTextBox.Focus();
                    
                    }
                }
                
    
            }
        }

        private void zoneSizeTextBox_Leave(object sender, EventArgs e)
        {
            zoneSizeTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void zoneSizeTextBox_Enter(object sender, EventArgs e)
        {
            zoneSizeTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void enterpretationTextBox_Leave(object sender, EventArgs e)
        {
            enterpretationTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void enterpretationTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if ((enterpretationTextBox.Text == ""))
                {
                    enterpretationTextBox.Focus();
                    return;
                }
                ClearText();

            }
        }

        private void enterpretationTextBox_Enter(object sender, EventArgs e)
        {
            enterpretationTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void zoneSizeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (zoneSizeTextBox.Text != "")
                {
                    if (_gt.FnSeekRecordNewLab("tb_Parameter_Definition_Microbiology_Default_Result","MasterId='" + idTextBox.Text + "'"))
                    {
                        if (_gt.FnSeekRecordNewLab("tb_Parameter_Definition_Microbiology_Default_Result","MasterId='" + idTextBox.Text + "' AND '" + zoneSizeTextBox.Text + "' BETWEEN LowerVal AND UpperVal"))
                        {
                            enterpretationTextBox.Text = _gt.FncReturnFielValueLab("tb_Parameter_Definition_Microbiology_Default_Result","MasterId='" + idTextBox.Text + "' AND '" + zoneSizeTextBox.Text + "' BETWEEN LowerVal AND UpperVal", "Result");
                            // enterpretationTextBox.Focus();
                            ClearText();
                            return;
                        }
                    }
                }
                enterpretationTextBox.Focus();

                //enterpretationTextBox.Focus();
            }
        }

        private void drugTextBox_TextChanged(object sender, EventArgs e)
        {
            PopulateDrugGridView();
        }

        private void PopulateDrugGridView()
        {
            var data = _gt.GetAllDrugName();
            foreach (var list in data)
            {
                dataGridView2.Rows.Add(list.ParameterName, list.ShortName, "");
            }
            _gt.GridColor(dataGridView2);

         
            dataGridView2.CurrentCell.Selected = false;
            
        }

        private void dataGridView6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string rslt = dataGridView6.SelectedCells[2].Value.ToString();
                idTextBox.Text = dataGridView6.SelectedCells[0].Value.ToString();
                drugTextBox.Text = rslt;
                zoneSizeTextBox.Focus();
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            _gt.DeleteInsertLab("Update tb_DefaultLabDoctorSetting SET LabInCharge=" + labInchargecomboBox.SelectedValue + " WHERE MachineName='" + System.Environment.MachineName + "' ");
            MessageBox.Show(@"Update success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void organismTextBox_TextChanged(object sender, EventArgs e)
        {
            PopulateGridViewByType("Organism", organismTextBox.Text);
        }

        private void organismTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                dataGridView5.CurrentRow.Selected = true;
                dataGridView5.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView5.CurrentRow != null)
                {
                    string rslt = dataGridView5.Rows[0].Cells[1].Value.ToString();
                   // if (organismTextBox.Text == "") { organismTextBox.Text = rslt; } else { organismTextBox.Text += @"," + rslt; }
                    organismTextBox.Text = rslt;
                    
                    panelCultureDefaultResult.Hide();
                    colonyCountTextBox.Focus();
                }
                else
                {
                    colonyCountTextBox.Focus();
                }
            }
        }

        private void organismTextBox_Enter(object sender, EventArgs e)
        {
            _textInfo = 1;
            organismTextBoxForGrid.BackColor = DbConnection.EnterFocus();
            PopulateGridViewByType("Organism", "");
            panelCultureDefaultResult.Visible = true;
        }

        private void organismTextBox_Leave(object sender, EventArgs e)
        {
            organismTextBoxForGrid.BackColor = DbConnection.LeaveFocus();
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    int msId = 0;
            //    int rowIndex = dataGridView2.CurrentCell.RowIndex;
            //    string drugName = dataGridView2.Rows[rowIndex].Cells[0].Value.ToString();
            //    string result = dataGridView2.Rows[rowIndex].Cells[1].Value.ToString();

            //    msId = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_Parameter_Definition_Microbiology", "Parameter='" + drugName + "'", "Id"));
            //    string enterpre = "";
            //    if (msId!=0)
            //    {
            //        if (_gt.FnSeekRecordNewLab("tb_Parameter_Definition_Microbiology_Default_Result", "MasterId=" + msId + ""))
            //        {
            //            enterpre= _gt.FncReturnFielValueLab("tb_Parameter_Definition_Microbiology_Default_Result", "MasterId='" + msId + "' AND '" + result + "' BETWEEN LowerVal AND UpperVal", "Result");
            //            dataGridView2.Rows[rowIndex].Cells[2].Value = enterpre;
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //}
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
           

        }

        private void ReportPrintMicrobiologyUi_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode==Keys.F1)
            //{
            //    FourDigitTextBox.Select(5,2);
            //    FourDigitTextBox.Focus();
            //}
            if (e.KeyCode == Keys.F11)
            {
                saveAndPrintButton.PerformClick();
            }

        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Return)
            //{
            //    saveAndPrintButton.Select();
            //}
        }



        private void commentsTextBox_Enter(object sender, EventArgs e)
        {
            _textInfo = 10;
            commentsTextBox.BackColor = DbConnection.EnterFocus();
            PopulateGridViewByType("Microbiology", "");
            panelCultureDefaultResult.Visible = true;
        }

        private void dataGridView5_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_textInfo == 10)
            {
                commentsTextBox.Text = dataGridView5.SelectedCells[1].Value.ToString();
                panelCultureDefaultResult.Hide();
            }
        }

        
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                e.Handled = true;
                GetDataFromDb();
                dataGridView2.Rows[0].Cells[2].Selected = true;
                dataGridView2.Focus();
            }
        }

       


 

       



    }
}
