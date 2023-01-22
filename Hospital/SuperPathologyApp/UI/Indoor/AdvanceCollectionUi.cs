using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Gateway.Indoor;
using SuperPathologyApp.Model;
using SuperPathologyApp.Model.Indoor;
using SuperPathologyApp.Report;

namespace SuperPathologyApp.UI.Indoor
{
    public partial class AdvanceCollectionUi : Form
    {
        public AdvanceCollectionUi()
        {
            InitializeComponent();
        }

        readonly AdvanceGateway _gt = new AdvanceGateway();

        private void contactNoTextBox_Enter(object sender, EventArgs e)
        {
            contactNoTextBox.BackColor = Hlp.EnterFocus();
            HelpDataGridLoadByPatientId();
        }

        private void contactNoTextBox_Leave(object sender, EventArgs e)
        {
            contactNoTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void ptNameTextBox_Enter(object sender, EventArgs e)
        {
            ptNameTextBox.BackColor = Hlp.EnterFocus();
        }

        private void ptNameTextBox_Leave(object sender, EventArgs e)
        {
            ptNameTextBox.BackColor = Hlp.LeaveFocus();
        }

       
     
        private void HelpDataGridLoadByPatientId()
        {
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,AdmNo,AdmDate,PtName,ContactNo,BedName As Bed FROM V_Admission_List WHERE  ReleaseStatus=0 AND (convert(varchar,Id)+PtName+BedName+ContactNo)  LIKE '%"+ contactNoTextBox.Text +"%'");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 70;
            dataGridView1.Columns[2].Width = 90;
            dataGridView1.Columns[3].Width = 200;
            dataGridView1.Columns[4].Width = 90;
            dataGridView1.Columns[5].Width = 80;
        }
   

     

       

       

        private void AdmissionUi_Load(object sender, EventArgs e)
        {
            ClearText();

        }

        private void advanceAmtTextBox_Enter(object sender, EventArgs e)
        {
            advanceAmtTextBox.BackColor = Hlp.EnterFocus();
        }

        private void advanceAmtTextBox_Leave(object sender, EventArgs e)
        {
            advanceAmtTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CheckSuccess())
            {
                var aMdl = new AdmissionModel
                {
                  
                    AdmId =Convert.ToInt32(admIdTextBox.Text),
                    AdvanceAmt = Convert.ToDouble(advanceAmtTextBox.Text),
                };

                string msg = _gt.Save(aMdl, saveButton.Text);
                if (msg == _gt.SaveSuccessMessage)
                {

                    MessageBox.Show(_gt.SaveSuccessMessage, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearText();
                   
                    if (aMdl.AdvanceAmt>0)
                    {
                        OpenAdvanceBill(aMdl.AdvanceAmtTrNo);    
                    }
                }
                else
                {
                    MessageBox.Show(msg, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }

        private void OpenAdvanceBill(string admNo)
        {
            var dt = new IndoorReportViewer("AdvanceSlip", "SELECT * FROM V_Advance_Collection_List WHERE TrNo='" + admNo + "'", "Advance Receive", "V_Advance_Collection_List", "");
            dt.Show();
        }

        private void ClearText()
        {
            contactNoTextBox.Focus();
            contactNoTextBox.Text = "";
            ptNameTextBox.Text = "";
            addressTextBox.Text = "";
            admIdTextBox.Text = "0";
            saveButton.Text = "&Save";
        }

        private string _textInfo = "";

        private void contactNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
          


            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                _textInfo = "contact";
                dataGridView1.Focus();
            }
        }

        readonly PatientGateway _pt=new PatientGateway();
        private void HelpDataGridLoadByContactNo(DataGridView dg)
        {
            dg.DataSource = _pt.GetRegisterPatientList(contactNoTextBox.Text, 0);
            Hlp.GridFirstRowDeselect(dg);
            // Hlp.GridColor(dg);
            dg.Columns[0].Visible = false;
            dg.Columns[1].Width = 90;
            dg.Columns[2].Width = 90;
            dg.Columns[3].Width = 200;
            dg.Columns[4].Width = 180;
            dg.Columns[5].Visible = false;
            dg.Columns[6].Visible = false;
        }



        private bool CheckSuccess()
        {
            bool isChecked = true;

            if (ptNameTextBox.Text == "")
            {
                MessageBox.Show(@"Input your patient name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ptNameTextBox.Focus();
                isChecked = false;
            }
            if (contactNoTextBox.Text == "")
            {
                MessageBox.Show(@"Input your patient contact", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                contactNoTextBox.Focus();
                isChecked = false;
            }

            if (_gt.FnSeekRecordNewLab("tb_in_ADMISSION", "Id='" + admIdTextBox.Text + "' AND ReleaseStatus=0")==false)
            {
                MessageBox.Show(@"Invalid Patient", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                contactNoTextBox.Focus();
                isChecked = false;
            }
          

            return isChecked;
        }

        private void ptNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (ptNameTextBox.Text=="")
                {
                    ptNameTextBox.Focus();
                }
                else
                {
                   
                }
            }
        }

        private void yearTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                UpdateDateOfBirth();
             

            }
        }
        private void UpdateDateOfBirth()
        {
           
        }

        private void monthTextBox_Enter(object sender, EventArgs e)
        {
           
        }

        private void monthTextBox_Leave(object sender, EventArgs e)
        {
          
        }

        private void yearTextBox_Enter(object sender, EventArgs e)
        {
            
        }

        private void yearTextBox_Leave(object sender, EventArgs e)
        {
           
        }

        private void dayTextBox_Enter(object sender, EventArgs e)
        {
           
        }

        private void dayTextBox_Leave(object sender, EventArgs e)
        {
           
        }

        private void monthTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                UpdateDateOfBirth();
               

            }
        }

        private void dayTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                UpdateDateOfBirth();
                gendertextBox.Focus();

            }
        }

        private void genderComboBox_Enter(object sender, EventArgs e)
        {
            gendertextBox.BackColor = Hlp.EnterFocus();
        }

        private void genderComboBox_Leave(object sender, EventArgs e)
        {
            gendertextBox.BackColor = Hlp.LeaveFocus();
        }

        private void addressTextBox_Enter(object sender, EventArgs e)
        {
            addressTextBox.BackColor = Hlp.EnterFocus();
        }

        private void addressTextBox_Leave(object sender, EventArgs e)
        {
            addressTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void spouseNameTextBox_Enter(object sender, EventArgs e)
        {
           
        }

        private void spouseNameTextBox_Leave(object sender, EventArgs e)
        {
           
        }

        private void bedNoTextBox_Enter(object sender, EventArgs e)
        {
           

        }

        private void bedNoTextBox_Leave(object sender, EventArgs e)
        {
            
        }

        private void genderComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                addressTextBox.Focus();
            }
        }

        private void addressTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                
            }

        }

        private void spouseNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                
            }

        }

        private void emgrContactTextBox_Enter(object sender, EventArgs e)
        {
          
        }

        private void emgrContactTextBox_Leave(object sender, EventArgs e)
        {
           
        }

        private void emgrContactTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                
            }
        }

        private void chiefComplainTextBox_Enter(object sender, EventArgs e)
        {
            
        }

        private void chiefComplainTextBox_Leave(object sender, EventArgs e)
        {
            
        }

        private void chiefComplainTextBox_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void refDrCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
               
                
            }

            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                dataGridView1.Focus();
            }
        }

        private void consDrCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                

            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                dataGridView1.Focus();
            }

        }

        

        private void contactNoTextBox_TextChanged(object sender, EventArgs e)
        {
            HelpDataGridLoadByPatientId();
        }

       

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string gccode = "";
                string gcdesc = "";
                gccode = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[0].Value.ToString();
                gcdesc = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[1].Value.ToString();

                switch (_textInfo)
                {
                    case "contact":
                        contactNoTextBox.Text = gcdesc;
                        var dt = Hlp.GetRegAndBedId(Convert.ToInt32(gccode));
                        ptNameTextBox.Text = dt.Patient.Name;
                        gendertextBox.Text = dt.Patient.Sex;
                        addressTextBox.Text = dt.Patient.Address;
                        bedNoTextBox.Text = dt.Bed.Name;
                        admIdTextBox.Text = gccode;
                         advanceAmtTextBox.Focus();
                        break;
                }

            }
        }

        private void bedNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                dataGridView1.Focus();
            }

        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT TrNo,TrDate,PtName,BedName,Amount FROM V_Advance_Collection_List", searchTextBox.Text, "(TrNo+PtName)");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 200;
            dataGridView1.Columns[3].Width = 90;
            dataGridView1.Columns[4].Width = 70;

            //dataGridView1.Columns[4].Visible = false;
        }

        private void searchTextBox_Enter(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT TrNo,TrDate,PtName,BedName,Amount FROM V_Advance_Collection_List WHERE TrDate='" + Hlp.GetServerDate().ToString("yyyy-MM-dd") + "'");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 200;
            dataGridView1.Columns[3].Width = 90;
            dataGridView1.Columns[4].Width = 70;

            searchTextBox.BackColor = Hlp.EnterFocus();


        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.Selected)
            {
                if (dataGridView1.CurrentRow != null)
                {
                    string invNo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    //OpenAdmisionForm(invNo);
                   // string trNo =_gt.FncReturnFielValueLab("tb_in_ADMISSION", "AdmNo='" + invNo + "'", "TrNo");
                    OpenAdvanceBill(invNo);
                }
            }
        }

        private void OpenAdmisionForm(string invNo)
        {
            var dt = new IndoorReportViewer("Admission", "SELECT * FROM V_Admission_List WHERE AdmNo='" + invNo + "'", "Admission", "V_Admission_List", "");
            dt.Show();
        }

        private void AdvanceCollectionUi_Load(object sender, EventArgs e)
        {
            ClearText();
        }

        private void searchTextBox_Leave(object sender, EventArgs e)
        {
            searchTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {

                try
                {
                    string invNo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    int admId = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_in_ADVANCE_COLLECTION", "TrNo='" + invNo + "'", "AdmId"));





                    if (MessageBox.Show(@"Do you want to request for cancel this advance ?", @"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='" + Hlp.UserName + "' AND ParentName='Indoor' AND PermisionName='Advance Collection-Delete'"))
                        {
                            _gt.DeleteInsertLab("INSERT INTO  DEL_RECORD_OF_BILL_DELETE  (BillNo, BillDate, BillTime, RegId, PatientName, MobileNo, Address, Age, Sex, RefDrId, UnderDrId, TotalAmt, LessAmt, LessFrom, CollAmt, Remarks, PostedBy,ModuleName,Status,MasterId,PcName,IpAddress) SELECT a.TrNo, a.TrDate, '', a.RegId, b.PtName, b.ContactNo, b.PtAddress, b.PtAge, b.PtSex, b.RefId, b.UnderDrId, a.Amount, 0, 0, 0, '', '" + Hlp.UserName + "','In-Advance','Pending'," + admId + ",'" + Environment.UserName + "','" + Hlp.IpAddress() + "' FROM tb_in_ADVANCE_COLLECTION a LEFT JOIN tb_in_ADMISSION b ON a.AdmId=b.Id WHERE a.TrNo='" + invNo + "' AND a.AdmId='" + admId + "'");
                            MessageBox.Show(@"Advance cancel request success.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            searchTextBox.Focus();
                        }
                        else
                        {
                            MessageBox.Show(@"You need permission to do this task.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
             
               

            }
        }














    }
}
