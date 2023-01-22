using System;
using System.Drawing;
using System.Windows.Forms;
using SuperPathologyApp.Gateway;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using SuperPathologyApp.Report;
using SuperPathologyApp.Model.Accounts;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SuperPathologyApp.UI.Accounts
{
    public partial class ExpenseEntry : Form
    {

        readonly ExpenseVoucherGateway _gtTestCode = new ExpenseVoucherGateway();
        public string Textinfo = "";
       


        public ExpenseEntry()
        {
            InitializeComponent();
        }

        private void testCodeTextBox_Enter(object sender, EventArgs e)
        {
            expenseNameTextBox.BackColor = DbConnection.EnterFocus();
           
        }

       

        

        private void testCodeTextBox_Leave(object sender, EventArgs e)
        {
            expenseNameTextBox.BackColor = DbConnection.LeaveFocus();
        }

     

    
        private void GridColor(DataGridView dg)
        {
            int i = 0;
            while (i < dg.Rows.Count)
            {
                dg.Rows[i].DefaultCellStyle.BackColor = Color.Azure;
                i += 2;
            }
        }
   
        public void Gridwidth(int no)
        {
            switch (no)
            {
                case 1:
                    dataGridView1.Columns[0].Width = 50;
                    dataGridView1.Columns[1].Width = 303;
                    break;
               
             

            }
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.CurrentCell = null;
            
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeRows = false;
           
            
            //dataGridView3.EnableHeadersVisualStyles = false;
            //dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            //dataGridView3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dataGridView3.AllowUserToResizeRows = false;



        }

     

       

        private void testCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                Textinfo = "1";
                dataGridView1.Focus();
            }

            //if (e.KeyCode==Keys.Enter)
            //{
            //    if (_gt.FnSeekRecordNewLab("ChannelDefination ", "Pcode='" + expenseNameTextBox.Text + "'"))
            //    {
            //        dataGridView3.Rows.Clear();
            //        PopulateGridViewData();
                   
            //    }
            //    else
            //    {
            //        MessageBox.Show(@"Invalid Item Please Check", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);  
            //    }
            //}



        }

        private void GridShow(string search,int deptId,int subDeptId)
        {
            if (departmentComboBox.Text != "" || departmentComboBox.Text != "--Select--")
            {
                dataGridView1.DataSource = _gtTestCode.GetTestCodeList(search, deptId,subDeptId,"Expense");
            }
            else
            {
                dataGridView1.DataSource = _gtTestCode.GetTestCodeList(expenseNameTextBox.Text, 0, 0, "Expense");
            }
            GridColor(dataGridView1);
            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns[1].Width = 160;
            dataGridView1.Columns[2].Width = 160;
            dataGridView1.Columns[3].Width = 230;
            dataGridView1.Rows[0].Selected = false;

            //dataGridView1.Columns[4].Visible = false;
            //dataGridView1.Columns[5].Visible = false;
            //dataGridView1.Columns[6].Visible = false;
            //dataGridView1.Columns[7].Visible = false;
        }




     


     
       

      

 


     

        private void ClearAdd()
        {
            expenseCodeTextBox.Text = "";
            expenseNameTextBox.Text = "";
            remarksTextBox.Text = "";
        }

       
        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string gccode = "";
                string gcdesc = "";
                gccode = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[0].Value.ToString();
                gcdesc = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[1].Value.ToString();

                switch (Textinfo)
                {
                    case "1":
                        expenseCodeTextBox.Text = gccode;
                        expenseNameTextBox.Text = _gtTestCode.FncReturnFielValueLab("tb_ac_HEAD","Type='Expense' AND Code='" + expenseCodeTextBox.Text + "'","Name");
                        amountTextBox.Focus();
                        departmentComboBox.SelectedValue = Convert.ToInt32(expenseCodeTextBox.Text.ToString().Substring(0, 2));
                        subDeptComboBox.SelectedValue = Convert.ToInt32(expenseCodeTextBox.Text.ToString().Substring(2, 2));

                        break;
                }
            }
        }

    
     
    

       
      


        private void button1_Click(object sender, EventArgs e)
        {
            if (departmentComboBox.Text == "--Select--" || departmentComboBox.Text == "")
            {
                return;
            }

            if (_gtTestCode.FnSeekRecordNewLab("tb_ac_DEPARTMENT", "Name='" + departmentComboBox.Text.Trim() + "'") == false)
            {
                _gtTestCode.DeleteInsertLab("INSERT INTO tb_ac_DEPARTMENT(Id,Name)VALUES('" + Hlp.GetAutoIncrementVal("Id", "tb_ac_DEPARTMENT", 2) + "','" + departmentComboBox.Text.Trim() + "')");
                MessageBox.Show(@"Added Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _gtTestCode.LoadComboBox("SELECT Id,Name AS Description FROM tb_ac_DEPARTMENT Order By Id", departmentComboBox);

            }
            else
            {
                MessageBox.Show(@"Already exists please check.", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
            if (dataGridView2.Rows.Count==0)
            {
                MessageBox.Show(@"Please Add Expense First", @"Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string trNo = "";
            if (button4.Text == "&Save")
            {
                trNo = Hlp.GetAutoIncrementVal("TrNo", "tb_ac_EXPENSE_ENTRY", 8);
            }
            else
            {
                trNo = trNoTextBox.Text;
                _gtTestCode.DeleteInsertLab("DELETE FROM tb_ac_EXPENSE_ENTRY WHERE TrNo='"+ trNo +"'");
            }


           
            for (int i = 0; i < dataGridView2.Rows.Count-1; i++)
            {
                _gtTestCode.DeleteInsertLab("INSERT INTO tb_ac_EXPENSE_ENTRY(TrDate, TrNo, Code, Description, Amount, Remarks, PostedBy, PcName, IpAddress)VALUES('" + Hlp.GetServerDate().ToString("yyyy-MM-dd") + "', '" + trNo + "', '" + dataGridView2.Rows[i].Cells[0].Value.ToString() + "', '" + dataGridView2.Rows[i].Cells[1].Value.ToString() + "', '" + dataGridView2.Rows[i].Cells[2].Value.ToString() + "', '" + remarksTextBox.Text + "', '" + Hlp.UserName + "', '" + Environment.UserName + "', '" + Hlp.IpAddress() + "')");
            }
            MessageBox.Show(@"Save Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dataGridView2.Rows.Clear();
            ClearAdd();
            ClearText();
            expenseNameTextBox.Focus();


            if (_gtTestCode.FnSeekRecordNewLab("V_Expense_List", "TrNo='" + trNo + "'"))
            {
                var dt = new LabReqViewer("ExpenseVoucher", "SELECT * FROM V_Expense_List WHERE TrNo='" + trNo + "'", "Voucher", "V_Expense_List", "Accounts");
                dt.Show();
            }









        }


       
        private void expenseNameTextBox_TextChanged(object sender, EventArgs e)
        {
            GridShow(expenseNameTextBox.Text, Convert.ToInt32(departmentComboBox.SelectedValue), Convert.ToInt32(subDeptComboBox.SelectedValue));
        }

        private void departmentComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (departmentComboBox.SelectedIndex!=0)
            {
                _gtTestCode.LoadComboBox("SELECT Id,Name AS Description FROM tb_ac_SUB_DEPARTMENT WHERE DeptId='"+ departmentComboBox.SelectedValue +"' Order By Id", subDeptComboBox);
                GridShow(expenseNameTextBox.Text, Convert.ToInt32(departmentComboBox.SelectedValue), Convert.ToInt32(subDeptComboBox.SelectedValue));
            }
        }

        private void ExpenseEntry_Load(object sender, EventArgs e)
        {
            _gtTestCode.LoadComboBox("SELECT Id, Name AS Description FROM tb_ac_DEPARTMENT", departmentComboBox);
            GridShow(expenseNameTextBox.Text, Convert.ToInt32(departmentComboBox.SelectedValue), Convert.ToInt32(subDeptComboBox.SelectedValue));
            ClearText();



        }

        private void ClearText()
        {
            trNoTextBox.Text = Hlp.GetAutoIncrementVal("TrNo", "tb_ac_EXPENSE_ENTRY", 8);
            trDateTextBox.Text = Hlp.GetServerDate().ToString("dd-MMM-yyyy");
            button4.Text = "&Save";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void amountTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&(e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void subDeptComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (subDeptComboBox.SelectedIndex != 0)
            {
                GridShow(expenseNameTextBox.Text, Convert.ToInt32(departmentComboBox.SelectedValue), Convert.ToInt32(subDeptComboBox.SelectedValue));
            }
        }

        private void expenseNameTextBox_Enter(object sender, EventArgs e)
        {
            expenseNameTextBox.BackColor = Hlp.EnterFocus();
        }

        private void expenseNameTextBox_Leave(object sender, EventArgs e)
        {
            expenseNameTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void amountTextBox_Enter(object sender, EventArgs e)
        {
            amountTextBox.BackColor = Hlp.EnterFocus();
        }

        private void amountTextBox_Leave(object sender, EventArgs e)
        {
            amountTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void remarksTextBox_Enter(object sender, EventArgs e)
        {
            remarksTextBox.BackColor = Hlp.EnterFocus();
        }

        private void remarksTextBox_Leave(object sender, EventArgs e)
        {
            remarksTextBox.BackColor = Hlp.LeaveFocus();
        }

        private void expenseNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (_gtTestCode.FnSeekRecordNewLab("tb_ac_EXPENSE","Code='"+ expenseNameTextBox.Text +"'"))
                {
                    expenseCodeTextBox.Text = expenseNameTextBox.Text;
                    expenseNameTextBox.Text = _gtTestCode.FncReturnFielValueLab("tb_ac_EXPENSE","Code='" + expenseCodeTextBox.Text + "'","Name");
                  

                    amountTextBox.Focus();

                   

                    departmentComboBox.SelectedValue = Convert.ToInt32(expenseCodeTextBox.Text.ToString().Substring(0, 2));
                    subDeptComboBox.SelectedValue = Convert.ToInt32(expenseCodeTextBox.Text.ToString().Substring(2, 2));



                }
                else
                {
                    MessageBox.Show(@"Invalid Expense.Please Check??", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                Textinfo = "1";
                dataGridView1.Rows[0].Selected = true;
                dataGridView1.Focus();
            }









        }

        private void amountTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (amountTextBox.Text.Length>0)
                {
                    remarksTextBox.Focus();
                }
            }
        }

        private void remarksTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAdd.Focus();
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_gtTestCode.FnSeekRecordNewLab("tb_ac_HEAD","Type='Expense' AND Code='"+ expenseCodeTextBox.Text +"'"))
            {
                if (IsEdit == false)
                {

                    if (_gtTestCode.IsDuplicate(expenseCodeTextBox.Text, dataGridView2) == false)
                    {
                        dataGridView2.Rows.Add(expenseCodeTextBox.Text, expenseNameTextBox.Text, amountTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show(@"Duplicate Code Found.Please Check", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    dataGridView2.CurrentRow.Cells[0].Value = expenseCodeTextBox.Text;
                    dataGridView2.CurrentRow.Cells[1].Value = expenseNameTextBox.Text;
                    dataGridView2.CurrentRow.Cells[2].Value = amountTextBox.Text;
                    IsEdit = false;
                }
                dataGridView2.Rows[0].Selected = false;
                expenseCodeTextBox.Text = "";
                expenseNameTextBox.Text = "";
                amountTextBox.Text = "";
                expenseNameTextBox.Focus();
 
            }
            else
            {
                MessageBox.Show(@"Invalid Code.Please Check", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        

      

        private void searchTextBox_Enter(object sender, EventArgs e)
        {
            GridShow(Hlp.GetServerDate(),"");
        }

        private void GridShow(DateTime dateTime, string searchString)
        {
            dataGridView1.DataSource = _gtTestCode.GetTestCodeList(dateTime, searchString,"Expense");
            GridColor(dataGridView1);
            dataGridView1.Columns[0].Width = 150;
            dataGridView1.Columns[1].Width = 150;
            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Rows[0].Selected = false;
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            GridShow(Convert.ToDateTime(trDateTextBox.Text), searchTextBox.Text);
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {


           
            
           
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (dataGridView1.CurrentCell == null)
                {
                    return;
                }
                string invNo = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                if (_gtTestCode.FnSeekRecordNewLab("V_Expense_List", "TrNo='" + invNo + "'"))
                {
                   // _gtTestCode.DeleteInsertLab("INSERT INTO  DEL_RECORD_OF_BILL_DELETE  (BillNo, BillDate, BillTime, RegId, PatientName, MobileNo, Address, Age, Sex, RefDrId, UnderDrId, TotalAmt, LessAmt, LessFrom, CollAmt, Remarks, PostedBy,ModuleName,Status,MasterId) SELECT TrNo AS BillNo,TrDate AS  BillDate,'' AS BillTime,0 AS RegId,'' AS  PatientName,'' AS  MobileNo,'' AS  Address,'' AS  Age,'' AS  Sex,'0' AS RefDrId,'0' AS  UnderDrId,SUM(Amount) AS TotalAmt,  0 AS LessAmt,'' AS LessFrom,0 AS  CollAmt,Remarks, '" + Hlp.UserName + "','Voucher-Expense','Pending','0'  FROM tb_ac_EXPENSE_ENTRY WHERE TrNo='"+ invNo +"' GROUP BY TrNo,TrDate,Remarks ");
                    _gtTestCode.DeleteInsertLab("INSERT INTO  DEL_RECORD_OF_BILL_DELETE  (BillNo, BillDate, BillTime, RegId, PatientName, MobileNo, Address, Age, Sex, RefDrId, UnderDrId, TotalAmt, LessAmt, LessFrom, CollAmt, Remarks, PostedBy,ModuleName,Status,MasterId) SELECT TrNo AS BillNo,TrDate AS  BillDate,'' AS BillTime,0 AS RegId,'' AS  PatientName,'' AS  MobileNo,'' AS  Address,'' AS  Age,'' AS  Sex,'0' AS RefDrId,'0' AS  UnderDrId,SUM(Amount) AS TotalAmt,  0 AS LessAmt,'' AS LessFrom,0 AS  CollAmt,Remarks, '" + Hlp.UserName + "','Voucher','Pending','0'  FROM tb_ac_EXPENSE_ENTRY WHERE TrNo='" + invNo + "' GROUP BY TrNo,TrDate,Remarks ");
                   MessageBox.Show(@"Voucher cancel request success.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                string invNo = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                if (_gtTestCode.FnSeekRecordNewLab("V_Expense_List", "TrNo='" + invNo + "'"))
                {
                    var dt = new LabReqViewer("ExpenseVoucher", "SELECT * FROM V_Expense_List WHERE TrNo='" + invNo + "'", "Voucher", "V_Expense_List", "Accounts");
                    dt.Show();
                }
            }
            
            
        }
        private  bool IsEdit ;
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            IsEdit = true;


            string code = dataGridView2.CurrentRow.Cells[0].Value.ToString();
            string name = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            string amount = dataGridView2.CurrentRow.Cells[2].Value.ToString();
           


            expenseCodeTextBox.Text = code;
            expenseNameTextBox.Text = name;
            amountTextBox.Text = amount;
            expenseNameTextBox.Focus();
        }

        private void trNoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {

                if (_gtTestCode.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='" + Hlp.UserName + "' AND PermisionName='Entry---Can Expense Edit'"))
                {
                    if (_gtTestCode.FnSeekRecordNewLab("tb_ac_EXPENSE_ENTRY", "TrNo='" + trNoTextBox.Text + "' AND TrDate='" + Convert.ToDateTime(trDateTextBox.Text).ToString("yyyy-MM-dd") + "'"))
                    {
                        var data = GetExpenseEntryByTrNo();
                        if (data.Count > 0)
                        {
                            dataGridView2.Rows.Clear();
                            foreach (var item in data)
                            {
                                dataGridView2.Rows.Add(item.Code, item.Description, item.Amount);
                            }
                            remarksTextBox.Text = data[0].Remarks;
                        }

                        button4.Text = "&Update";
                    }
                }
                else
                {
                    MessageBox.Show(@"You need additional permision to perform this action.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
                }
                
                
               

            }
        }

        private List<ExpenseModel> GetExpenseEntryByTrNo()
        {
            var mdl = new List<ExpenseModel>();
            _gtTestCode.ConLab.Open();
            var cmd = new SqlCommand("SELECT Code,Description,Amount,Remarks FROM tb_ac_EXPENSE_ENTRY WHERE TrNo='" + trNoTextBox.Text + "'", _gtTestCode.ConLab);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                mdl.Add(new ExpenseModel()
                {
                    Code = rdr["Code"].ToString(),
                    Description = rdr["Description"].ToString(),
                    Amount = Convert.ToDouble(rdr["Amount"].ToString()),
                    Remarks = rdr["Remarks"].ToString(),

                });
            }
            rdr.Close();
            _gtTestCode.ConLab.Close();
            return mdl;
        }
    }
}
