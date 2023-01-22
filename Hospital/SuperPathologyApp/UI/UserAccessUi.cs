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
    public partial class UserAccessUi : Form
    {
        public UserAccessUi()
        {
            InitializeComponent();
        }
        DbConnection _gt=new DbConnection();
        private void UserAccessUi_Load(object sender, EventArgs e)
        {
            LoadUserNameGrid();
            
            
            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_DOCTOR", reportDoctorComboBox);


            treeView1.Nodes.Add("Registration");
            treeView1.Nodes.Add("Diagnosis");
                treeView1.Nodes[1].Nodes.Add("Bill");
                    treeView1.Nodes[1].Nodes[0].Nodes.Add("Edit");
                    treeView1.Nodes[1].Nodes[0].Nodes.Add("Delete");
                    treeView1.Nodes[1].Nodes[0].Nodes.Add("Can Give Full Less");
                    treeView1.Nodes[1].Nodes[0].Nodes.Add("Can Remove Item When Edit");


                treeView1.Nodes[1].Nodes.Add("Due Collection");
                    treeView1.Nodes[1].Nodes[1].Nodes.Add("Delete");
                treeView1.Nodes[1].Nodes.Add("Report");
                    treeView1.Nodes[1].Nodes[2].Nodes.Add("Sales Report");
                    treeView1.Nodes[1].Nodes[2].Nodes.Add("Due Report");
            treeView1.Nodes.Add("Setup");
                treeView1.Nodes[2].Nodes.Add("Doctor");
                    treeView1.Nodes[2].Nodes[0].Nodes.Add("Entry");
                    treeView1.Nodes[2].Nodes[0].Nodes.Add("Honouriam");
                treeView1.Nodes[2].Nodes.Add("Test");
                    treeView1.Nodes[2].Nodes[1].Nodes.Add("Delete");
            treeView1.Nodes.Add("Lab");

            treeView1.Nodes[3].Nodes.Add("Report Common");
            treeView1.Nodes[3].Nodes.Add("Report Imaging");
            treeView1.Nodes[3].Nodes.Add("Report Individual");
            treeView1.Nodes[3].Nodes.Add("Can Print Due Report");

            treeView1.Nodes[3].Nodes.Add("Setup");

            treeView1.Nodes[3].Nodes[4].Nodes.Add("Parameter");
            treeView1.Nodes[3].Nodes[4].Nodes.Add("Default Result");
            treeView1.Nodes[3].Nodes[4].Nodes.Add("Doctor Seal");
            treeView1.Nodes[3].Nodes[4].Nodes.Add("Report Header");
 
            
            treeView1.Nodes.Add("User");
            treeView1.Nodes.Add("Admin");
            treeView1.Nodes.Add("Accounts");
            treeView1.Nodes[6].Nodes.Add("A.Setup");
            treeView1.Nodes[6].Nodes.Add("Entry");
            treeView1.Nodes[6].Nodes[1].Nodes.Add("Expense");
            treeView1.Nodes[6].Nodes[1].Nodes.Add("--Can Expense Edit");
            treeView1.Nodes[6].Nodes[1].Nodes.Add("Income");
            treeView1.Nodes[6].Nodes.Add("Report");

            treeView1.Nodes.Add("Indoor");



            treeView1.Nodes[7].Nodes.Add("Admission");
            treeView1.Nodes[7].Nodes[0].Nodes.Add("Edit");
            treeView1.Nodes[7].Nodes[0].Nodes.Add("Delete");
            
            treeView1.Nodes[7].Nodes.Add("Bill");
            treeView1.Nodes[7].Nodes[1].Nodes.Add("Edit");
            treeView1.Nodes[7].Nodes[1].Nodes.Add("Delete");
            treeView1.Nodes[7].Nodes[1].Nodes.Add("Can Give Full Less");
            treeView1.Nodes[7].Nodes.Add("Advance Collection");
            treeView1.Nodes[7].Nodes[2].Nodes.Add("Delete");

            treeView1.Nodes[7].Nodes.Add("Due Collection");
            treeView1.Nodes[7].Nodes[2].Nodes.Add("Delete");
            treeView1.Nodes[7].Nodes.Add("Report");
            treeView1.Nodes[7].Nodes[3].Nodes.Add("Sales Report");
            treeView1.Nodes[7].Nodes[3].Nodes.Add("Due Report");




            treeView1.Nodes.Add("Pharmacy");
            treeView1.Nodes[8].Nodes.Add("Bill");
            treeView1.Nodes[8].Nodes[0].Nodes.Add("Edit");
            treeView1.Nodes[8].Nodes[0].Nodes.Add("Delete");
            
            treeView1.Nodes[8].Nodes.Add("Due-Coll(Register)");
            treeView1.Nodes[8].Nodes[1].Nodes.Add("DELETE");
            treeView1.Nodes[8].Nodes.Add("Due-Coll(Indoor)");
            treeView1.Nodes[8].Nodes[2].Nodes.Add("DELETE");
            treeView1.Nodes[8].Nodes.Add("Bill-Return");
            treeView1.Nodes[8].Nodes[3].Nodes.Add("DELETE");

            treeView1.Nodes[8].Nodes.Add("Purchase");
            treeView1.Nodes[8].Nodes[4].Nodes.Add("Edit");
            treeView1.Nodes[8].Nodes[4].Nodes.Add("Delete");

            treeView1.Nodes[8].Nodes.Add("Due-Payment");
            treeView1.Nodes[8].Nodes[5].Nodes.Add("Delete");

            treeView1.Nodes[8].Nodes.Add("Purchase-Payment");
            treeView1.Nodes[8].Nodes[6].Nodes.Add("Delete");

            treeView1.Nodes[8].Nodes.Add("Setup");
            treeView1.Nodes[8].Nodes[7].Nodes.Add("Item");
            treeView1.Nodes[8].Nodes[7].Nodes.Add("Supplier");
            treeView1.Nodes[8].Nodes[7].Nodes.Add("Customer");



            treeView1.Nodes[8].Nodes.Add("Report");


        }

        private void LoadUserNameGrid()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Hlp.LoadDbByQuery(0, "SELECT distinct UserName FROM tb_USER_PRIVILEGE ", userNameTextBox.Text, "(UserName)");
            Hlp.GridFirstRowDeselect(dataGridView1);
            dataGridView1.Columns[0].Width = 200;
        }

     
        private void button3_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in treeView1.Nodes)
            {
                if (node.Nodes.Count>0)
                {
                    node.Expand();
                    foreach (TreeNode subNode in node.Nodes)
                    {
                        subNode.Checked = true;
                        if (subNode.Nodes.Count > 0)
                        {
                            subNode.Expand();
                            foreach (TreeNode subSubNode in subNode.Nodes)
                            {
                                subSubNode.Checked = true;
                            }
                        }
                    }
                }
                node.Checked = true;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {


            DeselectAll();





        }

        private void DeselectAll()
        {
            foreach (TreeNode node in treeView1.Nodes)
            {
                if (node.Nodes.Count > 0)
                {
                    foreach (TreeNode subNode in node.Nodes)
                    {
                        subNode.Checked = false;
                        if (subNode.Nodes.Count > 0)
                        {
                            foreach (TreeNode subSubNode in subNode.Nodes)
                            {
                                subSubNode.Checked = false;
                            }
                        }
                    }
                }
                node.Checked = false;
            }
        }

        private void treeView1_Click(object sender, EventArgs e)
        {
            
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //string nameOfTree = treeView1.SelectedNode.Checked.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE", "UserName='" + userNameTextBox.Text + "'"))
                {
                    _gt.DeleteInsertLab("DELETE FROM tb_USER_PRIVILEGE WHERE UserName='" + userNameTextBox.Text + "'");
                    _gt.DeleteInsertLab("DELETE FROM tb_USER_PRIVILEGE_DTLS WHERE UserName='" + userNameTextBox.Text + "'");
                }
                if (reportDoctorComboBox.SelectedIndex!=0)
                {
                    _gt.DeleteInsertLab("Update tb_DOCTOR SET ReportUserName='' WHERE ReportUserName='" + userNameTextBox.Text + "'");
                    _gt.DeleteInsertLab("Update tb_DOCTOR SET ReportUserName='"+ userNameTextBox.Text +"' WHERE Id='"+ reportDoctorComboBox.SelectedValue +"'");
                }







                var lists = new List<UserAccessModel>();
                foreach (TreeNode node in treeView1.Nodes)
                {
                    if (node.Checked)
                    {
                        string parentName = node.Text;
                        if (node.Nodes.Count > 0)
                        {
                            foreach (TreeNode subNode in node.Nodes)
                            {
                                if (subNode.Checked)
                                {
                                    string childName = subNode.Text;
                                    lists.Add(new UserAccessModel() { UserName = userNameTextBox.Text, Password = _gt.GetEncryptedData(passwordTextBox.Text), ParentName = parentName, ChildName = subNode.Text, });
                                    if (subNode.Nodes.Count > 0)
                                    {
                                        foreach (TreeNode subSubNode in subNode.Nodes)
                                        {
                                            if (subSubNode.Checked)
                                            {
                                                _gt.DeleteInsertLab("INSERT INTO tb_USER_PRIVILEGE_DTLS(UserName,PermisionName,ParentName)VALUES('" + userNameTextBox.Text + "','" + childName + "-" + subSubNode.Text + "','"+ parentName +"')");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lists.Add(new UserAccessModel() { UserName = userNameTextBox.Text, Password = _gt.GetEncryptedData(passwordTextBox.Text), ParentName = parentName, ChildName = parentName, });
                        }
                    }
                }
                foreach (var list in lists)
                {
                    _gt.ConLab.Open();
                    const string query = "INSERT INTO tb_USER_PRIVILEGE(UserName,Password, ParentName, ChildName, PostedBy)VALUES (@UserName,@Password, @ParentName, @ChildName, @AuthorizedBy)";
                    var cmd = new SqlCommand(query, _gt.ConLab);
                    cmd.Parameters.AddWithValue("@UserName", list.UserName);
                    cmd.Parameters.AddWithValue("@Password", list.Password);
                    cmd.Parameters.AddWithValue("@ParentName", list.ParentName);
                    cmd.Parameters.AddWithValue("@ChildName", list.ChildName);
                    cmd.Parameters.AddWithValue("@AuthorizedBy", Hlp.UserName);
                    cmd.ExecuteNonQuery();
                    _gt.ConLab.Close();
                }
                userNameTextBox.Text = "";
                passwordTextBox.Text = "";
                DeselectAll();
                MessageBox.Show(@"Save success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        string textInfo = "";
        private void userNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE", "UserName='" + userNameTextBox.Text + "'"))
                {
                    passwordTextBox.Text = _gt.GetDecryptedData(_gt.FncReturnFielValueLab("tb_USER_PRIVILEGE", "UserName='" + userNameTextBox.Text + "'", "Password"));
                    var userAccessData = GetDataByUserName(userNameTextBox.Text);
                    DeselectAll();
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        string parentName = node.Text;
                        foreach (var model in userAccessData)
                        {
                            if (model.ParentName == parentName)
                            {
                                node.Expand();
                                node.Checked = true;
                            }
                        }
                        if (node.Nodes.Count>0)
                        {
                            foreach (TreeNode subNode in node.Nodes)
                            {
                                string childName = subNode.Text;
                                foreach (var userAccessModel in userAccessData)
                                {
                                    if (userAccessModel.ChildName == childName)
                                    {
                                        subNode.Expand();
                                        subNode.Checked = true;
                                    }
                                }
                                if (subNode.Nodes.Count>0)
                                {
                                    foreach (TreeNode treeNode in subNode.Nodes)
                                    {
                                        string subsubNodeName =childName+"-"+ treeNode.Text;
                                        
                                        if (_gt.FnSeekRecordNewLab("tb_USER_PRIVILEGE_DTLS", "UserName='" + userNameTextBox.Text + "' AND PermisionName='" + subsubNodeName + "'"))
                                        {
                                            treeNode.Checked = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (_gt.FnSeekRecordNewLab("tb_DOCTOR", "ReportUserName='"+ userNameTextBox.Text +"'"))
                {
                    reportDoctorComboBox.SelectedValue = _gt.FncReturnFielValueLab("tb_DOCTOR","ReportUserName='" + userNameTextBox.Text + "'", "Id");
                }
                passwordTextBox.Focus();
            }


            if (e.KeyCode==Keys.Down||e.KeyCode==Keys.Up)
            {
                textInfo = "user";
                dataGridView1.Focus();
            }










        }

        public  List<UserAccessModel> GetDataByUserName(string userName)
        {
            var lists = new List<UserAccessModel>();
            string query = "SELECT ParentName,ChildName FROM tb_USER_PRIVILEGE WHERE UserName='" + userName + "'";
            _gt.ConLab.Open();
            var cmd = new SqlCommand(query, _gt.ConLab);
            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                lists.Add(new UserAccessModel()
                {
                    ParentName = rdr["ParentName"].ToString(),
                    ChildName = rdr["ChildName"].ToString(),
                });
            }
            rdr.Close();
            _gt.ConLab.Close();
            return lists;

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
            LoadUserNameGrid();
        }

        private void passwordTextBox_Leave(object sender, EventArgs e)
        {
            passwordTextBox.BackColor = DbConnection.LeaveFocus();
        }

        private void passwordTextBox_Enter(object sender, EventArgs e)
        {
            passwordTextBox.BackColor = DbConnection.EnterFocus();
        }

        private void userNameTextBox_TextChanged(object sender, EventArgs e)
        {
            LoadUserNameGrid();
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string gccode = "";
                gccode = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex - 1].Cells[0].Value.ToString();

                switch (textInfo)
                {
                    case "user":
                        userNameTextBox.Text = gccode;
                        userNameTextBox.Focus();                       
                        break;
                    
                   





                }

            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Delete)
            {
                if (MessageBox.Show(@"Do you want to delete this user ?", @"Information", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
                else
                {
                    string invNo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    _gt.DeleteInsertLab("DELETE FROM tb_USER_PRIVILEGE WHERE UserName='"+ invNo +"'");
                    _gt.DeleteInsertLab("DELETE FROM tb_USER_PRIVILEGE_DTLS WHERE UserName='" + invNo + "'");

                    MessageBox.Show(@"User delete success.", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUserNameGrid();

                }
            }
        }

       
    }
}
