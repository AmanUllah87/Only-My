using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyDataBase.DB_Helper;
using MyDataBase.Report1;

namespace MyDataBase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        readonly DB_Helper.DbConnection _gt = new DB_Helper.DbConnection();
        private void SqlQuerySend_Click(object sender, EventArgs e)
        {

            try
            {
                _gt.DeleteInsertLab(@"BACKUP DATABASE ["+ _gt.DatabaseName + "] TO DISK = 'E:\\B\\19.01.2022.bak' with init");

                MessageBox.Show(@"Success");
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
        }
//-----------------------------------------------------------------------------
        private void ConnectionTest_Click(object sender, EventArgs e)
        {
            try
            {
                _gt.ConLab.Open();
                MessageBox.Show("Connection Open  !");
                _gt.ConLab.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Connection Close !");
            }
        }
//--------------------------------------SQL Query------------------------------------
        SqlDataAdapter da;
        DataSet ds;
        private void SqlQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string query = (@"select Id,Name,Charge,MAXDISCOUNT from [dbo].[tb_TESTCHART] order by SubProjectId asc");


                da = new SqlDataAdapter(query, _gt.ConLab);
                ds = new DataSet();
                da.Fill(ds);
                _gt.ConLab.Close();
                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please call 01710233300");
            }
        }

        private void DeleteAllBill_Click(object sender, EventArgs e)
        {
            if ((richTextBox1.Text == "allahhu123456"))
            {

                try
                {
                    _gt.DeleteInsertLab(@"
Truncate table tb_BILL_DETAIL
Truncate table tb_BILL_LEDGER
Truncate table tb_BILL_MASTER
Truncate table tb_PATIENT
Truncate table tb_DUE_COLL
Truncate table tb_LAB_STRICKER_INFO

Truncate table DEL_RECORD_OF_BILL_DELETE
Truncate table Update_Record_Of_Patient

                                       ");
                    MessageBox.Show("OK");
                    richTextBox1.Text = "";

                }
                catch (Exception exception)
                {

                    MessageBox.Show(exception.Message);
                    richTextBox1.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Please Call 01710233300");
            }
        }

        private void DeleteDoctor_Click(object sender, EventArgs e)
        {
            if ((richTextBox1.Text == "allahhu123456"))
            {
                try
                {
                    _gt.DeleteInsertLab(@"
Truncate table tb_DOCTOR
Truncate table tb_DOCTOR_LEDGER
                                   ");
                    MessageBox.Show("OK");

                }
                catch (Exception exception)
                {

                    MessageBox.Show(exception.Message);
                }
            }
            else
            {
                MessageBox.Show("Please Call 01710233300");
            }
        }

        private void DeleteUserName_Click(object sender, EventArgs e)
        {
            if ((richTextBox1.Text == "allahhu123456"))
            {
                try
                {
                    _gt.DeleteInsertLab(@"
Delete from tb_USER_PRIVILEGE where UserName !='aman' AND UserName != 'neaz'
Delete from tb_USER_PRIVILEGE_DTLS where UserName !='aman' AND UserName != 'neaz'
                                   ");
                    MessageBox.Show("OK");
                    richTextBox1.Text ="";

                }
                catch (Exception exception)
                {

                    MessageBox.Show(exception.Message);
                    richTextBox1.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Please Call 01710233300");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DrListPrint_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM tb_DOCTOR WHERE 1=1";
            //string query = "SELECT * FROM V_Due_Invoice_List WHERE BillDate between' " + dtDateFromDateTimePicker.Value.ToString("yyyy-MM-dd") + "' AND '" + dtDateToDateTimePicker.Value.ToString("yyyy-MM-dd") + "' " + cond + "";
            var dt = new DrPrint("DrNamePrint", query, "tb_DOCTOR");
            dt.Show();
        }

        private void ShowQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string query = (richTextBox1.Text);


                da = new SqlDataAdapter(query, _gt.ConLab);
                ds = new DataSet();
                da.Fill(ds);
                _gt.ConLab.Close();
                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteExpense_Click(object sender, EventArgs e)
        {
            if ((richTextBox1.Text == "allahhu123456"))
            {
                try
                {
                    _gt.DeleteInsertLab(@"
Truncate table tb_ac_DEPARTMENT
Truncate table tb_ac_EXPENSE
Truncate table tb_ac_EXPENSE_ENTRY
Truncate table tb_ac_HEAD
Truncate table tb_ac_SUB_DEPARTMENT
                                   ");
                    MessageBox.Show("OK");
                    richTextBox1.Text = "";

                }
                catch (Exception exception)
                {

                    MessageBox.Show(exception.Message);
                    richTextBox1.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Please Call 01710233300");
            }
        }

        private void IndoorBillAll_Click(object sender, EventArgs e)
        {
            if ((richTextBox1.Text == "allahhu123456"))
            {
                try
                {
                    _gt.DeleteInsertLab(@"
                                          truncate table tb_in_ADMISSION
                                          truncate table tb_in_ADVANCE_COLLECTION
                                          truncate table tb_in_PATIENT_LEDGER
                                          truncate table tb_in_BILL_DETAIL
                                          truncate table tb_IN_BILL_MASTER
                                          update tb_in_BED set BookStatus = '0'
                                   ");
                    MessageBox.Show("OK");
                    richTextBox1.Text = "";

                }
                catch (Exception exception)
                {

                    MessageBox.Show(exception.Message);
                    richTextBox1.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Please Call 01710233300");
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void DeleteBed_Click(object sender, EventArgs e)
        {
            if ((richTextBox1.Text == "allahhu123456"))
            {
                try
                {
                    _gt.DeleteInsertLab(@"
                                          truncate table [dbo].[tb_in_BED]
                                          truncate table [dbo].[tb_in_BED_TYPE]
                                   ");
                    MessageBox.Show("OK");
                    richTextBox1.Text = "";

                }
                catch (Exception exception)
                {

                    MessageBox.Show(exception.Message);
                    richTextBox1.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Please Call 01710233300");
            }
        }
    }  
}
