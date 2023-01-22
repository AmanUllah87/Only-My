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
    public partial class AddiotionalReportViewer : Form
    {
        public AddiotionalReportViewer(string reportFileName,string type,string dateRange)
        {
            this._reportFileName = reportFileName;
            this._dateRange = dateRange;
            this._title = type;
            InitializeComponent();
        }


        readonly DbConnection _db=new DbConnection();
        private readonly string _reportFileName;
        private readonly string _dateRange;
        private readonly string _title;

        readonly ReportDocument _rprt=new ReportDocument();
        private void AddiotionalReportViewer_Load(object sender, EventArgs e)
        {
            try
            {
                _db.ConLab.Open();
                string path = Application.StartupPath;
                path = path + @"\Report\File\";

                _rprt.Load(path + "" + _reportFileName + ".rpt");
               // var cmd = new SqlCommand(SampleSearchUi.Query, _db.ConLab);
               // var sda = new SqlDataAdapter(cmd);
               // var ds = new SampleProcessDS();
              //  sda.Fill(ds, "A_TMP_Sample_Process_Tracking");
              //  _rprt.SetDataSource(ds);
                _db.ConLab.Close();

                _rprt.SetParameterValue("lcComName", _db.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ComName"));
                _rprt.SetParameterValue("lcComAddress", _db.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "Address"));
                _rprt.SetParameterValue("lcDateRange", _dateRange);
                _rprt.SetParameterValue("lcTitle", _title);
               
                
                crystalReportViewer1.ReportSource = _rprt;
                crystalReportViewer1.ShowGroupTreeButton = false;
                crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;

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
    }
}
