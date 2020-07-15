namespace GoogleDriveIntegration
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
            this.button1 = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tbFolderName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDeleteToken = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCredentialsPath = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(122, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(249, 38);
            this.button1.TabIndex = 3;
            this.button1.Text = "Fetch data from Drive";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(12, 187);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(169, 15);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Status: Please click the button";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbFolderName
            // 
            this.tbFolderName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.tbFolderName.Location = new System.Drawing.Point(122, 45);
            this.tbFolderName.Name = "tbFolderName";
            this.tbFolderName.Size = new System.Drawing.Size(249, 26);
            this.tbFolderName.TabIndex = 2;
            this.tbFolderName.TextChanged += new System.EventHandler(this.tbFolderName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Folder Name:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDeleteToken
            // 
            this.btnDeleteToken.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteToken.Location = new System.Drawing.Point(122, 121);
            this.btnDeleteToken.Name = "btnDeleteToken";
            this.btnDeleteToken.Size = new System.Drawing.Size(249, 38);
            this.btnDeleteToken.TabIndex = 4;
            this.btnDeleteToken.Text = "Delete Existing Token";
            this.btnDeleteToken.UseVisualStyleBackColor = true;
            this.btnDeleteToken.Click += new System.EventHandler(this.btnDeleteToken_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Credentials Path:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCredentialsPath
            // 
            this.tbCredentialsPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.tbCredentialsPath.Location = new System.Drawing.Point(122, 10);
            this.tbCredentialsPath.Name = "tbCredentialsPath";
            this.tbCredentialsPath.Size = new System.Drawing.Size(214, 26);
            this.tbCredentialsPath.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.button2.Location = new System.Drawing.Point(343, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(28, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "...";
            this.button2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 215);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tbCredentialsPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDeleteToken);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFolderName);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Knowledge Drive";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox tbFolderName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDeleteToken;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCredentialsPath;
        private System.Windows.Forms.Button button2;
    }
}

