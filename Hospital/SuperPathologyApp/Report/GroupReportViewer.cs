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
    public partial class GroupReportViewer : Form
    {
        public GroupReportViewer(string reportFileName, string query, string dateRange,string dataSetName,string machineName)
        {
            this._reportFileName = reportFileName;
            this._dateRange = dateRange;
            this._query = query;
            this._dataSetName = dataSetName;
            _machineName = machineName;

            InitializeComponent();
        }


        readonly DbConnection _db=new DbConnection();
        private readonly string _reportFileName;
        private readonly string _dateRange;
        private readonly string _query;
        private readonly string _dataSetName;
        private readonly string _machineName;


        readonly ReportDocument _rprt=new ReportDocument();
      
        private void GroupReportViewer_Load(object sender, EventArgs e)
        {
          //  const string reportFileName = "GroupReport";
            try
            {
                string comName = _db.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ComName");
                string comAddress = _db.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "Address");

                _db.ConLab.Open();
                string path = Application.StartupPath;
                path = path + @"\Report\File\";
                _rprt.Load(path + "" + _reportFileName + ".rpt");
                var cmd = new SqlCommand(_query, _db.ConLab);
                var sda = new SqlDataAdapter(cmd);
                var ds = new GroupReportDS();
                sda.Fill(ds, _dataSetName);
                _rprt.SetDataSource(ds);
                _rprt.SetParameterValue("lcComName", comName);
                _rprt.SetParameterValue("lcComAddress", comAddress);
                _rprt.SetParameterValue("lcDateRange", _dateRange);
                _rprt.SetParameterValue("lcTitle", _machineName);



                crystalReportViewer1.ReportSource = _rprt;
                crystalReportViewer1.ShowGroupTreeButton = false;
                crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;

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

        private void GroupReportViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            _rprt.Dispose();
            crystalReportViewer1.Dispose();
        }

        private void GroupReportViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            _rprt.Dispose();
            crystalReportViewer1.Dispose();
        }
    }
}
