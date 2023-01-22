using System;
using System.Windows.Forms;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Report;

namespace SuperPathologyApp.UI.Accounts
{
    public partial class AccountsReportUi : Form
    {
        public AccountsReportUi()
        {
            InitializeComponent();
        }
        DbConnection _gt = new DbConnection();
      
    
      

        private void button8_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM V_Expense_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' ";
            var dt = new LabReqViewer("DailyExpenseList", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Expense_List", "Accounts");
            dt.Show();
        }



        private void ExpenseReportUi_Load(object sender, EventArgs e)
        {
          //  _gt.LoadComboBox("SELECT Id, Name AS Description FROM tb_ac_DEPARTMENT", departmentComboBox);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM V_Datewise_Income_Expense WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' ";
            var dt = new LabReqViewer("DatewiseIncomeExpense", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Datewise_Income_Expense", "Accounts");
            dt.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM V_Income_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' ";
            var dt = new LabReqViewer("DailyIncomeList", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Expense_List", "Accounts");
            dt.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (deptCodeTextBox.Text != "")
            {
                cond += " AND DeptId=" + deptCodeTextBox.Text + "";
            }
            string query = "SELECT * FROM V_Expense_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' "+ cond +"";
            var dt = new LabReqViewer("deptWiseExpenseList", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Expense_List", "Accounts");
            dt.Show();
        }

        private void deptCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            LoadDataGridByDeptCode();
            

        }

        private void LoadDataGridByDeptCode()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name FROM tb_ac_DEPARTMENT", deptCodeTextBox.Text, "(convert(varchar,Id)+Name)");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[1].Width = 350;

        }

        private void deptCodeTextBox_Enter(object sender, EventArgs e)
        {
            deptCodeTextBox.BackColor = Hlp.EnterFocus();
            LoadDataGridByDeptCode();
        }

        private void deptCodeTextBox_Leave(object sender, EventArgs e)
        {
            deptCodeTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void subDeptCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            LoadDataGridBySubDept();
        }

        private void LoadDataGridBySubDept()
        {
            string cond = "";
            if (deptCodeTextBox.Text!="")
            {
                cond += " And DeptId='"+ deptCodeTextBox.Text +"'";
            }
            
            
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name FROM tb_ac_SUB_DEPARTMENT Where 1=1 " + cond + " AND (convert(varchar,Id)+Name) LIKE '%"+ subDeptCodeTextBox.Text +"%'");
            
            
            
            
            
            
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[1].Width = 350;
        }
        private void LoadDataGridByDetails()
        {
            string cond = "";
            if (detailsCodeTextBox.Text != "")
            {
                cond += " And Id='" + deptCodeTextBox.Text + "'";
            }


            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT Id,Name FROM tb_ac_HEAD Where 1=1 " + cond + " AND (convert(varchar,Id)+Name) LIKE '%" + subDeptCodeTextBox.Text + "%'");


            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[1].Width = 350;
        }
        private void subDeptCodeTextBox_Enter(object sender, EventArgs e)
        {
            subDeptCodeTextBox.BackColor = Hlp.EnterFocus();
            LoadDataGridBySubDept();
        }

        private void subDeptCodeTextBox_Leave(object sender, EventArgs e)
        {
            subDeptCodeTextBox.BackColor = Hlp.LeaveFocus();

        }
        string textInfo = "";
        private void deptCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Up||e.KeyCode==Keys.Down)
            {
                textInfo = "dept";
                dataGridView1.Focus();
            }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string gccode = "";
                string gcdesc = "";
                gccode = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[0].Value.ToString();
                gcdesc = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[1].Value.ToString();

                switch (textInfo)
                {
                    case "dept":
                        deptCodeTextBox.Text = gccode;
                        deptNameTextBox.Text = gcdesc;
                        subDeptCodeTextBox.Focus();
                        break;
                    case "subdept":
                        subDeptCodeTextBox.Text = gccode;
                        subDeptNameTextBox.Text = gcdesc;
                        detailsCodeTextBox.Focus();
                        break;
                    case "details":
                        detailsCodeTextBox.Text = gccode;
                        detailsTextBox.Text = gcdesc;
                        detailsCodeTextBox.Focus();
                        break;
                    


                }

            }
        }

        private void subDeptCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                textInfo = "subdept";
                dataGridView1.Focus();
            }

        }

        private void detailsCodeTextBox_Enter(object sender, EventArgs e)
        {
            detailsCodeTextBox.BackColor = Hlp.EnterFocus();
            LoadDataGridByDetails();//Aman

        }

        private void detailsCodeTextBox_Leave(object sender, EventArgs e)
        {
            detailsCodeTextBox.BackColor = Hlp.LeaveFocus();

        }

        private void detailsCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                textInfo = "details";
                dataGridView1.Focus();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string cond = "";
            if (subDeptCodeTextBox.Text != "")
            {
                cond += " AND SubDeptId=" + subDeptCodeTextBox.Text + "";
            }
            string query = "SELECT * FROM V_Expense_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("subDeptWiseExpenseList", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Expense_List", "Accounts");
            dt.Show();
        }

        private void DeptDetails_Click(object sender, EventArgs e)//Aman
        {
            string cond = "";
            if (deptCodeTextBox.Text != "")
            {
                cond += " AND DeptId=" + deptCodeTextBox.Text + "";
            }
            string query = "SELECT * FROM V_Expense_List WHERE TrDate Between '" + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new LabReqViewer("deptWiseExpenseDetails", query, "Date from " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + " and " + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd"), "V_Expense_List", "Accounts");
            dt.Show();
        }

        private void detailsCodeTextBox_TextChanged(object sender, EventArgs e)//Aman
        {
            LoadDataGridByDetails();//Aman
        }
    }
}
