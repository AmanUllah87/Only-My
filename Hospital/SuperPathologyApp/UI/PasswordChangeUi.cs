using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;

namespace SuperPathologyApp.UI
{
    public partial class PasswordChangeUi : Form
    {
        public PasswordChangeUi()
        {
            InitializeComponent();
        }
        readonly DbConnection _gt=new DbConnection();
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (_gt.FnSeekRecordNewLab("tb_UserAccess","UserName='" + userNameTextBox.Text + "' AND   Password='" + _gt.GetEncryptedData(passwordTextBox.Text) + "'"))
                {
                    if (newPasswordTextBox.Text!=confirmPasswordTextBox.Text)
                    {
                        MessageBox.Show(@"Password And Confirm Password Did Not Match", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        newPasswordTextBox.Focus();
                        return;
                    }
                    if (newPasswordTextBox.Text=="")
                    {
                        MessageBox.Show(@"Password Can Not Blank", @"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        newPasswordTextBox.Focus();
                        return;
                    }
                    _gt.DeleteInsertLab("UPDATE tb_UserAccess SET Password='"+ _gt.GetEncryptedData(confirmPasswordTextBox.Text) +"' WHERE UserName='" + userNameTextBox.Text + "'");
                    passwordTextBox.Text = "";
                    newPasswordTextBox.Text = "";
                    confirmPasswordTextBox.Text = "";
                    MessageBox.Show(@"Password Change Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(@"Invalid UserName Or Password", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (Exception exception)
            {
                if (_gt.ConLab.State == ConnectionState.Open)
                {
                    _gt.ConLab.Close();
                }
                MessageBox.Show(exception.Message, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void userNameTextBox_Leave(object sender, EventArgs e)
        {
            userNameTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void userNameTextBox_Enter(object sender, EventArgs e)
        {
            userNameTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void passwordTextBox_Leave(object sender, EventArgs e)
        {
            passwordTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void passwordTextBox_Enter(object sender, EventArgs e)
        {
            passwordTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void PasswordChangeUi_Load(object sender, EventArgs e)
        {
            userNameTextBox.Text = Hlp.UserName;
            passwordTextBox.Focus();
        }

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                newPasswordTextBox.Focus();
            }
        }

        private void newPasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                confirmPasswordTextBox.Focus();
            }
        }

        private void confirmPasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
            }
        }
      
    }
}
