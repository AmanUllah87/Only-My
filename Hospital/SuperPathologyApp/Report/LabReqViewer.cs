using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
 
using System.Windows.Forms;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Report.DataSet;
using SuperPathologyApp.UI;
using CrystalDecisions.CrystalReports.Engine;

namespace SuperPathologyApp.Report
{
    public partial class LabReqViewer : Form
    {
        public LabReqViewer(string reportFileName, string query, string dateRange, string dataSetName,string title)
        {
            this._reportFileName = reportFileName;
            this._dateRange = dateRange;
            this._query = query;
            this._dataSetName = dataSetName;
            this._title= title;


            InitializeComponent();
        }


        readonly DbConnection _db=new DbConnection();
        private readonly string _reportFileName;
        private readonly string _dateRange;
        private readonly string _query;
        private readonly string _dataSetName;
        private readonly string _title;

        readonly ReportDocument _rprt = new ReportDocument();

      

        private void LabReqViewer_Load(object sender, EventArgs e)
        {
            try
            {
                string comName = _db.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ComName");
                string comAddress = _db.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "Address");

                _db.ConLab.Open();
                string path = Application.StartupPath;
                if (_title == "LabReport")
                {
                    path = path + @"\Report\File\Lab\";

                }
                else if (_title == "Accounts")
                {
                    path = path + @"\Report\File\Accounts\";
                }
                else 
                {
                    path = path + @"\Report\File\Diagnosis\";
                }
               
                
                _rprt.Load(path + "" + _reportFileName + ".rpt");
              //  _rprt.Load(path + "" + _reportFileName + ".rdlc");
                
                var cmd = new SqlCommand(_query, _db.ConLab);
                var sda = new SqlDataAdapter(cmd);
                var ds = new GroupReportDS();
                ds.EnforceConstraints = false;
                sda.Fill(ds, _dataSetName);
                _rprt.SetDataSource(ds);
                _rprt.SetParameterValue("lcComName", comName);
                _rprt.SetParameterValue("lcComAddress", comAddress);
                _rprt.SetParameterValue("lcDateRange", _dateRange);
                _rprt.SetParameterValue("lcTitle", _title);


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

        private void LabReqViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            _rprt.Dispose();
            crystalReportViewer1.Dispose();
        }

        private void LabReqViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            _rprt.Dispose();
            crystalReportViewer1.Dispose();
        }
    }
}
