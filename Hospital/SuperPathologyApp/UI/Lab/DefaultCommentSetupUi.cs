using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using SuperPathologyApp.Gateway.DB_Helper;

using SuperPathologyApp.Gateway;
namespace SuperPathologyApp.UI.Lab
{
    public partial class DefaultCommentSetupUi : Form
    {
        private PrintDocument _printDocument = new PrintDocument();
       
        public DefaultCommentSetupUi()
        {
            InitializeComponent();
            _printDocument.BeginPrint += _printDocument_BeginPrint;
            _printDocument.PrintPage += _printDocument_PrintPage;
        }

        private void _printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Print the content of RichTextBox. Store the last character printed.
            //_checkPrint = rchEditor.Print(_checkPrint, rtbDoc.TextLength, e);

            //// Check for more pages
            //e.HasMorePages = _checkPrint < rtbDoc.TextLength;
        }


        readonly DbConnection _gt=new DbConnection();
        GroupSetupGateway gtTestCode = new GroupSetupGateway();
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }


     

  
        private void DefaultCommentSetupUi_Load(object sender, EventArgs e)
        {
//            _gt.LoadComboBox("SELECT Id,Name AS Description FROM tb_Group Order By Id Desc", groupNameComboBox);

            rtbDoc.SelectionTabs = new int[] { 100, 200, 300, 400 };
            rtbDoc.AcceptsTab = true;
        }

        private void idTextBox_Enter(object sender, EventArgs e)
        {
            GetDefaultResult();
        }

        private void GetDefaultResult()
        {
            //dataGridView1.DataSource = GetDefaultResultForCheck(idTextBox.Text);
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 220;
            _gt.GridColor(dataGridView1);

            if (dataGridView1.CurrentRow != null)
            {
                dataGridView1.CurrentRow.Selected = false;
            }
        }
        public DataTable GetDefaultResultForCheck(string searchString)
        {
            try
            {
                string cond = "";
                if (searchString != "")
                {
                    cond += " AND Comments like '%" + searchString + "%'";
                }

                string query = @"SELECT Id,Comments FROM tb_Default_Comment_Setup WHERE IsShow=1 " + cond + " AND DeptName='Microbiology' Order by Id ";

                _gt.ConLab.Open();
                var da = new SqlDataAdapter(query, _gt.ConLab);
                var ds = new DataSet();
                da.Fill(ds, "pCode");
                var table = ds.Tables["pCode"];
                _gt.ConLab.Close();
                return table;
            }
            catch (Exception)
            {


                if (_gt.ConLab.State == ConnectionState.Open)
                {
                    _gt.ConLab.Close();
                }

                throw;
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dataGridView1.CurrentCell.Selected)
                {
                    string rslt = dataGridView1.SelectedCells[1].Value.ToString();
                    //idTextBox.Text = dataGridView1.SelectedCells[0].Value.ToString();
                    //commentTextBox.Text = rslt;
                    //commentTextBox.Focus();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_gt.FnSeekRecordNewLab("tb_Default_Comment_Setup","Id='"+ idTextBox.Text +"'"))
                //{
                //    _gt.ConLab.Open();
                //    string query = @"UPDATE tb_Default_Comment_Setup SET Comments='"+ commentTextBox.Text +"' WHERE Id='"+ idTextBox.Text  +"'";
                //    var cmd = new SqlCommand(query, _gt.ConLab);
                //    cmd.ExecuteNonQuery();
                //    _gt.ConLab.Close();
                //    MessageBox.Show(@"Update Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    idTextBox.Text = "";
                //    commentTextBox.Text = "";
                //    commentTextBox.Focus();
                //}
                //else
                //{
                //    _gt.ConLab.Open();
                //    const string query = @"INSERT INTO tb_Default_Comment_Setup(Comments) VALUES (@Comments)";
                //    var cmd = new SqlCommand(query, _gt.ConLab);
                //    cmd.Parameters.Clear();
                //    cmd.Parameters.AddWithValue("@Comments", commentTextBox.Text);
                //    cmd.ExecuteNonQuery();
                //    _gt.ConLab.Close();
                //    MessageBox.Show(@"Save Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    idTextBox.Text = "";
                //    commentTextBox.Text = "";
                //    commentTextBox.Focus();

                //}
                
    


            }
            catch (Exception )
            {
                if (_gt.ConLab.State == ConnectionState.Open)
                {
                    _gt.ConLab.Close();
                }
                throw;
            }
        }

        private void idTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode==Keys.Down)
            {
                if (dataGridView1.Rows.Count>0)
                {
                    dataGridView1.CurrentRow.Selected = true;
                    dataGridView1.Focus();
                    
                }
                
            }




        }

      

        private void tbrLeft_Click(object sender, EventArgs e)
        {
            rtbDoc.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void tbrRight_Click(object sender, EventArgs e)
        {
            rtbDoc.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void tbrCenter_Click(object sender, EventArgs e)
        {
            rtbDoc.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void tbrBold_Click(object sender, EventArgs e)
        {
            BoldToolStripMenuItem_Click(this, e);
        }
        private void BoldToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!(rtbDoc.SelectionFont == null))
                {
                    System.Drawing.Font currentFont = rtbDoc.SelectionFont;
                    System.Drawing.FontStyle newFontStyle;

                    newFontStyle = rtbDoc.SelectionFont.Style ^ FontStyle.Bold;

                    rtbDoc.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void tbrItalic_Click(object sender, EventArgs e)
        {
            ItalicToolStripMenuItem_Click(this, e);
        }
        private void ItalicToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!(rtbDoc.SelectionFont == null))
                {
                    System.Drawing.Font currentFont = rtbDoc.SelectionFont;
                    System.Drawing.FontStyle newFontStyle;

                    newFontStyle = rtbDoc.SelectionFont.Style ^ FontStyle.Italic;

                    rtbDoc.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void tbrUnderline_Click(object sender, EventArgs e)
        {
            UnderlineToolStripMenuItem_Click(this, e);
        }
        private void UnderlineToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!(rtbDoc.SelectionFont == null))
                {
                    System.Drawing.Font currentFont = rtbDoc.SelectionFont;
                    System.Drawing.FontStyle newFontStyle;

                    newFontStyle = rtbDoc.SelectionFont.Style ^ FontStyle.Underline;

                    rtbDoc.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void tbrFont_Click(object sender, EventArgs e)
        {
            try
            {
                FontDialog1.Font = !(rtbDoc.SelectionFont == null) ? rtbDoc.SelectionFont : null;
                FontDialog1.ShowApply = true;
                if (FontDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rtbDoc.SelectionFont = FontDialog1.Font;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void tbrNew_Click(object sender, EventArgs e)
        {
            NewToolStripMenuItem_Click(this, e);
        }
        private void NewToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                rtbDoc.Clear();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void tbrSave_Click(object sender, EventArgs e)
        {

            _gt.DeleteInsertLab("DELETE FROM tb_REPORT_COMMENT_DEFAULT WHERE TestId='" + testCodeTextBox.Text + "'");
            _gt.ConLab.Open();

            const string query = "INSERT INTO tb_REPORT_COMMENT_DEFAULT(TestId,Name) VALUES (@TestId,@Name)";
            var cmd = new SqlCommand(query, _gt.ConLab);
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@TestId", testCodeTextBox.Text);
            cmd.Parameters.AddWithValue("@Name", rtbDoc.Rtf);
            cmd.ExecuteNonQuery();

            _gt.ConLab.Close();


            MessageBox.Show(@"Save Success", @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);





        }

        private void tspColor_Click(object sender, EventArgs e)
        {
            FontColorToolStripMenuItem_Click(this, new EventArgs());
        }
        private void FontColorToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                ColorDialog1.Color = rtbDoc.ForeColor;
                if (ColorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rtbDoc.SelectionColor = ColorDialog1.Color;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

      

        private void addBulletToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                rtbDoc.BulletIndent = 10;
                rtbDoc.SelectionBullet = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void removeBulletToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                rtbDoc.SelectionBullet = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void indentFiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                rtbDoc.SelectionIndent = 5;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                rtbDoc.SelectionIndent = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void testCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            GridDataShow();
        }

        private void GridDataShow()
        {
            dataGridView1.DataSource = gtTestCode.GetTestCodeList(testCodeTextBox.Text);
            _gt.GridColor(dataGridView1);
            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 300;
        }

        private void testCodeTextBox_Enter(object sender, EventArgs e)
        {
            GridDataShow();
        }

        private string textinfo = "";
        private void testCodeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up)
            {
                textinfo = "1";
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

                switch (textinfo)
                {
                    case "1":
                        testCodeTextBox.Text = gccode;
                        testNameTextBox.Text = gcdesc;

                        if (_gt.FnSeekRecordNewLab("tb_REPORT_COMMENT_DEFAULT", "TestId='" + testCodeTextBox.Text + "'"))
                        {
                          rtbDoc.Rtf=  _gt.FncReturnFielValueLab("tb_REPORT_COMMENT_DEFAULT","TestId='" + testCodeTextBox.Text + "'", "Name");
                        }
                  
                        break;
                   






                }
            }
        }
       

        private void _printDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            //_checkPrint = 0;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
                _printDocument.Print();
        }

 
    }
}
