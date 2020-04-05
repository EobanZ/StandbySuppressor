namespace StandbySuppressor
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblStatusLabel = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnToggle = new System.Windows.Forms.Button();
            this.cmbMethod = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chbDisplay = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblStatusLabel
            // 
            this.lblStatusLabel.AutoSize = true;
            this.lblStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusLabel.Location = new System.Drawing.Point(14, 9);
            this.lblStatusLabel.Name = "lblStatusLabel";
            this.lblStatusLabel.Size = new System.Drawing.Size(111, 32);
            this.lblStatusLabel.TabIndex = 0;
            this.lblStatusLabel.Text = "Status: ";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(124, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(53, 32);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Off";
            // 
            // btnToggle
            // 
            this.btnToggle.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggle.Location = new System.Drawing.Point(89, 155);
            this.btnToggle.Name = "btnToggle";
            this.btnToggle.Size = new System.Drawing.Size(114, 44);
            this.btnToggle.TabIndex = 2;
            this.btnToggle.Text = "Toggle";
            this.btnToggle.UseVisualStyleBackColor = true;
            this.btnToggle.Click += new System.EventHandler(this.btnToggle_Click);
            // 
            // cmbMethod
            // 
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Items.AddRange(new object[] {
            "PowerAvailabilityRequests",
            "SetThreadExecutionState"});
            this.cmbMethod.Location = new System.Drawing.Point(20, 73);
            this.cmbMethod.Name = "cmbMethod";
            this.cmbMethod.Size = new System.Drawing.Size(242, 24);
            this.cmbMethod.TabIndex = 3;
            this.cmbMethod.SelectedIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Method:";
            // 
            // chbDisplay
            // 
            this.chbDisplay.AutoSize = true;
            this.chbDisplay.Location = new System.Drawing.Point(20, 103);
            this.chbDisplay.Name = "chbDisplay";
            this.chbDisplay.Size = new System.Drawing.Size(136, 21);
            this.chbDisplay.TabIndex = 5;
            this.chbDisplay.Text = "Keep Display On";
            this.chbDisplay.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 233);
            this.Controls.Add(this.chbDisplay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbMethod);
            this.Controls.Add(this.btnToggle);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblStatusLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "StandbySuppressor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatusLabel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnToggle;
        private System.Windows.Forms.ComboBox cmbMethod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chbDisplay;
    }
}

