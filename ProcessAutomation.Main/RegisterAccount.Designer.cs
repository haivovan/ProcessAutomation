namespace ProcessAutomation.Main
{
    partial class RegisterAccount
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
            this.registerAccountBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // registerAccountBrowser
            // 
            this.registerAccountBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registerAccountBrowser.Location = new System.Drawing.Point(0, 0);
            this.registerAccountBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.registerAccountBrowser.Name = "registerAccountBrowser";
            this.registerAccountBrowser.Size = new System.Drawing.Size(1085, 624);
            this.registerAccountBrowser.TabIndex = 0;
            // 
            // RegisterAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1085, 624);
            this.Controls.Add(this.registerAccountBrowser);
            this.Name = "RegisterAccount";
            this.Text = "RegisterAccount";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.WebBrowser registerAccountBrowser;
    }
}