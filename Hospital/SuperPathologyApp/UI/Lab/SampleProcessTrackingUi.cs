using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Model;
using SuperPathologyApp.Report;
using SuperPathologyApp.Gateway.DB_Helper;

namespace SuperPathologyApp.UI
{
    public partial class SampleProcessTrackingUi : Form
    {
        readonly SampleSearchGateway _gt=new SampleSearchGateway();
        public static string  Query="";
        public SampleProcessTrackingUi()
        {
            InitializeComponent();
           
        }

        private void GridWidth(int status,DataGridView dg)
        {
            dg.EnableHeadersVisualStyles = false;
            //dg.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            //dg.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dg.AllowUserToResizeRows = false;
          //  dataGridView1.RowHeadersVisible = true;

            switch (status)
            {
                case 1:
                    dg.AutoGenerateColumns = false;
                  
                    dg.ColumnCount = 29;
                    dg.Columns[0].HeaderText = @"InvNo";
                    dg.Columns[1].HeaderText = @"InvDate";
                    dg.Columns[2].HeaderText = @"LabNo";
                    dg.Columns[3].HeaderText = @"PtName";
                    dg.Columns[4].HeaderText = @"Age";
                    dg.Columns[5].HeaderText = @"Sex";
                    dg.Columns[6].HeaderText = @"Description";

                    dg.Columns[7].HeaderText = @"Stricker Print";
                    dg.Columns[8].HeaderText = @"Stricker Print Time";
                    dg.Columns[9].HeaderText = @"User";

                    dg.Columns[10].HeaderText = @"Sample Collected";
                    dg.Columns[11].HeaderText = @"Sample Collection Time";
                    dg.Columns[12].HeaderText = @"User";

                    dg.Columns[13].HeaderText = @"S.R.InLab";
                    dg.Columns[14].HeaderText = @"S.R.InLab Time";
                    dg.Columns[15].HeaderText = @"User";

                    dg.Columns[16].HeaderText = @"Report Print";
                    dg.Columns[17].HeaderText = @"R.Print Time";
                    dg.Columns[18].HeaderText = @"User";

                    dg.Columns[19].HeaderText = @"Report.Process";
                    dg.Columns[20].HeaderText = @"R.Process Time";
                    dg.Columns[21].HeaderText = @"User";

                    dg.Columns[22].HeaderText = @"R.Receive.InDel.Counter";
                    dg.Columns[23].HeaderText = @"R.R In Delivery Time";
                    dg.Columns[24].HeaderText = @"User";

                    dg.Columns[25].HeaderText = @"R.Deliver";
                    dg.Columns[26].HeaderText = @"R.Deliver Time";
                    dg.Columns[27].HeaderText = @"User";
                    dg.Columns[28].HeaderText = @"FinalStatus";










                    dg.Columns[0].Width = 100;
                    dg.Columns[1].Width = 100;
                    dg.Columns[2].Width = 100;
                    dg.Columns[3].Width = 160;
                    dg.Columns[4].Width = 80;
                    dg.Columns[5].Width = 60;
                    dg.Columns[6].Width = 250;
                    dg.Columns[7].Width = 85;
                    dg.Columns[8].Width = 150;
                    dg.Columns[9].Width = 60;


                    dg.Columns[10].Width = 110;
                    dg.Columns[11].Width = 150;
                    dg.Columns[12].Width = 60;

                    dg.Columns[13].Width = 65;
                    dg.Columns[14].Width = 150;
                    dg.Columns[15].Width = 60;

                    dg.Columns[16].Width = 65;
                    dg.Columns[17].Width = 150;
                    dg.Columns[18].Width = 60;

                    dg.Columns[19].Width = 65;
                    dg.Columns[20].Width = 150;
                    dg.Columns[21].Width = 60;

                    dg.Columns[22].Width = 65;
                    dg.Columns[23].Width = 150;
                    dg.Columns[24].Width = 60;

                    dg.Columns[25].Width = 65;
                    dg.Columns[26].Width = 150;
                    dg.Columns[27].Width = 60;



                    dg.Columns[4].Visible =false;
                    dg.Columns[5].Visible = false;




                    break;

            }
            _gt.GridColor(dataGridView1);
        }
        private void SampleSearchUi_Load(object sender, EventArgs e)
        {
            //_gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_SampleStatusInfo Order By Id ", sampleStatusComboBox);
            //comboBox1.SelectedIndex = 0;
            GridWidth(1, dataGridView1);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void saveButton_Click(object sender, EventArgs e)
        {
            GetDetailsList();
            sampleNoTextBox.Focus();
            sampleNoTextBox.Select(4, 4);
        }
        private void GetDetailsList()
        {
            dataGridView1.Rows.Clear();
            GetReport();
         //   _gt.GridColor(dataGridView1);
            dataGridView1.CurrentCell = null;
            UpdateColumColor();
        }

        private void UpdateColumColor()
        {
            dataGridView1.Columns[7].DefaultCellStyle.BackColor = Color.Honeydew;
            dataGridView1.Columns[8].DefaultCellStyle.BackColor = Color.Honeydew;
            dataGridView1.Columns[9].DefaultCellStyle.BackColor = Color.Honeydew;

            dataGridView1.Columns[10].DefaultCellStyle.BackColor = Color.Khaki;
            dataGridView1.Columns[11].DefaultCellStyle.BackColor = Color.Khaki;
            dataGridView1.Columns[12].DefaultCellStyle.BackColor = Color.Khaki;

            dataGridView1.Columns[13].DefaultCellStyle.BackColor = Color.Honeydew;
            dataGridView1.Columns[14].DefaultCellStyle.BackColor = Color.Honeydew;
            dataGridView1.Columns[15].DefaultCellStyle.BackColor = Color.Honeydew;

            dataGridView1.Columns[16].DefaultCellStyle.BackColor = Color.Khaki;
            dataGridView1.Columns[17].DefaultCellStyle.BackColor = Color.Khaki;
            dataGridView1.Columns[18].DefaultCellStyle.BackColor = Color.Khaki;

            dataGridView1.Columns[19].DefaultCellStyle.BackColor = Color.Honeydew;
            dataGridView1.Columns[20].DefaultCellStyle.BackColor = Color.Honeydew;
            dataGridView1.Columns[21].DefaultCellStyle.BackColor = Color.Honeydew;

            dataGridView1.Columns[22].DefaultCellStyle.BackColor = Color.Khaki;
            dataGridView1.Columns[23].DefaultCellStyle.BackColor = Color.Khaki;
            dataGridView1.Columns[24].DefaultCellStyle.BackColor = Color.Khaki;

            dataGridView1.Columns[25].DefaultCellStyle.BackColor = Color.Honeydew;
            dataGridView1.Columns[26].DefaultCellStyle.BackColor = Color.Honeydew;
            dataGridView1.Columns[27].DefaultCellStyle.BackColor = Color.Honeydew;

        }



        private string GetReport()
        {
            string lcTimeFrom = "";
            string lcTimeTo = "";
            string lcDateFrom ="";
            string lcDateTo = "";


            string cond = "";

            if (sampleNoTextBox.Text != "")
            {
                cond += " AND LabNo like '%" + sampleNoTextBox.Text + "%'";
            }
            if (ptNameTextBox.Text != "")
            {
                cond += " AND ShortDesc like '%" + ptNameTextBox.Text + "%'";
            }
           
            
            
            if (invoiceNoTextBox.Text != "")
            {
                cond += " AND InvNo ='" + invoiceNoTextBox.Text + "'";
            }
            if (timeCheckBox.Checked)
            {
                lcTimeFrom = timeFrom.Value.ToString("HH:mm");
                lcTimeTo = timeTo.Value.ToString("HH:mm");
                lcDateFrom = dtDateFrom.Value.ToString("yyyy-MM-dd");
                lcDateTo = dtDateTo.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                cond += " AND InvDate Between '" + dtDateFrom.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateTo.Value.ToString("yyyy-MM-dd") + "'";
               // cond += " AND Convert(Date,CollTime) Between '" + dtDateFrom.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateTo.Value.ToString("yyyy-MM-dd") + "'";

            }
            #region
            if (timeCheckBox.Checked)
            {
                cond += " AND CollTime BETWEEN    '" + lcDateFrom + " " + lcTimeFrom + "' AND '" + lcDateTo + " " + lcTimeTo + "'";
            }

            Query = @"SELECT  InvNo, InvDate, LabNo, PatientName, Age, Sex, Pcode, ShortDesc, CollStatus, CollTime, CollUser, 
                SendStatus, SendTime, SendUser, 
                ReceiveInLabStatus, ReceiveInLabTime, ReceiveInLabUser, 
                ReportProcessStatus, ReportProcessTime, ReportProcessUser, 
                ReportPrintStatus, ReportPrintTime, ReportPrintUser, 
                RReceiveInDelCounterStatus, RReceiveInDelCounterTime, RReceiveInDelCounterUser, 
                DeliverToPatientStatus, DeliverToPatientTime, DeliverToPatientUser, 
                FinalStatus, '' AS PtStatus,MasterId FROM V_Sample_Status_Info WHERE 1=1 " + cond + " Order by LabNo";
            var list = new List<TestCodeModel>();
            _gt.ConLab.Open();
            var cmd = new SqlCommand(Query, _gt.ConLab);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new TestCodeModel()
                {
                    PtInvNo = rdr["InvNo"].ToString(),
                    PtInvDate = Convert.ToDateTime(rdr["InvDate"].ToString()),
                    LabNo = rdr["LabNo"].ToString(),
                    PtName = rdr["PatientName"].ToString(),
                    PtAge = rdr["Age"].ToString(),
                    PtSex = rdr["Sex"].ToString(),
                    ItemDesc = rdr["ShortDesc"].ToString(),

                    SampleCollectionStatus = rdr["CollStatus"].ToString(),
                    SampleCollectionUserName = rdr["CollUser"].ToString(),
                    SampleCollectionTime = rdr["CollTime"].ToString(),

                    SampleSendStatus = rdr["SendStatus"].ToString(),
                    SampleSendTime = rdr["SendTime"].ToString(),
                    SampleSendUserName = rdr["SendUser"].ToString(),

                    SampleReceiveInLabStatus = rdr["ReceiveInLabStatus"].ToString(),
                    SampleReceiveInLabUserName = rdr["ReceiveInLabUser"].ToString(),
                    SampleReceiveInLabStatusTime = rdr["ReceiveInLabTime"].ToString(),

                    SampleReportProcessStatus = rdr["ReportProcessStatus"].ToString(),
                    SampleReportProcessUserName = rdr["ReportProcessUser"].ToString(),
                    SampleReportProcessTime = rdr["ReportProcessTime"].ToString(),

                    ReportPrintStatus = rdr["ReportPrintStatus"].ToString(),
                    ReportPrintUserName = rdr["ReportPrintUser"].ToString(),
                    ReportPrintTime = rdr["ReportPrintTime"].ToString(),

                    ReportReceiveInDeliveryCounterStatus = rdr["RReceiveInDelCounterStatus"].ToString(),
                    ReportReceiveInDeliveryCounterUserName = rdr["RReceiveInDelCounterUser"].ToString(),
                    ReportReceiveInDeliveryCounterTime = rdr["RReceiveInDelCounterTime"].ToString(),

                    DeliverToPatientStatus = rdr["DeliverToPatientStatus"].ToString(),
                    DeliverToPatientUserName = rdr["DeliverToPatientUser"].ToString(),
                    DeliverToPatientTime = rdr["DeliverToPatientTime"].ToString(),

                    PtType = rdr["PtStatus"].ToString(),
                    FinalStatus = rdr["FinalStatus"].ToString(),

                });
            }
            rdr.Close();
            _gt.ConLab.Close();
            int l = 0;
            foreach (var model in list)
            {
                if (model.SampleSendStatus=="Send")
                {
                    model.SampleSendStatus = "Sample Collected";
                }
                
                
                dataGridView1.Rows.Add(model.PtInvNo, model.PtInvDate.ToString("yyyy-MM-dd"), model.LabNo, model.PtName, model.PtAge, model.PtSex, model.ItemDesc, model.SampleCollectionStatus, model.SampleCollectionTime, model.SampleCollectionUserName, model.SampleSendStatus, model.SampleSendTime, model.SampleSendUserName, model.SampleReceiveInLabStatus, model.SampleReceiveInLabStatusTime, model.SampleReceiveInLabUserName, model.ReportPrintStatus, model.ReportPrintTime, model.ReportPrintUserName, model.SampleReportProcessStatus, model.SampleReportProcessTime, model.SampleReportProcessUserName, model.ReportReceiveInDeliveryCounterStatus, model.ReportReceiveInDeliveryCounterTime, model.ReportReceiveInDeliveryCounterUserName, model.DeliverToPatientStatus, model.DeliverToPatientTime, model.DeliverToPatientUserName, model.FinalStatus);
                
                
                if (model.FinalStatus=="Cancel")
                {
                    dataGridView1.Rows[l].DefaultCellStyle.BackColor = Color.Coral;                    
                }
                l++;
            }
            #endregion
            return "";
        }

    
        private void button1_Click(object sender, EventArgs e)
        {

            _gt.DeleteInsertLab("DELETE FROM A_TMP_Sample_Process_Tracking");
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    _gt.DeleteInsertLab("INSERT INTO A_TMP_Sample_Process_Tracking (InvNo, InvDate, LabNo, PatientName, Age, Sex, ShortDesc, Type, CollStatus, CollTime, UserName) VALUES('" + dataGridView1.Rows[i].Cells[0].Value + "', '" + dataGridView1.Rows[i].Cells[1].Value + "', '" + dataGridView1.Rows[i].Cells[2].Value + "', '" + dataGridView1.Rows[i].Cells[3].Value + "', '" + dataGridView1.Rows[i].Cells[4].Value + "', '" + dataGridView1.Rows[i].Cells[5].Value + "', '" + dataGridView1.Rows[i].Cells[6].Value + "', '" + sampleStatusComboBox.Text + "', '" + dataGridView1.Rows[i].Cells[7].Value + "', '" + dataGridView1.Rows[i].Cells[8].Value + "', '" + dataGridView1.Rows[i].Cells[9].Value + "')");
            //}


            //Query = "SELECT * FROM A_TMP_Sample_Process_Tracking";
            //var dt = new AddiotionalReportViewer("SampleProcessInfo", "Sample Process Tracking (" + sampleStatusComboBox.Text + ")", "Reporting date from " + dtDateFrom.Value.ToString("yyyy-MM-dd") + " AND " + dtDateTo.Value.ToString("yyyy-MM-dd"));
            //dt.ShowDialog();

        }

        private void sampleNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                GetDetailsList();
                sampleNoTextBox.Focus();
                sampleNoTextBox.Select(4, 4);
            }
        }

        private void invoiceNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                GetDetailsList();
                sampleNoTextBox.Select(5, 4);
            }


        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == 0)
            //{
            //    dataGridView1.CurrentCell.Selected = true;
            //    dataGridView1.CurrentCell.Style.BackColor = Color.Gold;
            //}
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridView1.CurrentCell.Selected)
            //{
            //    Hlp.AutoPrint = false;
            //    string invNo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            //    string query = "SELECT * FROM VW_InvoicePrint WHERE InvNo='" + invNo + "'";
            //    string reportFileName = "LabReqView";
            //    string dataSetName = "VW_InvoicePrint";
            //    string dateRange = "";
            //    var dt = new LabReqViewer(reportFileName, query, dateRange, dataSetName);
            //    dt.ShowDialog();

            //}
        }
    }
}
