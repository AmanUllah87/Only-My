using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Model;
using SuperPathologyApp.Report.DataSet;
using SuperPathologyApp.Report.File;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Image = System.Drawing.Image;


namespace SuperPathologyApp.UI
{
    public partial class BarcodePrintUi : Form
    {
        public BarcodePrintUi()
        {
            InitializeComponent();
            invoiceNoTextBox.Focus();

        }
        BarcodePrintGateway _gt=new BarcodePrintGateway();
      

        private void label3_Click(object sender, EventArgs e)
        {
        }
        private void frmBarcodePrint_Load(object sender, EventArgs e)
        {
            
            GridWidth(dataGridView1);
          //  int a = dataGridView2.Rows.Count;
          //  DoctorStrickerheckBox.Checked = true;
          //  Location = Hlp.GetPoint();
          
            invoiceNoTextBox.Focus();
        }
        private void GridWidth(DataGridView dataGridView1)
        {
            dataGridView1.Columns[0].Width = 100;
            dataGridView1.Columns[1].Width = 400;
           
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 60;
           
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.CurrentCell = null;



            //dataGridView2.Columns[0].Width = 100;
            //dataGridView2.Columns[1].Width = 156;
            //dataGridView2.Columns[2].Width = 80;
           
           
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.CurrentCell = null;



            //dataGridView1.EnableHeadersVisualStyles = false;
            //dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            //dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dataGridView1.AllowUserToResizeRows = false;

            //dataGridView2.EnableHeadersVisualStyles = false;
            //dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            //dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dataGridView2.AllowUserToResizeRows = false;

            //dataGridView3.EnableHeadersVisualStyles = false;
            //dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            //dataGridView3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dataGridView3.AllowUserToResizeRows = false;


            
            foreach (DataGridViewColumn dc in dataGridView1.Columns)
            {
                dc.ReadOnly = !dc.Index.Equals(1);
            }
           
           
           // dataGridView1.Columns[4].ReadOnly = false;
           
        }

        private void ClearText()
        {
            ptNameTextBox.Text = "";
            ptAgeTextBox.Text = "";
            drNameTextBox.Text = "";
            ptGenderTextBox.Text = "";
            ptMobileNoTextBox.Text = "";
            bedNoTextBox.Text = "";
            invNoTextBox.Text = @"0";

        }


        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            
        }
        private void invoiceNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                
                dataGridView2.Rows.Clear();
                dataGridView1.Rows.Clear();
                lblMessage.Text = "";
                int masterId = 0;

                if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "BillNo='" + invoiceNoTextBox.Text + "'"))
                {
                    invDateDateTimePicker.Value = Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + invoiceNoTextBox.Text + "'", "BillDate"));
                    masterId = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + invoiceNoTextBox.Text + "' AND BillDate='" + invDateDateTimePicker.Value.ToString("yyyy-MM-dd") + "'", "Id"));
                    TransferDataFromOtherDbToLabDbByInvNoAndDate(masterId);
                   // invNoTextBox
                }
                else
                {
                    MessageBox.Show(@"Invalid Invoice No. Please Check", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; 
                }
              
                GetInvoiceDetailByInvNo(masterId);
                invNoTextBox.Text = masterId.ToString();
                if (dataGridView1.Rows.Count>0)
                {
                    dataGridView1.CurrentCell.Selected = false;     
                }
                          

                button1.Enabled = true;
                button1.Focus();

            }

         


        }
        public string TransferDataFromOtherDbToLabDbByInvNoAndDate(int masterId)
        {
            return _gt.TransferDataFromOtherDbToLabDbByInvNoAndDate(masterId);
        }


        private void PopulateCollectedSampleGrid(int masterId)
        {
            var dataForSampleCollected = _gt.GetDataFromLabSmapleInvestigation(masterId);
            dataGridView1.Rows.Clear();
            if (dataForSampleCollected.Count > 0)
            {
                foreach (var codeModel in dataForSampleCollected)
                {
                    dataGridView1.Rows.Add(codeModel.SampleNo, codeModel.TestName,codeModel.SampleCollectionStatus, true);
                    //if (codeModel.TestName.ToUpper().Contains("CBC"))
                    //{
                    //    dataGridView1.Rows.Add(codeModel.SampleNo, "ESR",codeModel.SampleCollectionStatus, true);
                    //  //  dataGridView1.Rows.Add(codeModel.SampleNo, "SLIDE-01", true);
                    //   // dataGridView1.Rows.Add(codeModel.SampleNo, "SLIDE-02", true);
                    //}
                }
                dataGridView1.Rows[0].Selected = false;
            }
            _gt.GridColor(dataGridView1);
        }

      

        private void GetInvoiceDetailByInvNo(int masterId)
        {
            var ptHeaddata = _gt.GetPatientDetails(masterId);
            
            lblPtName.Text = ptHeaddata.PtName;
            lblMobileNo.Text = ptHeaddata.PtMobileNo;
            lblAge.Text = ptHeaddata.PtAge;
            lblSex.Text = ptHeaddata.PtSex;
            lblMobileNo.Text = ptHeaddata.PtMobileNo;
            lblDate.Text = ptHeaddata.PtInvDate.ToString("dd-MMM-yyyy");
            lblDrName.Text = ptHeaddata.DrName;
            PopulateCollectedSampleGrid(masterId);
        }
   
   

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null ? true : (!(bool)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value));
            }
            if ((e.ColumnIndex==0)||(e.ColumnIndex==1))
            {
                var gtPartialSample = GetPartialCollectionInfo(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                dataGridView2.Rows.Clear();

                if (gtPartialSample.Count > 1)
                {
                    foreach (var mDl in gtPartialSample)
                    {
                        bool isCancel=true;
                        if (mDl.IsPrint==0)
                        {
                            isCancel = false;
                        }
                        
                        dataGridView2.Rows.Add(mDl.TestCode, mDl.TestName, isCancel);
                    }
                    _gt.GridColor(dataGridView2);
                    dataGridView2.CurrentCell.Selected = false;
                }    
            }



        }
        private void button1_Click(object sender, EventArgs e)
        {
            refInvNotextBox.Text = invoiceNoTextBox.Text;
            invoiceNoTextBox.Focus();
           // invoiceNoTextBox.Select();
            invoiceNoTextBox.Text = "";
            
            
            if ((invNoTextBox.Text==@"0")||(invNoTextBox.Text==""))
            {
                MessageBox.Show(@"Invalid InvoiceNo Please Check", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                invoiceNoTextBox.Focus();
                return;
            }
           
            var mdl = new List<TestCodeModel>();


            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int checkedValue =Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
                string sampleNo = dataGridView1.Rows[i].Cells[0].Value.ToString();
                string testName = dataGridView1.Rows[i].Cells[1].Value.ToString();
                if (checkedValue==1)
                {
                    mdl.Add(new TestCodeModel()
                    {
                        SampleNo = sampleNo,
                        TestName = testName,
                    });     
                }
            }

          

            foreach (var testCodeModel in mdl)
            {
                if (testCodeModel.SampleNo.Length > 8)
                {
                    _gt.DeleteInsertLab("Update tb_LAB_STRICKER_INFO SET CollStatus='Collected',SampleNo='" + testCodeModel.SampleNo + "',CollTime='" + Hlp.GetServerDate().ToString("yyyy-MM-dd HH:mm:ss") + "',CollUser='" + Hlp.UserName + "',SendStatus='Send',SendTime='" + Hlp.GetServerDate().ToString("yyyy-MM-dd HH:mm:ss") + "',SendUser='" + Hlp.UserName + "',FinalStatus='Sample Send' Where SampleNo='" + testCodeModel.SampleNo + "' AND MasterId=" + invNoTextBox.Text + " AND CollStatus='Pending'");
                }
                PrintBarcode128BySampleNo(testCodeModel.SampleNo, testCodeModel.TestName);
            }


            if (DoctorStrickerheckBox.Checked)
            {
                PrintBarcode128BySampleNo(mdl[0].SampleNo.Substring(0, 8), "Lab-Copy");
            }
           
            invNoTextBox.Text = @"0";
            lblMessage.Text = "";
            msgBarcodePrint.Text = "";
           // dataGridView1.Rows.Clear();

            invoiceNoTextBox.Focus();
            //invoiceNoTextBox.Select();
            invoiceNoTextBox.Text = "";

          

        }

  

        public byte[] ImageToByteArray(Image imageIn)
        {
            var ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

       
        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

      

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[3].Value=false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[4].Value = true;
            }

        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show(@"Do you want to cancel this collection?", @"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
                else
                {
                    string labNo = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                    string collTime = dataGridView2.CurrentRow.Cells[3].Value.ToString();

                    if (_gt.FnSeekRecordNewLab("tb_LabSampleStatusInfo","SampleNo='"+ labNo +"'"))
                    {
                        _gt.DeleteInsertLab("UPDATE tb_LabSampleStatusInfo SET CollStatus='Pending',CollTime=null,CollUser='Pending',SendUser='Pending',SendTime=null,SendStatus='Pending' WHERE SampleNo='" + labNo + "' AND ReceiveInLabSTatus='Pending'");
                        MessageBox.Show(@"Sample collection cancel success", @"Information",MessageBoxButtons.OK, MessageBoxIcon.Information);
                        PopulateCollectedSampleGrid(Convert.ToInt32(invNoTextBox.Text));
                      
                    }
                    else
                    {
                        MessageBox.Show(@"Invalid sampleNo or time", @"Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                }
            }

            if (e.KeyCode == Keys.F10)
            {
                if (_gt.FnSeekRecordNewLab("tb_InvMaster","Id="+ invNoTextBox.Text +""))
                {
                    string labNo = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                    PrintBarcode128BySampleNo(labNo.Substring(0, 8), drNameTextBox.Text);
                }

            }





        }

        private List<TestCodeModel> GetPartialCollectionInfo(string sampleNo)
        {
            var lists = new List<TestCodeModel>();
            try
            {
                string query = @"SELECT a.TestId As TestCode,b.Name AS TestName,a.Valid FROM tb_BILL_DETAIL a LEFT JOIN tb_TESTCHART b ON a.TestId=b.Id WHERE MasterId='" + invNoTextBox.Text + "' AND a.SampleNo='" + sampleNo + "' ";
                
                _gt.ConLab.Open();
                var cmd = new SqlCommand(query, _gt.ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lists.Add(new TestCodeModel()
                    {
                        TestCode = rdr["TestCode"].ToString(),
                        TestName = rdr["TestName"].ToString(),
                        IsPrint = Convert.ToInt32(rdr["Valid"]),
                    });
                }
                rdr.Close();
                _gt.ConLab.Close();
                return lists;
            }
            catch (Exception exception)
            {
                if (_gt.ConLab.State == ConnectionState.Open)
                {
                    _gt.ConLab.Close();
                }
                MessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            return new List<TestCodeModel>();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();
            panel1.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count<1)
            {
                MessageBox.Show(@"Invalid operation?", @"Information",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show(@"Do you want to cancel all collection?", @"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }
            else
            {

                int masterId = 0;
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    string labNo = dataGridView2.Rows[i].Cells[0].Value.ToString();
                    string collTime = dataGridView2.Rows[i].Cells[3].Value.ToString();
                    if (_gt.FnSeekRecordNewLab("tb_LabSampleStatusInfo", "SampleNo='" + labNo + "'"))
                    {
                        masterId =Convert.ToInt32(_gt.FncReturnFielValueLab("tb_LabSampleStatusInfo", "SampleNo='" + labNo + "'","MasterId"));
                        _gt.DeleteInsertLab("UPDATE tb_LabSampleStatusInfo SET CollStatus='Pending',CollTime=null,CollUser='Pending',SendUser='Pending',SendTime=null,SendStatus='Pending' WHERE SampleNo='" + labNo + "' AND ReceiveInLabSTatus='Pending'");
                    }
                }
                MessageBox.Show(@"Sample collection cancel success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PopulateCollectedSampleGrid(Convert.ToInt32(masterId));
               
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count < 1)
            {
                MessageBox.Show(@"Invalid operation?", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show(@"Do you want to reprint all collection?", @"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }
            else
            {
                string barcodeId = "";
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    barcodeId = dataGridView2.Rows[i].Cells[0].Value.ToString();
                    string testName = dataGridView2.Rows[i].Cells[1].Value.ToString();
                    PrintBarcode128BySampleNo(barcodeId, testName);

                    //Image myimg = Code128Rendering.MakeBarcodeImage(barcodeId, 1, true);
                    //pictBarcode.Image = myimg;
                    //GenerateBarcode(barcodeId, testName);
                }


                if (DoctorStrickerheckBox.Checked)
                {
                    PrintBarcode128BySampleNo(barcodeId.Substring(0, 8), drNameTextBox.Text);
                    PrintBarcode128BySampleNo(barcodeId.Substring(0, 8), drNameTextBox.Text);
                    PrintBarcode128BySampleNo(barcodeId.Substring(0, 8), drNameTextBox.Text);

                }
                invoiceNoTextBox.Text = "";
                invoiceNoTextBox.Focus();
            }
        }

        private void invoiceNoTextBox_Enter(object sender, EventArgs e)
        {
            invoiceNoTextBox.BackColor = DbConnection.EnterFocus();
            

        }

        private void invoiceNoTextBox_Leave(object sender, EventArgs e)
        {
            invoiceNoTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show(@"Do you want to cancel this collection?", @"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
                else
                {
                    string labNo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    string desc = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    if (_gt.FnSeekRecordNewLab("tb_LabSampleStatusInfo", "SampleNo='" + labNo + "'"))
                    {
                        
                        //_gt.DeleteInsertLab("DELETE FROM tb_LabSampleStatusInfo WHERE SampleNo='" + labNo + "'");
                        _gt.DeleteInsertLab("UPDATE tb_LabSampleStatusInfo SET CollStatus='Pending',CollTime='',CollUser='Pending',SendStatus='Pending',SendTime='',SendUser='Pending',FinalStatus='Cancel',EntryDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',ReceiveInLabUser='"+ Hlp.UserName +"' WHERE SampleNo='" + labNo + "' AND MasterId='" + invNoTextBox.Text + "' AND ReceiveInLabStatus='Pending'");
                        _gt.DeleteInsertLab("DELETE FROM tb_Invdetails WHERE LabNo='" + labNo + "' AND MasterId='" + invNoTextBox.Text + "'");

                        //_gt.DeleteInsertLab("Update tb_LabSampleStatusInfo SET CollStatus='Collected',SampleNo='" + testCodeModel.SampleNo + "',CollTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',CollUser='" + Hlp.UserName + "',FinalStatus='Sample Collected' Where SampleNo='" + testCodeModel.SampleNo + "' AND MasterId=" + invNoTextBox.Text + " AND CollStatus='Pending'");


                        _gt.DeleteInsertLab("INSERT INTO Del_Record_Of_Sample(InvNo, InvDate, SampleNo, Description, UserName) VALUES ('" + invoiceNoTextBox.Text + "', '" + invDateDateTimePicker.Value.ToString("yyyy-MM-dd") + "', '" + labNo + "', '" + desc + "', '" + Hlp.UserName + "')");
                        MessageBox.Show(@"Sample cancel success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        PopulateCollectedSampleGrid(Convert.ToInt32(invNoTextBox.Text));
                    }
                    else
                    {
                        MessageBox.Show(@"Invalid SampleNo", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

      

        private void button8_Click(object sender, EventArgs e)
        {

            var mdl = new List<TestCodeModel>();
            string query = "";
            query = @"SELECT PCode,ShortDesc,SubSubDeptName FROM InvestigationChart WHERE PCode <>''";
            _gt.ConOther.Open();
            var cmd = new SqlCommand(query, _gt.ConOther);
            cmd.CommandTimeout = 60;
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {

                mdl.Add(new TestCodeModel()
                {
                    TestCode = rdr["PCode"].ToString(),
                    TestName = rdr["ShortDesc"].ToString(),
                    ChildDesc = rdr["SubSubDeptName"].ToString(),
                });
            }
            rdr.Close();
            _gt.ConOther.Close();


            foreach (var model in mdl)
            {
                if (_gt.FnSeekRecordNewLab("tb_LabInvestigationChart", "Pcode='" + model.TestCode + "'")==false)
                {
                    _gt.DeleteInsertLab("INSERT INTO tb_LabInvestigationChart(PCode,ShortDesc,SubSubDeptName)VALUES('" + model.TestCode + "','" + model.TestName + "','" + model.ChildDesc + "')");
                }

            }
            MessageBox.Show("Updated");


        }

        private void button9_Click(object sender, EventArgs e)
        {

            if (invoiceNoTextBox.Text=="")
            {
                return;
            }
            
            
            if (MessageBox.Show(@"Do you want to cancel this invoice?", @"Information", MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }
            else if (_gt.FnSeekRecordNewLab("tb_LAB_STRICKER_INFO", "MasterId='" + Convert.ToInt32(invNoTextBox.Text) + "' AND ReceiveInLabStatus='Collected'"))
            {
                MessageBox.Show(@"This Invoice Has Sample That Already Receive In Lab ", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "Id='" + Convert.ToInt32(invNoTextBox.Text) + "'"))
                {
                   // _gt.DeleteInsertLab("DELETE FROM tb_BILL_MASTER WHERE Id='" + Convert.ToInt32(invNoTextBox.Text) + "'");
                   // _gt.DeleteInsertLab("DELETE FROM tb_BILL_DETAIL WHERE MasterId='" + Convert.ToInt32(invNoTextBox.Text) + "'");
                    _gt.DeleteInsertLab("DELETE FROM tb_LAB_STRICKER_INFO WHERE MasterId='" + Convert.ToInt32(invNoTextBox.Text) + "'");
                  //  _gt.DeleteInsertLab("INSERT INTO Del_Record_Of_Sample(InvNo, InvDate, SampleNo, Description, UserName,HostName) VALUES ('" + invNoTextBox.Text + "', '" + invDateDateTimePicker.Value.ToString("yyyy-MM-dd") + "', 'smplNo', '" + invoiceNoTextBox.Text  + "', '" + Hlp.UserName + "', '" + System.Environment.MachineName + "')");

                    MessageBox.Show(@"Invoice Cancel Success.", @"Information", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    dataGridView1.Rows.Clear();
                    invoiceNoTextBox.Text = "";
                    return;
                }
                else
                {
                    MessageBox.Show(@"Invalid Invoice Please Check.", @"Warning", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
            } 
        }

        private void button10_Click(object sender, EventArgs e)
        {

          




            var list = new List<TestCodeModel>();
            const string query = @"SELECT distinct UserName,PassWord FROM Password";
            _gt.ConOther.Open();
            var cmd = new SqlCommand(query, _gt.ConOther);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new TestCodeModel()
                {
                    UserName = rdr["UserName"].ToString(),
                    DrName  = rdr["PassWord"].ToString(),
                });
            }
            rdr.Close();
            _gt.ConOther.Close();

            foreach (var test in list)
            {
                _gt.DeleteInsertLab("INSERT INTO tb_UserAccess (UserName,Password,ParentName,ChildName ,AuthorizedBy) SELECT '" + test.UserName + "','" + test.DrName + "',ParentName,ChildName ,'1' FROM tb_UserAccess WHERE UserName='Rossi'");
            }
            
            
            
            
            foreach (var test in list)
            {
                _gt.DeleteInsertLab("INSERT INTO tb_UserAccess_Groupwise (UserName,GroupName) SELECT '" + test.UserName + "',GroupName FROM tb_UserAccess_Groupwise WHERE UserName='Rossi'");
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                var list = new List<TestCodeModel>();
                string query = @"SELECT Pcode FROM tb_LabInvestigationChart";
                _gt.ConOther.Open();
                var cmd = new SqlCommand(query, _gt.ConLab);
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new TestCodeModel()
                    {
                        TestCode = rdr["Pcode"].ToString(),
                        //DrName = rdr["Password"].ToString(),

                    });
                }
                rdr.Close();
                _gt.ConOther.Close();

                foreach (var test in list)
                {
                    string subsubDeptName = _gt.FncReturnFielValueOther("InvestigationChart","Pcode='" + test.TestCode + "'", "SubSubDeptName");
                    _gt.DeleteInsertLab("UPDATE tb_LabInvestigationChart SET SubSubDeptName='"+ subsubDeptName +"' WHERE Pcode='"+ test.TestCode +"'");
                }










            }
            catch (Exception )
            {
                if (_gt.ConOther.State == ConnectionState.Open)
                {
                    _gt.ConOther.Close();
                }
                throw;
            }
            













    
            
        }

        static int CountNonSpaceChars(string value)
        {
            int result = 0;
            foreach (char c in value)
            {
               
                    result++;
            }
            return result;
        }

        private void PrintBarcode128BySampleNo(string sampleNo, string testName)
        {

            
            msgBarcodePrint.Text =testName;

            if (CountNonSpaceChars(testName)>75)
            {
                testName = testName.Substring(0,75);
            }

            var dataSet = new DataSet();
            var reportDocument = new ReportDocument();
            var dataTable = new DataTable();
            dataTable.Columns.Add("InvNo", typeof(string));
            dataTable.Columns.Add("InvDate", typeof(DateTime));
            dataTable.Columns.Add("BarCode", typeof(string));
            dataTable.Columns.Add("PtName", typeof(string));
            dataTable.Columns.Add("CollDateString", typeof(string));
            dataTable.Columns.Add("PtSex", typeof(string));
            dataTable.Columns.Add("TestName", typeof(string));
            dataTable.Columns.Add("CollTime", typeof(string));
            dataTable.Columns.Add("UserName", typeof(string));
            dataTable.Columns.Add("BedNo", typeof(string));
            dataTable.Columns.Add("PtType", typeof(string));
            dataTable.Columns.Add("SampleNo", typeof(string));
            dataTable.Columns.Add("HostName", typeof(string));


            dataTable.Rows.Clear();
            DataRow row = dataTable.NewRow();
            row[0] = refInvNotextBox.Text;
            row[1] = invDateDateTimePicker.Value;
            row[2] = (object)this.Code128B((object)sampleNo);

            DateTime collTime = Hlp.GetServerDate();
            string name = lblPtName.Text + "-" + lblSex.Text.Substring(0, 1) + "-" + lblAge.Text;
            string dateString = collTime.ToString("dd/MM/yy") + "-" + collTime.ToString("h:mm:ss tt");


            row[3] = name;
            row[4] = dateString;
            row[5] = lblSex.Text;
            row[6] = testName;
            row[7] = collTime;
            row[8] = Hlp.UserName;
            row[9] = bedNoTextBox.Text;

            //string ptStaus = _gt.FncReturnFielValueLab("tb_InvMaster", "Id=" + invNoTextBox.Text + "", "PtStatus");
            //ptStaus = ptStaus == "Indoor" ? "IPD" : "OPD";

            row[10] = "";
            row[11] = sampleNo;
            row[12] = Environment.MachineName;

            dataTable.Rows.Add(row);
            string path = Application.StartupPath;
            path = path + @"\Report\File\Lab\BarcodeNew.rpt";
            reportDocument.Load(path);
            reportDocument.SetDataSource(dataTable);
            reportDocument.PrintToPrinter(1, true, 0, 0);
            reportDocument.Dispose();
            reportDocument.Close();

        }

        public bool IsInt(string num)
        {
            try
            {
                int a = Convert.ToInt32(num);
                return true;
            }
            catch (Exception)
            {

                return   false;
            }
        }

        public string Code128B(object rawData)
        {

            

            object obj2;
            object obj3;
            object obj4;
            object obj5 = null;
            object obj6 = null;
            string str = null;
            object obj7 = null;
            object obj8 = null;
            object obj9 = null;
            object obj10 = null;
            object obj11 = null;
            object obj12 = null;
            object obj13 = null;
            object obj14 = null;
            object obj15 = null;
            object obj16 = null;
            object obj17 = null;
            object obj18 = null;
            object obj19 = null;
            object obj20 = null;
            object obj21 = null;
            int num;
            int num2;
            object[] objArray;
            object[] objArray2;
            bool[] flagArray;
            obj14 = (int)0x20;
            obj10 = (int)0x12;
            obj19 = (int)0x68;
            obj21 = (int)0;
            if (Operators.ConditionalCompareObjectEqual(obj21, (int)0, false))
            {
                obj20 = "š";
                obj12 = "›";
                obj9 = "œ";
                obj13 = "–";
                obj18 = "•";
                obj16 = "€";
            }
            else if (Operators.ConditionalCompareObjectEqual(obj21, (int)1, false))
            {
                obj20 = "\x00cc";
                obj12 = "\x00cd";
                obj9 = "\x00ce";
                obj13 = "\x00c9";
                obj18 = "\x00c8";
                obj16 = "€";
            }
        TR_002A:
            num2 = Strings.Len(rawData);
            num = 1;
        TR_0029:
            while (num <= num2)
            {
                obj5 = Operators.AddObject(obj5, (int)1);
                if (num == 1)
                {
                    if (!this.IsInt(Strings.Mid(Conversions.ToString(rawData), num, 1)))
                    {
                        obj17 = obj20;
                    }
                    else if (!this.IsInt(Strings.Mid(Conversions.ToString(rawData), num + 1, 1)))
                    {
                        obj17 = obj20;
                    }
                    else
                    {
                        obj17 = obj12;
                        obj15 = (int)1;
                        obj19 = Operators.AddObject(obj19, (int)1);
                    }
                }
            TR_0020:
                if (!this.IsInt(Strings.Mid(Conversions.ToString(rawData), num, 1)))
                {
                    if (Operators.ConditionalCompareObjectEqual(obj15, (int)1, false))
                    {
                        obj7 = Operators.ConcatenateObject(obj7, obj13);
                        obj15 = (int)0;
                        objArray2 = new object[] { obj13 };
                        objArray = objArray2;
                        flagArray = new bool[] {true };
                        if (flagArray[0])
                        {
                            obj13 = objArray[0];
                        }
                    TR_001B:
                        obj19 = Operators.AddObject(obj19, Operators.MultiplyObject(Operators.SubtractObject(Operators.SubtractObject(NewLateBinding.LateGet(null, typeof(Strings), "Asc", objArray, null, null, flagArray), obj14), obj10), obj5));
                        obj5 = Operators.AddObject(obj5, (int)1);
                    }
                TR_0019:
                    obj3 = Strings.Mid(Conversions.ToString(rawData), num, 1);
                    objArray2 = new object[] { obj3 };
                    objArray = objArray2;
                    flagArray = new bool[] { true };
                    if (flagArray[0])
                    {
                        obj3 = objArray[0];
                    }
                TR_0017:
                    obj6 = Operators.MultiplyObject(Operators.SubtractObject(NewLateBinding.LateGet(null, typeof(Strings), "Asc", objArray, null, null, flagArray), obj14), obj5);
                }
                else if (this.IsInt(Strings.Mid(Conversions.ToString(rawData), num + 1, 1)))
                {
                    if (Operators.ConditionalCompareObjectEqual(obj15, (int)0, false))
                    {
                        obj7 = Operators.ConcatenateObject(obj7, obj18);
                        obj15 = (int)1;
                        objArray = new object[] { obj18 };
                        objArray2 = objArray;
                        flagArray = new bool[] { true };
                        if (flagArray[0])
                        {
                            obj18 = objArray2[0];
                        }
                    TR_0008:
                        obj19 = Operators.AddObject(obj19, Operators.MultiplyObject(Operators.SubtractObject(Operators.SubtractObject(NewLateBinding.LateGet(null, typeof(Strings), "Asc", objArray2, null, null, flagArray), obj14), obj10), obj5));
                        obj5 = Operators.AddObject(obj5, (int)1);
                    }
                TR_0007:
                    obj8 = Strings.Mid(Conversions.ToString(rawData), num, 1) + Strings.Mid(Conversions.ToString(rawData), num + 1, 1);
                    obj3 = !Operators.ConditionalCompareObjectGreaterEqual(Operators.AddObject(obj14, obj8), (int)0x7f, false) ? ((char)Strings.Chr(Conversions.ToInteger(Operators.AddObject(obj14, obj8)))) : ((char)Strings.Chr(Conversions.ToInteger(Operators.AddObject(Operators.AddObject(obj14, obj8), obj10))));
                    num += 1;
                    obj6 = Operators.MultiplyObject(Operators.SubtractObject(Operators.AddObject(obj14, obj8), obj14), obj5);
                }
                else
                {
                    if (Operators.ConditionalCompareObjectEqual(obj15, (int)1, false))
                    {
                        obj7 = Operators.ConcatenateObject(obj7, obj13);
                        obj15 = (int)0;
                        objArray2 = new object[] { obj13 };
                        objArray = objArray2;
                        flagArray = new bool[] { true };
                        if (flagArray[0])
                        {
                            obj13 = objArray[0];
                        }
                    TR_0011:
                        obj19 = Operators.AddObject(obj19, Operators.MultiplyObject(Operators.SubtractObject(Operators.SubtractObject(NewLateBinding.LateGet(null, typeof(Strings), "Asc", objArray, null, null, flagArray), obj14), obj10), obj5));
                        obj5 = Operators.AddObject(obj5, (int)1);
                    }
                TR_000F:
                    obj3 = Strings.Mid(Conversions.ToString(rawData), num, 1);
                    objArray2 = new object[] { obj3 };
                    objArray = objArray2;
                    flagArray = new bool[] { true };
                    if (flagArray[0])
                    {
                        obj3 = objArray[0];
                    }
                TR_000D:
                    obj6 = Operators.MultiplyObject(Operators.SubtractObject(NewLateBinding.LateGet(null, typeof(Strings), "Asc", objArray, null, null, flagArray), obj14), obj5);
                }
            TR_0005:
                objArray2 = new object[] { obj3 };
                objArray = objArray2;
                flagArray = new bool[] { true };
                if (flagArray[0])
                {
                    obj3 = objArray[0];
                }
            TR_0003:
                if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(null, typeof(Strings), "Asc", objArray, null, null, flagArray), (int)0x20, false))
                {
                    obj3 = obj16;
                }
            TR_0001:
                obj7 = Operators.ConcatenateObject(obj7, obj3);
                obj19 = Operators.AddObject(obj19, obj6);
                num += 1;
            }
            obj4 = Operators.ModObject(obj19, (int)0x67);
            obj11 = !Operators.ConditionalCompareObjectGreaterEqual(Operators.AddObject(obj4, obj14), (int)0x7f, false) ? Operators.AddObject(obj4, obj14) : Operators.AddObject(Operators.AddObject(obj4, obj14), obj10);
            obj6 = (Operators.CompareString(Conversions.ToString(Strings.Chr(Conversions.ToInteger(obj11))), " ", false) == 0) ? obj16 : ((char)Strings.Chr(Conversions.ToInteger(obj11)));
            return Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(obj17, obj7), obj6), obj9));
        }

        private void lblMessage_Click(object sender, EventArgs e)
        {

        }

      

        private void button12_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count>0)
            {
                string barcodeId = dataGridView1.Rows[0].Cells[0].Value.ToString();
                PrintBarcode128BySampleNo(barcodeId.Substring(0, 8), drNameTextBox.Text);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //var gtPartialSample = GetPartialCollectionInfo(dataGridView2.CurrentRow.Cells[0].Value.ToString());


            //if (gtPartialSample.Count > 1)
            //{
            //   // dataGridView1.CurrentRow.Cells[4].Value = false;
            //    dataGridView3.Rows.Clear();

            //    panel1.Visible = true;
            //    foreach (var mDl in gtPartialSample)
            //    {
            //        string labNo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            //        dataGridView3.Rows.Add(mDl.TestCode, mDl.TestName, true);
            //    }

            //    _gt.GridColor(dataGridView3);

            //}
            //else
            //{
            //    panel1.Visible = false;
            //}
        }

        private void button13_Click(object sender, EventArgs e)
        {

            if (dataGridView2.CurrentCell.Selected)
            {
                var gtPartialSample = GetPartialCollectionInfo(dataGridView2.CurrentRow.Cells[0].Value.ToString());
                if (gtPartialSample.Count > 1)
                {
                    collGridPrintButton.Visible = true;
                    panel1.Visible = true;
                    foreach (var mDl in gtPartialSample)
                    {
                        dataGridView3.Rows.Add(dataGridView2.CurrentRow.Cells[0].Value.ToString(), mDl.TestName, true);
                    }
                    _gt.GridColor(dataGridView3);
                }
                else
                {
                    panel1.Visible = false;
                }
            }

        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count < 1)
            {
                return;
            }
            
            
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                int checkedValue = Convert.ToInt32(dataGridView2.Rows[i].Cells[2].Value);
                if (checkedValue==0)
                {
                    _gt.DeleteInsertLab("Update tb_BILL_DETAIL SET Valid=0 WHERE MasterId=" + invNoTextBox.Text + " AND TestId='" + dataGridView2.Rows[i].Cells[0].Value + "'");
                }
            }
            invoiceNoTextBox.Focus();
            GetInvoiceDetailByInvNo(Convert.ToInt32(invNoTextBox.Text));
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = (dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null ? true : (!(bool)dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value));
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count < 1)
            {
                return;
            }

            int maxNo = Convert.ToInt32(Hlp.GetNumberByCharacter(_gt.FncReturnFielValueLab("tb_LAB_STRICKER_INFO", "MasterId=" + invNoTextBox.Text + "", "Isnull(MAX(Right(SampleNo,1)),1)")));
            maxNo += 1;
            string invNo = invoiceNoTextBox.Text; //_gt.FncReturnFielValueLab("tb_LabSampleStatusInfo", "MasterId=" + invNoTextBox.Text + "", "LEFT(SampleNo,8)");
            string labNo = invNo + Hlp.GetCharacterByNo(maxNo);
            string testName = "";
                      
            
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                int checkedValue = Convert.ToInt32(dataGridView2.Rows[i].Cells[2].Value);
                if (checkedValue == 0)
                {
                    _gt.DeleteInsertLab("Update tb_BILL_DETAIL SET Valid=1,SampleNo='" + labNo + "' WHERE MasterId=" + invNoTextBox.Text + " AND TestId='" + dataGridView2.Rows[i].Cells[0].Value + "'");
                    _gt.DeleteInsertLab("Update tb_LAB_STRICKER_INFO SET SampleNo='" + labNo + "', CollStatus='Collected',CollTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',CollUser='" + Hlp.UserName + "',SendStatus='Send',SendTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',SendUser='" + Hlp.UserName + "',FinalStatus='Sample Send' WHERE MasterId=" + invNoTextBox.Text + " AND TestId='" + dataGridView2.Rows[i].Cells[0].Value + "'");
                    
                    if (testName=="")
                    {
                        testName = dataGridView2.Rows[i].Cells[1].Value.ToString();
                    }
                    else
                    {
                        testName += "," + dataGridView2.Rows[i].Cells[1].Value;
                    }
                    //_gt.DeleteInsertLab("Update tb_MASTER_INFO SET OpdSampleNo=OpdSampleNo+1");
                }
            }
            if (testName!="")
            {
                PrintBarcode128BySampleNo(labNo, testName);    
            }
            invoiceNoTextBox.Text = "";
           // invoiceNoTextBox.Select(4,3);
            invoiceNoTextBox.Focus();
            GetInvoiceDetailByInvNo(Convert.ToInt32(invNoTextBox.Text));

        }

        private void button16_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[3].Value = true;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void invoiceNoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
           

        }

        private void invDateDateTimePicker_Enter(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count>0)
            {
                button1.Focus();
            }
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count<1)
            {
                return;
            }
            
            
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                int checkedValue = Convert.ToInt32(dataGridView2.Rows[i].Cells[2].Value);
                if (checkedValue == 1)
                {
                    _gt.DeleteInsertLab("Update tb_invdetails SET Valid=1 WHERE MasterId=" + invNoTextBox.Text + " AND Code='" + dataGridView2.Rows[i].Cells[0].Value + "'");
                }
            }
            invoiceNoTextBox.Focus();
            GetInvoiceDetailByInvNo(Convert.ToInt32(invNoTextBox.Text));
        }

        private void button18_Click(object sender, EventArgs e)
        {
           
            
            if (dataGridView1.Rows.Count > 0)
            {
                string barcodeId = dataGridView1.Rows[0].Cells[0].Value.ToString();
                //PrintBarcode128BySampleNo(barcodeId.Substring(0, 8), drNameTextBox.Text);
                PrintBarcode128BySampleNo(barcodeId.Substring(0, 8), "Lab-Copy");
            }
        }

        private void BarcodePrintUi_Activated(object sender, EventArgs e)
        {
            invoiceNoTextBox.Focus();
           // invoiceNoTextBox.Select(4,3);

        }

        private void invoiceNoTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
