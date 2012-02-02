namespace Cocos2d_xna.Wizard
{
    partial class OpenxliveComfirmForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenxliveComfirmForm));
            this.cboxOpenxlive = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.linkOpenxlive = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cboxOpenxlive
            // 
            this.cboxOpenxlive.AutoSize = true;
            this.cboxOpenxlive.Checked = true;
            this.cboxOpenxlive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxOpenxlive.Location = new System.Drawing.Point(28, 82);
            this.cboxOpenxlive.Name = "cboxOpenxlive";
            this.cboxOpenxlive.Size = new System.Drawing.Size(263, 17);
            this.cboxOpenxlive.TabIndex = 0;
            this.cboxOpenxlive.Text = "Create Cocos2d-xna project with Openxlive SDK ?";
            this.cboxOpenxlive.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(262, 135);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(93, 27);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(371, 135);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(93, 27);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // linkOpenxlive
            // 
            this.linkOpenxlive.AutoSize = true;
            this.linkOpenxlive.Location = new System.Drawing.Point(25, 142);
            this.linkOpenxlive.Name = "linkOpenxlive";
            this.linkOpenxlive.Size = new System.Drawing.Size(104, 13);
            this.linkOpenxlive.TabIndex = 3;
            this.linkOpenxlive.TabStop = true;
            this.linkOpenxlive.Text = "Go to openxlive.com";
            this.linkOpenxlive.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkOpenxlive_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(28, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(216, 53);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // OpenxliveComfirmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 181);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.linkOpenxlive);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cboxOpenxlive);
            this.Name = "OpenxliveComfirmForm";
            this.Text = "New Cocos2d-XNA Project Options";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cboxOpenxlive;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.LinkLabel linkOpenxlive;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}