namespace CheckIn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAdmin));
            this.buttonOneKey = new System.Windows.Forms.Button();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.buttonSetFalse = new System.Windows.Forms.Button();
            this.buttonSetTrue = new System.Windows.Forms.Button();
            this.buttonMoveSeats = new System.Windows.Forms.Button();
            this.buttonSettings = new System.Windows.Forms.Button();
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
            // buttonCalculate
            // 
            this.buttonCalculate.Location = new System.Drawing.Point(12, 1);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(139, 40);
            this.buttonCalculate.TabIndex = 1;
            this.buttonCalculate.Text = "计算";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // buttonSetFalse
            // 
            this.buttonSetFalse.Location = new System.Drawing.Point(12, 68);
            this.buttonSetFalse.Name = "buttonSetFalse";
            this.buttonSetFalse.Size = new System.Drawing.Size(139, 40);
            this.buttonSetFalse.TabIndex = 2;
            this.buttonSetFalse.Text = "全部置为FALSE";
            this.buttonSetFalse.UseVisualStyleBackColor = true;
            // 
            // buttonSetTrue
            // 
            this.buttonSetTrue.Location = new System.Drawing.Point(12, 138);
            this.buttonSetTrue.Name = "buttonSetTrue";
            this.buttonSetTrue.Size = new System.Drawing.Size(139, 40);
            this.buttonSetTrue.TabIndex = 3;
            this.buttonSetTrue.Text = "全部置为TRUE";
            this.buttonSetTrue.UseVisualStyleBackColor = true;
            this.buttonSetTrue.Click += new System.EventHandler(this.button4_Click);
            // 
            // buttonMoveSeats
            // 
            this.buttonMoveSeats.Location = new System.Drawing.Point(12, 205);
            this.buttonMoveSeats.Name = "buttonMoveSeats";
            this.buttonMoveSeats.Size = new System.Drawing.Size(139, 40);
            this.buttonMoveSeats.TabIndex = 4;
            this.buttonMoveSeats.Text = "换座位";
            this.buttonMoveSeats.UseVisualStyleBackColor = true;
            // 
            // buttonSettings
            // 
            this.buttonSettings.Location = new System.Drawing.Point(12, 272);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(139, 40);
            this.buttonSettings.TabIndex = 5;
            this.buttonSettings.Text = "设置";
            this.buttonSettings.UseVisualStyleBackColor = true;
            // 
            // FrmAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 430);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.buttonMoveSeats);
            this.Controls.Add(this.buttonSetTrue);
            this.Controls.Add(this.buttonSetFalse);
            this.Controls.Add(this.buttonCalculate);
            this.Controls.Add(this.buttonOneKey);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmAdmin";
            this.Text = "CheckInAdmin";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmAdmin_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOneKey;
        private System.Windows.Forms.Button buttonCalculate;
        private System.Windows.Forms.Button buttonSetFalse;
        private System.Windows.Forms.Button buttonSetTrue;
        private System.Windows.Forms.Button buttonMoveSeats;
        private System.Windows.Forms.Button buttonSettings;
    }
}