using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
 
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Model;

namespace SuperPathologyApp.UI
{
    public partial class MasterSetupUi : Form
    {
        public MasterSetupUi()
        {
            InitializeComponent();
        }
        MasterSetupGateway aGateway=new MasterSetupGateway();
        private void button2_Click(object sender, EventArgs e)
        {
            Close();  
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var aModel=new MasterSetupModel();
            aModel.UseBarcodeForSample =Convert.ToInt32(useBarcodeForSampleCheckBox.Checked);
            aModel.ComName = comNameTextBox.Text;
            aModel.Address = addressTextBox.Text;
            aModel.ReportNo =Convert.ToDouble(reportNoTextBox.Text);
            aModel.IpdSampleNo = Convert.ToDouble(ipdSerialNoTextBox.Text);
            aModel.OpdSampleNo = Convert.ToDouble(opdSerialNoTextBox.Text);
            aModel.HistoSampleNo = Convert.ToDouble(histopathologySlTextBox.Text);
            


            string msg = aGateway.Save(aModel);
            MessageBox.Show(msg, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MasterSetupUi_Load(object sender, EventArgs e)
        {
            var aModel=  aGateway.GetCheckedValue();
            //useBarcodeForSampleCheckBox.Checked = aModel.UseBarcodeForSample==1;
            comNameTextBox.Text = aModel.ComName;
            addressTextBox.Text = aModel.Address;
            ipdSerialNoTextBox.Text = aModel.IpdSampleNo.ToString();
            opdSerialNoTextBox.Text = aModel.OpdSampleNo.ToString();
            reportNoTextBox.Text = aModel.ReportNo.ToString();
            histopathologySlTextBox.Text = aModel.HistoSampleNo.ToString();

        }
    }
}
