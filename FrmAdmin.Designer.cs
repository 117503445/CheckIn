﻿namespace CheckIn
{
    partial class FrmAdmin
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
            this.buttonOneKey = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonOneKey
            // 
            this.buttonOneKey.Location = new System.Drawing.Point(582, 347);
            this.buttonOneKey.Name = "buttonOneKey";
            this.buttonOneKey.Size = new System.Drawing.Size(227, 71);
            this.buttonOneKey.TabIndex = 0;
            this.buttonOneKey.Text = "每周归档";
            this.buttonOneKey.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 1);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(139, 40);
            this.button2.TabIndex = 1;
            this.button2.Text = "计算";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 68);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(139, 40);
            this.button3.TabIndex = 2;
            this.button3.Text = "全部置为FALSE";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 138);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(139, 40);
            this.button4.TabIndex = 3;
            this.button4.Text = "全部置为TRUE";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 205);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(139, 40);
            this.button5.TabIndex = 4;
            this.button5.Text = "换座位";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(12, 272);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(139, 40);
            this.button6.TabIndex = 5;
            this.button6.Text = "设置";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // FrmAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 430);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.buttonOneKey);
            this.Name = "FrmAdmin";
            this.Text = "CheckInAdmin";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmAdmin_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOneKey;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
    }
}