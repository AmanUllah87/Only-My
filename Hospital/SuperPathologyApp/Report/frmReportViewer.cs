using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
 
using System.Windows.Forms;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Report.DataSet;
using SuperPathologyApp.Report.File;
using SuperPathologyApp.UI;
using CrystalDecisions.CrystalReports.Engine;

namespace SuperPathologyApp.Report
{
    public partial class FrmReportViewer : Form
    {
        private readonly string _reportFileName = "";
        private readonly string _rptQuery = "";
        private readonly string _rptDataSet = "";
       
     
        
        public FrmReportViewer(string reportFileName,string rptQuery)
        {
            InitializeComponent();
            this._reportFileName = reportFileName;
            this._rptQuery = rptQuery;
        }
        public FrmReportViewer(string reportFileName, string rptQuery,string dataSetName)
        {
            InitializeComponent();
            this._reportFileName = reportFileName;
            this._rptQuery = rptQuery;
            this._rptDataSet = _rptDataSet = dataSetName;
        }
        readonly ReportDocument _rprt = new ReportDocument();
        readonly DbConnection _db = new DbConnection();
     
        private void frmReportViewer_Load(object sender, EventArgs e)
        {
            try
            {
                string comName = _db.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ComName");
                string comAddress = _db.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "Address");


                _db.ConLab.Open();
                string path = Application.StartupPath;
                path = path + @"\Report\File\Lab\";

                _rprt.Load(path + "" + _reportFileName + ".rpt");
                var cmd = new SqlCommand(_rptQuery, _db.ConLab);
                var sda = new SqlDataAdapter(cmd);
                var ds = new DataSet1();
                sda.Fill(ds, _rptDataSet != "" ? _rptDataSet : "DT_LAB_REPORT_VIEW");
                _rprt.SetDataSource(ds);

                _rprt.SetParameterValue("lcComName", comName);
                _rprt.SetParameterValue("lcComAddress",comAddress );
                _rprt.SetParameterValue("lcDateRange", Hlp.UserName);
                _rprt.SetParameterValue("lcTitle", "");

                if (Hlp.AutoPrint)
                {
                    _rprt.PrintToPrinter(1, true, 0, 0);
                    Close();
                }
                else
                {
                    crystalReportViewer1.ReportSource = _rprt;
                    crystalReportViewer1.ShowGroupTreeButton = false;
                    crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
                    
                    crystalReportViewer1.ShowExportButton=true;
                }
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

        private void FrmReportViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            _rprt.Dispose();
            crystalReportViewer1.Dispose();
        }

        private void FrmReportViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            _rprt.Dispose();
            crystalReportViewer1.Dispose();
        }
    }
}
