namespace MyDataBase
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.ShowQuery = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.ConnectionTest = new System.Windows.Forms.Button();
            this.SqlQuerySend = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.DeleteUserName = new System.Windows.Forms.Button();
            this.DeleteDoctor = new System.Windows.Forms.Button();
            this.DeleteAllBill = new System.Windows.Forms.Button();
            this.SqlQuery = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.DrListPrint = new System.Windows.Forms.Button();
            this.DeleteExpense = new System.Windows.Forms.Button();
            this.IndoorBillAll = new System.Windows.Forms.Button();
            this.DeleteBed = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.RosyBrown;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.ShowQuery);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dateTimePicker2);
            this.panel1.Controls.Add(this.dateTimePicker1);
            this.panel1.Controls.Add(this.ConnectionTest);
            this.panel1.Controls.Add(this.SqlQuerySend);
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Location = new System.Drawing.Point(7, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(887, 164);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(821, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "15.09.22";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // ShowQuery
            // 
            this.ShowQuery.BackColor = System.Drawing.Color.ForestGreen;
            this.ShowQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowQuery.Location = new System.Drawing.Point(626, 88);
            this.ShowQuery.Name = "ShowQuery";
            this.ShowQuery.Size = new System.Drawing.Size(92, 39);
            this.ShowQuery.TabIndex = 8;
            this.ShowQuery.Text = "Show";
            this.ShowQuery.UseVisualStyleBackColor = false;
            this.ShowQuery.Click += new System.EventHandler(this.ShowQuery_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(616, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "To:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(55, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "From:";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker2.Location = new System.Drawing.Point(655, 14);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(118, 22);
            this.dateTimePicker2.TabIndex = 5;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Location = new System.Drawing.Point(109, 14);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(118, 22);
            this.dateTimePicker1.TabIndex = 4;
            // 
            // ConnectionTest
            // 
            this.ConnectionTest.BackColor = System.Drawing.Color.LightSeaGreen;
            this.ConnectionTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectionTest.Location = new System.Drawing.Point(786, 98);
            this.ConnectionTest.Name = "ConnectionTest";
            this.ConnectionTest.Size = new System.Drawing.Size(98, 45);
            this.ConnectionTest.TabIndex = 2;
            this.ConnectionTest.Text = "ConnectionTest";
            this.ConnectionTest.UseVisualStyleBackColor = false;
            this.ConnectionTest.Click += new System.EventHandler(this.ConnectionTest_Click);
            // 
            // SqlQuerySend
            // 
            this.SqlQuerySend.BackColor = System.Drawing.Color.ForestGreen;
            this.SqlQuerySend.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SqlQuerySend.Location = new System.Drawing.Point(626, 42);
            this.SqlQuerySend.Name = "SqlQuerySend";
            this.SqlQuerySend.Size = new System.Drawing.Size(92, 39);
            this.SqlQuerySend.TabIndex = 1;
            this.SqlQuerySend.Text = "Sql Query Send";
            this.SqlQuerySend.UseVisualStyleBackColor = false;
            this.SqlQuerySend.Click += new System.EventHandler(this.SqlQuerySend_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(3, 42);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(617, 85);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // DeleteUserName
            // 
            this.DeleteUserName.BackColor = System.Drawing.Color.ForestGreen;
            this.DeleteUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteUserName.Location = new System.Drawing.Point(304, 475);
            this.DeleteUserName.Name = "DeleteUserName";
            this.DeleteUserName.Size = new System.Drawing.Size(93, 62);
            this.DeleteUserName.TabIndex = 10;
            this.DeleteUserName.Text = "Delete All UserName";
            this.DeleteUserName.UseVisualStyleBackColor = false;
            this.DeleteUserName.Click += new System.EventHandler(this.DeleteUserName_Click);
            // 
            // DeleteDoctor
            // 
            this.DeleteDoctor.BackColor = System.Drawing.Color.ForestGreen;
            this.DeleteDoctor.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteDoctor.Location = new System.Drawing.Point(205, 475);
            this.DeleteDoctor.Name = "DeleteDoctor";
            this.DeleteDoctor.Size = new System.Drawing.Size(93, 62);
            this.DeleteDoctor.TabIndex = 9;
            this.DeleteDoctor.Text = "Delete All Doctor";
            this.DeleteDoctor.UseVisualStyleBackColor = false;
            this.DeleteDoctor.Click += new System.EventHandler(this.DeleteDoctor_Click);
            // 
            // DeleteAllBill
            // 
            this.DeleteAllBill.BackColor = System.Drawing.Color.ForestGreen;
            this.DeleteAllBill.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteAllBill.Location = new System.Drawing.Point(106, 475);
            this.DeleteAllBill.Name = "DeleteAllBill";
            this.DeleteAllBill.Size = new System.Drawing.Size(93, 62);
            this.DeleteAllBill.TabIndex = 8;
            this.DeleteAllBill.Text = "Delete All Diagnostic Bill";
            this.DeleteAllBill.UseVisualStyleBackColor = false;
            this.DeleteAllBill.Click += new System.EventHandler(this.DeleteAllBill_Click);
            // 
            // SqlQuery
            // 
            this.SqlQuery.BackColor = System.Drawing.Color.ForestGreen;
            this.SqlQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SqlQuery.Location = new System.Drawing.Point(7, 475);
            this.SqlQuery.Name = "SqlQuery";
            this.SqlQuery.Size = new System.Drawing.Size(93, 62);
            this.SqlQuery.TabIndex = 3;
            this.SqlQuery.Text = "Test Chart";
            this.SqlQuery.UseVisualStyleBackColor = false;
            this.SqlQuery.Click += new System.EventHandler(this.SqlQuery_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(7, 185);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(887, 284);
            this.dataGridView1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(415, 579);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "Super Soft";
            // 
            // DrListPrint
            // 
            this.DrListPrint.BackColor = System.Drawing.Color.ForestGreen;
            this.DrListPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DrListPrint.Location = new System.Drawing.Point(410, 475);
            this.DrListPrint.Name = "DrListPrint";
            this.DrListPrint.Size = new System.Drawing.Size(93, 62);
            this.DrListPrint.TabIndex = 11;
            this.DrListPrint.Text = "Dr list";
            this.DrListPrint.UseVisualStyleBackColor = false;
            this.DrListPrint.Click += new System.EventHandler(this.DrListPrint_Click);
            // 
            // DeleteExpense
            // 
            this.DeleteExpense.BackColor = System.Drawing.Color.ForestGreen;
            this.DeleteExpense.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteExpense.Location = new System.Drawing.Point(509, 475);
            this.DeleteExpense.Name = "DeleteExpense";
            this.DeleteExpense.Size = new System.Drawing.Size(93, 62);
            this.DeleteExpense.TabIndex = 12;
            this.DeleteExpense.Text = "Delete All Expense";
            this.DeleteExpense.UseVisualStyleBackColor = false;
            this.DeleteExpense.Click += new System.EventHandler(this.DeleteExpense_Click);
            // 
            // IndoorBillAll
            // 
            this.IndoorBillAll.BackColor = System.Drawing.Color.ForestGreen;
            this.IndoorBillAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IndoorBillAll.Location = new System.Drawing.Point(608, 475);
            this.IndoorBillAll.Name = "IndoorBillAll";
            this.IndoorBillAll.Size = new System.Drawing.Size(93, 62);
            this.IndoorBillAll.TabIndex = 13;
            this.IndoorBillAll.Text = "Delete All IndoorBill";
            this.IndoorBillAll.UseVisualStyleBackColor = false;
            this.IndoorBillAll.Click += new System.EventHandler(this.IndoorBillAll_Click);
            // 
            // DeleteBed
            // 
            this.DeleteBed.BackColor = System.Drawing.Color.ForestGreen;
            this.DeleteBed.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteBed.Location = new System.Drawing.Point(707, 475);
            this.DeleteBed.Name = "DeleteBed";
            this.DeleteBed.Size = new System.Drawing.Size(93, 62);
            this.DeleteBed.TabIndex = 14;
            this.DeleteBed.Text = "Delete Bed";
            this.DeleteBed.UseVisualStyleBackColor = false;
            this.DeleteBed.Click += new System.EventHandler(this.DeleteBed_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Goldenrod;
            this.ClientSize = new System.Drawing.Size(906, 606);
            this.Controls.Add(this.DeleteBed);
            this.Controls.Add(this.IndoorBillAll);
            this.Controls.Add(this.DeleteExpense);
            this.Controls.Add(this.DrListPrint);
            this.Controls.Add(this.DeleteUserName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DeleteDoctor);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.DeleteAllBill);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SqlQuery);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "Only for Super Soft Engineer.";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button SqlQuerySend;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button ConnectionTest;
        private System.Windows.Forms.Button SqlQuery;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button DeleteAllBill;
        private System.Windows.Forms.Button DeleteUserName;
        private System.Windows.Forms.Button DeleteDoctor;
        private System.Windows.Forms.Button DrListPrint;
        private System.Windows.Forms.Button ShowQuery;
        private System.Windows.Forms.Button DeleteExpense;
        private System.Windows.Forms.Button IndoorBillAll;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button DeleteBed;
    }
}

