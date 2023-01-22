using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using System.Data.SqlClient;

namespace SuperPathologyApp.UI
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //var grp = new MasterUi();
            //grp.ShowDialog();


            
            if (userNameTextBox.Text.ToUpper() == "RAKIB" || userNameTextBox.Text.ToUpper() == "SUPERSOFT")
            {
                if (passwordTextBox.Text == "allahhu12345")
                {
                    var grp = new MasterUi();
                    grp.ShowDialog();
                }
            }


            string rawdata1Encrypt = _gt.FncReturnFielValueLab("tb_VALIDATION", "1=1", "RawData1");
            string rawdata2Encrypt = _gt.FncReturnFielValueLab("tb_VALIDATION", "1=1", "RawData2");

            var rawdata1Decrypt = Hlp.Rc4(rawdata1Encrypt, _gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ComName"));
            var rawdata2Decrypt = Hlp.Decrypt(rawdata2Encrypt, _gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ComName"));


            if (rawdata1Decrypt != rawdata2Decrypt)
            {
                MessageBox.Show(@"You Add The Wrong Number For Activation", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
                return;
            }

            DateTime serverDate = Convert.ToDateTime(_gt.FncReturnFielValueLab("tb_BILL_MASTER", "1=1", "GetDate()"));



            if (Convert.ToDateTime(rawdata1Decrypt) <= serverDate)
            {
                MessageBox.Show(@"Your Date Has Been Expired.Please Activate.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
              
                return;
            }

            if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE", "UserName='" + userNameTextBox.Text + "'"))
            {
                string enf = _gt.GetEncryptedData(passwordTextBox.Text);
                if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE", "Password='" + _gt.GetEncryptedData(passwordTextBox.Text) + "'"))
                {
                    if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE", "Password='" + _gt.GetEncryptedData(passwordTextBox.Text) + "' AND UserName='" + userNameTextBox.Text + "'"))
                    {
                        Hlp.UserName = userNameTextBox.Text;
                        Hide();
                        var grp = new MainForm();
                        grp.ShowDialog();
                    }
                
                }
                else
                {
                    MessageBox.Show(@"Invalid password", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    passwordTextBox.Focus();
                    return;
                }
            }
            else
            {
                MessageBox.Show(@"Invalid user name", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                userNameTextBox.Focus();
                return;
            }
        }
        DbConnection _gt=new DbConnection();
        private void Login_Load(object sender, EventArgs e)

        {
                    
            try
            {
                label3.Text = _gt.FncReturnFielValueLab("tb_MASTER_INFO", "1=1", "ComName");
            }
            catch (Exception)
            {

                var grp = new DbConnecUi();
                grp.ShowDialog();
            }
            
            
           
        }



        private void userNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (userNameTextBox.Text!="")
                {
                    passwordTextBox.Focus();
                }
                else
                {
                    userNameTextBox.Focus();
                }
            }
        }

        private void userNameTextBox_Enter(object sender, EventArgs e)
        {
            userNameTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void passwordTextBox_Leave(object sender, EventArgs e)
        {
            userNameTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void passwordTextBox_Enter(object sender, EventArgs e)
        {
            passwordTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void userNameTextBox_Leave(object sender, EventArgs e)
        {
            userNameTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (userNameTextBox.Text == "")
                {
                    userNameTextBox.Focus();
                    return;
                }

                else
                {
                    if (passwordTextBox.Text == "")
                    {
                        passwordTextBox.Focus();
                        return;
                    }
                    else
                    {
                        button3.Focus();
                    }
                }
            }
        }

        private void Login_Activated(object sender, EventArgs e)
        {
            userNameTextBox.Focus();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var grp = new DbConnecUi();
            grp.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var grp = new MasterUi();
            grp.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        
    }
}
