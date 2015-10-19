namespace Cherry_Hill_9th_Ward_Map
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.previewBtn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.wardListTab = new System.Windows.Forms.TabPage();
            this.lblNumFamsOnList = new System.Windows.Forms.Label();
            this.OnWardList = new System.Windows.Forms.DataGridView();
            this.notOnWardListTab = new System.Windows.Forms.TabPage();
            this.lblNumFamsNotOnList = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.EditFamilyBtn = new System.Windows.Forms.Button();
            this.DeleteFamilyBtn = new System.Windows.Forms.Button();
            this.AddFamilyBtn = new System.Windows.Forms.Button();
            this.NotOnWardList = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.wardListPathTextBox = new System.Windows.Forms.TextBox();
            this.wardListBrowseBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.newFileNotOnWardListBtn = new System.Windows.Forms.Button();
            this.notOnWardListBrowseBtn = new System.Windows.Forms.Button();
            this.notOnwardListPathTextBox = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.wardListTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OnWardList)).BeginInit();
            this.notOnWardListTab.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NotOnWardList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // previewBtn
            // 
            this.previewBtn.Location = new System.Drawing.Point(629, 165);
            this.previewBtn.Name = "previewBtn";
            this.previewBtn.Size = new System.Drawing.Size(75, 20);
            this.previewBtn.TabIndex = 2;
            this.previewBtn.Text = "Preview";
            this.previewBtn.UseVisualStyleBackColor = true;
            this.previewBtn.Click += new System.EventHandler(this.previewBtn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.wardListTab);
            this.tabControl1.Controls.Add(this.notOnWardListTab);
            this.tabControl1.Location = new System.Drawing.Point(53, 184);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(632, 388);
            this.tabControl1.TabIndex = 4;
            // 
            // wardListTab
            // 
            this.wardListTab.Controls.Add(this.lblNumFamsOnList);
            this.wardListTab.Controls.Add(this.OnWardList);
            this.wardListTab.Location = new System.Drawing.Point(4, 22);
            this.wardListTab.Name = "wardListTab";
            this.wardListTab.Padding = new System.Windows.Forms.Padding(3);
            this.wardListTab.Size = new System.Drawing.Size(624, 362);
            this.wardListTab.TabIndex = 0;
            this.wardListTab.Text = "Families On Ward List";
            this.wardListTab.UseVisualStyleBackColor = true;
            // 
            // lblNumFamsOnList
            // 
            this.lblNumFamsOnList.AutoSize = true;
            this.lblNumFamsOnList.Location = new System.Drawing.Point(16, 334);
            this.lblNumFamsOnList.Name = "lblNumFamsOnList";
            this.lblNumFamsOnList.Size = new System.Drawing.Size(99, 13);
            this.lblNumFamsOnList.TabIndex = 5;
            this.lblNumFamsOnList.Text = "Number of Families:";
            // 
            // OnWardList
            // 
            this.OnWardList.AllowUserToAddRows = false;
            this.OnWardList.AllowUserToDeleteRows = false;
            this.OnWardList.AllowUserToResizeRows = false;
            this.OnWardList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.OnWardList.Location = new System.Drawing.Point(6, 6);
            this.OnWardList.Name = "OnWardList";
            this.OnWardList.ReadOnly = true;
            this.OnWardList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.OnWardList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.OnWardList.Size = new System.Drawing.Size(612, 308);
            this.OnWardList.TabIndex = 4;
            this.OnWardList.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.OnWardList_ColumnWidthChanged);
            // 
            // notOnWardListTab
            // 
            this.notOnWardListTab.Controls.Add(this.lblNumFamsNotOnList);
            this.notOnWardListTab.Controls.Add(this.groupBox3);
            this.notOnWardListTab.Controls.Add(this.NotOnWardList);
            this.notOnWardListTab.Location = new System.Drawing.Point(4, 22);
            this.notOnWardListTab.Name = "notOnWardListTab";
            this.notOnWardListTab.Padding = new System.Windows.Forms.Padding(3);
            this.notOnWardListTab.Size = new System.Drawing.Size(624, 362);
            this.notOnWardListTab.TabIndex = 1;
            this.notOnWardListTab.Text = "Familes Not On Ward List";
            this.notOnWardListTab.UseVisualStyleBackColor = true;
            // 
            // lblNumFamsNotOnList
            // 
            this.lblNumFamsNotOnList.AutoSize = true;
            this.lblNumFamsNotOnList.Location = new System.Drawing.Point(6, 265);
            this.lblNumFamsNotOnList.Name = "lblNumFamsNotOnList";
            this.lblNumFamsNotOnList.Size = new System.Drawing.Size(99, 13);
            this.lblNumFamsNotOnList.TabIndex = 9;
            this.lblNumFamsNotOnList.Text = "Number of Families:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.EditFamilyBtn);
            this.groupBox3.Controls.Add(this.DeleteFamilyBtn);
            this.groupBox3.Controls.Add(this.AddFamilyBtn);
            this.groupBox3.Location = new System.Drawing.Point(339, 265);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(279, 49);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Families";
            // 
            // EditFamilyBtn
            // 
            this.EditFamilyBtn.Location = new System.Drawing.Point(190, 18);
            this.EditFamilyBtn.Name = "EditFamilyBtn";
            this.EditFamilyBtn.Size = new System.Drawing.Size(60, 21);
            this.EditFamilyBtn.TabIndex = 9;
            this.EditFamilyBtn.Text = "Edit";
            this.EditFamilyBtn.UseVisualStyleBackColor = true;
            this.EditFamilyBtn.Click += new System.EventHandler(this.EditFamilyBtn_Click);
            // 
            // DeleteFamilyBtn
            // 
            this.DeleteFamilyBtn.Location = new System.Drawing.Point(108, 18);
            this.DeleteFamilyBtn.Name = "DeleteFamilyBtn";
            this.DeleteFamilyBtn.Size = new System.Drawing.Size(60, 21);
            this.DeleteFamilyBtn.TabIndex = 8;
            this.DeleteFamilyBtn.Text = "Delete";
            this.DeleteFamilyBtn.UseVisualStyleBackColor = true;
            this.DeleteFamilyBtn.Click += new System.EventHandler(this.DeleteFamilyBtn_Click);
            // 
            // AddFamilyBtn
            // 
            this.AddFamilyBtn.Location = new System.Drawing.Point(26, 18);
            this.AddFamilyBtn.Name = "AddFamilyBtn";
            this.AddFamilyBtn.Size = new System.Drawing.Size(60, 21);
            this.AddFamilyBtn.TabIndex = 7;
            this.AddFamilyBtn.Text = "Add";
            this.AddFamilyBtn.UseVisualStyleBackColor = true;
            this.AddFamilyBtn.Click += new System.EventHandler(this.AddFamilyBtn_Click);
            // 
            // NotOnWardList
            // 
            this.NotOnWardList.AllowUserToAddRows = false;
            this.NotOnWardList.AllowUserToDeleteRows = false;
            this.NotOnWardList.AllowUserToResizeRows = false;
            this.NotOnWardList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.NotOnWardList.Location = new System.Drawing.Point(6, 6);
            this.NotOnWardList.Name = "NotOnWardList";
            this.NotOnWardList.ReadOnly = true;
            this.NotOnWardList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.NotOnWardList.Size = new System.Drawing.Size(612, 253);
            this.NotOnWardList.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.wardListPathTextBox);
            this.groupBox1.Controls.Add(this.wardListBrowseBtn);
            this.groupBox1.Location = new System.Drawing.Point(40, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(683, 57);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ward List File";
            // 
            // wardListPathTextBox
            // 
            this.wardListPathTextBox.Location = new System.Drawing.Point(19, 23);
            this.wardListPathTextBox.Name = "wardListPathTextBox";
            this.wardListPathTextBox.Size = new System.Drawing.Size(553, 20);
            this.wardListPathTextBox.TabIndex = 3;
            // 
            // wardListBrowseBtn
            // 
            this.wardListBrowseBtn.Location = new System.Drawing.Point(589, 20);
            this.wardListBrowseBtn.Name = "wardListBrowseBtn";
            this.wardListBrowseBtn.Size = new System.Drawing.Size(75, 23);
            this.wardListBrowseBtn.TabIndex = 2;
            this.wardListBrowseBtn.Text = "Browse";
            this.wardListBrowseBtn.UseVisualStyleBackColor = true;
            this.wardListBrowseBtn.Click += new System.EventHandler(this.wardListBrowseBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.newFileNotOnWardListBtn);
            this.groupBox2.Controls.Add(this.notOnWardListBrowseBtn);
            this.groupBox2.Controls.Add(this.notOnwardListPathTextBox);
            this.groupBox2.Location = new System.Drawing.Point(40, 74);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(683, 78);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "People Not On Ward List File";
            // 
            // newFileNotOnWardListBtn
            // 
            this.newFileNotOnWardListBtn.Location = new System.Drawing.Point(589, 48);
            this.newFileNotOnWardListBtn.Name = "newFileNotOnWardListBtn";
            this.newFileNotOnWardListBtn.Size = new System.Drawing.Size(75, 23);
            this.newFileNotOnWardListBtn.TabIndex = 9;
            this.newFileNotOnWardListBtn.Text = "New File";
            this.newFileNotOnWardListBtn.UseVisualStyleBackColor = true;
            this.newFileNotOnWardListBtn.Click += new System.EventHandler(this.newFileNotOnWardListBtn_Click);
            // 
            // notOnWardListBrowseBtn
            // 
            this.notOnWardListBrowseBtn.Location = new System.Drawing.Point(589, 19);
            this.notOnWardListBrowseBtn.Name = "notOnWardListBrowseBtn";
            this.notOnWardListBrowseBtn.Size = new System.Drawing.Size(75, 23);
            this.notOnWardListBrowseBtn.TabIndex = 8;
            this.notOnWardListBrowseBtn.Text = "Browse";
            this.notOnWardListBrowseBtn.UseVisualStyleBackColor = true;
            this.notOnWardListBrowseBtn.Click += new System.EventHandler(this.notOnWardListBrowseBtn_Click);
            // 
            // notOnwardListPathTextBox
            // 
            this.notOnwardListPathTextBox.Location = new System.Drawing.Point(19, 19);
            this.notOnwardListPathTextBox.Name = "notOnwardListPathTextBox";
            this.notOnwardListPathTextBox.Size = new System.Drawing.Size(553, 20);
            this.notOnwardListPathTextBox.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 579);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.previewBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cherry Hill 9th Ward Map";
            this.tabControl1.ResumeLayout(false);
            this.wardListTab.ResumeLayout(false);
            this.wardListTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OnWardList)).EndInit();
            this.notOnWardListTab.ResumeLayout(false);
            this.notOnWardListTab.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NotOnWardList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button previewBtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage wardListTab;
        private System.Windows.Forms.DataGridView OnWardList;
        private System.Windows.Forms.TabPage notOnWardListTab;
        private System.Windows.Forms.DataGridView NotOnWardList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox wardListPathTextBox;
        private System.Windows.Forms.Button wardListBrowseBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button notOnWardListBrowseBtn;
        private System.Windows.Forms.TextBox notOnwardListPathTextBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button DeleteFamilyBtn;
        private System.Windows.Forms.Button AddFamilyBtn;
        private System.Windows.Forms.Label lblNumFamsOnList;
        private System.Windows.Forms.Label lblNumFamsNotOnList;
        private System.Windows.Forms.Button EditFamilyBtn;
        private System.Windows.Forms.Button newFileNotOnWardListBtn;
    }
}

