using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;

namespace SuperPathologyApp.UI
{
    public partial class UpdatePateintInfo : Form
    {
        public UpdatePateintInfo()
        {
            InitializeComponent();
        }
        readonly DbConnection _gt=new DbConnection();
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }




        BarcodePrintGateway _gtBarcode=new BarcodePrintGateway();
        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Are you sure want to update patient details?", @"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }
            else
            {
                if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "'"))
                {
                    // _gt.DeleteInsertLab(@"INSERT INTO Update_Record_Of_Patient (BillNo, Name, DrName, Sex,  PostedBy)VALUES('" + FourDigitTextBox.Text + "', '" + lblPtName.Text + "', '" + lblDrName.Text  + "', '" + lblSex.Text  + "', '" + Hlp.UserName + "')");
                    _gt.DeleteInsertLab("UPDATE tb_BILL_MASTER SET PatientName='" + nameTextBox.Text + "',Sex='" + ptSexComboBox.Text + "',Age='" + ageTextBox.Text + "',Remarks='" + Remarks.Text + "',UnderDrId='" + drNameComboBox.SelectedValue + "' WHERE BillNo='" + billNoTextBox.Text + "' ");

                    MessageBox.Show(@"Update Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show(@"Invalid InvoiceNo Or Date", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }

        private void billNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "'"))
                {
                    invDateDateTimePicker.Value = Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "'", "BillDate"));
                    nameTextBox.Text = _gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "'", "PatientName");
                    ageTextBox.Text = _gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "'", "Age");
                    ptSexComboBox.Text = _gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "'", "Sex");
                    drNameComboBox.SelectedValue = _gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "'", "UnderDrId");
                    Remarks.Text = _gt.FncReturnFielValueLab("tb_BILL_MASTER", "BillNo='" + billNoTextBox.Text + "'", "Remarks");
                }
            }
        }

        private void billNoTextBox_Enter(object sender, EventArgs e)
        {
            billNoTextBox.BackColor = Hlp.EnterFocus();
        }

        private void billNoTextBox_Leave(object sender, EventArgs e)
        {
            billNoTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void UpdatePateintInfo_Load(object sender, EventArgs e)
        {
            billNoTextBox.Text ="IN"+ Hlp.GetServerDate().Year.ToString().Substring(2, 2) + Hlp.GetServerDate().Month.ToString().PadLeft(2, '0');
            billNoTextBox.Select(billNoTextBox.Text.Length, 0);
            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_DOCTOR Order By Id ", drNameComboBox);
        }
    }
}
