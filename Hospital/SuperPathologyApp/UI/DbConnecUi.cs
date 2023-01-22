using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SuperPathologyApp.Gateway.DB_Helper;
using SuperPathologyApp.Model;
using System.Data.SqlClient;

namespace SuperPathologyApp.UI
{
    public partial class DbConnecUi : Form
    {
        public DbConnecUi()
        {
            InitializeComponent();
           
        }
        static readonly string PathAddressLabSoft = Application.StartupPath + "\\dbConnection.txt";
        private void button3_Click(object sender, EventArgs e)
        {
            if (passwordTextBox.Text.ToUpper()=="ALLAHHU@12345")
            {
                txtSqlEncrypted.Text = Hlp.GetEncryptedDataForDb(txtSqlName.Text);
                txtDbEncrypted.Text = Hlp.GetEncryptedDataForDb(txtDbName.Text);
                txtUserEncrypted.Text = Hlp.GetEncryptedDataForDb(txtuserName.Text);
                txtPasswordEncrypted.Text = Hlp.GetEncryptedDataForDb(txtPassword.Text);

               
                using (var writer = new StreamWriter(PathAddressLabSoft))
                {
                    writer.WriteLine(txtSqlEncrypted.Text);
                    writer.WriteLine(txtDbEncrypted.Text);
                    writer.WriteLine(txtUserEncrypted.Text);
                    writer.WriteLine(txtPasswordEncrypted.Text);
                } 
            }
            else
            {
                MessageBox.Show(@"Invalid password", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }

        readonly DbConnection _gt=new DbConnection();
      

        private void userNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                
            }
        }

        private void userNameTextBox_Enter(object sender, EventArgs e)
        {
            txtSqlName.BackColor = DbConnection.EnterFocus();
        }

        private void userNameTextBox_Leave(object sender, EventArgs e)
        {
            txtSqlName.BackColor = DbConnection.LeaveFocus();
        }

        private void Login_Activated(object sender, EventArgs e)
        {
            txtSqlName.Focus();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void DbConnecUi_Load(object sender, EventArgs e)
        {

        }

        
        
    }
}
