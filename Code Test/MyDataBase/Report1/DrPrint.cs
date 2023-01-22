using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using MyDataBase.DB_Helper;
using MyDataBase.Report1.DataSet;
using MyDataBase.Report1;


namespace MyDataBase.Report1
{
    public partial class DrPrint : Form
    {
        public DrPrint(string reportFileName, string query, string dataSetName)
       // public DrPrint(string reportFileName, string query, string dateRange, string dataSetName, string title)
        {
            
            this._reportFileName = reportFileName;
            //this._dateRange = dateRange;
            this._query = query;
            this._dataSetName = dataSetName;
          //  this._title= title;

             InitializeComponent();
           
        }


        readonly DbConnection _db=new DbConnection();
        private readonly string _reportFileName;
        private readonly string _dateRange;
        private readonly string _query;
        private readonly string _dataSetName;
        private readonly string _title;
        private readonly string _rptQuery = "";
         private readonly string _rptDataSet = "";
     

        readonly ReportDocument _rprt = new ReportDocument();
       
        private string p;
      

        private void DrPrint_Load(object sender, EventArgs e)
        {
            try
            {
                

                _db.ConLab.Open();

                string path = Application.StartupPath;
                path = path + @"\Report1\";

                _rprt.Load(path + "" + _reportFileName + ".rpt");
                var cmd = new SqlCommand(_rptQuery, _db.ConLab);
                var sda = new SqlDataAdapter(cmd);
                var ds = new DrPrintA();
                sda.Fill(ds, _rptDataSet != "" ? _rptDataSet : "DT_LAB_REPORT_VIEW");
                _rprt.SetDataSource(ds);

               

                

                
                    crystalReportViewer1.ReportSource = _rprt;
                    crystalReportViewer1.ShowGroupTreeButton = false;
                    crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;

                    crystalReportViewer1.ShowExportButton = true;
                
                _db.ConLab.Close();

            }
            catch (Exception exception)
            {
                if (_db.ConLab.State == ConnectionState.Open)
                {
                    _db.ConLab.Close();
                }
                MessageBox.Show(exception.Message);
            }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
            
            
           
        }


    
}
