namespace FileToXML
{
    partial class frmExecute
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
            this.components = new System.ComponentModel.Container();
            this.btnBrowseSource = new System.Windows.Forms.Button();
            this.sfdSave = new System.Windows.Forms.SaveFileDialog();
            this.ofdLoad = new System.Windows.Forms.OpenFileDialog();
            this.chkCompress = new System.Windows.Forms.CheckBox();
            this.lblSource = new System.Windows.Forms.Label();
            this.lblDestination = new System.Windows.Forms.Label();
            this.btnBrowseDestination = new System.Windows.Forms.Button();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.chkOverWrite = new System.Windows.Forms.CheckBox();
            this.chkDescription = new System.Windows.Forms.CheckBox();
            this.btnEditDescription = new System.Windows.Forms.Button();
            this.tmrCilpboardPoll = new System.Windows.Forms.Timer(this.components);
            this.chkClipboard = new System.Windows.Forms.CheckBox();
            this.btnModes = new System.Windows.Forms.Button();
            this.cmbCompression = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnBrowseSource
            // 
            this.btnBrowseSource.BackColor = System.Drawing.Color.Cyan;
            this.btnBrowseSource.Location = new System.Drawing.Point(13, 28);
            this.btnBrowseSource.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseSource.Name = "btnBrowseSource";
            this.btnBrowseSource.Size = new System.Drawing.Size(100, 28);
            this.btnBrowseSource.TabIndex = 0;
            this.btnBrowseSource.Text = "Browse...";
            this.btnBrowseSource.UseVisualStyleBackColor = false;
            this.btnBrowseSource.Click += new System.EventHandler(this.btnBrowseSource_Click);
            // 
            // sfdSave
            // 
            this.sfdSave.Filter = "B64 Encoded Files *.b64.txt|*.b64.txt|All Files *.*|*.*";
            // 
            // ofdLoad
            // 
            this.ofdLoad.AddExtension = false;
            this.ofdLoad.Filter = "All Files *.*|*.*";
            this.ofdLoad.ReadOnlyChecked = true;
            // 
            // chkCompress
            // 
            this.chkCompress.Location = new System.Drawing.Point(41, 63);
            this.chkCompress.Margin = new System.Windows.Forms.Padding(4);
            this.chkCompress.Name = "chkCompress";
            this.chkCompress.Size = new System.Drawing.Size(460, 44);
            this.chkCompress.TabIndex = 2;
            this.chkCompress.Text = "Compress file before encoding (not recommended for some images, binaries, music f" +
    "iles (such as MP3/WMA/AAC), etc.)";
            this.chkCompress.UseVisualStyleBackColor = true;
            this.chkCompress.CheckedChanged += new System.EventHandler(this.chkCompress_CheckedChanged);
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(13, 9);
            this.lblSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(70, 17);
            this.lblSource.TabIndex = 3;
            this.lblSource.Text = "SOURCE:";
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.Location = new System.Drawing.Point(13, 112);
            this.lblDestination.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(104, 17);
            this.lblDestination.TabIndex = 6;
            this.lblDestination.Text = "DESTINATION:";
            // 
            // btnBrowseDestination
            // 
            this.btnBrowseDestination.BackColor = System.Drawing.Color.Cyan;
            this.btnBrowseDestination.Location = new System.Drawing.Point(13, 132);
            this.btnBrowseDestination.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowseDestination.Name = "btnBrowseDestination";
            this.btnBrowseDestination.Size = new System.Drawing.Size(100, 28);
            this.btnBrowseDestination.TabIndex = 4;
            this.btnBrowseDestination.Text = "Browse...";
            this.btnBrowseDestination.UseVisualStyleBackColor = false;
            this.btnBrowseDestination.Click += new System.EventHandler(this.btnBrowseDestination_Click);
            // 
            // txtSource
            // 
            this.txtSource.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtSource.Location = new System.Drawing.Point(121, 31);
            this.txtSource.Margin = new System.Windows.Forms.Padding(4);
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(588, 22);
            this.txtSource.TabIndex = 7;
            // 
            // txtDestination
            // 
            this.txtDestination.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtDestination.Location = new System.Drawing.Point(121, 134);
            this.txtDestination.Margin = new System.Windows.Forms.Padding(4);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.ReadOnly = true;
            this.txtDestination.Size = new System.Drawing.Size(588, 22);
            this.txtDestination.TabIndex = 8;
            this.txtDestination.TextChanged += new System.EventHandler(this.txtDestination_TextChanged);
            // 
            // btnExecute
            // 
            this.btnExecute.BackColor = System.Drawing.Color.Lime;
            this.btnExecute.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnExecute.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExecute.Location = new System.Drawing.Point(370, 197);
            this.btnExecute.Margin = new System.Windows.Forms.Padding(4);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(204, 30);
            this.btnExecute.TabIndex = 9;
            this.btnExecute.Text = "EXECUTE";
            this.btnExecute.UseVisualStyleBackColor = false;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.BackColor = System.Drawing.Color.Red;
            this.btnQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnQuit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuit.Location = new System.Drawing.Point(581, 197);
            this.btnQuit.Margin = new System.Windows.Forms.Padding(4);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(128, 30);
            this.btnQuit.TabIndex = 10;
            this.btnQuit.Text = "QUIT";
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // chkOverWrite
            // 
            this.chkOverWrite.AutoSize = true;
            this.chkOverWrite.Location = new System.Drawing.Point(41, 168);
            this.chkOverWrite.Margin = new System.Windows.Forms.Padding(4);
            this.chkOverWrite.Name = "chkOverWrite";
            this.chkOverWrite.Size = new System.Drawing.Size(292, 21);
            this.chkOverWrite.TabIndex = 11;
            this.chkOverWrite.Text = "Overwrite destination (silences warnings).";
            this.chkOverWrite.UseVisualStyleBackColor = true;
            this.chkOverWrite.CheckedChanged += new System.EventHandler(this.chkOverWrite_CheckedChanged);
            // 
            // chkDescription
            // 
            this.chkDescription.AutoSize = true;
            this.chkDescription.Location = new System.Drawing.Point(508, 63);
            this.chkDescription.Margin = new System.Windows.Forms.Padding(4);
            this.chkDescription.Name = "chkDescription";
            this.chkDescription.Size = new System.Drawing.Size(150, 21);
            this.chkDescription.TabIndex = 12;
            this.chkDescription.Text = "Include Description";
            this.chkDescription.UseVisualStyleBackColor = true;
            this.chkDescription.CheckedChanged += new System.EventHandler(this.chkDescription_CheckedChanged);
            // 
            // btnEditDescription
            // 
            this.btnEditDescription.Location = new System.Drawing.Point(672, 63);
            this.btnEditDescription.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditDescription.Name = "btnEditDescription";
            this.btnEditDescription.Size = new System.Drawing.Size(37, 28);
            this.btnEditDescription.TabIndex = 13;
            this.btnEditDescription.Text = "...";
            this.btnEditDescription.UseVisualStyleBackColor = true;
            this.btnEditDescription.Visible = false;
            this.btnEditDescription.Click += new System.EventHandler(this.btnEditDescription_Click);
            // 
            // tmrCilpboardPoll
            // 
            this.tmrCilpboardPoll.Tick += new System.EventHandler(this.tmrCilpboardPoll_Tick);
            // 
            // chkClipboard
            // 
            this.chkClipboard.AutoSize = true;
            this.chkClipboard.Location = new System.Drawing.Point(508, 91);
            this.chkClipboard.Name = "chkClipboard";
            this.chkClipboard.Size = new System.Drawing.Size(146, 21);
            this.chkClipboard.TabIndex = 14;
            this.chkClipboard.Text = "Watch Clipboard...";
            this.chkClipboard.UseVisualStyleBackColor = true;
            this.chkClipboard.CheckedChanged += new System.EventHandler(this.chkClipboard_CheckedChanged);
            // 
            // btnModes
            // 
            this.btnModes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnModes.Location = new System.Drawing.Point(13, 196);
            this.btnModes.Name = "btnModes";
            this.btnModes.Size = new System.Drawing.Size(157, 31);
            this.btnModes.TabIndex = 15;
            this.btnModes.Text = "CHANGE MODES";
            this.btnModes.UseVisualStyleBackColor = false;
            this.btnModes.Click += new System.EventHandler(this.btnModes_Click);
            // 
            // cmbCompression
            // 
            this.cmbCompression.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCompression.Enabled = false;
            this.cmbCompression.FormattingEnabled = true;
            this.cmbCompression.Items.AddRange(new object[] {
            "None",
            "GZip"});
            this.cmbCompression.Location = new System.Drawing.Point(401, 91);
            this.cmbCompression.Name = "cmbCompression";
            this.cmbCompression.Size = new System.Drawing.Size(91, 24);
            this.cmbCompression.TabIndex = 16;
            this.cmbCompression.SelectedIndexChanged += new System.EventHandler(this.cmbCompression_SelectedIndexChanged);
            // 
            // frmExecute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnQuit;
            this.ClientSize = new System.Drawing.Size(729, 238);
            this.Controls.Add(this.cmbCompression);
            this.Controls.Add(this.btnModes);
            this.Controls.Add(this.chkClipboard);
            this.Controls.Add(this.btnEditDescription);
            this.Controls.Add(this.chkDescription);
            this.Controls.Add(this.chkOverWrite);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnExecute);
            this.Controls.Add(this.txtDestination);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.lblDestination);
            this.Controls.Add(this.btnBrowseDestination);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.chkCompress);
            this.Controls.Add(this.btnBrowseSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmExecute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ENCODE A FILE";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmExecute_FormClosed);
            this.Load += new System.EventHandler(this.frmExecute_Load);
            this.Shown += new System.EventHandler(this.frmExecute_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowseSource;
        private System.Windows.Forms.SaveFileDialog sfdSave;
        private System.Windows.Forms.OpenFileDialog ofdLoad;
        private System.Windows.Forms.CheckBox chkCompress;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.Button btnBrowseDestination;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.CheckBox chkOverWrite;
        private System.Windows.Forms.CheckBox chkDescription;
        private System.Windows.Forms.Button btnEditDescription;
        private System.Windows.Forms.Timer tmrCilpboardPoll;
        private System.Windows.Forms.CheckBox chkClipboard;
        private System.Windows.Forms.Button btnModes;
        private System.Windows.Forms.ComboBox cmbCompression;
    }
}