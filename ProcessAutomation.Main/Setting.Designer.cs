namespace ProcessAutomation.Main
{
    partial class Setting
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb30s = new System.Windows.Forms.CheckBox();
            this.cbGiaDinh = new System.Windows.Forms.CheckBox();
            this.cbHanhLang = new System.Windows.Forms.CheckBox();
            this.cbCayBang = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtMoney_30s = new System.Windows.Forms.TextBox();
            this.txtMoney_GD = new System.Windows.Forms.TextBox();
            this.txtMoney_HL = new System.Windows.Forms.TextBox();
            this.txtMoney_CB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb30s);
            this.groupBox1.Controls.Add(this.cbGiaDinh);
            this.groupBox1.Controls.Add(this.cbHanhLang);
            this.groupBox1.Controls.Add(this.cbCayBang);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 272);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cài Đặt Web Chạy";
            // 
            // cb30s
            // 
            this.cb30s.AutoSize = true;
            this.cb30s.Location = new System.Drawing.Point(178, 154);
            this.cb30s.Margin = new System.Windows.Forms.Padding(6);
            this.cb30s.Name = "cb30s";
            this.cb30s.Size = new System.Drawing.Size(66, 29);
            this.cb30s.TabIndex = 25;
            this.cb30s.Text = "30s";
            this.cb30s.UseVisualStyleBackColor = true;
            this.cb30s.Enabled = false;
            // 
            // cbGiaDinh
            // 
            this.cbGiaDinh.AutoSize = true;
            this.cbGiaDinh.Location = new System.Drawing.Point(11, 154);
            this.cbGiaDinh.Margin = new System.Windows.Forms.Padding(6);
            this.cbGiaDinh.Name = "cbGiaDinh";
            this.cbGiaDinh.Size = new System.Drawing.Size(151, 29);
            this.cbGiaDinh.TabIndex = 26;
            this.cbGiaDinh.Text = "GiaDinhVina";
            this.cbGiaDinh.UseVisualStyleBackColor = true;
            this.cbGiaDinh.Enabled = false;
            // 
            // cbHanhLang
            // 
            this.cbHanhLang.AutoSize = true;
            this.cbHanhLang.Location = new System.Drawing.Point(178, 61);
            this.cbHanhLang.Margin = new System.Windows.Forms.Padding(6);
            this.cbHanhLang.Name = "cbHanhLang";
            this.cbHanhLang.Size = new System.Drawing.Size(130, 29);
            this.cbHanhLang.TabIndex = 27;
            this.cbHanhLang.Text = "HanhLang";
            this.cbHanhLang.UseVisualStyleBackColor = true;
            // 
            // cbCayBang
            // 
            this.cbCayBang.AutoSize = true;
            this.cbCayBang.Location = new System.Drawing.Point(11, 61);
            this.cbCayBang.Margin = new System.Windows.Forms.Padding(6);
            this.cbCayBang.Name = "cbCayBang";
            this.cbCayBang.Size = new System.Drawing.Size(119, 29);
            this.cbCayBang.TabIndex = 28;
            this.cbCayBang.Text = "CayBang";
            this.cbCayBang.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtMoney_30s);
            this.groupBox2.Controls.Add(this.txtMoney_GD);
            this.groupBox2.Controls.Add(this.txtMoney_HL);
            this.groupBox2.Controls.Add(this.txtMoney_CB);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(365, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(494, 272);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Số tiền tối thiếu";
            // 
            // txtMoney_30s
            // 
            this.txtMoney_30s.Location = new System.Drawing.Point(190, 205);
            this.txtMoney_30s.Name = "txtMoney_30s";
            this.txtMoney_30s.Size = new System.Drawing.Size(213, 31);
            this.txtMoney_30s.TabIndex = 2;
            this.txtMoney_30s.Leave += new System.EventHandler(this.txtMoney_30s_Leave);
            // 
            // txtMoney_GD
            // 
            this.txtMoney_GD.Location = new System.Drawing.Point(190, 151);
            this.txtMoney_GD.Name = "txtMoney_GD";
            this.txtMoney_GD.Size = new System.Drawing.Size(213, 31);
            this.txtMoney_GD.TabIndex = 2;
            this.txtMoney_GD.Leave += new System.EventHandler(this.txtMoney_GD_Leave);
            // 
            // txtMoney_HL
            // 
            this.txtMoney_HL.Location = new System.Drawing.Point(190, 100);
            this.txtMoney_HL.Name = "txtMoney_HL";
            this.txtMoney_HL.Size = new System.Drawing.Size(213, 31);
            this.txtMoney_HL.TabIndex = 2;
            this.txtMoney_HL.Leave += new System.EventHandler(this.txtMoney_HL_Leave);
            // 
            // txtMoney_CB
            // 
            this.txtMoney_CB.Location = new System.Drawing.Point(190, 45);
            this.txtMoney_CB.Name = "txtMoney_CB";
            this.txtMoney_CB.Size = new System.Drawing.Size(213, 31);
            this.txtMoney_CB.TabIndex = 2;
            this.txtMoney_CB.Leave += new System.EventHandler(this.txtMoney_CB_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 211);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 25);
            this.label4.TabIndex = 1;
            this.label4.Text = "30s";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "GiaDinhVina";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "HanhLang";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "CayBang";
            // 
            // okBtn
            // 
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new System.Drawing.Point(724, 299);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(135, 34);
            this.okBtn.TabIndex = 27;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 345);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Setting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setting";
            this.Load += new System.EventHandler(this.Setting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cb30s;
        private System.Windows.Forms.CheckBox cbGiaDinh;
        private System.Windows.Forms.CheckBox cbHanhLang;
        private System.Windows.Forms.CheckBox cbCayBang;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.TextBox txtMoney_30s;
        private System.Windows.Forms.TextBox txtMoney_GD;
        private System.Windows.Forms.TextBox txtMoney_HL;
        private System.Windows.Forms.TextBox txtMoney_CB;
    }
}