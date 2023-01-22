using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SuperPathologyApp.Gateway.DB_Helper;

namespace SuperPathologyApp.UI
{
    public partial class VaqGroupSetupUi : Form
    {
        public VaqGroupSetupUi()
        {
            InitializeComponent();
        }
        readonly DbConnection _gt=new DbConnection();
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void VaqGroupSetupUi_Load(object sender, EventArgs e)
        {
            _gt.LoadComboBox("SELECT distinct 0 AS Id,ReportFileName AS Description FROM tb_TESTCHART WHERE LEN(ReportFileName)>0", groupNameComboBox);

        }

        private void groupNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //headerNameTextBox.Text = _gt.FncReturnFielValueLab("tb_Group", "Id=" + groupNameComboBox.SelectedValue + "","HeaderName");
        }

        private void groupNameComboBox_Leave(object sender, EventArgs e)
        {


        }

        private void groupNameComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (groupNameComboBox.SelectedIndex != 0)
                {
                    headerNameTextBox.Text = _gt.FncReturnFielValueLab("tb_REPORT_HEADER_DEFAULT", "ReportFileName='" + groupNameComboBox.Text  + "'", "HeaderName");
                    headerNameTextBox.Focus();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (_gt.FnSeekRecordNewLab("tb_REPORT_HEADER_DEFAULT", "ReportFileName='" + groupNameComboBox.Text + "'"))
                {
                    _gt.DeleteInsertLab("Update tb_REPORT_HEADER_DEFAULT SET HeaderName='" + headerNameTextBox.Text + "' WHERE ReportFileName='" + groupNameComboBox.Text + "'");
                }
                else
                {
                    _gt.DeleteInsertLab("INSERT INTO tb_REPORT_HEADER_DEFAULT (HeaderName,ReportFileName)VALUES('" + headerNameTextBox.Text + "','" + groupNameComboBox.Text + "')");
                
                }
                
                
                
                
                
                
                headerNameTextBox.Text = "";
                MessageBox.Show(@"Save Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
