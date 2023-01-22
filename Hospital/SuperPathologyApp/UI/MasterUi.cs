using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using SuperPathologyApp.Gateway.DB_Helper;

namespace SuperPathologyApp.UI
{
    public partial class MasterUi : Form
    {
        public MasterUi()
        {
            InitializeComponent();
        }
        
       
        private void button3_Click(object sender, EventArgs e)
        {


            if (passwordTextBox.Text.ToUpper()!="ALLAHHU@12345")
            {
                MessageBox.Show(@"Invalid Password.PLease Check.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }




            _gt.DeleteInsertLab("UPDATE tb_MASTER_INFO SET ComName='" + hospitalNameTextBox.Text + "', Address='" + addressTextBox.Text + "', ReportNo='" + reportNoTextBox.Text + "', StrickerPrint='" + strickerPrintCheckBox.Checked + "', GroupwiseBill='" + groupwiseBillCheckBox.Checked + "', UnderDrComision='" + underDrComCheckBox.Checked + "', UseExpenseEntry='" + expenseCheckBox.Checked + "', UseIndoor='" + indoorModulecheckBox.Checked + "'");




            



            var rawdata1Encrypt =Hlp.Rc4(valiDateDateTimePicker.Value.ToString("yyyy-MM-dd"), hospitalNameTextBox.Text.Trim());
           




            var rawdata2Encrypt = Hlp.Encrypt(valiDateDateTimePicker.Value.ToString("yyyy-MM-dd"), hospitalNameTextBox.Text.Trim());





           // _gt.DeleteInsertLab("UPDATE tb_VALIDATION SET RawData1='" + rawdata1Encrypt + "', RawData2='" + rawdata2Encrypt + "'");


            _gt.ConLab.Open();
            
            string query = "";
            query = "UPDATE tb_VALIDATION SET RawData1=@RawData1, RawData2=@RawData2 ";
            var cmd = new SqlCommand(query,_gt.ConLab);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@RawData1", rawdata1Encrypt);
            cmd.Parameters.AddWithValue("@RawData2", rawdata2Encrypt);
            cmd.ExecuteNonQuery();
            _gt.ConLab.Close();
            
            
     
            
            
            
            MessageBox.Show(@"Update Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            Environment.Exit(0);
        }

        readonly DbConnection _gt=new DbConnection();
       


        private void userNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (hospitalNameTextBox.Text!="")
                {
                    addressTextBox.Focus();
                }
                else
                {
                    hospitalNameTextBox.Focus();
                }
            }
        }

        private void userNameTextBox_Enter(object sender, EventArgs e)
        {
            hospitalNameTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void passwordTextBox_Leave(object sender, EventArgs e)
        {
            hospitalNameTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void passwordTextBox_Enter(object sender, EventArgs e)
        {
            addressTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void userNameTextBox_Leave(object sender, EventArgs e)
        {
            hospitalNameTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (hospitalNameTextBox.Text == "")
                {
                    hospitalNameTextBox.Focus();
                    return;
                }

                else
                {
                    if (addressTextBox.Text == "")
                    {
                        addressTextBox.Focus();
                        return;
                    }
                    else
                    {
                        button3.Focus();
                    }
                }
            }
        }

        private void Login_Activated(object sender, EventArgs e)
        {
            hospitalNameTextBox.Focus();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MasterUi_Load(object sender, EventArgs e)
        {
           

            try
            {
                expenseCheckBox.Checked = Convert.ToBoolean(_gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "UseExpenseEntry"));
                
                hospitalNameTextBox.Text = _gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ComName");
                addressTextBox.Text = _gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "Address");
                reportNoTextBox.Text = _gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ReportNo");
                strickerPrintCheckBox.Checked = Convert.ToBoolean(_gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "StrickerPrint"));
                groupwiseBillCheckBox.Checked = Convert.ToBoolean(_gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "GroupwiseBill"));
                underDrComCheckBox.Checked = Convert.ToBoolean(_gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "UnderDrComision"));
                indoorModulecheckBox.Checked = Convert.ToBoolean(_gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "UseIndoor"));

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
               
            }
           


            try
            {
                string rawdata1Encrypt = _gt.FncReturnFielValueLab("tb_VALIDATION", "1=1", "RawData1");
                var rawdata1Decrypt = Hlp.Rc4(rawdata1Encrypt, _gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ComName"));
                valiDateDateTimePicker.Value = Convert.ToDateTime(rawdata1Decrypt);

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

        private void button1_Click(object sender, EventArgs e)
        {
            _gt.DeleteInsertLab(@"ALTER PROC [dbo].[Sp_Get_InvoicePrint] (@invNo int,@groupName varchar(500))   AS
IF	LEN(@groupName)=0
BEGIN
SELECT a.Id AS MasterId,a.BillNo,a.BillDate,a.BillTime, a.PatientName,a.MobileNo,a.Address,a.Age,a.Sex,c.Name AS RefDr,d.Name As ConsDr,a.TotalAmt,a.LessAmt,a.CollAmt,a.LessFrom ,a.LessType,a.LessPc, a.PostedBy,
b.Charge,e.Name AS TestName,a.Remarks,(SELECT Isnull(SUM(TotalAmt),0) FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueColl,(SELECT Isnull(SUM(LessAmt),0) FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLess,
(Select Top 1 LessFrom FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLessFrom,(Select Top 1 LessPc FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLessPc,(Select Top 1 LessType FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLessType,
a.RefDrId,a.UnderDrId,b.TestId,b.HnrAmt,b.TotHnrAmt,b.DrLess,CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END  AS SubProjectName,a.DeliveryDate,a.DeliveryNumber,a.DeliveryTimeAmPm,b.GridRemarks,a.LastPrintPc
FROM tb_BILL_MASTER a LEFT JOIN tb_BILL_DETAIL b ON a.Id=b.MasterId
LEFT JOIN tb_Doctor c ON a.RefDrId=c.Id
LEFT JOIN tb_Doctor d ON a.UnderDrId=d.Id
LEFT JOIN tb_TestChart e ON b.TestId=e.Id 
WHERE a.Id=@invNo
END
IF	LEN(@groupName)>0
BEGIN
SELECT a.Id AS MasterId,a.BillNo,a.BillDate,a.BillTime, a.PatientName,a.MobileNo,a.Address,a.Age,a.Sex,c.Name AS RefDr,d.Name As ConsDr,a.TotalAmt,a.LessAmt,a.CollAmt,a.LessFrom ,a.LessType,a.LessPc, a.PostedBy,b.Charge,e.Name AS TestName,a.Remarks
,(SELECT Isnull(SUM(TotalAmt),0) FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueColl,(SELECT Isnull(SUM(LessAmt),0) FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLess,
(Select Top 1 LessFrom FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLessFrom,(Select Top 1 LessPc FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLessPc,(Select Top 1 LessType FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLessType,
a.RefDrId,a.UnderDrId,b.TestId,b.HnrAmt,b.TotHnrAmt,b.DrLess,CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END  AS SubProjectName,a.DeliveryDate,a.DeliveryNumber,a.DeliveryTimeAmPm,b.GridRemarks,a.LastPrintPc
FROM tb_BILL_MASTER a LEFT JOIN tb_BILL_DETAIL b ON a.Id=b.MasterId
LEFT JOIN tb_Doctor c ON a.RefDrId=c.Id
LEFT JOIN tb_Doctor d ON a.UnderDrId=d.Id
LEFT JOIN tb_TestChart e ON b.TestId=e.Id 
WHERE a.Id=@invNo AND CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END = @groupName
END");
            MessageBox.Show(@"Success");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
             _gt.DeleteInsertLab(@"ALTER TABLE tb_BILL_DETAIL ADD SampleNo Nvarchar(50) NOT NULL DEFAULT ''");
             MessageBox.Show("Success");
        
            }
            catch (Exception)
            {
                ;
            }
            try
            {
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_LAB_STRICKER_INFO](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MasterId] [int] NOT NULL,
	[InvNo] [nvarchar](50) NOT NULL,
	[SampleNo] [nvarchar](50) NOT NULL,
	[ReportNo] [nvarchar](50) NOT NULL DEFAULT (N'N/A'),
	[TestId] [int] NOT NULL,
	[CollStatus] [nvarchar](10) NOT NULL DEFAULT (N'Pending') ,
	[CollTime] [datetime] NULL,
	[CollUser] [nvarchar](15) NOT NULL DEFAULT (N'Pending') ,
	[SendStatus] [nvarchar](50) NOT NULL DEFAULT (N'Pending') ,
	[SendTime] [datetime] NULL,
	[SendUser] [nvarchar](50) NOT NULL DEFAULT (N'Pending') ,
	[ReceiveInLabStatus] [nvarchar](10) NOT NULL DEFAULT (N'Pending') ,
	[ReceiveInLabTime] [datetime] NULL,
	[ReceiveInLabUser] [nvarchar](50) NOT NULL DEFAULT (N'Pending') ,
	[ReportPrintStatus] [nvarchar](10) NOT NULL DEFAULT (N'Pending') ,
	[ReportPrintTime] [datetime] NULL,
	[ReportPrintUser] [nvarchar](50) NOT NULL DEFAULT (N'Pending') ,
	[ReportProcessStatus] [nvarchar](10) NOT NULL DEFAULT (N'Pending') ,
	[ReportProcessTime] [datetime] NULL,
	[ReportProcessUser] [nvarchar](50) NOT NULL DEFAULT (N'Pending') ,
	[RReceiveInDelCounterStatus] [nvarchar](10) NOT NULL DEFAULT (N'Pending') ,
	[RReceiveInDelCounterTime] [datetime] NULL,
	[RReceiveInDelCounterUser] [nvarchar](50) NOT NULL DEFAULT (N'Pending') ,
	[DeliverToPatientStatus] [nvarchar](10) NOT NULL DEFAULT (N'Pending') ,
	[DeliverToPatientTime] [datetime] NULL,
	[DeliverToPatientUser] [nvarchar](50) NOT NULL DEFAULT (N'Pending') ,
	[RDeliveryDate] [date] NULL,
	[ReportFileName] [nvarchar](150) NOT NULL DEFAULT ('') ,
	[VaqName] [nvarchar](500) NOT NULL DEFAULT ('') ,
	[EntryDate] [datetime] NOT NULL DEFAULT GetDate(),
	[HOSTNAME] [nvarchar](50) NOT NULL DEFAULT (N'N/A') ,
	[FinalStatus] [nvarchar](50) NOT NULL DEFAULT ('') )
");
             MessageBox.Show(@"Success");
        
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }








            try
            {
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_Sample_Status_Info]
AS
SELECT a.MasterId,b.BillNO AS InvNo,b.BillDate AS  InvDate, a.SampleNo AS LabNo,b.PatientName,b.Age,b.Sex,c.Id AS Pcode,c.Name AS ShortDesc,a.CollStatus,a.CollTime,a.CollUser,a.SendStatus,a.SendTime,a.SendUser,a.ReceiveInLabStatus, a.ReceiveInLabTime, a.ReceiveInLabUser, a.ReportProcessStatus, a.ReportProcessTime, a.ReportProcessUser, a.ReportPrintStatus, a.ReportPrintTime, a.ReportPrintUser, a.RReceiveInDelCounterStatus, a.RReceiveInDelCounterTime, a.RReceiveInDelCounterUser, a.DeliverToPatientStatus, a.DeliverToPatientTime, a.DeliverToPatientUser,a.ReportNo,a.ReportFileName AS  VaqGroup,a.FinalStatus,a.EntryDate
FROM tb_LAB_STRICKER_INFO  a LEFT JOIN tb_BILL_MASTER b ON a.MasterId=b.Id 
LEFT JOIN tb_TESTCHART c ON a.TestId=c.Id");
                MessageBox.Show(@"Success");

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
  
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE DEL_RECORD_OF_BILL_DELETE ALTER COLUMN PostedBy Nvarchar(100);");
                _gt.DeleteInsertLab(@"ALTER TABLE DEL_RECORD_OF_BILL_DELETE ALTER COLUMN ModuleName Nvarchar(100);");
            }
            catch (Exception exception)
            {
                
                MessageBox.Show(exception.Message);
            }










            try
            {
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_DEFAULT_DELIVERY_DATETIME](
	[Date] [date] NOT NULL,
	[TimeNumber] [nvarchar](50) NOT NULL,
	[TimeAmPm] [nvarchar](50) NOT NULL,
	[HostName] [nvarchar](50) NOT NULL
) ON [PRIMARY]
ALTER TABLE [dbo].[tb_DEFAULT_DELIVERY_DATETIME] ADD  CONSTRAINT [DF_tb_DEFAULT_DELIVERY_DATETIME_Date]  DEFAULT (getdate()) FOR [Date]
ALTER TABLE [dbo].[tb_DEFAULT_DELIVERY_DATETIME] ADD  CONSTRAINT [DF_tb_DEFAULT_DELIVERY_DATETIME_TimeNumber]  DEFAULT ((5)) FOR [TimeNumber]
ALTER TABLE [dbo].[tb_DEFAULT_DELIVERY_DATETIME] ADD  CONSTRAINT [DF_tb_DEFAULT_DELIVERY_DATETIME_TimeAmPm]  DEFAULT (N'PM') FOR [TimeAmPm]
ALTER TABLE [dbo].[tb_DEFAULT_DELIVERY_DATETIME] ADD  CONSTRAINT [DF_tb_DEFAULT_DELIVERY_DATETIME_HostName]  DEFAULT (N'WORK-PC') FOR [HostName]");
                MessageBox.Show(@"Success");

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }



            try
            {
                _gt.DeleteInsertLab("ALTER TABLE tb_BILL_MASTER ADD DeliveryDate Date NOT NULL DEFAULT GETDATE()");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            try
            {
                _gt.DeleteInsertLab("ALTER TABLE tb_BILL_MASTER ADD DeliveryNumber Nvarchar(50) NOT NULL DEFAULT ''");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            try
            {
                _gt.DeleteInsertLab("ALTER TABLE tb_BILL_MASTER ADD DeliveryTimeAmPm Nvarchar(50) NOT NULL DEFAULT ''");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
          









        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab("ALTER TABLE tb_TESTCHART ADD ReportFee FLOAT NOT NULL DEFAULT 0");
                MessageBox.Show(@"Success");

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }


        }

        private void button7_Click(object sender, EventArgs e)
        {


            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE DEL_RECORD_OF_BILL_DELETE ADD AuthorizedDateTime Nvarchar (150)");
                _gt.DeleteInsertLab(@"ALTER TABLE DEL_RECORD_OF_BILL_DELETE ADD AuthorizedBy Nvarchar (150)");
                _gt.DeleteInsertLab(@"ALTER TABLE DEL_RECORD_OF_BILL_DELETE ADD Status Nvarchar (150)");
                _gt.DeleteInsertLab(@"ALTER TABLE DEL_RECORD_OF_BILL_DELETE ADD MasterId Int NOT NULL DEFAULT 0");
                _gt.DeleteInsertLab(@"ALTER TABLE tb_MASTER_INFO ADD UseFinancialService INT NOT NULL DEFAULT 0");
                _gt.DeleteInsertLab(@"ALTER TABLE tb_BILL_MASTER ADD LastPrintPc Nvarchar(150) NOT NULL DEFAULT ''");

                MessageBox.Show(@"Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            
            try
            {
                _gt.DeleteInsertLab(@"ALTER PROC [dbo].[Sp_Get_InvoicePrint] (@invNo int,@groupName varchar(500))   AS
IF	LEN(@groupName)=0
BEGIN
SELECT a.Id AS MasterId,a.BillNo,a.BillDate,a.BillTime, a.PatientName,a.MobileNo,a.Address,a.Age,a.Sex,c.Name AS RefDr,d.Name As ConsDr,a.TotalAmt,a.LessAmt,a.CollAmt,a.LessFrom ,a.LessType,a.LessPc, a.PostedBy,b.Charge,e.Name AS TestName,a.Remarks,Isnull(f.TotalAmt,0) AS DueColl,Isnull(f.LessAmt,0) AS DueLess,f.LessFrom AS DueLessFrom,f.LessPc AS DueLessPc,f.LessType AS DueLessType,a.RefDrId,a.UnderDrId,b.TestId,b.HnrAmt,b.TotHnrAmt,b.DrLess,CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END  AS SubProjectName,a.DeliveryDate,a.DeliveryNumber,a.DeliveryTimeAmPm
FROM tb_BILL_MASTER a LEFT JOIN tb_BILL_DETAIL b ON a.Id=b.MasterId
LEFT JOIN tb_Doctor c ON a.RefDrId=c.Id
LEFT JOIN tb_Doctor d ON a.UnderDrId=d.Id
LEFT JOIN tb_TestChart e ON b.TestId=e.Id 
LEFT JOIN tb_DUE_COLL f ON a.Id=f.MasterId 
WHERE a.Id=@invNo
END

IF	LEN(@groupName)>0
BEGIN
SELECT a.Id AS MasterId,a.BillNo,a.BillDate,a.BillTime, a.PatientName,a.MobileNo,a.Address,a.Age,a.Sex,c.Name AS RefDr,d.Name As ConsDr,a.TotalAmt,a.LessAmt,a.CollAmt,a.LessFrom ,a.LessType,a.LessPc, a.PostedBy,b.Charge,e.Name AS TestName,a.Remarks,Isnull(f.TotalAmt,0) AS DueColl,Isnull(f.LessAmt,0) AS DueLess,f.LessFrom AS DueLessFrom,f.LessPc AS DueLessPc,f.LessType AS DueLessType,a.RefDrId,a.UnderDrId,b.TestId,b.HnrAmt,b.TotHnrAmt,b.DrLess,CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END  AS SubProjectName,a.DeliveryDate,a.DeliveryNumber,a.DeliveryTimeAmPm
FROM tb_BILL_MASTER a LEFT JOIN tb_BILL_DETAIL b ON a.Id=b.MasterId
LEFT JOIN tb_Doctor c ON a.RefDrId=c.Id
LEFT JOIN tb_Doctor d ON a.UnderDrId=d.Id
LEFT JOIN tb_TestChart e ON b.TestId=e.Id 
LEFT JOIN tb_DUE_COLL f ON a.Id=f.MasterId 
WHERE a.Id=@invNo AND CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END = @groupName
END
");
                MessageBox.Show("Success");

            }
            catch (Exception)
            {
                ;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"ALTER PROC [dbo].[SP_DEL_ALL_DATA]
AS
BEGIN

Truncate table tb_BILL_DETAIL
Truncate table tb_BILL_LEDGER
Truncate table tb_BILL_MASTER
Truncate table tb_DUE_COLL
Truncate table tb_LAB_STRICKER_INFO
Truncate table tb_DOCTOR_LEDGER
Truncate table DEL_RECORD_OF_BILL_DELETE
Truncate table tb_FINANCIAL_COLLECTION
END
");
            }
            catch (Exception)
            {
                ;
            }




            try
            {
                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Due_Invoice_List]
AS
SELECT a.MasterId,b.BillNo,b.BillDate,b.PatientName,b.Age,b.Sex,b.MobileNo,c.Name As ConsDrName,c.Id AS ConsDrId,
Isnull(SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt),0) AS Balance,b.TotalAmt AS BillAmt
FROM tb_BILL_LEDGER a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id
INNER JOIN tb_Doctor c ON b.UnderDrId=c.Id

GROUP BY a.MasterId,b.PatientName,b.Age,b.Sex,b.MobileNo,b.BillDate,b.BillNo,c.Name,c.Id,b.TotalAmt
HAVING SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt)>0
");

                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            
            
            
            
            
            
            
            
            
            
            
            try
            {
                _gt.DeleteInsertLab("ALTER TABLE tb_DOCTOR ADD ReportUserName Nvarchar(500) NOT NULL DEFAULT ''");
                
                
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE DEL_RECORD_OF_BILL_DELETE ADD EntryDate Datetime NOT NULL DEFAULT GetDate()");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            
            
            
            
            
            try
            {
                _gt.DeleteInsertLab("ALTER TABLE tb_MASTER_INFO ADD RemarksInGrid INT NOT NULL DEFAULT 0");
                _gt.DeleteInsertLab("ALTER TABLE tb_BILL_DETAIL ADD GridRemarks Nvarchar(500) NOT NULL DEFAULT ''");
                _gt.DeleteInsertLab("ALTER TABLE tb_MASTER_INFO ADD ReportPrintDuePercentage FLOAT NOT NULL DEFAULT 50");


                

                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            try
            {
                _gt.DeleteInsertLab(@"ALTER PROC [dbo].[Sp_Get_InvoicePrint] (@invNo int,@groupName varchar(500))   AS
IF	LEN(@groupName)=0
BEGIN
SELECT a.Id AS MasterId,a.BillNo,a.BillDate,a.BillTime, a.PatientName,a.MobileNo,a.Address,a.Age,a.Sex,c.Name AS RefDr,d.Name As ConsDr,a.TotalAmt,a.LessAmt,a.CollAmt,a.LessFrom ,a.LessType,a.LessPc, a.PostedBy,b.Charge,e.Name AS TestName,a.Remarks,Isnull(f.TotalAmt,0) AS DueColl,Isnull(f.LessAmt,0) AS DueLess,f.LessFrom AS DueLessFrom,f.LessPc AS DueLessPc,f.LessType AS DueLessType,a.RefDrId,a.UnderDrId,b.TestId,b.HnrAmt,b.TotHnrAmt,b.DrLess,CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END  AS SubProjectName,a.DeliveryDate,a.DeliveryNumber,a.DeliveryTimeAmPm,b.GridRemarks,a.LastPrintPc
FROM tb_BILL_MASTER a LEFT JOIN tb_BILL_DETAIL b ON a.Id=b.MasterId
LEFT JOIN tb_Doctor c ON a.RefDrId=c.Id
LEFT JOIN tb_Doctor d ON a.UnderDrId=d.Id
LEFT JOIN tb_TestChart e ON b.TestId=e.Id 
LEFT JOIN tb_DUE_COLL f ON a.Id=f.MasterId 
WHERE a.Id=@invNo
END

IF	LEN(@groupName)>0
BEGIN
SELECT a.Id AS MasterId,a.BillNo,a.BillDate,a.BillTime, a.PatientName,a.MobileNo,a.Address,a.Age,a.Sex,c.Name AS RefDr,d.Name As ConsDr,a.TotalAmt,a.LessAmt,a.CollAmt,a.LessFrom ,a.LessType,a.LessPc, a.PostedBy,b.Charge,e.Name AS TestName,a.Remarks,Isnull(f.TotalAmt,0) AS DueColl,Isnull(f.LessAmt,0) AS DueLess,f.LessFrom AS DueLessFrom,f.LessPc AS DueLessPc,f.LessType AS DueLessType,a.RefDrId,a.UnderDrId,b.TestId,b.HnrAmt,b.TotHnrAmt,b.DrLess,CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END  AS SubProjectName,a.DeliveryDate,a.DeliveryNumber,a.DeliveryTimeAmPm,b.GridRemarks,a.LastPrintPc
FROM tb_BILL_MASTER a LEFT JOIN tb_BILL_DETAIL b ON a.Id=b.MasterId
LEFT JOIN tb_Doctor c ON a.RefDrId=c.Id
LEFT JOIN tb_Doctor d ON a.UnderDrId=d.Id
LEFT JOIN tb_TestChart e ON b.TestId=e.Id 
LEFT JOIN tb_DUE_COLL f ON a.Id=f.MasterId 
WHERE a.Id=@invNo AND CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END = @groupName
END
");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }















        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_FINANCIAL_SERVICE](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL
) ON [PRIMARY]");
            }
            catch (Exception)
            {
                ;
            }
            try
            {
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_FINANCIAL_COLLECTION](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MasterId] [int] NOT NULL,
	[FinancialId] [int] NOT NULL,
	[Amount] [float] NOT NULL,
	[ModuleName] [nvarchar](50) NOT NULL
) ON [PRIMARY]
ALTER TABLE [dbo].[tb_FINANCIAL_COLLECTION] ADD  CONSTRAINT [DF_tb_FINANCIAL_COLLECTION_Amount]  DEFAULT ((0)) FOR [Amount]");




                _gt.DeleteInsertLab(@"INSERT INTO tb_FINANCIAL_SERVICE(Name)VALUES('Bkash')");
                _gt.DeleteInsertLab(@"INSERT INTO tb_FINANCIAL_SERVICE(Name)VALUES('Nagad')");
                _gt.DeleteInsertLab(@"INSERT INTO tb_FINANCIAL_SERVICE(Name)VALUES('Rocket')");






            }
            catch (Exception)
            {
                ;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {

                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_REPORT_COMMENT_DEFAULT](
	[TestId] [int] NOT NULL,
	[Name] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
");


                _gt.DeleteInsertLab(@"ALTER TABLE tb_REPORT_MASTER ADD RtfFile Nvarchar(Max) NOT NULL DEFAULT ''");
  
                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_REPORT]
AS
SELECT  b.BillNo,a.MasterId, b.BillDate,  a.TestChartId, a.MachineParam, a.ReportParam, a.Result, a.Unit, a.NormalRange, a.ReportingGroup, a.GroupSl, a.ParamSl,'' AS CollUser,a.IsBold, a.IsPrint, 
c.LeftDrName, c.LeftDrDegree, c.MiddleDrName, c.MiddleDrDegree,c.RightDrName,c.RightDrDegree, b.PatientName,b.Age,b.Sex,b.MobileNo,
d.Name AS UnderDrName,(SELECT Top 1 Specimen FROM tb_TESTCHART WHERE Id=a.TestChartId)AS Specimen,a.ReportNo,
'' AS CollTime,c.Comment,c.RtfFile
FROM tb_REPORT_DETAILS a LEFT JOIN  tb_BILL_MASTER b ON a.MasterId=b.Id
INNER JOIN tb_REPORT_MASTER c ON a.MasterId=c.MasterId AND a.ReportNo=c.ReportNo
INNER JOIN tb_Doctor d ON b.UnderDrId=d.Id
AND LTRIM(a.Result)<>''
");


                
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                
                MessageBox.Show(exception.Message);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"CREATE TABLE tb_REPORT_CALCULATION
(
	TestId Int NOT NULL,
	Parameter  nvarchar(50) NULL,
	[Sign] [nvarchar](50) NULL,
	Result [float] NULL,
	GroupB [nvarchar](50) NULL
) ON [PRIMARY]
");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"ALTER PROC [dbo].[SP_GET_INVOICENO] (@StockType Int)   AS
IF @StockType=1
BEGIN
SELECT RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) + 
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo
FROM tb_BILL_MASTER  WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) AND BillDate>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' END IF @StockType=2 BEGIN SELECT RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(TrNo, 4))),0)+ 1), 4) AS InvNo FROM tb_DUE_COLL  WHERE YEAR(TrDate)=YEAR(GetDate()) AND MONTH(TrDate)=MONTH(GetDate()) AND TrDate>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' END");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"ALTER PROC [dbo].[SP_GET_INVOICENO] (@StockType Int)   AS
IF @StockType=1
BEGIN
SELECT RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('000000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 6))),0)+ 1), 6) AS InvNo 
FROM tb_BILL_MASTER  WHERE YEAR(BillDate)=YEAR(GetDate()) AND BillDate>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' END IF @StockType=2 BEGIN SELECT  RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + RIGHT('000000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(TrNo, 6))),0)+ 1), 6) AS InvNo FROM tb_DUE_COLL  WHERE YEAR(BillDate)=YEAR(GetDate()) AND TrDate>='" + DateTime.Now.ToString("yyyy-MM-dd") + "' END");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[EDIT_RECORD_OF_BILL](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrNo] [nvarchar](50) NOT NULL,
	[TrDate] [date] NOT NULL,
	[EntryDate] [datetime] NOT NULL DEFAULT (getdate()),
	[PatientName] [nvarchar](250) NOT NULL,
	[PrevAmt] [float] NOT NULL DEFAULT ((0)),
	[CurrAmt] [float] NOT NULL DEFAULT ((0)),
	[PrevTestNo] [int] NOT NULL DEFAULT ((0)),
	[CurrTestNo] [int] NOT NULL DEFAULT ((0)),
	[PostedBy] [nvarchar](50) NOT NULL,
	[PrevRefDrId] [int] NOT NULL ,
	[CurrRefDrId] [int] NOT NULL,
	[ModuleName] [nvarchar](50) NOT NULL,
	[PcName] [nvarchar](500) NOT NULL,
	[IpAddress] [nvarchar](500) NOT NULL
) ON [PRIMARY]
");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE tb_BILL_MASTER ADD PcName Nvarchar(500) NOT NULL DEFAULT ''");
                _gt.DeleteInsertLab(@"ALTER TABLE tb_BILL_MASTER ADD IpAddress Nvarchar(500) NOT NULL DEFAULT ''");
                _gt.DeleteInsertLab(@"ALTER TABLE tb_DUE_COLL ADD PcName Nvarchar(500) NOT NULL DEFAULT ''");
                _gt.DeleteInsertLab(@"ALTER TABLE tb_DUE_COLL ADD IpAddress Nvarchar(500) NOT NULL DEFAULT ''");

                _gt.DeleteInsertLab(@"ALTER TABLE DEL_RECORD_OF_BILL_DELETE ADD PcName Nvarchar(500) NOT NULL DEFAULT ''");
                _gt.DeleteInsertLab(@"ALTER TABLE DEL_RECORD_OF_BILL_DELETE ADD IpAddress Nvarchar(500) NOT NULL DEFAULT ''");


                





                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
//            try
//            {
//                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ac_DEPARTMENT](
//	[Id] [int] NOT NULL,
//	[Name] [nvarchar](250) NOT NULL
//) ON [PRIMARY]
//");
//                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ac_EXPENSE](
//	[Id] [int] NOT NULL,
//	[DeptId] [int] NOT NULL,
//	[Name] [nvarchar](500) NOT NULL,
//	[Code] [nvarchar](50) NOT NULL
//) ON [PRIMARY]");
//                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ac_EXPENSE_ENTRY](
//	[Id] [int] IDENTITY(1,1) NOT NULL,
//	[TrDate] [date] NOT NULL,
//	[TrNo] [nvarchar](50) NOT NULL,
//	[ExpId] [int] NOT NULL,
//	[Description] [nvarchar](500) NOT NULL,
//	[Amount] [float] NOT NULL,
//	[Remarks] [nvarchar](1500) NOT NULL,
//	[PostedBy] [nvarchar](50) NOT NULL,
//	[PcName] [nvarchar](500) NOT NULL,
//	[IpAddress] [nvarchar](50) NOT NULL,
//	[EntryDate] [datetime] NOT NULL  DEFAULT (getdate()) 
//) ON [PRIMARY]");
              




//                MessageBox.Show("Success");
//            }
//            catch (Exception exception)
//            {
//                MessageBox.Show(exception.Message);
//            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {

                try
                {
                    _gt.DeleteInsertLab("ALTER TABLE tb_MASTER_INFO ADD UseExpenseEntry bit NOT NULL DEFAULT 0");
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }

                try
                {
                    _gt.DeleteInsertLab(@"DROP TABLE [tb_ac_DEPARTMENT]");
                }
                catch (Exception)
                {
                    ;
                }
                try
                {
                    _gt.DeleteInsertLab(@"DROP TABLE [tb_ac_EXPENSE]");
                }
                catch (Exception)
                {
                    ;
                }
                try
                {
                    _gt.DeleteInsertLab(@"DROP TABLE [tb_ac_EXPENSE_ENTRY]");
                }
                catch (Exception)
                {
                    ;
                }
                try
                {
                    _gt.DeleteInsertLab(@"DROP TABLE [tb_ac_SUB_DEPARTMENT]");
                }
                catch (Exception)
                {
                    ;
                }
                try
                {
                    _gt.DeleteInsertLab(@"DROP TABLE [tb_ac_INCOME_ENTRY]");
                }
                catch (Exception)
                {
                    ;
                }
                try
                {
                    _gt.DeleteInsertLab(@"DROP TABLE [tb_ac_HEAD]");
                }
                catch (Exception)
                {
                    ;
                }
                try
                {
                    _gt.DeleteInsertLab(@"DROP VIEW [V_Income_List]");
                }
                catch (Exception)
                {
                    ;
                }
                try
                {
                    _gt.DeleteInsertLab(@"DROP VIEW [V_Expense_List]");
                }
                catch (Exception)
                {
                    ;
                }
                try
                {
                    _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ac_DEPARTMENT](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL
) ON [PRIMARY]
CREATE TABLE [dbo].[tb_ac_EXPENSE](
	[Id] [int] NOT NULL,
	[SubDeptId] [int] NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Code] [nvarchar](50) NOT NULL
) ON [PRIMARY]
CREATE TABLE [dbo].[tb_ac_EXPENSE_ENTRY](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrDate] [date] NOT NULL,
	[TrNo] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[Amount] [float] NOT NULL,
	[Remarks] [nvarchar](1500) NOT NULL,
	[PostedBy] [nvarchar](50) NOT NULL,
	[PcName] [nvarchar](500) NOT NULL,
	[IpAddress] [nvarchar](50) NOT NULL,
	[EntryDate] [datetime] NOT NULL  DEFAULT (getdate())
) ON [PRIMARY]
CREATE TABLE [dbo].[tb_ac_SUB_DEPARTMENT](
	[Id] [int] NOT NULL,
	[DeptId] [int] NOT NULL,
	[Name] [nvarchar](500) NOT NULL
) ON [PRIMARY]
CREATE TABLE [dbo].[tb_ac_INCOME_ENTRY](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrDate] [date] NOT NULL,
	[TrNo] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[Amount] [float] NOT NULL,
	[Remarks] [nvarchar](1500) NOT NULL,
	[PostedBy] [nvarchar](50) NOT NULL,
	[PcName] [nvarchar](500) NOT NULL,
	[IpAddress] [nvarchar](50) NOT NULL,
	[EntryDate] [datetime] NOT NULL DEFAULT (getdate())
) ON [PRIMARY]
CREATE TABLE [dbo].[tb_ac_HEAD](
	[Id] [int] NOT NULL,
	[DeptId] [int] NOT NULL,
	[SubDeptId] [int] NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL
) ON [PRIMARY]");
                }
                catch (Exception)
                {
                    ;
                }
                try
                {
                    _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_Income_List]
AS
SELECT a.TrDate,a.TrNo,a.Code,a.Description,a.Amount,a.Remarks,a.PostedBy,c.Id AS SubDeptId,c.Name AS SubDeptName,d.Id AS DeptId,d.Name AS DeptName 
FROM tb_ac_INCOME_ENTRY a 
INNER JOIN tb_ac_HEAD b ON a.Code=b.Code
INNER JOIN tb_ac_SUB_DEPARTMENT c ON b.SubDeptId=c.Id
INNER JOIN tb_ac_DEPARTMENT d ON b.DeptId=d.Id
WHERE b.Type='Income'");
                }
                catch (Exception)
                {
                    ;
                }
                try
                {
                    _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_Expense_List]
AS
SELECT a.TrDate,a.TrNo,a.Code,a.Description,a.Amount,a.Remarks,a.PostedBy,c.Id AS SubDeptId,c.Name AS SubDeptName,d.Id AS DeptId,d.Name AS DeptName 
FROM tb_ac_EXPENSE_ENTRY a 
INNER JOIN tb_ac_HEAD b ON a.Code=b.Code
INNER JOIN tb_ac_SUB_DEPARTMENT c ON b.SubDeptId=c.Id
INNER JOIN tb_ac_DEPARTMENT d ON b.DeptId=d.Id
WHERE b.Type='Expense'");
                }
                catch (Exception)
                {
                    ;
                }



                MessageBox.Show(@"Success");
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            try
            {








                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_REPORT_RESULT_DEFAULT](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TestId] [int] NOT NULL,
	[Param] [nvarchar](100) NOT NULL,
	[Result] [nvarchar](500) NOT NULL
) ON [PRIMARY]");


                MessageBox.Show(@"Success");
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            try
            {

                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_VALIDATION](
	[RawData1] [nvarchar](1500) NOT NULL,
	[RawData2] [nvarchar](1500) NOT NULL
) ON [PRIMARY]");


                _gt.DeleteInsertLab(@"INSERT [dbo].[tb_VALIDATION] ([RawData1], [RawData2]) VALUES (N'[	å©
Ú', N'u6hIIMy23oXwxvnhRKtplqio5wdcM92q')");
                MessageBox.Show(@"Success");
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE tb_ac_EXPENSE_ENTRY DROP COLUMN ExpId
ALTER TABLE tb_ac_EXPENSE_ENTRY ADD Code Nvarchar(50) NOT NULL ");













            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                
                
                
                
                
                
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_Expense_List]
AS
SELECT a.TrDate,a.TrNo,a.Code,a.Description,a.Amount,a.Remarks,a.PostedBy,c.Id AS SubDeptId,c.Name AS SubDeptName,d.Id AS DeptId,d.Name AS DeptName 
FROM tb_ac_EXPENSE_ENTRY a 
INNER JOIN tb_ac_HEAD b ON a.Code=b.Code
INNER JOIN tb_ac_SUB_DEPARTMENT c ON b.SubDeptId=c.Id
INNER JOIN tb_ac_DEPARTMENT d ON b.DeptId=d.Id
WHERE b.Type='Expense'");


                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_Income_List]
AS
SELECT a.TrDate,a.TrNo,a.Code,a.Description,a.Amount,a.Remarks,a.PostedBy,c.Id AS SubDeptId,c.Name AS SubDeptName,d.Id AS DeptId,d.Name AS DeptName 
FROM tb_ac_INCOME_ENTRY a 
INNER JOIN tb_ac_HEAD b ON a.Code=b.Code
INNER JOIN tb_ac_SUB_DEPARTMENT c ON b.SubDeptId=c.Id
INNER JOIN tb_ac_DEPARTMENT d ON b.DeptId=d.Id
WHERE b.Type='Income'");

                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ac_INCOME_ENTRY](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrDate] [date] NOT NULL,
	[TrNo] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[Amount] [float] NOT NULL,
	[Remarks] [nvarchar](1500) NOT NULL,
	[PostedBy] [nvarchar](50) NOT NULL,
	[PcName] [nvarchar](500) NOT NULL,
	[IpAddress] [nvarchar](50) NOT NULL,
	[EntryDate] [datetime] NOT NULL DEFAULT (getdate())
) ON [PRIMARY]");


                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ac_HEAD](
	[Id] [int] NOT NULL,
	[DeptId] [int] NOT NULL,
	[SubDeptId] [int] NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL
) ON [PRIMARY]");
                MessageBox.Show("Success");
                 }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            try
            {
                                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Datewise_Income_Expense]
AS
SELECT TrDate ,Status,SUM(CollAmt) AS Income,0 AS Expense
FROM V_Daily_Collection_List GROUP BY Status,TrDate
UNION ALL
SELECT TrDate ,'Income' AS Status,SUM(Amount) AS Income,0 AS Expense
FROM V_Expense_List GROUP BY TrDate
UNION ALL
SELECT TrDate ,'Expense' AS Status,0 AS Income,SUM(Amount) AS Expense
FROM V_Expense_List GROUP BY TrDate
");





                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                
              MessageBox.Show(exception.Message);
            }


        }

        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"CREATE VIEW V_Test_Wise_Doctor
AS 
SELECT a.BillDate,a.RefDrId,b.Name AS RefDrName, COUNT(d.TestId) AS TestCount,d.TestId
FROM tb_BILL_MASTER a INNER JOIN tb_DOCTOR b ON a.RefDrId=b.Id
INNER JOIN tb_BILL_DETAIL d ON a.Id=d.MasterId
GROUP BY a.BillDate,a.RefDrId,b.Name,d.TestId
");
                MessageBox.Show("Success");

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab("ALTER TABLE tb_MASTER_INFO ADD AutoPrintDiagBill BIT NOT NULL DEFAULT 0");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button19_Click_2(object sender, EventArgs e)
        {
            try
            {

                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Due_Collection_List]
AS
SELECT a.TrNo,a.TrDate,a.MasterId,b.PatientName,b.MobileNo,a.TotalAmt,a.LessAmt,a.PostedBy,b.UnderDrId AS ConsDrId,c.Name AS ConsDrName,b.BillDate,b.BillNo
FROM tb_DUE_COLL a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.id
INNER JOIN tb_Doctor c ON b.UnderDrId=c.Id
");



                MessageBox.Show(@"Success");
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
        }

        private void button20_Click_1(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_TESTCHART_PARAM_MICRO_GROWTH](
	[SlNo] [int] NOT NULL,
	[DrugName] [nvarchar](500) NOT NULL
) ON [PRIMARY]


INSERT INTO tb_TESTCHART_PARAM_MICRO_GROWTH(SlNo,DrugName)VALUES('1','AMIKACIN')
INSERT INTO tb_TESTCHART_PARAM_MICRO_GROWTH(SlNo,DrugName)VALUES('2','AMOXICLAV')
INSERT INTO tb_TESTCHART_PARAM_MICRO_GROWTH(SlNo,DrugName)VALUES('3','AMOXYCILLIN')
INSERT INTO tb_TESTCHART_PARAM_MICRO_GROWTH(SlNo,DrugName)VALUES('4','AMPICILLIN')
INSERT INTO tb_TESTCHART_PARAM_MICRO_GROWTH(SlNo,DrugName)VALUES('5','AZITHROMYCIN')
INSERT INTO tb_TESTCHART_PARAM_MICRO_GROWTH(SlNo,DrugName)VALUES('6','CEFEPIME')
INSERT INTO tb_TESTCHART_PARAM_MICRO_GROWTH(SlNo,DrugName)VALUES('7','CEFOTAXIME')
INSERT INTO tb_TESTCHART_PARAM_MICRO_GROWTH(SlNo,DrugName)VALUES('8','FOSFOMYCIN')
INSERT INTO tb_TESTCHART_PARAM_MICRO_GROWTH(SlNo,DrugName)VALUES('9','CEPHRADINE')
INSERT INTO tb_TESTCHART_PARAM_MICRO_GROWTH(SlNo,DrugName)VALUES('10','OXACILLIN')
");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            try
            {
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[A_HeaderView](
	[InvNo] [nvarchar](50) NOT NULL,
	[InvDate] [date] NOT NULL,
	[PtName] [nvarchar](2500) NOT NULL,
	[Age] [nvarchar](50) NOT NULL,
	[Sex] [nvarchar](50) NOT NULL,
	[DrName] [nvarchar](500) NOT NULL
) ON [PRIMARY]");
            }
            catch (Exception exception)
            {
                
                MessageBox.Show(exception.Message);
            }




        }

        private void button22_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"
ALTER VIEW [dbo].[V_Due_Collection_List]
AS
SELECT a.TrNo,a.TrDate,a.MasterId,b.PatientName,b.MobileNo,a.TotalAmt,a.LessAmt,a.PostedBy,b.UnderDrId AS ConsDrId,c.Name AS ConsDrName,b.BillDate,b.BillNo,b.RefDrId,d.Name AS RefdrName
FROM tb_DUE_COLL a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.id
INNER JOIN tb_Doctor c ON b.UnderDrId=c.Id
INNER JOIN tb_Doctor d ON b.RefDrId=d.Id");
                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Due_Invoice_List]
AS
SELECT a.MasterId,b.BillNo,b.BillDate,b.PatientName,b.Age,b.Sex,b.MobileNo,c.Name As ConsDrName,c.Id AS ConsDrId,
Isnull(SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt),0) AS Balance,b.TotalAmt AS BillAmt,b.RefDrId,d.Name AS RefDrName
FROM tb_BILL_LEDGER a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id
INNER JOIN tb_Doctor c ON b.UnderDrId=c.Id
INNER JOIN tb_Doctor d ON b.RefDrId=d.Id

GROUP BY a.MasterId,b.PatientName,b.Age,b.Sex,b.MobileNo,b.BillDate,b.BillNo,c.Name,c.Id,b.TotalAmt,b.RefDrId,d.Name
HAVING SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt)>0
");
            }
            catch (Exception)
            {
                
                ;
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Doctor_Honoriam]
AS
SELECT a.BillNo,a.BillDate ,b.Charge,b.TestId,b.HnrAmt,b.LessAmt As DrLess,b.HnrAmt AS TotHnrAmt,c.Name,a.RefDrId,d.Name AS RefDrName
FROM tb_BILL_MASTER a INNER JOIN tb_DOCTOR_LEDGER b ON a.Id=b.MasterId AND a.RefDrId=b.DrId
INNER JOIN tb_TESTCHART c ON b.TestId=c.Id
INNER JOIN tb_DOCTOR d ON a.RefDrId=d.Id WHERE b.ModuleName='Diagnosis'");
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_Doctor_Honoriam_UnderDrwise]
AS
SELECT a.BillNo,a.BillDate ,b.Charge,b.TestId,b.HnrAmt,b.LessAmt As DrLess,b.HnrAmt AS TotHnrAmt,c.Name,a.UnderDrId,d.Name AS RefDrName
FROM tb_BILL_MASTER a INNER JOIN tb_DOCTOR_LEDGER b ON a.Id=b.MasterId AND a.UnderDrId=b.DrId
INNER JOIN tb_TESTCHART c ON b.TestId=c.Id
INNER JOIN tb_DOCTOR d ON a.UnderDrId=d.Id WHERE b.ModuleName='Diagnosis'");
                MessageBox.Show("Success");
            }
            catch (Exception)
            {

                ;
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {

            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE tb_MASTER_INFO ADD UseIndoor Bit NOT NULL DEFAULT 0");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            
            
            
            
            
            
            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE tb_TestChart ADD IsDoctor INT NOT NULL DEFAULT 0;ALTER TABLE tb_TestChart ADD IsIndoorItem INT NOT NULL DEFAULT 0;");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            try
            {
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_in_BED_TYPE](
	[Name] [nvarchar](50) NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[tb_in_PATIENT_LEDGER](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrNo] [nvarchar](50) NOT NULL,
	[TrDate] [date] NOT NULL,
	[RegId] [int] NOT NULL,
	[AdmId] [int] NOT NULL,
	[BedId] [int] NOT NULL,
	[TestId] [int] NOT NULL,
	[TestName] [nvarchar](2500) NOT NULL,
	[Charge] [float] NOT NULL CONSTRAINT [DF_tb_in_PATIENT_LEDGER_Charge]  DEFAULT ((0)),
	[Unit] [int] NOT NULL,
	[TotCharge] [float] NOT NULL CONSTRAINT [DF_tb_in_PATIENT_LEDGER_TotCharge]  DEFAULT ((0)),
	[LessAmt] [float] NOT NULL CONSTRAINT [DF_tb_in_PATIENT_LEDGER_LessAmt]  DEFAULT ((0)),
	[CollAmt] [float] NOT NULL CONSTRAINT [DF_tb_in_PATIENT_LEDGER_CollAmt]  DEFAULT ((0)),
	[RtnAmt] [float] NOT NULL CONSTRAINT [DF_tb_in_PATIENT_LEDGER_RtnAmt]  DEFAULT ((0)),
	[ExtraLessAmt] [float] NOT NULL CONSTRAINT [DF_Table_1_AdjustAmt]  DEFAULT ((0))
) ON [PRIMARY];


CREATE TABLE [dbo].[tb_in_DEPARTMENT](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL
) ON [PRIMARY];

CREATE TABLE [dbo].[tb_in_BED](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Floor] [nvarchar](500) NULL,
	[BedType] [nvarchar](500) NULL,
	[Charge] [float] NOT NULL CONSTRAINT [DF_tb_IN_BED_Charge]  DEFAULT ((0)),
	[SrvCharge] [float] NOT NULL CONSTRAINT [DF_tb_IN_BED_SrvCharge]  DEFAULT ((0)),
	[BookStatus] [int] NOT NULL CONSTRAINT [DF_Table_1_BookedStatus]  DEFAULT ((0)),
	[DeptId] [int] NOT NULL,
	[PostedBy] [nvarchar](50) NOT NULL,
	[EntryDate] [datetime] NOT NULL CONSTRAINT [DF_tb_IN_BED_EntryDate]  DEFAULT (getdate())
) ON [PRIMARY];

CREATE TABLE [dbo].[tb_in_ADVANCE_COLLECTION](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrNo] [nvarchar](50) NOT NULL,
	[TrDate] [date] NOT NULL,
	[RegId] [int] NOT NULL,
	[AdmId] [int] NOT NULL,
	[BedId] [int] NOT NULL,
	[Amount] [money] NOT NULL CONSTRAINT [DF_tb_in_Advance_Collection_Amount]  DEFAULT ((0)),
	[PostedBy] [nvarchar](500) NOT NULL,
	[EntryDate] [datetime] NOT NULL CONSTRAINT [DF_tb_in_Advance_Collection_EntryDate]  DEFAULT (getdate()),
	[PcName] [nvarchar](50) NOT NULL,
	[IpAddress] [nvarchar](50) NOT NULL
) ON [PRIMARY];




CREATE TABLE [dbo].[tb_in_ADMISSION](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeptId] [int] NOT NULL,
	[RegId] [int] NOT NULL,
	[AdmNo] [nvarchar](50) NOT NULL,
	[AdmDate] [date] NOT NULL,
	[AdmTime] [nvarchar](50) NOT NULL,
	[PtName] [nvarchar](2500) NOT NULL,
	[ContactNo] [nvarchar](50) NOT NULL,
	[PtSex] [nvarchar](50) NOT NULL,
	[PtAge] [nvarchar](500) NOT NULL,
	[PtAddress] [nvarchar](2550) NOT NULL,
	[SpouseName] [nvarchar](2550) NOT NULL,
	[EmgrContact] [nvarchar](50) NOT NULL,
	[ChiefComplain] [nvarchar](2550) NOT NULL,
	[RefId] [int] NOT NULL CONSTRAINT [DF_Table_1_RefDrId]  DEFAULT ((0)),
	[UnderDrId] [int] NOT NULL CONSTRAINT [DF_tb_IN_ADMISSION_UnderDrId]  DEFAULT ((0)),
	[BedId] [int] NOT NULL,
	[AdmCharge] [float] NOT NULL CONSTRAINT [DF_tb_IN_ADMISSION_AdmCharge]  DEFAULT ((0)),
	[ReleaseStatus] [int] NOT NULL CONSTRAINT [DF_tb_IN_ADMISSION_ReleaseStatus]  DEFAULT ((0)),
	[PostedBy] [nvarchar](50) NOT NULL,
	[EntryDate] [datetime] NOT NULL CONSTRAINT [DF_tb_IN_ADMISSION_EntryDate]  DEFAULT (getdate()),
	[PcName] [nvarchar](50) NOT NULL,
	[IpAddress] [nvarchar](50) NOT NULL,
	[TrNo] [nvarchar](50) NULL
) ON [PRIMARY]");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }

        private void button25_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_Advance_Collection_List]
AS
SELECT a.TrNo,a.TrDate,a.Amount,a.PostedBy AS UserName ,b.Id,b.DeptId,b.RegId,b.AdmNo,b.AdmDate,b.AdmTime,b.PtName,b.ContactNo,b.PtAge,b.PtSex,b.PtAddress,b.ChiefComplain,b.RefId,b.RefDrName,b.UnderDrId,b.UnderDrName,b.BedName,b.Floor
FROM tb_in_ADVANCE_COLLECTION a INNER JOIN V_Admission_List b ON a.AdmId=b.Id");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            try
            {
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_IN_BED_WITH_DEPT] AS
SELECT a.Id,a.Name,a.Floor,a.BedType,a.Charge,a.SrvCharge,a.PostedBy,a.DeptId ,b.Name AS DeptName
FROM tb_IN_BED a INNER JOIN tb_IN_DEPARTMENT b ON a.DeptId=b.Id;");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }


            try
            {
                _gt.DeleteInsertLab("DROP VIEW V_Admission_List");
            }
            catch (Exception)
            {
                
                ;
            }










            try
            {
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_Admission_List]
AS
SELECT a.*,b.Name AS RefDrName,c.Name AS UnderDrName ,d.Name AS BedName,d.Floor,d.DeptName
FROM tb_in_ADMISSION a 
INNER JOIN tb_DOCTOR b ON a.RefId=b.Id
INNER JOIN tb_DOCTOR c ON a.UnderDrId=c.Id
INNER JOIN V_IN_BED_WITH_DEPT d ON a.BedId=d.Id;");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Due_Invoice_List]
AS
SELECT a.MasterId,b.BillNo,b.BillDate,b.PatientName,b.Age,b.Sex,b.MobileNo,c.Name As ConsDrName,c.Id AS ConsDrId,
Isnull(SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt),0) AS Balance,b.TotalAmt AS BillAmt,b.RefDrId,d.Name AS RefDrName
FROM tb_BILL_LEDGER a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id
INNER JOIN tb_Doctor c ON b.UnderDrId=c.Id
INNER JOIN tb_Doctor d ON b.RefDrId=d.Id

GROUP BY a.MasterId,b.PatientName,b.Age,b.Sex,b.MobileNo,b.BillDate,b.BillNo,c.Name,c.Id,b.TotalAmt,b.RefDrId,d.Name
HAVING SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt)>0");

                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_DigitalVd](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BillNo] [nvarchar](50) NOT NULL,
	[BillDate] [date] NOT NULL,
	[DeptName] [nvarchar](50) NOT NULL,
	[Price] [float] NOT NULL CONSTRAINT [DF_tb_DigitalVd_Price]  DEFAULT ((0)),
	[Less] [float] NOT NULL CONSTRAINT [DF_tb_DigitalVd_Less]  DEFAULT ((0)),
	[RefAmt] [float] NOT NULL CONSTRAINT [DF_tb_DigitalVd_RefAmt]  DEFAULT ((0)),
	[RefId] [int] NOT NULL,
	[DrOneAmt] [float] NOT NULL CONSTRAINT [DF_tb_DigitalVd_DrOneAmt]  DEFAULT ((0)),
	[DrOneId] [int] NOT NULL,
	[DrTwoAmt] [float] NOT NULL CONSTRAINT [DF_tb_DigitalVd_DrTwoAmt]  DEFAULT ((0)),
	[DrTwoId] [int] NOT NULL,
	[DrThreeAmt] [float] NOT NULL CONSTRAINT [DF_tb_DigitalVd_DrThreeAmt]  DEFAULT ((0)),
	[DrThreeName] [nvarchar](500) NULL,
	[Balance] [float] NOT NULL CONSTRAINT [DF_tb_DigitalVd_Balance]  DEFAULT ((0)),
	[Paid] [float] NOT NULL CONSTRAINT [DF_tb_DigitalVd_Paid]  DEFAULT ((0)),
	[Remarks] [nvarchar](500) NULL,
	[EntryDate] [datetime] NOT NULL CONSTRAINT [DF_tb_DigitalVd_EntryDate]  DEFAULT (getdate()),
	[PostedBy] [nvarchar](50) NOT NULL,
	[PcName] [nvarchar](50) NOT NULL,
	[IpAddress] [nvarchar](50) NOT NULL
) ON [PRIMARY]
");

                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE tb_DOCTOR_LEDGER Add ModuleName Nvarchar NOT NULL DEFAULT 'Diagnosis'");

                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }


            try
            {
                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Doctor_Honoriam]
                    AS
                    SELECT a.BillNo,a.BillDate ,b.Charge,b.TestId,b.HnrAmt,b.LessAmt As DrLess,b.HnrAmt AS TotHnrAmt,c.Name,a.RefDrId,d.Name AS RefDrName
                    FROM tb_BILL_MASTER a INNER JOIN tb_DOCTOR_LEDGER b ON a.Id=b.MasterId
                    INNER JOIN tb_TESTCHART c ON b.TestId=c.Id
                    INNER JOIN tb_DOCTOR d ON b.drid=d.Id
                    WHERE b.ModuleName='Diagnosis'");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            try
            {
                _gt.DeleteInsertLab(@"CREATE PROC [dbo].[S_ReleaseNo]    AS
BEGIN
SELECT 'R'+RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) + 
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo
FROM tb_IN_BILL_MASTER  WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate())  END
");
            }
            catch (Exception exception)
            {
               MessageBox.Show(exception.Message);
            }

        }

        private void button29_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"IF OBJECT_ID('V_Digital_Vd', 'U') IS NOT NULL   DROP VIEW V_Digital_Vd; ");


                _gt.DeleteInsertLab(@"Create VIEW  V_Digital_Vd AS
SELECT a.BillNo,a.BillDate,a.DeptName,a.Price,a.Less,a.RefId ,b.Name AS ReName,a.RefAmt,a.DrOneId,Isnull(c.Name,'') AS DrOneName,a.DrOneAmt,a.DrTwoId,Isnull(d.Name,'') AS DrTwoName,a.DrTwoAmt,a.DrThreeName,a.DrThreeAmt
FROM tb_DigitalVd a INNER JOIN tb_DOCTOR b ON a.RefId=b.Id
INNER JOIN tb_DOCTOR c ON a.DrOneId=c.Id
INNER JOIN tb_DOCTOR d ON a.DrTwoId=d.Id"); 
            }
            catch (Exception rException)
            {
                MessageBox.Show(rException.Message);
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (_gt.FnSeekRecordNewLab("tb_in_ADMISSION","COUNT(Id)>0"))
            {
                return;
            }



            try
            {
                _gt.DeleteInsertLab("ALTER TABLE tb_MASTER_INFO ADD UseIndoor Bit NOT NULL DEFAULT 0");
                MessageBox.Show(@"Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            
            
            
            
            
            
            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE tb_BILL_MASTER ADD AdmId INT DEFAULT 0 NOT NULL");
               
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE tb_DUE_COLL ADD AdmId INT DEFAULT 0 NOT NULL");
              
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE tb_TESTCHART ADD ChangeCharge INT  NOT NUll DEFAULT 0");
               
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            try
            {
                _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_in_PATIENT_LEDGER', 'U') IS NOT NULL   DROP TABLE tb_in_PATIENT_LEDGER");
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_in_PATIENT_LEDGER](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrNo] [nvarchar](50) NOT NULL,
	[TrDate] [date] NOT NULL,
	[RegId] [int] NOT NULL,
	[AdmId] [int] NOT NULL,
	[BedId] [int] NOT NULL,
	[TestId] [int] NOT NULL,
	[TestName] [nvarchar](2500) NOT NULL,
	[Charge] [float] NOT NULL   DEFAULT ((0)),
	[Unit] [int] NOT NULL,
	[TotCharge] [float] NOT NULL  DEFAULT ((0)),
	[LessAmt] [float] NOT NULL  DEFAULT ((0)),
	[CollAmt] [float] NOT NULL  DEFAULT ((0)),
	[RtnAmt] [float] NOT NULL   DEFAULT ((0)),
	[ExtraLessAmt] [float] NOT NULL   DEFAULT ((0)),
	[DrId] [int] NOT NULL   DEFAULT ((0)),
	[DrAmt] [float] NOT NULL  DEFAULT ((0))
) ON [PRIMARY]");
                
               
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            try
            {
                _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_in_FIXED_ID', 'U') IS NOT NULL   DROP TABLE tb_in_FIXED_ID");
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_in_FIXED_ID](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](500) NOT NULL
) ON [PRIMARY];
INSERT [dbo].[tb_in_FIXED_ID] ([Id], [Name]) VALUES (14281, N'Advance Collection');
INSERT [dbo].[tb_in_FIXED_ID] ([Id], [Name]) VALUES (1624, N'Admission Fee');
INSERT [dbo].[tb_in_FIXED_ID] ([Id], [Name]) VALUES (14282, N'Diagnosis Bill');
INSERT [dbo].[tb_in_FIXED_ID] ([Id], [Name]) VALUES (14283, N'Release Time Collection');");

               
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            try
            {
                _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_in_DEPARTMENT', 'U') IS NOT NULL   DROP TABLE tb_in_DEPARTMENT");
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_in_DEPARTMENT](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL
) ON [PRIMARY]");

                _gt.DeleteInsertLab(@"INSERT INTO tb_in_DEPARTMENT(Name)VALUES('Cardiology')");
                _gt.DeleteInsertLab(@"INSERT INTO tb_in_DEPARTMENT(Name)VALUES('Gynaecology')");
                _gt.DeleteInsertLab(@"INSERT INTO tb_in_DEPARTMENT(Name)VALUES('Ophthalmology')");
                _gt.DeleteInsertLab(@"INSERT INTO tb_in_DEPARTMENT(Name)VALUES('Surgery')");
                _gt.DeleteInsertLab(@"INSERT INTO tb_in_DEPARTMENT(Name)VALUES('Medicine')");


               
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }



            try
            {
                _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_IN_BILL_MASTER', 'U') IS NOT NULL   DROP TABLE tb_IN_BILL_MASTER");
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_IN_BILL_MASTER](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BillNo] [nvarchar](50) NOT NULL,
	[BillDate] [date] NOT NULL,
	[BillTime] [nvarchar](50) NOT NULL,
	[AdmId] [int] NOT NULL,
	[RegId] [int] NOT NULL,
	[BedId] [int] NOT NULL,
	[TotalAmt] [float] NOT NULL DEFAULT ((0)),
	[TotalDiscount] [float] NOT NULL   DEFAULT ((0)),
	[AdvanceAmt] [float] NOT NULL   DEFAULT ((0)),
	[PaidAmt] [float] NOT NULL   DEFAULT ((0)),
	[Remarks] [nvarchar](1550) NULL,
	[PostedBy] [nvarchar](50) NOT NULL,
	[PcName] [nvarchar](50) NOT NULL,
	[IpAddress] [nvarchar](50) NOT NULL,
	[Entrydate] [datetime] NOT NULL  DEFAULT (getdate())
) ON [PRIMARY]");


                _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_in_BILL_DETAIL', 'U') IS NOT NULL   DROP TABLE tb_in_BILL_DETAIL");
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_in_BILL_DETAIL](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MasterId] [int] NOT NULL,
	[BillNo] [nvarchar](50) NOT NULL,
	[TestId] [int] NOT NULL,
	[TestName] [nvarchar](1500) NOT NULL  DEFAULT (''),
	[Charge] [float] NOT NULL  DEFAULT ((0)),
	[Unit] [float] NOT NULL  DEFAULT ((0)),
	[TotCharge] [float] NOT NULL  DEFAULT ((0)),
	[LessAmt] [float] NOT NULL   DEFAULT ((0)),
	[ExtraLess] [float] NOT NULL   DEFAULT ((0)),
	[DrAmt] [float] NOT NULL  DEFAULT ((0)),
	[DrId] [int] NOT NULL   DEFAULT ((0))
) ON [PRIMARY]");

                _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_in_BED_TYPE', 'U') IS NOT NULL   DROP TABLE tb_in_BED_TYPE");
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_in_BED_TYPE](
	[Name] [nvarchar](50) NOT NULL
) ON [PRIMARY]");


                _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_in_BED', 'U') IS NOT NULL   DROP TABLE tb_in_BED");
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_in_BED](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
	[Floor] [nvarchar](500) NULL,
	[BedType] [nvarchar](500) NULL,
	[Charge] [float] NOT NULL   DEFAULT ((0)),
	[SrvCharge] [float] NOT NULL  DEFAULT ((0)),
	[BookStatus] [int] NOT NULL  DEFAULT ((0)),
	[DeptId] [int] NOT NULL,
	[PostedBy] [nvarchar](50) NOT NULL,
	[EntryDate] [datetime] NOT NULL   DEFAULT (getdate())
) ON [PRIMARY]");

                _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_in_ADVANCE_COLLECTION', 'U') IS NOT NULL   DROP TABLE tb_in_ADVANCE_COLLECTION");
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_in_ADVANCE_COLLECTION](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrNo] [nvarchar](50) NOT NULL,
	[TrDate] [date] NOT NULL,
	[RegId] [int] NOT NULL,
	[AdmId] [int] NOT NULL,
	[BedId] [int] NOT NULL,
	[Amount] [float] NOT NULL   DEFAULT ((0)),
	[PostedBy] [nvarchar](500) NOT NULL,
	[EntryDate] [datetime] NOT NULL   DEFAULT (getdate()),
	[PcName] [nvarchar](50) NOT NULL,
	[IpAddress] [nvarchar](50) NOT NULL
) ON [PRIMARY]");


                _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_in_ADMISSION', 'U') IS NOT NULL   DROP TABLE tb_in_ADMISSION");
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_in_ADMISSION](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeptId] [int] NOT NULL,
	[RegId] [int] NOT NULL,
	[AdmNo] [nvarchar](50) NOT NULL,
	[AdmDate] [date] NOT NULL,
	[AdmTime] [nvarchar](50) NOT NULL,
	[PtName] [nvarchar](2500) NOT NULL,
	[ContactNo] [nvarchar](50) NOT NULL,
	[PtSex] [nvarchar](50) NOT NULL,
	[PtAge] [nvarchar](500) NOT NULL,
	[PtAddress] [nvarchar](2550) NOT NULL,
	[SpouseName] [nvarchar](2550) NOT NULL,
	[EmgrContact] [nvarchar](50) NOT NULL,
	[ChiefComplain] [nvarchar](2550) NOT NULL,
	[RefId] [int] NOT NULL  DEFAULT ((0)),
	[UnderDrId] [int] NOT NULL  DEFAULT ((0)),
	[BedId] [int] NOT NULL,
	[AdmCharge] [float] NOT NULL   DEFAULT ((0)),
	[ReleaseStatus] [int] NOT NULL   DEFAULT ((0)),
	[PostedBy] [nvarchar](50) NOT NULL,
	[EntryDate] [datetime] NOT NULL   DEFAULT (getdate()),
	[PcName] [nvarchar](50) NOT NULL,
	[IpAddress] [nvarchar](50) NOT NULL,
	[TrNo] [nvarchar](50) NULL
) ON [PRIMARY]");



                _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_in_TEST_ADD_LIST', 'U') IS NOT NULL   DROP TABLE tb_in_TEST_ADD_LIST");
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_in_TEST_ADD_LIST](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrNo] [nvarchar](50) NOT NULL,
	[TrDate] [date] NOT NULL,
	[AdmId] [int] NOT NULL,
	[BedId] [int] NOT NULL,
	[TestId] [int] NOT NULL,
	[TestName] [nvarchar](500) NOT NULL,
	[Charge] [float] NOT NULL,
	[Unit] [float] NOT NULL,
	[TotCharge] [float] NOT NULL,
	[Remarks] [nvarchar](500) NOT NULL,
	[PostedBy] [nvarchar](50) NOT NULL,
	[IpAddress] [nvarchar](50) NOT NULL,
	[EntryDate] [datetime] NOT NULL  DEFAULT (getdate())
) ON [PRIMARY]");









                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }







            try
            {
                _gt.DeleteInsertLab(@"IF OBJECT_ID('V_Indoor_Bill', 'V')  IS NOT NULL  DROP VIEW V_Indoor_Bill; ");
                _gt.DeleteInsertLab(@"IF OBJECT_ID('[V_in_TestAdd_Bill]', 'V')  IS NOT NULL  DROP VIEW [V_in_TestAdd_Bill]; ");
                _gt.DeleteInsertLab(@"IF OBJECT_ID('[V_in_DueList]', 'V') IS NOT NULL   DROP VIEW [V_in_DueList]; ");
                _gt.DeleteInsertLab(@"IF OBJECT_ID('[V_In_Diag_Due_Coll_List]', 'V') IS NOT NULL   DROP VIEW [V_In_Diag_Due_Coll_List]; ");
                _gt.DeleteInsertLab(@"IF OBJECT_ID('[V_in_Daily_Collection_List]', 'V') IS NOT NULL   DROP VIEW [V_in_Daily_Collection_List]; ");
                _gt.DeleteInsertLab(@"IF OBJECT_ID('[V_in_Counted_List_Of_Tested_Item]', 'V') IS NOT NULL   DROP VIEW [V_in_Counted_List_Of_Tested_Item]; ");
                _gt.DeleteInsertLab(@"IF OBJECT_ID('[V_IN_BED_WITH_DEPT]', 'V') IS NOT NULL   DROP VIEW [V_IN_BED_WITH_DEPT] ; ");
                _gt.DeleteInsertLab(@"IF OBJECT_ID('[V_Advance_Collection_List]', 'V') IS NOT NULL   DROP VIEW [V_Advance_Collection_List] ; ");

                
                _gt.DeleteInsertLab(@"IF OBJECT_ID('[V_Admission_List]', 'V') IS NOT NULL   DROP VIEW [V_Admission_List];");
                _gt.DeleteInsertLab(@"IF OBJECT_ID('[V_Due_Invoice_List]', 'V') IS NOT NULL   DROP VIEW [V_Due_Invoice_List];");
                _gt.DeleteInsertLab(@"IF OBJECT_ID('[V_in_DueList]', 'V') IS NOT NULL   DROP VIEW [V_in_DueList]; ");
                _gt.DeleteInsertLab(@"IF OBJECT_ID('[V_in_Due_Collection_List]', 'V') IS NOT NULL   DROP VIEW [V_in_Due_Collection_List]; ");

                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_IN_BED_WITH_DEPT] AS
SELECT a.Id,a.Name,a.Floor,a.BedType,a.Charge,a.SrvCharge,a.PostedBy,a.DeptId ,b.Name AS DeptName,a.BookStatus
FROM tb_IN_BED a INNER JOIN tb_IN_DEPARTMENT b ON a.DeptId=b.Id
");

                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_Indoor_Bill]
AS
SELECT a.Id As BillId, a.BillNo,a.BillDate,a.BillTime,a.AdmId,b.AdmNo,b.PtName,b.PtAge,b.PtSex,b.PtAddress,c.DeptName,c.DeptId,a.TotalAmt,a.TotalDiscount,a.AdvanceAmt,a.PaidAmt,a.Remarks,a.PostedBy,d.Charge,d.Unit,d.TotCharge,d.TestId,d.LessAmt,d.TestName, CASE WHEN d.DrId!=0 THEN '(B).Doctor' ELSE '(A).Hospital' END AS GroupName
FROM tb_IN_BILL_MASTER a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id
INNER JOIN V_IN_BED_WITH_DEPT c ON a.BedId=c.Id
INNER JOIN tb_in_BILL_DETAIL d ON a.Id=d.MasterId");
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_in_TestAdd_Bill]
AS
SELECT a.TrNo AS BillNo,a.TrDate AS  BillDate,b.PtName As PatientName,b.ContactNo AS  MobileNo,b.PtAddress,a.TestId,a.TestName,
a.Charge,a.Unit,a.TotCharge,a.Remarks,a.PostedBy,c.Name AS Bed,c.DeptName,d.Name AS UnderDrName
FROM tb_in_TEST_ADD_LIST a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id 
INNER JOIN V_IN_BED_WITH_DEPT c ON a.BedId=c.Id
INNER JOIN tb_DOCTOR d ON b.UnderDrId=d.Id
");


                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_Admission_List]
AS
SELECT a.*,b.Name AS RefDrName,c.Name AS UnderDrName ,d.Name AS BedName,d.Floor,d.DeptName
FROM tb_in_ADMISSION a 
INNER JOIN tb_DOCTOR b ON a.RefId=b.Id
INNER JOIN tb_DOCTOR c ON a.UnderDrId=c.Id
INNER JOIN V_IN_BED_WITH_DEPT d ON a.BedId=d.Id
");
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_In_Diag_Due_Coll_List]
AS
SELECT a.TrNo,a.TrDate,b.PtName AS  PatientName,b.ContactNo AS  MobileNo,a.TotalAmt,a.LessAmt,a.PostedBy,b.UnderDrId AS ConsDrId,c.Name AS ConsDrName,b.DeptName,b.BedName,a.AdmId
FROM tb_DUE_COLL a 
INNER JOIN V_Admission_List b ON a.AdmId=b.id
INNER JOIN tb_Doctor c ON b.UnderDrId=c.Id 
WHERE a.AdmId<>0
");
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_in_Daily_Collection_List]
AS
SELECT BillNo AS  TrNo,BillDate AS  TrDate,a.Id AS MasterId,a.BillNo,a.BillDate,b.PtName AS  PatientName,a.PostedBy,a.TotalAmt AS  SalesAmt,
a.TotalDiscount AS  LessAmt,a.PaidAmt AS  CollAmt,'ReleaseTimeCollection'AS Status 
FROM tb_IN_BILL_MASTER a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id
");
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_in_Counted_List_Of_Tested_Item]
AS
SELECT c.Name AS TestName,a.TestId, COUNT(TestId) AS NoOfTest, SUM(a.Charge) AS Charge,c.SubProjectId,'Procedure' AS GroupName,b.BillDate
FROM tb_in_BILL_DETAIL a INNER JOIN tb_IN_BILL_MASTER B on a.MasterId=b.id
INNER JOIN tb_TestChart  c ON a.TestId=c.Id
GROUP BY c.Name,a.TestId,c.SubProjectId,b.BillDate
");

                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_Advance_Collection_List]
AS
SELECT a.TrNo,a.TrDate,a.Amount,a.PostedBy AS UserName ,b.Id,b.DeptId,b.RegId,b.AdmNo,b.AdmDate,b.AdmTime,b.PtName,b.ContactNo,b.PtAge,b.PtSex,b.PtAddress,b.ChiefComplain,b.RefId,b.RefDrName,b.UnderDrId,b.UnderDrName,b.BedName,b.Floor
FROM tb_in_ADVANCE_COLLECTION a INNER JOIN V_Admission_List b ON a.AdmId=b.Id
");


                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_Due_Invoice_List]
AS
SELECT a.MasterId,b.BillNo,b.BillDate,b.PatientName,b.Age,b.Sex,b.MobileNo,c.Name As ConsDrName,c.Id AS ConsDrId,
Isnull(SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt),0) AS Balance,b.TotalAmt AS BillAmt,b.RefDrId,d.Name AS RefDrName,b.AdmId
FROM tb_BILL_LEDGER a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id
INNER JOIN tb_Doctor c ON b.UnderDrId=c.Id
INNER JOIN tb_Doctor d ON b.RefDrId=d.Id 
GROUP BY a.MasterId,b.PatientName,b.Age,b.Sex,b.MobileNo,b.BillDate,b.BillNo,c.Name,c.Id,b.TotalAmt,b.RefDrId,d.Name,b.AdmId
HAVING SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt)>0
");
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_in_DueList]
AS
SELECT b.AdmNo,b.PtName,b.PtSex,b.PtAge,b.ContactNo, a.AdmId,a.BedId,c.Name AS Bed,c.DeptName,c.DeptId, b.RefId ,d.Name AS RefDrName,b.UnderDrId,e.Name AS UnderDrName,SUM(a.TotCharge-a.LessAmt-a.CollAmt-a.ExtraLessAmt-(-a.RtnAmt)) AS DueAmt,f.BillDate AS ReleaseDate,f.BillNo
FROM tb_in_PATIENT_LEDGER a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id
INNER JOIN V_IN_BED_WITH_DEPT c ON a.BedId=c.Id
INNER JOIN tb_DOCTOR d ON b.RefId=d.Id
INNER JOIN tb_DOCTOR e ON b.UnderDrId=e.Id
INNER JOIN tb_IN_BILL_MASTER f ON b.id=f.AdmId
GROUP BY a.AdmId,a.BedId,c.Name,c.DeptName,c.DeptId, b.RefId ,d.Name,b.UnderDrId,e.Name,b.AdmNo,b.PtName,b.PtSex,b.PtAge,b.ContactNo,f.BillDate,f.BillNo
HAVING SUM(a.TotCharge-a.LessAmt-a.CollAmt-a.ExtraLessAmt-(-a.RtnAmt))>0
");
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_in_Due_Collection_List]
AS
SELECT a.TrNo,a.TrDate,a.AdmId ,b.PtName AS  PatientName,b.ContactNo AS MobileNo,a.TotalAmt,a.LessAmt,a.PostedBy,b.UnderDrId AS ConsDrId,c.Name AS ConsDrName,a.Remarks
FROM tb_DUE_COLL a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.id
INNER JOIN tb_Doctor c ON b.UnderDrId=c.Id
");

                
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }


        }

        private void button31_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab("ALTER TABLE tb_DOCTOR_LEDGER ALTER COLUMN ModuleName Nvarchar(20)");
                MessageBox.Show(@"Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {

           
            if (_gt.FnSeekRecordNewLab("tb_ph_ITEM", "COUNT(Id)>0"))
            {
                return;
            }
            
            
            try
            {
                _gt.DeleteInsertLab("ALTER TABLE tb_MASTER_INFO ADD UsePharmacy bit not null default 0");
                MessageBox.Show(@"Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }



            try
            {
                _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_ph_SUPPLIER', 'U') IS NOT NULL   DROP TABLE tb_ph_SUPPLIER");
                _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_ph_ITEM', 'U') IS NOT NULL   DROP TABLE tb_ph_ITEM");
                _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_ph_ITEM_GROUP', 'U') IS NOT NULL   DROP TABLE tb_ph_ITEM_GROUP");
                _gt.DeleteInsertLab(@"IF OBJECT_ID('V_ph_ITEM_LIST', 'U') IS NOT NULL   DROP VIEW V_ph_ITEM_LIST");



                //

                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ph_SUPPLIER](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[ContactNo] [nvarchar](max) NOT NULL,
	[EntryDate] [datetime] NOT NULL  DEFAULT (getdate())
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");


                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ph_ITEM](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GroupId] [int] NOT NULL,
	[SuppId] [int] NOT NULL,
	[Name] [nvarchar](400) NOT NULL,
	[GenericName] [nvarchar](max) NOT NULL DEFAULT (''),
	[SalePrice] [float] NOT NULL DEFAULT ((0)),
	[WholeSalePrice] [float] NOT NULL DEFAULT ((0)),
	[ReOrderQty] [int] NOT NULL DEFAULT ((0)),
	[IsDiscItem] [int] NOT NULL DEFAULT ((1)),
	[EntryDate] [datetime] NOT NULL DEFAULT (getdate()),
	[PostedBy] [nvarchar](max) NOT NULL,
	[LastUpdateDtls] [nvarchar](max) NOT NULL,
	[PcName] [nvarchar](max) NOT NULL,
	[IpAddress] [nvarchar](max) NOT NULL,
	[LastPPrice] [float] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_tb_ph_ITEM] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
");



                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ph_ITEM_GROUP](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");




                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_ph_ITEM_LIST] AS
SELECT a.Id,a.GroupId, a.SuppId, a.Name, a.GenericName, a.SalePrice, a.WholeSalePrice, a.ReOrderQty, a.IsDiscItem,b.Name AS SuppName,c.Name AS GroupName
FROM tb_ph_ITEM a INNER JOIN tb_ph_Supplier b ON a.SuppId=b.Id
INNER JOIN tb_ph_ITEM_GROUP c ON a.GroupId=c.Id
");






                MessageBox.Show(@"Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }

        private void button24_Click_1(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab("ALTER TABLE tb_MASTER_INFO ADD IsCheckPrintNo BIT NOT NULL DEFAULT 0");
                MessageBox.Show(@"Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            try
            {
                _gt.DeleteInsertLab("ALTER TABLE tb_BILL_DETAIL ADD BillTimeLess FLOAT NOT NULL DEFAULT 0");
                MessageBox.Show(@"Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            try
            {
                _gt.DeleteInsertLab(@"ALTER PROC [dbo].[Sp_Get_InvoicePrint] (@invNo int,@groupName varchar(500))   AS
IF	LEN(@groupName)=0
BEGIN
SELECT a.Id AS MasterId,a.BillNo,a.BillDate,a.BillTime, a.PatientName,a.MobileNo,a.Address,a.Age,a.Sex,c.Name AS RefDr,d.Name As ConsDr,a.TotalAmt,a.LessAmt,a.CollAmt,a.LessFrom ,a.LessType,a.LessPc, a.PostedBy,b.Charge,e.Name AS TestName,a.Remarks,Isnull(f.TotalAmt,0) AS DueColl,Isnull(f.LessAmt,0) AS DueLess,f.LessFrom AS DueLessFrom,f.LessPc AS DueLessPc,f.LessType AS DueLessType,a.RefDrId,a.UnderDrId,b.TestId,b.HnrAmt,b.TotHnrAmt,b.DrLess,CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END  AS SubProjectName,a.DeliveryDate,a.DeliveryNumber,a.DeliveryTimeAmPm,b.BillTimeLess
FROM tb_BILL_MASTER a LEFT JOIN tb_BILL_DETAIL b ON a.Id=b.MasterId
LEFT JOIN tb_Doctor c ON a.RefDrId=c.Id
LEFT JOIN tb_Doctor d ON a.UnderDrId=d.Id
LEFT JOIN tb_TestChart e ON b.TestId=e.Id 
LEFT JOIN tb_DUE_COLL f ON a.Id=f.MasterId 
WHERE a.Id=@invNo
END

IF	LEN(@groupName)>0
BEGIN
SELECT a.Id AS MasterId,a.BillNo,a.BillDate,a.BillTime, a.PatientName,a.MobileNo,a.Address,a.Age,a.Sex,c.Name AS RefDr,d.Name As ConsDr,a.TotalAmt,a.LessAmt,a.CollAmt,a.LessFrom ,a.LessType,a.LessPc, a.PostedBy,b.Charge,e.Name AS TestName,a.Remarks,Isnull(f.TotalAmt,0) AS DueColl,Isnull(f.LessAmt,0) AS DueLess,f.LessFrom AS DueLessFrom,f.LessPc AS DueLessPc,f.LessType AS DueLessType,a.RefDrId,a.UnderDrId,b.TestId,b.HnrAmt,b.TotHnrAmt,b.DrLess,CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END  AS SubProjectName,a.DeliveryDate,a.DeliveryNumber,a.DeliveryTimeAmPm,b.BillTimeLess
FROM tb_BILL_MASTER a LEFT JOIN tb_BILL_DETAIL b ON a.Id=b.MasterId
LEFT JOIN tb_Doctor c ON a.RefDrId=c.Id
LEFT JOIN tb_Doctor d ON a.UnderDrId=d.Id
LEFT JOIN tb_TestChart e ON b.TestId=e.Id 
LEFT JOIN tb_DUE_COLL f ON a.Id=f.MasterId 
WHERE a.Id=@invNo AND CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END = @groupName
END");
                MessageBox.Show(@"Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }



            

        }

        private void button25_Click_1(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_BILL_RETURN', 'U') IS NOT NULL   DROP TABLE tb_BILL_RETURN");

                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_BILL_RETURN](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrNo] [nvarchar](50) NOT NULL,
	[TrDate] [date] NOT NULL,
	[MasterId] [int] NOT NULL,
	[InvoiceValue] [float] NOT NULL  DEFAULT ((0)),
	[TotalReturn] [float] NOT NULL   DEFAULT ((0)),
	[ItemId] [int] NOT NULL,
	[Charge] [float] NOT NULL  DEFAULT ((0)),
	[LessAmt] [float] NOT NULL   DEFAULT ((0)),
	[RtnAmt] [float] NOT NULL   DEFAULT ((0)),
	[PostedBy] [nvarchar](500) NOT NULL,
	[EntryDate] [datetime] NOT NULL  DEFAULT (getdate()),
	[PcName] [nvarchar](50) NOT NULL,
	[IpAddress] [nvarchar](50) NOT NULL,
	[CashRtn] [float] NOT NULL DEFAULT ((0)),
	[DueAdjust] [float] NOT NULL   DEFAULT ((0))
) ON [PRIMARY]");




                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Daily_Collection_List]
AS
SELECT a.TrNo,a.TrDate,a.MasterId,b.BillNo,b.BillDate,b.PatientName,
a.PostedBy,a.SalesAmt,a.LessAmt,a.CollAmt,case when a.SalesAmt >0 THEN 'SaleTimeCollection' ELSE 'DueCollection' END AS Status 
FROM tb_BILL_LEDGER a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id 

UNION ALL

SELECT TOP 1 a.TrNo,a.TrDate,a.MasterId,b.BillNo,b.BillDate,b.PatientName,
a.PostedBy,0 AS SalesAmt,0 AS LessAmt,-(CashRtn) AS CollAmt,'CashReturn' AS Status 
FROM tb_BILL_RETURN a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id 
");








                
                MessageBox.Show(@"Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button33_Click(object sender, EventArgs e)
        {

            try 
	{




        _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_in_Daily_Collection_List]
AS
SELECT a.BillNo AS  TrNo,a.BillDate AS  TrDate,a.Id AS MasterId,b.PtName AS  PatientName,a.PostedBy,a.TotalAmt AS  SalesAmt,
a.TotalDiscount AS  LessAmt,a.PaidAmt AS  CollAmt,'ReleaseTimeCollection'AS Status 
FROM tb_IN_BILL_MASTER a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id
UNION ALL

SELECT a.TrNo,a.TrDate,a.Id AS MasterId,b.PtName AS  PatientName,a.PostedBy,0 AS  SalesAmt,
0 AS  LessAmt,a.Amount AS  CollAmt,'AdvanceCollection'AS Status 
FROM tb_in_ADVANCE_COLLECTION a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id

UNION ALL

SELECT a.TrNo,a.TrDate,a.Id AS MasterId,b.PtName AS  PatientName,a.PostedBy,0 AS  SalesAmt,
a.LessAmt,a.TotalAmt AS  CollAmt,'DueCollection'AS Status 
FROM tb_DUE_COLL a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id
");




              _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Admission_List]
AS
SELECT a.*,b.Name AS RefDrName,c.Name AS UnderDrName ,d.Name AS BedName,d.Floor,e.Name AS DeptName
FROM tb_in_ADMISSION a 
INNER JOIN tb_DOCTOR b ON a.RefId=b.Id
INNER JOIN tb_DOCTOR c ON a.UnderDrId=c.Id
INNER JOIN tb_in_BED d ON a.BedId=d.Id
INNER JOIN tb_in_DEPARTMENT e ON a.DeptId=e.Id");



              _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_in_DueList]
AS
SELECT b.AdmNo,b.PtName,b.PtSex,b.PtAge,b.ContactNo, a.AdmId,a.BedId,c.Name AS Bed,c.Name as  DeptName,b.DeptId, b.RefId ,d.Name AS RefDrName,b.UnderDrId,e.Name AS UnderDrName,SUM(a.TotCharge-a.LessAmt-a.CollAmt-a.ExtraLessAmt-(-a.RtnAmt)) AS DueAmt,f.BillDate AS ReleaseDate,f.BillNo
FROM tb_in_PATIENT_LEDGER a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id
INNER JOIN tb_in_DEPARTMENT c ON b.DeptId=c.Id
INNER JOIN tb_DOCTOR d ON b.RefId=d.Id
INNER JOIN tb_DOCTOR e ON b.UnderDrId=e.Id
INNER JOIN tb_IN_BILL_MASTER f ON b.id=f.AdmId
GROUP BY a.AdmId,a.BedId,c.Name,c.Name,b.DeptId, b.RefId ,d.Name,b.UnderDrId,e.Name,b.AdmNo,b.PtName,b.PtSex,b.PtAge,b.ContactNo,f.BillDate,f.BillNo
HAVING SUM(a.TotCharge-a.LessAmt-a.CollAmt-a.ExtraLessAmt-(-a.RtnAmt))>0
");

              _gt.DeleteInsertLab(@"
ALTER VIEW [dbo].[V_Indoor_Bill]
AS
SELECT a.Id As BillId, a.BillNo,a.BillDate,a.BillTime,a.AdmId,b.AdmNo,b.PtName,b.PtAge,b.PtSex,b.PtAddress,c.Name AS DeptName,b.DeptId,a.TotalAmt,a.TotalDiscount,a.AdvanceAmt,a.PaidAmt,a.Remarks,a.PostedBy,d.Charge,d.Unit,d.TotCharge,d.TestId,d.LessAmt,d.TestName, CASE WHEN d.DrId!=0 THEN '(B).Doctor' ELSE '(A).Hospital' END AS GroupName
FROM tb_IN_BILL_MASTER a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id
INNER JOIN tb_in_DEPARTMENT c ON b.DeptId=c.Id
INNER JOIN tb_in_BILL_DETAIL d ON a.Id=d.MasterId
");


              _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Admission_List]
AS
SELECT a.*,b.Name AS RefDrName,c.Name AS UnderDrName ,d.Name AS BedName,d.Floor,e.Name AS DeptName,d.Charge AS BedCharge
FROM tb_in_ADMISSION a 
INNER JOIN tb_DOCTOR b ON a.RefId=b.Id
INNER JOIN tb_DOCTOR c ON a.UnderDrId=c.Id
INNER JOIN tb_in_BED d ON a.BedId=d.Id
INNER JOIN tb_in_DEPARTMENT e ON a.DeptId=e.Id
");


                MessageBox.Show(@"Success");
                
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE tb_ph_ITEM ADD LastPPrice Float NOT NULL DEFAULT 0");

                MessageBox.Show(@"Success");

            }
            catch (Exception exception)
            {
                
                 MessageBox.Show(exception.Message);
            }
            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE tb_REPORT_DETAILS ADD BillNo varchar(20) NOT NULL DEFAULT ''");
            
                      MessageBox.Show(@"Success");

            }
            catch (Exception exception)
            {
                
                 MessageBox.Show(exception.Message);
            }

        }

        private void button35_Click(object sender, EventArgs e)
        {
            try
            {
               


                _gt.DeleteInsertLab(@"ALTER PROC [dbo].[Sp_Sales_ledger] (@dateFrom datetime,@dateTo dateTime) AS

BEGIN
SELECT 0 AS Id,'-' AS TrNo,@dateFrom AS TrDate,'-' AS BillNo,@dateFrom AS BillDate,0 AS MasterId ,'' AS PatientName,'' AS Address,'' AS MobileNo,0 AS SalesAmt,0 AS LessAmt,0 AS CollAmt,0 AS ReturnAmount,Isnull(SUM(a.SalesAmt-a.LessAmt-a.CollAmt-a.RtnAmt),0) AS BalAmt, 'O/B' AS Status 
FROM tb_BILL_LEDGER a WHERE  MasterId IN (SELECT Id FROM tb_BILL_MASTER WHERE AdmId=0) AND  a.TrDate < @dateFrom
UNION ALL
SELECT a.Id,a.TrNo,a.TrDate,b.BillNo,b.BillDate,a.MasterId ,b.PatientName ,b.Address,b.MobileNo,a.SalesAmt AS SalesAmt,a.LessAmt AS LessAmt,a.CollAmt AS CollAmt,a.RtnAmt,0 AS BalAmt,case when a.SalesAmt>0 Then 'Sales' ELSE 'DueColl' END AS Status 
FROM tb_BILL_LEDGER a LEFT JOIN tb_BILL_MASTER b ON a.MasterId=b.Id 
WHERE   a.TrDate BETWEEN @dateFrom AND @dateTo AND b.AdmId=0
END");
                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Due_Invoice_List]
AS
SELECT a.MasterId,b.BillNo,b.BillDate,b.PatientName,b.Age,b.Sex,b.MobileNo,c.Name As ConsDrName,c.Id AS ConsDrId,
Isnull(SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt),0) AS Balance,b.TotalAmt AS BillAmt,b.RefDrId,d.Name AS RefDrName,b.AdmId
FROM tb_BILL_LEDGER a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id
INNER JOIN tb_Doctor c ON b.UnderDrId=c.Id
INNER JOIN tb_Doctor d ON b.RefDrId=d.Id WHERE b.AdmId=0
GROUP BY a.MasterId,b.PatientName,b.Age,b.Sex,b.MobileNo,b.BillDate,b.BillNo,c.Name,c.Id,b.TotalAmt,b.RefDrId,d.Name,b.AdmId
HAVING SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt)>0");

                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Due_Collection_List]
AS
SELECT a.TrNo,a.TrDate,a.MasterId,b.PatientName,b.MobileNo,a.TotalAmt,a.LessAmt,a.PostedBy,b.UnderDrId AS ConsDrId,c.Name AS ConsDrName,b.BillDate,b.BillNo
FROM tb_DUE_COLL a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.id
INNER JOIN tb_Doctor c ON b.UnderDrId=c.Id WHERE b.AdmId=0");

                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Daily_Collection_List]
AS
SELECT a.TrNo,a.TrDate,a.MasterId,b.BillNo,b.BillDate,b.PatientName,
a.PostedBy,a.SalesAmt,a.LessAmt,a.CollAmt,case when a.SalesAmt >0 THEN 'SaleTimeCollection' ELSE 'DueCollection' END AS Status 
FROM tb_BILL_LEDGER a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id WHERE b.AdmId=0

UNION ALL

SELECT TOP 1 a.TrNo,a.TrDate,a.MasterId,b.BillNo,b.BillDate,b.PatientName,
a.PostedBy,0 AS SalesAmt,0 AS LessAmt,-(CashRtn) AS CollAmt,'CashReturn' AS Status 
FROM tb_BILL_RETURN a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id  WHERE b.AdmId=0
");


                _gt.DeleteInsertLab(@"IF OBJECT_ID('V_in_Daily_Collection_List', 'U') IS NOT NULL   DROP VIEW V_in_Daily_Collection_List");
                _gt.DeleteInsertLab(@"IF OBJECT_ID('V_in_DueList', 'U') IS NOT NULL   DROP VIEW V_in_DueList");

                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_in_Daily_Collection_List]
AS
SELECT a.BillNo AS  TrNo,a.BillDate AS  TrDate,a.Id AS MasterId,b.PtName AS  PatientName,a.PostedBy,a.TotalAmt AS  SalesAmt,
a.TotalDiscount AS  LessAmt,a.PaidAmt AS  CollAmt,'ReleaseTimeCollection'AS Status 
FROM tb_IN_BILL_MASTER a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id
UNION ALL

SELECT a.TrNo,a.TrDate,a.Id AS MasterId,b.PtName AS  PatientName,a.PostedBy,0 AS  SalesAmt,
0 AS  LessAmt,a.Amount AS  CollAmt,'AdvanceCollection'AS Status 
FROM tb_in_ADVANCE_COLLECTION a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id

UNION ALL

SELECT a.TrNo,a.TrDate,a.Id AS MasterId,b.PatientName,a.PostedBy,a.SalesAmt,
a.LessAmt,a.CollAmt,'DiagnosisCollection'AS Status 
FROM tb_BILL_LEDGER a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id WHERE b.AdmId<>0
");


                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_in_DueList]
AS
SELECT b.AdmNo,b.PtName,b.PtSex,b.PtAge,b.ContactNo, a.AdmId,a.BedId,c.Name AS Bed,c.Name as  DeptName,b.DeptId, b.RefId ,d.Name AS RefDrName,b.UnderDrId,e.Name AS UnderDrName,SUM(a.TotCharge-a.LessAmt-a.CollAmt-a.ExtraLessAmt-(-a.RtnAmt)) AS DueAmt,f.BillDate AS ReleaseDate,f.BillNo,'ReleaseTimeDue' AS Status
FROM tb_in_PATIENT_LEDGER a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id
INNER JOIN tb_in_DEPARTMENT c ON b.DeptId=c.Id
INNER JOIN tb_DOCTOR d ON b.RefId=d.Id
INNER JOIN tb_DOCTOR e ON b.UnderDrId=e.Id
INNER JOIN tb_IN_BILL_MASTER f ON b.id=f.AdmId
GROUP BY a.AdmId,a.BedId,c.Name,c.Name,b.DeptId, b.RefId ,d.Name,b.UnderDrId,e.Name,b.AdmNo,b.PtName,b.PtSex,b.PtAge,b.ContactNo,f.BillDate,f.BillNo
HAVING SUM(a.TotCharge-a.LessAmt-a.CollAmt-a.ExtraLessAmt-(-a.RtnAmt))>0

UNION ALL

SELECT c.AdmNo,b.PatientName AS PtName,b.Sex AS PtSex,b.Age AS PtAge,b.MobileNo AS  ContactNo, b.AdmId,c.BedId,c.BedName AS Bed,c.DeptName,c.DeptId, b.RefDrId AS RefId ,d.Name AS RefDrName,b.UnderDrId,e.Name AS UnderDrName,Isnull(SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt),0) AS DueAmt,GetDate() AS ReleaseDate,'' AS BillNo,'Diagnosis' AS Status
FROM tb_BILL_LEDGER a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id 
INNER JOIN V_Admission_List c ON b.AdmId=c.Id
INNER JOIN tb_DOCTOR d ON b.RefDrId=d.Id
INNER JOIN tb_DOCTOR e ON b.UnderDrId=e.Id
GROUP BY c.AdmNo,b.PatientName ,b.Sex,b.Age ,b.MobileNo , b.AdmId,c.BedId,c.BedName ,c.DeptName,c.DeptId, b.RefDrId  ,d.Name ,b.UnderDrId,e.Name 
HAVING SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt)>0");

                MessageBox.Show("Success");

            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"
ALTER PROC [dbo].[Sp_Get_InvoicePrint] (@invNo int,@groupName varchar(500))   AS
IF	LEN(@groupName)=0
BEGIN
SELECT a.Id AS MasterId,a.BillNo,a.BillDate,a.BillTime, a.PatientName,a.MobileNo,a.Address,a.Age,a.Sex,c.Name AS RefDr,d.Name As ConsDr,a.TotalAmt,a.LessAmt,a.CollAmt,a.LessFrom ,a.LessType,a.LessPc, a.PostedBy,b.Charge,e.Name AS TestName,a.Remarks,(SELECT ISNULL(SUM(TotalAmt),0) FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueColl,(SELECT ISNULL(SUM(LessAmt),0) FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLess,(SELECT TOP 1 LessFrom FROM tb_DUE_COLL WHERE MasterId=a.Id)  AS DueLessFrom,(SELECT TOP 1 LessPc FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLessPc,(SELECT TOP 1 LessType FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLessType,a.RefDrId,a.UnderDrId,b.TestId,b.HnrAmt,b.TotHnrAmt,b.DrLess,CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END  AS SubProjectName,a.DeliveryDate,a.DeliveryNumber,a.DeliveryTimeAmPm,b.BillTimeLess,b.GridRemarks
FROM tb_BILL_MASTER a LEFT JOIN tb_BILL_DETAIL b ON a.Id=b.MasterId
LEFT JOIN tb_Doctor c ON a.RefDrId=c.Id
LEFT JOIN tb_Doctor d ON a.UnderDrId=d.Id
LEFT JOIN tb_TestChart e ON b.TestId=e.Id 
WHERE a.Id=@invNo

END

IF	LEN(@groupName)>0
BEGIN
SELECT a.Id AS MasterId,a.BillNo,a.BillDate,a.BillTime, a.PatientName,a.MobileNo,a.Address,a.Age,a.Sex,c.Name AS RefDr,d.Name As ConsDr,a.TotalAmt,a.LessAmt,a.CollAmt,a.LessFrom ,a.LessType,a.LessPc, a.PostedBy,b.Charge,e.Name AS TestName,a.Remarks,(SELECT ISNULL(SUM(TotalAmt),0) FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueColl,(SELECT ISNULL(SUM(LessAmt),0) FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLess,(SELECT TOP 1 LessFrom FROM tb_DUE_COLL WHERE MasterId=a.Id)  AS DueLessFrom,(SELECT TOP 1 LessPc FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLessPc,(SELECT TOP 1 LessType FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLessType,a.RefDrId,a.UnderDrId,b.TestId,b.HnrAmt,b.TotHnrAmt,b.DrLess,CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END  AS SubProjectName,a.DeliveryDate,a.DeliveryNumber,a.DeliveryTimeAmPm,b.BillTimeLess,b.GridRemarks
FROM tb_BILL_MASTER a LEFT JOIN tb_BILL_DETAIL b ON a.Id=b.MasterId
LEFT JOIN tb_Doctor c ON a.RefDrId=c.Id
LEFT JOIN tb_Doctor d ON a.UnderDrId=d.Id
LEFT JOIN tb_TestChart e ON b.TestId=e.Id 
LEFT JOIN tb_DUE_COLL f ON a.Id=f.MasterId 
WHERE a.Id=@invNo AND CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END = @groupName
END
");


                

                MessageBox.Show("Success");

            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
            try
            {
                _gt.DeleteInsertLab(@"CREATE VIEW V_in_Draft_Bill AS
SELECT a.*,b.PtName,b.PtAge,b.PtAddress,b.AdmDate,b.ChiefComplain,b.BedName,b.ContactNo,b.UnderDrName,b.RefDrName 
FROM A_tb_IN_BILL_TMP a LEFT JOIN V_Admission_List b ON a.AdmId=b.Id");
                MessageBox.Show("Success");

            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }




            try
            {
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[A_tb_IN_BILL_TMP](
	[BillNo] [nvarchar](50) NULL,
	[BillDate] [datetime] NOT NULL,
	[BillTime] [nvarchar](50) NULL,
	[AdmId] [int] NOT NULL  DEFAULT ((0)),
	[RegId] [int] NOT NULL  DEFAULT ((0)),
	[BedId] [int] NOT NULL  DEFAULT ((0)),
	[TotalAmt] [float] NOT NULL  DEFAULT ((0)),
	[TotalDiscount] [float] NOT NULL  DEFAULT ((0)),
	[AdvanceAmt] [float] NOT NULL  DEFAULT ((0)),
	[PaidAmt] [float] NOT NULL  DEFAULT ((0)),
	[Remarks] [nvarchar](500) NULL,
	[PostedBy] [nvarchar](50) NULL,
	[TestId] [int] NOT NULL  DEFAULT ((0)),
	[TestName] [nvarchar](500) NULL,
	[Charge] [float] NOT NULL DEFAULT ((0)),
	[Unit] [int] NOT NULL  DEFAULT ((0)),
	[TotCharge] [float] NOT NULL  DEFAULT ((0)),
	[LessAmt] [float] NOT NULL DEFAULT ((0)),
	[ExtraLess] [float] NOT NULL  DEFAULT ((0))
) ON [PRIMARY]
");
                MessageBox.Show("Success");

            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }









        }

        private void button37_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_MPO](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL
) ON [PRIMARY]
");
                MessageBox.Show("Success");

            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE tb_Doctor ADD MpoId INT NOT NULL DEFAULT 0");
                MessageBox.Show("Success");

            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }


            try
            {
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_MpoContribution]
AS
SELECT b.BillDate,b.BillNo,b.MobileNo, a.MasterId,b.PatientName,b.Age,b.Sex,b.UnderDrId,c.Name AS UnderDrName, SUM(a.SalesAmt) AS SalesAmt,SUM(a.LessAmt) AS LessAmt,SUM(a.CollAmt) AS CollAmt,SUM(a.SalesAmt-a.LessAmt-a.CollAmt) AS DueAmt,Isnull(e.Name,'') AS MpoName,Isnull(e.id,0) AS MpoId 
FROM tb_BILL_LEDGER a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id
INNER JOIN tb_DOCTOR c ON b.UnderDrId=c.Id
INNER JOIN tb_DOCTOR d ON b.RefDrId=d.Id
LEFT JOIN tb_MPO e ON d.MpoId=e.Id
GROUP BY b.BillDate,b.BillNo,a.MasterId,b.PatientName,b.Age,b.Sex,b.UnderDrId,c.Name,b.MobileNo,e.id,e.Name");
                MessageBox.Show("Success");

            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }



            try
            {
                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_in_Daily_Collection_List]
AS
SELECT a.BillNo AS  TrNo,a.BillDate AS  TrDate,a.Id AS MasterId,b.PtName AS  PatientName,a.PostedBy,a.TotalAmt AS  SalesAmt,
a.TotalDiscount AS  LessAmt,a.PaidAmt AS  CollAmt,'ReleaseTimeCollection'AS Status 
FROM tb_IN_BILL_MASTER a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id
UNION ALL

SELECT a.TrNo,a.TrDate,a.Id AS MasterId,b.PtName AS  PatientName,a.PostedBy,0 AS  SalesAmt,
0 AS  LessAmt,a.Amount AS  CollAmt,'AdvanceCollection'AS Status 
FROM tb_in_ADVANCE_COLLECTION a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id

UNION ALL

SELECT a.TrNo,a.TrDate,a.Id AS MasterId,b.PtName AS  PatientName,a.PostedBy,0 AS  SalesAmt,
a.LessAmt,a.TotalAmt AS  CollAmt,'DueCollection'AS Status 
FROM tb_DUE_COLL a INNER JOIN tb_in_ADMISSION b ON a.AdmId=b.Id

");
                MessageBox.Show(@"Success");

            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }

        }

        private void button38_Click(object sender, EventArgs e)
        {

            try
            {
                _gt.DeleteInsertLab(@"ALTER PROC [dbo].[SP_GET_INVOICENO] (@StockType Int)   AS
IF @StockType=1
BEGIN
SELECT 'IN'+RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) + 
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo
FROM tb_BILL_MASTER  WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) AND BillDate>='2022-02-28' END 
IF @StockType=2 
BEGIN 
SELECT  'DC'+RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(TrNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_DUE_COLL  
WHERE YEAR(TrDate)=YEAR(GetDate()) AND MONTH(TrDate)=MONTH(GetDate()) AND TrDate>='2022-02-28' 
END
IF @StockType=3
BEGIN 
SELECT  'P'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_PURCHASE_MASTER  
WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) 
END
");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }


























            try
            {
                MessageBox.Show("Success");

                _gt.DeleteInsertLab(@"CREATE VIEW V_ph_Purchase_List
AS
SELECT a.BillNo,a.BillDate,a.ReceiptNo,a.ReceiptDate,a.SuppId,a.TotItem,a.TotAmt,a.NetAmt,a.TotVat,a.TotComision,a.TotLess,a.TotPaid,a.Remarks,a.PostedBy,b.Name AS SupplierName,b.Address,c.ItemId,d.Name AS ItemName,c.Qty,c.UnitPrice,c.BQty,c.UnitTotal,c.VatPc,c.VatAmt,c.ComPc,c.ComAmt,c.ExpireDate,c.Tp
FROM tb_ph_PURCHASE_MASTER a 
INNER JOIN tb_ph_SUPPLIER b ON a.SuppId=b.Id
INNER JOIN tb_ph_PURCHASE_DETAIL c ON a.Id=c.MasterId
INNER JOIN tb_ph_ITEM d ON c.ItemId=d.id
");
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
           
            
            
            try
            {
                MessageBox.Show("Success");

                _gt.DeleteInsertLab(@"");
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }





        }

        private void button39_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Success");

                _gt.DeleteInsertLab(@"ALTER TABLE tb_DUE_COLL ALTER COLUMN TrDate date");
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }


            try
            {
                MessageBox.Show("Success");

                _gt.DeleteInsertLab(@"ALTER TABLE tb_BILL_LEDGER ALTER COLUMN TrDate date");
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }

        }

        private void button40_Click(object sender, EventArgs e)
        {







            //
            _gt.DeleteInsertLab(@"IF OBJECT_ID('V_ph_Daily_Collection_List', 'U') IS NOT NULL   DROP VIEW V_ph_Daily_Collection_List");
            _gt.DeleteInsertLab(@"IF OBJECT_ID('V_StockList', 'U') IS NOT NULL   DROP VIEW V_StockList");
            _gt.DeleteInsertLab(@"IF OBJECT_ID('V_ph_Sales_BIll', 'U') IS NOT NULL   DROP VIEW V_ph_Sales_BIll");
            _gt.DeleteInsertLab(@"IF OBJECT_ID('V_ph_Gross_Profit', 'U') IS NOT NULL   DROP VIEW V_ph_Gross_Profit");
            


            _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_ph_PURCHASE_DETAIL', 'U') IS NOT NULL   DROP TABLE tb_ph_PURCHASE_DETAIL");
            _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_ph_PURCHASE_LEDGER', 'U') IS NOT NULL   DROP TABLE tb_ph_PURCHASE_LEDGER");
            _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_ph_PURCHASE_MASTER', 'U') IS NOT NULL   DROP TABLE tb_ph_PURCHASE_MASTER");
            _gt.DeleteInsertLab(@"IF OBJECT_ID('tb_ph_STOCK_LEDGER', 'U') IS NOT NULL   DROP TABLE tb_ph_STOCK_LEDGER");

            _gt.DeleteInsertLab(@"IF OBJECT_ID('V_ph_ITEM_LIST', 'U') IS NOT NULL   DROP VIEW V_ph_ITEM_LIST");
            _gt.DeleteInsertLab(@"IF OBJECT_ID('V_ph_Purchase_List', 'U') IS NOT NULL   DROP VIEW V_ph_Purchase_List");
            _gt.DeleteInsertLab(@"IF OBJECT_ID('V_ph_Supplier_Due_List', 'U') IS NOT NULL   DROP VIEW V_ph_Supplier_Due_List");


            _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ph_PURCHASE_MASTER](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BillNo] [nvarchar](50) NOT NULL,
	[BillDate] [date] NOT NULL  DEFAULT (getdate()),
	[ReceiptNo] [nvarchar](50) NULL,
	[ReceiptDate] [date] NOT NULL   DEFAULT (getdate()),
	[SuppId] [int] NOT NULL,
	[TotItem] [float] NOT NULL  DEFAULT ((0)),
	[TotAmt] [float] NOT NULL   DEFAULT ((0)),
	[NetAmt] [float] NOT NULL   DEFAULT ((0)),
	[TotVat] [float] NOT NULL   DEFAULT ((0)),
	[TotComision] [float] NOT NULL   DEFAULT ((0)),
	[TotLess] [float] NOT NULL   DEFAULT ((0)),
	[TotPaid] [float] NOT NULL   DEFAULT ((0)),
	[Remarks] [nvarchar](500) NOT NULL   DEFAULT (N'N/A'),
	[PostedBy] [nvarchar](50) NOT NULL,
	[EntryDate] [datetime] NOT NULL  DEFAULT (getdate()),
	[PcName] [nvarchar](150) NOT NULL,
	[IpAddress] [nvarchar](50) NOT NULL
) ON [PRIMARY]");

            _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ph_PURCHASE_DETAIL](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MasterId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[Qty] [float] NOT NULL  DEFAULT ((0)),
	[UnitPrice] [float] NOT NULL  DEFAULT ((0)),
	[BQty] [float] NOT NULL  DEFAULT ((0)),
	[UnitTotal] [float] NOT NULL   DEFAULT ((0)),
	[VatPc] [float] NOT NULL  DEFAULT ((0)),
	[VatAmt] [float] NOT NULL  DEFAULT ((0)),
	[ComPc] [float] NOT NULL DEFAULT ((0)),
	[ComAmt] [float] NOT NULL  DEFAULT ((0)),
	[Tp] [float] NOT NULL DEFAULT ((0)),
	[ExpireDate] [date] NOT NULL
) ON [PRIMARY]
");


            _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ph_STOCK_LEDGER](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MasterId] [int] NOT NULL,
	[TrNo] [nvarchar](50) NOT NULL,
	[TrDate] [date] NOT NULL,
	[ItemId] [int] NOT NULL,
	[PurchasePrice] [float] NOT NULL   DEFAULT ((0)),
	[SalesPrice] [float] NOT NULL  DEFAULT ((0)),
	[InQty] [float] NOT NULL DEFAULT ((0)),
	[OutQty] [float] NOT NULL  DEFAULT ((0)),
	[Module] [nvarchar](50) NOT NULL,
	[EntryDate] [datetime] NOT NULL  DEFAULT (getdate())
) ON [PRIMARY]");
            _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ph_PURCHASE_LEDGER](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrNo] [nvarchar](50) NOT NULL,
	[TrDate] [date] NOT NULL,
	[SuppId] [int] NOT NULL,
	[MasterId] [int] NOT NULL,
	[PurchaseAmt] [float] NOT NULL,
	[LessAmt] [float] NOT NULL   DEFAULT ((0)),
	[RtnAmt] [float] NOT NULL   DEFAULT ((0)),
	[PaymentAmt] [float] NOT NULL  DEFAULT ((0)),
	[PostedBy] [nvarchar](50) NOT NULL
) ON [PRIMARY]");
            _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_ph_Purchase_List]
AS
SELECT a.BillNo,a.BillDate,a.ReceiptNo,a.ReceiptDate,a.SuppId,a.TotItem,a.TotAmt,a.NetAmt,a.TotVat,a.TotComision,a.TotLess,a.TotPaid,a.Remarks,a.PostedBy,b.Name AS SupplierName,b.Address,c.ItemId,d.Name AS ItemName,c.Qty,c.UnitPrice,c.BQty,c.UnitTotal,c.VatPc,c.VatAmt,c.ComPc,c.ComAmt,c.ExpireDate,c.Tp
FROM tb_ph_PURCHASE_MASTER a 
INNER JOIN tb_ph_SUPPLIER b ON a.SuppId=b.Id
INNER JOIN tb_ph_PURCHASE_DETAIL c ON a.Id=c.MasterId
INNER JOIN tb_ph_ITEM d ON c.ItemId=d.id
");

            _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_ph_ITEM_LIST] AS
SELECT a.Id,a.GroupId, a.SuppId, a.Name, a.GenericName, a.SalePrice, a.WholeSalePrice, a.ReOrderQty, a.IsDiscItem,b.Name AS SuppName,c.Name AS GroupName
FROM tb_ph_ITEM a INNER JOIN tb_ph_Supplier b ON a.SuppId=b.Id
INNER JOIN tb_ph_ITEM_GROUP c ON a.GroupId=c.Id");

            _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_ph_Supplier_Due_List]
AS
SELECT a.SuppId,b.Name AS SuppName,b.AddRess,b.ContactNo, SUM(a.PurchaseAmt-a.LessAmt-a.RtnAmt-a.PaymentAmt) AS DueAmt,(SELECT TOP 1 TrDate FROM tb_ph_PURCHASE_LEDGER WHERE SuppId=a.SuppId Order by Id Desc) LastPaymentDate
FROM tb_ph_PURCHASE_LEDGER a INNER JOIN tb_ph_SUPPLIER b ON a.SuppId=b.Id
Group By a.SuppId,b.Name ,b.AddRess,b.ContactNo
HAVING SUM(a.PurchaseAmt-a.LessAmt-a.RtnAmt-a.PaymentAmt)<>0");


            _gt.DeleteInsertLab(@"ALTER PROC [dbo].[SP_GET_INVOICENO] (@StockType Int)   AS
IF @StockType=1
BEGIN
SELECT 'IN'+RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) + 
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo
FROM tb_BILL_MASTER  WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) AND BillDate>='2022-02-28' END IF @StockType=2 BEGIN SELECT  'DC'+RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(TrNo, 4))),0)+ 1), 4) AS InvNo FROM tb_DUE_COLL  WHERE YEAR(TrDate)=YEAR(GetDate()) AND MONTH(TrDate)=MONTH(GetDate()) AND TrDate>='2022-02-28' 
END
IF @StockType=3
BEGIN 
SELECT  'P'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_PURCHASE_MASTER  
WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) 
END");





            _gt.DeleteInsertLab(@"CREATE VIEW V_StockList
AS
SELECT a.ItemId,b.Name,b.GenericName,b.SalePrice, SUM(a.InQty-a.OutQty) AS BalQty
FROM tb_ph_STOCK_LEDGER a INNER JOIN  tb_ph_ITEM b ON a.ItemId=b.Id
GROUP BY a.ItemId,b.Name,b.GenericName,b.SalePrice
");





            _gt.DeleteInsertLab(@"ALTER PROC [dbo].[SP_GET_INVOICENO] (@StockType Int)   AS
IF @StockType=1
BEGIN
SELECT 'IN'+RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) + 
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo
FROM tb_BILL_MASTER  WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) AND BillDate>='2022-02-28' END IF @StockType=2 BEGIN SELECT  'DC'+RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(TrNo, 4))),0)+ 1), 4) AS InvNo FROM tb_DUE_COLL  WHERE YEAR(TrDate)=YEAR(GetDate()) AND MONTH(TrDate)=MONTH(GetDate()) AND TrDate>='2022-02-28' 
END
IF @StockType=3
BEGIN 
SELECT  'P'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_PURCHASE_MASTER  
WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) 
END
IF @StockType=4
BEGIN 
SELECT  'S'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_SALES_MASTER  
WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) 
END
");


            _gt.DeleteInsertLab(@"CREATE VIEW V_ph_Sales_BIll
AS
SELECT a.Id,a.BillNo,a.BillDate,a.TotAmt,a.TotVat,a.TotLess,a.TotPaid,a.PostedBy ,b.ItemId,c.Name AS ItemName, SUM(Qty) AS Qty,b.UnitPrice,b.UnitTotal
FROM tb_ph_SALES_MASTER a INNER JOIN tb_ph_SALES_DETAIL b ON a.Id=b.MasterId
INNER JOIN tb_ph_ITEM c ON b.ItemId=c.Id
GROUP BY a.Id,a.BillNo,a.BillDate,a.TotAmt,a.TotVat,a.TotLess,a.TotPaid,a.PostedBy ,b.ItemId,c.Name,b.UnitPrice,b.UnitTotal
");



            _gt.DeleteInsertLab(@"CREATE VIEW V_ph_Gross_Profit
AS
SELECT a.Id,a.BillNo,a.BillDate,a.PostedBy ,b.ItemId,c.Name AS ItemName, SUM(b.Qty*b.PurchasePrice) AS PurchasePrice, b.UnitPrice,b.Qty, b.UnitTotal AS SalesPrice,b.LessAmt
FROM tb_ph_SALES_MASTER a INNER JOIN tb_ph_SALES_DETAIL b ON a.Id=b.MasterId
INNER JOIN tb_ph_ITEM c ON b.ItemId=c.Id
GROUP BY a.Id,a.BillNo,a.BillDate,a.PostedBy ,b.ItemId,c.Name, b.UnitPrice,b.UnitTotal ,b.LessAmt,b.Qty
");








            _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_ph_Daily_Collection_List]
AS
SELECT a.BillNo AS  TrNo,a.BillDate AS  TrDate,a.Id AS MasterId,Isnull(b.Name,'Honourable Customer') AS  PatientName,a.PostedBy,a.TotAmt AS  SalesAmt,
a.TotLess AS  LessAmt,a.TotPaid AS  CollAmt,'SalesTimeCollection'AS Status 
FROM tb_ph_SALES_MASTER a LEFT JOIN tb_PATIENT b ON a.CustId=b.Id
UNION ALL

SELECT a.TrNo,a.TrDate,a.MasterId,Isnull(c.Name,'Honourable Customer') AS  PatientName,a.PostedBy,0 AS  SalesAmt,
a.LessAmt,a.CollectionAmt AS  CollAmt,'DueCollection'AS Status 
FROM tb_ph_DUE_COLL a INNER JOIN tb_ph_SALES_MASTER b ON a.MasterId=b.Id
INNER JOIN tb_PATIENT c ON b.CustId=c.Id
");






        }

        private void button41_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE tb_ph_SALES_MASTER ADD AdmId INT NOT NULL DEFAULT 0");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE tb_ph_DUE_COLL ADD CustId INT NOT NULL DEFAULT 0");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }


            try
            {
                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_ph_Sales_BIll]
AS
SELECT a.Id,a.BillNo,a.BillDate,a.TotAmt,a.TotVat,a.TotLess,a.TotPaid,a.PostedBy ,b.ItemId,c.Name AS ItemName, SUM(Qty) AS Qty,b.UnitPrice,b.UnitTotal,a.AdmId,Isnull(d.PtName,'') AS PtName,Isnull(d.PtSex,'') AS PtSex,Isnull(d.PtAge,'') AS PtAge,Isnull(d.PtAddress,'') AS PtAddress,Isnull(d.AdmNo,'') AS AdmNo
FROM tb_ph_SALES_MASTER a INNER JOIN tb_ph_SALES_DETAIL b ON a.Id=b.MasterId
INNER JOIN tb_ph_ITEM c ON b.ItemId=c.Id
LEFT JOIN tb_in_Admission d ON a.AdmId=d.Id
GROUP BY a.Id,a.BillNo,a.BillDate,a.TotAmt,a.TotVat,a.TotLess,a.TotPaid,a.PostedBy ,b.ItemId,c.Name,b.UnitPrice,b.UnitTotal,a.AdmId,d.PtName,d.PtSex,d.PtAge,d.PtAddress,d.AdmNo
");
                MessageBox.Show("Success");

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }




            try
            {
                _gt.DeleteInsertLab(@"ALTER PROC [dbo].[SP_GET_INVOICENO] (@StockType Int)   AS
IF @StockType=1
BEGIN
SELECT 'IN'+RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) + 
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo
FROM tb_BILL_MASTER  WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) AND BillDate>='2022-02-28' END IF @StockType=2 BEGIN SELECT  'DC'+RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(TrNo, 4))),0)+ 1), 4) AS InvNo FROM tb_DUE_COLL  WHERE YEAR(TrDate)=YEAR(GetDate()) AND MONTH(TrDate)=MONTH(GetDate()) AND TrDate>='2022-02-28' 
END
IF @StockType=3
BEGIN 
SELECT  'P'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_PURCHASE_MASTER  
WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) 
END
IF @StockType=4
BEGIN 
SELECT  'S'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_SALES_MASTER  
WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) 
END
IF @StockType=5
BEGIN 
SELECT  'D'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(TrNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_DUE_COLL
WHERE YEAR(TrDate)=YEAR(GetDate()) AND MONTH(TrDate)=MONTH(GetDate()) 
END
");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }



            try
            {
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_In_PH_Due_Coll_List]
AS
SELECT a.TrNo,a.TrDate,b.PtName AS  PatientName,b.ContactNo AS  MobileNo,a.CollectionAmt AS TotalAmt,a.LessAmt,a.PostedBy,b.UnderDrId AS ConsDrId,c.Name AS ConsDrName,b.DeptName,b.BedName,a.AdmId
FROM tb_ph_DUE_COLL a 
INNER JOIN V_Admission_List b ON a.AdmId=b.id
INNER JOIN tb_Doctor c ON b.UnderDrId=c.Id 
WHERE a.AdmId<>0
");
           
            MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }



            try
            {
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_ph_Sales_BIll_Reg_Customer]
AS
SELECT a.Id,a.BillNo,a.BillDate,a.TotAmt,a.TotVat,a.TotLess,a.TotPaid,a.PostedBy ,b.ItemId,c.Name AS ItemName, SUM(Qty) AS Qty,b.UnitPrice,b.UnitTotal,a.AdmId,Isnull(d.Name,'') AS PtName,Isnull(d.Sex,'') AS PtSex,'' AS PtAge,Isnull(d.Address,'') AS PtAddress,d.RegNo,a.CustId
FROM tb_ph_SALES_MASTER a INNER JOIN tb_ph_SALES_DETAIL b ON a.Id=b.MasterId
INNER JOIN tb_ph_ITEM c ON b.ItemId=c.Id
INNER JOIN tb_PATIENT d ON a.CustId=d.Id
GROUP BY a.Id,a.BillNo,a.BillDate,a.TotAmt,a.TotVat,a.TotLess,a.TotPaid,a.PostedBy ,b.ItemId,c.Name,b.UnitPrice,b.UnitTotal,a.AdmId,d.Name,d.Sex,d.Address,d.RegNo,a.CustId
");

                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }




        }

        private void button42_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Doctor_Honoriam]
AS
SELECT a.BillNo,a.BillDate ,b.Charge,b.TestId,b.HnrAmt,b.LessAmt As DrLess,b.HnrAmt AS TotHnrAmt,c.Name,a.RefDrId,d.Name AS RefDrName
FROM tb_BILL_MASTER a INNER JOIN tb_DOCTOR_LEDGER b ON a.Id=b.MasterId AND a.RefDrId=b.DrId
INNER JOIN tb_TESTCHART c ON b.TestId=c.Id
INNER JOIN tb_DOCTOR d ON a.RefDrId=d.Id WHERE d.TakeCom=1");
                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Doctor_Honoriam_UnderDrwise]
AS
SELECT a.BillNo,a.BillDate ,b.Charge,b.TestId,b.HnrAmt,b.LessAmt As DrLess,b.HnrAmt AS TotHnrAmt,c.Name,a.UnderDrId,d.Name AS RefDrName
FROM tb_BILL_MASTER a INNER JOIN tb_DOCTOR_LEDGER b ON a.Id=b.MasterId AND a.UnderDrId=b.DrId
INNER JOIN tb_TESTCHART c ON b.TestId=c.Id
INNER JOIN tb_DOCTOR d ON a.UnderDrId=d.Id WHERE d.TakeCom=1");


                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button43_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_ph_Due_Invoice_List]
AS
SELECT a.MasterId,b.BillNo,b.BillDate,c.PtName,c.PtSex,c.ContactNo,Isnull(SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt),0) AS Balance,
b.TotAmt AS BillAmt,'Indoor' AS Status
FROM tb_ph_SALES_LEDGER a INNER JOIN tb_ph_SALES_MASTER b ON a.MasterId=b.Id
INNER JOIN tb_in_ADMISSION c ON b.AdmId=c.Id WHERE b.AdmId<>0
GROUP BY a.MasterId,c.PtName,c.PtSex,c.ContactNo,b.BillDate,b.BillNo,b.TotAmt
HAVING SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt)>0
UNION ALL
SELECT a.MasterId,b.BillNo,b.BillDate,c.Name AS  PtName,c.Sex AS  PtSex,c.ContactNo,Isnull(SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt),0) AS Balance,
b.TotAmt AS BillAmt,'Register' AS Status
FROM tb_ph_SALES_LEDGER a INNER JOIN tb_ph_SALES_MASTER b ON a.MasterId=b.Id
INNER JOIN tb_PATIENT c ON b.CustId=c.Id WHERE b.AdmId=0 AND b.CustId<>0
GROUP BY a.MasterId,c.Name,c.Sex,c.ContactNo,b.BillDate,b.BillNo,b.TotAmt
HAVING SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt)>0
");


                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            try
            {
                _gt.DeleteInsertLab(@"
CREATE VIEW [dbo].[V_Reg_PH_Due_Coll_List]
AS
SELECT a.TrNo,a.TrDate,b.Name AS  PatientName,b.ContactNo AS  MobileNo,a.CollectionAmt AS TotalAmt,a.LessAmt,a.PostedBy,a.CustId
FROM tb_ph_DUE_COLL a 
INNER JOIN tb_PATIENT b ON a.CustId=b.id

WHERE a.CustId<>0
");


                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }



            try
            {
                _gt.DeleteInsertLab(@"
ALTER PROC [dbo].[SP_GET_INVOICENO] (@StockType Int)   AS
IF @StockType=1
BEGIN
SELECT 'IN'+RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) + 
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo
FROM tb_BILL_MASTER  WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) AND BillDate>='2022-02-28' END IF @StockType=2 BEGIN SELECT  'DC'+RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(TrNo, 4))),0)+ 1), 4) AS InvNo FROM tb_DUE_COLL  WHERE YEAR(TrDate)=YEAR(GetDate()) AND MONTH(TrDate)=MONTH(GetDate()) AND TrDate>='2022-02-28' 
END
IF @StockType=3
BEGIN 
SELECT  'P'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_PURCHASE_MASTER  
WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) 
END
IF @StockType=4
BEGIN 
SELECT  'S'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_SALES_MASTER  
WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) 
END
IF @StockType=5
BEGIN 
SELECT  'D'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(TrNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_DUE_COLL
WHERE YEAR(TrDate)=YEAR(GetDate()) AND MONTH(TrDate)=MONTH(GetDate()) 
END
IF @StockType=6
BEGIN 
SELECT  'D'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(TrNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_PURCHASE_DUE_PAYMENT
WHERE YEAR(TrDate)=YEAR(GetDate()) AND MONTH(TrDate)=MONTH(GetDate()) 
END");


                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button44_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"CREATE VIEW V_ph_Curr_Stock
AS
Select a.ItemId,b.Name AS ItemName,b.SuppId, c.Name AS SuppName,  AVG(a.PurchasePrice) AS PurchasePrice,SUM(a.InQty-a.OutQty) AS BalQty 
FROM tb_ph_STOCK_LEDGER a INNER JOIN  tb_ph_ITEM  b  
ON a.ItemId=b.Id  INNER JOIN tb_ph_SUPPLIER c ON b.SuppId=c.Id 
GROUP BY  a.ItemId,b.Name,b.SuppId, c.Name
");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button45_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"ALTER PROC [dbo].[SP_GET_INVOICENO] (@StockType Int)   AS
IF @StockType=1
BEGIN
SELECT 'IN'+RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) + 
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo
FROM tb_BILL_MASTER  WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) AND BillDate>='2022-02-28' END IF @StockType=2 BEGIN SELECT  'DC'+RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(TrNo, 4))),0)+ 1), 4) AS InvNo FROM tb_DUE_COLL  WHERE YEAR(TrDate)=YEAR(GetDate()) AND MONTH(TrDate)=MONTH(GetDate()) AND TrDate>='2022-02-28' 
END
IF @StockType=3
BEGIN 
SELECT  'P'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_PURCHASE_MASTER  
WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) 
END
IF @StockType=4
BEGIN 
SELECT  'S'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_SALES_MASTER  
WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) 
END
IF @StockType=5
BEGIN 
SELECT  'D'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(TrNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_DUE_COLL
WHERE YEAR(TrDate)=YEAR(GetDate()) AND MONTH(TrDate)=MONTH(GetDate()) 
END
IF @StockType=6
BEGIN 
SELECT  'D'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(TrNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_PURCHASE_DUE_PAYMENT
WHERE YEAR(TrDate)=YEAR(GetDate()) AND MONTH(TrDate)=MONTH(GetDate()) 
END
IF @StockType=7
BEGIN 
SELECT  'PR'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_PURCHASE_RETURN_MASTER
WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) 
END

IF @StockType=8
BEGIN 
SELECT  'SR'+
RIGHT('00' + Convert(varchar,YEAR(GETDATE())), 2) + 
RIGHT('00' + Convert(varchar,MONTH(Getdate())), 2) +  
RIGHT('0000'+ Convert(varchar,ISNULL(Max(Convert(integer, RIGHT(BillNo, 4))),0)+ 1), 4) AS InvNo 
FROM tb_ph_SALES_RETURN_MASTER
WHERE YEAR(BillDate)=YEAR(GetDate()) AND MONTH(BillDate)=MONTH(GetDate()) 
END");    
                MessageBox.Show("Success");



                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ph_PURCHASE_RETURN_MASTER](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BillNo] [nvarchar](50) NOT NULL,
	[BillDate] [date] NOT NULL,
	[SuppId] [int] NOT NULL,
	[TotItem] [float] NOT NULL,
	[TotAmt] [float] NOT NULL,
	[Remarks] [nvarchar](500) NOT NULL,
	[PostedBy] [nvarchar](50) NOT NULL,
	[EntryDate] [datetime] NOT NULL  DEFAULT (getdate()),
	[PcName] [nvarchar](150) NOT NULL,
	[IpAddress] [nvarchar](50) NOT NULL
) ON [PRIMARY]
");
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ph_PURCHASE_RETURN_DETAIL](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MasterId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[Qty] [float] NOT NULL,
	[UnitPrice] [float] NOT NULL,
	[UnitTotal] [float] NOT NULL,
	[ExpireDate] [date] NULL
) ON [PRIMARY]");


                MessageBox.Show("Success");






                _gt.DeleteInsertLab(@"CREATE VIEW [dbo].[V_ph_Purchase_Return_BIll]
AS
SELECT d.Name AS SuppName,d.Address AS SuppAddress,d.ContactNo, a.SuppId, a.Id,a.BillNo,a.BillDate,a.TotAmt,a.PostedBy ,b.ItemId,c.Name AS ItemName,b.Qty,b.UnitPrice,b.UnitTotal
FROM tb_ph_PURCHASE_RETURN_MASTER a INNER JOIN tb_ph_PURCHASE_RETURN_DETAIL b ON a.Id=b.MasterId
INNER JOIN tb_ph_ITEM c ON b.ItemId=c.Id
LEFT JOIN tb_ph_SUPPLIER d ON a.SuppId=d.Id
");





            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button46_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE tb_ph_STOCK_LEDGER ADD CustId Int Not Null Default 0;
                                      ALTER TABLE tb_ph_STOCK_LEDGER ADD AdmId Int Not Null Default 0;");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void button47_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ph_SALES_RETURN_MASTER](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BillNo] [nvarchar](50) NOT NULL,
	[BillDate] [date] NOT NULL,
	[RefNo] [nvarchar](50) NOT NULL,
	[RefDate] [date] NOT NULL  DEFAULT (getdate()),
	[CustId] [int] NOT NULL   DEFAULT ((0)),
	[TotItem] [float] NOT NULL,
	[TotAmt] [float] NOT NULL,
	[Remarks] [nvarchar](500) NULL,
	[PostedBy] [nvarchar](50) NOT NULL,
	[EntryDate] [datetime] NOT NULL  DEFAULT (getdate()),
	[PcName] [nvarchar](150) NOT NULL,
	[IpAddress] [nvarchar](50) NOT NULL,
	[AdmId] [int] NOT NULL
) ON [PRIMARY]");


                _gt.DeleteInsertLab(@"CREATE TABLE [dbo].[tb_ph_SALES_RETURN_DETAIL](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MasterId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[PurchasePrice] [float] NOT NULL,
	[Qty] [float] NOT NULL,
	[UnitPrice] [float] NOT NULL,
	[UnitTotal] [float] NOT NULL
) ON [PRIMARY]");


                MessageBox.Show("Success");

                _gt.DeleteInsertLab(@"ALTER TABLE tb_ph_SALES_RETURN_MASTER ADD TotLess Money NOT NULL DEFAULT 0");

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

        }

        private void button48_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_ph_Due_Invoice_List]
AS
SELECT a.AdmId,c.PtName,c.PtSex,c.ContactNo,Isnull(SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt),0) AS DueAmt,'Indoor' AS Status
FROM tb_ph_SALES_LEDGER a INNER JOIN tb_in_ADMISSION c ON a.AdmId=c.Id WHERE a.AdmId<>0
GROUP BY a.AdmId,c.PtName,c.PtSex,c.ContactNo
HAVING  SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt)>0
UNION ALL
SELECT c.Id AS AdmId,c.Name AS  PtName,c.Sex AS  PtSex,c.ContactNo,Isnull(SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt),0) AS DueAmt,'Register' AS Status
FROM tb_ph_SALES_LEDGER a INNER JOIN tb_ph_SALES_MASTER b ON a.MasterId=b.Id
INNER JOIN tb_PATIENT c ON b.CustId=c.Id WHERE b.AdmId=0 AND b.CustId<>0
GROUP BY c.Id,c.Name,c.Sex,c.ContactNo
HAVING SUM(a.SalesAmt-a.LessAmt-a.RtnAmt-a.CollAmt)>0");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }


            try
            {
                _gt.DeleteInsertLab(@"ALTER TABLE tb_ph_SALES_LEDGER ADD AdmId INT NOT NULL DEFAULT 0;
UPDATE tb_ph_SALES_LEDGER SET AdmId=b.AdmiD
FROM tb_ph_SALES_LEDGER a INNER JOIN tb_ph_SALES_MASTER b ON b.Id=a.MasterId");
                MessageBox.Show("Success");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }






        }

        private void button49_Click(object sender, EventArgs e)
        {
            _gt.DeleteInsertLab(@"
CREATE VIEW V_SUMMARY
AS
SELECT TrDate,'Outdoor' AS DeptName, SUM(CollAmt) AS CollAmt,0 AS Expense
FROM V_Daily_Collection_List 
GROUP BY TrDate
UNION ALL
SELECT TrDate,'Indoor',SUM(CollAmt) ,0
FROM V_in_Daily_Collection_List 
GROUP BY TrDate
UNION ALL
SELECT TrDate,'Pharmacy',ROUND(SUM(CollAmt) ,0),0
FROM V_ph_Daily_Collection_List
GROUP BY TrDate
UNION ALL
SELECT TrDate,'Expense',0,ROUND(SUM(Amount) ,0)
FROM tb_ac_EXPENSE_ENTRY
GROUP BY TrDate
");


            MessageBox.Show("Success");
        }

        private void button50_Click(object sender, EventArgs e)
        {
            _gt.DeleteInsertLab(@"CREATE VIEW V_Hnr_Summary
AS
SELECT a.MasterId,a.BillNo,b.BillDate,b.PatientName,b.RefDrId,c.Name As DrName,(STUFF((SELECT ',' +  CONVERT(varchar, Name) FROM V_TestName m WHERE m.MasterId=a.MasterId FOR XML PATH('')), 1, 1, '')) TestName ,
SUM(a.Charge) AS Charge,SUM(HnrAmt)  AS HnrAmt,SUM(DrLess) As DrLess,SUM(a.HnrAmt-a.DrLess) HnrToPay,ISNULL((SELECT Balance FROM V_Due_Invoice_List WHERE MasterId=a.MasterId),0) AS DueAmt
FROM tb_BILL_DETAIL a INNER JOIN tb_BILL_MASTER b ON a.MasterId=b.Id  INNER JOIN tb_DOCTOR c ON b.RefDrId=c.Id 
GROUP BY a.MasterId,a.BillNo,b.BillDate,b.PatientName,b.RefDrId,c.Name");
        }

        private void button51_Click(object sender, EventArgs e)
        {
            _gt.DeleteInsertLab("ALTER TABLE tb_in_PATIENT_LEDGER ADD LessAdjust Float NOT NULL DEFAULT 0");
        }

        private void button52_Click(object sender, EventArgs e)
        {
            _gt.DeleteInsertLab("ALTER TABLE tb_ph_SALES_LEDGER ADD LessAdjust FLoat NOT NULL DEFAULT 0");
        }

        private void button53_Click(object sender, EventArgs e)
        {
            _gt.DeleteInsertLab(@"ALTER PROC [dbo].[Sp_Get_InvoicePrint] (@invNo int,@groupName varchar(500))   AS IF	LEN(@groupName)=0 
BEGIN
SELECT a.Id AS MasterId,a.BillNo,a.BillDate,a.BillTime, a.PatientName,a.MobileNo,a.Address,a.Age,a.Sex,c.Name AS RefDr,d.Name As ConsDr,a.TotalAmt,a.LessAmt,a.CollAmt,a.LessFrom ,a.LessType,a.LessPc, a.PostedBy,b.Charge,e.Name AS TestName,a.Remarks,(SELECT ISNULL(SUM(TotalAmt),0) FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueColl,(SELECT ISNULL(SUM(LessAmt),0) FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLess,(SELECT TOP 1 LessFrom FROM tb_DUE_COLL WHERE MasterId=a.Id)  AS DueLessFrom,(SELECT TOP 1 LessPc FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLessPc,(SELECT TOP 1 LessType FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLessType,a.RefDrId,a.UnderDrId,b.TestId,b.HnrAmt,b.TotHnrAmt,b.DrLess,CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END  AS SubProjectName,a.DeliveryDate,a.DeliveryNumber,a.DeliveryTimeAmPm,b.BillTimeLess,b.GridRemarks,a.AdmId
FROM tb_BILL_MASTER a LEFT JOIN tb_BILL_DETAIL b ON a.Id=b.MasterId
LEFT JOIN tb_Doctor c ON a.RefDrId=c.Id
LEFT JOIN tb_Doctor d ON a.UnderDrId=d.Id
LEFT JOIN tb_TestChart e ON b.TestId=e.Id 
WHERE a.Id=@invNo

END

IF	LEN(@groupName)>0
BEGIN
SELECT a.Id AS MasterId,a.BillNo,a.BillDate,a.BillTime, a.PatientName,a.MobileNo,a.Address,a.Age,a.Sex,c.Name AS RefDr,d.Name As ConsDr,a.TotalAmt,a.LessAmt,a.CollAmt,a.LessFrom ,a.LessType,a.LessPc, a.PostedBy,b.Charge,e.Name AS TestName,a.Remarks,(SELECT ISNULL(SUM(TotalAmt),0) FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueColl,(SELECT ISNULL(SUM(LessAmt),0) FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLess,(SELECT TOP 1 LessFrom FROM tb_DUE_COLL WHERE MasterId=a.Id)  AS DueLessFrom,(SELECT TOP 1 LessPc FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLessPc,(SELECT TOP 1 LessType FROM tb_DUE_COLL WHERE MasterId=a.Id) AS DueLessType,a.RefDrId,a.UnderDrId,b.TestId,b.HnrAmt,b.TotHnrAmt,b.DrLess,CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END  AS SubProjectName,a.DeliveryDate,a.DeliveryNumber,a.DeliveryTimeAmPm,b.BillTimeLess,b.GridRemarks,a.AdmId
FROM tb_BILL_MASTER a LEFT JOIN tb_BILL_DETAIL b ON a.Id=b.MasterId
LEFT JOIN tb_Doctor c ON a.RefDrId=c.Id
LEFT JOIN tb_Doctor d ON a.UnderDrId=d.Id
LEFT JOIN tb_TestChart e ON b.TestId=e.Id 
LEFT JOIN tb_DUE_COLL f ON a.Id=f.MasterId 
WHERE a.Id=@invNo AND CASE WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='Usg' THEN 'USG' WHEN (SELECT Name FROM tb_SubProject WHERE Id=e.SubProjectId)='X-Ray' THEN 'X-RAY' ELSE 'PATHOLOGY' END = @groupName
END");



            _gt.DeleteInsertLab(@"ALTER VIEW [dbo].[V_Admission_List]
AS
SELECT a.*,ISNULL(b.Name,'') AS RefDrName,ISNULL(c.Name,'') AS UnderDrName ,ISNULL(d.Name,'') AS BedName,ISNULL(d.BedType,'') AS BedType,ISNULL(d.Floor,'') AS [Floor],ISNULL(e.Name,'') AS DeptName,ISNULL(d.Charge,0) AS BedCharge
FROM tb_in_ADMISSION a 
LEFT JOIN tb_DOCTOR b ON a.RefId=b.Id
LEFT JOIN tb_DOCTOR c ON a.UnderDrId=c.Id
LEFT JOIN tb_in_BED d ON a.BedId=d.Id
LEFT JOIN tb_in_DEPARTMENT e ON a.DeptId=e.Id

");










        }


        
    }
}
