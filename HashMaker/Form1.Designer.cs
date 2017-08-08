namespace HashMaker
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
            this.generateHashBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.hashBox = new System.Windows.Forms.TextBox();
            this.selectBtn = new System.Windows.Forms.Button();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // generateHashBtn
            // 
            this.generateHashBtn.Location = new System.Drawing.Point(197, 92);
            this.generateHashBtn.Name = "generateHashBtn";
            this.generateHashBtn.Size = new System.Drawing.Size(92, 23);
            this.generateHashBtn.TabIndex = 0;
            this.generateHashBtn.Text = "Generate Hash";
            this.generateHashBtn.UseVisualStyleBackColor = true;
            this.generateHashBtn.Click += new System.EventHandler(this.generateHashBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "File:";
            // 
            // hashBox
            // 
            this.hashBox.Location = new System.Drawing.Point(9, 95);
            this.hashBox.Name = "hashBox";
            this.hashBox.Size = new System.Drawing.Size(177, 20);
            this.hashBox.TabIndex = 4;
            // 
            // selectBtn
            // 
            this.selectBtn.Location = new System.Drawing.Point(197, 29);
            this.selectBtn.Name = "selectBtn";
            this.selectBtn.Size = new System.Drawing.Size(92, 23);
            this.selectBtn.TabIndex = 5;
            this.selectBtn.Text = "Select file";
            this.selectBtn.UseVisualStyleBackColor = true;
            this.selectBtn.Click += new System.EventHandler(this.selectBtn_Click);
            // 
            // pathBox
            // 
            this.pathBox.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.pathBox.Enabled = false;
            this.pathBox.Location = new System.Drawing.Point(45, 31);
            this.pathBox.Name = "pathBox";
            this.pathBox.ReadOnly = true;
            this.pathBox.Size = new System.Drawing.Size(141, 20);
            this.pathBox.TabIndex = 6;
            this.pathBox.Text = "not selected";
            this.pathBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label2.Location = new System.Drawing.Point(74, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "please select file";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 127);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pathBox);
            this.Controls.Add(this.selectBtn);
            this.Controls.Add(this.hashBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.generateHashBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Hash Maker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button generateHashBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox hashBox;
        private System.Windows.Forms.Button selectBtn;
        private System.Windows.Forms.TextBox pathBox;
        private System.Windows.Forms.Label label2;
    }
}

