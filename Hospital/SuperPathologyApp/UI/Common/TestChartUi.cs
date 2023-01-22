using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
 
using System.Windows.Forms;

namespace SuperPathologyApp.UI
{
    public partial class TestChartUi : Form
    {
        public TestChartUi()
        {
            InitializeComponent();
        }
        TestChartGateway _gt = new TestChartGateway();
        public void Gridwidth(int no)
        {
            switch (no)
            {
                case 1:
                    dataGridView1.Columns[0].Width = 65;
                    dataGridView1.Columns[1].Width = 240;
                    dataGridView1.Columns[2].Width = 70;

                    dataGridView1.Columns[3].Visible = false;
                    dataGridView1.Columns[4].Visible = false;
                    dataGridView1.Columns[5].Visible = false;
                    dataGridView1.Columns[6].Visible = false;
                    dataGridView1.Columns[7].Visible = false;
                    dataGridView1.Columns[8].Visible = false;
                    dataGridView1.Columns[9].Visible = false;
                    dataGridView1.Columns[10].Visible = false;
                    dataGridView1.Columns[11].Visible = false;
                    dataGridView1.Columns[12].Visible = false;
                    dataGridView1.Columns[13].Visible = false;
                    dataGridView1.Columns[14].Visible = false;
                    dataGridView1.Columns[15].Visible = false;
                    //dataGridView1.Columns[16].Visible = false;


                    break;

            }
        }

     




        private void button1_Click(object sender, EventArgs e)
        {
           

        }

        private void button10_Click(object sender, EventArgs e)
        {
            




        }

      

        private void frmGroupSetup_Activated(object sender, EventArgs e)
        {
           // testCodeTextBox.Focus(); 
        }

        private void groupNameComboBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

      
        public void RefreshText() 
        {

            //dataGridView2.DataSource = _gt.GetTestCodeList("");
            Gridwidth(1);
       //     _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_Group Order By Id Desc", groupNameComboBox);
         //   _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_VaqGroup Order By Id Desc", vaqGroupNameComboBox);



            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;


            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView2.AllowUserToResizeRows = false;







        }

        private void groupNameComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = true;

            if (e.KeyChar == 13)
            {
               // vaqGroupNameComboBox.Focus();
                

            }

        }

  

        private void masterCodeTextBox_Enter(object sender, EventArgs e)
        {

        }

  

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            

          
        }



        private void vaqGroupNameComboBox_Leave(object sender, EventArgs e)
        {
            testNameTextBox.BackColor = DbConnection.LeaveFocus();
        }


        private void masterCodeTextBox_Leave(object sender, EventArgs e)
        {
            testNameTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void masterCodeTextBox_Enter_1(object sender, EventArgs e)
        {
            testNameTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void testCodeTextBox_TextChanged(object sender, EventArgs e)
        {
           // dataGridView3.DataSource = _gt.GetTestCodeList(testCodeTextBox.Text);
           // _gt.GridColor(dataGridView3);
        }



        private void masterCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            LoadDg1(testNameTextBox.Text,2);

        }

    

        private void testCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
              //  textinfo = "1";
                dataGridView2.Focus();
            }
           


        }

   
        private void groupNameComboBox_KeyDown_1(object sender, KeyEventArgs e)
        {
           
        }

        private void vaqGroupNameComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                testNameTextBox.Focus();
            }
        }

        private void masterCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                chargeTextBox.Focus();
            }
        }



        private void testNameTextBox_Enter(object sender, EventArgs e)
        {
            testNameTextBox.BackColor = Hlp.EnterFocus();
        }

        private void testNameTextBox_Leave(object sender, EventArgs e)
        {
            testNameTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void chargeTextBox_Enter(object sender, EventArgs e)
        {
            chargeTextBox.BackColor = Hlp.EnterFocus();
        }

        private void chargeTextBox_Leave(object sender, EventArgs e)
        {
            chargeTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void defaultHnouriamTextBox_Enter(object sender, EventArgs e)
        {
            defaultHnouriamPcTextBox.BackColor = Hlp.EnterFocus();
        }

        private void defaultHnouriamTextBox_Leave(object sender, EventArgs e)
        {
            defaultHnouriamPcTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void reportFileNameTextBox_Enter(object sender, EventArgs e)
        {
            reportFileNameTextBox.BackColor = Hlp.EnterFocus();
        }

        private void reportFileNameTextBox_Leave(object sender, EventArgs e)
        {
            reportFileNameTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (testNameTextBox.Text == "")
            {
                MessageBox.Show(@"Please add Test name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                testNameTextBox.Focus();
                return;
            }

            if (Hlp.IsNumeric(chargeTextBox.Text)==false)
            {
                MessageBox.Show(@"Please add valid charge.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chargeTextBox.Focus();
                return;
            }
            if (defaultHnouriamPcTextBox.Text=="") 
            {
                defaultHnouriamPcTextBox.Text = "0";
            }
            if (subProjectNameComboBox.SelectedValue== "0")
            {
                MessageBox.Show(@"Please add department name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                subProjectNameComboBox.Focus();
                return;
            }

            var lists = new List<VacutainerModel>();
            for (int i = 0; i < dataGridView2.Rows.Count ; i++)
            {
                lists.Add(new VacutainerModel()
                {
                    VaqId = Convert.ToInt32(dataGridView2.Rows[i].Cells[0].Value),
                    Charge= Convert.ToDouble(dataGridView2.Rows[i].Cells[2].Value),
                });
            }

            if (reportFeeTextBox.Text=="")
            {
                reportFeeTextBox.Text = "0";
            }
            var mdl = new TestChartModel
            {
                TestId =Convert.ToInt32(idTextBox.Text),
                Name = testNameTextBox.Text,
                Charge =Convert.ToDouble(chargeTextBox.Text),
                DefaulHonouriam = Convert.ToDouble(defaultHonouriamTextBox.Text),
                IsVaqItem = Convert.ToInt32(isVaqComboBox.SelectedIndex),
                IsGiveDiscount = Convert.ToInt32(giveDiscountComboBox.SelectedIndex),
                SubProject=new SubProjectModel{SubProjectId=Convert.ToInt32(subProjectNameComboBox.SelectedValue)},
                ReportFileName = reportFileNameTextBox.Text,
                ReportFee = Convert.ToDouble(reportFeeTextBox.Text),
                Vaq=lists,
                VaqName = vaqNameComboBox.Text,
                IsDoctorItem = Convert.ToInt32(isDoctorComboBox.SelectedIndex),
                IsChangeCharge = Convert.ToInt32(changeChargecomboBox.SelectedIndex),

            };

            
            MessageBox.Show(_gt.Save(mdl), @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDg1("",2);
            idTextBox.Text = "0";
            dataGridView2.Rows.Clear();


        }

        private void TestChartUi_Load(object sender, EventArgs e)
        {
            Hlp.GridColor(dataGridView2);
            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_SubProject Order By Id", subProjectNameComboBox);
            subProjectNameComboBox.SelectedIndex = 0;
            isVaqComboBox.SelectedIndex = 0;
            giveDiscountComboBox.SelectedIndex = 1;
            txtInfo = "1";
            LoadDg1("",2);
            honouriamStatusComboBox.SelectedIndex = 0;
            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_TESTCHART_VAQ Order By Id", vaqNameComboBox);



            changeChargecomboBox.SelectedIndex = 0;
        
        
        
        }

        private void LoadDg1(string search,int isVaq)
        {
            dataGridView1.DataSource = _gt.GetTestCodeList(0, search,isVaq);
            Hlp.GridColor(dataGridView1);
            Gridwidth(1);
        }

        string txtInfo = "";
        private void testNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Down)
            {
                txtInfo = "1";
                dataGridView1.Focus();
            }

            if (e.KeyCode == Keys.Enter)
            {
                if (testNameTextBox.Text =="")
                {
                    MessageBox.Show(@"Invalid test name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    testNameTextBox.Focus();
                    return;
                }
                chargeTextBox.Focus();
            }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string gccode = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[0].Value.ToString();
                 string gcdesc = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[1].Value.ToString();

                switch (txtInfo)
                {
                    case "1":
                        idTextBox.Text = gccode;
                        testNameTextBox.Text = gcdesc;
                        GetTestById(idTextBox.Text);
                        testNameTextBox.Focus();
                        break;
                    case "2":


                        if (Hlp.DataGridDuplicateCheck(gccode,dataGridView2) == false)
                        {
                            dataGridView2.Rows.Add(gccode, gcdesc, dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[2].Value.ToString());
                        }
                        else
                        {
                            MessageBox.Show(@"This item has already exist.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        
                       
                        
                        vaqNameSearchTextBox.Text = "";
                        vaqNameSearchTextBox.Focus();
                        break;

                }

            }
        }

        private void GetTestById(string testId)
        {
            try
            {
                var dt = _gt.GetTestCodeList(Convert.ToInt32(testId), "",2);
                if (dt.Rows.Count > 0)
                {
                    chargeTextBox.Text = dt.Rows[0]["Charge"].ToString();
                    giveDiscountComboBox.SelectedIndex = Convert.ToInt32(dt.Rows[0]["IsDiscountItem"]);
                    defaultHonouriamTextBox.Text = dt.Rows[0]["MaxDiscount"].ToString();
                    isVaqComboBox.SelectedIndex = Convert.ToInt32(dt.Rows[0]["IsVaqItem"]);
                    subProjectNameComboBox.SelectedValue = Convert.ToInt32(dt.Rows[0]["SubProjectId"]);
                    reportFileNameTextBox.Text = dt.Rows[0]["ReportFileName"].ToString();
                    vaqNameComboBox.Text  = dt.Rows[0]["VaqName"].ToString();
                    reportFeeTextBox.Text = dt.Rows[0]["ReportFee"].ToString();
                    changeChargecomboBox.SelectedIndex = Convert.ToInt32(dt.Rows[0]["ChangeCharge"]);

                    isDoctorComboBox.SelectedIndex = Convert.ToInt32(dt.Rows[0]["IsDoctor"]);


                }
                var lists = _gt.GetVaqListByTestId(Convert.ToInt32(testId));
                dataGridView2.Rows.Clear();
                foreach (var item in lists)
                {
                    dataGridView2.Rows.Add(item.VaqId, _gt.FncReturnFielValueLab("tb_TestChart", "Id=" + item.VaqId + "", "Name"), item.Charge);
                }
                Hlp.GridColor(dataGridView2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void vaqNameSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            LoadDg1(vaqNameSearchTextBox.Text, 1);
        }

        private void vaqNameSearchTextBox_Enter(object sender, EventArgs e)
        {
            vaqNameSearchTextBox.BackColor = Hlp.EnterFocus();
            LoadDg1("", 1);
            txtInfo = "2";
        }

        private void vaqNameSearchTextBox_Leave(object sender, EventArgs e)
        {
            vaqNameSearchTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void vaqNameSearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Down)
            {
                dataGridView1.Focus();
            }
        }

        private void chargeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Hlp.IsNumeric(chargeTextBox.Text)==false)
                {
                    chargeTextBox.Focus();
                }
                else
                {
                    giveDiscountComboBox.Focus();
                }
            }
        }

        private void giveDiscountComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                defaultHnouriamPcTextBox.Focus();
            }
        }

        private void isVaqComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
               subProjectNameComboBox.Focus();
            }
        }

        private void subProjectNameComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                reportFileNameTextBox.Focus();
            }
        }

        private void reportFileNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.Focus();
            }
        }

        private void defaultHnouriamPcTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                if (Hlp.IsNumeric(defaultHnouriamPcTextBox.Text) == false)
                {
                    defaultHnouriamPcTextBox.Focus();
                }
                else
                {
                    honouriamStatusComboBox.Focus();
                }
            }
        }

        private void honouriamStatusComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                switch (honouriamStatusComboBox.SelectedIndex)
                {
                    case 0:
                        double amt=Convert.ToDouble(chargeTextBox.Text) * 0.01 * Convert.ToDouble(defaultHnouriamPcTextBox.Text);
                        defaultHonouriamTextBox.Text = Math.Round(amt).ToString();
                        break;
                    case 1:
                        defaultHonouriamTextBox.Text = defaultHnouriamPcTextBox.Text;
                        break;
                }
                isVaqComboBox.Focus();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)//Aman
        {
            testNameTextBox.Text = "";
            chargeTextBox.Text = "";
            reportFileNameTextBox.Text = "";
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                string id = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                if (_gt.FnSeekRecordNewLab("tb_BILL_DETAIL", "TestId='" + id + "'"))
                {
                    MessageBox.Show(@"This test has transaction can not delete!!", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='" + Hlp.UserName + "' AND ParentName='Setup' AND PermisionName='Test-Delete'"))
                    {
                        _gt.DeleteInsertLab("DELETE tb_TESTCHART WHERE  Id='" + id + "'");
                        _gt.DeleteInsertLab("DELETE tb_TESTCHART_PARAM WHERE  TestChartId='" + id + "'");
                        this.dataGridView1.Rows.RemoveAt(this.dataGridView1.CurrentCell.RowIndex);
                        MessageBox.Show(@"Test delete success.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(@"You need permision to do this task!!", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                    
                    
                    
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (_gt.FnSeekRecordNewLab("tb_TESTCHART_VAQ","Name='"+ vaqNameComboBox.Text +"'")==false)
            {
                _gt.DeleteInsertLab("INSERT INTO tb_TESTCHART_VAQ(Name)VALUES('"+ vaqNameComboBox.Text  +"')");
                _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_TESTCHART_VAQ Order By Id", vaqNameComboBox);

            }
        }

        private void dataGridView2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Delete)
            {
                dataGridView2.Rows.RemoveAt(this.dataGridView2.CurrentCell.RowIndex);

            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

  
    }
}
