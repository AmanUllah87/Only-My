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
    public partial class AdmissionUi : Form
    {
        public AdmissionUi()
        {
            InitializeComponent();
        }

        readonly AdmissionGateway _gt=new AdmissionGateway();

        private void contactNoTextBox_Enter(object sender, EventArgs e)
        {
            contactNoTextBox.BackColor = Hlp.EnterFocus();
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

        private void refDrCodeTextBox_Enter(object sender, EventArgs e)
        {
            refDrCodeTextBox.BackColor = Hlp.EnterFocus();
            HelpDataGridLoadByRefDr();
            _textInfo = "refDr";

        }
        private void HelpDataGridLoadByRefDr()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name FROM tb_DOCTOR", refDrCodeTextBox.Text, "(convert(varchar,Id)+Name)");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[1].Width = 380;
        }
        private void HelpDataGridLoadByConsDr()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name FROM tb_DOCTOR", consDrCodeTextBox.Text, "(convert(varchar,Id)+Name)");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[1].Width = 380;
        }
        private void HelpDataGridLoadByBedNo()
        {
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name,Floor,BedType,Charge FROM V_IN_BED_WITH_DEPT WHERE BookStatus=0 AND (convert(varchar,Id)+Name+BedType) LIKE '%" + bedNoTextBox.Text + "%'");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[2].Width = 80;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 70;

        }

        private void refDrCodeTextBox_Leave(object sender, EventArgs e)
        {
            refDrCodeTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void consDrCodeTextBox_Enter(object sender, EventArgs e)
        {
            consDrCodeTextBox.BackColor = Hlp.EnterFocus();
            HelpDataGridLoadByConsDr();
            _textInfo = "consDr";
        }

        private void consDrCodeTextBox_Leave(object sender, EventArgs e)
        {
            consDrCodeTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void departmentComboBox_Enter(object sender, EventArgs e)
        {
            departmentComboBox.BackColor = Hlp.EnterFocus();
        }

        private void departmentComboBox_Leave(object sender, EventArgs e)
        {
            departmentComboBox.BackColor = Hlp.LeaveFocus();
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
                    Patient = new PatientModel
                    {
                        PatientId=Convert.ToInt32(Hlp.IsNumeric(admNoTextBox.Text)?admNoTextBox.Text:"0"),
                        Name = ptNameTextBox.Text,
                        Sex = genderComboBox.Text,
                        Address = addressTextBox.Text,
                        ContactNo = contactNoTextBox.Text,
                        Dob = dobDateTimePicker.Value,
                        Spouse = spouseNameTextBox.Text,
                       
                       
                    },
                    AdmId =Convert.ToInt32(admIdTextBox.Text),
                    RefDoctor = new DoctorModel(){DrId =Convert.ToInt32(refDrCodeTextBox.Text),},
                    UnderDoctor = new DoctorModel() { DrId = Convert.ToInt32(consDrCodeTextBox.Text), },
                    Department= new DepartmentModel() { DeptId= Convert.ToInt32(departmentComboBox.SelectedValue), },
                    Bed = new BedModel{BedId = Convert.ToInt32(bedNoTextBox.Text)},
                    AdmissionCharge = Convert.ToDouble(admChargeTextBox.Text),
                    AdvanceAmt = Convert.ToDouble(Hlp.IsNumeric(advanceAmtTextBox.Text)?advanceAmtTextBox.Text:"0"),
                    EmergencyContact = emgrContactTextBox.Text,
                    ChiefComplain = chiefComplainTextBox.Text,
                    AdmNo=admNoTextBox.Text,
                };

                aMdl.Patient.RegId = Hlp.InsertIntoPatient(aMdl.Patient.Name, aMdl.Patient.Address, aMdl.Patient.ContactNo, aMdl.Patient.Sex, aMdl.Patient.Dob);

                if (yearTextBox.Text != "0")
                {
                    aMdl.Patient.Age = yearTextBox.Text + "Y ";
                }
                if (monthTextBox.Text != "0")
                {
                    aMdl.Patient.Age += monthTextBox.Text.PadLeft(2, '0') + "M ";
                }
                if (dayTextBox.Text != "0")
                {
                    aMdl.Patient.Age += dayTextBox.Text.PadLeft(2, '0') + "D";
                }

                string msg = _gt.Save(aMdl, saveButton.Text);
                MessageBox.Show(msg, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearText();

                if (msg == _gt.SaveSuccessMessage)
                {
                    OpenAdmisionForm(aMdl.AdmNo);
                    if (aMdl.AdvanceAmt>0)
                    {
                        OpenAdvanceBill(aMdl.AdvanceAmtTrNo);    
                    }
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
            admNoTextBox.Text = _gt.GetInvoiceNo("tb_IN_ADMISSION", "AdmNo", "AdmDATE");
            contactNoTextBox.Focus();
            contactNoTextBox.Text = "";
            ptNameTextBox.Text = "";
            yearTextBox.Text = "";
            addressTextBox.Text = "";
            refDrCodeTextBox.Text = "";
            refDrNameTextBox.Text = "";
            consDrCodeTextBox.Text = "";
            consDrNameTextBox.Text = "";
            bedNoTextBox.Text = "";
            bedNoString.Text = "";
            bedTypeTextBox.Text = "";
            chargeTextBox.Text = "";
            addressTextBox.Text = "";
            spouseNameTextBox.Text = "";
            emgrContactTextBox.Text = "";
            advanceAmtTextBox.Text = "";
           // dataGridView1.Rows.Clear();
            monthTextBox.Text = "";
            yearTextBox.Text = "";
            dayTextBox.Text = "";
            saveButton.Text = "&Save";
            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_IN_DEPARTMENT Order by Id", departmentComboBox);
            admChargeTextBox.Text = _gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "AdmissionCharge");//Aman
        }

        private string _textInfo = "";

        private void contactNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (contactNoTextBox.TextLength != 11)
                {
                    contactNoTextBox.Focus();
                    return;
                }
              
                if (_gt.FnSeekRecordNewLab("tb_PATIENT", "ContactNo='" + contactNoTextBox.Text + "'"))
                {
                    HelpDataGridLoadByContactNo(dataGridView1);
                }
                else
                {
                    ptNameTextBox.Focus();
                }

            }


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
            if (_gt.FnSeekRecordNewLab("tb_Doctor", "Id='" + refDrCodeTextBox.Text + "'") == false)
            {
                MessageBox.Show(@"Invalid Doctor!!!Please Check.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                refDrCodeTextBox.Focus();
                isChecked = false;
            }
            if (_gt.FnSeekRecordNewLab("tb_Doctor", "Id='" + consDrCodeTextBox.Text + "'") == false)
            {
                MessageBox.Show(@"Invalid Doctor!!!Please Check.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                consDrCodeTextBox.Focus();
                isChecked = false;
            }
            if (bedNoString.Text == "")
            {
                MessageBox.Show(@"Invalid Bed!!!Please Check.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ptNameTextBox.Focus();
                isChecked = false;
            }
            if (departmentComboBox.Text == "--Select--")
            {
                MessageBox.Show(@"Invalid Department!!!Please Check.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ptNameTextBox.Focus();
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
                    yearTextBox.Focus();
                }
            }
        }

        private void yearTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                UpdateDateOfBirth();
                monthTextBox.Focus();

            }
        }
        private void UpdateDateOfBirth()
        {
            if (yearTextBox.Text == "")
            {
                yearTextBox.Text = "0";
            }
            if (monthTextBox.Text == "")
            {
                monthTextBox.Text = "0";
            }
            if (dayTextBox.Text == "")
            {
                dayTextBox.Text = "0";
            }
            dobDateTimePicker.Value = Hlp.CalculateDob(DateTime.Now, Convert.ToInt32(yearTextBox.Text), Convert.ToInt32(monthTextBox.Text), Convert.ToInt32(dayTextBox.Text));

        }

        private void monthTextBox_Enter(object sender, EventArgs e)
        {
            monthTextBox.BackColor = Hlp.EnterFocus();
        }

        private void monthTextBox_Leave(object sender, EventArgs e)
        {
            monthTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void yearTextBox_Enter(object sender, EventArgs e)
        {
            yearTextBox.BackColor = Hlp.EnterFocus();
        }

        private void yearTextBox_Leave(object sender, EventArgs e)
        {
            yearTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void dayTextBox_Enter(object sender, EventArgs e)
        {
            dayTextBox.BackColor = Hlp.EnterFocus();
        }

        private void dayTextBox_Leave(object sender, EventArgs e)
        {
            dayTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void monthTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                UpdateDateOfBirth();
                dayTextBox.Focus();

            }
        }

        private void dayTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                UpdateDateOfBirth();
                genderComboBox.Focus();

            }
        }

        private void genderComboBox_Enter(object sender, EventArgs e)
        {
            genderComboBox.BackColor = Hlp.EnterFocus();
        }

        private void genderComboBox_Leave(object sender, EventArgs e)
        {
            genderComboBox.BackColor = Hlp.LeaveFocus();
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
            spouseNameTextBox.BackColor = Hlp.EnterFocus();
        }

        private void spouseNameTextBox_Leave(object sender, EventArgs e)
        {
            spouseNameTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void bedNoTextBox_Enter(object sender, EventArgs e)
        {
            bedNoTextBox.BackColor = Hlp.EnterFocus();
            HelpDataGridLoadByBedNo();
            _textInfo = "bed";

        }

        private void bedNoTextBox_Leave(object sender, EventArgs e)
        {
            bedNoTextBox.BackColor = Hlp.LeaveFocus();
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
                spouseNameTextBox.Focus();
            }

        }

        private void spouseNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                emgrContactTextBox.Focus();
            }

        }

        private void emgrContactTextBox_Enter(object sender, EventArgs e)
        {
            emgrContactTextBox.BackColor = Hlp.EnterFocus();
        }

        private void emgrContactTextBox_Leave(object sender, EventArgs e)
        {
            emgrContactTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void emgrContactTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                chiefComplainTextBox.Focus();
            }
        }

        private void chiefComplainTextBox_Enter(object sender, EventArgs e)
        {
            chiefComplainTextBox.BackColor = Hlp.EnterFocus();
        }

        private void chiefComplainTextBox_Leave(object sender, EventArgs e)
        {
            chiefComplainTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void chiefComplainTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                refDrCodeTextBox.Focus();
            }
        }

        private void refDrCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (_gt.FnSeekRecordNewLab("tb_DOCTOR", "Id=" + Convert.ToInt32(Hlp.StringToDouble(refDrCodeTextBox.Text)) + ""))
                {
                    refDrNameTextBox.Text = _gt.FncReturnFielValueLab("tb_DOCTOR", "Id=" + Convert.ToInt32(refDrCodeTextBox.Text) + "", "Name");
                    refDrCodeTextBox.Focus();
                }
                else
                {
                    MessageBox.Show(@"Please Add Valid Doctor Name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refDrCodeTextBox.Focus();
                }
                
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
                if (consDrCodeTextBox.Text == "" && _gt.FnSeekRecordNewLab("tb_DOCTOR", "Id=" + Convert.ToInt32(refDrCodeTextBox.Text) + ""))
                {
                    consDrCodeTextBox.Text = refDrCodeTextBox.Text;
                }
                
                
                if (_gt.FnSeekRecordNewLab("tb_DOCTOR", "Id=" + Convert.ToInt32(consDrCodeTextBox.Text) + ""))
                {
                    consDrNameTextBox.Text = _gt.FncReturnFielValueLab("tb_DOCTOR", "Id=" + Convert.ToInt32(consDrCodeTextBox.Text) + "", "Name");
                    bedNoTextBox.Focus();
                }
                else
                {
                    MessageBox.Show(@"Please Add Valid Doctor Name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    consDrCodeTextBox.Focus();
                }

            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                dataGridView1.Focus();
            }

        }

        private void refDrCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            HelpDataGridLoadByRefDr();
        }

        private void consDrCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            HelpDataGridLoadByConsDr();
        }

        private void contactNoTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void bedNoTextBox_TextChanged(object sender, EventArgs e)
        {
            HelpDataGridLoadByBedNo();
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
                        var dt = _pt.GetRegisterPatientList("", Convert.ToInt32(gccode));
                        if (dt.Rows.Count > 0)
                        {
                            ptNameTextBox.Text = dt.Rows[0]["Name"].ToString();
                            genderComboBox.Text = dt.Rows[0]["Sex"].ToString();
                            addressTextBox.Text = dt.Rows[0]["Address"].ToString();
                            DateTime dob = Convert.ToDateTime(dt.Rows[0]["Dob"]);
                            var mdl = Hlp.CalculateAgeByDobReturnAgeModel(dob);
                            yearTextBox.Text = mdl.Year;
                            monthTextBox.Text = mdl.Month;
                            dayTextBox.Text = mdl.Day;
                            spouseNameTextBox.Focus();
                        }
                       // refDrCodeTextBox.Focus();
                        break;
                    case "refDr":
                        refDrCodeTextBox.Text = gccode;
                        refDrNameTextBox.Text = gcdesc;
                        consDrCodeTextBox.Focus();
                        break;
                    case "consDr":
                        consDrCodeTextBox.Text = gccode;
                        consDrNameTextBox.Text = gcdesc;
                        bedNoTextBox.Focus();
                        break;
                    case "bed":
                        bedNoTextBox.Text = gccode;
                        bedNoString.Text = gcdesc;
                        dt = _gt.GetBedList(Convert.ToInt32(gccode), "");
                        if (dt.Rows.Count > 0)
                        {
                            bedNoString.Text = dt.Rows[0]["Name"].ToString();
                            bedTypeTextBox.Text = dt.Rows[0]["BedType"].ToString();
                            chargeTextBox.Text = dt.Rows[0]["Charge"].ToString();
                            departmentComboBox.SelectedValue = dt.Rows[0]["DeptId"].ToString();
                            advanceAmtTextBox.Focus();
                        }
                        break;
                    //case "adm":
                    //    admNoTextBox.Text = gccode;
                    //    dt = _gt.GetAdmissionDataForEdit(Convert.ToInt32(gccode), "");
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        bedNoString.Text = dt.Rows[0]["Name"].ToString();
                    //        bedTypeTextBox.Text = dt.Rows[0]["BedType"].ToString();
                    //        chargeTextBox.Text = dt.Rows[0]["Charge"].ToString();
                    //        departmentComboBox.SelectedValue = dt.Rows[0]["DeptId"].ToString();
                    //        advanceAmtTextBox.Focus();
                    //    }
                    //    break;










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
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT AdmNo,AdmDate,PtName,ContactNo,ChiefComplain FROM tb_in_ADMISSION  WHERE ReleaseStatus=0 AND (AdmNo+PtName+ContactNo+ChiefComplain)  LIKE '%"+ searchTextBox.Text +"%'");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 250;
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
        }

        private void searchTextBox_Enter(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT AdmNo,AdmDate,PtName,ContactNo,ChiefComplain FROM tb_in_ADMISSION WHERE AdmDate='" + Hlp.GetServerDate().ToString("yyyy-MM-dd") + "' AND ReleaseStatus=0 ");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 250;
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            searchTextBox.BackColor = Hlp.EnterFocus();


        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.Selected)
            {
                if (dataGridView1.CurrentRow != null)
                {
                    string invNo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    OpenAdmisionForm(invNo);
                    string trNo =_gt.FncReturnFielValueLab("tb_in_ADMISSION", "AdmNo='" + invNo + "'", "TrNo");
                    OpenAdvanceBill(trNo);
                }
            }
        }

        private void OpenAdmisionForm(string invNo)
        {
            var dt = new IndoorReportViewer("Admission", "SELECT * FROM V_Admission_List WHERE AdmNo='" + invNo + "'", "Admission", "V_Admission_List", "");
            dt.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        private void admNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                
                if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='" + Hlp.UserName + "' AND ParentName='Indoor' AND PermisionName='Admission-Edit'")==false)//aman
                {



                    MessageBox.Show(@"You have no permission to change admission", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                            
                if (_gt.FnSeekRecordNewLab("tb_in_Admission","AdmNo='"+ admNoTextBox.Text +"' AND ReleaseStatus=0"))
                {
                    var dat = _gt.GetAdmissionDataForEdit(admNoTextBox.Text);

                        ptNameTextBox.Text=dat.Patient.Name;
                        genderComboBox.Text=dat.Patient.Sex;
                        addressTextBox.Text=dat.Patient.Address;
                        contactNoTextBox.Text=dat.Patient.ContactNo;
                       //  dobDateTimePicker.Value,
                        spouseNameTextBox.Text=dat.Patient.Spouse;
                    admIdTextBox.Text=dat.Patient.PatientId.ToString();
                    refDrCodeTextBox.Text=dat.RefDoctor.DrId.ToString();
                    refDrNameTextBox.Text=dat.RefDoctor.Name;

                    consDrCodeTextBox.Text=dat.UnderDoctor.DrId.ToString();
                    consDrNameTextBox.Text=dat.UnderDoctor.Name;

                    departmentComboBox.SelectedValue=dat.Department.DeptId;
                    bedNoString.Text=dat.Bed.Name;
                    bedNoTextBox.Text = dat.Bed.BedId.ToString();

                    bedTypeTextBox.Text = dat.Bed.BedType;
                    chargeTextBox.Text = dat.Bed.Charge.ToString();
         
                    emgrContactTextBox.Text=dat.EmergencyContact;
                    chiefComplainTextBox.Text = dat.ChiefComplain;
                    saveButton.Text = "&Update";

                    try
                    {
                        string[] split = dat.Patient.Age.Split(new Char[] { 'Y', 'M', 'D' }, StringSplitOptions.RemoveEmptyEntries);
                        yearTextBox.Text = split[0].Trim();
                        monthTextBox.Text = split[1].Trim();
                        dayTextBox.Text = split[2].Trim();
                        UpdateDateOfBirth();
                      
                    }
                    catch (Exception)
                    {
                        ;
                    }
                   

                    
                    
                    
                    
                   








                }
            }
        }

        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode==Keys.Down||e.KeyCode==Keys.Up)
            //{
            //    if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            //    {
            //        _textInfo = "adm";
            //        dataGridView1.Focus();
            //    }
            //}
        }

        private void searchTextBox_Leave(object sender, EventArgs e)
        {
            searchTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
               
                    string invNo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    int admId = Convert.ToInt32(_gt.FncReturnFielValueLab("tb_in_ADMISSION", "AdmNo='" + invNo + "'", "Id"));
                    if (_gt.FnSeekRecordNewLab("tb_ph_SALES_MASTER", "AdmId=" + admId + ""))
                    {
                        MessageBox.Show(@"This  patient has already taken pharmacy service. Can not delete.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (_gt.FnSeekRecordNewLab("tb_BILL_MASTER", "AdmId=" + admId + ""))
                    {
                        MessageBox.Show(@"This  patient has already taken outdoor service. Can not delete.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }


                    else
                    {

                        if (MessageBox.Show(@"Do you want to request for cancel this admission ?", @"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        {
                            return;
                        }
                        else
                        {
                            if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='" + Hlp.UserName + "' AND ParentName='Indoor' AND PermisionName='Admission-Delete'"))
                            {
                                _gt.DeleteInsertLab("INSERT INTO  DEL_RECORD_OF_BILL_DELETE  (BillNo, BillDate, BillTime, RegId, PatientName, MobileNo, Address, Age, Sex, RefDrId, UnderDrId, TotalAmt, LessAmt, LessFrom, CollAmt, Remarks, PostedBy,ModuleName,Status,MasterId,PcName,IpAddress) SELECT AdmNo, AdmDate, '', RegId, PtName, ContactNo, PtAddress, PtAge, PtSex, RefId, UnderDrId, 0, 0, 0, 0, '', '"+ Hlp.UserName +"','In-Admission','Pending',"+ admId +",'"+ Environment.UserName +"','"+ Hlp.IpAddress() +"' FROM tb_in_ADMISSION WHERE Id="+ admId +"");
                                MessageBox.Show(@"Admission cancel request success.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                searchTextBox.Focus();
                            }
                            else
                            {
                                MessageBox.Show(@"You need permission to do this task.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }


                        }
                    }
               


            }
        }














    }
}
