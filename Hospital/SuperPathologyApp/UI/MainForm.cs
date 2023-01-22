using System;
using System.Data;
using System.Windows.Forms;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.UI.Accounts;
using SuperPathologyApp.UI.Common;
using SuperPathologyApp.UI.Diagnosis;
using SuperPathologyApp.UI.Indoor;
using SuperPathologyApp.UI.Lab;
using SuperPathologyApp.UI.Pharmacy;
using SuperPathologyApp.UI.Reagent;

namespace SuperPathologyApp.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

 
     
        private void doctorInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new DoctorSetupUi();
            grp.Show();

        }

        readonly DbConnection _gt=new DbConnection();
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = "Super Pathology Management System for " + _gt.FncReturnFielValueLab("tb_MASTER_INFO","1=1","ComName");
                label2.Text = @"Logged In As: " + Hlp.UserName;
                string poweredBy = label1.Text;
                string lastUpdate = System.IO.File.GetLastWriteTime(Application.StartupPath + @"\SuperPath.exe").ToString();
                label1.Text = poweredBy + @"/Last Update: " + lastUpdate;
                GetUserPrivilege();

                btnStrickerPrint.Visible = _gt.FnSeekRecordNewLab("tb_MASTER_INFO","StrickerPrint=1");
                accountsToolStripMenuItem.Visible = !_gt.FnSeekRecordNewLab("tb_MASTER_INFO", "UseExpenseEntry=0");
                medicineToolStripMenuItem.Visible = !_gt.FnSeekRecordNewLab("tb_MASTER_INFO", "UsePharmacy=0");
              //  reagentToolStripMenuItem.Visible = false;


            }
            catch (Exception exception)
            {
               MessageBox.Show(exception.Message);
            }
        }

        private void GetUserPrivilege()
        {
            registrationMenu.Visible = false;
            billToolStripMenuItem1.Visible = false;
            dueCollectionToolStripMenuItem.Visible = false;
            reportToolStripMenuItem.Visible = false;
            doctorEntryMenu.Visible = false;
           // doctorHonoriumMenu.Visible = false;//Aman
            testMenu.Visible = false;
            reportCommonToolStripMenuItem.Visible = false;
            reportImagingToolStripMenuItem.Visible = false;
            reportIndividualToolStripMenuItem.Visible = false;
            setupMenu.Visible = false;
            userToolStripMenuItem.Visible = false;

            accountsToolStripMenuItem.Visible = false;
            adminToolStripMenuItem.Visible = false;
            accountsSetupToolStripMenuItem.Visible = false;
          //  accountsReportToolStripMenuItem1.Visible = false;
            toolStripMenuItem1.Visible = false;
            indoorToolStripMenuItem.Visible = false;


            button8.Enabled = false;
            button9.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            


            var ds = _gt.Search("SELECT ChildName,ParentName FROM tb_USER_PRIVILEGE WHERE UserName='"+ Hlp.UserName +"'");
            if (ds.Rows.Count > 0)
            {
                foreach (DataRow row in ds.Rows)
                {
                    string menuDb = row["ChildName"].ToString();
                    string parent = row["ParentName"].ToString();
                    
                    switch (menuDb)
                    {
                        case "Registration":
                            registrationMenu.Visible = true;
                            break;
                        case "Bill":
                            if (parent == "Indoor")
                            {
                                indoorToolStripMenuItem.Visible = true;
                                if (_gt.FnSeekRecordNewLab("tb_MASTER_INFO", "UseIndoor=0"))
                                {
                                    indoorToolStripMenuItem.Visible = false;    
                                }                            
                            }
                            else
                            {
                                billToolStripMenuItem1.Visible = true;
                                button8.Enabled = true;
                            }
                            break;
                        case "Due Collection":
                            dueCollectionToolStripMenuItem.Visible = true;
                            button9.Enabled = true; 
                            break;
                        case "Report":
                            if (parent == "Accounts")
                            {
                                accountsSetupToolStripMenuItem.Visible = true;
                            }
                            else
                            {
                                reportToolStripMenuItem.Visible = true;
                                button10.Enabled = true; 
                                
                            }

                            break;
                        case "Doctor":
                            doctorEntryMenu.Visible = true;
                            setupMenu.Visible = true;//Aman
                            break;
                        //case "Honouriam":
                        //    doctorHonoriumMenu.Visible = true;
                        //    setupMenu.Visible = true;//Aman
                        //    break;
                        case "Test":
                            testMenu.Visible = true;
                            break;
                        case "Report Common":
                            reportCommonToolStripMenuItem.Visible = true;
                            button11.Enabled = true; 
                            break;
                        case "Report Imaging":
                            reportImagingToolStripMenuItem.Visible = true;
                            break;
                        case "Report Individual":
                            reportIndividualToolStripMenuItem.Visible = true;
                            break;
                        case "Setup":
                            setupMenu.Visible = true;
                            toolStripMenuItem1.Visible = true;
                            break;
                        case "User":
                            userToolStripMenuItem.Visible = true;
                            break;
                        case "Admin":
                            adminToolStripMenuItem.Visible = true;
                            break;
                        case "Accounts":
                            accountsToolStripMenuItem.Visible = true;
                            break;
                        case "A.Setup":
                            accountsSetupToolStripMenuItem.Visible = true;
                            break;
                        case "Indoor":
                            indoorToolStripMenuItem.Visible = true;
                            break;
                        

                        default:
                            break;
                    }
                }
            }

        }

   
       

       
      

        private void parameterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var grp = new ParameterSetupUi();
            grp.Show();
        }



        private void masterCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new MasterSetupUi();
            grp.Show();
        }

        private void barcodePrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void parentTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

   


        private void sampleReceiveInLabToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void sampleCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new BarcodePrintUi();
            grp.Show();
        }

        private void reportProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void receiveInDeliveryCounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
           
        }

        private void reportDeliveredToPatientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void sampleCollection_Click(object sender, EventArgs e)
        {

        }

        private void reportPrintStripMenuItem_Click(object sender, EventArgs e)
        {
            Hlp.GroupName = "";
         
            Hlp.InvoiceNoComeFromIndividualSearch = "";            
            var grp = new ReportPrintUi();
            grp.Show();
        }

        private void micorbiologyReportPrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new ReportPrintMicrobiologyUi(0);
            grp.Show();
        }

    

        private void groupSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new VaqGroupSetupUi();
            grp.Show();
        }

        private void iPDReportSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void iPDReportReceiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hlp.IsNursStation = true;
            
        }

        private void individualSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void processTrackingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new SampleProcessTrackingUi();
            grp.Show();
        }

        private void mappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void outSampleIssueToolStripMenuItem_Click(object sender, EventArgs e)
        {
           

        }

        private void searchToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           
        }

        private void dueReportPrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var grp = new BedUi();
            grp.Show();

        }


 

        private void finalStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void verificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void updatePatientInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new UpdatePateintInfo();
            grp.Show();
        }

        private void pathologyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new DefaultResultSetupUi();
            grp.Show();
        }

        private void repeatTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void linkToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var grp = new ReportPrintMicrobiologyUi(0);
            grp.Show();
        }

     

        private void patientHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var grp = new PatientHistoryUi();
            //grp.Show();
        }


        private void groupReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

 
        private void reportPrintHba1CToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void labRequisitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

    

        private void channelSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void reportSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void parameterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var grp = new ParameterSetupUi();
            grp.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var grp = new DoctorSetupUiNew();
            grp.Show();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            var grp = new DoctorHonouriamSetupUi();
            grp.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var grp = new BarcodePrintUi();
            grp.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var grp = new TestChartUi();
            grp.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var grp = new DiagnosisDueCollUi();
            grp.Show();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            var grp = new DiagnosisReportUi();
            grp.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var grp = new DiagnosisBillUi();
            Hlp.FirstFormVal = "";
            grp.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var grp = new DiagnosisDueCollUi();
            grp.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Hlp.GroupName = "COMMON REPORT";
            var grp = new ReportPrintUi();
            grp.Show();
        }

        private void doctorToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var grp = new DoctorSetupUiNew();
            grp.Show();
        }

        private void honoriumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new DoctorHonouriamSetupUi();
            grp.Show();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new TestChartUi();
            grp.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var grp = new DiagnosisReportUi();
            grp.Show();
        }

        private void billToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var grp = new DiagnosisBillUi();
            grp.Show();
        }

        private void reportCommonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hlp.GroupName = "COMMON REPORT";
            var grp = new ReportPrintUi();
            grp.Show();
        }

        private void dueCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new DiagnosisDueCollUi();
            grp.Show();
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new DiagnosisReportUi();
            grp.Show();
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new UserAccessUi();
            grp.Show();
        }

        private void parameterToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            var grp = new ParameterSetupUi();
            grp.Show();
        }

        private void defaultResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new DefaultResultSetupUi();
            grp.Show();
        }

        private void doctorSealToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new DoctorSetupUi();
            grp.Show();
        }

        private void reportHeaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new VaqGroupSetupUi();
            grp.Show();
        }

        private void haematologyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hlp.GroupName = "HAEMATOLOGY";
            var grp = new ReportPrintUi();
            grp.Show();
        }

        private void biochemistryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hlp.GroupName = "BIOCHEMISTRY";
            var grp = new ReportPrintUi();
            grp.Show();

        }

        private void immunologyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hlp.GroupName = "IMMUNOLOGY";
            var grp = new ReportPrintUi();
            grp.Show();

        }

        private void serologyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hlp.GroupName = "SEROLOGY";
            var grp = new ReportPrintUi();
            grp.Show();

        }

        private void urineToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Hlp.GroupName = "URINE";
            var grp = new ReportPrintUi();
            grp.Show();

        }

        private void stoolToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Hlp.GroupName = "STOOL";
            var grp = new ReportPrintUi();
            grp.Show();

        }

        private void reportImagingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new ReportPrintImagingUi();
            grp.Show();
        }

        private void userwiseCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new UserwiseCollectionReportUi();
            grp.Show();
        }

        private void reagentMappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new ChannelSetupUi();
            grp.Show();
        }

        private void sampleStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new SampleProcessTrackingUi();
            grp.Show();
        }

        private void registrationMenu_Click(object sender, EventArgs e)
        {
            var grp = new PatientRegistrationUi();
            grp.Show();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void defaultCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new DefaultCommentSetupUi();
            grp.Show();
        }

        private void expenseEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new ExpenseSetup();
            grp.Show();
        }

        private void entryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void reportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var grp = new AccountsReportUi();
            grp.Show();


        }

        private void voucherBillDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new VoucherDeleteRequestUi();
            grp.Show();
        }

        private void expenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new ExpenseEntry();
            grp.Show();
        }

        private void incomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new IncomeEntry();
            grp.Show();
        }

        private void expireDateUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void microbiologyGrowthToolStripMenuItem_Click(object sender, EventArgs e)
        {
         //   Hlp.GroupName = "MICROBIOLOGY GROWTH";
            Hlp.GroupName = "MICROBIOLOGY";

            var grp = new ReportPrintMicroGrowthUi();

            grp.Show();
        }

        private void setupMenu_Click(object sender, EventArgs e)
        {

        }

        private void admissionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new AdmissionUi();
            grp.Show();
        }

        private void bedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new BedUi();
            grp.Show();
        }

        private void advanceCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new AdvanceCollectionUi();
            grp.Show();
        }

        private void releaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new ReleaseUi();
            grp.Show();
        }

        private void comisionUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new DigitalVdUi();
            grp.Show();
        }

        private void reportToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            var grp = new IndoorReportUi();
            grp.Show();
        }

        private void testEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new IndoorTestAddUi();
            grp.Show();
        }

        private void indoorPatientDueCollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new DiagnosisDueCollForIndoorUi();
            grp.Show();
        }

        private void dueCollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new IndoorDueCollUi();
            grp.Show();
        }

        private void supplierToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var grp = new SupplierUi();
            grp.Show();
        }

        private void supplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new ItemUi();
            grp.Show();
        }

        private void billReturnToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var grp = new DiagnosisBillReturnUi();
            grp.Show();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            var grp = new DiagnosisBillReturnUi();
            grp.Show();
        }

        private void purchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new MedicinePurchaseUi();
            grp.Show();
        }

        private void reportToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var grp = new StockRelatedReportUi();
            grp.Show();
        }

        private void reportAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new PharmacyReportUi();
            grp.Show();
        }

        private void billToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new MedicineSalesUi();
            grp.Show();
        }

        private void dueCollIndoorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new DueCollForIndoorUi();
            grp.Show();
        }

        private void dueCollOutdoorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new DueCollForRegisterPatientUi();
            grp.Show();
        }

        private void duePaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new SupplierDuePaymentUi();
            grp.Show();
        }

        private void purchaseReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new SupplierReturnUi();
            grp.Show();
        }

        private void billReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new SalesReturnUi();
            grp.Show();
        }

        private void itemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new ReagentUi();
            grp.Show();
        }

        private void supplierToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            var grp = new ReagentSupplierUi();
            grp.Show();
        }

        private void purchaseReceiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new ReagentPurchaseUi();
            grp.Show();
        }

        private void honouriamPaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new HonouriamPaymentUi();
            grp.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            var grp = new ReagentMachineParamSetupUi();
            grp.Show();
        }

        private void reagentStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var grp = new ReagentStockRelatedReportUi();
            grp.Show();
        }
    }
}
